namespace TheSingularityWorkshop.FSM_COS;

/// <summary>
/// Describes when a published manifest entry is intended to enter composition.
/// </summary>
/// <remarks>
/// These are plan roles, not domain semantics. This metadata does not by itself
/// perform localization or make the composition engine defer loading.
/// </remarks>
public enum ManifestLoadStage
{
    /// <summary>
    /// Labels an entry intended to be part of an Experience's bootstrap set.
    /// The current composition pass does not implement bootstrap-only loading.
    /// </summary>
    Bootstrap = 0,

    /// <summary>
    /// Labels an entry intended for initial preparation outside the bootstrap set.
    /// The current composition pass does not schedule loading by stage.
    /// </summary>
    Resident = 1,

    /// <summary>
    /// Labels an entry intended for later Experience-owned promotion.
    /// The current composition pass does not automatically defer or promote it.
    /// </summary>
    Deferred = 2
}
