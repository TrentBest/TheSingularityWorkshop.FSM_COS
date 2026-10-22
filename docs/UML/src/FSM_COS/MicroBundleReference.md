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

---

## 🔗 Resources & Support

- [FSM_COS](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS) · [FSM_API](https://github.com/TrentBest/FSM_API) · [FSM_Serialization](https://github.com/TrentBest/TheSingularityWorkshop.FSM_Serialization) · [WebPage](https://github.com/TrentBest/WebPage)
- [GitHub](https://github.com/TrentBest) · [Coder Legion](https://coderlegion.com/) · [Profile](https://coderlegion.com/user/The%2BSingularity%2BWorkshop) · [Patreon](https://www.patreon.com/c/TheSingularityWorkshop) · [PayPal](https://www.paypal.com/donate/?hosted_button_id=3Z7263LCQMV9J)

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar and brand image" width="200" /></a></p>
<p align="center"><em>The Singularity Workshop — Tools for the curious, the bold, and the systemically inclined.</em><br /><strong>Because state shouldn't be a mess.</strong><br /><em>And because static boundaries are invitations to cause trouble.</em></p>
