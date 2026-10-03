using TheSingularityWorkshop.FSM_API;
using TheSingularityWorkshop.FSM_UserIO;
using TheSingularityWorkshop.MicroBundleDomain;

namespace TheSingularityWorkshop.FSM_COS;

/// <summary>Machine-oriented description of the runtime FSM_COS must assemble.</summary>
public sealed record RuntimeManifest(
    ulong RuntimeId,
    IReadOnlyList<MicroBundleDependencyRequest> Bundles,
    IStateContext? ExperienceContext = null,
    SemanticIntent? Intent = null)
{
    public static RuntimeManifest Empty(
        ulong runtimeId,
        IStateContext? experienceContext = null,
        SemanticIntent? intent = null) =>
        new(
            runtimeId,
            Array.Empty<MicroBundleDependencyRequest>(),
            experienceContext,
            intent);
}
