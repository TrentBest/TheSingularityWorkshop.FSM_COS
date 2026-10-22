![The Singularity Workshop animated moniker](https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Renderer/development/docs/assets/moniker.svg)

# SnapshotIsolationTests

**Source:** [`tests/FSM_COS.Tests/SnapshotIsolationTests.cs`](../../../../tests/FSM_COS.Tests/SnapshotIsolationTests.cs)  
**Source baseline:** `6c43f39a2be1465360abb03ed27d577da42c1ab1`  
**Source file blob:** `4b878f7c350f764f1cb5ae9d8153065b5d79b8ac`  
**Authorship:** manually authored against current `development`; not generated from reflection or GUI.SVG.

## Purpose

Focused tests proving that runtime schedules and load plans isolate their internal state from caller-owned mutable collections and expose read-only views.

## UML class card

~~~mermaid
classDiagram
    class SnapshotIsolationTests {
        +Schedule_snapshots_all_input_and_derived_collections() void
        +Load_plan_snapshots_input_entries() void
        +Schedule_derived_collections_reject_mutation_through_list_interfaces() void
    }
    class RuntimeManifestSchedule
    class RuntimeManifestLoadPlan
    class RuntimeManifestEntry
    class RuntimeManifestDependency
    SnapshotIsolationTests ..> RuntimeManifestSchedule : checks defensive snapshots
    SnapshotIsolationTests ..> RuntimeManifestLoadPlan : checks defensive snapshots
    RuntimeManifestSchedule *-- RuntimeManifestEntry : copied entries
    RuntimeManifestSchedule *-- RuntimeManifestDependency : copied edges
~~~

## Coverage

- Clearing the original input lists does not mutate schedule/load-plan snapshots.
- Derived collections remain stable after construction.
- Read-only interface casts reject attempted mutation.

## Use cases

1. Mutate caller-owned source collections after constructing a schedule or plan and verify the saved snapshot does not change.
2. Attempt to mutate public collection views and verify they remain read-only.
3. Keep the guarantee scoped to collection structure; snapshot isolation does not make domain-owned MicroBundle objects immutable.




---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
