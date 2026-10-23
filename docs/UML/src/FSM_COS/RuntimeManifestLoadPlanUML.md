# RuntimeManifestLoadPlan — UML neighborhood

**Target:** `TheSingularityWorkshop.FSM_COS.RuntimeManifestLoadPlan`  
**Evidence class:** manually authored, source-checked structural view derived from the existing type companion.  
**Source page:** [RuntimeManifestLoadPlan.md](RuntimeManifestLoadPlan.md)  
**Source declaration:** [RuntimeManifestLoadPlan.cs](../../../../src/FSM_COS/RuntimeManifestLoadPlan.cs)  
**Generation status:** not generated; no runtime trace is implied.

## Purpose

Tracks localization/composition availability for published manifest entries. The host or repository adapter performs localization and actual loading, then reports successful transitions to the plan.

## Target type model

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

## Consumers, dependencies, and boundary

Relationships are limited to those documented in the linked type companion. This page does not claim a repository-wide caller scan. Consumers not represented in the documented model remain unknown until a compiler-backed source scan is performed. Package references and external types must be confirmed against project metadata and semantic symbols before being treated as exhaustive.

## Use cases

1. Track a host-reported manifest entry from `Published` through `Localized` to `Loaded`.
2. Inspect or update plan state when the host has performed the corresponding operation.
3. Do not treat the plan as an artifact downloader or a method that calls `IMicroBundle.Load`.

## Coverage notes

- Manually maintained and source-checked, not generated from Roslyn or reflection.
- Structural type relationships do not prove invocation order or runtime data flow.
- Sequence, activity/control-flow, and measured complexity evidence remain separate deliverables where applicable.

See [class neighborhood standard](../../Standard/CLASS_NEIGHBORHOODS.md), [traceability standard](../../Standard/TRACEABILITY.md), and the [coverage inventory](../../INVENTORY.md).
