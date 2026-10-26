using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using TheSingularityWorkshop.FSM_COS;

namespace FSM_COS.Benchmarks;

/// <summary>Measures retrieval costs from an already-composed runtime assembly.</summary>
[MemoryDiagnoser]
[ShortRunJob]
public class RuntimeAssemblyLookupBenchmarks
{
    private RuntimeAssembly _assembly = null!;
    private ulong _existingBundleId;
    private ulong _missingBundleId;

    [Params(1, 10, 100, 1000)]
    public int BundleCount { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        var (manifest, catalog) = BenchmarkWorkloads.IndependentGraph(BundleCount);
        _assembly = new FsmCos(catalog).Execute(manifest);
        _existingBundleId = (ulong)BundleCount;
        _missingBundleId = (ulong)BundleCount + 1;
    }

    [Benchmark(Description = "COS-ASM-001 Lookup existing bundle by ID")]
    public bool COS_ASM_001_LookupExistingBundleById() =>
        _assembly.TryGetBundle(_existingBundleId, out _);

    [Benchmark(Description = "COS-ASM-002 Lookup missing bundle by ID")]
    public bool COS_ASM_002_LookupMissingBundleById() =>
        _assembly.TryGetBundle(_missingBundleId, out _);

    [Benchmark(Description = "COS-ASM-003 Typed lookup existing bundle by ID")]
    public bool COS_ASM_003_TypedLookupExistingBundleById() =>
        _assembly.TryGetBundle<BenchmarkBundle>(_existingBundleId, out _);

    [Benchmark(Description = "COS-ASM-004 Typed lookup missing bundle by ID")]
    public bool COS_ASM_004_TypedLookupMissingBundleById() =>
        _assembly.TryGetBundle<BenchmarkBundle>(_missingBundleId, out _);

    [Benchmark(Description = "COS-ASM-005 Lookup first bundle by type")]
    public bool COS_ASM_005_LookupFirstBundleByType() =>
        _assembly.TryGetBundle<BenchmarkBundle>(out _);
}
