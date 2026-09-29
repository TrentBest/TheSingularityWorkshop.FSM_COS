namespace TheSingularityWorkshop.FSM_COS;

/// <summary>Context shared by bundles during arbitration.</summary>
public sealed class ArbitrationContext
{
    private readonly List<IMicroBundle> _loadedBundles;

    internal ArbitrationContext(ulong runtimeId, List<IMicroBundle> loadedBundles)
    {
        RuntimeId = runtimeId;
        _loadedBundles = loadedBundles;
    }

    public ulong RuntimeId { get; }

    /// <summary>Gets the bundles that completed loading and are participating in arbitration.</summary>
    public IReadOnlyList<IMicroBundle> LoadedBundles => _loadedBundles;

    /// <summary>Determines whether a bundle with the specified ID is installed.</summary>
    public bool Contains(ulong bundleId) => _loadedBundles.Any(bundle => bundle.Id == bundleId);

    /// <summary>Finds an installed bundle by ID without exposing the catalog.</summary>
    public bool TryGetBundle(ulong bundleId, out IMicroBundle? bundle)
    {
        bundle = _loadedBundles.FirstOrDefault(candidate => candidate.Id == bundleId);
        return bundle is not null;
    }

    /// <summary>Finds an installed bundle of the requested type.</summary>
    public bool TryGetBundle<TBundle>(out TBundle? bundle)
        where TBundle : class, IMicroBundle
    {
        bundle = _loadedBundles.OfType<TBundle>().FirstOrDefault();
        return bundle is not null;
    }
}
