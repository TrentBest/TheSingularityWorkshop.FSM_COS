using TheSingularityWorkshop.FSM_API;
using TheSingularityWorkshop.FSM_COS;
using TheSingularityWorkshop.MicroBundleDomain;
using Xunit;

namespace FSM_COS.Tests;

public sealed class FsmCosTests
{
    [Fact]
    public void Execute_loads_dependencies_before_requesting_bundle()
    {
        var catalog = new TestCatalog(new TestBundle(2), new TestBundle(1, MicroBundleDependencyRequest.Unconfigured(2)));

        var assembly = new FsmCos(catalog).Execute(
            new RuntimeManifest(42, new[] { MicroBundleDependencyRequest.Unconfigured(1) }));

        Assert.Equal(new ulong[] { 2, 1 }, assembly.Bundles.Select(x => x.Id));
    }

    [Fact]
    public void Execute_passes_configuration_to_requested_bundle()
    {
        var configuration = new byte[] { 7, 11, 13 };
        var bundle = new TestBundle(7);

        new FsmCos(new TestCatalog(bundle)).Execute(
            new RuntimeManifest(42, new[] { new MicroBundleDependencyRequest(7, configuration) }));

        Assert.Equal(configuration, bundle.Configuration.ToArray());
    }

    [Fact]
    public void Execute_passes_dependency_configuration_before_loading_dependency()
    {
        var dependency = new TestBundle(2);
        var root = new TestBundle(1, new MicroBundleDependencyRequest(2, new byte[] { 5, 8, 13 }));

        new FsmCos(new TestCatalog(dependency, root)).Execute(
            new RuntimeManifest(42, new[] { MicroBundleDependencyRequest.Unconfigured(1) }));

        Assert.Equal(new byte[] { 5, 8, 13 }, dependency.Configuration.ToArray());
    }

