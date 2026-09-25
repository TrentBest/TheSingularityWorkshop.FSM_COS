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
        Assert.Equal(3, bundle.ArbitrationCalls); Assert.Equal(3, assembly.ArbitrationRounds);
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
        public Memory<byte> Configuration { get; private set; }
        public int ChangesRemaining { get; set; }
        public int ArbitrationCalls { get; private set; }
        public void Load(MicroBundleLoadContext context) => Configuration = context.TryGetConfiguration(Id, out var value) ? value.ToArray() : ReadOnlyMemory<byte>.Empty;
        public bool Arbitrate(ArbitrationContext context, int roundIndex) { ArbitrationCalls++; if (ChangesRemaining <= 0) return false; ChangesRemaining--; return true; }
    }
}
