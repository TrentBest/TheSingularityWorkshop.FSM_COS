![The Singularity Workshop animated moniker](https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Renderer/development/docs/assets/moniker.svg)

# RuntimeAssembly

**Source:** [`src/FSM_COS/RuntimeAssembly.cs`](../../../src/FSM_COS/RuntimeAssembly.cs)  
**Source baseline:** `9a5d9b72c42f1221e800926b09640d6e00f751ac`  
**Source file blob:** `6ef0c80aacce694fc513a7afaea76309d79fe6a5`  
**Authorship:** manually authored and checked against current `development`; not GUI.SVG-generated.

## Purpose

The immutable result model returned after successful composition. It exposes the runtime identity, assembled MicroBundles, arbitration-round count, optional semantic intent, and bundle lookup helpers.

## UML class card

~~~mermaid
classDiagram
    class RuntimeAssembly {
        +RuntimeId: ulong
        +Bundles: IReadOnlyList~IMicroBundle~
        +LoadedBundles: IReadOnlyList~IMicroBundle~
        +ArbitrationRounds: int
        +Intent: SemanticIntent
        +TryGetBundle(bundleId, bundle) bool
        +TryGetBundle~TBundle~(bundleId, bundle) bool
        +TryGetBundle~TBundle~(bundle) bool
    }
    class IMicroBundle
    class SemanticIntent
    RuntimeAssembly o-- "0..*" IMicroBundle : assembled result
    RuntimeAssembly ..> SemanticIntent : optional request intent
~~~

The diagram abbreviates nullable annotations and generic constraints. The C# source is authoritative.

## Important boundaries

- Construction is internal; the composition engine owns creation of the result.
- The bundle list is the assembled result snapshot; lookup helpers do not trigger loading.
- The type is not a UI renderer, process scheduler, repository/localization service, or rollback boundary.
- A returned assembly reports what composition produced; it does not prove arbitrary runtime semantics are mutually compatible.

## Use cases

1. A host inspects the assembled runtime and its arbitration count.
2. A host resolves a bundle by identity or expected type.
3. Tests verify that failed composition does not yield a successful assembly.

## Verification status

Source-reviewed against the recorded baseline. Mermaid rendering, automated link checks, and inclusion in generated package documentation remain unverified.

---

## 🔗 Resources & Support

- [FSM_COS](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS) · [FSM_API](https://github.com/TrentBest/FSM_API) · [FSM_Serialization](https://github.com/TrentBest/TheSingularityWorkshop.FSM_Serialization) · [WebPage](https://github.com/TrentBest/WebPage)
- [GitHub](https://github.com/TrentBest) · [Coder Legion](https://coderlegion.com/) · [Profile](https://coderlegion.com/user/The%2BSingularity%2BWorkshop) · [Patreon](https://www.patreon.com/c/TheSingularityWorkshop) · [PayPal](https://www.paypal.com/donate/?hosted_button_id=3Z7263LCQMV9J)

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar and brand image" width="200" /></a></p>
<p align="center"><em>The Singularity Workshop — Tools for the curious, the bold, and the systemically inclined.</em><br /><strong>Because state shouldn't be a mess.</strong><br /><em>And because static boundaries are invitations to cause trouble.</em></p>
