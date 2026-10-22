![The Singularity Workshop animated moniker](https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Renderer/development/docs/assets/moniker.svg)

# RuntimeManifestPlanningTests

**Source:** [`tests/FSM_COS.Tests/RuntimeManifestPlanningTests.cs`](../../../../tests/FSM_COS.Tests/RuntimeManifestPlanningTests.cs)  
**Source baseline:** `6c43f39a2be1465360abb03ed27d577da42c1ab1`  
**Source file blob:** `8f4f1f8ee010f8a3b8bc7b68582998a57bccc940`  
**Authorship:** manually authored against current `development`; not generated from reflection or GUI.SVG.

## Purpose

Tests for staged manifest validation, dependency schedule construction, bounded graph processing, load-plan state transitions, and deferred-promotion eligibility.

## UML class card

~~~mermaid
classDiagram
    class RuntimeManifestPlanningTests {
        +33 Fact tests
    }
    class RuntimeManifest
    class RuntimeManifestSchedule
    class RuntimeManifestLoadPlan
    class RuntimeManifestEntry
    class RuntimeManifestDependency
    class IManifestLoadEvaluator
    RuntimeManifestPlanningTests ..> RuntimeManifest : validates staged requests
    RuntimeManifestPlanningTests ..> RuntimeManifestSchedule : validates dependency graph
    RuntimeManifestPlanningTests ..> RuntimeManifestLoadPlan : tests state transitions
    RuntimeManifestPlanningTests ..> RuntimeManifestEntry : constructs staged entries
    RuntimeManifestPlanningTests ..> RuntimeManifestDependency : constructs edges
    RuntimeManifestPlanningTests ..> IManifestLoadEvaluator : tests promotion policy
~~~

## Coverage

- Enumeration/resource bounds, invalid entries, duplicate IDs, version drift, and schedule coverage.
- Deep acyclic graphs and cycle rejection without recursive stack growth.
- Dependency readiness, stage ordering, localization, and deferred promotion policy.
- Invalid transitions, undefined enum values, read-only collections, and propagated evaluator/predicate failures.

## Use cases

1. Verify published schedules reject malformed entries, missing endpoints, duplicate identities, cycles, and oversized graphs.
2. Check the load-plan state transition rules and the host evaluator's deferred-entry eligibility decisions.
3. Keep these tests distinct from proof of active deferred loading; the current composition path does not localize or promote deferred bundles.




---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
