# ManifestLoadStage — UML neighborhood

**Target:** `TheSingularityWorkshop.FSM_COS.ManifestLoadStage`  
**Evidence class:** manually authored, source-checked structural view derived from the existing type companion.  
**Source page:** [ManifestLoadStage.md](ManifestLoadStage.md)  
**Source declaration:** [ManifestLoadStage.cs](../../../../src/FSM_COS/ManifestLoadStage.cs)  
**Generation status:** not generated; no runtime trace is implied.

## Purpose

Enum labels the intended role of a published entry: Bootstrap, Resident, or Deferred.

## Target type model

~~~mermaid
classDiagram
    class ManifestLoadStage {
        <<enumeration>>
        Bootstrap
        Resident
        Deferred
    }
    RuntimeManifestEntry --> ManifestLoadStage : intended composition stage
~~~

## Consumers, dependencies, and boundary

The relationships shown in the class model above are limited to the relationships already documented in the linked type companion. This page does not claim a repository-wide caller scan. Consumers not represented by the documented model are unknown until a compiler-backed source scan is performed. Package references and external types must be confirmed against the project file and semantic symbols before being treated as exhaustive.

## Use cases

1. Label a published manifest entry as `Bootstrap`, `Resident`, or `Deferred` for host-owned planning.
2. Read the stage as descriptive metadata only; a stage value does not activate localization or deferred execution in the current FSM_COS composition path.

## Coverage notes

- The diagram is manually maintained and source-checked, not generated from Roslyn or reflection.
- Structural type relationships do not prove invocation order or runtime data flow.
- Sequence, activity/control-flow, and measured complexity evidence for this type remain separate deliverables where applicable.

See [class neighborhood standard](../../Standard/CLASS_NEIGHBORHOODS.md), [traceability standard](../../Standard/TRACEABILITY.md), and the [coverage inventory](../../INVENTORY.md).
