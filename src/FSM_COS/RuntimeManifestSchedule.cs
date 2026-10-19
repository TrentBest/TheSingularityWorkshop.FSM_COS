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

        for (var index = 0; index < entries.Count; index++)
        {
            var entry = entries[index];
            if (entry is null || !entry.IsValid)
            {
                var identity = entry is null ? "null entry" : $"MicroBundle {entry.Reference.BundleId}";
                throw new ArgumentException(
                    $"Manifest entry at index {index} must have a valid published identity ({identity}).",
                    nameof(entries));
            }
        }

        for (var index = 0; index < dependencies.Count; index++)
        {
            var dependency = dependencies[index];
            if (!dependency.IsValid)
            {
                throw new ArgumentException(
                    $"Dependency edge at index {index} (MicroBundle {dependency.BundleId} -> {dependency.DependencyId}) must be valid.",
                    nameof(dependencies));
            }
        }

        var ids = entries.Select(entry => entry.Reference.BundleId).ToHashSet();
        if (ids.Count != entries.Count)
        {
            var duplicateId = entries
                .GroupBy(entry => entry.Reference.BundleId)
                .First(group => group.Count() > 1)
                .Key;
            throw new ArgumentException(
                $"Manifest entries contain duplicate MicroBundle ID {duplicateId}.",
                nameof(entries));
        }

        var missingEntryEdge = dependencies.FirstOrDefault(edge =>
            !ids.Contains(edge.BundleId) || !ids.Contains(edge.DependencyId));
        if (dependencies.Any(edge => !ids.Contains(edge.BundleId) || !ids.Contains(edge.DependencyId)))
        {
            throw new ArgumentException(
                $"Dependency edge MicroBundle {missingEntryEdge.BundleId} -> {missingEntryEdge.DependencyId} references an ID absent from the manifest entries.",
                nameof(dependencies));
        }

        if (dependencies.Distinct().Count() != dependencies.Count)
        {
            var duplicate = dependencies
                .GroupBy(edge => edge)
                .First(group => group.Count() > 1)
                .Key;
            throw new ArgumentException(
                $"Duplicate dependency edge MicroBundle {duplicate.BundleId} -> {duplicate.DependencyId} is not allowed.",
                nameof(dependencies));
        }

        Entries = Array.AsReadOnly(entries.ToArray());
        Dependencies = Array.AsReadOnly(dependencies.ToArray());

        var entriesWithPrerequisites = dependencies.Select(edge => edge.BundleId).ToHashSet();
        IndependentEntries = Array.AsReadOnly(Entries.Where(entry => !entriesWithPrerequisites.Contains(entry.Reference.BundleId)).ToArray());
        DependencyEntries = Array.AsReadOnly(Entries.Where(entry => entriesWithPrerequisites.Contains(entry.Reference.BundleId)).ToArray());

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
        // Use adjacency lists and an explicit stack so untrusted/deep plans cannot exhaust the process stack.
        var outgoing = Dependencies
            .GroupBy(edge => edge.BundleId)
            .ToDictionary(group => group.Key, group => group.Select(edge => edge.DependencyId).ToArray());
        var states = new Dictionary<ulong, byte>();

        foreach (var rootId in ids)
        {
            if (states.ContainsKey(rootId))
                continue;

            states[rootId] = 1;
            var stack = new List<(ulong BundleId, int NextDependencyIndex)> { (rootId, 0) };

            while (stack.Count > 0)
            {
                var (bundleId, nextDependencyIndex) = stack[^1];
                var dependencies = outgoing.TryGetValue(bundleId, out var found)
                    ? found
                    : Array.Empty<ulong>();

                if (nextDependencyIndex < dependencies.Length)
                {
                    stack[^1] = (bundleId, nextDependencyIndex + 1);
                    var dependencyId = dependencies[nextDependencyIndex];

                    if (states.TryGetValue(dependencyId, out var state))
                    {
                        if (state == 1)
                            throw new ArgumentException(
                                $"Manifest dependency cycle detected at MicroBundle {dependencyId}.",
                                nameof(Dependencies));
                        continue;
                    }

                    states[dependencyId] = 1;
                    stack.Add((dependencyId, 0));
                    continue;
                }

                stack.RemoveAt(stack.Count - 1);
                states[bundleId] = 2;
            }
        }
    }
}
