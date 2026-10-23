namespace TheSingularityWorkshop.FSM_COS;

/// <summary>
/// Coordinates composition and retention of multiple runtime experiences for a host.
/// </summary>
/// <remarks>
/// The host owns the registry reference, not the MicroBundles themselves. Composing an
/// experience does not activate it; activation is an explicit, separate operation so a
/// host can prepare a replacement before switching the selected experience.
/// </remarks>
public sealed class RuntimeExperienceHost
{
    private readonly IFsmCos _composer;

    public RuntimeExperienceHost(IFsmCos composer, RuntimeExperienceRegistry? registry = null)
    {
        _composer = composer ?? throw new ArgumentNullException(nameof(composer));
        Registry = registry ?? new RuntimeExperienceRegistry();
    }

    /// <summary>Gets the registry containing composed experiences retained by this host.</summary>
    public RuntimeExperienceRegistry Registry { get; }

    /// <summary>
    /// Composes and retains a new experience without changing the currently active experience.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the composition succeeds but its runtime identity is already registered.
    /// </exception>
    public RuntimeAssembly ComposeAndRegister(
        RuntimeManifest manifest,
        IMicroBundleConfigurationSource? configurationSource = null)
    {
        ArgumentNullException.ThrowIfNull(manifest);

        var assembly = _composer.Execute(manifest, configurationSource);
        if (!Registry.TryRegister(assembly))
            throw new InvalidOperationException(
                $"Runtime experience {assembly.RuntimeId} is already registered.");

        return assembly;
    }

    /// <summary>Selects a previously composed experience without discarding other experiences.</summary>
    public bool TryActivate(ulong runtimeId) => Registry.TryActivate(runtimeId);

    /// <summary>Clears the active selection while retaining all composed experiences.</summary>
    public void ClearActive() => Registry.ClearActive();
}
