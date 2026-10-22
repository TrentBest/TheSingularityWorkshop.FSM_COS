![The Singularity Workshop animated moniker](https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Renderer/development/docs/assets/moniker.svg)

# RuntimeManifestPlanningTests

**Source:** [`tests/FSM_COS.Tests/RuntimeManifestPlanningTests.cs`](../../../tests/FSM_COS.Tests/RuntimeManifestPlanningTests.cs)  
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

1. Read the tests as executable evidence of the stated contracts and failure boundaries.
2. Use the test names as navigation to the precise assertion in the linked source file.
3. Re-run the test project on the exact commit before treating the evidence as current CI validation.

## Verification status

The test source was inspected and the documented `[Fact]` count was counted from the linked file. Mermaid rendering, automated links, and exact-HEAD CI are not yet verified by this companion.

---

## 🔗 Resources & Support

- [FSM_COS](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS) · [FSM_API](https://github.com/TrentBest/FSM_API) · [FSM_Serialization](https://github.com/TrentBest/TheSingularityWorkshop.FSM_Serialization) · [WebPage](https://github.com/TrentBest/WebPage)
- [GitHub](https://github.com/TrentBest) · [Coder Legion](https://coderlegion.com/) · [Profile](https://coderlegion.com/user/The%2BSingularity%2BWorkshop) · [Patreon](https://www.patreon.com/c/TheSingularityWorkshop) · [PayPal](https://www.paypal.com/donate/?hosted_button_id=3Z7263LCQMV9J)

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