    [Fact]
    public void Execute_deduplicates_duplicate_root_requests()
    {
        var bundle = new TestBundle(1);

        var assembly = new FsmCos(new TestCatalog(bundle)).Execute(
            new RuntimeManifest(42, new[]
            {
                new MicroBundleDependencyRequest(1, new byte[] { 1 }),
                new MicroBundleDependencyRequest(1, new byte[] { 2 })
            }));

        Assert.Single(assembly.Bundles);
        Assert.Equal(new byte[] { 1 }, bundle.Configuration.ToArray());
        Assert.Equal(1, bundle.LoadCalls);
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
                MicroBundleDependencyRequest.Unconfigured(1),
                MicroBundleDependencyRequest.Unconfigured(2)
            }));

        Assert.Equal(new ulong[] { 3, 1, 2 }, assembly.Bundles.Select(x => x.Id));
        Assert.Equal(1, shared.LoadCalls);
    }

    [Fact]
    public void Execute_arbitrates_until_a_round_makes_no_changes()
    {
        var bundle = new TestBundle(1) { ChangesRemaining = 2 };

        var assembly = new FsmCos(new TestCatalog(bundle)).Execute(
            new RuntimeManifest(42, new[] { MicroBundleDependencyRequest.Unconfigured(1) }));

        Assert.Equal(3, bundle.ArbitrationCalls);
        Assert.Equal(2, assembly.ArbitrationRounds);
    }

    [Fact]
    public void Execute_stops_after_the_first_stable_round()
    {
        var bundle = new TestBundle(1);

        var assembly = new FsmCos(new TestCatalog(bundle)).Execute(
            new RuntimeManifest(42, new[] { MicroBundleDependencyRequest.Unconfigured(1) }));

        Assert.Equal(1, bundle.ArbitrationCalls);
        Assert.Equal(0, assembly.ArbitrationRounds);
    }

    [Fact]
    public void Execute_throws_when_a_requested_bundle_cannot_be_resolved()
    {
        var exception = Assert.Throws<InvalidOperationException>(() =>
            new FsmCos(new TestCatalog()).Execute(
                new RuntimeManifest(42, new[] { MicroBundleDependencyRequest.Unconfigured(99) })));

        Assert.Contains("99", exception.Message);
    }

    [Fact]
    public void Execute_throws_when_dependencies_form_a_cycle()
    {
        var first = new TestBundle(1, MicroBundleDependencyRequest.Unconfigured(2));
        var second = new TestBundle(2, MicroBundleDependencyRequest.Unconfigured(1));

        var exception = Assert.Throws<InvalidOperationException>(() =>
            new FsmCos(new TestCatalog(first, second)).Execute(
                new RuntimeManifest(42, new[] { MicroBundleDependencyRequest.Unconfigured(1) })));

        Assert.Contains("cycle", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Execute_throws_when_arbitration_does_not_converge()
    {
        var bundle = new TestBundle(1) { ChangesRemaining = 100 };

        var exception = Assert.Throws<InvalidOperationException>(() =>
            new FsmCos(new TestCatalog(bundle), maximumArbitrationRounds: 2)
                .Execute(new RuntimeManifest(42, new[] { MicroBundleDependencyRequest.Unconfigured(1) })));

        Assert.Contains("did not converge", exception.Message, StringComparison.OrdinalIgnoreCase);
        Assert.Equal(2, bundle.ArbitrationCalls);
    }

    [Fact]
    public void Execute_rejects_zero_bundle_ids()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new FsmCos(new TestCatalog()).Execute(
                new RuntimeManifest(42, new[] { MicroBundleDependencyRequest.Unconfigured(0) })));
    }

    [Fact]
    public void Execute_rejects_catalog_identity_mismatch()
    {
        var exception = Assert.Throws<InvalidOperationException>(() =>
            new FsmCos(new TestCatalog(new TestBundle(7)))
                .Execute(new RuntimeManifest(42, new[] { MicroBundleDependencyRequest.Unconfigured(8) })));

        Assert.Contains("8", exception.Message);
    }

    [Fact]
    public void Execute_passes_runtime_identity_and_experience_context_to_arbitration()
    {
        var context = new TestStateContext();
        var bundle = new TestBundle(1);

        new FsmCos(new TestCatalog(bundle)).Execute(
            new RuntimeManifest(1234, new[] { MicroBundleDependencyRequest.Unconfigured(1) }, context));

        Assert.Equal(1234UL, bundle.SeenRuntimeId);
        Assert.Same(context, bundle.SeenExperienceContext);
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
            new RuntimeManifest(42, new[] { MicroBundleDependencyRequest.Unconfigured(17) }));

        Assert.True(assembly.TryGetBundle(17, out var resolved));
        Assert.Same(bundle, resolved);
        Assert.False(assembly.TryGetBundle(99, out _));
    }

    [Fact]
    public void RuntimeAssembly_can_find_host_known_bundle_type()
    {
        var bundle = new TestBundle(23);

        var assembly = new FsmCos(new TestCatalog(bundle)).Execute(
            new RuntimeManifest(42, new[] { MicroBundleDependencyRequest.Unconfigured(23) }));

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
            new RuntimeManifest(42, new[] { MicroBundleDependencyRequest.Unconfigured(31) }));

        Assert.Same(assembly.Bundles, assembly.LoadedBundles);
        Assert.Single(assembly.LoadedBundles);
        Assert.Same(bundle, assembly.LoadedBundles[0]);
    }

    private sealed class TestCatalog : IMicroBundleCatalog
    {
        private readonly Dictionary<ulong, TheSingularityWorkshop.MicroBundleDomain.IMicroBundle> _bundles;

        public TestCatalog(params TheSingularityWorkshop.MicroBundleDomain.IMicroBundle[] bundles) =>
            _bundles = bundles.ToDictionary(x => x.Id);

        public bool TryResolve(ulong bundleId, out TheSingularityWorkshop.MicroBundleDomain.IMicroBundle? bundle) =>
            _bundles.TryGetValue(bundleId, out bundle);
    }

    private sealed class TestBundle : TheSingularityWorkshop.MicroBundleDomain.IMicroBundle
    {
        private readonly IReadOnlyList<MicroBundleDependencyRequest> _dependencies;

        public TestBundle(ulong id, params MicroBundleDependencyRequest[] dependencies)
        {
            Id = id;
            _dependencies = dependencies;
            Descriptor = new MicroBundleDescriptor(
                id,
                "0.1.0-test",
                dependencies.Select(x => new MicroBundleDependency(x.BundleId)));
        }

        public ulong Id { get; }
        public MicroBundleDescriptor Descriptor { get; }
        public IReadOnlyList<MicroBundleDependencyRequest> Dependencies => _dependencies;
        public ReadOnlyMemory<byte> Configuration { get; private set; }
        public int LoadCalls { get; private set; }
        public int ChangesRemaining { get; set; }
        public int ArbitrationCalls { get; private set; }
        public ulong SeenRuntimeId { get; private set; }
        public object? SeenExperienceContext { get; private set; }

        public void Load(IMicroBundleLoadContext context)
        {
            LoadCalls++;
            Configuration = context.TryGetConfiguration(Id, out var value)
                ? value
                : ReadOnlyMemory<byte>.Empty;
        }

        public bool Arbitrate(IMicroBundleArbitrationContext context, int roundIndex)
        {
            ArbitrationCalls++;
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
}
