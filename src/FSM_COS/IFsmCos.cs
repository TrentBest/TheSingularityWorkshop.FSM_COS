namespace TheSingularityWorkshop.FSM_COS;

/// <summary>Composes a runtime from a manifest and optional external configuration.</summary>
public interface IFsmCos
{
    RuntimeAssembly Execute(
        RuntimeManifest manifest,
        IMicroBundleConfigurationSource? configurationSource = null);
    /// <summary>Attempts composition and returns phase-aware diagnostics instead of throwing.</summary>
    RuntimeCompositionResult TryExecute(
        RuntimeManifest manifest,
        IMicroBundleConfigurationSource? configurationSource = null);
}
