![The Singularity Workshop animated moniker](https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Renderer/development/docs/assets/moniker.svg)

# RuntimeManifestLoadPlan

**Source:** [`src/FSM_COS/RuntimeManifestLoadPlan.cs`](../../../src/FSM_COS/RuntimeManifestLoadPlan.cs)  
**Source baseline:** `bf1ce2194eb9a5e5e3667d1fe3d23ff7e7ff71d3`  
**Source file blob:** `484a0882e792bc75e9e766b5368691fba561d618`  
**Authorship:** manually authored against current `development`; not GUI.SVG-generated.

## Purpose

Tracks localization/composition availability for published manifest entries. The host or repository adapter performs localization and actual loading, then reports successful transitions to the plan.

## UML class card

~~~mermaid
classDiagram
    class RuntimeManifestLoadPlan {
        +Entries: IReadOnlyList~RuntimeManifestEntry~
        +GetState(bundleId) MicroBundleRuntimeState
        +MarkLocalized(bundleId) void
        +MarkLoaded(bundleId) void
        +EvaluatePromotions() IReadOnlyList~ulong~
    }
    class RuntimeManifestSchedule
    class RuntimeManifestEntry
    class MicroBundleRuntimeState {
        <<enumeration>>
        Published
        Localized
        Loaded
    }
    RuntimeManifestLoadPlan *-- "0..*" RuntimeManifestEntry : immutable entry snapshot
    RuntimeManifestLoadPlan o-- RuntimeManifestSchedule : optional dependency metadata
    RuntimeManifestLoadPlan ..> MicroBundleRuntimeState : tracks per-entry state
~~~

## Important boundaries

- Constructing the plan does not localize or load anything.
- A host/repository adapter owns those operations and reports successful transitions.
- Promotion evaluation is metadata/state evaluation; it does not schedule an FSM_API process or automatically execute a deferred stage.

## Use cases

1. Validate and snapshot published dependency metadata before a host attempts composition.
2. Inspect dependency/state metadata without confusing it with artifact localization or execution.
3. Test invalid identity, dependency, and transition inputs within documented resource bounds.

## Verification status

Source-reviewed against the recorded baseline. Mermaid rendering, automated link checking, and exact-HEAD CI have not been verified.
