#if WORKSHOP_USE_REPOSITORY_SOURCES || WORKSHOP_ENABLE_TYPED_PROVIDER_PREPARATION
using TheSingularityWorkshop.FSM_COS;
using TheSingularityWorkshop.MicroBundleDomain;
using Xunit;

namespace FSM_COS.Tests;

public sealed class ProviderPreparationTests
{
    [Fact]
    public void Execute_prepares_every_bundle_in_deterministic_order_before_any_load()
    {
        var events = new List<string>();
        var provider = new DemoCapability();
        var providerBundle = new PreparatoryBundle(
            2,
            events,
            new MicroBundleProvider("tooling", "2.1.0"),
            new MicroBundleProvider("legacy-tooling"));
        var consumerBundle = new PreparatoryBundle(1, events)
        {
            ProviderReference = new MicroBundleProviderReference(2, "1.0.0", "tooling", "2.1.0")
        };
        var manifest = new RuntimeManifest(
            42,
            [new MicroBundleManifestEntry(2, "1.0.0"), new MicroBundleManifestEntry(1, "1.0.0")]);

        var assembly = new FsmCos(new TestCatalog(consumerBundle, providerBundle))
            .Execute(manifest, configurationSource: null, providerResolver: new TestProviderResolver(provider));

        Assert.Equal(new[] { "prepare:1", "prepare:2", "load:2", "load:1" }, events);
        Assert.Same(provider, consumerBundle.ResolvedProvider);
        Assert.Equal(2, assembly.Bundles.Count);
    }

    [Fact]
    public void Preparation_context_rejects_unpinned_or_mismatched_provider_references()
    {
        var events = new List<string>();
        var bundle = new PreparatoryBundle(7, events, new MicroBundleProvider("tooling", "2.1.0"))
        {
            ProviderReference = new MicroBundleProviderReference(7, "wrong-bundle-version", "tooling", "2.1.0"),
            ProbeUnpinnedProvider = true
        };

        var result = new FsmCos(new TestCatalog(bundle))
            .TryExecute(
                new RuntimeManifest(42, [new MicroBundleManifestEntry(7, "1.0.0")]),
                configurationSource: null,
                new TestProviderResolver(new DemoCapability()));

        Assert.True(result.Succeeded);
        Assert.False(bundle.UnpinnedProviderResolved);
        Assert.False(bundle.MismatchedProviderResolved);
    }

    [Fact]
    public void TryExecute_reports_provider_preparation_failure_before_loading()
    {
        var events = new List<string>();
        var expected = new InvalidOperationException("preparation failed");
        var bundle = new PreparatoryBundle(7, events) { PreparationException = expected };

        var result = new FsmCos(new TestCatalog(bundle)).TryExecute(
            new RuntimeManifest(42, [new MicroBundleManifestEntry(7, "1.0.0")]));

        Assert.False(result.Succeeded);
        Assert.Equal(RuntimeCompositionPhase.ProviderPreparation, result.FailurePhase);
        Assert.Same(expected, result.Exception);
        Assert.Empty(events);
    }

    private sealed class DemoCapability { }

    private sealed class TestProviderResolver(object value) : IMicroBundleProviderResolver
    {
        public bool TryResolve<TProvider>(
            MicroBundleProviderReference reference,
            out TProvider? provider)
            where TProvider : class
        {
            provider = value as TProvider;
            return provider is not null;
        }
    }

    private sealed class TestCatalog(params IMicroBundle[] bundles) : IMicroBundleCatalog
    {
        private readonly IReadOnlyDictionary<ulong, IMicroBundle> _bundles =
            bundles.ToDictionary(bundle => bundle.Id);

        public bool TryResolve(ulong bundleId, string version, out IMicroBundle? bundle)
        {
            if (_bundles.TryGetValue(bundleId, out var candidate) &&
                string.Equals(candidate.Descriptor.Version, version, StringComparison.Ordinal))
            {
                bundle = candidate;
                return true;
            }

            bundle = null;
            return false;
        }

        public bool TryResolve(ulong bundleId, out IMicroBundle? bundle) =>
            _bundles.TryGetValue(bundleId, out bundle);
    }

    private sealed class PreparatoryBundle : IMicroBundle, IMicroBundlePreparable
    {
        private readonly List<string> _events;

        public PreparatoryBundle(
            ulong id,
            List<string> events,
            params MicroBundleProvider[] providers)
        {
            Id = id;
            _events = events;
            Descriptor = new MicroBundleDescriptor(id, "1.0.0", providers: providers);
        }

        public ulong Id { get; }
        public MicroBundleDescriptor Descriptor { get; }
        public IReadOnlyList<MicroBundleDependencyRequest> Dependencies { get; } = [];
        public MicroBundleProviderReference ProviderReference { get; set; }
        public object? ResolvedProvider { get; private set; }
        public Exception? PreparationException { get; set; }
        public bool ProbeUnpinnedProvider { get; set; }
        public bool UnpinnedProviderResolved { get; private set; }
        public bool MismatchedProviderResolved { get; private set; }

        public void Prepare(IMicroBundlePreparationContext context)
        {
            if (PreparationException is not null)
                throw PreparationException;

            _events.Add($"prepare:{Id}");
            if (Id == 1 && ProviderReference.IsValid)
            {
                var discovered = Assert.Single(context.Providers);
                Assert.Equal(ProviderReference, discovered);
                Assert.True(context.TryGetProvider<DemoCapability>(discovered, out var capability));
                ResolvedProvider = capability;
            }

            if (ProbeUnpinnedProvider)
            {
                UnpinnedProviderResolved = context.TryGetProvider<DemoCapability>(
                    new MicroBundleProviderReference(Id, "1.0.0", "tooling", ""),
                    out _);
                MismatchedProviderResolved = context.TryGetProvider<DemoCapability>(
                    ProviderReference,
                    out _);
            }
        }

        public void Load(IMicroBundleLoadContext context) => _events.Add($"load:{Id}");

        public bool Arbitrate(IMicroBundleArbitrationContext context, int roundIndex) => false;
    }
}

#endif
