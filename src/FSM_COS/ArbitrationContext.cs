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
    public IReadOnlyList<IMicroBundle> LoadedBundles => _loadedBundles;
}
