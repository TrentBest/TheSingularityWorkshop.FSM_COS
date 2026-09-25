namespace TheSingularityWorkshop.FSM_COS;

/// <summary>Machine-oriented description of the runtime FSM_COS must assemble.</summary>
public sealed record RuntimeManifest(
    ulong RuntimeId,
    IReadOnlyList<BundleRequest> Bundles)
{
    public static RuntimeManifest Empty(ulong runtimeId) =>
        new(runtimeId, Array.Empty<BundleRequest>());
}
