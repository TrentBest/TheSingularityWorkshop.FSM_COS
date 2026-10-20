# FsmCosTests

**Source:** `tests/FSM_COS.Tests/FsmCosTests.cs`  
**Baseline:** `ce43e8f5bf3dfc229f28678fc2e0d944c5c8d38a`  
**Authorship:** manually authored; not GUI.SVG-generated.

## Source-grounded summary

Large unit-test suite with private test catalogs, bundles, configuration providers, and malformed/mutating fixtures. Coverage areas include constructor/resource limits, preflight identity/version/graph validation, dependency-first load, configuration snapshotting, failure propagation, arbitration convergence, schedule consistency, and RuntimeAssembly lookup/snapshot.

## Diagram

```mermaid
flowchart TD
T[FsmCosTests] --> Limits[Resource bounds]
T --> Graph[Root/version/dependency preflight]
T --> Load[Dependency-first loading/configuration]
T --> Snap[Snapshot isolation]
T --> Arb[Arbitration and failures]
T --> Sched[Schedule versus resolved graph]
T --> Result[RuntimeAssembly API]
T --> Boundary[Dependency/domain boundary]
```

## Evidence boundary

Representative assertions include full-closure preflight before Load, deep-chain traversal without recursive stack growth, cycle rejection, configuration copy/limits, non-convergence failure, and read-only assembly snapshot. This page maps test assertions; it does not claim exhaustive coverage of every hostile or concurrent production scenario.

For behavior beyond the declaration summarized here, inspect the implementation and relevant tests at the same baseline. This page is not evidence of hosted rendering or accessibility behavior by itself.
