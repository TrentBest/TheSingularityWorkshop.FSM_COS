namespace TheSingularityWorkshop.FSM_COS;

/// <summary>The MicroBundle contract understood by FSM_COS.</summary>
public interface IMicroBundle
{
    ulong Id { get; }
    IReadOnlyList<BundleRequest> Dependencies { get; }
    void Load(MicroBundleLoadContext context);
    bool Arbitrate(ArbitrationContext context, int roundIndex);
}
