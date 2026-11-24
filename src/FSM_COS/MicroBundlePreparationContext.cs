#if WORKSHOP_USE_REPOSITORY_SOURCES || WORKSHOP_ENABLE_TYPED_PROVIDER_PREPARATION
using TheSingularityWorkshop.MicroBundleDomain;

namespace TheSingularityWorkshop.FSM_COS;

/// <summary>FSM_COS implementation of the domain-owned preparation context.</summary>
internal sealed class MicroBundlePreparationContext : IMicroBundlePreparationContext
{
    private readonly IReadOnlyDictionary<ulong, IMicroBundle> _bundles;
    private readonly IMicroBundleProviderResolver? _providerResolver;
    private readonly IReadOnlyList<MicroBundleProviderReference> _providers;

    internal MicroBundlePreparationContext(
        ulong runtimeId,
        IReadOnlyDictionary<ulong, IMicroBundle> bundles,
        IMicroBundleProviderResolver? providerResolver)
    {
        RuntimeId = runtimeId;
        _bundles = bundles;
        _providerResolver = providerResolver;
        _providers = Array.AsReadOnly(bundles.Values
            .OrderBy(bundle => bundle.Id)
            .SelectMany(bundle => bundle.Descriptor.Providers
                .Where(provider => !string.IsNullOrWhiteSpace(provider.ContractVersion))
                .OrderBy(provider => provider.Id, StringComparer.Ordinal)
                .Select(provider => new MicroBundleProviderReference(
                    bundle.Id,
                    bundle.Descriptor.Version,
                    provider.Id,
                    provider.ContractVersion!)))
            .ToArray());
    }

    public ulong RuntimeId { get; }
    public IReadOnlyList<MicroBundleProviderReference> Providers => _providers;

    public bool TryGetProvider<TProvider>(
        MicroBundleProviderReference reference,
        out TProvider? provider)
        where TProvider : class
    {
        provider = null;
        if (!reference.IsValid ||
            _providerResolver is null ||
            !_bundles.TryGetValue(reference.BundleId, out var bundle) ||
            !string.Equals(bundle.Descriptor.Version, reference.BundleVersion, StringComparison.Ordinal))
            return false;

        var declared = bundle.Descriptor.Providers.Any(candidate =>
            string.Equals(candidate.Id, reference.ProviderId, StringComparison.Ordinal) &&
            string.Equals(candidate.ContractVersion, reference.ContractVersion, StringComparison.Ordinal));
        if (!declared)
            return false;

        return _providerResolver.TryResolve(reference, out provider) && provider is not null;
    }
}

#endif
