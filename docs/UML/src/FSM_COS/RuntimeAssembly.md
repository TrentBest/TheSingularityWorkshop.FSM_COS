![The Singularity Workshop animated moniker](https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Renderer/development/docs/assets/moniker.svg)

# RuntimeAssembly

**Source:** [`src/FSM_COS/RuntimeAssembly.cs`](../../../../src/FSM_COS/RuntimeAssembly.cs)  
**Source baseline:** `9a5d9b72c42f1221e800926b09640d6e00f751ac`  
**Source file blob:** `6ef0c80aacce694fc513a7afaea76309d79fe6a5`  
**Authorship:** manually authored and checked against current `development`; not GUI.SVG-generated.

## Purpose

The immutable result model returned after successful composition. It exposes the runtime identity, assembled MicroBundles, arbitration-round count, optional semantic intent, and bundle lookup helpers.

## UML class card

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

The diagram abbreviates nullable annotations and generic constraints. The C# source is authoritative.

## Important boundaries

- Construction is internal; the composition engine owns creation of the result.
- The bundle list is the assembled result snapshot; lookup helpers do not trigger loading.
- The type is not a UI renderer, process scheduler, repository/localization service, or rollback boundary.
- A returned assembly reports what composition produced; it does not prove arbitrary runtime semantics are mutually compatible.

## Use cases

1. Retrieve an already composed MicroBundle by ID or a host-known bundle type.
2. Inspect the runtime identity and converged arbitration-round index when diagnosing composition.
3. Treat the collection views as read-only snapshots; the assembly is not a live replacement or retirement manager.




---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
