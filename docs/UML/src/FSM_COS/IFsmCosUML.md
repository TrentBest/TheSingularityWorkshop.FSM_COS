<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# IFsmCos — UML neighborhood

**Target:** `TheSingularityWorkshop.FSM_COS.IFsmCos`  
**Evidence class:** manually authored, source-checked structural view derived from the existing type companion.  
**Source page:** [IFsmCos.md](IFsmCos.md)  
**Source declaration:** [IFsmCos.cs](../../../../src/FSM_COS/IFsmCos.cs)  
**Generation status:** not generated; no runtime trace is implied.

## Purpose

Public composition abstraction that executes a runtime manifest and optionally accepts an external configuration source.

## Target type model

~~~mermaid
classDiagram
    class IFsmCos {
        <<interface>>
        +Execute(manifest, configurationSource) RuntimeAssembly
    }
    class FsmCos
    class RuntimeManifest
    class RuntimeAssembly
    IFsmCos <|.. FsmCos : implemented by
    IFsmCos ..> RuntimeManifest : consumes
    IFsmCos ..> RuntimeAssembly : returns
~~~

## Consumers, dependencies, and boundary

The relationships shown in the class model above are limited to the relationships already documented in the linked type companion. This page does not claim a repository-wide caller scan. Consumers not represented by the documented model are unknown until a compiler-backed source scan is performed. Package references and external types must be confirmed against the project file and semantic symbols before being treated as exhaustive.

## Use cases

1. Depend on `IFsmCos` when host code needs the composition operation without coupling callers to the concrete `FsmCos` implementation.
2. Pass a `RuntimeManifest` and, when needed, an `IMicroBundleConfigurationSource` to `Execute`.
3. Substitute a test implementation at a host boundary when testing orchestration separately from the real composition engine.

## Coverage notes

- The diagram is manually maintained and source-checked, not generated from Roslyn or reflection.
- Structural type relationships do not prove invocation order or runtime data flow.
- Sequence, activity/control-flow, and measured complexity evidence for this type remain separate deliverables where applicable.

See [class neighborhood standard](../../Standard/CLASS_NEIGHBORHOODS.md), [traceability standard](../../Standard/TRACEABILITY.md), and the [coverage inventory](../../INVENTORY.md).

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
