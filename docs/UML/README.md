<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# FSM_COS Type and UML Reference

This reference connects the public runtime model to its source companions and the UML diagrams currently maintained for FSM_COS. The companions and diagrams are manually authored; they are not automatically generated from the C# source. For exact signatures and behavior, the linked source and tests remain authoritative.

## Composition entry point

| Type | Responsibility | Reference |
|---|---|---|
| `FsmCos` | Resolves and validates the dependency closure, loads bundles in dependency order, and arbitrates toward convergence. | [Class diagram](ClassDiagram/FsmCos.svg) · [Class reference](ClassDiagram/FsmCos.md) · [Source](../../src/FSM_COS/FsmCos.cs) |
| `IFsmCos` | Public composition entry-point contract. | [Type companion](src/FSM_COS/IFsmCos.md) · [Source](../../src/FSM_COS/IFsmCos.cs) |
| `ArbitrationContext` | Context passed to MicroBundle arbitration. | [Type companion](src/FSM_COS/ArbitrationContext.md) · [Source](../../src/FSM_COS/ArbitrationContext.cs) |
| `IMicroBundleCatalog` | Consumer-owned resolver for requested MicroBundles. | [Type companion](src/FSM_COS/IMicroBundleCatalog.md) · [Source](../../src/FSM_COS/IMicroBundleCatalog.cs) |
| `IMicroBundleConfigurationSource` | Optional source of per-bundle configuration. | [Type companion](src/FSM_COS/IMicroBundleConfigurationSource.md) · [Source](../../src/FSM_COS/IMicroBundleConfigurationSource.cs) |

## Manifest and staged-load model

| Type | Responsibility | Reference |
|---|---|---|
| `RuntimeManifest` | Request for MicroBundle identities and versions. | [Type companion](src/FSM_COS/RuntimeManifest.md) · [Source](../../src/FSM_COS/RuntimeManifest.cs) |
| `MicroBundleManifestEntry` | Versioned identity for a requested manifest root. | [Type companion](src/FSM_COS/MicroBundleManifestEntry.md) · [Source](../../src/FSM_COS/MicroBundleManifestEntry.cs) |
| `MicroBundleReference` | Published MicroBundle identity, including version and content hash. | [Type companion](src/FSM_COS/MicroBundleReference.md) · [Source](../../src/FSM_COS/MicroBundleReference.cs) |
| `RuntimeManifestEntry` | Published identity and intended load stage for one capability. | [Type companion](src/FSM_COS/RuntimeManifestEntry.md) · [Source](../../src/FSM_COS/RuntimeManifestEntry.cs) |
| `RuntimeManifestDependency` | Dependency edge between published bundle identities. | [Type companion](src/FSM_COS/RuntimeManifestDependency.md) · [Source](../../src/FSM_COS/RuntimeManifestDependency.cs) |
| `RuntimeManifestSchedule` | Validated dependency graph and dependency-readiness metadata. | [Class diagram](ClassDiagram/RuntimeManifestSchedule.svg) · [Class reference](ClassDiagram/RuntimeManifestSchedule.md) · [Source](../../src/FSM_COS/RuntimeManifestSchedule.cs) |
| `RuntimeManifestLoadPlan` | Tracks published entries through host-reported localization and loading states. | [Type companion](src/FSM_COS/RuntimeManifestLoadPlan.md) · [Source](../../src/FSM_COS/RuntimeManifestLoadPlan.cs) |
| `MicroBundleRuntimeState` | State values used by the runtime manifest load plan. | [Source](../../src/FSM_COS/RuntimeManifestLoadPlan.cs) |
| `ManifestLoadStage` | Declares the intended stage for a manifest entry. | [Type companion](src/FSM_COS/ManifestLoadStage.md) · [Source](../../src/FSM_COS/ManifestLoadStage.cs) |
| `IManifestLoadEvaluator` | Lets the host/experience decide whether a deferred entry is wanted. | [Type companion](src/FSM_COS/IManifestLoadEvaluator.md) · [Source](../../src/FSM_COS/IManifestLoadEvaluator.cs) |
| `MicroBundleLoadContext` | Context supplied when a MicroBundle is loaded. | [Type companion](src/FSM_COS/MicroBundleLoadContext.md) · [Source](../../src/FSM_COS/MicroBundleLoadContext.cs) |

## Composition result

| Type | Responsibility | Reference |
|---|---|---|
| `RuntimeAssembly` | Read-only snapshot of the composed bundle order and arbitration result handed to the host. | [Type companion](src/FSM_COS/RuntimeAssembly.md) · [Source](../../src/FSM_COS/RuntimeAssembly.cs) |

## Test companions

These references summarize selected test scenarios. The test source is authoritative for the complete current test set.

- [FsmCosTests](tests/FSM_COS.Tests/FsmCosTests.md) — composition, dependency resolution, failure handling, limits, and arbitration.
- [RuntimeManifestPlanningTests](tests/FSM_COS.Tests/RuntimeManifestPlanningTests.md) — manifest validation, dependency schedules, staged-load state, and promotion evaluation.
- [SnapshotIsolationTests](tests/FSM_COS.Tests/SnapshotIsolationTests.md) — defensive snapshots and read-only collection behavior.
- [AiGuiCapabilityHandoffTests](tests/FSM_COS.Tests/AiGuiCapabilityHandoffTests.md) — proof that AI/GUI capabilities can be composed without adding AI or GUI package dependencies to the FSM_COS core.

## Scope of the diagrams

The two SVG class diagrams focus on `FsmCos` and `RuntimeManifestSchedule`. The remaining types have source-linked companions, but do not yet have individual SVG class diagrams. The schedule and load-plan types describe metadata and state transitions; they do not by themselves implement repository localization, background loading, or promotion of deferred bundles into a running `RuntimeAssembly`. See [Staged Manifest Loading](../STAGED_MANIFEST_LOADING.md) for that boundary.


---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
