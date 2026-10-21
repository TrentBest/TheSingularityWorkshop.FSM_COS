![The Singularity Workshop animated moniker](https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Renderer/development/docs/assets/moniker.svg)

# MicroBundleLoadContext

**Source:** [`src/FSM_COS/MicroBundleLoadContext.cs`](../../../src/FSM_COS/MicroBundleLoadContext.cs)  
**Source baseline:** `30e437100d1f3b49d07111e87f76cb4811ba8465`  
**Source file blob:** `4ede9c138d0fd5fca0638b135565e2724b283b62`  
**Authorship:** manually authored against current `development`; not GUI.SVG-generated.

## Purpose

FSM_COS implementation of the domain-owned load-context contract, exposing runtime identity and per-bundle configuration bytes.

## UML class card

~~~mermaid
classDiagram
    class MicroBundleLoadContext {
        +RuntimeId: ulong
        +Configuration: IReadOnlyDictionary~ulong, ReadOnlyMemory~byte~~
        +TryGetConfiguration(bundleId, configuration) bool
    }
    class IMicroBundleLoadContext
    MicroBundleLoadContext ..|> IMicroBundleLoadContext
~~~

## Important boundaries

- Construction and mutation are internal to FSM_COS composition.
- The context carries configuration; it does not localize artifacts or load bundles.

## Use cases

1. Keep composition contracts explicit at the boundary where they are owned.
2. Let tests inject adapters or contexts without confusing metadata, policy, and loading behavior.

## Verification status

Source-reviewed against the recorded baseline. Mermaid rendering, automated link checking, and exact-HEAD CI have not been verified.
