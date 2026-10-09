using TheSingularityWorkshop.FSM_API;

namespace TheSingularityWorkshop.FSM_COS;

/// <summary>
/// Experience-owned decision boundary for promoting deferred manifest entries.
/// </summary>
/// <remarks>
/// FSM_COS supplies a neutral contract; the Experience supplies the meaning.
/// This evaluator does not resolve dependencies, localize artifacts, or load
/// bundles. Those responsibilities remain with their respective boundaries.
/// </remarks>
public interface IManifestLoadEvaluator
{
    /// <summary>Returns whether a deferred entry is wanted by the Experience.</summary>
    bool ShouldLoad(RuntimeManifestEntry entry, IStateContext? experienceContext);
}
