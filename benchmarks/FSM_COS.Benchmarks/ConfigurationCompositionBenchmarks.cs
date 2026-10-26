using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using TheSingularityWorkshop.FSM_COS;
using TheSingularityWorkshop.MicroBundleDomain;

namespace FSM_COS.Benchmarks;

/// <summary>Measures runtime composition when every bundle receives external configuration.</summary>
[MemoryDiagnoser]
[ShortRunJob]
public class ConfigurationCompositionBenchmarks
{
    private FsmCos _composer = null!;
    private RuntimeManifest _manifest = null!;
    private BenchmarkConfigurationSource _configurationSource = null!;

    [Params(1, 10, 100, 1000)]
    public int BundleCount { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        var bundles = Enumerable.Range(1, BundleCount)
            .Select(index => new ConfiguredBenchmarkBundle((ulong)index))
            .ToArray();
        _composer = new FsmCos(new BenchmarkCatalog(bundles));
        _manifest = BenchmarkWorkloads.IndependentRoots(BundleCount);
        _configurationSource = new BenchmarkConfigurationSource();
    }

    [Benchmark(Description = "COS-CFG-001 Compose roots with external configuration")]
    public RuntimeAssembly COS_CFG_001_ComposeRootsWithExternalConfiguration() =>
        _composer.Execute(_manifest, _configurationSource);
}

internal sealed class BenchmarkConfigurationSource : IMicroBundleConfigurationSource
{
    private static readonly byte[] ConfigurationBytes = { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08 };

    public bool TryGetConfiguration(
        ulong runtimeId,
        ulong bundleId,
        string version,
        out ReadOnlyMemory<byte> configuration)
    {
        configuration = ConfigurationBytes;
        return true;
    }
}

internal sealed class ConfiguredBenchmarkBundle : IMicroBundle
{
    private readonly IReadOnlyList<MicroBundleDependencyRequest> _dependencies =
        Array.Empty<MicroBundleDependencyRequest>();

    public ConfiguredBenchmarkBundle(ulong id)
    {
        Id = id;
        Descriptor = new MicroBundleDescriptor(id, "1.0.0-benchmark", Array.Empty<MicroBundleDependency>());
    }

    public ulong Id { get; }
    public MicroBundleDescriptor Descriptor { get; }
    public IReadOnlyList<MicroBundleDependencyRequest> Dependencies => _dependencies;

    public void Load(IMicroBundleLoadContext context)
    {
        if (!context.TryGetConfiguration(Id, out _))
            throw new InvalidOperationException($"Configuration missing for benchmark bundle {Id}.");
    }

    public bool Arbitrate(IMicroBundleArbitrationContext context, int roundIndex) => false;
}
