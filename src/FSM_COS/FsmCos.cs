namespace TheSingularityWorkshop.FSM_COS;

public sealed class FsmCos : IFsmCos
{
    public const int DefaultMaximumArbitrationRounds = 10;
    private readonly IMicroBundleCatalog _catalog;
    private readonly int _maximumArbitrationRounds;

    public FsmCos(IMicroBundleCatalog catalog, int maximumArbitrationRounds = DefaultMaximumArbitrationRounds)
    {
        if (maximumArbitrationRounds < 1) throw new ArgumentOutOfRangeException(nameof(maximumArbitrationRounds));
        _catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
        _maximumArbitrationRounds = maximumArbitrationRounds;
    }

    public RuntimeAssembly Execute(RuntimeManifest manifest)
    {
        ArgumentNullException.ThrowIfNull(manifest);
        var loaded = new List<IMicroBundle>();
        var loadedIds = new HashSet<ulong>();
        var loading = new HashSet<ulong>();
        var loadContext = new MicroBundleLoadContext(manifest.RuntimeId);

        foreach (var request in manifest.Bundles)
            LoadRecursive(request, loaded, loadedIds, loading, loadContext);

        var arbitrationContext = new ArbitrationContext(manifest.RuntimeId, loaded);
        var rounds = 0;
        for (; rounds < _maximumArbitrationRounds; rounds++)
        {
            var changed = false;
            foreach (var bundle in loaded) changed |= bundle.Arbitrate(arbitrationContext, rounds);
            if (!changed) break;
        }

        if (rounds == _maximumArbitrationRounds)
            throw new InvalidOperationException($"FSM_COS arbitration did not converge within {_maximumArbitrationRounds} rounds.");

        return new RuntimeAssembly(manifest.RuntimeId, loaded, rounds);
    }

    private void LoadRecursive(BundleRequest request, List<IMicroBundle> loaded, HashSet<ulong> loadedIds, HashSet<ulong> loading, MicroBundleLoadContext loadContext)
    {
        if (loadedIds.Contains(request.BundleId)) return;
        if (!loading.Add(request.BundleId)) throw new InvalidOperationException($"MicroBundle dependency cycle detected at {request.BundleId}.");
        if (!_catalog.TryResolve(request.BundleId, out var bundle) || bundle is null)
            throw new InvalidOperationException($"MicroBundle {request.BundleId} could not be resolved.");

        loadContext.SetConfiguration(request.BundleId, request.Configuration);
        foreach (var dependency in bundle.Dependencies)
            LoadRecursive(dependency, loaded, loadedIds, loading, loadContext);

        bundle.Load(loadContext);
        loaded.Add(bundle);
        loadedIds.Add(bundle.Id);
        loading.Remove(request.BundleId);
    }
}
