![The Singularity Workshop animated moniker](https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Renderer/development/docs/assets/moniker.svg)

# RuntimeAssembly

**Source:** `src/FSM_COS/RuntimeAssembly.cs`  
**Baseline:** `ce43e8f5bf3dfc229f28678fc2e0d944c5c8d38a`  
**Authorship:** manually authored; not GUI.SVG-generated.

## Source-grounded summary

Sealed composition result. Internal constructor snapshots bundle references into an array wrapped by Array.AsReadOnly. Exposes RuntimeId, Bundles/LoadedBundles, ArbitrationRounds, optional SemanticIntent, and lookup overloads.

## Diagram

```mermaid
classDiagram
class RuntimeAssembly {
+ulong RuntimeId
+IReadOnlyList Bundles
+IReadOnlyList LoadedBundles
+int ArbitrationRounds
+SemanticIntent Intent
+TryGetBundle(ulong) bool
+TryGetBundle<TBundle>(ulong) bool
+TryGetBundle<TBundle>() bool
}
class IMicroBundle
class SemanticIntent
class FsmCos
FsmCos ..> RuntimeAssembly : constructs on success
RuntimeAssembly o--> IMicroBundle : snapshot of references
RuntimeAssembly ..> SemanticIntent : optional metadata
```

## Evidence boundary

Lookup methods search the snapshot by ID and/or type. The list wrapper prevents normal list mutation but does not make bundle instances immutable or roll back side effects.

For behavior beyond the declaration summarized here, inspect the implementation and relevant tests at the same baseline. This page is not evidence of hosted rendering or accessibility behavior by itself.

---

**The Singularity Workshop**  
[GitHub](https://github.com/TrentBest) · [Coder Legion](https://coderlegion.com/) · [Patreon](https://www.patreon.com/c/TheSingularityWorkshop) · [PayPal](https://www.paypal.com/donate/?hosted_button_id=3Z7263LCQMV9J)  
[ FSM_API ](https://github.com/TrentBest/FSM_API) · [ FSM_Serialization ](https://github.com/TrentBest/TheSingularityWorkshop.FSM_Serialization) · [ WebPage ](https://github.com/TrentBest/WebPage) · [ FSM_COS Documentation Standard ](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/DOCUMENTATION_STANDARD.md)  
**Because state shouldn't be a mess.**  
*And because static boundaries are invitations to cause trouble.*
