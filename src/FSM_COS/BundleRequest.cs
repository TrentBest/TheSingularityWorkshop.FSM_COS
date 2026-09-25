namespace TheSingularityWorkshop.FSM_COS;

/// <summary>Requests one MicroBundle and optional configuration.</summary>
public readonly record struct BundleRequest(
    ulong BundleId,
    ReadOnlyMemory<byte> Configuration)
{
    public static BundleRequest Unconfigured(ulong bundleId) =>
        new(bundleId, ReadOnlyMemory<byte>.Empty);
}
