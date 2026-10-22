![The Singularity Workshop animated moniker](https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Renderer/development/docs/assets/moniker.svg)

# IMicroBundleCatalog

**Source:** [`src/FSM_COS/IMicroBundleCatalog.cs`](../../../src/FSM_COS/IMicroBundleCatalog.cs)  
**Source baseline:** `30e437100d1f3b49d07111e87f76cb4811ba8465`  
**Source file blob:** `54925e70584fa414abc7c86669f8beb738004d6d`  
**Authorship:** manually authored against current `development`; not GUI.SVG-generated.

## Purpose

Domain adapter used by FSM_COS to resolve root bundles at requested versions and dependencies by identity.

## UML class card

~~~mermaid
classDiagram
    class IMicroBundleCatalog {
        <<interface>>
        +TryResolve(bundleId, version, bundle) bool
        +TryResolve(bundleId, bundle) bool
    }
    class IMicroBundle
    IMicroBundleCatalog ..> IMicroBundle : resolves domain-owned bundle
~~~

## Important boundaries

- The MicroBundleDomain owns the bundle contract and semantics.
- The catalog is a resolution boundary, not the composition engine or artifact cache policy.

## Use cases

1. Resolve a manifest root by both bundle ID and the exact requested version.
2. Resolve a declared dependency by bundle ID and return the matching domain-owned MicroBundle instance.
3. Adapt an in-memory map, repository, or other host-owned source without moving storage policy into FSM_COS.




---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
