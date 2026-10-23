# SEQ-FSMCOS-002 — Compose a published, pre-resolved runtime

**Evidence type:** Manual sequence diagram reviewed against `FsmCos.cs`, `RuntimeManifest.cs`, and `RuntimeManifestEntry.cs` on `development`.  
**Build condition:** This path is compiled only with `WORKSHOP_USE_REPOSITORY_SOURCES`.  
**Status:** Source-reviewed; tests and CI were not run for this documentation change. The diagram is not generated or captured at runtime.

## Intent and preconditions

Publication has already resolved arbitration and emitted a flattened manifest: the root-entry collection and load plan both enumerate the complete closure, the schedule records the dependency graph, and `PublishedArbitrationRounds` records the publisher's convergence round. At runtime, FSM_COS validates and restores this result rather than discovering dependencies recursively or rerunning arbitration.

## Sequence

```mermaid
sequenceDiagram
    autonumber
    participant Host as Host / publisher
    participant Engine as FsmCos
    participant Manifest as RuntimeManifest
    participant Catalog as IMicroBundleCatalog
    participant Config as IMicroBundleConfigurationSource
    participant Bundle as Resolved MicroBundles
    participant Context as MicroBundleLoadContext
    participant Assembly as RuntimeAssembly

    Host->>Engine: Execute(manifest, configurationSource)
    Engine->>Manifest: Snapshot request and ValidateStagedPlan()
    Note over Engine,Manifest: ArbitrationResolved requires LoadPlan, Schedule, and PublishedArbitrationRounds.
    Engine->>Engine: Validate limits and root ID/version requests
    loop Each flattened manifest entry
        Engine->>Catalog: TryResolve(bundleId, version)
        Catalog-->>Engine: Bundle instance or unresolved
        Engine->>Engine: Validate resolved identity
    end
    Engine->>Engine: ResolvePublishedFlatSet(roots)
    Note over Engine: Check descriptor/dependency identities and require every dependency in the flat set; no recursive catalog discovery.
    Engine->>Engine: Validate schedule against resolved graph
    Engine->>Context: Create runtime load context
    alt Missing configuration source
        Engine-->>Host: Fail before published loading
    else Configuration source supplied
        loop Each entry in published LoadPlan order
            Engine->>Engine: Verify all prerequisites already loaded
            Engine->>Config: TryGetConfiguration(runtimeId, bundleId, version)
            Config-->>Engine: Resolved configuration bytes or missing
            Engine->>Engine: Snapshot and enforce byte limit
            Engine->>Context: SetConfiguration(bundleId, bytes)
            Engine->>Bundle: Load(context)
            Bundle-->>Engine: Load returns or throws
        end
        Engine->>Engine: Validate published arbitration round against configured bound
        Note over Engine: Does not call Arbitrate() on this path.
        loop Each loaded bundle
            Engine->>Bundle: ExportResolvedConfiguration()
            Bundle-->>Engine: Exported configuration bytes
            Engine->>Engine: Compare exact bytes with supplied configuration
        end
        Engine->>Assembly: Construct immutable assembly snapshot
        Assembly-->>Host: RuntimeAssembly with recorded round and configurations
    end
```

## Failure and compensation

- Manifest validation, root resolution, flat-set validation, and schedule validation occur before bundle loading.
- The published load plan must place every dependency before its dependent. A violation fails the composition.
- Missing configuration source, missing configuration for a bundle, invalid configuration size, load failure, or resolved-configuration mismatch fails composition.
- Once loading has begun, attempted bundles are visited in reverse attempt order for compensation when they implement `IMicroBundleRollback`. Compensation failures are aggregated with the original composition exception.
- The `TryExecute` wrapper reports the current composition phase and exception; it does not promise that arbitrary external side effects can be undone.

## Key invariants and complexity

- **Flat closure:** every dependency declared by each resolved bundle must be represented in the published set. This mode does not recursively query the catalog for missing dependencies.
- **Plan integrity:** schedule entry IDs, versions, and dependency edges must match the resolved bundles.
- **Dependency order:** before loading bundle (b), each dependency of (b) must already be in the loaded-ID set.
- **Configuration integrity:** supplied configuration is snapshotted, total bytes are bounded, and the exported resolved bytes must exactly match the supplied bytes for every loaded bundle.
- **Arbitration integrity:** the publisher's recorded round is reused; the runtime does not rerun bundle arbitration. The round must be less than the configured maximum.
- **Expected work:** for (V) flattened bundles, (E) dependency edges, and (B) total configuration bytes, validation/loading/configuration processing is approximately (O(V + E + B)), excluding bundle callback costs. Exact runtime complexity has not been benchmarked by this document.

## Boundaries

This path restores a published plan; it does not fetch or localize artifact bytes, authenticate a publisher, independently prove the publisher's arbitration process, deploy the host, or activate deferred entries in an already-running assembly. Those require separate contracts and evidence.

## Source and test references

- [FsmCos.cs](../../src/FSM_COS/FsmCos.cs) — conditional flat-set resolution, published-plan loading, round reuse, configuration verification, compensation.
- [RuntimeManifest.cs](../../src/FSM_COS/RuntimeManifest.cs) — required published metadata and complete-flat-closure consistency.
- [RuntimeManifestEntry.cs](../../src/FSM_COS/RuntimeManifestEntry.cs) — published identity and stage entry.
- [FsmCosTests.cs](../../tests/FSM_COS.Tests/FsmCosTests.cs) — source of composition and validation tests; this documentation change did not run tests.
- [Reconciliation plan](../RECONCILIATION_PLAN.md) — broader coverage and automation gates.
