namespace TheSingularityWorkshop.FSM_COS;

/// <summary>The phase of composition active when a failed attempt stopped.</summary>
public enum RuntimeCompositionPhase
{
    /// <summary>Manifest and published planning metadata are checked.</summary>
    RequestValidation = 0,
    /// <summary>Root bundles and the dependency closure are resolved and validated.</summary>
    DependencyResolution = 1,
    /// <summary>Bundles and their configuration are loaded.</summary>
    BundleLoading = 2,
    /// <summary>Loaded bundles are arbitrated to convergence.</summary>
    Arbitration = 3
}

/// <summary>Outcome of an opt-in, non-throwing composition attempt.</summary>
/// <remarks>A failed result preserves the original exception. It does not imply rollback of side effects.</remarks>
public sealed class RuntimeCompositionResult
{
    private RuntimeCompositionResult(RuntimeAssembly? assembly, RuntimeCompositionPhase? failurePhase, Exception? exception)
    {
        Assembly = assembly;
        FailurePhase = failurePhase;
        Exception = exception;
    }

    public bool Succeeded => Assembly is not null;
    public RuntimeAssembly? Assembly { get; }
    public RuntimeCompositionPhase? FailurePhase { get; }
    public Exception? Exception { get; }

    internal static RuntimeCompositionResult Success(RuntimeAssembly assembly) => new(assembly, null, null);
    internal static RuntimeCompositionResult Failed(RuntimeCompositionPhase phase, Exception exception) =>
        new(null, phase, exception);
}
