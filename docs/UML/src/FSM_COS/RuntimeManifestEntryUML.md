<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# RuntimeManifestEntry — UML neighborhood

**Target:** `TheSingularityWorkshop.FSM_COS.RuntimeManifestEntry`  
**Evidence class:** manually authored, source-checked structural view derived from the existing type companion.  
**Source page:** [RuntimeManifestEntry.md](RuntimeManifestEntry.md)  
**Source declaration:** [RuntimeManifestEntry.cs](../../../../src/FSM_COS/RuntimeManifestEntry.cs)  
**Generation status:** not generated; no runtime trace is implied.

## Purpose

Immutable published artifact identity plus the intended ManifestLoadStage for one runtime capability.

## Target type model

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

## Consumers, dependencies, and boundary

Relationships are limited to those documented in the linked type companion. This page does not claim a repository-wide caller scan. Consumers not represented in the documented model remain unknown until a compiler-backed source scan is performed. Package references and external types must be confirmed against project metadata and semantic symbols before being treated as exhaustive.

## Use cases

1. Pair a published `MicroBundleReference` with its intended `ManifestLoadStage`.
2. Validate a published entry before composition and scheduling decisions.
3. Do not interpret an entry's stage as proof that its artifact has been localized or loaded.

## Coverage notes

- Manually maintained and source-checked, not generated from Roslyn or reflection.
- Structural type relationships do not prove invocation order or runtime data flow.
- Sequence, activity/control-flow, and measured complexity evidence remain separate deliverables where applicable.

See [class neighborhood standard](../../Standard/CLASS_NEIGHBORHOODS.md), [traceability standard](../../Standard/TRACEABILITY.md), and the [coverage inventory](../../INVENTORY.md).

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
