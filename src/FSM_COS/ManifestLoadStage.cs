namespace TheSingularityWorkshop.FSM_COS;

/// <summary>
/// Describes a manifest entry's intended composition stage.
/// </summary>
/// <remarks>
/// This is plan metadata. It does not perform localization or defer loading
/// until a host supplies those lifecycle capabilities.
/// </remarks>
public enum ManifestLoadStage
{
    /// <summary>Required during initial runtime preparation.</summary>
    Resident = 0,

    /// <summary>Eligible for later Experience-owned promotion.</summary>
    Deferred = 1
}
