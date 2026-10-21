![The Singularity Workshop animated moniker](https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Renderer/development/docs/assets/moniker.svg)

# SnapshotIsolationTests

**Source:** [`tests/FSM_COS.Tests/SnapshotIsolationTests.cs`](../../../tests/FSM_COS.Tests/SnapshotIsolationTests.cs)  
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

1. Read the tests as executable evidence of the stated contracts and failure boundaries.
2. Use the test names as navigation to the precise assertion in the linked source file.
3. Re-run the test project on the exact commit before treating the evidence as current CI validation.

## Verification status

The test source was inspected and the documented `[Fact]` count was counted from the linked file. Mermaid rendering, automated links, and exact-HEAD CI are not yet verified by this companion.
