using System.Collections.ObjectModel;
using TheSingularityWorkshop.MicroBundleDomain;

namespace TheSingularityWorkshop.FSM_COS;

/// <summary>
/// Explicitly reconciles a platform-local portable slot with a Domain-owned runtime MicroBundle.
/// </summary>
/// <remarks>
/// The slot and Domain ID are separate identity namespaces. This binding records their
/// relationship after materialization; it never rewrites a descriptor or infers equality.
/// </remarks>
public sealed class PlatformMicroBundleDomainBinding
{
    public PlatformMicroBundleDomainBinding(
        string canonicalOntology,
        PlatformMicroBundleSlot platformSlot,
        ulong domainBundleId,
        IMicroBundle bundle,
        string expectedVersion,
        string? verifiedSha256 = null)
    {
        PlatformMicroBundleSlotIndex.ValidateCanonicalOntology(canonicalOntology);
        if (domainBundleId == 0)
            throw new ArgumentOutOfRangeException(nameof(domainBundleId), "A Domain MicroBundle ID must be non-zero.");
        ArgumentNullException.ThrowIfNull(bundle);
        ArgumentException.ThrowIfNullOrWhiteSpace(expectedVersion);
        if (verifiedSha256 is not null &&
            (verifiedSha256.Length != 64 || !verifiedSha256.All(Uri.IsHexDigit)))
            throw new ArgumentException("A verified artifact SHA-256 must contain exactly 64 hexadecimal characters.", nameof(verifiedSha256));

        var descriptor = bundle.Descriptor
            ?? throw new ArgumentException("A materialized MicroBundle must expose a descriptor.", nameof(bundle));

        if (bundle.Id != domainBundleId || descriptor.Id != domainBundleId)
            throw new ArgumentException(
                $"The materialized MicroBundle ID ({bundle.Id}) and descriptor ID ({descriptor.Id}) must both equal the explicitly assigned Domain ID ({domainBundleId}).",
                nameof(bundle));

        if (!string.Equals(descriptor.Version, expectedVersion, StringComparison.Ordinal))
            throw new ArgumentException(
                $"The materialized MicroBundle version '{descriptor.Version}' does not match the requested exact version '{expectedVersion}'.",
                nameof(bundle));

        CanonicalOntology = canonicalOntology;
        PlatformSlot = platformSlot;
        DomainBundleId = domainBundleId;
        Bundle = bundle;
        Version = expectedVersion;
        VerifiedSha256 = verifiedSha256?.ToLowerInvariant();
    }

    public string CanonicalOntology { get; }
    public PlatformMicroBundleSlot PlatformSlot { get; }
    public ulong DomainBundleId { get; }
    public string Version { get; }
    /// <summary>Gets the artifact digest verified by the platform materializer, when available.</summary>
    public string? VerifiedSha256 { get; }
    public IMicroBundle Bundle { get; }
}

/// <summary>
/// Immutable validated set of explicit portable-slot-to-Domain bindings for one staged runtime.
/// </summary>
public sealed class PlatformMicroBundleDomainBindingSet
{
    private readonly IReadOnlyDictionary<string, PlatformMicroBundleDomainBinding> _byOntology;
    private readonly IReadOnlyDictionary<ulong, PlatformMicroBundleDomainBinding> _bySlot;
    private readonly IReadOnlyDictionary<ulong, PlatformMicroBundleDomainBinding> _byDomainId;

    public PlatformMicroBundleDomainBindingSet(IEnumerable<PlatformMicroBundleDomainBinding> bindings)
    {
        ArgumentNullException.ThrowIfNull(bindings);

        var ontology = new Dictionary<string, PlatformMicroBundleDomainBinding>(StringComparer.Ordinal);
        var slots = new Dictionary<ulong, PlatformMicroBundleDomainBinding>();
        var domainIds = new Dictionary<ulong, PlatformMicroBundleDomainBinding>();

        foreach (var binding in bindings)
        {
            ArgumentNullException.ThrowIfNull(binding);
            if (!ontology.TryAdd(binding.CanonicalOntology, binding))
                throw new ArgumentException($"Ontology '{binding.CanonicalOntology}' is bound more than once.", nameof(bindings));
            if (!slots.TryAdd(binding.PlatformSlot.Value, binding))
                throw new ArgumentException($"Platform slot {binding.PlatformSlot.Value} is bound more than once.", nameof(bindings));
            if (!domainIds.TryAdd(binding.DomainBundleId, binding))
                throw new ArgumentException($"Domain MicroBundle ID {binding.DomainBundleId} is bound more than once.", nameof(bindings));
        }

        _byOntology = new ReadOnlyDictionary<string, PlatformMicroBundleDomainBinding>(ontology);
        _bySlot = new ReadOnlyDictionary<ulong, PlatformMicroBundleDomainBinding>(slots);
        _byDomainId = new ReadOnlyDictionary<ulong, PlatformMicroBundleDomainBinding>(domainIds);
    }

    public IReadOnlyCollection<PlatformMicroBundleDomainBinding> Bindings =>
        Array.AsReadOnly(_byOntology.Values.ToArray());

    public bool TryGetByOntology(string canonicalOntology, out PlatformMicroBundleDomainBinding? binding) =>
        _byOntology.TryGetValue(canonicalOntology, out binding);

    public bool TryGetByPlatformSlot(PlatformMicroBundleSlot slot, out PlatformMicroBundleDomainBinding? binding) =>
        _bySlot.TryGetValue(slot.Value, out binding);

