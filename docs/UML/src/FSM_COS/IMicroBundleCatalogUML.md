<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# IMicroBundleCatalog — UML neighborhood

**Target:** `TheSingularityWorkshop.FSM_COS.IMicroBundleCatalog`  
**Evidence class:** manually authored, source-checked structural view derived from the existing type companion.  
**Source page:** [IMicroBundleCatalog.md](IMicroBundleCatalog.md)  
**Source declaration:** [IMicroBundleCatalog.cs](../../../../src/FSM_COS/IMicroBundleCatalog.cs)  
**Generation status:** not generated; no runtime trace is implied.

## Purpose

Domain adapter used by FSM_COS to resolve root bundles at requested versions and dependencies by identity.

## Target type model

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

## Consumers, dependencies, and boundary

The relationships shown in the class model above are limited to the relationships already documented in the linked type companion. This page does not claim a repository-wide caller scan. Consumers not represented by the documented model are unknown until a compiler-backed source scan is performed. Package references and external types must be confirmed against the project file and semantic symbols before being treated as exhaustive.

## Use cases

1. Resolve a manifest root by both bundle ID and the exact requested version.
2. Resolve a declared dependency by bundle ID and return the matching domain-owned MicroBundle instance.
3. Adapt an in-memory map, repository, or other host-owned source without moving storage policy into FSM_COS.

## Coverage notes

- The diagram is manually maintained and source-checked, not generated from Roslyn or reflection.
- Structural type relationships do not prove invocation order or runtime data flow.
- Sequence, activity/control-flow, and measured complexity evidence for this type remain separate deliverables where applicable.

See [class neighborhood standard](../../Standard/CLASS_NEIGHBORHOODS.md), [traceability standard](../../Standard/TRACEABILITY.md), and the [coverage inventory](../../INVENTORY.md).

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
