using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using TheSingularityWorkshop.FSM_COS;

namespace FSM_COS.Benchmarks;

/// <summary>Measures the host-facing registry operations separately from composition.</summary>
[MemoryDiagnoser]
[ShortRunJob]
public class RuntimeExperienceRegistryBenchmarks
{
    private RuntimeExperienceRegistry _registry = null!;
    private RuntimeAssembly[] _assemblies = Array.Empty<RuntimeAssembly>();
    private RuntimeAssembly _target = null!;
    private FsmCos _composer = null!;

    [Params(1, 10, 100, 1000)]
    public int ExperienceCount { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _composer = new FsmCos(new BenchmarkCatalog(Array.Empty<BenchmarkBundle>()));
        _assemblies = Enumerable.Range(1, ExperienceCount)
            .Select(id => _composer.Execute(RuntimeManifest.Empty((ulong)id)))
            .ToArray();
        _target = _assemblies[^1];
        _registry = new RuntimeExperienceRegistry();
        foreach (var assembly in _assemblies)
            _registry.TryRegister(assembly);
        _registry.TryActivate(_target.RuntimeId);
    }

    [Benchmark(Description = "COS-REG-001 Register one experience in empty registry")]
    public bool COS_REG_001_RegisterOneExperienceInEmptyRegistry()
    {
        var registry = new RuntimeExperienceRegistry();
        return registry.TryRegister(_target);
    }

    [Benchmark(Description = "COS-REG-002 Lookup existing experience")]
    public bool COS_REG_002_LookupExistingExperience() =>
        _registry.TryGet(_target.RuntimeId, out _);

    [Benchmark(Description = "COS-REG-003 Activate existing experience")]
    public bool COS_REG_003_ActivateExistingExperience() =>
        _registry.TryActivate(_target.RuntimeId);

    [Benchmark(Description = "COS-REG-004 Snapshot registered experiences")]
    public int COS_REG_004_SnapshotRegisteredExperiences() =>
        _registry.Assemblies.Count;
}
