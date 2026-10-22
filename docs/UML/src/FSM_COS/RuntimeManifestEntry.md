![The Singularity Workshop animated moniker](https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Renderer/development/docs/assets/moniker.svg)

# RuntimeManifestEntry

**Source:** [`src/FSM_COS/RuntimeManifestEntry.cs`](../../../src/FSM_COS/RuntimeManifestEntry.cs)  
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

1. Represent and validate published identity or dependency metadata.
2. Keep artifact resolution and runtime policy at the appropriate repository or composition boundary.

## Verification status

Source-reviewed against the recorded baseline. Mermaid rendering, automated link checking, and exact-HEAD CI have not been verified.

---

## 🔗 Resources & Support

- [FSM_COS](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS) · [FSM_API](https://github.com/TrentBest/FSM_API) · [FSM_Serialization](https://github.com/TrentBest/TheSingularityWorkshop.FSM_Serialization) · [WebPage](https://github.com/TrentBest/WebPage)
- [GitHub](https://github.com/TrentBest) · [Coder Legion](https://coderlegion.com/) · [Profile](https://coderlegion.com/user/The%2BSingularity%2BWorkshop) · [Patreon](https://www.patreon.com/c/TheSingularityWorkshop) · [PayPal](https://www.paypal.com/donate/?hosted_button_id=3Z7263LCQMV9J)

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar and brand image" width="200" /></a></p>
<p align="center"><em>The Singularity Workshop — Tools for the curious, the bold, and the systemically inclined.</em><br /><strong>Because state shouldn't be a mess.</strong><br /><em>And because static boundaries are invitations to cause trouble.</em></p>
