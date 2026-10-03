using TheSingularityWorkshop.FSM_API;
using TheSingularityWorkshop.MicroBundleDomain;

namespace TheSingularityWorkshop.FSM_COS;

/// <summary>FSM_COS implementation of the domain-owned arbitration context.</summary>
public sealed class ArbitrationContext : IMicroBundleArbitrationContext
{
    private readonly IReadOnlyList<IMicroBundle> _loadedBundles;

    internal ArbitrationContext(
        ulong runtimeId,
        IReadOnlyList<IMicroBundle> loadedBundles,
        IStateContext? experienceContext)
    {
        RuntimeId = runtimeId;
        _loadedBundles = loadedBundles;
        ExperienceContext = experienceContext;
    }

    public ulong RuntimeId { get; }

    /// <summary>Optional host FSM_API state context supplied for this composition.</summary>
    public IStateContext? ExperienceContext { get; }

    /// <summary>Bundles participating in the current composition.</summary>
    public IReadOnlyList<IMicroBundle> Bundles => _loadedBundles;

    /// <summary>Compatibility alias for existing composition callers.</summary>
    public IReadOnlyList<IMicroBundle> LoadedBundles => _loadedBundles;

    object? IMicroBundleArbitrationContext.ExperienceContext => ExperienceContext;
}
