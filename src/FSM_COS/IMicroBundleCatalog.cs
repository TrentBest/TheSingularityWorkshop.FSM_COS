using TheSingularityWorkshop.MicroBundleDomain;

namespace TheSingularityWorkshop.FSM_COS;

/// <summary>Resolves domain-owned MicroBundles available to the composition system.</summary>
public interface IMicroBundleCatalog
{
    bool TryResolve(ulong bundleId, out TheSingularityWorkshop.MicroBundleDomain.IMicroBundle? bundle);
}
