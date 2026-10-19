using TheSingularityWorkshop.FSM_API;

namespace TheSingularityWorkshop.FSM_COS;

/// <summary>
/// Tracks localization/composition availability for published manifest entries.
/// </summary>
/// <remarks>
/// This state tracker is separate from the composition engine. A host or
/// repository adapter must perform localization and actual loading, then report
/// successful transitions here; constructing this plan does not load anything.
/// </remarks>
public sealed class RuntimeManifestLoadPlan
{
    private readonly Dictionary<ulong, MicroBundleRuntimeState> _states;
    private readonly RuntimeManifestSchedule? _schedule;

    public RuntimeManifestLoadPlan(
        IReadOnlyList<RuntimeManifestEntry> entries,
        RuntimeManifestSchedule? schedule = null)
    {
        ArgumentNullException.ThrowIfNull(entries);

        for (var index = 0; index < entries.Count; index++)
        {
            var entry = entries[index];
            if (entry is null || !entry.IsValid)
            {
                var identity = entry is null ? "null entry" : $"MicroBundle {entry.Reference.BundleId}";
                throw new ArgumentException(
                    $"Load-plan entry at index {index} must have a valid published identity ({identity}).",
                    nameof(entries));
            }
        }

        var duplicateId = entries
            .GroupBy(entry => entry.Reference.BundleId)
            .FirstOrDefault(group => group.Count() > 1)?.Key;
        if (duplicateId.HasValue)
            throw new ArgumentException(
                $"Load-plan entries contain duplicate MicroBundle ID {duplicateId.Value}.",
                nameof(entries));

        Entries = Array.AsReadOnly(entries.ToArray());
        _schedule = schedule;
        if (_schedule is not null)
        {
            var scheduled = _schedule.Entries.ToDictionary(entry => entry.Reference.BundleId);
            var mismatch = Entries.FirstOrDefault(entry =>
                !scheduled.TryGetValue(entry.Reference.BundleId, out var scheduledEntry) ||
                scheduledEntry.Reference != entry.Reference ||
                scheduledEntry.Stage != entry.Stage);
            if (mismatch is not null)
            {
                throw new ArgumentException(
                    $"The load plan and dependency schedule disagree for MicroBundle {mismatch.Reference.BundleId} identity or stage.",
                    nameof(schedule));
            }

            if (scheduled.Count != Entries.Count)
            {
                var planIds = Entries.Select(entry => entry.Reference.BundleId).ToHashSet();
                var unexpected = scheduled.Keys.Except(planIds).OrderBy(id => id);
                throw new ArgumentException(
                    $"The dependency schedule contains MicroBundle IDs absent from the load plan: [{string.Join(", ", unexpected)}].",
                    nameof(schedule));
            }
        }

        _states = Entries.ToDictionary(
            entry => entry.Reference.BundleId,
            _ => MicroBundleRuntimeState.Published);
    }

    public IReadOnlyList<RuntimeManifestEntry> Entries { get; }

    public MicroBundleRuntimeState GetState(ulong bundleId) =>
        _states.TryGetValue(bundleId, out var state)
            ? state
            : throw new KeyNotFoundException($"MicroBundle {bundleId} is not present in the load plan.");

    public void MarkLocalized(ulong bundleId)
    {
        EnsureKnown(bundleId);
        if (GetState(bundleId) == MicroBundleRuntimeState.Loaded) return;
        _states[bundleId] = MicroBundleRuntimeState.Localized;
    }

    public void MarkLoaded(ulong bundleId)
    {
        EnsureKnown(bundleId);
        if (GetState(bundleId) != MicroBundleRuntimeState.Localized)
            throw new InvalidOperationException($"MicroBundle {bundleId} must be localized before it can be loaded.");

        if (_schedule is not null &&
            !_schedule.IsDependencyReady(bundleId,
                dependencyId => GetState(dependencyId) == MicroBundleRuntimeState.Loaded))
        {
            throw new InvalidOperationException(
                $"MicroBundle {bundleId} cannot be loaded until its dependencies are loaded.");
        }

        _states[bundleId] = MicroBundleRuntimeState.Loaded;
    }

    public IEnumerable<RuntimeManifestEntry> EvaluatePromotions(
        IManifestLoadEvaluator evaluator,
        IStateContext? experienceContext,
        RuntimeManifestSchedule? schedule = null)
    {
        ArgumentNullException.ThrowIfNull(evaluator);
        schedule ??= _schedule;
        if (schedule is not null)
        {
            var scheduled = schedule.Entries.ToDictionary(entry => entry.Reference.BundleId);
            var mismatch = Entries.FirstOrDefault(entry =>
                !scheduled.TryGetValue(entry.Reference.BundleId, out var scheduledEntry) ||
                scheduledEntry.Reference != entry.Reference ||
                scheduledEntry.Stage != entry.Stage);
            if (mismatch is not null)
            {
                throw new ArgumentException(
                    $"The promotion schedule disagrees with load-plan MicroBundle {mismatch.Reference.BundleId} identity or stage.",
                    nameof(schedule));
            }

            if (scheduled.Count != Entries.Count)
            {
                var planIds = Entries.Select(entry => entry.Reference.BundleId).ToHashSet();
                var unexpected = scheduled.Keys.Except(planIds).OrderBy(id => id);
                throw new ArgumentException(
                    $"The promotion schedule contains MicroBundle IDs absent from the load plan: [{string.Join(", ", unexpected)}].",
                    nameof(schedule));
            }

            if (_schedule is not null &&
                !_schedule.Dependencies.ToHashSet().SetEquals(schedule.Dependencies))
            {
                throw new ArgumentException(
                    "The promotion schedule must preserve the dependency edges validated for this load plan.",
                    nameof(schedule));
            }
        }

        foreach (var entry in Entries.OrderBy(entry => entry.Stage))
        {
            if (GetState(entry.Reference.BundleId) != MicroBundleRuntimeState.Localized)
                continue;

            var prerequisitesReady = schedule is null ||
                schedule.IsDependencyReady(entry.Reference.BundleId,
                    dependencyId => GetState(dependencyId) == MicroBundleRuntimeState.Loaded);
            if (!prerequisitesReady)
                continue;

            if (entry.Stage != ManifestLoadStage.Deferred ||
                evaluator.ShouldLoad(entry, experienceContext))
                yield return entry;
        }
    }

    private void EnsureKnown(ulong bundleId)
    {
        if (!_states.ContainsKey(bundleId))
            throw new KeyNotFoundException($"MicroBundle {bundleId} is not present in the load plan.");
    }
}

/// <summary>Availability state for one published MicroBundle.</summary>
public enum MicroBundleRuntimeState
{
    Published = 0,
    Localized = 1,
    Loaded = 2
}
