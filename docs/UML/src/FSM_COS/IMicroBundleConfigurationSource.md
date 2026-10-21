![The Singularity Workshop animated moniker](https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Renderer/development/docs/assets/moniker.svg)

# IMicroBundleConfigurationSource

**Source:** [`src/FSM_COS/IMicroBundleConfigurationSource.cs`](../../../src/FSM_COS/IMicroBundleConfigurationSource.cs)  
**Source baseline:** `30e437100d1f3b49d07111e87f76cb4811ba8465`  
**Source file blob:** `eaad15c340817391e41bfb3d5e6dc9028e193e5a`  
**Authorship:** manually authored against current `development`; not GUI.SVG-generated.

## Purpose

Optional source of configuration bytes scoped by runtime ID, bundle ID, and resolved version.

## UML class card

~~~mermaid
classDiagram
    class IMicroBundleConfigurationSource {
        <<interface>>
        +TryGetConfiguration(runtimeId, bundleId, version, configuration) bool
    }
    class MicroBundleLoadContext
    IMicroBundleConfigurationSource ..> MicroBundleLoadContext : supplies bytes consumed by composition
~~~

## Important boundaries

- The source returns configuration bytes; it does not define how a bundle interprets them.
- Repository location, localization, and bundle loading remain separate responsibilities.

## Use cases

1. Keep composition contracts explicit at the boundary where they are owned.
2. Let tests inject adapters or contexts without confusing metadata, policy, and loading behavior.

## Verification status

Source-reviewed against the recorded baseline. Mermaid rendering, automated link checking, and exact-HEAD CI have not been verified.
