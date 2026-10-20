# IMicroBundleConfigurationSource

**Source:** `src/FSM_COS/IMicroBundleConfigurationSource.cs`  
**Baseline:** `ce43e8f5bf3dfc229f28678fc2e0d944c5c8d38a`  
**Authorship:** manually authored; not GUI.SVG-generated.

## Source-grounded summary

Optional provider contract keyed by runtime ID, bundle ID, and resolved version; returns ReadOnlyMemory<byte> plus a boolean presence result.

## Diagram

```mermaid
classDiagram
class IMicroBundleConfigurationSource {
<<interface>>
+TryGetConfiguration(ulong, ulong, string, ReadOnlyMemory) bool
}
class FsmCos
class MicroBundleLoadContext
FsmCos ..> IMicroBundleConfigurationSource : optional provider
FsmCos ..> MicroBundleLoadContext : stores copied config
```

## Evidence boundary

FsmCos copies returned memory before storing it and bounds total configuration bytes. The interface does not guarantee provider purity, confidentiality, persistence, or immutability.

For behavior beyond the declaration summarized here, inspect the implementation and relevant tests at the same baseline. This page is not evidence of hosted rendering or accessibility behavior by itself.
