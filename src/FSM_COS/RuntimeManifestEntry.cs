namespace TheSingularityWorkshop.FSM_COS;

/// <summary>
/// Immutable published identity and intended stage for one runtime capability.
/// </summary>
public sealed record RuntimeManifestEntry(
    MicroBundleReference Reference,
    ManifestLoadStage Stage = ManifestLoadStage.Resident)
{
    public bool IsValid => Reference.IsValid;
}
