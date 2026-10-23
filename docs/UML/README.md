<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# FSM_COS Type and UML Reference

This reference connects the public runtime model to its source companions and the UML diagrams currently maintained for FSM_COS. The companions and diagrams are manually authored; they are not automatically generated from the C# source. For exact signatures and behavior, the linked source and tests remain authoritative.

## UML standards and coverage

- [UML coverage inventory](INVENTORY.md) — manual source/type inventory, current view status, and explicit remaining gaps.
- [UML standard reconciliation plan](RECONCILIATION_PLAN.md) — phased implementation, preservation/migration policy, core workflow priorities, and evidence gates.

- [Actionable artifact catalog](Standard/ARTIFACT_CATALOG.md) — concrete view inventory, coverage expectations, evidence states, and completion gate.

- [Comprehensive UML Standard](Standard/README.md) — the focused specification for each UML/documentation view, including sequence complexity analysis and MicroBundle/manifest inventories.
- [Living UML Visual Style](../UML_VISUAL_STYLE.md) — recursive data-card appearance, target-centered layout, provenance, and FSM_COS-specific boundaries.
- [Shared Documentation Standard](../../DOCUMENTATION_STANDARD.md) — cross-repository policy and evidence requirements.

The standard defines the intended coverage model. This index is still a manually maintained reference: do not infer complete source coverage, automatic generation, or CI stale-output enforcement from the existence of these documents.

## Workflow diagrams, lifecycle, and use cases

- [SEQ-FSMCOS-001 — Runtime composition sequence](SequenceDiagrams/FsmCos-Execute.md) — source-reviewed ordinary composition path, failure compensation, and static complexity notes.
- [SEQ-FSMCOS-002 — Published/pre-resolved composition](SequenceDiagrams/FsmCos-Published-PreResolved.md) — conditional flat-set restore path, dependency-order checks, configuration verification, and recorded arbitration round.
- [SEQ-FSMCOS-003 — Runtime experience selection](SequenceDiagrams/RuntimeExperienceHost-Selection.md) — compose-before-activation, registry retention, failed selection, removal, and lifecycle boundaries.
- [STATE-FSMCOS-001 — Runtime experience registry](StateLifecycle/RuntimeExperienceRegistry.md) — registry membership and host-selected identity states, invariants, rejected transitions, synchronization, and ownership boundaries.
- [UC-FSMCOS-001 — Compose a runtime](UseCases/User%20Goals/UC-FSMCOS-001-Compose-Runtime.md) — host goal, success path, failure paths, boundaries, and test evidence.
- [UC-FSMCOS-002 — Manage runtime experiences](UseCases/User%20Goals/UC-FSMCOS-002-Manage-Runtime-Experiences.md) — prepare and retain a replacement before selecting it, plus failure/removal boundaries.

## Composition entry point

| Type | Responsibility | Reference |
|---|---|---|
| `FsmCos` | Resolves and validates the dependency closure, loads bundles in dependency order, and arbitrates toward convergence. | [Consumer-first UML + use cases](ClassDiagram/FsmCosUML.md) · [Diagram SVG](ClassDiagram/FsmCosUML.svg) · [Source-detail view](ClassDiagram/FsmCos.md) · [Source](../../src/FSM_COS/FsmCos.cs) |
| `IFsmCos` | Public composition entry-point contract. | [Type companion](src/FSM_COS/IFsmCos.md) · [UML neighborhood](src/FSM_COS/IFsmCosUML.md) · [Source](../../src/FSM_COS/IFsmCos.cs) |
| `ArbitrationContext` | Context passed to MicroBundle arbitration. | [Type companion](src/FSM_COS/ArbitrationContext.md) · [UML neighborhood](src/FSM_COS/ArbitrationContextUML.md) · [Source](../../src/FSM_COS/ArbitrationContext.cs) |
| `IMicroBundleCatalog` | Consumer-owned resolver for requested MicroBundles. | [Type companion](src/FSM_COS/IMicroBundleCatalog.md) · [UML neighborhood](src/FSM_COS/IMicroBundleCatalogUML.md) · [Source](../../src/FSM_COS/IMicroBundleCatalog.cs) |
| `IMicroBundleConfigurationSource` | Optional source of per-bundle configuration. | [Type companion](src/FSM_COS/IMicroBundleConfigurationSource.md) · [UML neighborhood](src/FSM_COS/IMicroBundleConfigurationSourceUML.md) · [Source](../../src/FSM_COS/IMicroBundleConfigurationSource.cs) |

