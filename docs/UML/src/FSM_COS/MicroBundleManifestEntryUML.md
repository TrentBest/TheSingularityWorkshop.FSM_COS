<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# MicroBundleManifestEntry — UML neighborhood

**Target:** `TheSingularityWorkshop.FSM_COS.MicroBundleManifestEntry`  
**Evidence class:** manually authored, source-checked structural view derived from the existing type companion.  
**Source page:** [MicroBundleManifestEntry.md](MicroBundleManifestEntry.md)  
**Source declaration:** [MicroBundleManifestEntry.cs](../../../../src/FSM_COS/MicroBundleManifestEntry.cs)  
**Generation status:** not generated; no runtime trace is implied.

## Purpose

Small immutable value identifying a requested bundle by non-zero ID and required version.

## Target type model

~~~mermaid
classDiagram
    class MicroBundleManifestEntry {
        +BundleId: ulong
        +Version: string
        +MicroBundleManifestEntry(bundleId, version)
    }
    RuntimeManifest *-- "0..*" MicroBundleManifestEntry : requested roots
~~~

## Consumers, dependencies, and boundary

The relationships shown are limited to those documented in the linked type companion. This page does not claim a repository-wide caller scan. Consumers not represented in the documented model remain unknown until a compiler-backed source scan is performed. Package references and external types must be confirmed against project metadata and semantic symbols before being treated as exhaustive.

## Use cases

1. Add a root MicroBundle identity and requested version to a `RuntimeManifest`.
2. Express which root version the catalog must resolve; do not treat this value object as the resolved bundle instance.

## Coverage notes

- Manually maintained and source-checked, not generated from Roslyn or reflection.
- Structural type relationships do not prove invocation order or runtime data flow.
- Sequence, activity/control-flow, and measured complexity evidence remain separate deliverables where applicable.

See [class neighborhood standard](../../Standard/CLASS_NEIGHBORHOODS.md), [traceability standard](../../Standard/TRACEABILITY.md), and the [coverage inventory](../../INVENTORY.md).

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
