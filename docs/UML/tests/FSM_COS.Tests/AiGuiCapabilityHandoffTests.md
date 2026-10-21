![The Singularity Workshop animated moniker](https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Renderer/development/docs/assets/moniker.svg)

# AiGuiCapabilityHandoffTests

**Source:** `tests/FSM_COS.Tests/AiGuiCapabilityHandoffTests.cs`  
**Baseline:** `ce43e8f5bf3dfc229f28678fc2e0d944c5c8d38a`  
**Authorship:** manually authored; not GUI.SVG-generated.

## Source-grounded summary

Three test methods check core assembly reference boundaries, composition of ProtocolAI/GrammarAI/GUI capabilities with dependency-first ordering and typed retrieval, and a grammar reference to a protocol symbol.

## Diagram

```mermaid
classDiagram
class AiGuiCapabilityHandoffTests
class TestCatalog
class ProtocolCapabilityBundle
class GrammarCapabilityBundle
class AiExchangeGuiCapabilityBundle
class FsmCos
AiGuiCapabilityHandoffTests ..> FsmCos : Execute
FsmCos ..> TestCatalog : resolve
TestCatalog o--> ProtocolCapabilityBundle
TestCatalog o--> GrammarCapabilityBundle
TestCatalog o--> AiExchangeGuiCapabilityBundle
GrammarCapabilityBundle ..> ProtocolCapabilityBundle
AiExchangeGuiCapabilityBundle ..> ProtocolCapabilityBundle
AiExchangeGuiCapabilityBundle ..> GrammarCapabilityBundle
```

## Evidence boundary

Fixtures are private to this test file. Assertions do not prove every integration path, hosted rendering, accessibility-tree exposure, keyboard support, or screen-reader behavior.

For behavior beyond the declaration summarized here, inspect the implementation and relevant tests at the same baseline. This page is not evidence of hosted rendering or accessibility behavior by itself.

---

**The Singularity Workshop**  
[GitHub](https://github.com/TrentBest) · [Coder Legion](https://coderlegion.com/) · [Patreon](https://www.patreon.com/c/TheSingularityWorkshop) · [PayPal](https://www.paypal.com/donate/?hosted_button_id=3Z7263LCQMV9J)  
[ FSM_API ](https://github.com/TrentBest/FSM_API) · [ FSM_Serialization ](https://github.com/TrentBest/TheSingularityWorkshop.FSM_Serialization) · [ WebPage ](https://github.com/TrentBest/WebPage) · [ FSM_COS Documentation Standard ](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/DOCUMENTATION_STANDARD.md)  
**Because state shouldn't be a mess.**  
*And because static boundaries are invitations to cause trouble.*
