namespace TheSingularityWorkshop.FSM_COS;

/// <summary>
/// One executable MicroBundle entry in a RuntimeManifest load plan.
/// </summary>
/// <param name="Reference">Immutable published identity used to validate localized data.</param>
/// <param name="Request">The existing composition request and opaque configuration.</param>
/// <param name="Stage">Whether the entry is resident or deferred.</param>
public sealed record RuntimeManifestEntry(
    MicroBundleReference Reference,
    BundleRequest Request,
    ManifestLoadStage Stage = ManifestLoadStage.Resident)
{
    public bool IsValid =>
        Reference.IsValid &&
        Request.BundleId == Reference.BundleId;
}
