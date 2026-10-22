![The Singularity Workshop animated moniker](https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Renderer/development/docs/assets/moniker.svg)

# IFsmCos

**Source:** [`src/FSM_COS/IFsmCos.cs`](../../../../src/FSM_COS/IFsmCos.cs)  
**Source baseline:** `30e437100d1f3b49d07111e87f76cb4811ba8465`  
**Source file blob:** `6b3d18419609e364d00918b8ef632e443c93a31f`  
**Authorship:** manually authored against current `development`; not GUI.SVG-generated.

## Purpose

Public composition abstraction that executes a runtime manifest and optionally accepts an external configuration source.

## UML class card

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

## Important boundaries

- The interface defines composition entry; catalog resolution and bounded arbitration are implementation responsibilities.
- It does not promise rollback or arbitrary semantic compatibility.

## Use cases

1. Depend on `IFsmCos` when host code needs the composition operation without coupling callers to the concrete `FsmCos` implementation.
2. Pass a `RuntimeManifest` and, when needed, an `IMicroBundleConfigurationSource` to `Execute`.
3. Substitute a test implementation at a host boundary when testing orchestration separately from the real composition engine.




---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
