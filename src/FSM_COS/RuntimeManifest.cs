using TheSingularityWorkshop.FSM_API;

namespace TheSingularityWorkshop.FSM_COS;

/// <summary>Machine-oriented description of the MicroBundles and versions FSM_COS must assemble.</summary>
public sealed record RuntimeManifest(
    ulong RuntimeId,
    IReadOnlyList<MicroBundleManifestEntry> Bundles,
    IStateContext? ExperienceContext = null)
{
    public static RuntimeManifest Empty(ulong runtimeId, IStateContext? experienceContext = null) =>
        new(runtimeId, Array.Empty<MicroBundleManifestEntry>(), experienceContext);
}
