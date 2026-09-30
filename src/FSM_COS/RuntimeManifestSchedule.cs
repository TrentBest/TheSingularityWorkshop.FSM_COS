namespace TheSingularityWorkshop.FSM_COS;

/// <summary>
/// Publish-time execution schedule baked into a RuntimeManifest.
/// </summary>
/// <remarks>
/// The schedule separates fetch opportunity from composition ordering.
/// Independent entries may be localized as one broad batch while dependency
/// work proceeds according to the baked graph. Completed localization is
/// retained; it does not need to be re-requested.
/// </remarks>
public sealed class RuntimeManifestSchedule
{
    public RuntimeManifestSchedule(
        IReadOnlyList<RuntimeManifestEntry> entries,
        IReadOnlyList<RuntimeManifestDependency> dependencies)
    {
        ArgumentNullException.ThrowIfNull(entries);
        ArgumentNullException.ThrowIfNull(dependencies);

        if (entries.Any(entry => !entry.IsValid))
            throw new ArgumentException("All manifest entries must be valid.", nameof(entries));

        if (dependencies.Any(dependency => !dependency.IsValid))
            throw new ArgumentException("All manifest dependencies must be valid.", nameof(dependencies));

        var ids = entries.Select(entry => entry.Reference.BundleId).ToHashSet();

        if (ids.Count != entries.Count)
            throw new ArgumentException("Manifest entries must have unique MicroBundle IDs.", nameof(entries));

        if (dependencies.Any(edge =>
                !ids.Contains(edge.BundleId) ||
                !ids.Contains(edge.DependencyId)))
            throw new ArgumentException(
                "Every dependency edge must reference an entry in the manifest.",
                nameof(dependencies));

        Entries = entries;
        Dependencies = dependencies;

        var dependencyIds = dependencies
            .Select(edge => edge.DependencyId)
            .ToHashSet();

        IndependentEntries = entries
            .Where(entry => !dependencyIds.Contains(entry.Reference.BundleId))
            .ToArray();

        DependencyEntries = entries
            .Where(entry => dependencyIds.Contains(entry.Reference.BundleId))
            .ToArray();
    }

    public IReadOnlyList<RuntimeManifestEntry> Entries { get; }

    public IReadOnlyList<RuntimeManifestDependency> Dependencies { get; }

    /// <summary>
    /// Entries not required by another manifest entry. These are candidates
    /// for broad initial localization.
    /// </summary>
    public IReadOnlyList<RuntimeManifestEntry> IndependentEntries { get; }

    /// <summary>
    /// Entries required by another manifest entry.
    /// </summary>
    public IReadOnlyList<RuntimeManifestEntry> DependencyEntries { get; }

    public IReadOnlyList<RuntimeManifestDependency> DependenciesOf(ulong bundleId) =>
        Dependencies
            .Where(edge => edge.BundleId == bundleId)
            .ToArray();

    public bool IsDependencyReady(ulong bundleId, Func<ulong, bool> isLoaded)
    {
        ArgumentNullException.ThrowIfNull(isLoaded);

        return DependenciesOf(bundleId).All(edge => isLoaded(edge.DependencyId));
    }
}
