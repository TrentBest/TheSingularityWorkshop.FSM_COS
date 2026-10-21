![The Singularity Workshop animated moniker](https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Renderer/development/docs/assets/moniker.svg)

# AiGuiCapabilityHandoffTests

**Source:** [`tests/FSM_COS.Tests/AiGuiCapabilityHandoffTests.cs`](../../../tests/FSM_COS.Tests/AiGuiCapabilityHandoffTests.cs)  
**Source baseline:** `6c43f39a2be1465360abb03ed27d577da42c1ab1`  
**Source file blob:** `b28a6df5b7b5c4a7f8a6dc9ad9cf192b78c21f99`  
**Authorship:** manually authored against current `development`; not generated from reflection or GUI.SVG.

## Purpose

Integration-boundary tests demonstrating that FSM_COS composes real ProtocolAI, GrammarAI, and GUI capabilities without adding those package references to the core assembly.

## UML class card

~~~mermaid
classDiagram
    class AiGuiCapabilityHandoffTests {
        +Core_does_not_reference_AI_or_GUI_packages() void
        +Execute_assembles_ProtocolAI_GrammarAI_and_GUI_as_capabilities() void
        +Grammar_capability_references_the_ProtocolAI_capability_without_copying_its_vocabulary() void
    }
    class FsmCos
    class TestCatalog
    class ProtocolCapabilityBundle
    class GrammarCapabilityBundle
    class AiExchangeGuiCapabilityBundle
    AiGuiCapabilityHandoffTests ..> FsmCos : exercises
    AiGuiCapabilityHandoffTests ..> TestCatalog : supplies domain bundles
    TestCatalog o-- ProtocolCapabilityBundle
    TestCatalog o-- GrammarCapabilityBundle
    TestCatalog o-- AiExchangeGuiCapabilityBundle
~~~

## Coverage

- Assembly-reference boundary (core stays free of AI/GUI package references).
- Composition of multiple real capability packages through catalog-resolved bundles.
- Capability handoff and reuse across bundle dependencies.

## Use cases

1. Read the tests as executable evidence of the stated contracts and failure boundaries.
2. Use the test names as navigation to the precise assertion in the linked source file.
3. Re-run the test project on the exact commit before treating the evidence as current CI validation.

## Verification status

The test source was inspected and the documented `[Fact]` count was counted from the linked file. Mermaid rendering, automated links, and exact-HEAD CI are not yet verified by this companion.
