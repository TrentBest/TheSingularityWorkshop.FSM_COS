# UML and Source Documentation Coverage Inventory

**Repository:** TheSingularityWorkshop.FSM_COS  
**Branch assessed:** `development`  
**Assessment basis:** GitHub source-tree and documentation-tree inventory; source pages are source-checked, not compiler-generated.  
**Status:** Manual coverage inventory with a new CI-enforced file-level companion-presence check. The report itself is not automatically regenerated; compiler-backed declaration coverage and diagram freshness enforcement remain unimplemented. The implementation sequence is tracked in the [UML standard reconciliation plan](RECONCILIATION_PLAN.md).

## Coverage summary

The current core project contains 20 C# source files plus its project file. The `docs/UML/src/FSM_COS/` directory now has companion Markdown pages for all 20 source files, including individual UML neighborhood companions for declarations without a dedicated focused SVG. The four initially missing source companions have now been added:

- [IMicroBundleRollback](src/FSM_COS/IMicroBundleRollback.md) · [UML](src/FSM_COS/IMicroBundleRollbackUML.md)
- [RuntimeCompositionResult](src/FSM_COS/RuntimeCompositionResult.md) · [UML](src/FSM_COS/RuntimeCompositionResultUML.md)
- [RuntimeExperienceHost](src/FSM_COS/RuntimeExperienceHost.md) · [UML](src/FSM_COS/RuntimeExperienceHostUML.md)
- [RuntimeExperienceRegistry](src/FSM_COS/RuntimeExperienceRegistry.md) · [UML](src/FSM_COS/RuntimeExperienceRegistryUML.md)

The existing documentation includes a separate class-diagram area and several SVGs, but the inventory below distinguishes existing artifacts from the full standard. Existing diagram files are not assumed to meet every new requirement without individual review.

## Applicable-view inventory

| View | Status | Notes |
|---|---|---|
| Per-type source documentation | **20/20 files have a companion page; manual** | All 20 current C# source files have a companion page. Keep signatures/member details validated against source as code changes. |
| Target-centered class neighborhoods | **Partial / manual** | Existing `docs/UML/ClassDiagram/` diagrams; complete consumer/dependency/package coverage is not yet verified. |
| Companion `TypeNameUML.md` per declaration | **20/20 source files represented; manual** | Individual UML neighborhood pages now exist for all declared source files; `FsmCos` and `RuntimeManifestSchedule` use the focused `docs/UML/ClassDiagram/*UML.md` companions. Diagrams are authored, not generated. |
| Sequence diagrams | **Partial / manual** | Added source-reviewed `SEQ-FSMCOS-001` for ordinary runtime composition, `SEQ-FSMCOS-002` for the conditional published/pre-resolved path, and `SEQ-FSMCOS-003` for experience composition/registration/selection/removal. All three are manual diagrams; published-path coverage is limited to the `WORKSHOP_USE_REPOSITORY_SOURCES` build condition, and other workflows remain open. |
| Activity/control flow | **Partial / manual** | Complex dependency resolution, load, and arbitration branches require systematic coverage. |
| State/lifecycle | **Partial / manual** | Added `STATE-FSMCOS-001` for registry membership/selection metadata; load/rollback lifecycle and other stateful workflows still need explicit views. |
| Use cases | **Partial / human-authored** | Added `UC-FSMCOS-001` (compose runtime) and `UC-FSMCOS-002` (prepare/switch runtime experiences) under `UseCases/User Goals`; other categories and workflows remain open. |
| Architecture/components | **Partial / manual** | Package and host/kernel boundaries are documented conceptually; source/project evidence needs a consolidated view. |
| Deployment/runtime topology | **Not applicable to core library runtime alone** | The library does not itself define a complete deployment topology. Host-specific deployments belong in integration documentation. |
| Data models and lineage | **Partial / manual** | Manifest, load-plan, schedule, result, and assembly types have pages; end-to-end data lineage needs a dedicated view. |
| MicroBundle/manifest inventory | **Partial / manual** | Core contracts and manifest types are documented; do not mistake contract types for concrete locally implemented domain MicroBundles. |
| Requirement/test traceability | **Partial / manual** | Existing tests/docs must be linked per workflow; no complete automated mapping verified. |
| Decision/constraint records | **Partial / manual** | Architecture and boundary docs exist; cross-links from each relevant diagram remain to be audited. |
| Complexity/performance evidence | **Planned / source-checked where stated** | No repository-wide static complexity analyzer or generated benchmark correlation is claimed. |
| UML file-level companion CI gate | **Implemented; workflow execution pending** | `scripts/verify_uml_file_coverage.py` checks every `src/FSM_COS/**/*.cs` file has a source companion and UML-neighborhood page. This is file-level presence only, not compiler-backed declaration coverage. |

