# UC-FSMCOS-001 — Compose a runtime from requested MicroBundles

- **ID and title:** UC-FSMCOS-001 — Compose a runtime from requested MicroBundles
- **Category:** User Goals
- **Status:** Implemented; documentation manually source-reviewed
- **Actor/consumer and goal:** A host application requests a runtime containing specific versioned MicroBundles and needs either a composed `RuntimeAssembly` or an actionable composition failure.
- **Trigger and preconditions:** The host calls `IFsmCos.Execute` or `FsmCos.TryExecute` with a `RuntimeManifest`; the engine has an `IMicroBundleCatalog` capable of resolving the requested bundles. Optional configuration input is supplied through `IMicroBundleConfigurationSource`.

## Primary success path

1. The engine validates the manifest, configured size/count limits, root IDs, and requested versions.
2. Caller-owned manifest collections are snapshotted so later mutation does not alter the request mid-composition.
3. The engine resolves root bundles and validates their identities and dependency closure.
4. If a schedule is supplied, its entries, versions, and dependency edges are checked against the resolved graph.
5. The engine creates a load context and loads the requested roots and their dependencies in dependency order.
6. The engine arbitrates across loaded bundles until a round reports no changes, within the configured maximum round count.
7. The engine returns a `RuntimeAssembly` exposing a read-only snapshot of the loaded bundle order and the arbitration result.

## Alternative and failure paths

- **Invalid manifest or root request:** Reject before loading.
- **Missing or mismatched dependency:** Fail during dependency resolution, before bundle load callbacks.
- **Configuration, load, or arbitration callback throws:** Composition fails; attempted bundles are visited in reverse order for compensation where they implement `IMicroBundleRollback`.
- **Compensation callback also throws:** An `AggregateException` preserves the original composition exception and compensation failures.
- **Arbitration does not converge within the configured bound:** Fail; no non-converged assembly is returned.
- **Caller chooses `TryExecute`:** Return a `RuntimeCompositionResult` containing the failure phase and original exception instead of propagating the composition exception directly. This result contract does not itself guarantee rollback of every side effect.
- **Published/pre-resolved manifest build path:** When the repository-source conditional is enabled and the manifest is marked arbitration-resolved, a distinct flat-set path is used. Do not assume the ordinary authoring path describes that mode completely.

## Inputs, outputs, side effects, and boundaries

- **Inputs:** Runtime ID; versioned root entries; optional experience context; optional staged load plan and dependency schedule; optional semantic intent; optional configuration source.
- **Success output:** `RuntimeAssembly` with runtime ID, loaded bundles, arbitration rounds, and intent.
- **Failure output:** Exception from `Execute`, or `RuntimeCompositionResult` failure metadata from `TryExecute`.
- **Side effects:** Catalog resolution and bundle-defined configuration/load/arbitration/rollback callbacks may have side effects.
- **Boundary:** FSM_COS composes domain-owned MicroBundles. This use case does not claim to fetch or localize artifact bytes, automatically deploy a host, or activate a deferred bundle in an already-running assembly.

## Relevant API, types, and dependencies

- `IFsmCos`, `FsmCos`
- `RuntimeManifest`, `MicroBundleManifestEntry`, `RuntimeManifestSchedule`, `RuntimeManifestLoadPlan`
- `IMicroBundleCatalog`, `IMicroBundleConfigurationSource`, `MicroBundleLoadContext`
- `IMicroBundle` and optional `IMicroBundleRollback` contracts from MicroBundleDomain
- `RuntimeAssembly`, `RuntimeCompositionResult`, `RuntimeCompositionPhase`

## Tests and evidence

- [FsmCosTests.cs](../../../tests/FSM_COS.Tests/FsmCosTests.cs) — source of tests for successful composition, dependency-resolution failures, load/arbitration failures, compensation, configured bounds, assembly snapshots, and `TryExecute` phase reporting.
- [FsmCos source](../../../src/FSM_COS/FsmCos.cs)
- [Manifest source](../../../src/FSM_COS/RuntimeManifest.cs)
- [Composition result source](../../../src/FSM_COS/RuntimeCompositionResult.cs)
- [Sequence diagram and complexity notes](../../SequenceDiagrams/FsmCos-Execute.md)

## Known limitations and next step

The sequence and use case are manually authored and source-reviewed, not generated from Roslyn or observed at runtime. The ordinary authoring-composition path is documented here; published/pre-resolved execution, host-level experience selection, and generated per-declaration traceability need their own artifacts. Add a deterministic link/freshness gate before treating these documents as continuously verified.
