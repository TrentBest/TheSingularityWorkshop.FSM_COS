using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using TheSingularityWorkshop.FSM_COS;

namespace FSM_COS.Benchmarks;

/// <summary>Measures validation of a manifest's staged identity and dependency metadata.</summary>
[MemoryDiagnoser]
[ShortRunJob]
public class RuntimeManifestValidationBenchmarks
{
    private RuntimeManifest _validManifest = null!;
    private RuntimeManifest _manifestWithoutSchedule = null!;

    [Params(1, 10, 100, 1000)]
    public int BundleCount { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        var entries = Enumerable.Range(1, BundleCount)
            .Select(index => new RuntimeManifestEntry(
                new MicroBundleReference((ulong)index, "1.0.0-benchmark", $"sha256:benchmark-{index}"),
                index % 3 == 0 ? ManifestLoadStage.Deferred : ManifestLoadStage.Resident))
            .ToArray();
        var dependencies = Enumerable.Range(1, Math.Max(0, BundleCount - 1))
            .Select(index => new RuntimeManifestDependency((ulong)index, (ulong)(index + 1)))
            .ToArray();
        var schedule = new RuntimeManifestSchedule(entries, dependencies);
        var roots = entries
            .Select(entry => new MicroBundleManifestEntry(entry.Reference.BundleId, entry.Reference.Version))
            .ToArray();

        _validManifest = new RuntimeManifest(
            1,
            roots,
            LoadPlan: entries,
            Schedule: schedule);
        _manifestWithoutSchedule = new RuntimeManifest(1, roots);
    }

    [Benchmark(Description = "COS-VAL-001 Validate staged manifest and schedule")]
    public void COS_VAL_001_ValidateStagedManifestAndSchedule() =>
        _validManifest.ValidateStagedPlan();

    [Benchmark(Description = "COS-VAL-002 Validate manifest without staged metadata")]
    public void COS_VAL_002_ValidateManifestWithoutStagedMetadata() =>
        _manifestWithoutSchedule.ValidateStagedPlan();
}
