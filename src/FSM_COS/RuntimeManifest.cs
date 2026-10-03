using TheSingularityWorkshop.FSM_API;
using TheSingularityWorkshop.MicroBundleDomain;

namespace TheSingularityWorkshop.FSM_COS;

/// <summary>Machine-oriented description of the runtime FSM_COS must assemble.</summary>
public sealed record RuntimeManifest(
    ulong RuntimeId,
    IReadOnlyList<MicroBundleDependencyRequest> Bundles,
    IStateContext? ExperienceContext = null)
{
    public static RuntimeManifest Empty(ulong runtimeId, IStateContext? experienceContext = null) =>
        new(runtimeId, Array.Empty<MicroBundleDependencyRequest>(), experienceContext);
}
