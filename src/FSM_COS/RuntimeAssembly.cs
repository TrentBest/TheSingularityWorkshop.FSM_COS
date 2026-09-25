namespace TheSingularityWorkshop.FSM_COS;

/// <summary>The result of manifest composition.</summary>
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
    public int ArbitrationRounds { get; }
}
