using TheSingularityWorkshop.FSM_API;

namespace TheSingularityWorkshop.FSM_COS;

/// <summary>
/// Experience-owned decision boundary for promoting deferred manifest entries.
/// </summary>
/// <remarks>
/// FSM_COS supplies the mechanism; the Experience supplies the meaning.
/// This contract intentionally does not know about domains such as games,
/// mortality, science, GUI, or any other application-specific trigger.
/// </remarks>
public interface IManifestLoadEvaluator
{
    /// <summary>Returns whether an entry should be promoted into active composition.</summary>
    bool ShouldLoad(RuntimeManifestEntry entry, IStateContext? experienceContext);
}
