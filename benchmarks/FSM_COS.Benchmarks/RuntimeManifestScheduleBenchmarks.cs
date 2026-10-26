using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using TheSingularityWorkshop.FSM_COS;

namespace FSM_COS.Benchmarks;

/// <summary>Measures publish-time dependency-plan construction and query costs.</summary>
[MemoryDiagnoser]
[ShortRunJob]
public class RuntimeManifestScheduleBenchmarks
{
    private RuntimeManifestEntry[] _entries = Array.Empty<RuntimeManifestEntry>();
    private RuntimeManifestDependency[] _dependencies = Array.Empty<RuntimeManifestDependency>();
    private RuntimeManifestSchedule _schedule = null!;

    [Params(1, 10, 100, 1000)]
    public int BundleCount { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _entries = Enumerable.Range(1, BundleCount)
            .Select(index => new RuntimeManifestEntry(
                new MicroBundleReference((ulong)index, "1.0.0-benchmark", $"sha256:benchmark-{index}")))
            .ToArray();
        _dependencies = Enumerable.Range(1, Math.Max(0, BundleCount - 1))
            .Select(index => new RuntimeManifestDependency((ulong)index, (ulong)(index + 1)))
            .ToArray();
        _schedule = new RuntimeManifestSchedule(_entries, _dependencies);
    }

    [Benchmark(Description = "COS-SCH-001 Construct dependency schedule")]
    public RuntimeManifestSchedule COS_SCH_001_ConstructDependencySchedule() =>
        new(_entries, _dependencies);

    [Benchmark(Description = "COS-SCH-002 Query dependencies of middle entry")]
    public int COS_SCH_002_QueryDependenciesOfMiddleEntry() =>
        _schedule.DependenciesOf((ulong)Math.Max(1, BundleCount / 2)).Count;

    [Benchmark(Description = "COS-SCH-003 Check dependency readiness")]
    public bool COS_SCH_003_CheckDependencyReadiness() =>
        _schedule.IsDependencyReady(1, static _ => true);

    [Benchmark(Description = "COS-SCH-004 Enumerate independent entries")]
    public int COS_SCH_004_EnumerateIndependentEntries() =>
        _schedule.IndependentEntries.Count;
}
