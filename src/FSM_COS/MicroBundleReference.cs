namespace TheSingularityWorkshop.FSM_COS;

/// <summary>
/// Immutable published identity for one MicroBundle artifact.
/// </summary>
/// <remarks>
/// The reference records the identity of expected bytes; it does not itself
/// resolve the artifact or verify its content hash. Those checks belong at
/// the repository/localization boundary.
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

    public bool Matches(MicroBundleManifestEntry entry) =>
        IsValid &&
        BundleId == entry.BundleId &&
        string.Equals(Version, entry.Version, StringComparison.Ordinal);
}
