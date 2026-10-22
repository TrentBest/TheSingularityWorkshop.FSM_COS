![The Singularity Workshop animated moniker](https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Renderer/development/docs/assets/moniker.svg)

# FsmCosTests

**Source:** [`tests/FSM_COS.Tests/FsmCosTests.cs`](../../../tests/FSM_COS.Tests/FsmCosTests.cs)  
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
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar and brand image" width="200" /></a></p>
<p align="center"><em>The Singularity Workshop — Tools for the curious, the bold, and the systemically inclined.</em><br /><strong>Because state shouldn't be a mess.</strong><br /><em>And because static boundaries are invitations to cause trouble.</em></p>
