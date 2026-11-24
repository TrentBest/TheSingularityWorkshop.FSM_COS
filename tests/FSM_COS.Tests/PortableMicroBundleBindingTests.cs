#if WORKSHOP_USE_REPOSITORY_SOURCES
using TheSingularityWorkshop.MicroBundleDomain;
using TheSingularityWorkshop.FSM_COS;
using Xunit;

namespace FSM_COS.Tests;

public sealed class PortableMicroBundleBindingTests
{
    [Fact]
    public void Execute_requires_explicit_binding_set_for_portable_bundles()
    {
        var bundle = new TestPortableBundle(1, "workshop:capability/root", "1.0.0");

        var exception = Assert.Throws<InvalidOperationException>(() =>
            new FsmCos(new TestCatalog(bundle)).Execute(
                new RuntimeManifest(42, [new MicroBundleManifestEntry(1, "1.0.0")])));

        Assert.Contains("require an explicit PlatformMicroBundleDomainBindingSet", exception.Message);
        Assert.Equal(0, bundle.LoadCalls);
    }

    [Fact]
    public void Execute_validates_portable_projection_before_loading_the_dependency_closure()
    {
        var configuration = new byte[] { 4, 8, 15, 16, 23, 42 };
        var dependencyReference = new PortableMicroBundleReference("workshop:capability/dependency", "2.0.0");
        var dependency = new TestPortableBundle(2, dependencyReference.Ontology, dependencyReference.Version);
        var root = new TestPortableBundle(
            1,
            "workshop:capability/root",
            "1.0.0",
            [new PortableMicroBundleDependency(dependencyReference, configuration)],
            [2]);

        var bindings = new PlatformMicroBundleDomainBindingSet(
        [
            Binding(dependency, 20),
            Binding(root, 10)
        ]);

        var assembly = new FsmCos(new TestCatalog(root, dependency)).Execute(
            new RuntimeManifest(42, [new MicroBundleManifestEntry(1, "1.0.0")]),
            configurationSource: null,
            providerResolver: null,
            domainBindingSet: bindings);

        Assert.Equal(new ulong[] { 2, 1 }, assembly.Bundles.Select(bundle => bundle.Id));
        Assert.Equal(1, dependency.LoadCalls);
        Assert.Equal(1, root.LoadCalls);
    }

    [Fact]
    public void Execute_rejects_runtime_configuration_that_differs_from_portable_declaration_before_loading()
    {
        var declaredConfiguration = new byte[] { 1, 2, 3 };
        var runtimeConfiguration = new byte[] { 1, 2, 4 };
        var dependencyReference = new PortableMicroBundleReference("workshop:capability/dependency", "2.0.0");
        var dependency = new TestPortableBundle(2, dependencyReference.Ontology, dependencyReference.Version);
        var root = new TestPortableBundle(
            1,
            "workshop:capability/root",
            "1.0.0",
            [new PortableMicroBundleDependency(dependencyReference, declaredConfiguration)],
            [2],
            runtimeConfigurationOverride: runtimeConfiguration);
        var bindings = new PlatformMicroBundleDomainBindingSet([Binding(root, 10), Binding(dependency, 20)]);

        var exception = Assert.Throws<InvalidOperationException>(() =>
            new FsmCos(new TestCatalog(root, dependency)).Execute(
                new RuntimeManifest(42, [new MicroBundleManifestEntry(1, "1.0.0")]),
                configurationSource: null,
                providerResolver: null,
                domainBindingSet: bindings));

        Assert.Contains("does not match its portable identity, exact version, and configuration", exception.Message);
        Assert.Equal(0, root.LoadCalls);
        Assert.Equal(0, dependency.LoadCalls);
    }

    [Fact]
    public void Execute_rejects_an_unbound_portable_dependency_before_loading()
    {
        var dependencyReference = new PortableMicroBundleReference("workshop:capability/dependency", "2.0.0");
        var dependency = new TestPortableBundle(2, dependencyReference.Ontology, dependencyReference.Version);
        var root = new TestPortableBundle(
            1,
            "workshop:capability/root",
            "1.0.0",
            [new PortableMicroBundleDependency(dependencyReference)],
            [2]);
        var bindings = new PlatformMicroBundleDomainBindingSet([Binding(root, 10)]);

        var exception = Assert.Throws<InvalidOperationException>(() =>
            new FsmCos(new TestCatalog(root, dependency)).Execute(
                new RuntimeManifest(42, [new MicroBundleManifestEntry(1, "1.0.0")]),
                configurationSource: null,
                providerResolver: null,
                domainBindingSet: bindings));

        Assert.Contains("has no matching explicit platform binding", exception.Message);
        Assert.Equal(0, root.LoadCalls);
        Assert.Equal(0, dependency.LoadCalls);
    }

