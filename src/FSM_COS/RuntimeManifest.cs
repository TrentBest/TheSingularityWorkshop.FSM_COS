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

        var plan = LoadPlan ?? Schedule!.Entries;
        for (var index = 0; index < plan.Count; index++)
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
        foreach (var requested in Bundles)
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

        var scheduleById = Schedule.Entries.ToDictionary(entry => entry.Reference.BundleId);
        foreach (var entry in LoadPlan)
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
}
