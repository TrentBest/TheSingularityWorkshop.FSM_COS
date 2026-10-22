![The Singularity Workshop animated moniker](https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Renderer/development/docs/assets/moniker.svg)

# ManifestLoadStage

**Source:** [`src/FSM_COS/ManifestLoadStage.cs`](../../../../src/FSM_COS/ManifestLoadStage.cs)  
**Source baseline:** `30e437100d1f3b49d07111e87f76cb4811ba8465`  
**Source file blob:** `e7313fb890ac15cc1f61648056edb885ac856a0f`  
**Authorship:** manually authored against current `development`; not GUI.SVG-generated.

## Purpose

Enum labels the intended role of a published entry: Bootstrap, Resident, or Deferred.

## UML class card

~~~mermaid
classDiagram
    class ManifestLoadStage {
        <<enumeration>>
        Bootstrap
        Resident
        Deferred
    }
    RuntimeManifestEntry --> ManifestLoadStage : intended composition stage
~~~

## Important boundaries

- Stage is metadata, not an execution guarantee.
- Current composition does not implement bootstrap-only loading, stage-scheduled loading, or automatic deferred promotion.

## Use cases

1. Label a published manifest entry as `Bootstrap`, `Resident`, or `Deferred` for host-owned planning.
2. Read the stage as descriptive metadata only; a stage value does not activate localization or deferred execution in the current FSM_COS composition path.




---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
