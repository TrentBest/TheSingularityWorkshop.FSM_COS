namespace TheSingularityWorkshop.FSM_COS;

/// <summary>
/// The stable result of runtime composition.
/// </summary>
/// <remarks>
/// The assembly deliberately exposes MicroBundles rather than platform-specific
/// products. Hosts can retrieve their own composed capabilities without making
/// FSM_COS depend on GUI, AI, browser, desktop, Unity, or provider assemblies.
/// </remarks>
public sealed class RuntimeAssembly
{
    internal RuntimeAssembly(ulong runtimeId, IReadOnlyList<IMicroBundle> bundles, int arbitrationRounds)
    {
        RuntimeId = runtimeId;
        Bundles = bundles;
        ArbitrationRounds = arbitrationRounds;
    }

    /// <summary>Stable identity of the composed runtime.</summary>
    public ulong RuntimeId { get; }

    /// <summary>MicroBundles installed into this stable runtime composition.</summary>
    public IReadOnlyList<IMicroBundle> Bundles { get; }

    /// <summary>Compatibility alias emphasizing that the collection is the result of loading.</summary>
    public IReadOnlyList<IMicroBundle> LoadedBundles => Bundles;

    /// <summary>Number of arbitration rounds required to converge.</summary>
    public int ArbitrationRounds { get; }

    /// <summary>Finds a loaded MicroBundle by its stable identity.</summary>
    public bool TryGetBundle(ulong bundleId, out IMicroBundle? bundle)
    {
        bundle = Bundles.FirstOrDefault(candidate => candidate.Id == bundleId);
        return bundle is not null;
    }

    /// <summary>Finds a loaded MicroBundle of a specific host-known type.</summary>
    public bool TryGetBundle<TBundle>(ulong bundleId, out TBundle? bundle)
        where TBundle : class, IMicroBundle
    {
        bundle = Bundles
            .OfType<TBundle>()
            .FirstOrDefault(candidate => candidate.Id == bundleId);

        return bundle is not null;
    }

    /// <summary>Finds the first loaded MicroBundle of a host-known type.</summary>
    public bool TryGetBundle<TBundle>(out TBundle? bundle)
        where TBundle : class, IMicroBundle
    {
        bundle = Bundles.OfType<TBundle>().FirstOrDefault();
        return bundle is not null;
    }
}
