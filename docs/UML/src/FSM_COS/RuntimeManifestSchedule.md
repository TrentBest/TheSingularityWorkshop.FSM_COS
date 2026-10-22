![The Singularity Workshop animated moniker](https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Renderer/development/docs/assets/moniker.svg)

# RuntimeManifestSchedule

**Source:** [`src/FSM_COS/RuntimeManifestSchedule.cs`](../../../../src/FSM_COS/RuntimeManifestSchedule.cs)  
**Source baseline:** `bf1ce2194eb9a5e5e3667d1fe3d23ff7e7ff71d3`  
**Source file blob:** `28f1c71f6a1f85eb177147740c4a15a1052d5cd8`  
**Authorship:** manually authored against current `development`; not GUI.SVG-generated.

## Purpose

Publish-time dependency analysis for a runtime manifest. It snapshots and bounds entry/edge inputs, validates identity and dependency endpoints, rejects duplicate identities/edges and cycles, and exposes dependency ordering metadata.

## UML class card

~~~mermaid
classDiagram
    class RuntimeManifestSchedule {
        +DefaultMaximumEntryCount$
        +DefaultMaximumDependencyCount$
        +Entries: IReadOnlyList~RuntimeManifestEntry~
        +Dependencies: IReadOnlyList~RuntimeManifestDependency~
        +IndependentEntries: IReadOnlyList~RuntimeManifestEntry~
        +DependencyEntries: IReadOnlyList~RuntimeManifestEntry~
        +DependenciesOf(bundleId) IReadOnlyList~RuntimeManifestDependency~
        +IsDependencyReady(bundleId, isLoaded) bool
    }
    class RuntimeManifestEntry
    class RuntimeManifestDependency
    class MicroBundleReference
    RuntimeManifestSchedule *-- "0..*" RuntimeManifestEntry : snapshots
    RuntimeManifestSchedule *-- "0..*" RuntimeManifestDependency : dependency edges
    RuntimeManifestEntry *-- MicroBundleReference : published identity
~~~

## Important boundaries

- This is publish-time dependency analysis, not a process scheduler.
- It does not fetch or localize artifacts and does not execute staged loading.
- Resource limits bound input enumeration; schedule validity does not prove bundle semantics are compatible.

## Use cases

1. Validate a published schedule's entries and dependency edges before composition.
2. Query which dependencies belong to an entry or whether the host reports them ready.
3. Use schedule validation as a consistency check, not as a replacement for the FSM_API process scheduler or a deferred-loading engine.




---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
