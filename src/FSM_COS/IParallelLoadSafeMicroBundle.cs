using TheSingularityWorkshop.MicroBundleDomain;

namespace TheSingularityWorkshop.FSM_COS;

/// <summary>
/// Optional capability contract for MicroBundles whose Load implementation is safe to execute
/// concurrently with other explicitly parallel-safe bundles.
/// </summary>
/// <remarks>
/// A bundle that does not implement this interface, or returns false, must be loaded alone.
/// This is a capability declaration, not a guarantee that the bundle has no external side effects.
/// </remarks>
public interface IParallelLoadSafeMicroBundle : IMicroBundle
{
    /// <summary>Gets whether this bundle permits concurrent invocation of its Load method.</summary>
    bool SupportsParallelLoad { get; }
}
