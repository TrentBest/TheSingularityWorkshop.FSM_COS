using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using TheSingularityWorkshop.FSM_COS;
using TheSingularityWorkshop.MicroBundleDomain;

namespace FSM_COS.Benchmarks;

/// <summary>Measures composition cost when arbitration requires one changing round before convergence.</summary>
[MemoryDiagnoser]
[ShortRunJob]
public class ArbitrationConvergenceBenchmarks
{
    private FsmCos _composer = null!;
    private RuntimeManifest _manifest = null!;

    [Params(1, 10, 100, 1000)]
    public int BundleCount { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        var bundles = Enumerable.Range(1, BundleCount)
            .Select(index => new ConvergingBenchmarkBundle((ulong)index))
            .ToArray();
        _composer = new FsmCos(new BenchmarkCatalog(bundles));
        _manifest = BenchmarkWorkloads.IndependentRoots(BundleCount);
    }

    [Benchmark(Description = "COS-ARB-001 Compose bundles with one changing arbitration round")]
    public RuntimeAssembly COS_ARB_001_ComposeUntilArbitrationConverges() =>
        _composer.Execute(_manifest);
}

internal sealed class ConvergingBenchmarkBundle : IMicroBundle
{
    private readonly IReadOnlyList<MicroBundleDependencyRequest> _dependencies =
        Array.Empty<MicroBundleDependencyRequest>();

    public ConvergingBenchmarkBundle(ulong id)
    {
        Id = id;
        Descriptor = new MicroBundleDescriptor(id, "1.0.0-benchmark", Array.Empty<MicroBundleDependency>());
    }

    public ulong Id { get; }
    public MicroBundleDescriptor Descriptor { get; }
    public IReadOnlyList<MicroBundleDependencyRequest> Dependencies => _dependencies;

    public void Load(IMicroBundleLoadContext context) { }

    // Simulate a deterministic bundle that changes state in round zero and then settles.
    public bool Arbitrate(IMicroBundleArbitrationContext context, int roundIndex) => roundIndex == 0;
}
