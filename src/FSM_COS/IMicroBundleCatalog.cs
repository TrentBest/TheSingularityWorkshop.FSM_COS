namespace TheSingularityWorkshop.FSM_COS;

/// <summary>Resolves MicroBundles available to the composition system.</summary>
public interface IMicroBundleCatalog
{
    bool TryResolve(ulong bundleId, out IMicroBundle? bundle);
}
