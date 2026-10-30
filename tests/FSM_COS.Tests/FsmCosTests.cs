using TheSingularityWorkshop.FSM_API;
using TheSingularityWorkshop.FSM_COS;
using TheSingularityWorkshop.FSM_UserIO;
using TheSingularityWorkshop.MicroBundleDomain;
using Xunit;

namespace FSM_COS.Tests;

public sealed class FsmCosTests
{
    [Fact]
    public void Constructor_rejects_nonpositive_composition_resource_limits()
    {
        var catalog = new TestCatalog();

        Assert.Throws<ArgumentOutOfRangeException>(() => new FsmCos(catalog, 10, 0, 1));
        Assert.Throws<ArgumentOutOfRangeException>(() => new FsmCos(catalog, 10, 1, 0));
        Assert.Throws<ArgumentOutOfRangeException>(() => new FsmCos(catalog, 10, 1, 1, -1));
    }

    [Fact]
    public void Execute_bounds_actual_root_enumeration_when_a_custom_list_underreports_count()
    {
        var first = new TestBundle(1);
        var second = new TestBundle(2);
        var roots = new MisreportingReadOnlyList<MicroBundleManifestEntry>(
            new[] { Entry(1), Entry(2) },
            reportedCount: 1);

        var exception = Assert.Throws<InvalidOperationException>(() =>
            new FsmCos(new TestCatalog(first, second), 10, 1, 10)
                .Execute(new RuntimeManifest(42, roots)));

        Assert.Contains("Runtime manifest root bundles exceeds the configured maximum of 1", exception.Message);
        Assert.Equal(0, first.LoadCalls);
        Assert.Equal(0, second.LoadCalls);
    }

    [Fact]
    public void Execute_rejects_root_count_over_limit_before_loading()
    {
        var first = new TestBundle(1);
        var second = new TestBundle(2);

        var exception = Assert.Throws<InvalidOperationException>(() =>
            new FsmCos(new TestCatalog(first, second), 10, 1, 10)
                .Execute(new RuntimeManifest(42, new[] { Entry(1), Entry(2) })));

        Assert.Contains("configured maximum is 1", exception.Message);
        Assert.Equal(0, first.LoadCalls);
        Assert.Equal(0, second.LoadCalls);
    }

    [Fact]
    public void Execute_rejects_load_plan_count_over_configured_bundle_limit_before_loading()
    {
        var root = new TestBundle(1);
        var plan = new[]
        {
            new RuntimeManifestEntry(new MicroBundleReference(1, "0.1.0-test", new string('1', 64))),
            new RuntimeManifestEntry(new MicroBundleReference(2, "0.1.0-test", new string('2', 64)))
        };

        var exception = Assert.Throws<InvalidOperationException>(() =>
            new FsmCos(new TestCatalog(root), 10, 1, 10).Execute(
                new RuntimeManifest(42, new[] { Entry(1) }, LoadPlan: plan)));

        Assert.Contains("Runtime manifest load plan contains 2 entries", exception.Message);
        Assert.Equal(0, root.LoadCalls);
    }

    [Fact]
    public void Execute_rejects_schedule_entry_count_over_configured_bundle_limit_before_loading()
    {
        var root = new TestBundle(1);
        var schedule = new RuntimeManifestSchedule(
            new[]
            {
                new RuntimeManifestEntry(new MicroBundleReference(1, "0.1.0-test", new string('1', 64))),
                new RuntimeManifestEntry(new MicroBundleReference(2, "0.1.0-test", new string('2', 64)))
            },
            Array.Empty<RuntimeManifestDependency>());

        var exception = Assert.Throws<InvalidOperationException>(() =>
            new FsmCos(new TestCatalog(root), 10, 1, 10).Execute(
                new RuntimeManifest(42, new[] { Entry(1) }, Schedule: schedule)));

        Assert.Contains("Runtime manifest schedule contains 2 entries", exception.Message);
        Assert.Equal(0, root.LoadCalls);
    }

    [Fact]
    public void Execute_rejects_schedule_dependency_count_over_configured_limit_before_loading()
    {
        var root = new TestBundle(1);
        var schedule = new RuntimeManifestSchedule(
            new[]
            {
                new RuntimeManifestEntry(new MicroBundleReference(1, "0.1.0-test", new string('1', 64))),
                new RuntimeManifestEntry(new MicroBundleReference(2, "0.1.0-test", new string('2', 64))),
                new RuntimeManifestEntry(new MicroBundleReference(3, "0.1.0-test", new string('3', 64)))
            },
            new[]
            {
                new RuntimeManifestDependency(2, 1),
                new RuntimeManifestDependency(3, 1)
            });

        var exception = Assert.Throws<InvalidOperationException>(() =>
            new FsmCos(new TestCatalog(root), 10, 10, 1).Execute(
                new RuntimeManifest(42, new[] { Entry(1) }, Schedule: schedule)));

        Assert.Contains("Runtime manifest schedule contains 2 dependency edges", exception.Message);
        Assert.Equal(0, root.LoadCalls);
    }

    [Fact]
    public void Execute_rejects_dependency_graph_over_limit_before_loading_any_bundle()
    {
        var dependency = new TestBundle(2);
        var root = new TestBundle(1, MicroBundleDependencyRequest.Unconfigured(2));

        var exception = Assert.Throws<InvalidOperationException>(() =>
            new FsmCos(new TestCatalog(root, dependency), 10, 1, 10)
                .Execute(new RuntimeManifest(42, new[] { Entry(1) })));

        Assert.Contains("configured maximum of 1 bundles", exception.Message);
        Assert.Equal(0, root.LoadCalls);
        Assert.Equal(0, dependency.LoadCalls);
    }

    [Fact]
    public void Execute_rejects_dependency_request_count_over_limit_before_resolving_or_loading()
    {
        var firstDependency = new TestBundle(2);
        var secondDependency = new TestBundle(3);
        var root = new TestBundle(
            1,
            MicroBundleDependencyRequest.Unconfigured(2),
            MicroBundleDependencyRequest.Unconfigured(3));

        var exception = Assert.Throws<InvalidOperationException>(() =>
            new FsmCos(new TestCatalog(root, firstDependency, secondDependency), 10, 10, 1)
                .Execute(new RuntimeManifest(42, new[] { Entry(1) })));

        Assert.Contains("configured maximum of 1 dependency requests", exception.Message);
        Assert.Equal(0, root.LoadCalls);
        Assert.Equal(0, firstDependency.LoadCalls);
        Assert.Equal(0, secondDependency.LoadCalls);
    }

