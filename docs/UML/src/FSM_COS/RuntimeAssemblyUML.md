# RuntimeAssembly — UML neighborhood

**Target:** `TheSingularityWorkshop.FSM_COS.RuntimeAssembly`  
**Evidence class:** manually authored, source-checked structural view derived from the existing type companion.  
**Source page:** [RuntimeAssembly.md](RuntimeAssembly.md)  
**Source declaration:** [RuntimeAssembly.cs](../../../../src/FSM_COS/RuntimeAssembly.cs)  
**Generation status:** not generated; no runtime trace is implied.

## Purpose

The immutable result model returned after successful composition. It exposes the runtime identity, assembled MicroBundles, arbitration-round count, optional semantic intent, and bundle lookup helpers.

## Target type model

~~~mermaid
classDiagram
    class RuntimeAssembly {
        +RuntimeId: ulong
        +Bundles: IReadOnlyList~IMicroBundle~
        +LoadedBundles: IReadOnlyList~IMicroBundle~
        +ArbitrationRounds: int
        +Intent: SemanticIntent
        +TryGetBundle(bundleId, bundle) bool
        +TryGetBundle~TBundle~(bundleId, bundle) bool
        +TryGetBundle~TBundle~(bundle) bool
    }
    class IMicroBundle
    class SemanticIntent
    RuntimeAssembly o-- "0..*" IMicroBundle : assembled result
    RuntimeAssembly ..> SemanticIntent : optional request intent
~~~

## Consumers, dependencies, and boundary

The relationships shown are limited to those documented in the linked type companion. This page does not claim a repository-wide caller scan. Consumers not represented in the documented model remain unknown until a compiler-backed source scan is performed. Package references and external types must be confirmed against project metadata and semantic symbols before being treated as exhaustive.

## Use cases

1. Retrieve an already composed MicroBundle by ID or a host-known bundle type.
2. Inspect the runtime identity and converged arbitration-round index when diagnosing composition.
3. Treat the collection views as read-only snapshots; the assembly is not a live replacement or retirement manager.

## Coverage notes

- Manually maintained and source-checked, not generated from Roslyn or reflection.
- Structural type relationships do not prove invocation order or runtime data flow.
- Sequence, activity/control-flow, and measured complexity evidence remain separate deliverables where applicable.

See [class neighborhood standard](../../Standard/CLASS_NEIGHBORHOODS.md), [traceability standard](../../Standard/TRACEABILITY.md), and the [coverage inventory](../../INVENTORY.md).
