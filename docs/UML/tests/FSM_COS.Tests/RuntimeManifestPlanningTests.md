# RuntimeManifestPlanningTests

**Source:** `tests/FSM_COS.Tests/RuntimeManifestPlanningTests.cs`  
**Baseline:** `ce43e8f5bf3dfc229f28678fc2e0d944c5c8d38a`  
**Authorship:** manually authored; not GUI.SVG-generated.

## Source-grounded summary

33 test methods cover bounded input enumeration, staged root/version consistency, schedule validation and deep/cyclic graphs, stage ordering, deferred promotion readiness, invalid/duplicate identities and edges, load-plan transitions, mismatched schedules, snapshot/read-only behavior, and propagated failures.

## Diagram

```mermaid
flowchart TD
T[RuntimeManifestPlanningTests] --> Manifest[Staged manifest consistency]
T --> Schedule[Entries, edges, cycle validation]
T --> State[Published to localized to loaded]
T --> Promotion[Deferred candidates and prerequisites]
T --> Errors[Bounds and failure propagation]
T --> Isolation[Snapshot/read-only collections]
```

## Evidence boundary

This suite tests metadata and state-tracker contracts. It does not prove real artifact localization, hosted lifecycle behavior, or automatic hot swapping.

For behavior beyond the declaration summarized here, inspect the implementation and relevant tests at the same baseline. This page is not evidence of hosted rendering or accessibility behavior by itself.
