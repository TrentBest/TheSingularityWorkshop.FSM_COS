namespace TheSingularityWorkshop.FSM_COS;

/// <summary>Composes a runtime from a manifest and optional external configuration.</summary>
public interface IFsmCos
{
    RuntimeAssembly Execute(
        RuntimeManifest manifest,
        IMicroBundleConfigurationSource? configurationSource = null);

    /// <summary>Composes with an optional host-supplied typed provider resolver.</summary>
    RuntimeAssembly Execute(
        RuntimeManifest manifest,
        IMicroBundleConfigurationSource? configurationSource,
        TheSingularityWorkshop.MicroBundleDomain.IMicroBundleProviderResolver? providerResolver);

    /// <summary>Attempts composition and returns phase-aware diagnostics instead of throwing.</summary>
    RuntimeCompositionResult TryExecute(
        RuntimeManifest manifest,
        IMicroBundleConfigurationSource? configurationSource = null);

    /// <summary>Attempts composition with an optional host-supplied typed provider resolver.</summary>
    RuntimeCompositionResult TryExecute(
        RuntimeManifest manifest,
        IMicroBundleConfigurationSource? configurationSource,
        TheSingularityWorkshop.MicroBundleDomain.IMicroBundleProviderResolver? providerResolver);
}
