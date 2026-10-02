using TheSingularityWorkshop.MicroBundleDomain;

namespace TheSingularityWorkshop.FSM_COS;

/// <summary>
/// Compatibility name for the domain-owned MicroBundle dependency request.
/// </summary>
/// <remarks>
/// Dependency identity and configuration belong to MicroBundleDomain. FSM_COS
/// only retains this derived type temporarily so existing composition callers
/// can migrate without introducing a second semantic contract.
/// </remarks>
public readonly record struct BundleRequest(ulong BundleId, ReadOnlyMemory<byte> Configuration)
{
    public static BundleRequest Unconfigured(ulong bundleId) =>
        new(bundleId, ReadOnlyMemory<byte>.Empty);

    internal MicroBundleDependencyRequest ToDomainRequest() =>
        new(BundleId, Configuration);
}
