using TheSingularityWorkshop.FSM_COS;
using TheSingularityWorkshop.MicroBundleDomain;

var dataBundle = new DemoBundle(
    id: 2,
    version: "1.0.0",
    name: "Data source");

var reportBundle = new DemoBundle(
    id: 1,
    version: "1.0.0",
    name: "Report",
    dependencyIds: [dataBundle.Id]);

var catalog = new DemoCatalog(dataBundle, reportBundle);
var manifest = new RuntimeManifest(
    RuntimeId: 1001,
    Bundles: [new MicroBundleManifestEntry(reportBundle.Id, "1.0.0")]);

var assembly = new FsmCos(catalog).Execute(manifest);

Console.WriteLine();
Console.WriteLine($"Assembly order: {string.Join(" -> ", assembly.Bundles.Select(bundle => bundle.Id))}");

sealed class DemoCatalog : IMicroBundleCatalog
{
    private readonly IReadOnlyDictionary<ulong, IMicroBundle> _bundles;

    public DemoCatalog(params IMicroBundle[] bundles) =>
        _bundles = bundles.ToDictionary(bundle => bundle.Id);

    public bool TryResolve(ulong bundleId, string version, out IMicroBundle? bundle)
    {
        if (_bundles.TryGetValue(bundleId, out var found) &&
            string.Equals(found.Descriptor.Version, version, StringComparison.Ordinal))
        {
            bundle = found;
            return true;
        }

        bundle = null;
        return false;
    }

    public bool TryResolve(ulong bundleId, out IMicroBundle? bundle) =>
        _bundles.TryGetValue(bundleId, out bundle);
}

sealed class DemoBundle : IMicroBundle
{
    private readonly string _name;

    public DemoBundle(
        ulong id,
        string version,
        string name,
        IEnumerable<ulong>? dependencyIds = null)
    {
        _name = name;
        var ids = (dependencyIds ?? []).ToArray();
        Descriptor = new MicroBundleDescriptor(
            id,
            version,
            dependencies: ids.Select(dependencyId => new MicroBundleDependency(dependencyId)));
        Dependencies = ids
            .Select(MicroBundleDependencyRequest.Unconfigured)
            .ToArray();
    }

    public MicroBundleDescriptor Descriptor { get; }
    public ulong Id => Descriptor.Id;
    public IReadOnlyList<MicroBundleDependencyRequest> Dependencies { get; }

    public void Load(IMicroBundleLoadContext context) =>
        Console.WriteLine($"Loading {_name} (bundle {Id}) for runtime {context.RuntimeId}");

    public bool Arbitrate(IMicroBundleArbitrationContext context, int roundIndex) => false;
}
