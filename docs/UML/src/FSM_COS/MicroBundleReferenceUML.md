<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# MicroBundleReference — UML neighborhood

**Target:** `TheSingularityWorkshop.FSM_COS.MicroBundleReference`  
**Evidence class:** manually authored, source-checked structural view derived from the existing type companion.  
**Source page:** [MicroBundleReference.md](MicroBundleReference.md)  
**Source declaration:** [MicroBundleReference.cs](../../../../src/FSM_COS/MicroBundleReference.cs)  
**Generation status:** not generated; no runtime trace is implied.

## Purpose

Immutable published identity for a MicroBundle artifact, comprising bundle ID, version, and content hash.

## Target type model

~~~mermaid
classDiagram
    class MicroBundleReference {
        +BundleId: ulong
        +Version: string
        +ContentHash: string
        +IsValid: bool
        +Matches(entry) bool
    }
    class MicroBundleManifestEntry
    MicroBundleReference ..> MicroBundleManifestEntry : compares ID and version
~~~

## Consumers, dependencies, and boundary

The relationships shown are limited to those documented in the linked type companion. This page does not claim a repository-wide caller scan. Consumers not represented in the documented model remain unknown until a compiler-backed source scan is performed. Package references and external types must be confirmed against project metadata and semantic symbols before being treated as exhaustive.

## Use cases

1. Record the published identity, version, and content-hash metadata associated with a manifest entry.
2. Compare a reference with a requested entry when validating published schedule metadata.
3. Do not treat the stored hash as proof that FSM_COS has fetched or verified artifact bytes.

## Coverage notes

- Manually maintained and source-checked, not generated from Roslyn or reflection.
- Structural type relationships do not prove invocation order or runtime data flow.
- Sequence, activity/control-flow, and measured complexity evidence remain separate deliverables where applicable.

See [class neighborhood standard](../../Standard/CLASS_NEIGHBORHOODS.md), [traceability standard](../../Standard/TRACEABILITY.md), and the [coverage inventory](../../INVENTORY.md).

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
