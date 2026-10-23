# UML and Source Documentation Coverage Inventory

**Repository:** TheSingularityWorkshop.FSM_COS  
**Branch assessed:** `development`  
**Assessment basis:** GitHub source-tree and documentation-tree inventory; source pages are source-checked, not compiler-generated.  
**Status:** Initial manual inventory. This is not yet a CI-enforced or automatically regenerated report.

## Coverage summary

The current core project contains 19 C# source files plus its project file. The existing `docs/UML/src/FSM_COS/` directory has 16 per-type Markdown pages. The following four source files had no companion page at the time of this audit and are being added:

- [IMicroBundleRollback](src/FSM_COS/IMicroBundleRollback.md)
- [RuntimeCompositionResult](src/FSM_COS/RuntimeCompositionResult.md)
- [RuntimeExperienceHost](src/FSM_COS/RuntimeExperienceHost.md)
- [RuntimeExperienceRegistry](src/FSM_COS/RuntimeExperienceRegistry.md)

The existing documentation includes a separate class-diagram area and several SVGs, but the inventory below distinguishes existing artifacts from the full standard. Existing diagram files are not assumed to meet every new requirement without individual review.

## Applicable-view inventory

| View | Status | Notes |
|---|---|---|
| Per-type source documentation | **Partial / manual** | Existing pages plus the four additions above; validate all member details against source. |
| Target-centered class neighborhoods | **Partial / manual** | Existing `docs/UML/ClassDiagram/` diagrams; complete consumer/dependency/package coverage is not yet verified. |
| Companion `TypeNameUML.md` per declaration | **Partial / manual** | New companion pages are added for the four previously undocumented types. Full one-per-type compliance remains open. |
| Sequence diagrams | **Partial / manual** | Need source-linked normal, alternative, and failure paths for key composition workflows. |
| Activity/control flow | **Partial / manual** | Complex dependency resolution, load, and arbitration branches require systematic coverage. |
| State/lifecycle | **Partial / manual** | Runtime experience selection and load/rollback lifecycle need explicit views. |
| Use cases | **Partial / human-authored** | Existing product docs inform intent; dedicated categorized use-case files need further reconciliation. |
| Architecture/components | **Partial / manual** | Package and host/kernel boundaries are documented conceptually; source/project evidence needs a consolidated view. |
| Deployment/runtime topology | **Not applicable to core library runtime alone** | The library does not itself define a complete deployment topology. Host-specific deployments belong in integration documentation. |
| Data models and lineage | **Partial / manual** | Manifest, load-plan, schedule, result, and assembly types have pages; end-to-end data lineage needs a dedicated view. |
| MicroBundle/manifest inventory | **Partial / manual** | Core contracts and manifest types are documented; do not mistake contract types for concrete locally implemented domain MicroBundles. |
| Requirement/test traceability | **Partial / manual** | Existing tests/docs must be linked per workflow; no complete automated mapping verified. |
| Decision/constraint records | **Partial / manual** | Architecture and boundary docs exist; cross-links from each relevant diagram remain to be audited. |
| Complexity/performance evidence | **Planned / source-checked where stated** | No repository-wide static complexity analyzer or generated benchmark correlation is claimed. |
| Generated/stale-output CI gate | **Not implemented / not verified** | No claim of repository-wide diagram generation or freshness enforcement. |

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

1. Add and validate a companion UML neighborhood for every in-scope declaration.
2. Build source-backed sequence, activity, state, and data-lineage views for composition, failure/rollback, and experience selection.
3. Reconcile use cases with product intent and link each to source and tests.
4. Add a deterministic inventory/generation tool and CI freshness/coverage checks.
5. Report separate source, relationship, use-case, MicroBundle, and freshness coverage metrics.

This inventory is deliberately honest about partial coverage. It is not a statement that the full standard is already satisfied.
