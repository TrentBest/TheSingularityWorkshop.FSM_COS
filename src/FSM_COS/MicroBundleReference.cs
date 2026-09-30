namespace TheSingularityWorkshop.FSM_COS;

/// <summary>
/// Immutable machine identity for a published MicroBundle version.
/// </summary>
/// <remarks>
/// FSM_COS carries this identity but does not resolve it. Repository and cache
/// layers decide where the corresponding immutable bytes live.
/// </remarks>
public readonly record struct MicroBundleReference(
    ulong BundleId,
    string Version,
    string ContentHash)
{
    public bool IsValid =>
        BundleId != 0 &&
        !string.IsNullOrWhiteSpace(Version) &&
        !string.IsNullOrWhiteSpace(ContentHash);
}
