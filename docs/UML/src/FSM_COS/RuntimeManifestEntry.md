![The Singularity Workshop animated moniker](https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Renderer/development/docs/assets/moniker.svg)

# RuntimeManifestEntry

**Source:** [`src/FSM_COS/RuntimeManifestEntry.cs`](../../../../src/FSM_COS/RuntimeManifestEntry.cs)  
**Source baseline:** `342c6fe71aee5ae9089547348c140a612e083248`  
**Source file blob:** `acd95c2ce7ff7f10ce4696d3ff3c11dec70837ad`  
**Authorship:** manually authored against current `development`; not GUI.SVG-generated.

## Purpose

Immutable published artifact identity plus the intended ManifestLoadStage for one runtime capability.

## UML class card

~~~mermaid
classDiagram
    class RuntimeManifestEntry {
        +Reference: MicroBundleReference
        +Stage: ManifestLoadStage
        +IsValid: bool
    }
    class MicroBundleReference {
        +BundleId: ulong
        +Version: string
        +ContentHash: string
        +IsValid: bool
        +Matches(entry) bool
    }
    class ManifestLoadStage {
        <<enumeration>>
        Bootstrap
        Resident
        Deferred
    }
    RuntimeManifestEntry *-- MicroBundleReference : published identity
    RuntimeManifestEntry --> ManifestLoadStage : intended stage
~~~

## Important boundaries

- `IsValid` validates the reference and enum value; it does not retrieve or hash-check artifact bytes.
- Stage values describe intended policy, not proof that the current composition engine performs deferred or bootstrap-only loading.

## Use cases

1. Pair a published `MicroBundleReference` with its intended `ManifestLoadStage`.
2. Validate a published entry before composition and scheduling decisions.
3. Do not interpret an entry's stage as proof that its artifact has been localized or loaded.




---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
