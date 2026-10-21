# Source-to-UML Map

This folder is the source-grounded index for FSM_COS type diagrams. It is deliberately an inventory first, not a claim that every type already has a generated diagram.

## Evidence snapshot

- Repository: `TrentBest/TheSingularityWorkshop.FSM_COS`
- Branch inspected: `development` (the current working branch, not the published line)
- Source subtree inspected: `src/FSM_COS/`
- Source tree commit at the time of this update: `e1eeb62147091801332d0b7ef97259b0fb60e4c8`
- Inspection scope: the complete Git tree returned for the project source directory; it contained 16 C# files and one project file.
- Verification boundary: this is a GitHub source-tree inventory. It is not a build, test run, generator run, browser rendering check, or package inspection.

Because development is live, this inventory must be refreshed whenever source changes. A diagram is not current merely because it exists.

## Reading the status labels

- **Inventory confirmed** — file and declared type were observed in the source tree/content at the evidence snapshot above.
- **Diagram not yet inventoried here** — this index does not yet assert a matching UML artifact exists or is correct.
- **Needs source review** — relationships, visibility, and behavior should be checked against the implementation before drawing connectors.
- **Generated/manual** — record this only after the artifact's production method is known; do not infer it from appearance.

## Source-file to declared-type inventory

| Source file | Declared type(s) observed | Primary responsibility (summary) | UML status in this index |
|---|---|---|---|
| [FsmCos.cs](../../src/FSM_COS/FsmCos.cs) | `FsmCos`, nested `LoadFrame` | Composition orchestration and internal load traversal | [Class diagram](ClassDiagram/FsmCos.svg) and [detail](ClassDiagram/FsmCos.md) added; source review documented, visual/browser validation pending |
| [IFsmCos.cs](../../src/FSM_COS/IFsmCos.cs) | `IFsmCos` | Public composition entry-point abstraction | Diagram not yet inventoried here |
| [ArbitrationContext.cs](../../src/FSM_COS/ArbitrationContext.cs) | `ArbitrationContext` | Context passed through arbitration | Diagram not yet inventoried here |
| [IManifestLoadEvaluator.cs](../../src/FSM_COS/IManifestLoadEvaluator.cs) | `IManifestLoadEvaluator` | Experience-owned decision boundary for promoting deferred manifest entries | Diagram not yet inventoried here |
| [IMicroBundleCatalog.cs](../../src/FSM_COS/IMicroBundleCatalog.cs) | `IMicroBundleCatalog` | Consumer-owned MicroBundle resolution boundary | Diagram not yet inventoried here |
| [IMicroBundleConfigurationSource.cs](../../src/FSM_COS/IMicroBundleConfigurationSource.cs) | `IMicroBundleConfigurationSource` | Optional per-bundle configuration source | Diagram not yet inventoried here |
| [ManifestLoadStage.cs](../../src/FSM_COS/ManifestLoadStage.cs) | `ManifestLoadStage` | Intended entry stage for a manifest entry | Diagram not yet inventoried here |
| [MicroBundleLoadContext.cs](../../src/FSM_COS/MicroBundleLoadContext.cs) | `MicroBundleLoadContext` | Context supplied during bundle loading | Diagram not yet inventoried here |
| [MicroBundleManifestEntry.cs](../../src/FSM_COS/MicroBundleManifestEntry.cs) | `MicroBundleManifestEntry` | Versioned manifest root identity | Diagram not yet inventoried here |
| [MicroBundleReference.cs](../../src/FSM_COS/MicroBundleReference.cs) | `MicroBundleReference` | Immutable published artifact identity | Diagram not yet inventoried here |
| [RuntimeAssembly.cs](../../src/FSM_COS/RuntimeAssembly.cs) | `RuntimeAssembly` | Stable result of runtime composition | Diagram not yet inventoried here |
| [RuntimeManifest.cs](../../src/FSM_COS/RuntimeManifest.cs) | `RuntimeManifest` | Machine-oriented request for bundle identities and versions | Diagram not yet inventoried here |
| [RuntimeManifestDependency.cs](../../src/FSM_COS/RuntimeManifestDependency.cs) | `RuntimeManifestDependency` | Published dependency edge between bundle IDs | Diagram not yet inventoried here |
| [RuntimeManifestEntry.cs](../../src/FSM_COS/RuntimeManifestEntry.cs) | `RuntimeManifestEntry` | Immutable identity and intended stage for a runtime capability | Diagram not yet inventoried here |
| [RuntimeManifestLoadPlan.cs](../../src/FSM_COS/RuntimeManifestLoadPlan.cs) | `RuntimeManifestLoadPlan`, `MicroBundleRuntimeState` | Availability/composition state tracking | Diagram not yet inventoried here; two declared types in one file |
| [RuntimeManifestSchedule.cs](../../src/FSM_COS/RuntimeManifestSchedule.cs) | `RuntimeManifestSchedule` | Publish-time dependency analysis for a runtime manifest | Diagram not yet inventoried here |