    [Fact]
    public void Execute_rejects_portable_digest_that_differs_from_materializer_verified_digest()
    {
        var expectedDigest = new string('a', 64);
        var verifiedDigest = new string('b', 64);
        var identity = new PortableMicroBundleReference("workshop:capability/root", "1.0.0", expectedDigest);
        var root = new TestPortableBundle(1, identity);
        var binding = new PlatformMicroBundleDomainBinding(
            identity.Ontology,
            new PlatformMicroBundleSlot(10),
            root.Id,
            root,
            identity.Version,
            verifiedDigest);
        var bindings = new PlatformMicroBundleDomainBindingSet([binding]);

        var exception = Assert.Throws<InvalidOperationException>(() =>
            new FsmCos(new TestCatalog(root)).Execute(
                new RuntimeManifest(42, [new MicroBundleManifestEntry(1, "1.0.0")]),
                configurationSource: null,
                providerResolver: null,
                domainBindingSet: bindings));

        Assert.Contains("does not match the SHA-256 verified", exception.Message);
        Assert.Equal(0, root.LoadCalls);
    }

    private static PlatformMicroBundleDomainBinding Binding(TestPortableBundle bundle, ulong slot) =>
        new(bundle.PortableIdentity.Ontology, new PlatformMicroBundleSlot(slot), bundle.Id, bundle, bundle.Descriptor.Version);

    private sealed class TestPortableBundle : IPortableMicroBundle
    {
        private readonly IReadOnlyList<MicroBundleDependencyRequest> _dependencies;

        public TestPortableBundle(
            ulong id,
            string ontology,
            string version,
            IReadOnlyList<PortableMicroBundleDependency>? portableDependencies = null,
            IReadOnlyList<ulong>? domainDependencyIds = null,
            byte[]? runtimeConfigurationOverride = null)
            : this(
                id,
                new PortableMicroBundleReference(ontology, version),
                portableDependencies,
                domainDependencyIds,
                runtimeConfigurationOverride)
        {
        }

        public TestPortableBundle(
            ulong id,
            PortableMicroBundleReference identity,
            IReadOnlyList<PortableMicroBundleDependency>? portableDependencies = null,
            IReadOnlyList<ulong>? domainDependencyIds = null,
            byte[]? runtimeConfigurationOverride = null)
        {
            Id = id;
            PortableIdentity = identity;
            PortableDependencies = Array.AsReadOnly((portableDependencies ?? []).ToArray());
            var ids = domainDependencyIds ?? [];
            if (ids.Count != PortableDependencies.Count)
                throw new ArgumentException("Test dependency ID count must match portable dependencies.");

            var descriptorDependencies = PortableDependencies
                .Select((dependency, index) => MicroBundleDependency.Pinned(
                    ids[index],
                    dependency.Reference.Version))
                .ToArray();
            Descriptor = new MicroBundleDescriptor(id, identity.Version, descriptorDependencies);

            _dependencies = Array.AsReadOnly(PortableDependencies
                .Select((dependency, index) => MicroBundleDependencyRequest.Pinned(
                    ids[index],
                    dependency.Reference.Version,
                    runtimeConfigurationOverride is null
                        ? dependency.Configuration
                        : runtimeConfigurationOverride))
                .ToArray());
        }

        public ulong Id { get; }
        public MicroBundleDescriptor Descriptor { get; }
        public PortableMicroBundleReference PortableIdentity { get; }
        public IReadOnlyList<PortableMicroBundleDependency> PortableDependencies { get; }
        public IReadOnlyList<MicroBundleDependencyRequest> Dependencies => _dependencies;
        public int LoadCalls { get; private set; }

        public void Load(IMicroBundleLoadContext context) => LoadCalls++;
        public bool Arbitrate(IMicroBundleArbitrationContext context, int roundIndex) => false;
    }

    private sealed class TestCatalog : IMicroBundleCatalog
    {
        private readonly IReadOnlyDictionary<ulong, IMicroBundle> _bundles;

        public TestCatalog(params IMicroBundle[] bundles) =>
            _bundles = bundles.ToDictionary(bundle => bundle.Id);

        public bool TryResolve(ulong bundleId, string version, out IMicroBundle? bundle)
        {
            if (_bundles.TryGetValue(bundleId, out bundle) &&
                string.Equals(bundle.Descriptor.Version, version, StringComparison.Ordinal))
                return true;
            bundle = null;
            return false;
        }

        public bool TryResolve(ulong bundleId, out IMicroBundle? bundle) =>
            _bundles.TryGetValue(bundleId, out bundle);
    }
}
#endif
