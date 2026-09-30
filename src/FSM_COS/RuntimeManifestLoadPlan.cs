using TheSingularityWorkshop.FSM_API;

namespace TheSingularityWorkshop.FSM_COS;

/// <summary>
/// Tracks the published, localized, and loaded states of a manifest execution plan.
/// </summary>
/// <remarks>
/// Localization is deliberately distinct from loading. A MicroBundle may be
/// available locally without being instantiated in the active RuntimeAssembly.
/// </remarks>
public sealed class RuntimeManifestLoadPlan
{
    private readonly IReadOnlyList<RuntimeManifestEntry> _entries;
    private readonly Dictionary<ulong, MicroBundleRuntimeState> _states;

    public RuntimeManifestLoadPlan(IReadOnlyList<RuntimeManifestEntry> entries)
    {
        ArgumentNullException.ThrowIfNull(entries);

        if (entries.Any(entry => !entry.IsValid))
            throw new ArgumentException(
                "Every manifest load-plan entry must have a valid identity matching its BundleRequest.",
                nameof(entries));

        _entries = entries;
        _states = entries
            .GroupBy(entry => entry.Reference.BundleId)
            .ToDictionary(group => group.Key, _ => MicroBundleRuntimeState.Published);
    }

    public IReadOnlyList<RuntimeManifestEntry> Entries => _entries;

    public MicroBundleRuntimeState GetState(ulong bundleId) =>
        _states.TryGetValue(bundleId, out var state)
            ? state
            : throw new KeyNotFoundException($"MicroBundle {bundleId} is not present in the manifest.");

    /// <summary>Marks immutable artifact bytes as available in local cache.</summary>
    public void MarkLocalized(ulong bundleId)
    {
        EnsureKnown(bundleId);

        var state = GetState(bundleId);
        if (state == MicroBundleRuntimeState.Loaded) return;

        _states[bundleId] = MicroBundleRuntimeState.Localized;
    }

    /// <summary>Promotes localized data into the active composition.</summary>
    public void MarkLoaded(ulong bundleId)
    {
        EnsureKnown(bundleId);

        if (GetState(bundleId) != MicroBundleRuntimeState.Localized)
            throw new InvalidOperationException(
                $"MicroBundle {bundleId} must be localized before it can be loaded.");

        _states[bundleId] = MicroBundleRuntimeState.Loaded;
    }

    /// <summary>
    /// Returns localized entries that the Experience has elected to promote,
    /// preserving manifest order.
    /// </summary>
    public IEnumerable<RuntimeManifestEntry> EvaluatePromotions(
        IManifestLoadEvaluator evaluator,
        IStateContext? experienceContext)
    {
        ArgumentNullException.ThrowIfNull(evaluator);

        foreach (var entry in _entries)
        {
            if (GetState(entry.Reference.BundleId) != MicroBundleRuntimeState.Localized)
                continue;

            if (entry.Stage == ManifestLoadStage.Resident ||
                evaluator.ShouldLoad(entry, experienceContext))
                yield return entry;
        }
    }

    private void EnsureKnown(ulong bundleId)
    {
        if (!_states.ContainsKey(bundleId))
            throw new KeyNotFoundException(
                $"MicroBundle {bundleId} is not present in the manifest.");
    }
}

/// <summary>Runtime availability state for one published MicroBundle.</summary>
public enum MicroBundleRuntimeState
{
    Published = 0,
    Localized = 1,
    Loaded = 2
}
