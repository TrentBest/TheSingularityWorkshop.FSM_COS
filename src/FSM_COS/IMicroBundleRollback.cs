using TheSingularityWorkshop.MicroBundleDomain;

namespace TheSingularityWorkshop.FSM_COS;

/// <summary>
/// Optional compensation contract for MicroBundles that can undo their own load-time effects.
/// </summary>
/// <remarks>
/// FSM_COS invokes this callback in reverse load-attempt order when loading or arbitration fails.
/// Compensation is best-effort: bundles that do not implement this contract cannot be rolled back,
/// and a bundle must make its implementation safe after a partially completed or failed Load call.
/// This is not a guarantee of transactional atomicity.
/// </remarks>
public interface IMicroBundleRollback
{
    /// <summary>Attempts to undo side effects created by the corresponding load attempt.</summary>
    void RollbackLoad(IMicroBundleLoadContext context);
}
