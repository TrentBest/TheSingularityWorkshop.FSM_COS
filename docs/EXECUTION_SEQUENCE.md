# FSM_COS Execute Sequence

> This sequence documents the current `FsmCos.Execute` implementation. It is a source-derived contract view, not a runtime trace. The implementation and tests remain authoritative.

## Successful composition path

```mermaid
sequenceDiagram
    autonumber
    actor Host as Caller / Host
    participant COS as FsmCos.Execute
    participant Catalog as IMicroBundleCatalog
    participant Bundle as Resolved MicroBundles
    participant Config as IMicroBundleConfigurationSource
    participant Assembly as RuntimeAssembly

    Host->>COS: Execute(RuntimeManifest, configurationSource?)
    COS->>COS: Bound checks and caller-collection snapshots
    COS->>COS: Validate staged plan and root IDs/versions
    loop Each distinct requested root
        COS->>Catalog: TryResolve(bundleId, requestedVersion)
        Catalog-->>COS: Exact bundle or resolution failure
        COS->>COS: Validate descriptor identity/version
    end
    COS->>COS: Resolve complete dependency graph
    Note over COS,Bundle: Dependency requests and configuration bounds are checked before Load
    opt Manifest includes a published schedule
        COS->>COS: Compare scheduled IDs, versions, and edges with resolved graph
    end
    loop Each requested root, traversing dependencies depth-first
        COS->>Config: TryGetConfiguration(runtimeId, bundleId, version)
        Config-->>COS: Optional override; dependency configuration remains fallback
        Note over COS,Bundle: Enter dependencies before invoking this bundle's Load
        COS->>Bundle: Load(loadContext) after its dependencies finish
    end
    COS->>COS: Create ArbitrationContext
    loop Up to configured maximum arbitration rounds
        COS->>Bundle: Arbitrate(context, zero-based round)
        Bundle-->>COS: Changed? (aggregated across loaded bundles)
        alt No bundle reports a change
            COS->>Assembly: Construct RuntimeAssembly
            Assembly-->>Host: Return successful composition
        else At least one bundle reports a change
            Note over COS: Continue while rounds remain
        end
    end
    COS-->>Host: Throw if every permitted round reports changes
```

## Failure boundaries

- **Before loading:** invalid input bounds, malformed/conflicting root requests, catalog misses, descriptor mismatches, invalid dependency requests, graph cycles, dependency/resource limits, or a supplied schedule that does not match the resolved graph reject composition before any bundle `Load` method is called.
- **During loading:** configuration and bundle-load failures propagate. Earlier bundle side effects are not transactionally rolled back.
- **During arbitration:** exceptions propagate; continued changes through the configured round limit produce a non-convergence exception. A `RuntimeAssembly` is returned only after a stable round.
- **After return:** FSM_COS hands the assembly to its caller. The host owns execution, presentation, scheduling, and application lifecycle; this sequence does not place those responsibilities inside the kernel.

## Interpretation cautions

- A supplied load plan/schedule is validated metadata; it does not itself implement repository localization or automatic deferred loading.
- The schedule, when present, is compared with the actual resolved dependency closure and edges.
- The `round` argument is zero-based. It is not interchangeable with the number of arbitration calls.
- This diagram describes ordering and boundaries, not timing, thread scheduling, or a live execution event stream.

## Evidence to maintain

When `FsmCos.Execute`, dependency resolution, loading, or arbitration changes, review this diagram against `src/FSM_COS/FsmCos.cs` and the relevant tests in `tests/FSM_COS.Tests/FsmCosTests.cs`, `RuntimeManifestPlanningTests.cs`, and `SnapshotIsolationTests.cs`. The exact candidate's CI run remains the authority for whether tests passed.
