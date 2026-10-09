using TheSingularityWorkshop.FSM_API;
using TheSingularityWorkshop.FSM_COS;
using Xunit;

namespace FSM_COS.Tests;

public sealed class RuntimeManifestPlanningTests
{
    [Fact]
    public void Staged_plan_preserves_current_versioned_manifest_contract()
    {
        var reference = Reference(7, "1.2.0");
        var entry = new RuntimeManifestEntry(reference, ManifestLoadStage.Resident);
        var schedule = new RuntimeManifestSchedule(new[] { entry }, Array.Empty<RuntimeManifestDependency>());
        var manifest = new RuntimeManifest(
            42,
            new[] { new MicroBundleManifestEntry(7, "1.2.0") },
            LoadPlan: new[] { entry },
            Schedule: schedule);

        manifest.ValidateStagedPlan();

        Assert.Equal("1.2.0", manifest.Bundles[0].Version);
        Assert.Equal(reference, manifest.LoadPlan![0].Reference);
    }

    [Fact]
    public void Staged_plan_rejects_version_drift_before_composition()
    {
        var entry = new RuntimeManifestEntry(Reference(7, "1.2.0"));
        var manifest = new RuntimeManifest(
            42,
            new[] { new MicroBundleManifestEntry(7, "2.0.0") },
            LoadPlan: new[] { entry });

        Assert.Throws<InvalidOperationException>(manifest.ValidateStagedPlan);
    }

    [Fact]
    public void Schedule_separates_entries_without_prerequisites_and_enforces_readiness()
    {
        var root = new RuntimeManifestEntry(Reference(1, "1.0.0"));
        var dependent = new RuntimeManifestEntry(Reference(2, "1.0.0"), ManifestLoadStage.Deferred);
        var schedule = new RuntimeManifestSchedule(
            new[] { root, dependent },
            new[] { new RuntimeManifestDependency(2, 1) });

        Assert.Equal(new ulong[] { 1 }, schedule.IndependentEntries.Select(x => x.Reference.BundleId));
        Assert.Equal(new ulong[] { 2 }, schedule.DependencyEntries.Select(x => x.Reference.BundleId));
        Assert.False(schedule.IsDependencyReady(2, _ => false));
        Assert.True(schedule.IsDependencyReady(2, id => id == 1));
    }

    [Fact]
    public void Schedule_rejects_dependency_cycles()
    {
        var first = new RuntimeManifestEntry(Reference(1, "1.0.0"));
        var second = new RuntimeManifestEntry(Reference(2, "1.0.0"));

        Assert.Throws<ArgumentException>(() => new RuntimeManifestSchedule(
            new[] { first, second },
            new[]
            {
                new RuntimeManifestDependency(1, 2),
                new RuntimeManifestDependency(2, 1)
            }));
    }

    [Fact]
    public void Bootstrap_entries_are_offered_before_resident_and_deferred_entries()
    {
        var resident = new RuntimeManifestEntry(Reference(1, "1.0.0"), ManifestLoadStage.Resident);
        var deferred = new RuntimeManifestEntry(Reference(2, "1.0.0"), ManifestLoadStage.Deferred);
        var bootstrap = new RuntimeManifestEntry(Reference(3, "1.0.0"), ManifestLoadStage.Bootstrap);
        var plan = new RuntimeManifestLoadPlan(new[] { resident, deferred, bootstrap });

        plan.MarkLocalized(1);
        plan.MarkLocalized(2);
        plan.MarkLocalized(3);

        var promotions = plan.EvaluatePromotions(new AcceptAllEvaluator(), null)
            .Select(entry => entry.Reference.BundleId)
            .ToArray();

        Assert.Equal(new ulong[] { 3, 1, 2 }, promotions);
    }

    [Fact]
    public void Deferred_promotion_waits_for_localization_and_prerequisite_load()
    {
        var root = new RuntimeManifestEntry(Reference(1, "1.0.0"));
        var deferred = new RuntimeManifestEntry(Reference(2, "1.0.0"), ManifestLoadStage.Deferred);
        var entries = new[] { root, deferred };
        var schedule = new RuntimeManifestSchedule(
            entries,
            new[] { new RuntimeManifestDependency(2, 1) });
        var plan = new RuntimeManifestLoadPlan(entries, schedule);
        var evaluator = new AcceptAllEvaluator();

        plan.MarkLocalized(1);
        plan.MarkLocalized(2);

        Assert.Empty(plan.EvaluatePromotions(evaluator, null)
            .Where(entry => entry.Reference.BundleId == 2));
        Assert.Throws<InvalidOperationException>(() => plan.MarkLoaded(2));

        plan.MarkLoaded(1);

        Assert.Contains(plan.EvaluatePromotions(evaluator, null),
            entry => entry.Reference.BundleId == 2);
        plan.MarkLoaded(2);
        Assert.Equal(MicroBundleRuntimeState.Loaded, plan.GetState(2));
    }

    private static MicroBundleReference Reference(ulong id, string version) =>
        new(id, version, $"sha256:{id}-{version}");

    private sealed class AcceptAllEvaluator : IManifestLoadEvaluator
    {
        public bool ShouldLoad(RuntimeManifestEntry entry, IStateContext? experienceContext) => true;
    }
}
