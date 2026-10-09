namespace TheSingularityWorkshop.FSM_COS;

/// <summary>
/// Publish-time dependency analysis for a runtime manifest.
/// </summary>
/// <remarks>
/// This is a dependency plan, not a process scheduler. FSM_API remains the
/// owner of sequential process-group scheduling, while FSM_COS owns runtime
/// composition. Repository/cache implementations own localization.
/// </remarks>
public sealed class RuntimeManifestSchedule
{
    public RuntimeManifestSchedule(
        IReadOnlyList<RuntimeManifestEntry> entries,
        IReadOnlyList<RuntimeManifestDependency> dependencies)
    {
        ArgumentNullException.ThrowIfNull(entries);
        ArgumentNullException.ThrowIfNull(dependencies);

        if (entries.Any(entry => entry is null || !entry.IsValid))
            throw new ArgumentException("All manifest entries must have valid published identities.", nameof(entries));

        if (dependencies.Any(dependency => !dependency.IsValid))
            throw new ArgumentException("All dependency edges must be valid.", nameof(dependencies));

        var ids = entries.Select(entry => entry.Reference.BundleId).ToHashSet();
        if (ids.Count != entries.Count)
            throw new ArgumentException("Manifest entries must have unique MicroBundle IDs.", nameof(entries));

        if (dependencies.Any(edge => !ids.Contains(edge.BundleId) || !ids.Contains(edge.DependencyId)))
            throw new ArgumentException("Every dependency edge must reference an entry in the manifest.", nameof(dependencies));

        if (dependencies.Distinct().Count() != dependencies.Count)
            throw new ArgumentException("Duplicate dependency edges are not allowed.", nameof(dependencies));

        Entries = entries.ToArray();
        Dependencies = dependencies.ToArray();

        var entriesWithPrerequisites = dependencies.Select(edge => edge.BundleId).ToHashSet();
        IndependentEntries = Entries.Where(entry => !entriesWithPrerequisites.Contains(entry.Reference.BundleId)).ToArray();
        DependencyEntries = Entries.Where(entry => entriesWithPrerequisites.Contains(entry.Reference.BundleId)).ToArray();

        ValidateAcyclic(ids);
    }

    public IReadOnlyList<RuntimeManifestEntry> Entries { get; }
    public IReadOnlyList<RuntimeManifestDependency> Dependencies { get; }
    public IReadOnlyList<RuntimeManifestEntry> IndependentEntries { get; }
    public IReadOnlyList<RuntimeManifestEntry> DependencyEntries { get; }

    public IReadOnlyList<RuntimeManifestDependency> DependenciesOf(ulong bundleId) =>
        Dependencies.Where(edge => edge.BundleId == bundleId).ToArray();

    public bool IsDependencyReady(ulong bundleId, Func<ulong, bool> isLoaded)
    {
        ArgumentNullException.ThrowIfNull(isLoaded);
        if (!Entries.Any(entry => entry.Reference.BundleId == bundleId))
            throw new KeyNotFoundException($"MicroBundle {bundleId} is not present in the schedule.");

        return DependenciesOf(bundleId).All(edge => isLoaded(edge.DependencyId));
    }

    private void ValidateAcyclic(HashSet<ulong> ids)
    {
        var visited = new HashSet<ulong>();
        var visiting = new HashSet<ulong>();

        void Visit(ulong id)
        {
            if (visited.Contains(id)) return;
            if (!visiting.Add(id))
                throw new ArgumentException($"Manifest dependency cycle detected at MicroBundle {id}.", nameof(Dependencies));

            foreach (var edge in Dependencies.Where(edge => edge.BundleId == id))
                Visit(edge.DependencyId);

            visiting.Remove(id);
            visited.Add(id);
        }

        foreach (var id in ids) Visit(id);
    }
}
