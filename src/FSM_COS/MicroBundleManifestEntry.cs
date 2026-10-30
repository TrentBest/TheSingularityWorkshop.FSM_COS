namespace TheSingularityWorkshop.FSM_COS;

/// <summary>
/// Identifies one MicroBundle artifact requested by a runtime manifest.
/// A version-only entry is retained for legacy/in-process use; a published
/// immutable dependency closure must use the content-pinned constructor.
/// </summary>
public readonly record struct MicroBundleManifestEntry
{
    public MicroBundleManifestEntry(ulong bundleId, string version)
        : this(bundleId, version, null)
    {
    }

    /// <summary>Creates a manifest entry pinned to an exact version and SHA-256 content hash.</summary>
    public MicroBundleManifestEntry(ulong bundleId, string version, string? contentHash)
    {
        if (bundleId == 0)
            throw new ArgumentOutOfRangeException(nameof(bundleId), "A MicroBundle manifest ID must be non-zero.");

        if (string.IsNullOrWhiteSpace(version))
            throw new ArgumentException("A MicroBundle manifest version is required.", nameof(version));

        if (contentHash is not null &&
            (contentHash.Length != 64 || contentHash.Any(character => !Uri.IsHexDigit(character))))
            throw new ArgumentException(
                "A content hash must be a 64-character hexadecimal SHA-256 value.",
                nameof(contentHash));

        BundleId = bundleId;
        Version = version;
        // SHA-256 pins use the repository-wide lowercase hexadecimal representation.
#pragma warning disable CA1308 // Lowercase hexadecimal is the canonical MicroBundle hash representation.
        ContentHash = contentHash?.ToLowerInvariant();
#pragma warning restore CA1308
    }

    public ulong BundleId { get; }
    public string Version { get; }

    /// <summary>Gets the expected SHA-256 hash, or null for a legacy version-only entry.</summary>
    public string? ContentHash { get; }

    /// <summary>Gets whether this entry pins both the exact version and artifact bytes.</summary>
    public bool IsPinned => ContentHash is { Length: 64 } &&
        ContentHash.All(Uri.IsHexDigit);
}
