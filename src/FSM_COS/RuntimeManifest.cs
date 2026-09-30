using TheSingularityWorkshop.FSM_API;

namespace TheSingularityWorkshop.FSM_COS;

/// <summary>Machine-oriented description of the runtime FSM_COS must assemble.</summary>
/// <remarks>
/// <see cref="LoadPlan"/> is the staged execution structure for published
/// MicroBundle identities. The optional <see cref="Schedule"/> is the
/// publish-time analysis of fetch opportunity and dependency ordering.
/// </remarks>
public sealed record RuntimeManifest(
    ulong RuntimeId,
    IReadOnlyList<BundleRequest> Bundles,
    IStateContext? ExperienceContext = null,
    IReadOnlyList<RuntimeManifestEntry>? LoadPlan = null,
    RuntimeManifestSchedule? Schedule = null)
{
    public static RuntimeManifest Empty(
        ulong runtimeId,
        IStateContext? experienceContext = null) =>
        new(runtimeId, Array.Empty<BundleRequest>(), experienceContext);
}
