namespace TheSingularityWorkshop.FSM_COS;

/// <summary>Composes a runtime from a manifest and optional external configuration.</summary>
public interface IFsmCos
{
    RuntimeAssembly Execute(
        RuntimeManifest manifest,
        IMicroBundleConfigurationSource? configurationSource = null);

#if WORKSHOP_USE_REPOSITORY_SOURCES || WORKSHOP_ENABLE_TYPED_PROVIDER_PREPARATION
    /// <summary>Composes with an optional host-supplied typed provider resolver.</summary>
    RuntimeAssembly Execute(
        RuntimeManifest manifest,
        IMicroBundleConfigurationSource? configurationSource,
        TheSingularityWorkshop.MicroBundleDomain.IMicroBundleProviderResolver? providerResolver);

#if WORKSHOP_USE_REPOSITORY_SOURCES
    /// <summary>Composes portable MicroBundles only after validating their platform-to-Domain dependency projection.</summary>
    RuntimeAssembly Execute(
        RuntimeManifest manifest,
        IMicroBundleConfigurationSource? configurationSource,
        TheSingularityWorkshop.MicroBundleDomain.IMicroBundleProviderResolver? providerResolver,
        PlatformMicroBundleDomainBindingSet domainBindingSet);
#endif
#endif

    /// <summary>Attempts composition and returns phase-aware diagnostics instead of throwing.</summary>
    RuntimeCompositionResult TryExecute(
        RuntimeManifest manifest,
        IMicroBundleConfigurationSource? configurationSource = null);

#if WORKSHOP_USE_REPOSITORY_SOURCES || WORKSHOP_ENABLE_TYPED_PROVIDER_PREPARATION
    /// <summary>Attempts composition with an optional host-supplied typed provider resolver.</summary>
    RuntimeCompositionResult TryExecute(
        RuntimeManifest manifest,
        IMicroBundleConfigurationSource? configurationSource,
        TheSingularityWorkshop.MicroBundleDomain.IMicroBundleProviderResolver? providerResolver);

#if WORKSHOP_USE_REPOSITORY_SOURCES
    /// <summary>Attempts portable composition with an explicit platform-to-Domain binding set.</summary>
    RuntimeCompositionResult TryExecute(
        RuntimeManifest manifest,
        IMicroBundleConfigurationSource? configurationSource,
        TheSingularityWorkshop.MicroBundleDomain.IMicroBundleProviderResolver? providerResolver,
        PlatformMicroBundleDomainBindingSet domainBindingSet);
#endif
#endif
}
