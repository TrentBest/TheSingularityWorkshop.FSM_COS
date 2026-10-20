# MicroBundleReference

**Source:** `src/FSM_COS/MicroBundleReference.cs`  
**Baseline:** `ce43e8f5bf3dfc229f28678fc2e0d944c5c8d38a`  
**Authorship:** manually authored; not GUI.SVG-generated.

## Source-grounded summary

Readonly record struct of BundleId, Version, ContentHash, with IsValid and Matches(MicroBundleManifestEntry).

## Diagram

```mermaid
classDiagram
class MicroBundleReference {
<<struct>>
+ulong BundleId
+string Version
+string ContentHash
+IsValid bool
+Matches(MicroBundleManifestEntry) bool
}
class RuntimeManifestEntry
class MicroBundleManifestEntry
RuntimeManifestEntry --> MicroBundleReference : Reference
MicroBundleReference ..> MicroBundleManifestEntry : ID/version match
```

## Evidence boundary

IsValid checks nonzero ID and nonblank version/hash. Matches compares ID and version ordinally. Neither verifies content hash against artifact bytes nor resolves an artifact.

For behavior beyond the declaration summarized here, inspect the implementation and relevant tests at the same baseline. This page is not evidence of hosted rendering or accessibility behavior by itself.
