using TheSingularityWorkshop.MicroBundleDomain;

namespace TheSingularityWorkshop.FSM_COS;

/// <summary>Resolves domain-owned MicroBundles available to the composition system.</summary>
public interface IMicroBundleCatalog
{
    /// <summary>Resolves a requested root MicroBundle at its manifest version.</summary>
    bool TryResolve(
        ulong bundleId,
        string version,
        out TheSingularityWorkshop.MicroBundleDomain.IMicroBundle? bundle);

    /// <summary>Resolves a dependency by its domain-owned identity.</summary>
    bool TryResolve(
        ulong bundleId,
        out TheSingularityWorkshop.MicroBundleDomain.IMicroBundle? bundle);
}
