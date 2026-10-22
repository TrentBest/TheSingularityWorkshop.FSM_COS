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

---

## 🔗 Resources & Support

- [FSM_COS](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS) · [FSM_API](https://github.com/TrentBest/FSM_API) · [FSM_Serialization](https://github.com/TrentBest/TheSingularityWorkshop.FSM_Serialization) · [WebPage](https://github.com/TrentBest/WebPage)
- [GitHub](https://github.com/TrentBest) · [Coder Legion](https://coderlegion.com/) · [Profile](https://coderlegion.com/user/The%2BSingularity%2BWorkshop) · [Patreon](https://www.patreon.com/c/TheSingularityWorkshop) · [PayPal](https://www.paypal.com/donate/?hosted_button_id=3Z7263LCQMV9J)

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
