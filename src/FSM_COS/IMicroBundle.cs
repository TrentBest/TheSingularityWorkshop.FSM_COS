using TheSingularityWorkshop.MicroBundleDomain;

namespace TheSingularityWorkshop.FSM_COS;

/// <summary>The MicroBundle contract understood by FSM_COS.</summary>
public interface IMicroBundle
{
    /// <summary>Domain-owned identity, version, dependencies, and providers.</summary>
    MicroBundleDescriptor Descriptor { get; }

    /// <summary>Stable MicroBundle identity.</summary>
    ulong Id => Descriptor.Id;

    /// <summary>Composition dependencies, including optional configuration.</summary>
    IReadOnlyList<BundleRequest> Dependencies { get; }

    /// <summary>Installs the bundle into the composition.</summary>
    void Load(MicroBundleLoadContext context);

    /// <summary>Participates in composition arbitration.</summary>
    bool Arbitrate(ArbitrationContext context, int roundIndex);
}
