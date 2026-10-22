![The Singularity Workshop animated moniker](https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Renderer/development/docs/assets/moniker.svg)

# FsmCosTests

**Source:** [`tests/FSM_COS.Tests/FsmCosTests.cs`](../../../../tests/FSM_COS.Tests/FsmCosTests.cs)  
**Source baseline:** `6c43f39a2be1465360abb03ed27d577da42c1ab1`  
**Source file blob:** `6ee7f5e2b5feb0286f08fb8403b92f17edc31447`  
**Authorship:** manually authored against current `development`; not generated from reflection or GUI.SVG.

## Purpose

Primary composition-engine regression suite covering validation, dependency closure, catalog identity, configuration, resource bounds, arbitration, and the immutable result contract.

## UML class card

~~~mermaid
classDiagram
    class FsmCosTests {
        +86 Fact tests
    }
    class FsmCos
    class TestCatalog
    class TestBundle
    class RuntimeManifest
    class RuntimeAssembly
    class IMicroBundleConfigurationSource
    FsmCosTests ..> FsmCos : exercises
    FsmCosTests ..> TestCatalog : injects resolution behavior
    FsmCosTests ..> TestBundle : observes load/arbitration
    FsmCosTests ..> RuntimeManifest : supplies requests
    FsmCosTests ..> RuntimeAssembly : checks successful results
    FsmCosTests ..> IMicroBundleConfigurationSource : injects configuration
~~~

## Coverage

- Constructor limits, malformed/hostile collections, and preflight-before-load behavior.
- Root/version validation, complete dependency closure, shared dependencies, cycles, and deep chains.
- Configuration byte limits, copies/snapshot isolation, and error propagation.
- Bounded arbitration, stable convergence, and non-convergence failures.
- Domain-contract compatibility, immutable assembly results, and explicit no-rollback behavior on a dependent load failure.

## Use cases

1. Find the exact regression test for a failure such as a missing dependency, identity mismatch, cycle, or non-converging arbitration.
2. Use the configuration and resource-limit tests when changing validation or input bounds.
3. Run the complete test project on the candidate commit after modifying composition behavior; a test page is navigation, not a substitute for a passing run.




---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
