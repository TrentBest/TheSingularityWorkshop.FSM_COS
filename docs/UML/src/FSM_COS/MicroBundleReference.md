![The Singularity Workshop animated moniker](https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Renderer/development/docs/assets/moniker.svg)

# MicroBundleReference

**Source:** [`src/FSM_COS/MicroBundleReference.cs`](../../../src/FSM_COS/MicroBundleReference.cs)  
**Source baseline:** `342c6fe71aee5ae9089547348c140a612e083248`  
**Source file blob:** `931905da936669cb1fdad47f0fef8015f0e4d451`  
**Authorship:** manually authored against current `development`; not GUI.SVG-generated.

## Purpose

Immutable published identity for a MicroBundle artifact, comprising bundle ID, version, and content hash.

## UML class card

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

## Important boundaries

- The reference records expected identity; it does not resolve the artifact or verify the content hash.
- `Matches` compares bundle ID and version; repository/localization code owns content retrieval and hash verification.

## Use cases

1. Represent and validate published identity or dependency metadata.
2. Keep artifact resolution and runtime policy at the appropriate repository or composition boundary.

## Verification status

Source-reviewed against the recorded baseline. Mermaid rendering, automated link checking, and exact-HEAD CI have not been verified.
