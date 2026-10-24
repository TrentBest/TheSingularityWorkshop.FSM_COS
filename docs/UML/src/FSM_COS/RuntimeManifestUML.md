<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# RuntimeManifest — UML neighborhood

**Target:** `TheSingularityWorkshop.FSM_COS.RuntimeManifest`  
**Evidence class:** manually authored, source-checked structural view derived from the existing type companion.  
**Source page:** [RuntimeManifest.md](RuntimeManifest.md)  
**Source declaration:** [RuntimeManifest.cs](../../../../src/FSM_COS/RuntimeManifest.cs)  
**Generation status:** not generated; no runtime trace is implied.

## Purpose

A machine-oriented request for a runtime composition. It carries the runtime identity, root MicroBundle/version requests, optional experience context, optional staged-load metadata, optional dependency schedule, and optional semantic intent.

## Target type model

~~~mermaid
classDiagram
    class RuntimeManifest {
        +RuntimeId: ulong
        +Bundles: IReadOnlyList~MicroBundleManifestEntry~
        +ExperienceContext: IStateContext
        +LoadPlan: IReadOnlyList~RuntimeManifestEntry~
        +Schedule: RuntimeManifestSchedule
        +Intent: SemanticIntent
        +Empty(runtimeId, context, intent)$ RuntimeManifest
        +ValidateStagedPlan() void
    }
    class MicroBundleManifestEntry
    class RuntimeManifestEntry
    class RuntimeManifestSchedule
    class IStateContext
    class SemanticIntent
    RuntimeManifest *-- "0..*" MicroBundleManifestEntry : root requests
    RuntimeManifest o-- "0..*" RuntimeManifestEntry : optional staged plan
    RuntimeManifest o-- "0..1" RuntimeManifestSchedule : optional schedule
    RuntimeManifest ..> IStateContext : optional context
    RuntimeManifest ..> SemanticIntent : optional intent
~~~

## Consumers, dependencies, and boundary

The relationships shown are limited to those documented in the linked type companion. This page does not claim a repository-wide caller scan. Consumers not represented in the documented model remain unknown until a compiler-backed source scan is performed. Package references and external types must be confirmed against project metadata and semantic symbols before being treated as exhaustive.

## Use cases

1. Request one or more root MicroBundles by ID and version.
2. Supply optional host-owned state context or semantic intent when the host has those values.
3. Attach schedule/load-plan metadata for validation or host planning without assuming that it activates deferred loading.

## Coverage notes

- Manually maintained and source-checked, not generated from Roslyn or reflection.
- Structural type relationships do not prove invocation order or runtime data flow.
- Sequence, activity/control-flow, and measured complexity evidence remain separate deliverables where applicable.

See [class neighborhood standard](../../Standard/CLASS_NEIGHBORHOODS.md), [traceability standard](../../Standard/TRACEABILITY.md), and the [coverage inventory](../../INVENTORY.md).

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
