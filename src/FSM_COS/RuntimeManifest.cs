using TheSingularityWorkshop.FSM_API;

namespace TheSingularityWorkshop.FSM_COS;

/// <summary>Machine-oriented description of the runtime FSM_COS must assemble.</summary>
/// <remarks>
/// <see cref="LoadPlan"/> is the staged execution structure for published
/// MicroBundle identities. The legacy <see cref="Bundles"/> collection remains
/// available for compatibility with the alpha.3 composition contract.
/// </remarks>
public sealed record RuntimeManifest(
    ulong RuntimeId,
    IReadOnlyList<BundleRequest> Bundles,
    IStateContext? ExperienceContext = null,
    IReadOnlyList<RuntimeManifestEntry>? LoadPlan = null)
{
    public static RuntimeManifest Empty(
        ulong runtimeId,
        IStateContext? experienceContext = null) =>
        new(runtimeId, Array.Empty<BundleRequest>(), experienceContext);
}
