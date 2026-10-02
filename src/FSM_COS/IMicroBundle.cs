using TheSingularityWorkshop.MicroBundleDomain;

namespace TheSingularityWorkshop.FSM_COS;

/// <summary>
/// Compatibility name for the domain-owned MicroBundle contract.
/// </summary>
/// <remarks>
/// FSM_COS does not define a second MicroBundle contract. This alias keeps
/// composition-side source readable while the executable contract remains owned
/// by <see cref="TheSingularityWorkshop.MicroBundleDomain.IMicroBundle"/>.
/// </remarks>
public interface IMicroBundle : TheSingularityWorkshop.MicroBundleDomain.IMicroBundle
{
}
