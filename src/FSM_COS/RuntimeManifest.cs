using TheSingularityWorkshop.FSM_API;

namespace TheSingularityWorkshop.FSM_COS;

/// <summary>Machine-oriented description of the runtime FSM_COS must assemble.</summary>
public sealed record RuntimeManifest(
    ulong RuntimeId,
    IReadOnlyList<BundleRequest> Bundles,
    IStateContext? ExperienceContext = null)
{
    public static RuntimeManifest Empty(ulong runtimeId, IStateContext? experienceContext = null) =>
        new(runtimeId, Array.Empty<BundleRequest>(), experienceContext);
}
