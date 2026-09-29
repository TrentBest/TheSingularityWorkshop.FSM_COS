using TheSingularityWorkshop.FSM_API;

namespace TheSingularityWorkshop.FSM_COS;

/// <summary>Context shared by bundles during arbitration.</summary>
public sealed class ArbitrationContext
{
    private readonly List<IMicroBundle> _loadedBundles;

    internal ArbitrationContext(
        ulong runtimeId,
        List<IMicroBundle> loadedBundles,
        IStateContext? experienceContext)
    {
        RuntimeId = runtimeId;
        _loadedBundles = loadedBundles;
        ExperienceContext = experienceContext;
    }

    public ulong RuntimeId { get; }

    /// <summary>Optional FSM_API state context supplied by the host for this composition.</summary>
    public IStateContext? ExperienceContext { get; }

    public IReadOnlyList<IMicroBundle> LoadedBundles => _loadedBundles;
}
