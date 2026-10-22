![The Singularity Workshop animated moniker](https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Renderer/development/docs/assets/moniker.svg)

# RuntimeManifestLoadPlan

**Source:** [`src/FSM_COS/RuntimeManifestLoadPlan.cs`](../../../../src/FSM_COS/RuntimeManifestLoadPlan.cs)  
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
        +EvaluatePromotions(evaluator, experienceContext, schedule) IEnumerable~RuntimeManifestEntry~
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
- Promotion evaluation requires an IManifestLoadEvaluator and optional experience context, and yields eligible entries; it does not itself load artifacts or schedule an FSM_API process.

## Use cases

1. Track a host-reported manifest entry from `Published` through `Localized` to `Loaded`.
2. Inspect or update plan state when the host has performed the corresponding operation.
3. Do not treat the plan as an artifact downloader or a method that calls `IMicroBundle.Load`.




---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