## Source declaration inventory

| Source file | Companion page |
|---|---|
| `ArbitrationContext.cs` | [ArbitrationContext.md](src/FSM_COS/ArbitrationContext.md) |
| `FsmCos.cs` | [FsmCos.md](src/FSM_COS/FsmCos.md) |
| `IFsmCos.cs` | [IFsmCos.md](src/FSM_COS/IFsmCos.md) |
| `IManifestLoadEvaluator.cs` | [IManifestLoadEvaluator.md](src/FSM_COS/IManifestLoadEvaluator.md) |
| `IMicroBundleCatalog.cs` | [IMicroBundleCatalog.md](src/FSM_COS/IMicroBundleCatalog.md) |
| `IMicroBundleConfigurationSource.cs` | [IMicroBundleConfigurationSource.md](src/FSM_COS/IMicroBundleConfigurationSource.md) |
| `IMicroBundleRollback.cs` | [IMicroBundleRollback.md](src/FSM_COS/IMicroBundleRollback.md) |
| `ManifestLoadStage.cs` | [ManifestLoadStage.md](src/FSM_COS/ManifestLoadStage.md) |
| `MicroBundleLoadContext.cs` | [MicroBundleLoadContext.md](src/FSM_COS/MicroBundleLoadContext.md) |
| `MicroBundleManifestEntry.cs` | [MicroBundleManifestEntry.md](src/FSM_COS/MicroBundleManifestEntry.md) |
| `MicroBundleReference.cs` | [MicroBundleReference.md](src/FSM_COS/MicroBundleReference.md) |
| `RuntimeAssembly.cs` | [RuntimeAssembly.md](src/FSM_COS/RuntimeAssembly.md) |
| `RuntimeCompositionResult.cs` | [RuntimeCompositionResult.md](src/FSM_COS/RuntimeCompositionResult.md) |
| `RuntimeExperienceHost.cs` | [RuntimeExperienceHost.md](src/FSM_COS/RuntimeExperienceHost.md) |
| `RuntimeExperienceRegistry.cs` | [RuntimeExperienceRegistry.md](src/FSM_COS/RuntimeExperienceRegistry.md) |
| `RuntimeManifest.cs` | [RuntimeManifest.md](src/FSM_COS/RuntimeManifest.md) |
| `RuntimeManifestDependency.cs` | [RuntimeManifestDependency.md](src/FSM_COS/RuntimeManifestDependency.md) |
| `RuntimeManifestEntry.cs` | [RuntimeManifestEntry.md](src/FSM_COS/RuntimeManifestEntry.md) |
| `RuntimeManifestLoadPlan.cs` | [RuntimeManifestLoadPlan.md](src/FSM_COS/RuntimeManifestLoadPlan.md) |
| `RuntimeManifestSchedule.cs` | [RuntimeManifestSchedule.md](src/FSM_COS/RuntimeManifestSchedule.md) |

## MicroBundle classification rule

The core project defines **contracts and manifest/data types** used to describe, discover, configure, load, arbitrate, and report MicroBundles. These types are not automatically concrete MicroBundles. A concrete locally defined MicroBundle must be identified from an actual implementation of the domain contract, not inferred from a name, manifest entry, test fixture, or interface. External contracts, test doubles, examples, and unresolved references must be categorized separately.

## Next completion gates

1. Review all 20 authored UML neighborhoods against current source and validate links/member relationships; presence does not imply exhaustive caller coverage.
2. Extend source-backed sequence, activity, state, and data-lineage views beyond the initial ordinary-composition sequence; cover failure/rollback and remaining host workflows more deeply; ordinary composition, published/pre-resolved composition, and runtime experience selection now have dedicated manual sequence diagrams.
3. Reconcile use cases with product intent and link each to source and tests.
4. Add a deterministic inventory/generation tool and CI freshness/coverage checks.
5. Report separate source, relationship, use-case, MicroBundle, and freshness coverage metrics.

This inventory is deliberately honest about partial coverage. It is not a statement that the full standard is already satisfied.
