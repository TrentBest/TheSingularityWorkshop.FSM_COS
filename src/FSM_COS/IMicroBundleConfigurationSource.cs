namespace TheSingularityWorkshop.FSM_COS;

/// <summary>Supplies optional per-MicroBundle configuration to a runtime composition.</summary>
public interface IMicroBundleConfigurationSource
{
    /// <summary>
    /// Attempts to obtain configuration for a MicroBundle in a specific runtime.
    /// </summary>
    /// <param name="runtimeId">The runtime being assembled.</param>
    /// <param name="bundleId">The MicroBundle identity.</param>
    /// <param name="version">The resolved MicroBundle version.</param>
    /// <param name="configuration">The configuration bytes when present.</param>
    /// <returns><see langword="true"/> when configuration exists; otherwise <see langword="false"/>.</returns>
    bool TryGetConfiguration(
        ulong runtimeId,
        ulong bundleId,
        string version,
        out ReadOnlyMemory<byte> configuration);
}
