using TheSingularityWorkshop.FSM_API;
using TheSingularityWorkshop.FSM_COS;
using Xunit;

namespace FSM_COS.Tests;

public sealed class RuntimeManifestPlanningTests
{
    [Fact]
    public void Public_staged_plan_validation_bounds_enumeration_when_a_list_underreports_count()
    {
        var entries = Enumerable.Range(1, RuntimeManifestSchedule.DefaultMaximumEntryCount + 1)
            .Select(id => new RuntimeManifestEntry(Reference((ulong)id, "1.0.0")))
            .ToArray();
        var manifest = new RuntimeManifest(
            42,
            new[] { new MicroBundleManifestEntry(1, "1.0.0") },
            LoadPlan: new MisreportingReadOnlyList<RuntimeManifestEntry>(entries, reportedCount: 1));

        var exception = Assert.Throws<InvalidOperationException>(manifest.ValidateStagedPlan);

        Assert.Contains("Runtime manifest load plan exceeds the configured maximum of 10000 entries", exception.Message);
    }

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
    public void Staged_plan_validation_rejects_a_null_root_collection()
    {
        var manifest = new RuntimeManifest(
            42,
            null!,
            LoadPlan: new[] { new RuntimeManifestEntry(Reference(7, "1.2.0")) });

        Assert.Throws<ArgumentNullException>(manifest.ValidateStagedPlan);
    }

