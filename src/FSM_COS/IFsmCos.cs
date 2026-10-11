namespace TheSingularityWorkshop.FSM_COS;

/// <summary>Composes a runtime from a manifest and optional external configuration.</summary>
public interface IFsmCos
{
    RuntimeAssembly Execute(
        RuntimeManifest manifest,
        IMicroBundleConfigurationSource? configurationSource = null);
}
