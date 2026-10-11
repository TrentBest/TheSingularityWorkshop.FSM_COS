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

        Assert.DoesNotContain(
            plan.EvaluatePromotions(evaluator, null),
            entry => entry.Reference.BundleId == 2);
        Assert.Throws<InvalidOperationException>(() => plan.MarkLoaded(2));

        plan.MarkLoaded(1);

        Assert.Contains(plan.EvaluatePromotions(evaluator, null),
            entry => entry.Reference.BundleId == 2);
        plan.MarkLoaded(2);
        Assert.Equal(MicroBundleRuntimeState.Loaded, plan.GetState(2));
    }

    [Fact]
    public void Published_reference_validity_and_manifest_matching_are_explicit()
    {
        var reference = Reference(7, "1.2.0");
        Assert.True(reference.IsValid);
        Assert.True(reference.Matches(new MicroBundleManifestEntry(7, "1.2.0")));
        Assert.False(reference.Matches(new MicroBundleManifestEntry(7, "2.0.0")));
        Assert.False(reference.Matches(new MicroBundleManifestEntry(8, "1.2.0")));
        Assert.False(new MicroBundleReference(0, "1.2.0", "sha256:content").IsValid);
        Assert.False(new MicroBundleReference(7, " ", "sha256:content").IsValid);
        Assert.False(new MicroBundleReference(7, "1.2.0", " ").IsValid);
    }

    [Fact]
    public void Schedule_rejects_invalid_entries_edges_and_duplicate_relationships()
    {
        var valid = new RuntimeManifestEntry(Reference(1, "1.0.0"));
        var invalid = new RuntimeManifestEntry(new MicroBundleReference(0, "", ""));

        Assert.Throws<ArgumentNullException>(() =>
            new RuntimeManifestSchedule(null!, Array.Empty<RuntimeManifestDependency>()));
        Assert.Throws<ArgumentNullException>(() =>
            new RuntimeManifestSchedule(new[] { valid }, null!));
        Assert.Throws<ArgumentException>(() =>
            new RuntimeManifestSchedule(new[] { invalid }, Array.Empty<RuntimeManifestDependency>()));
        Assert.Throws<ArgumentException>(() =>
            new RuntimeManifestSchedule(new[] { valid, valid }, Array.Empty<RuntimeManifestDependency>()));
        Assert.Throws<ArgumentException>(() =>
            new RuntimeManifestSchedule(new[] { valid }, new[] { new RuntimeManifestDependency(1, 1) }));
        Assert.Throws<ArgumentException>(() =>
            new RuntimeManifestSchedule(new[] { valid }, new[] { new RuntimeManifestDependency(1, 2) }));
        Assert.Throws<ArgumentException>(() =>
            new RuntimeManifestSchedule(
                new[] { valid, new RuntimeManifestEntry(Reference(2, "1.0.0")) },
                new[] { new RuntimeManifestDependency(2, 1), new RuntimeManifestDependency(2, 1) }));
    }

    [Fact]
    public void Schedule_readiness_rejects_unknown_entries_and_null_predicates()
    {
        var schedule = new RuntimeManifestSchedule(
            new[] { new RuntimeManifestEntry(Reference(1, "1.0.0")) },
            Array.Empty<RuntimeManifestDependency>());

        Assert.Throws<ArgumentNullException>(() => schedule.IsDependencyReady(1, null!));
        Assert.Throws<KeyNotFoundException>(() => schedule.IsDependencyReady(99, _ => true));
    }

    [Fact]
    public void Load_plan_rejects_invalid_entries_and_a_different_schedule()
    {
        var valid = new RuntimeManifestEntry(Reference(1, "1.0.0"), ManifestLoadStage.Resident);
        var differentStage = new RuntimeManifestEntry(Reference(1, "1.0.0"), ManifestLoadStage.Deferred);
        var schedule = new RuntimeManifestSchedule(
            new[] { differentStage },
            Array.Empty<RuntimeManifestDependency>());

        Assert.Throws<ArgumentNullException>(() => new RuntimeManifestLoadPlan(null!));
        Assert.Throws<ArgumentException>(() => new RuntimeManifestLoadPlan(new[]
        {
            new RuntimeManifestEntry(new MicroBundleReference(0, "", ""))
        }));
        Assert.Throws<ArgumentException>(() => new RuntimeManifestLoadPlan(new[] { valid, valid }));
        Assert.Throws<ArgumentException>(() => new RuntimeManifestLoadPlan(new[] { valid }, schedule));
    }

    [Fact]
    public void Load_plan_guards_state_transitions_and_unknown_ids()
    {
        var plan = new RuntimeManifestLoadPlan(new[]
        {
            new RuntimeManifestEntry(Reference(1, "1.0.0"))
        });

        Assert.Throws<KeyNotFoundException>(() => plan.GetState(99));
        Assert.Throws<KeyNotFoundException>(() => plan.MarkLocalized(99));
        Assert.Throws<KeyNotFoundException>(() => plan.MarkLoaded(99));
        Assert.Throws<InvalidOperationException>(() => plan.MarkLoaded(1));
        Assert.Throws<ArgumentNullException>(() => plan.EvaluatePromotions(null!, null).ToArray());

        plan.MarkLocalized(1);
        plan.MarkLoaded(1);
        Assert.Equal(MicroBundleRuntimeState.Loaded, plan.GetState(1));
    }

    [Fact]
    public void Manifest_plan_requires_every_requested_root_and_matching_schedule_metadata()
    {
        var planned = new RuntimeManifestEntry(Reference(2, "1.0.0"));
        var manifestMissingRoot = new RuntimeManifest(
            42,
            new[] { new MicroBundleManifestEntry(1, "1.0.0") },
            LoadPlan: new[] { planned });

        Assert.Throws<InvalidOperationException>(manifestMissingRoot.ValidateStagedPlan);

        var resident = new RuntimeManifestEntry(Reference(1, "1.0.0"), ManifestLoadStage.Resident);
        var deferred = new RuntimeManifestEntry(Reference(1, "1.0.0"), ManifestLoadStage.Deferred);
        var manifestWithMismatch = new RuntimeManifest(
            42,
            new[] { new MicroBundleManifestEntry(1, "1.0.0") },
            LoadPlan: new[] { resident },
            Schedule: new RuntimeManifestSchedule(new[] { deferred }, Array.Empty<RuntimeManifestDependency>()));

        Assert.Throws<InvalidOperationException>(manifestWithMismatch.ValidateStagedPlan);
    }

    [Fact]
    public void Deferred_evaluator_can_decline_a_localized_bundle()
    {
        var deferred = new RuntimeManifestEntry(Reference(2, "1.0.0"), ManifestLoadStage.Deferred);
        var plan = new RuntimeManifestLoadPlan(new[] { deferred });
        plan.MarkLocalized(2);

        var promotions = plan.EvaluatePromotions(new DeclineDeferredEvaluator(), null);

        Assert.Empty(promotions);
    }

    private static MicroBundleReference Reference(ulong id, string version) =>
        new(id, version, $"sha256:{id}-{version}");

    private sealed class AcceptAllEvaluator : IManifestLoadEvaluator
    {
        public bool ShouldLoad(RuntimeManifestEntry entry, IStateContext? experienceContext) => true;
    }

    private sealed class DeclineDeferredEvaluator : IManifestLoadEvaluator
    {
        public bool ShouldLoad(RuntimeManifestEntry entry, IStateContext? experienceContext) => false;
    }
}
