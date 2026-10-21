![The Singularity Workshop animated moniker](https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Renderer/development/docs/assets/moniker.svg)

# IManifestLoadEvaluator

**Source:** `src/FSM_COS/IManifestLoadEvaluator.cs`  
**Baseline:** `ce43e8f5bf3dfc229f28678fc2e0d944c5c8d38a`  
**Authorship:** manually authored; not GUI.SVG-generated.

## Source-grounded summary

Experience-owned decision interface: ShouldLoad(RuntimeManifestEntry, nullable IStateContext). XML remarks exclude dependency resolution, artifact localization, and loading from this contract.

## Diagram

```mermaid
classDiagram
class IManifestLoadEvaluator {
<<interface>>
+ShouldLoad(RuntimeManifestEntry, IStateContext) bool
}
class RuntimeManifestEntry
class IStateContext
class RuntimeManifestLoadPlan
IManifestLoadEvaluator ..> RuntimeManifestEntry : evaluates
IManifestLoadEvaluator ..> IStateContext : optional context
RuntimeManifestLoadPlan ..> IManifestLoadEvaluator : asks during promotion
```

## Evidence boundary

The source establishes a contract and call site, not a particular policy or host evaluator correctness.

For behavior beyond the declaration summarized here, inspect the implementation and relevant tests at the same baseline. This page is not evidence of hosted rendering or accessibility behavior by itself.

---

**The Singularity Workshop**  
[GitHub](https://github.com/TrentBest) · [Coder Legion](https://coderlegion.com/) · [Patreon](https://www.patreon.com/c/TheSingularityWorkshop) · [PayPal](https://www.paypal.com/donate/?hosted_button_id=3Z7263LCQMV9J)  
[ FSM_API ](https://github.com/TrentBest/FSM_API) · [ FSM_Serialization ](https://github.com/TrentBest/TheSingularityWorkshop.FSM_Serialization) · [ WebPage ](https://github.com/TrentBest/WebPage) · [ FSM_COS Documentation Standard ](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/DOCUMENTATION_STANDARD.md)  
**Because state shouldn't be a mess.**  
*And because static boundaries are invitations to cause trouble.*
