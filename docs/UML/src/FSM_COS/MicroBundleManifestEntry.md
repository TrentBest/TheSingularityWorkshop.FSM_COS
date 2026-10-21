![The Singularity Workshop animated moniker](https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Renderer/development/docs/assets/moniker.svg)

# MicroBundleManifestEntry

**Source:** [`src/FSM_COS/MicroBundleManifestEntry.cs`](../../../src/FSM_COS/MicroBundleManifestEntry.cs)  
**Source baseline:** `30e437100d1f3b49d07111e87f76cb4811ba8465`  
**Source file blob:** `f53611126fde9a3eacb5f596d6fa13fc4e61ce6e`  
**Authorship:** manually authored against current `development`; not GUI.SVG-generated.

## Purpose

Small immutable value identifying a requested bundle by non-zero ID and required version.

## UML class card

~~~mermaid
classDiagram
    class MicroBundleManifestEntry {
        +BundleId: ulong
        +Version: string
        +MicroBundleManifestEntry(bundleId, version)
    }
    RuntimeManifest *-- "0..*" MicroBundleManifestEntry : requested roots
~~~

## Important boundaries

- Construction rejects zero IDs and blank versions.
- This request value is not a resolved bundle and does not validate repository availability.

## Use cases

1. Keep composition contracts explicit at the boundary where they are owned.
2. Let tests inject adapters or contexts without confusing metadata, policy, and loading behavior.

## Verification status

Source-reviewed against the recorded baseline. Mermaid rendering, automated link checking, and exact-HEAD CI have not been verified.
