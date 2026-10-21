![The Singularity Workshop animated moniker](https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Renderer/development/docs/assets/moniker.svg)

# MicroBundleLoadContext

**Source:** `src/FSM_COS/MicroBundleLoadContext.cs`  
**Baseline:** `ce43e8f5bf3dfc229f28678fc2e0d944c5c8d38a`  
**Authorship:** manually authored; not GUI.SVG-generated.

## Source-grounded summary

Sealed IMicroBundleLoadContext implementation with runtime ID, configuration dictionary view, TryGetConfiguration, and internal SetConfiguration.

## Diagram

```mermaid
classDiagram
class MicroBundleLoadContext {
+ulong RuntimeId
+IReadOnlyDictionary Configuration
+TryGetConfiguration(ulong, ReadOnlyMemory) bool
~SetConfiguration(ulong, ReadOnlyMemory)
}
class IMicroBundleLoadContext
class IMicroBundle
MicroBundleLoadContext ..|> IMicroBundleLoadContext
IMicroBundle ..> IMicroBundleLoadContext : receives in Load
```

## Evidence boundary

The property exposes the private dictionary through IReadOnlyDictionary, not a defensive wrapper. FsmCos snapshots configuration byte buffers before insertion; do not infer deep immutability of arbitrary memory.

For behavior beyond the declaration summarized here, inspect the implementation and relevant tests at the same baseline. This page is not evidence of hosted rendering or accessibility behavior by itself.

---

**The Singularity Workshop**  
[GitHub](https://github.com/TrentBest) · [Coder Legion](https://coderlegion.com/) · [Patreon](https://www.patreon.com/c/TheSingularityWorkshop) · [PayPal](https://www.paypal.com/donate/?hosted_button_id=3Z7263LCQMV9J)  
[ FSM_API ](https://github.com/TrentBest/FSM_API) · [ FSM_Serialization ](https://github.com/TrentBest/TheSingularityWorkshop.FSM_Serialization) · [ WebPage ](https://github.com/TrentBest/WebPage) · [ FSM_COS Documentation Standard ](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/DOCUMENTATION_STANDARD.md)  
**Because state shouldn't be a mess.**  
*And because static boundaries are invitations to cause trouble.*