## Manifest and staged-load model

| Type | Responsibility | Reference |
|---|---|---|
| `RuntimeManifest` | Request for MicroBundle identities and versions. | [Type companion](src/FSM_COS/RuntimeManifest.md) · [UML neighborhood](src/FSM_COS/RuntimeManifestUML.md) · [Source](../../src/FSM_COS/RuntimeManifest.cs) |
| `MicroBundleManifestEntry` | Versioned identity for a requested manifest root. | [Type companion](src/FSM_COS/MicroBundleManifestEntry.md) · [UML neighborhood](src/FSM_COS/MicroBundleManifestEntryUML.md) · [Source](../../src/FSM_COS/MicroBundleManifestEntry.cs) |
| `MicroBundleReference` | Published MicroBundle identity, including version and content hash. | [Type companion](src/FSM_COS/MicroBundleReference.md) · [UML neighborhood](src/FSM_COS/MicroBundleReferenceUML.md) · [Source](../../src/FSM_COS/MicroBundleReference.cs) |
| `RuntimeManifestEntry` | Published identity and intended load stage for one capability. | [Type companion](src/FSM_COS/RuntimeManifestEntry.md) · [UML neighborhood](src/FSM_COS/RuntimeManifestEntryUML.md) · [Source](../../src/FSM_COS/RuntimeManifestEntry.cs) |
| `RuntimeManifestDependency` | Dependency edge between published bundle identities. | [Type companion](src/FSM_COS/RuntimeManifestDependency.md) · [UML neighborhood](src/FSM_COS/RuntimeManifestDependencyUML.md) · [Source](../../src/FSM_COS/RuntimeManifestDependency.cs) |
| `RuntimeManifestSchedule` | Validated dependency graph and dependency-readiness metadata. | [Consumer-first UML + use cases](ClassDiagram/RuntimeManifestScheduleUML.md) · [Diagram SVG](ClassDiagram/RuntimeManifestScheduleUML.svg) · [Source-detail view](ClassDiagram/RuntimeManifestSchedule.md) · [Source](../../src/FSM_COS/RuntimeManifestSchedule.cs) |
| `RuntimeManifestLoadPlan` | Tracks published entries through host-reported localization and loading states. | [Type companion](src/FSM_COS/RuntimeManifestLoadPlan.md) · [UML neighborhood](src/FSM_COS/RuntimeManifestLoadPlanUML.md) · [Source](../../src/FSM_COS/RuntimeManifestLoadPlan.cs) |
| `MicroBundleRuntimeState` | State values used by the runtime manifest load plan. | [Source](../../src/FSM_COS/RuntimeManifestLoadPlan.cs) |
| `ManifestLoadStage` | Declares the intended stage for a manifest entry. | [Type companion](src/FSM_COS/ManifestLoadStage.md) · [UML neighborhood](src/FSM_COS/ManifestLoadStageUML.md) · [Source](../../src/FSM_COS/ManifestLoadStage.cs) |
| `IManifestLoadEvaluator` | Lets the host/experience decide whether a deferred entry is wanted. | [Type companion](src/FSM_COS/IManifestLoadEvaluator.md) · [UML neighborhood](src/FSM_COS/IManifestLoadEvaluatorUML.md) · [Source](../../src/FSM_COS/IManifestLoadEvaluator.cs) |
| `MicroBundleLoadContext` | Context supplied when a MicroBundle is loaded. | [Type companion](src/FSM_COS/MicroBundleLoadContext.md) · [UML neighborhood](src/FSM_COS/MicroBundleLoadContextUML.md) · [Source](../../src/FSM_COS/MicroBundleLoadContext.cs) |

## Composition result and runtime experiences