    public bool TryGetByDomainBundleId(ulong domainBundleId, out PlatformMicroBundleDomainBinding? binding) =>
        _byDomainId.TryGetValue(domainBundleId, out binding);

#if WORKSHOP_USE_REPOSITORY_SOURCES
    /// <summary>
    /// Validates that every bundle in a portable composition has one explicit platform-slot
    /// and Domain-ID binding, and that its portable dependency declarations project exactly
    /// onto both numeric dependency surfaces before any bundle is activated.
    /// </summary>
    public void ValidatePortableDependencyProjection(IEnumerable<IMicroBundle> resolvedBundles)
    {
        ArgumentNullException.ThrowIfNull(resolvedBundles);

        var portableById = new Dictionary<ulong, IPortableMicroBundle>();
        foreach (var bundle in resolvedBundles)
        {
            ArgumentNullException.ThrowIfNull(bundle);
            if (bundle is not IPortableMicroBundle portable)
                throw new InvalidOperationException(
                    $"MicroBundle {bundle.Id} does not implement {nameof(IPortableMicroBundle)} in a portable composition.");
            if (!portableById.TryAdd(bundle.Id, portable))
                throw new InvalidOperationException(
                    $"Portable composition contains Domain MicroBundle ID {bundle.Id} more than once.");
        }

        foreach (var portable in portableById.Values)
        {
            var identity = portable.PortableIdentity;
            if (!identity.IsValid ||
                !_byOntology.TryGetValue(identity.Ontology, out var ownBinding) ||
                !ReferenceEquals(ownBinding.Bundle, portable) ||
                ownBinding.DomainBundleId != portable.Id ||
                !string.Equals(ownBinding.Version, identity.Version, StringComparison.Ordinal))
            {
                throw new InvalidOperationException(
                    $"Portable identity for Domain MicroBundle {portable.Id} has no matching explicit platform binding.");
            }

            ValidatePortableDigest(identity, ownBinding, portable.Id);

            var portableDependencies = portable.PortableDependencies
                ?? throw new InvalidOperationException(
                    $"Portable MicroBundle {portable.Id} returned no portable dependency list.");
            var expected = new Dictionary<ulong, PortableMicroBundleDependency>();

            foreach (var dependency in portableDependencies)
            {
                var reference = dependency.Reference;
                if (!dependency.IsValid ||
                    !_byOntology.TryGetValue(reference.Ontology, out var dependencyBinding))
                {
                    throw new InvalidOperationException(
                        $"Portable MicroBundle {portable.Id} has an invalid or unbound dependency '{reference.Ontology}'.");
                }

                if (!string.Equals(dependencyBinding.Version, reference.Version, StringComparison.Ordinal))
                    throw new InvalidOperationException(
                        $"Portable MicroBundle {portable.Id} requires '{reference.Ontology}' version '{reference.Version}', " +
                        $"but its platform binding is version '{dependencyBinding.Version}'.");

                ValidatePortableDigest(reference, dependencyBinding, portable.Id);

                if (!portableById.ContainsKey(dependencyBinding.DomainBundleId))
                    throw new InvalidOperationException(
                        $"Portable dependency '{reference.Ontology}' maps to Domain ID {dependencyBinding.DomainBundleId}, " +
                        "but that bundle is absent from the resolved composition closure.");

                if (!expected.TryAdd(dependencyBinding.DomainBundleId, dependency))
                    throw new InvalidOperationException(
                        $"Portable MicroBundle {portable.Id} maps more than one dependency declaration to Domain ID {dependencyBinding.DomainBundleId}.");
            }

            var descriptorDependencies = portable.Descriptor.Dependencies;
            if (descriptorDependencies.Count != expected.Count)
                throw new InvalidOperationException(
                    $"Portable MicroBundle {portable.Id} descriptor dependency count does not match its portable declarations.");

            foreach (var descriptorDependency in descriptorDependencies)
            {
                if (!expected.TryGetValue(descriptorDependency.BundleId, out var portableDependency) ||
                    !string.Equals(descriptorDependency.Version, portableDependency.Reference.Version, StringComparison.Ordinal))
                {
                    throw new InvalidOperationException(
                        $"Portable MicroBundle {portable.Id} descriptor dependency {descriptorDependency.BundleId} " +
                        "does not match its portable identity and exact version.");
                }
            }

            var runtimeDependencies = portable.Dependencies
                ?? throw new InvalidOperationException(
                    $"Portable MicroBundle {portable.Id} returned no runtime dependency request list.");
            if (runtimeDependencies.Count != expected.Count)
                throw new InvalidOperationException(
                    $"Portable MicroBundle {portable.Id} runtime dependency request count does not match its portable declarations.");

            var seenRuntimeIds = new HashSet<ulong>();
            foreach (var request in runtimeDependencies)
            {
                if (!seenRuntimeIds.Add(request.BundleId) ||
                    !expected.TryGetValue(request.BundleId, out var portableDependency) ||
                    !string.Equals(request.Version, portableDependency.Reference.Version, StringComparison.Ordinal) ||
                    !request.Configuration.Span.SequenceEqual(portableDependency.Configuration.Span))
                {
                    throw new InvalidOperationException(
                        $"Portable MicroBundle {portable.Id} runtime dependency request {request.BundleId} " +
                        "does not match its portable identity, exact version, and configuration.");
                }
            }
        }
    }

    private static void ValidatePortableDigest(
        PortableMicroBundleReference reference,
        PlatformMicroBundleDomainBinding binding,
        ulong ownerId)
    {
        if (reference.Sha256 is not null &&
            !string.Equals(reference.Sha256, binding.VerifiedSha256, StringComparison.Ordinal))
        {
            throw new InvalidOperationException(
                $"Portable MicroBundle reference '{reference.Ontology}' for Domain MicroBundle {ownerId} " +
                "does not match the SHA-256 verified by its platform materializer.");
        }
    }
#endif
}
