![The Singularity Workshop animated moniker](https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Renderer/development/docs/assets/moniker.svg)

# RuntimeManifestLoadPlan

**Source:** `src/FSM_COS/RuntimeManifestLoadPlan.cs`  
**Baseline:** `ce43e8f5bf3dfc229f28678fc2e0d944c5c8d38a`  
**Authorship:** manually authored; not GUI.SVG-generated.

## Source-grounded summary

Tracks Published, Localized, Loaded state per entry. Exposes GetState, MarkLocalized, MarkLoaded, and lazy EvaluatePromotions. The same file declares MicroBundleRuntimeState values 0/1/2.

## Diagram

```mermaid
classDiagram
class RuntimeManifestLoadPlan {
+GetState(ulong) MicroBundleRuntimeState
+MarkLocalized(ulong) void
+MarkLoaded(ulong) void
+EvaluatePromotions(IManifestLoadEvaluator, IStateContext, RuntimeManifestSchedule) IEnumerable
}
class MicroBundleRuntimeState {
<<enumeration>>
Published = 0
Localized = 1
Loaded = 2
}
class RuntimeManifestSchedule
class IManifestLoadEvaluator
RuntimeManifestLoadPlan --> MicroBundleRuntimeState : tracks state
RuntimeManifestLoadPlan ..> RuntimeManifestSchedule : prerequisites
RuntimeManifestLoadPlan ..> IManifestLoadEvaluator : deferred decisions
```

## Evidence boundary

Constructor snapshots/bounds entries and rejects invalid/duplicate IDs; optional schedule must match identity/stage coverage. MarkLoaded requires Localized and loaded prerequisites. EvaluatePromotions considers localized entries in stage order, calls evaluator only for Deferred entries, and validates replacement schedule consistency. This is a tracker/candidate evaluator, not an artifact loader or hot-reload mechanism.

For behavior beyond the declaration summarized here, inspect the implementation and relevant tests at the same baseline. This page is not evidence of hosted rendering or accessibility behavior by itself.

---

**The Singularity Workshop**  
[GitHub](https://github.com/TrentBest) · [Coder Legion](https://coderlegion.com/) · [Patreon](https://www.patreon.com/c/TheSingularityWorkshop) · [PayPal](https://www.paypal.com/donate/?hosted_button_id=3Z7263LCQMV9J)  
[ FSM_API ](https://github.com/TrentBest/FSM_API) · [ FSM_Serialization ](https://github.com/TrentBest/TheSingularityWorkshop.FSM_Serialization) · [ WebPage ](https://github.com/TrentBest/WebPage) · [ FSM_COS Documentation Standard ](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/DOCUMENTATION_STANDARD.md)  
**Because state shouldn't be a mess.**  
*And because static boundaries are invitations to cause trouble.*
