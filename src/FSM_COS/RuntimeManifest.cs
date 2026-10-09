using TheSingularityWorkshop.FSM_API;
using TheSingularityWorkshop.FSM_UserIO;

namespace TheSingularityWorkshop.FSM_COS;

/// <summary>
/// Machine-oriented description of the MicroBundles and versions FSM_COS must assemble.
/// </summary>
/// <param name="LoadPlan">Optional published identity/stage metadata; does not itself localize or load bundles.</param>
/// <param name="Schedule">Optional publish-time dependency plan, distinct from FSM_API process scheduling.</param>
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
        if (LoadPlan is null && Schedule is null)
            return;

        var plan = LoadPlan ?? Schedule!.Entries;
        if (plan.Any(entry => entry is null || !entry.IsValid))
            throw new InvalidOperationException("Runtime manifest contains an invalid staged-plan entry.");

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
        if (scheduleById.Count != byId.Count)
            throw new InvalidOperationException("Runtime manifest schedule and load plan contain different MicroBundle sets.");

        foreach (var entry in LoadPlan)
        {
            if (!scheduleById.TryGetValue(entry.Reference.BundleId, out var scheduled) ||
                scheduled.Reference != entry.Reference ||
                scheduled.Stage != entry.Stage)
            {
                throw new InvalidOperationException(
                    $"Runtime manifest schedule and load plan disagree for MicroBundle {entry.Reference.BundleId}.");
            }
        }
    }
}