    [Fact]
    public void Execute_rejects_a_root_collection_that_underreports_its_enumerated_size()
    {
        var first = new TestBundle(1);
        var second = new TestBundle(2);
        var catalog = new ThrowingCatalog(
            new TestCatalog(first, second),
            failingBundleId: 1,
            new InvalidOperationException("catalog must not be queried"));

        var exception = Assert.Throws<InvalidOperationException>(() =>
            new FsmCos(catalog, 10, 1, 10).Execute(
                new RuntimeManifest(42, new MisreportedRootList(Entry(1), Entry(2)))));

        Assert.Contains("root bundles", exception.Message, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("configured maximum of 1", exception.Message);
        Assert.Equal(0, first.LoadCalls);
        Assert.Equal(0, second.LoadCalls);
    }

    [Fact]
    public void Execute_rejects_dependency_request_collection_that_underreports_its_enumerated_size()
    {
        var root = new DependencyContractBundle(
            id: 1,
            descriptorDependencyIds: new[] { 2UL, 3UL },
            runtimeDependencies: new MisreportedDependencyList(
                MicroBundleDependencyRequest.Unconfigured(2),
                MicroBundleDependencyRequest.Unconfigured(3)));
        var first = new TestBundle(2);
        var second = new TestBundle(3);

        var exception = Assert.Throws<InvalidOperationException>(() =>
            new FsmCos(new TestCatalog(root, first, second), 10, 10, 1)
                .Execute(new RuntimeManifest(42, new[] { Entry(1) })));

        Assert.Contains("configured maximum of 1 dependency requests", exception.Message);
        Assert.Equal(0, root.LoadCalls);
        Assert.Equal(0, first.LoadCalls);
        Assert.Equal(0, second.LoadCalls);
    }

    [Fact]
    public void Execute_rejects_total_dependency_configuration_bytes_over_limit_before_loading()
    {
        var leaf = new TestBundle(3);
        var middle = new TestBundle(2, new MicroBundleDependencyRequest(3, new byte[] { 3, 4 }));
        var root = new TestBundle(1, new MicroBundleDependencyRequest(2, new byte[] { 1, 2 }));

        var exception = Assert.Throws<InvalidOperationException>(() =>
            new FsmCos(new TestCatalog(root, middle, leaf), 10, 10, 10, 3)
                .Execute(new RuntimeManifest(42, new[] { Entry(1) })));

        Assert.Contains("configured maximum of 3 bytes", exception.Message);
        Assert.Equal(0, root.LoadCalls);
        Assert.Equal(0, middle.LoadCalls);
        Assert.Equal(0, leaf.LoadCalls);
    }

    [Fact]
    public void Execute_accepts_dependency_configuration_at_exact_byte_limit()
    {
        var dependency = new TestBundle(2);
        var root = new TestBundle(1, new MicroBundleDependencyRequest(2, new byte[] { 1, 2, 3 }));

        var assembly = new FsmCos(new TestCatalog(root, dependency), 10, 10, 10, 3)
            .Execute(new RuntimeManifest(42, new[] { Entry(1) }));

        Assert.Equal(new ulong[] { 2, 1 }, assembly.Bundles.Select(bundle => bundle.Id));
    }

    [Fact]
    public void Execute_rejects_external_configuration_over_limit_before_loading_bundle()
    {
        var root = new TestBundle(1);
        var source = new TestConfigurationSource((1, "0.1.0-test", new byte[] { 1, 2, 3 }));

        var exception = Assert.Throws<InvalidOperationException>(() =>
            new FsmCos(new TestCatalog(root), 10, 10, 10, 2)
                .Execute(new RuntimeManifest(42, new[] { Entry(1) }), source));

        Assert.Contains("configuration provider", exception.Message);
        Assert.Contains("configured maximum of 2 bytes", exception.Message);
        Assert.Equal(0, root.LoadCalls);
    }

    [Fact]
    public void Execute_loads_dependencies_before_requesting_bundle()
    {
        var catalog = new TestCatalog(new TestBundle(2), new TestBundle(1, MicroBundleDependencyRequest.Unconfigured(2)));

        var assembly = new FsmCos(catalog).Execute(
            new RuntimeManifest(42, new[] { Entry(1) }));

        Assert.Equal(new ulong[] { 2, 1 }, assembly.Bundles.Select(x => x.Id));
    }

    [Fact]
    public void Execute_passes_configuration_to_requested_bundle()
    {
        var configuration = new byte[] { 7, 11, 13 };
        var bundle = new TestBundle(7);

        new FsmCos(new TestCatalog(bundle)).Execute(
            new RuntimeManifest(42, new[] { Entry(7) }), new TestConfigurationSource((7, "0.1.0-test", configuration)));

        Assert.Equal(configuration, bundle.Configuration.ToArray());
    }

    [Fact]
    public void Execute_copies_configuration_provider_memory_before_dependency_load_callbacks()
    {
        var configuration = new byte[] { 7, 11, 13 };
        var dependency = new TestBundle(2)
        {
            LoadAction = () => configuration[0] = 99
        };
        var root = new TestBundle(1, MicroBundleDependencyRequest.Unconfigured(2));

        new FsmCos(new TestCatalog(dependency, root)).Execute(
            new RuntimeManifest(42, new[] { Entry(1) }),
            new TestConfigurationSource((1, "0.1.0-test", configuration)));

        Assert.Equal(99, configuration[0]);
        Assert.Equal(new byte[] { 7, 11, 13 }, root.Configuration.ToArray());
    }

    [Fact]
    public void Execute_copies_dependency_configuration_before_catalog_callbacks_can_mutate_it()
    {
        var configuration = new byte[] { 7, 11, 13 };
        var dependency = new TestBundle(2);
        var root = new TestBundle(1, new MicroBundleDependencyRequest(2, configuration));
        var catalog = new MutatingDependencyCatalog(root, dependency, configuration);

        new FsmCos(catalog).Execute(new RuntimeManifest(42, new[] { Entry(1) }));

        Assert.Equal(99, configuration[0]);
        Assert.Equal(new byte[] { 7, 11, 13 }, dependency.Configuration.ToArray());
    }

    [Fact]
    public void Execute_propagates_configuration_source_failure_before_loading_bundle()
    {
        var bundle = new TestBundle(7);
        var expected = new InvalidOperationException("configuration source failed");

        var actual = Assert.Throws<InvalidOperationException>(() =>
            new FsmCos(new TestCatalog(bundle)).Execute(
                new RuntimeManifest(42, new[] { Entry(7) }),
                new ThrowingConfigurationSource(expected)));

        Assert.Same(expected, actual);
        Assert.Equal(0, bundle.LoadCalls);
        Assert.Equal(0, bundle.ArbitrationCalls);
    }

    [Fact]
    public void Execute_propagates_dependency_configuration_failure_before_loading_any_bundle()
    {
        var dependency = new TestBundle(2);
        var root = new TestBundle(1, MicroBundleDependencyRequest.Unconfigured(2));
        var expected = new InvalidOperationException("dependency configuration failed");

        var actual = Assert.Throws<InvalidOperationException>(() =>
            new FsmCos(new TestCatalog(dependency, root)).Execute(
                new RuntimeManifest(42, new[] { Entry(1) }),
                new SelectiveThrowingConfigurationSource(2, expected)));

        Assert.Same(expected, actual);
        Assert.Equal(0, dependency.LoadCalls);
        Assert.Equal(0, root.LoadCalls);
        Assert.Equal(0, dependency.ArbitrationCalls);
        Assert.Equal(0, root.ArbitrationCalls);
    }

    [Fact]
    public void Execute_propagates_bundle_load_failure_without_arbitrating_or_returning_assembly()
    {
        var expected = new InvalidOperationException("bundle load failed");
        var bundle = new TestBundle(7) { LoadException = expected };

        var actual = Assert.Throws<InvalidOperationException>(() =>
            new FsmCos(new TestCatalog(bundle)).Execute(
                new RuntimeManifest(42, new[] { Entry(7) })));

        Assert.Same(expected, actual);
        Assert.Equal(1, bundle.LoadCalls);
        Assert.Equal(0, bundle.ArbitrationCalls);
    }

    [Fact]
    public void Execute_compensates_load_attempts_in_reverse_order_when_a_dependent_load_fails()
    {
        var compensationOrder = new List<ulong>();
        var dependency = new TestBundle(2)
        {
            RollbackAction = () => compensationOrder.Add(2)
        };
        var expected = new InvalidOperationException("dependent load failed");
        var root = new TestBundle(1, new MicroBundleDependencyRequest(2, Array.Empty<byte>()))
        {
            LoadException = expected,
            RollbackAction = () => compensationOrder.Add(1)
        };

        var actual = Assert.Throws<InvalidOperationException>(() =>
            new FsmCos(new TestCatalog(dependency, root)).Execute(
                new RuntimeManifest(42, new[] { Entry(1) })));

        Assert.Same(expected, actual);
        Assert.Equal(1, dependency.LoadCalls);
        Assert.Equal(1, root.LoadCalls);
        Assert.Equal(new ulong[] { 1, 2 }, compensationOrder);
        Assert.Equal(1, dependency.RollbackCalls);
        Assert.Equal(1, root.RollbackCalls);
        Assert.Equal(0, dependency.ArbitrationCalls);
        Assert.Equal(0, root.ArbitrationCalls);
    }

    [Fact]
    public void Execute_preserves_composition_and_compensation_errors_when_rollback_fails()
    {
        var compositionFailure = new InvalidOperationException("dependent load failed");
        var compensationFailure = new InvalidOperationException("rollback failed");
        var root = new TestBundle(1)
        {
            LoadException = compositionFailure,
            RollbackException = compensationFailure
        };

        var aggregate = Assert.Throws<AggregateException>(() =>
            new FsmCos(new TestCatalog(root)).Execute(
                new RuntimeManifest(42, new[] { Entry(1) })));

        Assert.Same(compositionFailure, aggregate.InnerExceptions[0]);
        Assert.Same(compensationFailure, aggregate.InnerExceptions[1]);
    }

    [Fact]
    public void Execute_uses_defaults_when_configuration_is_absent()
    {
        var bundle = new TestBundle(9);

        new FsmCos(new TestCatalog(bundle)).Execute(
            new RuntimeManifest(42, new[] { Entry(9) }));

        Assert.Empty(bundle.Configuration.ToArray());
    }

    [Fact]
    public void Execute_rejects_a_manifest_version_that_the_catalog_does_not_supply()
    {
        var exception = Assert.Throws<InvalidOperationException>(() =>
            new FsmCos(new TestCatalog(new TestBundle(10)))
                .Execute(new RuntimeManifest(
                    42,
                    new[] { new MicroBundleManifestEntry(10, "9.9.9") })));

        Assert.Contains("10", exception.Message);
        Assert.Contains("9.9.9", exception.Message);
    }


    [Fact]
    public void Execute_rejects_default_root_manifest_entry_with_actionable_id_diagnostic()
    {
        var exception = Assert.Throws<InvalidOperationException>(() =>
            new FsmCos(new TestCatalog()).Execute(
                new RuntimeManifest(42, new[] { default(MicroBundleManifestEntry) })));

        Assert.Contains("root MicroBundle entry with ID 0", exception.Message);
    }

    [Fact]
    public void ManifestEntry_rejects_empty_versions()
    {
        Assert.Throws<ArgumentException>(() =>
            new MicroBundleManifestEntry(1, " "));
    }

    [Fact]
    public void External_configuration_overrides_dependency_request_configuration()
    {
        var dependency = new TestBundle(2);
        var root = new TestBundle(
            1,
            new MicroBundleDependencyRequest(2, new byte[] { 1, 2, 3 }));

        new FsmCos(new TestCatalog(dependency, root)).Execute(
            new RuntimeManifest(42, new[] { Entry(1) }),
            new TestConfigurationSource((2, "0.1.0-test", new byte[] { 9, 8, 7 })));

        Assert.Equal(new byte[] { 9, 8, 7 }, dependency.Configuration.ToArray());
    }

    [Fact]
    public void Execute_passes_dependency_configuration_before_loading_dependency()
    {
        var dependency = new TestBundle(2);
        var root = new TestBundle(1, new MicroBundleDependencyRequest(2, new byte[] { 5, 8, 13 }));

        new FsmCos(new TestCatalog(dependency, root)).Execute(
            new RuntimeManifest(42, new[] { Entry(1) }));

        Assert.Equal(new byte[] { 5, 8, 13 }, dependency.Configuration.ToArray());
    }

    [Fact]
    public void Execute_deduplicates_duplicate_root_requests()
    {
        var bundle = new TestBundle(1);

        var assembly = new FsmCos(new TestCatalog(bundle)).Execute(
            new RuntimeManifest(42, new[] { Entry(1), Entry(1) }));

        Assert.Single(assembly.Bundles);
        Assert.Empty(bundle.Configuration.ToArray());
        Assert.Equal(1, bundle.LoadCalls);
    }

    [Fact]
    public void Execute_rejects_conflicting_versions_for_the_same_root_bundle()
    {
        var bundle = new TestBundle(1);

        var exception = Assert.Throws<InvalidOperationException>(() =>
            new FsmCos(new TestCatalog(bundle)).Execute(
                new RuntimeManifest(42, new[]
                {
                    new MicroBundleManifestEntry(1, "0.1.0-test"),
                    new MicroBundleManifestEntry(1, "0.2.0-test")
                })));

        Assert.Contains("conflicting versions", exception.Message, StringComparison.OrdinalIgnoreCase);
        Assert.Equal(0, bundle.LoadCalls);
    }

    [Fact]
    public void Execute_uses_the_same_dependency_instance_validated_during_preflight()
    {
        var firstResolution = new TestBundle(2);
        var laterResolution = new TestBundle(2);
        var root = new TestBundle(1, MicroBundleDependencyRequest.Unconfigured(2));
        var catalog = new ChangingDependencyCatalog(root, firstResolution, laterResolution);

        var assembly = new FsmCos(catalog).Execute(
            new RuntimeManifest(42, new[] { Entry(1) }));

        Assert.Same(firstResolution, assembly.Bundles.Single(bundle => bundle.Id == 2));
        Assert.Equal(1, catalog.DependencyLookupCalls);
        Assert.Equal(1, firstResolution.LoadCalls);
        Assert.Equal(0, laterResolution.LoadCalls);
    }

    [Fact]
    public void Execute_deduplicates_shared_dependencies()
    {
        var shared = new TestBundle(3);
        var first = new TestBundle(1, MicroBundleDependencyRequest.Unconfigured(3));
        var second = new TestBundle(2, MicroBundleDependencyRequest.Unconfigured(3));

        var assembly = new FsmCos(new TestCatalog(shared, first, second)).Execute(
            new RuntimeManifest(42, new[]
            {
                Entry(1),
                Entry(2)
            }));

        Assert.Equal(new ulong[] { 3, 1, 2 }, assembly.Bundles.Select(x => x.Id));
        Assert.Equal(1, shared.LoadCalls);
    }

    [Fact]
    public void Execute_arbitrates_until_a_round_makes_no_changes()
    {
        var bundle = new TestBundle(1) { ChangesRemaining = 2 };

        var assembly = new FsmCos(new TestCatalog(bundle)).Execute(
            new RuntimeManifest(42, new[] { Entry(1) }));

        Assert.Equal(3, bundle.ArbitrationCalls);
        Assert.Equal(2, assembly.ArbitrationRounds);
    }

    [Fact]
    public void Execute_stops_after_the_first_stable_round()
    {
        var bundle = new TestBundle(1);

        var assembly = new FsmCos(new TestCatalog(bundle)).Execute(
            new RuntimeManifest(42, new[] { Entry(1) }));

        Assert.Equal(1, bundle.ArbitrationCalls);
        Assert.Equal(0, assembly.ArbitrationRounds);
    }

    [Fact]
    public void Execute_throws_when_a_requested_bundle_cannot_be_resolved()
    {
        var exception = Assert.Throws<InvalidOperationException>(() =>
            new FsmCos(new TestCatalog()).Execute(
                new RuntimeManifest(42, new[] { Entry(99) })));

        Assert.Contains("99", exception.Message);
        Assert.Contains("version", exception.Message, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("0.1.0-test", exception.Message);
    }

    [Fact]
    public void Execute_propagates_catalog_failure_while_resolving_a_requested_root()
    {
        var expected = new InvalidOperationException("catalog root lookup failed");
        var catalog = new ThrowingCatalog(
            new TestCatalog(new TestBundle(1)),
            failingBundleId: 1,
            expected);

        var actual = Assert.Throws<InvalidOperationException>(() =>
            new FsmCos(catalog).Execute(
                new RuntimeManifest(42, new[] { Entry(1) })));

        Assert.Same(expected, actual);
    }

    [Fact]
    public void Execute_propagates_catalog_failure_while_validating_a_scheduled_dependency_before_loading()
    {
        var expected = new InvalidOperationException("catalog dependency lookup failed");
        var root = new TestBundle(1, MicroBundleDependencyRequest.Unconfigured(2));
        var rootEntry = new RuntimeManifestEntry(
            new MicroBundleReference(1, "0.1.0-test", new string('a', 64)));
        var dependencyEntry = new RuntimeManifestEntry(
            new MicroBundleReference(2, "0.1.0-test", new string('b', 64)));
        var schedule = new RuntimeManifestSchedule(
            new[] { rootEntry, dependencyEntry },
            new[] { new RuntimeManifestDependency(1, 2) });
        var catalog = new ThrowingCatalog(
            new TestCatalog(root),
            failingBundleId: 2,
            expected);

        var actual = Assert.Throws<InvalidOperationException>(() =>
            new FsmCos(catalog).Execute(
                new RuntimeManifest(
                    42,
                    new[] { Entry(1) },
                    LoadPlan: new[] { rootEntry, dependencyEntry },
                    Schedule: schedule)));

        Assert.Same(expected, actual);
        Assert.Equal(0, root.LoadCalls);
    }

    [Fact]
    public void Execute_propagates_catalog_failure_while_resolving_an_unplanned_dependency_before_loading_root()
    {
        var expected = new InvalidOperationException("catalog dependency load lookup failed");
        var root = new TestBundle(1, MicroBundleDependencyRequest.Unconfigured(2));
        var catalog = new ThrowingCatalog(
            new TestCatalog(root),
            failingBundleId: 2,
            expected);

        var actual = Assert.Throws<InvalidOperationException>(() =>
            new FsmCos(catalog).Execute(
                new RuntimeManifest(42, new[] { Entry(1) })));

        Assert.Same(expected, actual);
        Assert.Equal(0, root.LoadCalls);
    }

    [Fact]
    public void Execute_resolves_the_entire_dependency_closure_before_loading_any_bundle()
    {
        var resolvedSibling = new TestBundle(2);
        var root = new TestBundle(
            1,
            MicroBundleDependencyRequest.Unconfigured(2),
            MicroBundleDependencyRequest.Unconfigured(3));

        var exception = Assert.Throws<InvalidOperationException>(() =>
            new FsmCos(new TestCatalog(root, resolvedSibling))
                .Execute(
                    new RuntimeManifest(42, new[] { Entry(1) }),
                    new ThrowingConfigurationSource(
                        new InvalidOperationException("configuration source must not be reached"))));

        Assert.Contains("3", exception.Message);
        Assert.Contains("could not be resolved", exception.Message, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("configuration source must not be reached", exception.Message);
        Assert.Equal(0, resolvedSibling.LoadCalls);
        Assert.Equal(0, root.LoadCalls);
    }

    [Fact]
    public void Execute_handles_a_deep_dependency_chain_without_recursive_stack_growth()
    {
        const int bundleCount = 2048;
        var bundles = Enumerable.Range(1, bundleCount)
            .Select(id => id == bundleCount
                ? new TestBundle((ulong)id)
                : new TestBundle((ulong)id, MicroBundleDependencyRequest.Unconfigured((ulong)id + 1)))
            .ToArray();

        var assembly = new FsmCos(new TestCatalog(bundles)).Execute(
            new RuntimeManifest(42, new[] { Entry(1) }));

        Assert.Equal(bundleCount, assembly.Bundles.Count);
        Assert.Equal((ulong)bundleCount, assembly.Bundles[0].Id);
        Assert.Equal(1UL, assembly.Bundles[^1].Id);
        Assert.All(bundles, bundle => Assert.Equal(1, bundle.LoadCalls));
    }

    [Fact]
    public void Execute_throws_when_dependencies_form_a_cycle()
    {
        var first = new TestBundle(1, MicroBundleDependencyRequest.Unconfigured(2));
        var second = new TestBundle(2, MicroBundleDependencyRequest.Unconfigured(1));

        var exception = Assert.Throws<InvalidOperationException>(() =>
            new FsmCos(new TestCatalog(first, second)).Execute(
                new RuntimeManifest(42, new[] { Entry(1) })));

        Assert.Contains("cycle", exception.Message, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("1", exception.Message);
        // Dependency cycles are detected before any bundle can perform load side effects.
        Assert.Equal(0, first.LoadCalls);
        Assert.Equal(0, second.LoadCalls);
    }

    [Fact]
    public void Execute_rejects_null_root_collection_before_catalog_lookup()
    {
        var expected = new InvalidOperationException("catalog must not be queried");
        var catalog = new ThrowingCatalog(new TestCatalog(), failingBundleId: 1, expected);

        Assert.Throws<ArgumentNullException>(() =>
            new FsmCos(catalog).Execute(new RuntimeManifest(42, null!)));
    }

    [Fact]
    public void Execute_rejects_catalog_success_with_a_null_root_before_loading()
    {
        var catalog = new NullRootCatalog();

        var exception = Assert.Throws<InvalidOperationException>(() =>
            new FsmCos(catalog).Execute(new RuntimeManifest(42, new[] { Entry(1) })));

        Assert.Contains("1", exception.Message);
        Assert.Contains("could not be resolved", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Execute_throws_when_arbitration_does_not_converge()
    {
        var bundle = new TestBundle(1) { ChangesRemaining = 100 };

        var exception = Assert.Throws<InvalidOperationException>(() =>
            new FsmCos(new TestCatalog(bundle), maximumArbitrationRounds: 2)
                .Execute(new RuntimeManifest(42, new[] { Entry(1) })));

        Assert.Contains("did not converge", exception.Message, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("2", exception.Message);
        Assert.Contains("rounds", exception.Message, StringComparison.OrdinalIgnoreCase);
        Assert.Equal(2, bundle.ArbitrationCalls);
    }

    [Fact]
    public void Execute_propagates_arbitration_failure_without_returning_an_assembly()
    {
        var expected = new InvalidOperationException("arbitration failed");
        var failing = new TestBundle(1) { ArbitrationException = expected };
        var later = new TestBundle(2);

        var actual = Assert.Throws<InvalidOperationException>(() =>
            new FsmCos(new TestCatalog(failing, later))
                .Execute(new RuntimeManifest(42, new[] { Entry(1), Entry(2) })));

        Assert.Same(expected, actual);
        Assert.Equal(1, failing.ArbitrationCalls);
        Assert.Equal(0, later.ArbitrationCalls);
    }

    [Fact]
    public void Execute_propagates_a_later_arbitration_failure_after_earlier_state_changes()
    {
        var first = new TestBundle(1) { ChangesRemaining = 1 };
        var expected = new InvalidOperationException("later arbitration failed");
        var later = new TestBundle(2) { ArbitrationException = expected };

        var actual = Assert.Throws<InvalidOperationException>(() =>
            new FsmCos(new TestCatalog(first, later)).Execute(
                new RuntimeManifest(42, new[] { Entry(1), Entry(2) })));

        Assert.Same(expected, actual);
        Assert.Equal(1, first.ArbitrationCalls);
        Assert.Equal(0, first.ChangesRemaining);
        Assert.Equal(1, later.ArbitrationCalls);
    }

    [Fact]
    public void Execute_rejects_zero_bundle_ids()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new FsmCos(new TestCatalog()).Execute(
                new RuntimeManifest(42, new[] { Entry(0) })));
    }

    [Fact]
    public void Execute_rejects_catalog_identity_mismatch()
    {
        var exception = Assert.Throws<InvalidOperationException>(() =>
            new FsmCos(new TestCatalog(new TestBundle(7)))
                .Execute(new RuntimeManifest(42, new[] { Entry(8) })));

        Assert.Contains("8", exception.Message);
    }

    [Fact]
    public void Execute_passes_runtime_identity_and_experience_context_to_arbitration()
    {
        var context = new TestStateContext();
        var bundle = new TestBundle(1);

        new FsmCos(new TestCatalog(bundle)).Execute(
            new RuntimeManifest(1234, new[] { Entry(1) }, context));

        Assert.Equal(1234UL, bundle.SeenRuntimeId);
        Assert.Same(context, bundle.SeenExperienceContext);
    }

    [Fact]
    public void Execute_snapshots_mutable_manifest_collections_before_bundle_callbacks()
    {
        var roots = new List<MicroBundleManifestEntry> { Entry(1), Entry(2) };
        var root = new TestBundle(1);
        var second = new TestBundle(2);
        root.LoadAction = () => roots.RemoveAt(1);

        var assembly = new FsmCos(new TestCatalog(root, second)).Execute(
            new RuntimeManifest(42, roots));

        Assert.Single(roots);
        Assert.Equal(new ulong[] { 1, 2 }, assembly.Bundles.Select(bundle => bundle.Id));
        Assert.Equal(1, second.LoadCalls);
    }

    [Fact]
    public void Execute_supports_an_empty_manifest()
    {
        var assembly = new FsmCos(new TestCatalog()).Execute(RuntimeManifest.Empty(77));

        Assert.Equal(77UL, assembly.RuntimeId);
        Assert.Empty(assembly.Bundles);
        Assert.Equal(0, assembly.ArbitrationRounds);
    }

    [Fact]
    public void Core_assembly_references_FSM_API_and_MicroBundleDomain()
    {
        var references = typeof(FsmCos).Assembly
            .GetReferencedAssemblies()
            .Select(x => x.Name)
            .ToHashSet(StringComparer.Ordinal);

        Assert.Contains(typeof(IStateContext).Assembly.GetName().Name!, references);
        Assert.Contains(typeof(MicroBundleDescriptor).Assembly.GetName().Name!, references);
        Assert.DoesNotContain("TheSingularityWorkshop.GUI.SVG", references);
    }

    [Fact]
    public void Execute_preserves_optional_semantic_intent_on_the_runtime_assembly()
    {
        var intent = new SemanticIntent("open-moniker", 123UL);

        var assembly = new FsmCos(new TestCatalog()).Execute(
            RuntimeManifest.Empty(88, intent: intent));

        Assert.Same(intent, assembly.Intent);
        Assert.Equal("open-moniker", assembly.Intent!.Name);
        Assert.Equal(123UL, assembly.Intent.ProtocolId);
    }

    [Fact]
    public void RuntimeManifest_empty_preserves_experience_context()
    {
        var context = new TestStateContext();

        var manifest = RuntimeManifest.Empty(88, context);

        Assert.Equal(88UL, manifest.RuntimeId);
        Assert.Empty(manifest.Bundles);
        Assert.Same(context, manifest.ExperienceContext);
    }

    [Fact]
    public void Execute_rejects_null_manifest()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new FsmCos(new TestCatalog()).Execute(null!));
    }

    [Fact]
    public void Constructor_rejects_invalid_arbitration_round_limit()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new FsmCos(new TestCatalog(), 0));
    }

    [Fact]
    public void Constructor_rejects_null_catalog()
    {
        Assert.Throws<ArgumentNullException>(() => new FsmCos(null!));
    }

    [Fact]
    public void RuntimeAssembly_can_find_loaded_bundle_by_id()
    {
        var bundle = new TestBundle(17);

        var assembly = new FsmCos(new TestCatalog(bundle)).Execute(
            new RuntimeManifest(42, new[] { Entry(17) }));

        Assert.True(assembly.TryGetBundle(17, out var resolved));
        Assert.Same(bundle, resolved);
        Assert.False(assembly.TryGetBundle(99, out _));
    }

    [Fact]
    public void RuntimeAssembly_can_find_host_known_bundle_type()
    {
        var bundle = new TestBundle(23);

        var assembly = new FsmCos(new TestCatalog(bundle)).Execute(
            new RuntimeManifest(42, new[] { Entry(23) }));

        Assert.True(assembly.TryGetBundle<TestBundle>(23, out var resolved));
        Assert.Same(bundle, resolved);
        Assert.True(assembly.TryGetBundle<TestBundle>(out var first));
        Assert.Same(bundle, first);
    }

    [Fact]
    public void RuntimeAssembly_exposes_loaded_bundle_alias_without_changing_composition()
    {
        var bundle = new TestBundle(31);

        var assembly = new FsmCos(new TestCatalog(bundle)).Execute(
            new RuntimeManifest(42, new[] { Entry(31) }));

        Assert.Same(assembly.Bundles, assembly.LoadedBundles);
        Assert.Single(assembly.LoadedBundles);
        Assert.Same(bundle, assembly.LoadedBundles[0]);
    }

    [Fact]
    public void Execute_accepts_a_schedule_that_matches_the_resolved_dependency_graph()
    {
        var dependency = new TestBundle(2);
        var root = new TestBundle(1, MicroBundleDependencyRequest.Unconfigured(2));
        var entries = new[]
        {
            new RuntimeManifestEntry(new MicroBundleReference(1, "0.1.0-test", new string('a', 64))),
            new RuntimeManifestEntry(new MicroBundleReference(2, "0.1.0-test", new string('b', 64)))
        };
        var schedule = new RuntimeManifestSchedule(
            entries,
            new[] { new RuntimeManifestDependency(1, 2) });

        var assembly = new FsmCos(new TestCatalog(dependency, root)).Execute(
            new RuntimeManifest(
                42,
                new[] { Entry(1) },
                LoadPlan: entries,
                Schedule: schedule));

        Assert.Equal(new ulong[] { 2, 1 }, assembly.Bundles.Select(bundle => bundle.Id));
        Assert.Equal(1, dependency.LoadCalls);
        Assert.Equal(1, root.LoadCalls);
    }

    [Fact]
    public void Execute_rejects_a_schedule_that_disagrees_with_the_resolved_graph_before_loading()
    {
        var dependency = new TestBundle(2);
        var root = new TestBundle(1, MicroBundleDependencyRequest.Unconfigured(2));
        var entries = new[]
        {
            new RuntimeManifestEntry(new MicroBundleReference(1, "0.1.0-test", new string('a', 64))),
            new RuntimeManifestEntry(new MicroBundleReference(2, "0.1.0-test", new string('b', 64)))
        };
        var schedule = new RuntimeManifestSchedule(entries, Array.Empty<RuntimeManifestDependency>());

        var exception = Assert.Throws<InvalidOperationException>(() =>
            new FsmCos(new TestCatalog(dependency, root)).Execute(
                new RuntimeManifest(
                    42,
                    new[] { Entry(1) },
                    LoadPlan: entries,
                    Schedule: schedule)));

        Assert.Contains("dependency edges do not match", exception.Message, StringComparison.OrdinalIgnoreCase);
        Assert.Equal(0, dependency.LoadCalls);
        Assert.Equal(0, root.LoadCalls);
    }

    [Fact]
    public void Explicit_root_version_wins_when_that_bundle_is_also_a_dependency()
    {
        var defaultDependency = new TestBundle(2, "1.0.0");
        var requestedDependency = new TestBundle(2, "2.0.0");
        var root = new TestBundle(1, MicroBundleDependencyRequest.Unconfigured(2));
        var catalog = new VersionedTestCatalog(
            new TheSingularityWorkshop.MicroBundleDomain.IMicroBundle[] { root, defaultDependency },
            new TheSingularityWorkshop.MicroBundleDomain.IMicroBundle[] { root, requestedDependency });

        var assembly = new FsmCos(catalog).Execute(
            new RuntimeManifest(
                42,
                new[]
                {
                    Entry(1),
                    new MicroBundleManifestEntry(2, "2.0.0")
                }));

        Assert.Same(requestedDependency, assembly.Bundles.Single(bundle => bundle.Id == 2));
        Assert.Equal(0, defaultDependency.LoadCalls);
        Assert.Equal(1, requestedDependency.LoadCalls);
    }

    [Fact]
    public void Schedule_validation_uses_an_explicit_root_version_when_it_is_also_a_dependency()
    {
        var defaultDependency = new TestBundle(2, "1.0.0");
        var requestedDependency = new TestBundle(2, "2.0.0");
        var root = new TestBundle(1, MicroBundleDependencyRequest.Unconfigured(2));
        var rootEntry = new RuntimeManifestEntry(
            new MicroBundleReference(1, "0.1.0-test", new string('a', 64)));
        var dependencyEntry = new RuntimeManifestEntry(
            new MicroBundleReference(2, "2.0.0", new string('c', 64)));
        var entries = new[] { rootEntry, dependencyEntry };
        var schedule = new RuntimeManifestSchedule(
            entries,
            new[] { new RuntimeManifestDependency(1, 2) });
        var catalog = new VersionedTestCatalog(
            new TheSingularityWorkshop.MicroBundleDomain.IMicroBundle[] { root, defaultDependency },
            new TheSingularityWorkshop.MicroBundleDomain.IMicroBundle[] { root, requestedDependency });

        var assembly = new FsmCos(catalog).Execute(
            new RuntimeManifest(
                42,
                new[] { Entry(1), new MicroBundleManifestEntry(2, "2.0.0") },
                LoadPlan: entries,
                Schedule: schedule));

        Assert.Same(requestedDependency, assembly.Bundles.Single(bundle => bundle.Id == 2));
        Assert.Equal(0, defaultDependency.LoadCalls);
        Assert.Equal(1, requestedDependency.LoadCalls);
    }

    [Fact]
    public void Execute_rejects_an_unresolvable_explicit_root_version_before_loading_dependencies()
    {
        var dependency = new TestBundle(2);
        var root = new TestBundle(1, MicroBundleDependencyRequest.Unconfigured(2));

        var exception = Assert.Throws<InvalidOperationException>(() =>
            new FsmCos(new TestCatalog(dependency, root)).Execute(
                new RuntimeManifest(
                    42,
                    new[]
                    {
                        Entry(1),
                        new MicroBundleManifestEntry(2, "9.9.9")
                    })));

        Assert.Contains("2", exception.Message);
        Assert.Contains("9.9.9", exception.Message);
        Assert.Equal(0, dependency.LoadCalls);
        Assert.Equal(0, root.LoadCalls);
    }

    [Fact]
    public void Execute_rejects_a_schedule_missing_a_resolved_dependency_before_loading()
    {
        var dependency = new TestBundle(2);
        var root = new TestBundle(1, MicroBundleDependencyRequest.Unconfigured(2));
        var rootEntry = new RuntimeManifestEntry(
            new MicroBundleReference(1, "0.1.0-test", new string('a', 64)));
        var schedule = new RuntimeManifestSchedule(
            new[] { rootEntry },
            Array.Empty<RuntimeManifestDependency>());

        var exception = Assert.Throws<InvalidOperationException>(() =>
            new FsmCos(new TestCatalog(dependency, root)).Execute(
                new RuntimeManifest(
                    42,
                    new[] { Entry(1) },
                    LoadPlan: new[] { rootEntry },
                    Schedule: schedule)));

        Assert.Contains("resolved dependency closure", exception.Message, StringComparison.OrdinalIgnoreCase);
        Assert.Equal(0, dependency.LoadCalls);
        Assert.Equal(0, root.LoadCalls);
    }

    [Fact]
    public void Execute_rejects_a_scheduled_dependency_version_mismatch_before_loading()
    {
        var dependency = new TestBundle(2);
        var root = new TestBundle(1, MicroBundleDependencyRequest.Unconfigured(2));
        var rootEntry = new RuntimeManifestEntry(
            new MicroBundleReference(1, "0.1.0-test", new string('a', 64)));
        var dependencyEntry = new RuntimeManifestEntry(
            new MicroBundleReference(2, "9.9.9", new string('b', 64)));
        var entries = new[] { rootEntry, dependencyEntry };
        var schedule = new RuntimeManifestSchedule(
            entries,
            new[] { new RuntimeManifestDependency(1, 2) });

        var exception = Assert.Throws<InvalidOperationException>(() =>
            new FsmCos(new TestCatalog(dependency, root)).Execute(
                new RuntimeManifest(
                    42,
                    new[] { Entry(1) },
                    LoadPlan: entries,
                    Schedule: schedule)));

        Assert.Contains("expects MicroBundle 2 version", exception.Message);
        Assert.Equal(0, dependency.LoadCalls);
        Assert.Equal(0, root.LoadCalls);
    }

    [Fact]
    public void Execute_rejects_a_catalog_that_returns_a_root_at_the_wrong_version()
    {
        var bundle = new TestBundle(1, "1.0.0");

        var exception = Assert.Throws<InvalidOperationException>(() =>
            new FsmCos(new FixedCatalog(bundle)).Execute(
                new RuntimeManifest(42, new[] { new MicroBundleManifestEntry(1, "2.0.0") })));

        Assert.Contains("resolved to version '1.0.0'", exception.Message);
        Assert.Equal(0, bundle.LoadCalls);
    }

    [Fact]
    public void Execute_rejects_a_catalog_that_returns_a_different_root_identity()
    {
        var bundle = new TestBundle(99);

        var exception = Assert.Throws<InvalidOperationException>(() =>
            new FsmCos(new FixedCatalog(bundle)).Execute(
                new RuntimeManifest(42, new[] { Entry(1) })));

        Assert.Contains("resolved request 1 to bundle 99", exception.Message);
        Assert.Equal(0, bundle.LoadCalls);
    }

    [Fact]
    public void Execute_rejects_descriptor_dependencies_missing_from_runtime_requests_before_loading()
    {
        var bundle = new DependencyContractBundle(
            id: 1,
            descriptorDependencyIds: new[] { 2UL },
            runtimeDependencies: Array.Empty<MicroBundleDependencyRequest>());

        var exception = Assert.Throws<InvalidOperationException>(() =>
            new FsmCos(new FixedCatalog(bundle)).Execute(
                new RuntimeManifest(42, new[] { Entry(1) })));

        Assert.Contains("dependency declarations do not match", exception.Message);
        Assert.Contains("2", exception.Message);
        Assert.Equal(0, bundle.LoadCalls);
    }

    [Fact]
    public void Execute_rejects_runtime_dependencies_missing_from_descriptor_before_loading()
    {
        var bundle = new DependencyContractBundle(
            id: 1,
            descriptorDependencyIds: Array.Empty<ulong>(),
            runtimeDependencies: new[] { MicroBundleDependencyRequest.Unconfigured(2) });

        var exception = Assert.Throws<InvalidOperationException>(() =>
            new FsmCos(new FixedCatalog(bundle)).Execute(
                new RuntimeManifest(42, new[] { Entry(1) })));

        Assert.Contains("dependency declarations do not match", exception.Message);
        Assert.Contains("2", exception.Message);
        Assert.Equal(0, bundle.LoadCalls);
    }

    [Fact]
    public void Execute_rejects_zero_dependency_ids_before_loading()
    {
        var bundle = new DependencyContractBundle(
            id: 1,
            descriptorDependencyIds: Array.Empty<ulong>(),
            runtimeDependencies: new[] { default(MicroBundleDependencyRequest) });

        var exception = Assert.Throws<InvalidOperationException>(() =>
            new FsmCos(new FixedCatalog(bundle)).Execute(
                new RuntimeManifest(42, new[] { Entry(1) })));

        Assert.Contains("dependency ID 0", exception.Message);
        Assert.Equal(0, bundle.LoadCalls);
    }

    [Fact]
    public void Execute_rejects_duplicate_runtime_dependency_requests_before_loading()
    {
        var bundle = new DependencyContractBundle(
            id: 1,
            descriptorDependencyIds: new[] { 2UL },
            runtimeDependencies: new[]
            {
                MicroBundleDependencyRequest.Unconfigured(2),
                MicroBundleDependencyRequest.Unconfigured(2)
            });

        var exception = Assert.Throws<InvalidOperationException>(() =>
            new FsmCos(new FixedCatalog(bundle)).Execute(
                new RuntimeManifest(42, new[] { Entry(1) })));

        Assert.Contains("more than once", exception.Message);
        Assert.Contains("2", exception.Message);
        Assert.Equal(0, bundle.LoadCalls);
    }

    [Fact]
    public void Execute_rejects_a_null_runtime_dependency_list_before_loading()
    {
        var bundle = new DependencyContractBundle(
            id: 1,
            descriptorDependencyIds: Array.Empty<ulong>(),
            runtimeDependencies: null);

        var exception = Assert.Throws<InvalidOperationException>(() =>
            new FsmCos(new FixedCatalog(bundle)).Execute(
                new RuntimeManifest(42, new[] { Entry(1) })));

        Assert.Contains("no dependency request list", exception.Message);
        Assert.Equal(0, bundle.LoadCalls);
    }

    [Fact]
    public void Execute_rejects_a_root_whose_descriptor_identity_disagrees_with_its_bundle_identity()
    {
        var bundle = new MismatchedDescriptorBundle(id: 1, descriptorId: 99);

        var exception = Assert.Throws<InvalidOperationException>(() =>
            new FsmCos(new FixedCatalog(bundle)).Execute(
                new RuntimeManifest(42, new[] { Entry(1) })));

        Assert.Contains("descriptor for bundle 99", exception.Message);
        Assert.Equal(0, bundle.LoadCalls);
    }

    [Fact]
    public void Execute_rejects_a_dependency_whose_descriptor_identity_disagrees_before_loading_root()
    {
        var dependency = new MismatchedDescriptorBundle(id: 2, descriptorId: 99);
        var root = new TestBundle(1, MicroBundleDependencyRequest.Unconfigured(2));

        var exception = Assert.Throws<InvalidOperationException>(() =>
            new FsmCos(new TestCatalog(root, dependency))
                .Execute(new RuntimeManifest(42, new[] { Entry(1) })));

        Assert.Contains("descriptor for bundle 99", exception.Message);
        Assert.Equal(0, root.LoadCalls);
        Assert.Equal(0, dependency.LoadCalls);
    }

    [Fact]
    public void Execute_rejects_a_root_without_a_descriptor_before_loading()
    {
        var bundle = new MalformedBundle(1);

        var exception = Assert.Throws<InvalidOperationException>(() =>
            new FsmCos(new FixedCatalog(bundle)).Execute(
                new RuntimeManifest(42, new[] { Entry(1) })));

        Assert.Contains("MicroBundle 1 returned no descriptor", exception.Message);
        Assert.Equal(0, bundle.LoadCalls);
    }

    [Fact]
    public void Execute_rejects_a_dependency_without_a_descriptor_before_loading()
    {
        var dependency = new MalformedBundle(2);
        var root = new TestBundle(1, MicroBundleDependencyRequest.Unconfigured(2));

        var exception = Assert.Throws<InvalidOperationException>(() =>
            new FsmCos(new TestCatalog(root, dependency)).Execute(
                new RuntimeManifest(42, new[] { Entry(1) })));

        Assert.Contains("MicroBundle 2 returned no descriptor", exception.Message);
        Assert.Equal(0, root.LoadCalls);
        Assert.Equal(0, dependency.LoadCalls);
    }

    [Fact]
    public void Schedule_validation_rejects_an_unresolvable_dependency_before_loading()
    {
        var root = new TestBundle(1, MicroBundleDependencyRequest.Unconfigured(2));
        var rootEntry = new RuntimeManifestEntry(
            new MicroBundleReference(1, "0.1.0-test", new string('a', 64)));
        var dependencyEntry = new RuntimeManifestEntry(
            new MicroBundleReference(2, "0.1.0-test", new string('d', 64)));
        var entries = new[] { rootEntry, dependencyEntry };
        var schedule = new RuntimeManifestSchedule(
            entries,
            new[] { new RuntimeManifestDependency(1, 2) });

        var exception = Assert.Throws<InvalidOperationException>(() =>
            new FsmCos(new TestCatalog(root)).Execute(
                new RuntimeManifest(
                    42,
                    new[] { Entry(1) },
                    LoadPlan: entries,
                    Schedule: schedule)));

        Assert.Contains("Scheduled MicroBundle 2 could not be resolved", exception.Message);
        Assert.Equal(0, root.LoadCalls);
    }

    [Fact]
    public void Schedule_validation_rejects_a_cycle_in_the_resolved_graph_before_loading()
    {
        var root = new TestBundle(1, MicroBundleDependencyRequest.Unconfigured(2));
        var dependency = new TestBundle(2, MicroBundleDependencyRequest.Unconfigured(1));
        var rootEntry = new RuntimeManifestEntry(
            new MicroBundleReference(1, "0.1.0-test", new string('a', 64)));
        var dependencyEntry = new RuntimeManifestEntry(
            new MicroBundleReference(2, "0.1.0-test", new string('b', 64)));
        var entries = new[] { rootEntry, dependencyEntry };
        // The published schedule is acyclic, but the domain-owned declarations are not.
        var schedule = new RuntimeManifestSchedule(
            entries,
            new[] { new RuntimeManifestDependency(1, 2) });

        var exception = Assert.Throws<InvalidOperationException>(() =>
            new FsmCos(new TestCatalog(root, dependency)).Execute(
                new RuntimeManifest(
                    42,
                    new[] { Entry(1) },
                    LoadPlan: entries,
                    Schedule: schedule)));

        Assert.Contains("dependency cycle detected at 1", exception.Message);
        Assert.Equal(0, root.LoadCalls);
        Assert.Equal(0, dependency.LoadCalls);
    }

    [Fact]
    public void Schedule_validation_rejects_a_dependency_without_a_descriptor_before_loading()
    {
        var root = new TestBundle(1, MicroBundleDependencyRequest.Unconfigured(2));
        var dependency = new MalformedBundle(2);
        var rootEntry = new RuntimeManifestEntry(
            new MicroBundleReference(1, "0.1.0-test", new string('a', 64)));
        var dependencyEntry = new RuntimeManifestEntry(
            new MicroBundleReference(2, "0.1.0-test", new string('b', 64)));
        var entries = new[] { rootEntry, dependencyEntry };
        var schedule = new RuntimeManifestSchedule(
            entries,
            new[] { new RuntimeManifestDependency(1, 2) });

        var exception = Assert.Throws<InvalidOperationException>(() =>
            new FsmCos(new TestCatalog(root, dependency)).Execute(
                new RuntimeManifest(
                    42,
                    new[] { Entry(1) },
                    LoadPlan: entries,
                    Schedule: schedule)));

        Assert.Contains("returned no descriptor", exception.Message);
        Assert.Equal(0, root.LoadCalls);
        Assert.Equal(0, dependency.LoadCalls);
    }

    [Fact]
    public void Arbitration_accepts_matching_domain_contracts_not_just_matching_provider_labels()
    {
        var tanker = new DockingContractBundle(31, "docking", "shared-capture-envelope-v1");
        var station = new DockingContractBundle(32, "docking", "shared-capture-envelope-v1");

        var assembly = new FsmCos(new TestCatalog(tanker, station)).Execute(
            new RuntimeManifest(42, new[] { Entry(31), Entry(32) }));

        Assert.Equal(new ulong[] { 31, 32 }, assembly.Bundles.Select(bundle => bundle.Id));
        Assert.Equal(1, tanker.ArbitrationCalls);
        Assert.Equal(1, station.ArbitrationCalls);
    }

    [Fact]
    public void Arbitration_rejects_shared_provider_label_when_domain_contracts_conflict()
    {
        var tanker = new DockingContractBundle(31, "docking", "tanker-capture-envelope-v1");
        var station = new DockingContractBundle(32, "docking", "station-capture-envelope-v2");

        var exception = Assert.Throws<InvalidOperationException>(() =>
            new FsmCos(new TestCatalog(tanker, station)).Execute(
                new RuntimeManifest(42, new[] { Entry(31), Entry(32) })));

        Assert.Contains("docking", exception.Message, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("contract", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    private static MicroBundleManifestEntry Entry(ulong bundleId) =>
        new(bundleId, "0.1.0-test");

    private sealed class ThrowingCatalog : IMicroBundleCatalog
    {
        private readonly IMicroBundleCatalog _inner;
        private readonly ulong _failingBundleId;
        private readonly Exception _exception;

        public ThrowingCatalog(
            IMicroBundleCatalog inner,
            ulong failingBundleId,
            Exception exception)
        {
            _inner = inner;
            _failingBundleId = failingBundleId;
            _exception = exception;
        }

        public bool TryResolve(ulong bundleId, string version, out IMicroBundle? bundle)
        {
            if (bundleId == _failingBundleId)
                throw _exception;

            return _inner.TryResolve(bundleId, version, out bundle);
        }

        public bool TryResolve(ulong bundleId, out IMicroBundle? bundle)
        {
            if (bundleId == _failingBundleId)
                throw _exception;

            return _inner.TryResolve(bundleId, out bundle);
        }
    }

    private sealed class MisreportedDependencyList : IReadOnlyList<MicroBundleDependencyRequest>
    {
        private readonly MicroBundleDependencyRequest[] _entries;

        public MisreportedDependencyList(params MicroBundleDependencyRequest[] entries) => _entries = entries;

        public int Count => Math.Min(1, _entries.Length);
        public MicroBundleDependencyRequest this[int index] => _entries[index];

        public IEnumerator<MicroBundleDependencyRequest> GetEnumerator() =>
            ((IEnumerable<MicroBundleDependencyRequest>)_entries).GetEnumerator();

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
    }

    private sealed class MisreportedRootList : IReadOnlyList<MicroBundleManifestEntry>
    {
        private readonly MicroBundleManifestEntry[] _entries;

        public MisreportedRootList(params MicroBundleManifestEntry[] entries) => _entries = entries;

        public int Count => Math.Min(1, _entries.Length);
        public MicroBundleManifestEntry this[int index] => _entries[index];

        public IEnumerator<MicroBundleManifestEntry> GetEnumerator() =>
            ((IEnumerable<MicroBundleManifestEntry>)_entries).GetEnumerator();

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
    }

    private sealed class TestConfigurationSource : IMicroBundleConfigurationSource
    {
        private readonly Dictionary<(ulong BundleId, string Version), ReadOnlyMemory<byte>> _configuration;

        public TestConfigurationSource(params (ulong BundleId, string Version, byte[] Configuration)[] entries) =>
            _configuration = entries.ToDictionary(
                x => (x.BundleId, x.Version),
                x => (ReadOnlyMemory<byte>)x.Configuration);

        public bool TryGetConfiguration(
            ulong runtimeId,
            ulong bundleId,
            string version,
            out ReadOnlyMemory<byte> configuration) =>
            _configuration.TryGetValue((bundleId, version), out configuration);
    }

    private sealed class ThrowingConfigurationSource : IMicroBundleConfigurationSource
    {
        private readonly Exception _exception;

        public ThrowingConfigurationSource(Exception exception) =>
            _exception = exception ?? throw new ArgumentNullException(nameof(exception));

        public bool TryGetConfiguration(
            ulong runtimeId,
            ulong bundleId,
            string version,
            out ReadOnlyMemory<byte> configuration)
        {
            configuration = ReadOnlyMemory<byte>.Empty;
            throw _exception;
        }
    }

    private sealed class SelectiveThrowingConfigurationSource : IMicroBundleConfigurationSource
    {
        private readonly ulong _failingBundleId;
        private readonly Exception _exception;

        public SelectiveThrowingConfigurationSource(ulong failingBundleId, Exception exception)
        {
            _failingBundleId = failingBundleId;
            _exception = exception ?? throw new ArgumentNullException(nameof(exception));
        }

        public bool TryGetConfiguration(
            ulong runtimeId,
            ulong bundleId,
            string version,
            out ReadOnlyMemory<byte> configuration)
        {
            configuration = ReadOnlyMemory<byte>.Empty;
            if (bundleId == _failingBundleId)
                throw _exception;

            return false;
        }
    }

    private sealed class ChangingDependencyCatalog : IMicroBundleCatalog
    {
        private readonly IMicroBundle _root;
        private readonly IMicroBundle _firstResolution;
        private readonly IMicroBundle _laterResolution;

        public ChangingDependencyCatalog(
            IMicroBundle root,
            IMicroBundle firstResolution,
            IMicroBundle laterResolution)
        {
            _root = root;
            _firstResolution = firstResolution;
            _laterResolution = laterResolution;
        }

        public int DependencyLookupCalls { get; private set; }

        public bool TryResolve(ulong bundleId, string version, out IMicroBundle? bundle)
        {
            bundle = bundleId == _root.Id ? _root : null;
            return bundle is not null &&
                string.Equals(bundle.Descriptor.Version, version, StringComparison.Ordinal);
        }

        public bool TryResolve(ulong bundleId, out IMicroBundle? bundle)
        {
            if (bundleId == _root.Id)
            {
                bundle = _root;
                return true;
            }

            if (bundleId == _firstResolution.Id)
            {
                DependencyLookupCalls++;
                bundle = DependencyLookupCalls == 1 ? _firstResolution : _laterResolution;
                return true;
            }

            bundle = null;
            return false;
        }
    }

    private sealed class MutatingDependencyCatalog : IMicroBundleCatalog
    {
        private readonly IMicroBundle _root;
        private readonly IMicroBundle _dependency;
        private readonly byte[] _configuration;

        public MutatingDependencyCatalog(IMicroBundle root, IMicroBundle dependency, byte[] configuration)
        {
            _root = root;
            _dependency = dependency;
            _configuration = configuration;
        }

        public bool TryResolve(ulong bundleId, string version, out IMicroBundle? bundle)
        {
            bundle = bundleId == _root.Id ? _root :
                bundleId == _dependency.Id ? _dependency : null;
            return bundle is not null &&
                string.Equals(bundle.Descriptor.Version, version, StringComparison.Ordinal);
        }

        public bool TryResolve(ulong bundleId, out IMicroBundle? bundle)
        {
            if (bundleId == _dependency.Id)
            {
                _configuration[0] = 99;
                bundle = _dependency;
                return true;
            }

            bundle = bundleId == _root.Id ? _root : null;
            return bundle is not null;
        }
    }

    private sealed class TestCatalog : IMicroBundleCatalog
    {
        private readonly Dictionary<ulong, TheSingularityWorkshop.MicroBundleDomain.IMicroBundle> _bundles;

        public TestCatalog(params TheSingularityWorkshop.MicroBundleDomain.IMicroBundle[] bundles) =>
            _bundles = bundles.ToDictionary(x => x.Id);

        public bool TryResolve(ulong bundleId, string version, out TheSingularityWorkshop.MicroBundleDomain.IMicroBundle? bundle) =>
            _bundles.TryGetValue(bundleId, out bundle) &&
            string.Equals(bundle.Descriptor.Version, version, StringComparison.Ordinal);

        public bool TryResolve(ulong bundleId, out TheSingularityWorkshop.MicroBundleDomain.IMicroBundle? bundle) =>
            _bundles.TryGetValue(bundleId, out bundle);
    }

    private sealed class DependencyOverrideCatalog : IMicroBundleCatalog
    {
        private readonly IMicroBundle _root;
        private readonly ulong _requestedDependencyId;
        private readonly IMicroBundle _dependency;

        public DependencyOverrideCatalog(IMicroBundle root, ulong requestedDependencyId, IMicroBundle dependency)
        {
            _root = root;
            _requestedDependencyId = requestedDependencyId;
            _dependency = dependency;
        }

        public bool TryResolve(ulong bundleId, string version, out IMicroBundle? bundle)
        {
            bundle = bundleId == _root.Id ? _root :
                bundleId == _requestedDependencyId ? _dependency : null;
            return bundle is not null &&
                string.Equals(bundle.Descriptor.Version, version, StringComparison.Ordinal);
        }

        public bool TryResolve(ulong bundleId, out IMicroBundle? bundle)
        {
            bundle = bundleId == _root.Id ? _root :
                bundleId == _requestedDependencyId ? _dependency : null;
            return bundle is not null;
        }
    }

    private sealed class NullRootCatalog : IMicroBundleCatalog
    {
        public bool TryResolve(ulong bundleId, string version, out IMicroBundle? bundle)
        {
            bundle = null;
            return bundleId == 1;
        }

        public bool TryResolve(ulong bundleId, out IMicroBundle? bundle)
        {
            bundle = null;
            return bundleId == 1;
        }
    }

    private sealed class NullDependencyCatalog : IMicroBundleCatalog
    {
        private readonly IMicroBundle _root;
        private readonly ulong _requestedDependencyId;

        public NullDependencyCatalog(IMicroBundle root, ulong requestedDependencyId)
        {
            _root = root;
            _requestedDependencyId = requestedDependencyId;
        }

        public bool TryResolve(ulong bundleId, string version, out IMicroBundle? bundle)
        {
            bundle = bundleId == _root.Id ? _root : null;
            return bundle is not null &&
                string.Equals(bundle.Descriptor.Version, version, StringComparison.Ordinal);
        }

        public bool TryResolve(ulong bundleId, out IMicroBundle? bundle)
        {
            if (bundleId == _requestedDependencyId)
            {
                bundle = null;
                return true;
            }

            bundle = bundleId == _root.Id ? _root : null;
            return bundle is not null;
        }
    }

    private sealed class FixedCatalog : IMicroBundleCatalog
    {
        private readonly IMicroBundle _bundle;

        public FixedCatalog(IMicroBundle bundle) => _bundle = bundle;

        public bool TryResolve(ulong bundleId, string version, out IMicroBundle? bundle)
        {
            bundle = _bundle;
            return true;
        }

        public bool TryResolve(ulong bundleId, out IMicroBundle? bundle)
        {
            bundle = _bundle;
            return true;
        }
    }

    private sealed class DependencyContractBundle : IMicroBundle
    {
        private readonly IReadOnlyList<MicroBundleDependencyRequest>? _runtimeDependencies;

        public DependencyContractBundle(
            ulong id,
            IEnumerable<ulong> descriptorDependencyIds,
            IReadOnlyList<MicroBundleDependencyRequest>? runtimeDependencies)
        {
            Id = id;
            _runtimeDependencies = runtimeDependencies;
            Descriptor = new MicroBundleDescriptor(
                id,
                "0.1.0-test",
                descriptorDependencyIds.Select(dependencyId => new MicroBundleDependency(dependencyId)));
        }

        public ulong Id { get; }
        public MicroBundleDescriptor Descriptor { get; }
        public IReadOnlyList<MicroBundleDependencyRequest> Dependencies => _runtimeDependencies!;
        public int LoadCalls { get; private set; }

        public void Load(IMicroBundleLoadContext context) => LoadCalls++;
        public bool Arbitrate(IMicroBundleArbitrationContext context, int roundIndex) => false;
    }

    private sealed class MismatchedDescriptorBundle : IMicroBundle
    {
        public MismatchedDescriptorBundle(
            ulong id,
            ulong descriptorId,
            params MicroBundleDependencyRequest[] dependencies)
        {
            Id = id;
            Dependencies = dependencies;
            Descriptor = new MicroBundleDescriptor(
                descriptorId,
                "0.1.0-test",
                dependencies.Select(dependency => new MicroBundleDependency(dependency.BundleId)));
        }

        public ulong Id { get; }
        public MicroBundleDescriptor Descriptor { get; }
        public IReadOnlyList<MicroBundleDependencyRequest> Dependencies { get; }
        public int LoadCalls { get; private set; }

        public void Load(IMicroBundleLoadContext context) => LoadCalls++;
        public bool Arbitrate(IMicroBundleArbitrationContext context, int roundIndex) => false;
    }

    private sealed class MalformedBundle : IMicroBundle
    {
        public MalformedBundle(ulong id) => Id = id;

        public ulong Id { get; }
        public MicroBundleDescriptor Descriptor => null!;
        public IReadOnlyList<MicroBundleDependencyRequest> Dependencies =>
            Array.Empty<MicroBundleDependencyRequest>();
        public int LoadCalls { get; private set; }

        public void Load(IMicroBundleLoadContext context) => LoadCalls++;
        public bool Arbitrate(IMicroBundleArbitrationContext context, int roundIndex) => false;
    }

    private sealed class VersionedTestCatalog : IMicroBundleCatalog
    {
        private readonly Dictionary<ulong, TheSingularityWorkshop.MicroBundleDomain.IMicroBundle> _byId;
        private readonly Dictionary<(ulong BundleId, string Version), TheSingularityWorkshop.MicroBundleDomain.IMicroBundle> _byVersion;

        public VersionedTestCatalog(
            TheSingularityWorkshop.MicroBundleDomain.IMicroBundle[] byId,
            TheSingularityWorkshop.MicroBundleDomain.IMicroBundle[] byVersion)
        {
            _byId = byId.ToDictionary(bundle => bundle.Id);
            _byVersion = byVersion.ToDictionary(
                bundle => (bundle.Id, bundle.Descriptor.Version));
        }

        public bool TryResolve(ulong bundleId, string version, out TheSingularityWorkshop.MicroBundleDomain.IMicroBundle? bundle) =>
            _byVersion.TryGetValue((bundleId, version), out bundle);

        public bool TryResolve(ulong bundleId, out TheSingularityWorkshop.MicroBundleDomain.IMicroBundle? bundle) =>
            _byId.TryGetValue(bundleId, out bundle);
    }

    /// <summary>
    /// Test-owned domain contract. The shared provider label is discovery metadata;
    /// this fixture's explicit contract identity is what determines compatibility.
    /// </summary>
    private sealed class DockingContractBundle : TheSingularityWorkshop.MicroBundleDomain.IMicroBundle
    {
        public DockingContractBundle(ulong id, string providerLabel, string contractId)
        {
            Id = id;
            ContractId = contractId;
            // The runtime manifest pins the fixture package version; the separate
            // contract identity below models domain compatibility, not package version.
            Descriptor = new MicroBundleDescriptor(
                id,
                "0.1.0-test",
                providers: new[] { new MicroBundleProvider(providerLabel) });
        }

        public ulong Id { get; }
        public string ContractId { get; }
        public MicroBundleDescriptor Descriptor { get; }
        public IReadOnlyList<MicroBundleDependencyRequest> Dependencies =>
            Array.Empty<MicroBundleDependencyRequest>();
        public int ArbitrationCalls { get; private set; }

        public void Load(IMicroBundleLoadContext context) { }


        public bool Arbitrate(IMicroBundleArbitrationContext context, int roundIndex)
        {
            ArbitrationCalls++;
            foreach (var peer in context.Bundles.OfType<DockingContractBundle>())
            {
                if (peer.Id != Id && !StringComparer.Ordinal.Equals(ContractId, peer.ContractId))
                    throw new InvalidOperationException(
                        $"Incompatible docking contract between MicroBundles {Id} and {peer.Id}: " +
                        $"'{ContractId}' versus '{peer.ContractId}'.");
            }

            return false;
        }
    }

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

    private sealed class TestBundle : TheSingularityWorkshop.MicroBundleDomain.IMicroBundle, IMicroBundleRollback
    {
        private readonly IReadOnlyList<MicroBundleDependencyRequest> _dependencies;

        public TestBundle(ulong id, params MicroBundleDependencyRequest[] dependencies)
            : this(id, "0.1.0-test", dependencies)
        {
        }

        public TestBundle(ulong id, string version, params MicroBundleDependencyRequest[] dependencies)
        {
            Id = id;
            _dependencies = dependencies;
            Descriptor = new MicroBundleDescriptor(
                id,
                version,
                dependencies.Select(x => new MicroBundleDependency(x.BundleId)));
        }

        public ulong Id { get; }
        public MicroBundleDescriptor Descriptor { get; }
        public IReadOnlyList<MicroBundleDependencyRequest> Dependencies => _dependencies;
        public ReadOnlyMemory<byte> Configuration { get; private set; }
        public int LoadCalls { get; private set; }
        public Exception? LoadException { get; set; }
        public Action? LoadAction { get; set; }
        public Action? RollbackAction { get; set; }
        public Exception? RollbackException { get; set; }
        public int RollbackCalls { get; private set; }
        public Exception? ArbitrationException { get; set; }
        public int ChangesRemaining { get; set; }
        public int ArbitrationCalls { get; private set; }
        public ulong SeenRuntimeId { get; private set; }
        public object? SeenExperienceContext { get; private set; }

        public void Load(IMicroBundleLoadContext context)
        {
            LoadCalls++;
            if (LoadException is not null)
                throw LoadException;

            Configuration = context.TryGetConfiguration(Id, out var value)
                ? value
                : ReadOnlyMemory<byte>.Empty;
            LoadAction?.Invoke();
        }

        public void RollbackLoad(IMicroBundleLoadContext context)
        {
            RollbackCalls++;
            RollbackAction?.Invoke();
            if (RollbackException is not null)
                throw RollbackException;
        }

        public bool Arbitrate(IMicroBundleArbitrationContext context, int roundIndex)
        {
            ArbitrationCalls++;
            if (ArbitrationException is not null)
                throw ArbitrationException;

            SeenRuntimeId = context.RuntimeId;
            SeenExperienceContext = context.ExperienceContext;

            if (ChangesRemaining <= 0) return false;
            ChangesRemaining--;
            return true;
        }
    }

    private sealed class TestStateContext : IStateContext
    {
        public string Name { get; set; } = "test";
        public bool IsValid { get; set; } = true;
    }

    [Fact]
    public void Execute_rejects_a_dependency_catalog_entry_without_a_descriptor_before_loading_root()
    {
        var root = new TestBundle(1, MicroBundleDependencyRequest.Unconfigured(2));
        var malformedDependency = new MalformedBundle(2);

        var exception = Assert.Throws<InvalidOperationException>(() =>
            new FsmCos(new TestCatalog(root, malformedDependency))
                .Execute(new RuntimeManifest(42, new[] { Entry(1) })));

        Assert.Contains("2", exception.Message);
        Assert.Contains("no descriptor", exception.Message, StringComparison.OrdinalIgnoreCase);
        Assert.Equal(0, root.LoadCalls);
        Assert.Equal(0, malformedDependency.LoadCalls);
    }

    [Fact]
    public void Execute_rejects_a_dependency_catalog_identity_mismatch_before_loading_root()
    {
        var root = new TestBundle(1, MicroBundleDependencyRequest.Unconfigured(2));
        var wrongDependency = new TestBundle(3);
        var catalog = new DependencyOverrideCatalog(root, requestedDependencyId: 2, wrongDependency);

        var exception = Assert.Throws<InvalidOperationException>(() =>
            new FsmCos(catalog).Execute(new RuntimeManifest(42, new[] { Entry(1) })));

        Assert.Contains("request 2", exception.Message, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("bundle 3", exception.Message, StringComparison.OrdinalIgnoreCase);
        Assert.Equal(0, root.LoadCalls);
        Assert.Equal(0, wrongDependency.LoadCalls);
    }

    [Fact]
    public void Execute_rejects_a_catalog_that_reports_success_with_a_null_dependency()
    {
        var root = new TestBundle(1, MicroBundleDependencyRequest.Unconfigured(2));
        var catalog = new NullDependencyCatalog(root, requestedDependencyId: 2);

        var exception = Assert.Throws<InvalidOperationException>(() =>
            new FsmCos(catalog).Execute(new RuntimeManifest(42, new[] { Entry(1) })));

        Assert.Contains("2", exception.Message);
        Assert.Contains("could not be resolved", exception.Message, StringComparison.OrdinalIgnoreCase);
        Assert.Equal(0, root.LoadCalls);
    }

    [Fact]
    public void RuntimeAssembly_exposes_a_read_only_snapshot_of_composed_bundles()
    {
        var bundle = new TestBundle(1);
        var assembly = new FsmCos(new TestCatalog(bundle)).Execute(
            new RuntimeManifest(42, new[] { Entry(1) }));

        var exposedBundles = Assert.IsAssignableFrom<IList<IMicroBundle>>(assembly.Bundles);
        Assert.Throws<NotSupportedException>(() => exposedBundles.Add(new TestBundle(2)));
        Assert.Single(assembly.Bundles);
        Assert.Same(bundle, assembly.Bundles[0]);
    }
    [Fact]
    public void TryExecute_returns_assembly_on_success()
    {
        var result = new FsmCos(new TestCatalog(new TestBundle(1)))
            .TryExecute(new RuntimeManifest(42, new[] { Entry(1) }));
        Assert.True(result.Succeeded);
        Assert.NotNull(result.Assembly);
        Assert.Null(result.FailurePhase);
        Assert.Null(result.Exception);
        Assert.Equal((ulong)42, result.Assembly!.RuntimeId);
    }

    [Fact]
    public void TryExecute_reports_dependency_resolution_phase_and_original_exception()
    {
        var result = new FsmCos(new TestCatalog())
            .TryExecute(new RuntimeManifest(42, new[] { Entry(404) }));
        Assert.False(result.Succeeded);
        Assert.Null(result.Assembly);
        Assert.Equal(RuntimeCompositionPhase.DependencyResolution, result.FailurePhase);
        Assert.IsType<InvalidOperationException>(result.Exception);
        Assert.Contains("could not be resolved", result.Exception!.Message);
    }

    [Fact]
    public void TryExecute_preserves_composition_and_compensation_failures()
    {
        var compositionFailure = new InvalidOperationException("bundle load failed");
        var compensationFailure = new InvalidOperationException("rollback failed");
        var bundle = new TestBundle(1)
        {
            LoadException = compositionFailure,
            RollbackException = compensationFailure
        };

        var result = new FsmCos(new TestCatalog(bundle))
            .TryExecute(new RuntimeManifest(42, new[] { Entry(1) }));

        Assert.False(result.Succeeded);
        Assert.Null(result.Assembly);
        Assert.Equal(RuntimeCompositionPhase.BundleLoading, result.FailurePhase);
        var aggregate = Assert.IsType<AggregateException>(result.Exception);
        Assert.Collection(
            aggregate.InnerExceptions,
            first => Assert.Same(compositionFailure, first),
            second => Assert.Same(compensationFailure, second));
        Assert.Equal(1, bundle.RollbackCalls);
    }

    [Fact]
    public void TryExecute_reports_request_validation_phase_for_null_manifest()
    {
        var result = new FsmCos(new TestCatalog()).TryExecute(null!);
        Assert.False(result.Succeeded);
        Assert.Equal(RuntimeCompositionPhase.RequestValidation, result.FailurePhase);
        Assert.IsType<ArgumentNullException>(result.Exception);
    }

}