    [Fact]
    public void Staged_plan_rejects_version_drift_before_composition()
    {
        var entry = new RuntimeManifestEntry(Reference(7, "1.2.0"));
        var manifest = new RuntimeManifest(
            42,
            new[] { new MicroBundleManifestEntry(7, "2.0.0") },
            LoadPlan: new[] { entry });

        var exception = Assert.Throws<InvalidOperationException>(manifest.ValidateStagedPlan);
        Assert.Contains("MicroBundle 7", exception.Message);
        Assert.Contains("2.0.0", exception.Message);
        Assert.Contains("1.2.0", exception.Message);
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
    public void Schedule_rejects_entry_count_over_limit_before_snapshotting()
    {
        var entries = Enumerable.Range(1, RuntimeManifestSchedule.DefaultMaximumEntryCount + 1)
            .Select(id => new RuntimeManifestEntry(Reference((ulong)id, "1.0.0")))
            .ToArray();

        var exception = Assert.Throws<ArgumentException>(() =>
            new RuntimeManifestSchedule(entries, Array.Empty<RuntimeManifestDependency>()));

        Assert.Contains("maximum of 10000 entries", exception.Message);
    }

    [Fact]
    public void Schedule_rejects_dependency_count_over_limit_before_snapshotting()
    {
        var entries = new[]
        {
            new RuntimeManifestEntry(Reference(1, "1.0.0")),
            new RuntimeManifestEntry(Reference(2, "1.0.0"))
        };
        var dependencies = Enumerable.Repeat(
                new RuntimeManifestDependency(1, 2),
                RuntimeManifestSchedule.DefaultMaximumDependencyCount + 1)
            .ToArray();

        var exception = Assert.Throws<ArgumentException>(() =>
            new RuntimeManifestSchedule(entries, dependencies));

        Assert.Contains("maximum of 100000 entries", exception.Message);
    }

    [Fact]
    public void Load_plan_rejects_entry_count_over_limit_before_snapshotting()
    {
        var entries = Enumerable.Range(1, RuntimeManifestSchedule.DefaultMaximumEntryCount + 1)
            .Select(id => new RuntimeManifestEntry(Reference((ulong)id, "1.0.0")))
            .ToArray();

        var exception = Assert.Throws<ArgumentException>(() => new RuntimeManifestLoadPlan(entries));

        Assert.Contains("maximum of 10000 entries", exception.Message);
    }

    [Fact]
    public void Schedule_validates_a_deep_acyclic_graph_without_recursive_stack_growth()
    {
        const int bundleCount = 5000;
        var entries = Enumerable.Range(1, bundleCount)
            .Select(id => new RuntimeManifestEntry(Reference((ulong)id, "1.0.0")))
            .ToArray();
        var dependencies = Enumerable.Range(1, bundleCount - 1)
            .Select(id => new RuntimeManifestDependency((ulong)id, (ulong)id + 1))
            .ToArray();

        var schedule = new RuntimeManifestSchedule(entries, dependencies);

        Assert.Equal(bundleCount, schedule.Entries.Count);
        Assert.Equal(bundleCount - 1, schedule.Dependencies.Count);
        Assert.True(schedule.IsDependencyReady(1, _ => true));
    }

    [Fact]
    public void Schedule_rejects_dependency_cycles()
    {
        var first = new RuntimeManifestEntry(Reference(1, "1.0.0"));
        var second = new RuntimeManifestEntry(Reference(2, "1.0.0"));

        var exception = Assert.Throws<ArgumentException>(() => new RuntimeManifestSchedule(
            new[] { first, second },
            new[]
            {
                new RuntimeManifestDependency(1, 2),
                new RuntimeManifestDependency(2, 1)
            }));

        Assert.Contains("Manifest dependency cycle detected at MicroBundle", exception.Message);
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
        var invalidEntry = Assert.Throws<ArgumentException>(() =>
            new RuntimeManifestSchedule(new[] { invalid }, Array.Empty<RuntimeManifestDependency>()));
        Assert.Contains("index 0", invalidEntry.Message);
        Assert.Contains("MicroBundle 0", invalidEntry.Message);
        var invalidEdge = Assert.Throws<ArgumentException>(() =>
            new RuntimeManifestSchedule(new[] { valid }, new[] { new RuntimeManifestDependency(0, 1) }));
        Assert.Contains("index 0", invalidEdge.Message);
        Assert.Contains("MicroBundle 0 -> 1", invalidEdge.Message);
        var duplicateEntries = Assert.Throws<ArgumentException>(() =>
            new RuntimeManifestSchedule(new[] { valid, valid }, Array.Empty<RuntimeManifestDependency>()));
        Assert.Contains("duplicate MicroBundle ID 1", duplicateEntries.Message);
        var selfDependency = Assert.Throws<ArgumentException>(() =>
            new RuntimeManifestSchedule(new[] { valid }, new[] { new RuntimeManifestDependency(1, 1) }));
        Assert.Contains("1 -> 1", selfDependency.Message);
        var missingEntry = Assert.Throws<ArgumentException>(() =>
            new RuntimeManifestSchedule(new[] { valid }, new[] { new RuntimeManifestDependency(1, 2) }));
        Assert.Contains("1 -> 2", missingEntry.Message);
        Assert.Contains("absent from the manifest entries", missingEntry.Message);
        var duplicateEdges = Assert.Throws<ArgumentException>(() =>
            new RuntimeManifestSchedule(
                new[] { valid, new RuntimeManifestEntry(Reference(2, "1.0.0")) },
                new[] { new RuntimeManifestDependency(2, 1), new RuntimeManifestDependency(2, 1) }));
        Assert.Contains("duplicate", duplicateEdges.Message, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("2 -> 1", duplicateEdges.Message);
    }

    [Fact]
    public void Schedule_readiness_rejects_unknown_entries_and_null_predicates()
    {
        var schedule = new RuntimeManifestSchedule(
            new[] { new RuntimeManifestEntry(Reference(1, "1.0.0")) },
            Array.Empty<RuntimeManifestDependency>());

        Assert.Throws<ArgumentNullException>(() => schedule.IsDependencyReady(1, null!));
        var unknownEntry = Assert.Throws<KeyNotFoundException>(() => schedule.IsDependencyReady(99, _ => true));
        Assert.Contains("MicroBundle 99", unknownEntry.Message);
        Assert.Contains("not present in the schedule", unknownEntry.Message);
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
        var invalidEntry = Assert.Throws<ArgumentException>(() => new RuntimeManifestLoadPlan(new[]
        {
            new RuntimeManifestEntry(new MicroBundleReference(0, "", ""))
        }));
        Assert.Contains("index 0", invalidEntry.Message);
        Assert.Contains("MicroBundle 0", invalidEntry.Message);
        var duplicateEntries = Assert.Throws<ArgumentException>(() =>
            new RuntimeManifestLoadPlan(new[] { valid, valid }));
        Assert.Contains("duplicate MicroBundle ID 1", duplicateEntries.Message);
        var scheduleMismatch = Assert.Throws<ArgumentException>(() =>
            new RuntimeManifestLoadPlan(new[] { valid }, schedule));
        Assert.Contains("schedule", scheduleMismatch.Message, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("MicroBundle 1", scheduleMismatch.Message);
    }

    [Fact]
    public void Load_plan_guards_state_transitions_and_unknown_ids()
    {
        var plan = new RuntimeManifestLoadPlan(new[]
        {
            new RuntimeManifestEntry(Reference(1, "1.0.0"))
        });

        var unknownState = Assert.Throws<KeyNotFoundException>(() => plan.GetState(99));
        Assert.Contains("MicroBundle 99", unknownState.Message);
        Assert.Contains("load plan", unknownState.Message);
        var unknownLocalization = Assert.Throws<KeyNotFoundException>(() => plan.MarkLocalized(99));
        Assert.Contains("MicroBundle 99", unknownLocalization.Message);
        var unknownLoad = Assert.Throws<KeyNotFoundException>(() => plan.MarkLoaded(99));
        Assert.Contains("MicroBundle 99", unknownLoad.Message);
        var prematureLoad = Assert.Throws<InvalidOperationException>(() => plan.MarkLoaded(1));
        Assert.Contains("localized before it can be loaded", prematureLoad.Message);
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

        var missingRoot = Assert.Throws<InvalidOperationException>(manifestMissingRoot.ValidateStagedPlan);
        Assert.Contains("missing requested MicroBundle 1", missingRoot.Message);

        var resident = new RuntimeManifestEntry(Reference(1, "1.0.0"), ManifestLoadStage.Resident);
        var deferred = new RuntimeManifestEntry(Reference(1, "1.0.0"), ManifestLoadStage.Deferred);
        var manifestWithMismatch = new RuntimeManifest(
            42,
            new[] { new MicroBundleManifestEntry(1, "1.0.0") },
            LoadPlan: new[] { resident },
            Schedule: new RuntimeManifestSchedule(new[] { deferred }, Array.Empty<RuntimeManifestDependency>()));

        var mismatch = Assert.Throws<InvalidOperationException>(manifestWithMismatch.ValidateStagedPlan);
        Assert.Contains("disagree for MicroBundle 1", mismatch.Message);
    }

    [Fact]
    public void Manifest_plan_reports_invalid_staged_entries_with_index_and_identity()
    {
        var manifest = new RuntimeManifest(
            42,
            new[] { new MicroBundleManifestEntry(1, "1.0.0") },
            LoadPlan: new[]
            {
                new RuntimeManifestEntry(new MicroBundleReference(0, "", ""))
            });

        var exception = Assert.Throws<InvalidOperationException>(manifest.ValidateStagedPlan);

        Assert.Contains("index 0", exception.Message);
        Assert.Contains("MicroBundle 0", exception.Message);
    }

    [Fact]
    public void Manifest_plan_reports_duplicate_bundle_ids_as_a_staged_plan_error()
    {
        var entry = new RuntimeManifestEntry(Reference(1, "1.0.0"));
        var manifest = new RuntimeManifest(
            42,
            new[] { new MicroBundleManifestEntry(1, "1.0.0") },
            LoadPlan: new[] { entry, entry });

        var exception = Assert.Throws<InvalidOperationException>(manifest.ValidateStagedPlan);

        Assert.Contains("duplicate MicroBundle ID 1", exception.Message);
    }

    [Fact]
    public void Promotion_schedule_must_cover_the_entire_load_plan()
    {
        var root = new RuntimeManifestEntry(Reference(1, "1.0.0"));
        var dependent = new RuntimeManifestEntry(Reference(2, "1.0.0"), ManifestLoadStage.Deferred);
        var plan = new RuntimeManifestLoadPlan(new[] { root, dependent });
        var incompleteSchedule = new RuntimeManifestSchedule(
            new[] { dependent },
            Array.Empty<RuntimeManifestDependency>());

        plan.MarkLocalized(1);
        plan.MarkLocalized(2);

        var mismatch = Assert.Throws<ArgumentException>(() =>
            plan.EvaluatePromotions(new AcceptAllEvaluator(), null, incompleteSchedule).ToArray());
        Assert.Contains("MicroBundle 1", mismatch.Message);
        Assert.Contains("identity or stage", mismatch.Message);
    }

    [Fact]
    public void Manifest_plan_rejects_schedule_entries_absent_from_the_load_plan()
    {
        var root = new RuntimeManifestEntry(Reference(1, "1.0.0"));
        var extra = new RuntimeManifestEntry(Reference(2, "1.0.0"));
        var manifest = new RuntimeManifest(
            42,
            new[] { new MicroBundleManifestEntry(1, "1.0.0") },
            LoadPlan: new[] { root },
            Schedule: new RuntimeManifestSchedule(new[] { root, extra }, Array.Empty<RuntimeManifestDependency>()));

        var exception = Assert.Throws<InvalidOperationException>(manifest.ValidateStagedPlan);

        Assert.Contains("absent from the load plan", exception.Message);
        Assert.Contains("2", exception.Message);
    }

    [Fact]
    public void Load_plan_rejects_schedule_entries_not_represented_in_the_plan()
    {
        var root = new RuntimeManifestEntry(Reference(1, "1.0.0"));
        var extra = new RuntimeManifestEntry(Reference(2, "1.0.0"));
        var schedule = new RuntimeManifestSchedule(new[] { root, extra }, Array.Empty<RuntimeManifestDependency>());

        var exception = Assert.Throws<ArgumentException>(() =>
            new RuntimeManifestLoadPlan(new[] { root }, schedule));

        Assert.Contains("schedule", exception.Message, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("2", exception.Message);
    }

    [Fact]
    public void Promotion_evaluation_rejects_schedule_entries_not_represented_in_the_plan()
    {
        var root = new RuntimeManifestEntry(Reference(1, "1.0.0"));
        var deferred = new RuntimeManifestEntry(Reference(2, "1.0.0"), ManifestLoadStage.Deferred);
        var plan = new RuntimeManifestLoadPlan(new[] { root, deferred });
        var extra = new RuntimeManifestEntry(Reference(3, "1.0.0"));
        var schedule = new RuntimeManifestSchedule(new[] { root, deferred, extra }, Array.Empty<RuntimeManifestDependency>());

        plan.MarkLocalized(1);
        plan.MarkLocalized(2);

        var exception = Assert.Throws<ArgumentException>(() =>
            plan.EvaluatePromotions(new AcceptAllEvaluator(), null, schedule).ToArray());

        Assert.Contains("schedule", exception.Message, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("3", exception.Message);
    }

    [Fact]
    public void Load_plan_localization_is_idempotent_for_an_already_localized_entry()
    {
        var plan = new RuntimeManifestLoadPlan(new[]
        {
            new RuntimeManifestEntry(Reference(1, "1.0.0"))
        });

        plan.MarkLocalized(1);
        plan.MarkLocalized(1);

        Assert.Equal(MicroBundleRuntimeState.Localized, plan.GetState(1));
    }

    [Fact]
    public void Promotion_schedule_cannot_replace_validated_dependency_edges()
    {
        var root = new RuntimeManifestEntry(Reference(1, "1.0.0"));
        var dependent = new RuntimeManifestEntry(Reference(2, "1.0.0"), ManifestLoadStage.Deferred);
        var entries = new[] { root, dependent };
        var validatedSchedule = new RuntimeManifestSchedule(
            entries,
            new[] { new RuntimeManifestDependency(2, 1) });
        var plan = new RuntimeManifestLoadPlan(entries, validatedSchedule);
        var weakenedSchedule = new RuntimeManifestSchedule(entries, Array.Empty<RuntimeManifestDependency>());

        plan.MarkLocalized(1);
        plan.MarkLocalized(2);

        var mismatch = Assert.Throws<ArgumentException>(() =>
            plan.EvaluatePromotions(new AcceptAllEvaluator(), null, weakenedSchedule).ToArray());
        Assert.Contains("dependency edges", mismatch.Message);
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

    [Fact]
    public void Schedule_rejects_undefined_load_stage_with_entry_index_and_identity()
    {
        var invalid = new RuntimeManifestEntry(Reference(7, "1.0.0"), (ManifestLoadStage)999);

        var exception = Assert.Throws<ArgumentException>(() =>
            new RuntimeManifestSchedule(new[] { invalid }, Array.Empty<RuntimeManifestDependency>()));

        Assert.Contains("index 0", exception.Message);
        Assert.Contains("MicroBundle 7", exception.Message);
    }

    [Fact]
    public void Load_plan_rejects_undefined_load_stage_with_entry_index_and_identity()
    {
        var invalid = new RuntimeManifestEntry(Reference(7, "1.0.0"), (ManifestLoadStage)999);

        var exception = Assert.Throws<ArgumentException>(() =>
            new RuntimeManifestLoadPlan(new[] { invalid }));

        Assert.Contains("index 0", exception.Message);
        Assert.Contains("MicroBundle 7", exception.Message);
    }

    [Fact]
    public void Staged_manifest_rejects_undefined_load_stage_with_entry_index_and_identity()
    {
        var invalid = new RuntimeManifestEntry(Reference(7, "1.0.0"), (ManifestLoadStage)999);
        var manifest = new RuntimeManifest(
            42,
            new[] { new MicroBundleManifestEntry(7, "1.0.0") },
            LoadPlan: new[] { invalid });

        var exception = Assert.Throws<InvalidOperationException>(manifest.ValidateStagedPlan);

        Assert.Contains("index 0", exception.Message);
        Assert.Contains("MicroBundle 7", exception.Message);
    }

    [Fact]
    public void Dependency_readiness_propagates_predicate_failures()
    {
        var schedule = new RuntimeManifestSchedule(
            new[]
            {
                new RuntimeManifestEntry(Reference(1, "1.0.0")),
                new RuntimeManifestEntry(Reference(2, "1.0.0"))
            },
            new[] { new RuntimeManifestDependency(2, 1) });
        var expected = new InvalidOperationException("load-state lookup failed");

        var actual = Assert.Throws<InvalidOperationException>(() =>
            schedule.IsDependencyReady(2, _ => throw expected));

        Assert.Same(expected, actual);
    }

    [Fact]
    public void Promotion_evaluation_propagates_evaluator_failures()
    {
        var deferred = new RuntimeManifestEntry(Reference(2, "1.0.0"), ManifestLoadStage.Deferred);
        var plan = new RuntimeManifestLoadPlan(new[] { deferred });
        plan.MarkLocalized(2);
        var expected = new InvalidOperationException("experience evaluation failed");

        var actual = Assert.Throws<InvalidOperationException>(() =>
            plan.EvaluatePromotions(new ThrowingEvaluator(expected), null).ToArray());

        Assert.Same(expected, actual);
        Assert.Equal(MicroBundleRuntimeState.Localized, plan.GetState(2));
    }

    private static MicroBundleReference Reference(ulong id, string version) =>
        new(id, version, $"sha256:{id}-{version}");

    private sealed class MisreportingReadOnlyList<T> : IReadOnlyList<T>
    {
        private readonly T[] _items;
        private readonly int _reportedCount;

        public MisreportingReadOnlyList(IEnumerable<T> items, int reportedCount)
        {
            _items = items.ToArray();
            _reportedCount = reportedCount;
        }

        public int Count => _reportedCount;
        public T this[int index] => _items[index];
        public IEnumerator<T> GetEnumerator() => ((IEnumerable<T>)_items).GetEnumerator();
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
    }

    private sealed class AcceptAllEvaluator : IManifestLoadEvaluator
    {
        public bool ShouldLoad(RuntimeManifestEntry entry, IStateContext? experienceContext) => true;
    }

    private sealed class ThrowingEvaluator(Exception exception) : IManifestLoadEvaluator
    {
        public bool ShouldLoad(RuntimeManifestEntry entry, IStateContext? experienceContext) =>
            throw exception;
    }

    private sealed class DeclineDeferredEvaluator : IManifestLoadEvaluator
    {
        public bool ShouldLoad(RuntimeManifestEntry entry, IStateContext? experienceContext) => false;
    }

    [Fact]
    public void Schedule_and_load_plan_collections_are_read_only_after_validation()
    {
        var first = new RuntimeManifestEntry(Reference(1, "1.0.0"));
        var second = new RuntimeManifestEntry(Reference(2, "1.0.0"));
        var edge = new RuntimeManifestDependency(2, 1);
        var schedule = new RuntimeManifestSchedule(new[] { first, second }, new[] { edge });
        var plan = new RuntimeManifestLoadPlan(new[] { first, second }, schedule);

        Assert.Throws<NotSupportedException>(() =>
            ((IList<RuntimeManifestEntry>)schedule.Entries).Add(
                new RuntimeManifestEntry(Reference(3, "1.0.0"))));
        Assert.Throws<NotSupportedException>(() =>
            ((IList<RuntimeManifestDependency>)schedule.Dependencies).Add(
                new RuntimeManifestDependency(1, 2)));
        Assert.Throws<NotSupportedException>(() =>
            ((IList<RuntimeManifestEntry>)schedule.IndependentEntries)[0] = second);
        Assert.Throws<NotSupportedException>(() =>
            ((IList<RuntimeManifestEntry>)plan.Entries).Add(
                new RuntimeManifestEntry(Reference(3, "1.0.0"))));

        Assert.Equal(new ulong[] { 1, 2 }, schedule.Entries.Select(entry => entry.Reference.BundleId));
        Assert.Equal(new[] { edge }, schedule.Dependencies);
    }
}
