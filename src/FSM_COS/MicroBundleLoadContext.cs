namespace TheSingularityWorkshop.FSM_COS;

/// <summary>Context supplied to MicroBundles during installation.</summary>
public sealed class MicroBundleLoadContext
{
    private readonly Dictionary<ulong, ReadOnlyMemory<byte>> _configuration = new();

    internal MicroBundleLoadContext(ulong runtimeId) => RuntimeId = runtimeId;

    public ulong RuntimeId { get; }
    public IReadOnlyDictionary<ulong, ReadOnlyMemory<byte>> Configuration => _configuration;

    internal void SetConfiguration(ulong bundleId, ReadOnlyMemory<byte> configuration) =>
        _configuration[bundleId] = configuration;

    public bool TryGetConfiguration(ulong bundleId, out ReadOnlyMemory<byte> configuration) =>
        _configuration.TryGetValue(bundleId, out configuration);
}
