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
