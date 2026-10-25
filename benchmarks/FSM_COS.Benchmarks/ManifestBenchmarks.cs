using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using TheSingularityWorkshop.FSM_COS;

namespace FSM_COS.Benchmarks;

[MemoryDiagnoser]
[ShortRunJob]
public class ManifestBenchmarks
{
    private MicroBundleManifestEntry[] _roots = Array.Empty<MicroBundleManifestEntry>();

    [Params(1, 10, 100, 1000)]
    public int BundleCount { get; set; }

    [GlobalSetup]
    public void Setup() =>
        _roots = Enumerable.Range(1, BundleCount)
            .Select(index => new MicroBundleManifestEntry((ulong)index, "1.0.0-benchmark"))
            .ToArray();

    [Benchmark(Description = "COS-MAN-001 Construct runtime manifest")]
    public RuntimeManifest COS_MAN_001_ConstructRuntimeManifest() =>
        new(1, _roots);
}
