<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop — animated workshop moniker" width="720" /></a></p>

# Manifest Planning Metadata and Load-Plan State

FSM_COS includes contracts for describing a published dependency schedule and tracking host-reported loading state. These contracts are useful to a host that plans staged composition, but **they do not make `FsmCos.Execute` a complete staged-loading pipeline**.

![Manifest planning and load-plan concepts](assets/runtime-manifest-pipeline.svg)

## 01 — What are staged-manifest contracts?

### Responsibilities

- **`RuntimeManifest`** identifies the requested root MicroBundles and their requested versions. It can also carry optional schedule and load-plan metadata.
- **`RuntimeManifestEntry`** combines a published `MicroBundleReference` with its intended `ManifestLoadStage`.
- **`RuntimeManifestSchedule`** snapshots and validates the published dependency graph. It rejects invalid or duplicate entries and edges, checks dependency endpoints, detects cycles, and exposes dependency-readiness queries.
- **`RuntimeManifestLoadPlan`** tracks each known entry through `Published`, `Localized`, and `Loaded`. It does not fetch artifact bytes or load a MicroBundle.
- **`IManifestLoadEvaluator`** lets the host or Experience decide whether a localized deferred entry is wanted. Dependency readiness remains a separate check.

The schedule is dependency metadata, not the FSM_API process scheduler. The load plan is state tracking, not an artifact repository or execution engine.

## 02 — Why is staged metadata separate from execution?

### When to use these contracts

Use the schedule to validate and query a published dependency graph before composition. Use the load plan when a host needs an explicit record of localization and loading state. Use `FsmCos.Execute` for the current synchronous composition behavior.

For the request contract, see [Runtime Manifest](RUNTIME_MANIFEST.md). For a runnable consumer and catalog implementation, see [Consuming MicroBundles](CONSUMING_MICROBUNDLES.md).


---

## 03 — How do current plan and state transitions work?

### Current composition behavior

When `FsmCos.Execute` receives a manifest, it resolves the requested MicroBundles through the host-supplied catalog, validates the reachable dependency closure, and loads dependencies before dependents. If schedule metadata is supplied, FSM_COS checks it against the catalog-resolved graph before calling bundle `Load()` methods. It then runs bounded arbitration and returns a `RuntimeAssembly` only when a complete round reports no changes.

The optional stage labels do **not** change this execution path. In particular, the current package does not:

- perform repository localization or verify artifact content hashes;
- run a bootstrap-only composition while other bundles localize in the background;
- promote deferred bundles into an already-running `RuntimeAssembly`;
- refresh a running composition or replace its bundle generation.

These are not current FSM_COS APIs. A host may build lifecycle behavior around the exposed contracts, but must not treat stage metadata alone as implementation of that lifecycle.

### Load-plan state transitions

A new plan starts each entry in `Published`.

- `MarkLocalized(bundleId)` records that the host has localized the known entry.
- `MarkLoaded(bundleId)` requires the entry to be localized. If the plan has a schedule, its declared prerequisites must already be loaded.
- `EvaluatePromotions(...)` considers eligible localized entries in stage order. Deferred entries are offered to the supplied `IManifestLoadEvaluator`; the evaluator does not override dependency-readiness checks.
- Unknown entry IDs, invalid stage values, incompatible schedules, and dependency-state violations are rejected rather than silently accepted.

The plan and schedule expose read-only collection snapshots. This protects collection structure, not the mutability of domain-owned `IMicroBundle` objects.

## 04 — See it in a minute

Inspect [RuntimeManifestPlanningTests.cs](../tests/FSM_COS.Tests/RuntimeManifestPlanningTests.cs) to see which plan validation and state-transition behaviors are tested. These tests do not turn plan metadata into a deferred activation engine.

## 05 — Choose your next step

- [Runtime Manifest](RUNTIME_MANIFEST.md) — published request contract.
- [RuntimeAssembly](RUNTIME_ASSEMBLY.md) — successful handoff contract.
- [Composition Failures](COMPOSITION_FAILURES.md) — phase-aware failure behavior.

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
