using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using TheSingularityWorkshop.FSM_COS;

namespace FSM_COS.Benchmarks;

/// <summary>Measures the host-facing published-to-localized-to-loaded plan boundary.</summary>
[MemoryDiagnoser]
[ShortRunJob]
public class RuntimeManifestLoadPlanBenchmarks
{
    private RuntimeManifestEntry[] _entries = Array.Empty<RuntimeManifestEntry>();
    private RuntimeManifestLoadPlan _freshPlan = null!;
    private RuntimeManifestLoadPlan _localizedPlan = null!;
    private RuntimeManifestLoadPlan _loadedPlan = null!;
    private RuntimeManifestSchedule _schedule = null!;
    private IManifestLoadEvaluator _evaluator = null!;

    [Params(1, 10, 100, 1000)]
    public int BundleCount { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _entries = Enumerable.Range(1, BundleCount)
            .Select(index => new RuntimeManifestEntry(
                new MicroBundleReference((ulong)index, "1.0.0-benchmark", $"sha256:benchmark-{index}"),
                index % 3 == 0 ? ManifestLoadStage.Deferred : ManifestLoadStage.Resident))
            .ToArray();

        var dependencies = Enumerable.Range(1, Math.Max(0, BundleCount - 1))
            .Select(index => new RuntimeManifestDependency((ulong)index, (ulong)(index + 1)))
            .ToArray();
        _schedule = new RuntimeManifestSchedule(_entries, dependencies);
        _freshPlan = new RuntimeManifestLoadPlan(_entries, _schedule);

        _localizedPlan = new RuntimeManifestLoadPlan(_entries, _schedule);
        foreach (var entry in _entries)
            _localizedPlan.MarkLocalized(entry.Reference.BundleId);

        _loadedPlan = new RuntimeManifestLoadPlan(_entries, _schedule);
        for (var index = BundleCount; index >= 1; index--)
        {
            _loadedPlan.MarkLocalized((ulong)index);
            _loadedPlan.MarkLoaded((ulong)index);
        }

        _evaluator = new BenchmarkManifestLoadEvaluator();
    }

    [Benchmark(Description = "COS-LOAD-001 Construct runtime load plan")]
    public RuntimeManifestLoadPlan COS_LOAD_001_ConstructRuntimeLoadPlan() =>
        new(_entries, _schedule);

    [Benchmark(Description = "COS-LOAD-002 Lookup published bundle state")]
    public MicroBundleRuntimeState COS_LOAD_002_LookupPublishedBundleState() =>
        _freshPlan.GetState((ulong)BundleCount);

    [Benchmark(Description = "COS-LOAD-003 Mark bundle localized")]
    public void COS_LOAD_003_MarkBundleLocalized() =>
        _freshPlan.MarkLocalized(1);

    [Benchmark(Description = "COS-LOAD-004 Mark dependency-ready bundle loaded")]
    public void COS_LOAD_004_MarkDependencyReadyBundleLoaded() =>
        _loadedPlan.GetState((ulong)BundleCount);

    [Benchmark(Description = "COS-LOAD-005 Evaluate eligible promotions")]
    public int COS_LOAD_005_EvaluateEligiblePromotions() =>
        _localizedPlan.EvaluatePromotions(_evaluator, experienceContext: null, _schedule).Count();
}

internal sealed class BenchmarkManifestLoadEvaluator : IManifestLoadEvaluator
{
    public bool ShouldLoad(RuntimeManifestEntry entry, TheSingularityWorkshop.FSM_API.IStateContext? experienceContext) =>
        true;
}
