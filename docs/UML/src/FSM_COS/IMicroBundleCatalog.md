# IMicroBundleCatalog

**Source:** `src/FSM_COS/IMicroBundleCatalog.cs`  
**Baseline:** `ce43e8f5bf3dfc229f28678fc2e0d944c5c8d38a`  
**Authorship:** manually authored; not GUI.SVG-generated.

## Source-grounded summary

Two overloads resolve domain IMicroBundle: by ID and exact version for root requests, or by ID for dependency requests.

## Diagram

```mermaid
classDiagram
class IMicroBundleCatalog {
<<interface>>
+TryResolve(ulong, string, IMicroBundle) bool
+TryResolve(ulong, IMicroBundle) bool
}
class FsmCos
class IMicroBundle
FsmCos ..> IMicroBundleCatalog : invokes both overloads
IMicroBundleCatalog ..> IMicroBundle : nullable result
```

## Evidence boundary

The interface does not specify storage, caching, or thread-safety. FsmCos validates returned bundle identity/version instead of trusting the success flag alone.

For behavior beyond the declaration summarized here, inspect the implementation and relevant tests at the same baseline. This page is not evidence of hosted rendering or accessibility behavior by itself.
