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

        if (entries.Any(entry => entry is null || !entry.IsValid))
            throw new ArgumentException("Every load-plan entry must have a valid published identity.", nameof(entries));

        if (entries.Select(entry => entry.Reference.BundleId).Distinct().Count() != entries.Count)
            throw new ArgumentException("Load-plan entries must have unique MicroBundle IDs.", nameof(entries));

        Entries = entries.ToArray();
        _schedule = schedule;
        if (_schedule is not null)
        {
            var scheduled = _schedule.Entries.ToDictionary(entry => entry.Reference.BundleId);
            if (scheduled.Count != Entries.Count ||
                Entries.Any(entry =>
                    !scheduled.TryGetValue(entry.Reference.BundleId, out var scheduledEntry) ||
                    scheduledEntry.Reference != entry.Reference ||
                    scheduledEntry.Stage != entry.Stage))
            {
                throw new ArgumentException(
                    "The load plan and dependency schedule must contain the same identities and stages.",
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

        foreach (var entry in Entries)
        {
            if (GetState(entry.Reference.BundleId) != MicroBundleRuntimeState.Localized)
                continue;

            if (entry.Stage == ManifestLoadStage.Resident)
            {
                if (schedule is null || schedule.IsDependencyReady(entry.Reference.BundleId,
                        dependencyId => GetState(dependencyId) == MicroBundleRuntimeState.Loaded))
                    yield return entry;

                continue;
            }

            if (evaluator.ShouldLoad(entry, experienceContext) &&
                (schedule is null || schedule.IsDependencyReady(entry.Reference.BundleId,
                    dependencyId => GetState(dependencyId) == MicroBundleRuntimeState.Loaded)))
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
