![The Singularity Workshop animated moniker](https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Renderer/development/docs/assets/moniker.svg)

# ArbitrationContext

**Source:** `src/FSM_COS/ArbitrationContext.cs`  
**Baseline:** `ce43e8f5bf3dfc229f28678fc2e0d944c5c8d38a`  
**Authorship:** manually authored; not GUI.SVG-generated.

## Source-grounded summary

Sealed implementation of IMicroBundleArbitrationContext. Internal constructor stores runtime ID, supplied bundle list, and nullable IStateContext. Exposes Bundles and LoadedBundles as aliases; explicitly implements ExperienceContext as object?.

## Diagram

```mermaid
classDiagram
class ArbitrationContext {
+ulong RuntimeId
+IStateContext ExperienceContext
+IReadOnlyList Bundles
+IReadOnlyList LoadedBundles
}
class IMicroBundleArbitrationContext
class IStateContext
class IMicroBundle
ArbitrationContext ..|> IMicroBundleArbitrationContext
ArbitrationContext --> IStateContext : optional context
ArbitrationContext o--> IMicroBundle : bundle list
```

## Evidence boundary

This class stores the list reference rather than copying/wrapping it. FsmCos currently passes its local loaded list. Read-only typing is not proof of deep immutability.

For behavior beyond the declaration summarized here, inspect the implementation and relevant tests at the same baseline. This page is not evidence of hosted rendering or accessibility behavior by itself.

---

**The Singularity Workshop**  
[GitHub](https://github.com/TrentBest) · [Coder Legion](https://coderlegion.com/) · [Patreon](https://www.patreon.com/c/TheSingularityWorkshop) · [PayPal](https://www.paypal.com/donate/?hosted_button_id=3Z7263LCQMV9J)  
[ FSM_API ](https://github.com/TrentBest/FSM_API) · [ FSM_Serialization ](https://github.com/TrentBest/TheSingularityWorkshop.FSM_Serialization) · [ WebPage ](https://github.com/TrentBest/WebPage) · [ FSM_COS Documentation Standard ](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/DOCUMENTATION_STANDARD.md)  
**Because state shouldn't be a mess.**  
*And because static boundaries are invitations to cause trouble.*
