# ArbitrationContext — UML neighborhood

**Target:** `TheSingularityWorkshop.FSM_COS.ArbitrationContext`  
**Evidence class:** manually authored, source-checked structural view derived from the existing type companion.  
**Source page:** [ArbitrationContext.md](ArbitrationContext.md)  
**Source declaration:** [ArbitrationContext.cs](../../../../src/FSM_COS/ArbitrationContext.cs)  
**Generation status:** not generated; no runtime trace is implied.

## Purpose

FSM_COS adapter implementing the domain-owned arbitration context with runtime identity, current bundle set, and optional Experience state context.

## Target type model

~~~mermaid
classDiagram
    class ArbitrationContext {
        +RuntimeId: ulong
        +ExperienceContext: IStateContext
        +Bundles: IReadOnlyList~IMicroBundle~
        +LoadedBundles: IReadOnlyList~IMicroBundle~
    }
    class IMicroBundleArbitrationContext
    class IMicroBundle
    ArbitrationContext ..|> IMicroBundleArbitrationContext
    ArbitrationContext o-- "0..*" IMicroBundle : current composition set
~~~

## Consumers, dependencies, and boundary

The relationships shown in the class model above are limited to the relationships already documented in the linked type companion. This page does not claim a repository-wide caller scan. Consumers not represented by the documented model are unknown until a compiler-backed source scan is performed. Package references and external types must be confirmed against the project file and semantic symbols before being treated as exhaustive.

## Use cases

1. Read the runtime ID, loaded bundle set, and optional Experience context while implementing `IMicroBundle.Arbitrate`.
2. Keep arbitration context limited to composition-time information; application lifecycle and presentation remain host-owned.

## Coverage notes

- The diagram is manually maintained and source-checked, not generated from Roslyn or reflection.
- Structural type relationships do not prove invocation order or runtime data flow.
- Sequence, activity/control-flow, and measured complexity evidence for this type remain separate deliverables where applicable.

See [class neighborhood standard](../../Standard/CLASS_NEIGHBORHOODS.md), [traceability standard](../../Standard/TRACEABILITY.md), and the [coverage inventory](../../INVENTORY.md).