The inventory intentionally records multiple declared types in a single source file where observed. It does not assume one file equals one class. It also does not infer inheritance, associations, or call relationships from names alone.

## Diagram conventions

The intended visual language is:

- **Thick blue outer border:** namespace/data card, labelled with the fully qualified namespace.
- **Colorful, readable type cards:** distinguish classes, interfaces, structs/records, and enums by a documented legend; color is not a substitute for labels.
- **Source-derived members only:** show signatures that exist in the reviewed source; omit imagined members.
- **Grounded relationships only:** every connector must be supported by declarations or implementation. A visual proximity is not a relationship.
- **No decorative connector rails masquerading as dependencies:** any stub or continuation rail must be labelled as such.
- **Accessible fallback:** include a text explanation and source links; never make color the sole way to distinguish type kinds or states.

## First detailed diagram: FsmCos

The first manually authored, source-grounded class view is [FsmCos.svg](ClassDiagram/FsmCos.svg), with its explanatory [detail page](ClassDiagram/FsmCos.md). It covers the public composition contract, catalog field, manifest/configuration inputs, result type, and private nested traversal frame. Its connector legend distinguishes implementation/field relationships from method input/output flow. The diagram is not generator-produced, and SVG rendering/accessibility checks have not yet been run.

## Second detailed diagram: manifest schedule and load plan

[RuntimeManifestSchedule.svg](ClassDiagram/RuntimeManifestSchedule.svg) and its [detail page](ClassDiagram/RuntimeManifestSchedule.md) document the distinction between publish-time dependency analysis and host-reported runtime availability. Related records and the Experience-owned deferred-load evaluator are included. The diagram is manually authored; SVG rendering/accessibility checks and build/test verification remain pending.

## Coverage still required

This file does not yet provide a full AST diagram set, per-type class diagrams, collaboration diagrams, or use-case diagrams. Those should be added in separate, reviewable assets after each diagram's source relationships have been checked. The next useful slice is `FsmCos` plus its directly consumed contracts, followed by manifest/load-plan types.

Before calling this automated or release-grade, add checks for source-file inventory, declared-type inventory (including nested/partial types), stale source revisions, missing/orphan diagrams, SVG/XML validity, accessible text, and generated-diff review. No such automation is claimed by this index.


## Current source companion

- [FsmCos](src/FSM_COS/FsmCos.md) — source-reviewed companion for current development baseline `326a021498156f2bb7dc7a31219d6118d3427e37`; diagram rendering and link checks remain unverified.

- [RuntimeManifest](src/FSM_COS/RuntimeManifest.md) — source-reviewed companion for the current development baseline.
- [RuntimeAssembly](src/FSM_COS/RuntimeAssembly.md) — source-reviewed companion for the current development baseline.

- [RuntimeManifestSchedule](src/FSM_COS/RuntimeManifestSchedule.md) — current-source dependency schedule contract.
- [RuntimeManifestLoadPlan](src/FSM_COS/RuntimeManifestLoadPlan.md) — current-source localization/composition state tracker.
- [RuntimeManifestEntry](src/FSM_COS/RuntimeManifestEntry.md) — current-source companion documenting the type's identity and responsibility boundaries.
- [MicroBundleReference](src/FSM_COS/MicroBundleReference.md) — current-source companion documenting the type's identity and responsibility boundaries.
- [RuntimeManifestDependency](src/FSM_COS/RuntimeManifestDependency.md) — current-source companion documenting the type's identity and responsibility boundaries.

- [IManifestLoadEvaluator](src/FSM_COS/IManifestLoadEvaluator.md) — current-source companion for the composition contract.
- [IMicroBundleCatalog](src/FSM_COS/IMicroBundleCatalog.md) — current-source companion for the composition contract.
- [IMicroBundleConfigurationSource](src/FSM_COS/IMicroBundleConfigurationSource.md) — current-source companion for the composition contract.
- [ManifestLoadStage](src/FSM_COS/ManifestLoadStage.md) — current-source companion for the composition contract.
- [MicroBundleManifestEntry](src/FSM_COS/MicroBundleManifestEntry.md) — current-source companion for the composition contract.
- [MicroBundleLoadContext](src/FSM_COS/MicroBundleLoadContext.md) — current-source companion for the composition contract.
- [IFsmCos](src/FSM_COS/IFsmCos.md) — current-source companion for the composition contract.
- [ArbitrationContext](src/FSM_COS/ArbitrationContext.md) — current-source companion for the composition contract.
