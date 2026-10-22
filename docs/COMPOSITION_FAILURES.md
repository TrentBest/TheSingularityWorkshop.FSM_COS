<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# Composition Outcomes and Failure Diagnostics

FSM_COS exposes two composition styles. Execute keeps the existing exception-based contract. TryExecute returns a RuntimeCompositionResult so a host can report failures without parsing exception messages.

## Inspecting a result

    RuntimeCompositionResult result = cos.TryExecute(manifest);
    if (result.Succeeded)
    {
        RuntimeAssembly assembly = result.Assembly!;
        // Hand the successfully composed runtime to the host.
    }
    else
    {
        Console.Error.WriteLine($"Composition stopped during {result.FailurePhase}.");
        Console.Error.WriteLine(result.Exception?.Message);
    }

A successful result contains an assembly and no failure details. A failed result contains the phase active when composition stopped and the original exception. The exception type, message, and inner details remain available.

## Failure phases

| Phase | What was happening | Typical causes |
|---|---|---|
| RequestValidation | Checking manifest roots and published planning metadata | Invalid manifest, conflicting requested versions, malformed plan metadata, input limits |
| DependencyResolution | Resolving roots and validating the reachable dependency graph | Missing bundle, identity/version mismatch, invalid dependency declaration, cycle, graph limit, schedule mismatch |
| BundleLoading | Obtaining configuration and calling bundle load methods | Configuration-source failure, configuration limit, exception from bundle loading |
| Arbitration | Reconciling loaded bundles until a round reports no changes | Arbitration callback failure or failure to converge within the configured bound |

The phase tells the host where composition stopped; it does not guarantee a root-cause category. Catalogs and bundle callbacks can throw their own exception types, which TryExecute preserves.

## Side-effect boundary

FSM_COS validates the complete reachable dependency graph before bundle loading begins. Once loading starts, bundle code and host-supplied callbacks can produce side effects. A failed result does not roll those effects back. A failure during arbitration likewise cannot undo mutations already performed by bundle code.

This result is diagnostic information, not a transaction, sandbox, or rollback mechanism.

## Compatibility

TryExecute is additive. Existing callers can continue to use Execute and retain its exception behavior. Both methods share the same composition pipeline and resource limits.

See [Architecture](ARCHITECTURE.md), [Runtime Boundary](RUNTIME_BOUNDARY.md), [Arbitration and Convergence](ARBITRATION.md), and [Consuming MicroBundles](CONSUMING_MICROBUNDLES.md) for the wider domain.

---
<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
