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
        string expectedVersion)
    {
        PlatformMicroBundleSlotIndex.ValidateCanonicalOntology(canonicalOntology);
        if (domainBundleId == 0)
            throw new ArgumentOutOfRangeException(nameof(domainBundleId), "A Domain MicroBundle ID must be non-zero.");
        ArgumentNullException.ThrowIfNull(bundle);
        ArgumentException.ThrowIfNullOrWhiteSpace(expectedVersion);

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
    }

    public string CanonicalOntology { get; }
    public PlatformMicroBundleSlot PlatformSlot { get; }
    public ulong DomainBundleId { get; }
    public string Version { get; }
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
}
