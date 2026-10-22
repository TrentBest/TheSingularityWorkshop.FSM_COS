![The Singularity Workshop animated moniker](https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Renderer/development/docs/assets/moniker.svg)

# ArbitrationContext

**Source:** [`src/FSM_COS/ArbitrationContext.cs`](../../../src/FSM_COS/ArbitrationContext.cs)  
**Source baseline:** `30e437100d1f3b49d07111e87f76cb4811ba8465`  
**Source file blob:** `e74a4d177f894de85cd2a34c4df00a182b973c07`  
**Authorship:** manually authored against current `development`; not GUI.SVG-generated.

## Purpose

FSM_COS adapter implementing the domain-owned arbitration context with runtime identity, current bundle set, and optional Experience state context.

## UML class card

~~~mermaid
classDiagram
    class ArbitrationContext {
        +RuntimeId: ulong
        +ExperienceContext: IStateContext
        +Bundles: IReadOnlyList~IMicroBundle~
        +LoadedBundles: IReadOnlyList~IMicroBundle~
    }
    class IMicroBundleArbitrationContext
    class IMicroBundle
    ArbitrationContext ..|> IMicroBundleArbitrationContext
    ArbitrationContext o-- "0..*" IMicroBundle : current composition set
~~~

## Important boundaries

- Constructor is internal; the composition engine owns the arbitration snapshot.
- `LoadedBundles` is a compatibility alias for `Bundles`; the context is not a scheduler or rollback mechanism.

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
