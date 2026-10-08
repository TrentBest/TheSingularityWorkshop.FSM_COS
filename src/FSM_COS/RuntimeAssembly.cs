using TheSingularityWorkshop.FSM_UserIO;
using TheSingularityWorkshop.MicroBundleDomain;

namespace TheSingularityWorkshop.FSM_COS;

/// <summary>
/// The stable result of runtime composition.
/// </summary>
/// <remarks>
/// FSM_COS exposes the domain-owned MicroBundle contract without redefining it.
/// Hosts retrieve composed capabilities without requiring GUI, AI, browser,
/// desktop, platform-specific, or provider assemblies in the composition kernel.
/// </remarks>
public sealed class RuntimeAssembly
{
    internal RuntimeAssembly(
        ulong runtimeId,
        IReadOnlyList<IMicroBundle> bundles,
        int arbitrationRounds,
        SemanticIntent? intent)
    {
        RuntimeId = runtimeId;
        Bundles = bundles;
        ArbitrationRounds = arbitrationRounds;
        Intent = intent;
    }

    public ulong RuntimeId { get; }
    public IReadOnlyList<IMicroBundle> Bundles { get; }
    public IReadOnlyList<IMicroBundle> LoadedBundles => Bundles;
    public int ArbitrationRounds { get; }

    /// <summary>Gets the semantic interaction intent carried by the runtime request, when one was supplied.</summary>
    public SemanticIntent? Intent { get; }

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
