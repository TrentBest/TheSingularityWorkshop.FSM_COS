using TheSingularityWorkshop.FSM_API;
using TheSingularityWorkshop.FSM_UserIO;

namespace TheSingularityWorkshop.FSM_COS;

/// <summary>
/// Machine-oriented description of the MicroBundles and versions FSM_COS must assemble.
/// </summary>
/// <param name="RuntimeId">Stable identity assigned to this runtime composition.</param>
/// <param name="Bundles">Versioned root MicroBundles requested by the host.</param>
/// <param name="ExperienceContext">Optional state context shared with the composition arbitration process.</param>
/// <param name="LoadPlan">Optional published identity/stage metadata; does not itself localize or load bundles.</param>
/// <param name="Schedule">Optional publish-time dependency plan, distinct from FSM_API process scheduling.</param>
/// <param name="Intent">Optional semantic intent supplied by the host for the assembled runtime.</param>
public sealed record RuntimeManifest(
    ulong RuntimeId,
    IReadOnlyList<MicroBundleManifestEntry> Bundles,
    IStateContext? ExperienceContext = null,
    IReadOnlyList<RuntimeManifestEntry>? LoadPlan = null,
    RuntimeManifestSchedule? Schedule = null,
    SemanticIntent? Intent = null)
{
    /// <summary>Gets or initializes the loading strategy. Sequential loading is the compatibility default.</summary>
    public RuntimeLoadingMode LoadingMode { get; init; } = RuntimeLoadingMode.Sequential;

    /// <summary>Gets or initializes the maximum parallel bundle loads when <see cref="LoadingMode"/> is Parallel.</summary>
    public int MaxDegreeOfParallelism { get; init; } = 4;

    public static RuntimeManifest Empty(
        ulong runtimeId,
        IStateContext? experienceContext = null,
        SemanticIntent? intent = null) =>
        new(runtimeId, Array.Empty<MicroBundleManifestEntry>(), experienceContext, Intent: intent);

    /// <summary>
    /// Validates consistency between the current versioned root contract and optional staged-plan metadata.
    /// </summary>
    /// <remarks>
    /// This validation does not claim that artifact bytes have been localized or that staged execution is active.
    /// </remarks>
    public void ValidateStagedPlan()
    {
        ArgumentNullException.ThrowIfNull(Bundles);

        if (LoadPlan is null && Schedule is null)
            return;

        // Bound enumeration here too: callers can invoke this public validator directly,
        // without going through FsmCos.Execute's input snapshots.
        var bundles = SnapshotBounded(
            Bundles,
            RuntimeManifestSchedule.DefaultMaximumEntryCount,
            "Runtime manifest root bundles");
        var plan = SnapshotBounded(
            LoadPlan ?? Schedule!.Entries,
            RuntimeManifestSchedule.DefaultMaximumEntryCount,
            "Runtime manifest load plan");
        for (var index = 0; index < plan.Length; index++)
        {
            var entry = plan[index];
            if (entry is null || !entry.IsValid)
            {
                var identity = entry is null ? "null entry" : $"MicroBundle {entry.Reference.BundleId}";
                throw new InvalidOperationException(
                    $"Runtime manifest staged-plan entry at index {index} has an invalid published identity ({identity}).");
            }
        }

        var duplicateId = plan
            .GroupBy(entry => entry.Reference.BundleId)
            .FirstOrDefault(group => group.Count() > 1)?.Key;
        if (duplicateId.HasValue)
            throw new InvalidOperationException(
                $"Runtime manifest load plan contains duplicate MicroBundle ID {duplicateId.Value}.");

        var byId = plan.ToDictionary(entry => entry.Reference.BundleId);
        foreach (var requested in bundles)
        {
            if (!byId.TryGetValue(requested.BundleId, out var planned))
                throw new InvalidOperationException(
                    $"Runtime manifest load plan is missing requested MicroBundle {requested.BundleId}.");

            if (!string.Equals(planned.Reference.Version, requested.Version, StringComparison.Ordinal))
                throw new InvalidOperationException(
                    $"Runtime manifest load plan requests MicroBundle {requested.BundleId} at version " +
                    $"'{planned.Reference.Version}', but the root manifest requests '{requested.Version}'.");
        }

        if (Schedule is null || LoadPlan is null)
            return;

        var scheduleEntries = SnapshotBounded(
            Schedule.Entries,
            RuntimeManifestSchedule.DefaultMaximumEntryCount,
            "Runtime manifest schedule");
        var scheduleById = scheduleEntries.ToDictionary(entry => entry.Reference.BundleId);
        foreach (var entry in plan)
        {
            if (!scheduleById.TryGetValue(entry.Reference.BundleId, out var scheduled) ||
                scheduled.Reference != entry.Reference ||
                scheduled.Stage != entry.Stage)
            {
                throw new InvalidOperationException(
                    $"Runtime manifest schedule and load plan disagree for MicroBundle {entry.Reference.BundleId} identity or stage.");
            }
        }

        if (scheduleById.Count != byId.Count)
        {
            var unexpected = scheduleById.Keys.Except(byId.Keys).OrderBy(id => id);
            throw new InvalidOperationException(
                $"Runtime manifest schedule contains MicroBundle IDs absent from the load plan: [{string.Join(", ", unexpected)}].");
        }
    }

    private static T[] SnapshotBounded<T>(
        IEnumerable<T> source,
        int maximumCount,
        string description)
    {
        var snapshot = new List<T>(Math.Min(maximumCount, 256));
        foreach (var item in source)
        {
            if (snapshot.Count >= maximumCount)
                throw new InvalidOperationException(
                    $"{description} exceeds the configured maximum of {maximumCount} entries.");
            snapshot.Add(item);
        }

        return snapshot.ToArray();
    }
}
