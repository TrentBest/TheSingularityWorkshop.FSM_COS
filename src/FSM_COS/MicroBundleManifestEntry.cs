namespace TheSingularityWorkshop.FSM_COS;

/// <summary>Identifies a MicroBundle and the version requested by a runtime manifest.</summary>
public readonly record struct MicroBundleManifestEntry
{
    public MicroBundleManifestEntry(ulong bundleId, string version)
    {
        if (bundleId == 0)
            throw new ArgumentOutOfRangeException(nameof(bundleId), "A MicroBundle manifest ID must be non-zero.");

        if (string.IsNullOrWhiteSpace(version))
            throw new ArgumentException("A MicroBundle manifest version is required.", nameof(version));

        BundleId = bundleId;
        Version = version;
    }

    public ulong BundleId { get; }
    public string Version { get; }
}
