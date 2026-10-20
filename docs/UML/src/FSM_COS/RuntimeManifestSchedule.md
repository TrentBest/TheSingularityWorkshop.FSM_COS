# RuntimeManifestSchedule

**Source:** `src/FSM_COS/RuntimeManifestSchedule.cs`  
**Baseline:** `ce43e8f5bf3dfc229f28678fc2e0d944c5c8d38a`  
**Authorship:** manually authored; not GUI.SVG-generated.

## Source-grounded summary

Bounded, snapshotting publish-time dependency plan. Validates entries/edges, duplicate IDs/edges, missing endpoints, and cycles iteratively. Exposes Entries, Dependencies, IndependentEntries, DependencyEntries, DependenciesOf, and IsDependencyReady.

## Diagram

```mermaid
classDiagram
class RuntimeManifestSchedule {
+DefaultMaximumEntryCount$
+DefaultMaximumDependencyCount$
+IReadOnlyList Entries
+IReadOnlyList Dependencies
+IReadOnlyList IndependentEntries
+IReadOnlyList DependencyEntries
+DependenciesOf(ulong) IReadOnlyList
+IsDependencyReady(ulong, Func) bool
}
class RuntimeManifestEntry
class RuntimeManifestDependency
RuntimeManifestSchedule o--> RuntimeManifestEntry : snapshots
RuntimeManifestSchedule o--> RuntimeManifestDependency : snapshots edges
```

## Evidence boundary

Limits are 10,000 entries and 100,000 edges. IsDependencyReady rejects null predicate/unknown ID and requires every dependency predicate to be true; predicate exceptions propagate. This is not an execution scheduler and does not load artifacts.

For behavior beyond the declaration summarized here, inspect the implementation and relevant tests at the same baseline. This page is not evidence of hosted rendering or accessibility behavior by itself.
