using Xunit;
using TheSingularityWorkshop.FSM_COS;
namespace FSM_COS.Tests;
public sealed class FsmCosTests
{
    [Fact] public void Execute_loads_dependencies_before_requesting_bundle()
    {
        var catalog = new TestCatalog(new TestBundle(2), new TestBundle(1, BundleRequest.Unconfigured(2)));
        var assembly = new FsmCos(catalog).Execute(new RuntimeManifest(42, new[] { BundleRequest.Unconfigured(1) }));
        Assert.Equal(new ulong[] { 2, 1 }, assembly.Bundles.Select(x => x.Id));
    }
    [Fact] public void Execute_passes_configuration_to_requested_bundle()
    {
        var configuration = new byte[] { 7, 11, 13 }; var bundle = new TestBundle(7);
        new FsmCos(new TestCatalog(bundle)).Execute(new RuntimeManifest(42, new[] { new BundleRequest(7, configuration) }));
        Assert.Equal(configuration, bundle.Configuration.ToArray());
    }
    [Fact] public void Execute_arbitrates_until_a_round_makes_no_changes()
    {
        var bundle = new TestBundle(1) { ChangesRemaining = 2 };
        var assembly = new FsmCos(new TestCatalog(bundle)).Execute(new RuntimeManifest(42, new[] { BundleRequest.Unconfigured(1) }));
        Assert.Equal(3, bundle.ArbitrationCalls); Assert.Equal(2, assembly.ArbitrationRounds);
    }

    [Fact] public void Execute_throws_when_a_requested_bundle_cannot_be_resolved()
    {
        var exception = Assert.Throws<InvalidOperationException>(() =>
            new FsmCos(new TestCatalog()).Execute(new RuntimeManifest(42, new[] { BundleRequest.Unconfigured(99) })));
        Assert.Contains("99", exception.Message);
    }

    [Fact] public void Execute_throws_when_dependencies_form_a_cycle()
    {
        var first = new TestBundle(1, BundleRequest.Unconfigured(2));
        var second = new TestBundle(2, BundleRequest.Unconfigured(1));

        var exception = Assert.Throws<InvalidOperationException>(() =>
            new FsmCos(new TestCatalog(first, second)).Execute(new RuntimeManifest(42, new[] { BundleRequest.Unconfigured(1) })));
        Assert.Contains("cycle", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact] public void Execute_passes_dependency_configuration_before_loading_dependency()
    {
        var dependency = new TestBundle(2);
        var root = new TestBundle(1, new BundleRequest(2, new byte[] { 5, 8, 13 }));

        new FsmCos(new TestCatalog(dependency, root)).Execute(
            new RuntimeManifest(42, new[] { BundleRequest.Unconfigured(1) }));

        Assert.Equal(new byte[] { 5, 8, 13 }, dependency.Configuration.ToArray());
    }

    [Fact] public void Execute_throws_when_arbitration_does_not_converge()
    {
        var bundle = new TestBundle(1) { ChangesRemaining = 100 };

        var exception = Assert.Throws<InvalidOperationException>(() =>
            new FsmCos(new TestCatalog(bundle), maximumArbitrationRounds: 2)
                .Execute(new RuntimeManifest(42, new[] { BundleRequest.Unconfigured(1) })));
        Assert.Contains("did not converge", exception.Message, StringComparison.OrdinalIgnoreCase);
        Assert.Equal(2, bundle.ArbitrationCalls);
    }

    [Fact] public void Constructor_rejects_invalid_arbitration_round_limit()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new FsmCos(new TestCatalog(), 0));
    }

    [Fact] public void Constructor_rejects_null_catalog()
    {
        Assert.Throws<ArgumentNullException>(() => new FsmCos(null!));
    }
    private sealed class TestCatalog : IMicroBundleCatalog
    {
        private readonly Dictionary<ulong, IMicroBundle> _bundles;
        public TestCatalog(params IMicroBundle[] bundles) => _bundles = bundles.ToDictionary(x => x.Id);
        public bool TryResolve(ulong bundleId, out IMicroBundle? bundle) => _bundles.TryGetValue(bundleId, out bundle);
    }
    private sealed class TestBundle : IMicroBundle
    {
        private readonly IReadOnlyList<BundleRequest> _dependencies;
        public TestBundle(ulong id, params BundleRequest[] dependencies) => (_dependencies, Id) = (dependencies, id);
        public ulong Id { get; }
        public IReadOnlyList<BundleRequest> Dependencies => _dependencies;
        public ReadOnlyMemory<byte> Configuration { get; private set; }
        public int ChangesRemaining { get; set; }
        public int ArbitrationCalls { get; private set; }
        public void Load(MicroBundleLoadContext context) => Configuration = context.TryGetConfiguration(Id, out var value) ? value : ReadOnlyMemory<byte>.Empty;
        public bool Arbitrate(ArbitrationContext context, int roundIndex) { ArbitrationCalls++; if (ChangesRemaining <= 0) return false; ChangesRemaining--; return true; }
    }
}