| Type | Responsibility | Reference |
|---|---|---|
| `RuntimeAssembly` | Read-only snapshot of the composed bundle order and arbitration result handed to the host. | [Type companion](src/FSM_COS/RuntimeAssembly.md) · [UML neighborhood](src/FSM_COS/RuntimeAssemblyUML.md) · [Source](../../src/FSM_COS/RuntimeAssembly.cs) |
| `RuntimeCompositionResult` | Non-throwing outcome with an assembly or phase/exception failure details. | [Type companion](src/FSM_COS/RuntimeCompositionResult.md) · [UML neighborhood](src/FSM_COS/RuntimeCompositionResultUML.md) · [Source](../../src/FSM_COS/RuntimeCompositionResult.cs) |
| `RuntimeExperienceHost` | Coordinates composition, registration, and explicit selection. | [Type companion](src/FSM_COS/RuntimeExperienceHost.md) · [UML neighborhood](src/FSM_COS/RuntimeExperienceHostUML.md) · [Source](../../src/FSM_COS/RuntimeExperienceHost.cs) |
| `RuntimeExperienceRegistry` | Thread-safe retention and selection metadata for composed assemblies. | [Type companion](src/FSM_COS/RuntimeExperienceRegistry.md) · [UML neighborhood](src/FSM_COS/RuntimeExperienceRegistryUML.md) · [Source](../../src/FSM_COS/RuntimeExperienceRegistry.cs) |

## Optional rollback contract

- `IMicroBundleRollback` — [Type companion](src/FSM_COS/IMicroBundleRollback.md) · [UML neighborhood](src/FSM_COS/IMicroBundleRollbackUML.md) · [Source](../../src/FSM_COS/IMicroBundleRollback.cs)

## Test companions

These references summarize selected test scenarios. The test source is authoritative for the complete current test set.

- [FsmCosTests](tests/FSM_COS.Tests/FsmCosTests.md) — composition, dependency resolution, failure handling, limits, and arbitration.
- [RuntimeManifestPlanningTests](tests/FSM_COS.Tests/RuntimeManifestPlanningTests.md) — manifest validation, dependency schedules, staged-load state, and promotion evaluation.
- [SnapshotIsolationTests](tests/FSM_COS.Tests/SnapshotIsolationTests.md) — defensive snapshots and read-only collection behavior.
- [AiGuiCapabilityHandoffTests](tests/FSM_COS.Tests/AiGuiCapabilityHandoffTests.md) — proof that AI/GUI capabilities can be composed without adding AI or GUI package dependencies to the FSM_COS core.

## Scope and known gaps

The consumer-first `FsmCosUML` view puts the host above the focal type, labels its supplied collaborators, and shows `RuntimeAssembly` below. The consumer-first `RuntimeManifestScheduleUML` view places the publisher above the focal type, shows graph inputs at the sides, and connects to the downstream load plan. Both focused companions include consumer use cases. Four previously undocumented source files now have type pages and UML neighborhoods. The wider standard is **not yet fully applied**: individual UML companions for all declarations, sequence/activity/state/data-lineage coverage, comprehensive test traceability, and generated freshness/coverage checks remain open. Three manually authored sequence diagrams now cover ordinary composition, the conditional published/pre-resolved path, and runtime experience selection/retention. One manually authored lifecycle diagram now covers registry membership and selection metadata. Two human-authored user-goal use cases cover composition and experience switching. No tests or CI were run for these documentation changes. There are no claims of automated diagram generation or CI enforcement. The schedule and load-plan types describe metadata and state transitions; they do not by themselves implement repository localization, background loading, or promotion of deferred bundles into a running `RuntimeAssembly`. See [Staged Manifest Loading](../STAGED_MANIFEST_LOADING.md) for that boundary.

---

## 🔗 Resources & Support

- [FSM_COS](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS) · [FSM_API](https://github.com/TrentBest/FSM_API) · [FSM_Serialization](https://github.com/TrentBest/TheSingularityWorkshop.FSM_Serialization) · [WebPage](https://github.com/TrentBest/WebPage)
- [GitHub](https://github.com/TrentBest) · [Coder Legion](https://coderlegion.com/) · [Profile](https://coderlegion.com/user/The%2BSingularity%2BWorkshop) · [Patreon](https://www.patreon.com/c/TheSingularityWorkshop) · [PayPal](https://www.paypal.com/donate/?hosted_button_id=3Z7263LCQMV9J)

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
