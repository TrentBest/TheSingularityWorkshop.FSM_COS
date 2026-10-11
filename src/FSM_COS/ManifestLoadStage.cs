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
    /// <summary>Required to establish the initial visible/runnable Experience.</summary>
    Bootstrap = 0,

    /// <summary>Participates in initial preparation but is not a bootstrap prerequisite.</summary>
    Resident = 1,

    /// <summary>Eligible for later Experience-owned promotion.</summary>
    Deferred = 2
}
