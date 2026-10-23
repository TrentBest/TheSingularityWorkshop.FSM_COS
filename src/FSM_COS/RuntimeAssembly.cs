using System.Collections.ObjectModel;
using TheSingularityWorkshop.FSM_UserIO;
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
    internal RuntimeAssembly(
        ulong runtimeId,
        IReadOnlyList<IMicroBundle> bundles,
        int arbitrationRounds,
        SemanticIntent? intent,
        IReadOnlyDictionary<ulong, ReadOnlyMemory<byte>>? resolvedConfigurations = null)
    {
        RuntimeId = runtimeId;
        Bundles = Array.AsReadOnly(bundles.ToArray());
        ArbitrationRounds = arbitrationRounds;
        Intent = intent;
        ResolvedConfigurations = new ReadOnlyDictionary<ulong, ReadOnlyMemory<byte>>(
            (resolvedConfigurations ?? new Dictionary<ulong, ReadOnlyMemory<byte>>())
                .ToDictionary(pair => pair.Key, pair => (ReadOnlyMemory<byte>)pair.Value.ToArray()));
    }

    public ulong RuntimeId { get; }
    public IReadOnlyList<IMicroBundle> Bundles { get; }
    public IReadOnlyList<IMicroBundle> LoadedBundles => Bundles;
    public int ArbitrationRounds { get; }

    /// <summary>Gets the semantic interaction intent carried by the runtime request, when one was supplied.</summary>
    public SemanticIntent? Intent { get; }

    /// <summary>
    /// Gets the deterministic effective configuration exported by bundles that support
    /// publish-resolved execution. The value can be persisted as a separate per-bundle
    /// configuration artifact and supplied on a later load.
    /// </summary>
    public IReadOnlyDictionary<ulong, ReadOnlyMemory<byte>> ResolvedConfigurations { get; }

    public bool TryGetResolvedConfiguration(ulong bundleId, out ReadOnlyMemory<byte> configuration) =>
        ResolvedConfigurations.TryGetValue(bundleId, out configuration);

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
