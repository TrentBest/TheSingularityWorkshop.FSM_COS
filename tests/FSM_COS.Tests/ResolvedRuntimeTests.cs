#if WORKSHOP_USE_REPOSITORY_SOURCES
using TheSingularityWorkshop.FSM_COS;
using TheSingularityWorkshop.MicroBundleDomain;
using Xunit;

namespace FSM_COS.Tests;

public sealed class ResolvedRuntimeTests
{
    [Fact]
    public void Pre_resolved_manifest_skips_arbitration_and_exposes_restorable_configuration()
    {
        var bundle = new ResolvedBundle(1);
        var catalog = new ResolvedCatalog(bundle);
        var entry = new RuntimeManifestEntry(
            new MicroBundleReference(1, "1.0.0", "sha256:bundle-1"));
        var schedule = new RuntimeManifestSchedule(
            new[] { entry },
            Array.Empty<RuntimeManifestDependency>());
        var manifest = new RuntimeManifest(
            42,
            new[] { new MicroBundleManifestEntry(1, "1.0.0") },
            LoadPlan: new[] { entry },
            Schedule: schedule,
            ArbitrationResolved: true,
            PublishedArbitrationRounds: 0);

        var assembly = new FsmCos(catalog).Execute(manifest, new EmptyResolvedConfigurationSource());

        Assert.Equal(0, bundle.ArbitrationCalls);
        Assert.Equal(0, assembly.ArbitrationRounds);
        Assert.True(assembly.TryGetResolvedConfiguration(1, out var configuration));
        Assert.Empty(configuration.ToArray());
    }

    [Fact]
    public void Pre_resolved_manifest_rejects_bundle_without_resolved_configuration_contract()
    {
        var bundle = new PlainBundle(1);
        var entry = new RuntimeManifestEntry(
            new MicroBundleReference(1, "1.0.0", "sha256:bundle-1"));
        var schedule = new RuntimeManifestSchedule(
            new[] { entry },
            Array.Empty<RuntimeManifestDependency>());
        var manifest = new RuntimeManifest(
            42,
            new[] { new MicroBundleManifestEntry(1, "1.0.0") },
            LoadPlan: new[] { entry },
            Schedule: schedule,
            ArbitrationResolved: true,
            PublishedArbitrationRounds: 0);

        var exception = Assert.Throws<InvalidOperationException>(() =>
            new FsmCos(new ResolvedCatalog(bundle)).Execute(manifest, new EmptyResolvedConfigurationSource()));

        Assert.Contains(nameof(IMicroBundleResolvedConfiguration), exception.Message);
        Assert.Equal(0, bundle.ArbitrationCalls);
    }

    private sealed class EmptyResolvedConfigurationSource : IMicroBundleConfigurationSource
    {
        public bool TryGetConfiguration(
            ulong runtimeId,
            ulong bundleId,
            string version,
            out ReadOnlyMemory<byte> configuration)
        {
            configuration = ReadOnlyMemory<byte>.Empty;
            return runtimeId == 42 && bundleId == 1 && version == "1.0.0";
        }
    }

    private sealed class ResolvedCatalog(IMicroBundle bundle) : IMicroBundleCatalog
    {
        public bool TryResolve(ulong bundleId, out IMicroBundle? resolved)
        {
            resolved = bundle.Id == bundleId ? bundle : null;
            return resolved is not null;
        }

        public bool TryResolve(ulong bundleId, string version, out IMicroBundle? resolved)
        {
            resolved = bundle.Id == bundleId && bundle.Descriptor.Version == version ? bundle : null;
            return resolved is not null;
        }
    }

    private sealed class ResolvedBundle(ulong id) : IMicroBundle, IMicroBundleResolvedConfiguration
    {
        public MicroBundleDescriptor Descriptor { get; } = new(id, "1.0.0");
        public IReadOnlyList<MicroBundleDependencyRequest> Dependencies { get; } = Array.Empty<MicroBundleDependencyRequest>();
        public int ArbitrationCalls { get; private set; }
        private ReadOnlyMemory<byte> _configuration;

        public void Load(IMicroBundleLoadContext context)
        {
            _configuration = context.TryGetConfiguration(id, out var configuration)
                ? configuration.ToArray()
                : ReadOnlyMemory<byte>.Empty;
        }

        public bool Arbitrate(IMicroBundleArbitrationContext context, int roundIndex)
        {
            ArbitrationCalls++;
            return false;
        }

        public ReadOnlyMemory<byte> ExportResolvedConfiguration() => _configuration.ToArray();
    }

    private sealed class PlainBundle(ulong id) : IMicroBundle
    {
        public MicroBundleDescriptor Descriptor { get; } = new(id, "1.0.0");
        public IReadOnlyList<MicroBundleDependencyRequest> Dependencies { get; } = Array.Empty<MicroBundleDependencyRequest>();
        public int ArbitrationCalls { get; private set; }

        public void Load(IMicroBundleLoadContext context) { }

        public bool Arbitrate(IMicroBundleArbitrationContext context, int roundIndex)
        {
            ArbitrationCalls++;
            return false;
        }
    }
}

#endif
