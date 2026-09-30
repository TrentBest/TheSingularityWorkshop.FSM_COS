namespace TheSingularityWorkshop.FSM_COS;

/// <summary>
/// Describes when a manifest entry is eligible for composition.
/// </summary>
public enum ManifestLoadStage
{
    /// <summary>Localize and load as part of initial runtime preparation.</summary>
    Resident = 0,

    /// <summary>Keep available for later Experience-owned promotion.</summary>
    Deferred = 1
}
