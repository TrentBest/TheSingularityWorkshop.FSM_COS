using TheSingularityWorkshop.MicroBundleDomain;

namespace TheSingularityWorkshop.FSM_COS;

/// <summary>
/// The stable result of runtime composition.
/// </summary>
/// <remarks>
/// FSM_COS exposes the domain-owned MicroBundle contract without redefining it.
/// Hosts retrieve composed capabilities without requiring GUI, AI, browser,
/// desktop, Unity, or provider assemblies in the composition kernel.
/// </remarks>
public sealed class RuntimeAssembly
{
    internal RuntimeAssembly(ulong runtimeId, IReadOnlyList<IMicroBundle> bundles, int arbitrationRounds)
    {
        RuntimeId = runtimeId;
        Bundles = bundles;
        ArbitrationRounds = arbitrationRounds;
    }

    public ulong RuntimeId { get; }
    public IReadOnlyList<IMicroBundle> Bundles { get; }
    public IReadOnlyList<IMicroBundle> LoadedBundles => Bundles;
    public int ArbitrationRounds { get; }

    public bool TryGetBundle(ulong bundleId, out IMicroBundle? bundle)
    {
        bundle = Bundles.FirstOrDefault(candidate => candidate.Id == bundleId);
        return bundle is not null;
    }

    public bool TryGetBundle<TBundle>(ulong bundleId, out TBundle? bundle)
        where TBundle : class, IMicroBundle
    {
        bundle = Bundles.OfType<TBundle>().FirstOrDefault(candidate => candidate.Id == bundleId);
        return bundle is not null;
    }

    public bool TryGetBundle<TBundle>(out TBundle? bundle)
        where TBundle : class, IMicroBundle
    {
        bundle = Bundles.OfType<TBundle>().FirstOrDefault();
        return bundle is not null;
    }
}
