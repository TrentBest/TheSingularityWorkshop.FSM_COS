# FSM_COS Gallery

**The manifest says what is requested; FSM_COS composes what must exist together; the host decides what happens next.**

This Gallery is the short visual index. Each exhibit links to its own deep dive for the contract, recipe, and evidence. These diagrams explain source-backed composition behavior; the crane animation is an illustration, not live telemetry.

## 01 — Runtime composition at a glance

![FSM_COS runtime composition overview](assets/fsm-cos-overview.svg)

**See:** how a manifest request becomes a dependency-closed runtime assembly.

**Deep dive:** [What FSM_COS is](WHAT_IS_FSM_COS.md) · [Architecture](ARCHITECTURE.md).

## 02 — Dependency resolution

![Dependency resolution](assets/dependency-resolution.svg)

**See:** why the requested capability set may require additional MicroBundles before it can be assembled.

**Deep dive:** [Domain and composition model](DOMAIN.md) · [Consuming MicroBundles](CONSUMING_MICROBUNDLES.md).

## 03 — Preflight validation and failure boundaries

![FSM_COS preflight](assets/fsm-cos-preflight.svg)

**See:** checks that must happen before the host receives an assembly.

**Deep dive:** [Runtime manifest](RUNTIME_MANIFEST.md) · [Runtime boundary](RUNTIME_BOUNDARY.md).

## 04 — Arbitration and convergence

![Arbitration convergence](assets/arbitration-convergence.svg)

**See:** the bounded reconciliation stage in which loaded MicroBundles can reconcile composition state before handoff.

**Deep dive:** [Arbitration](ARBITRATION.md).

## 05 — RuntimeAssembly handoff

![Runtime assembly handoff](assets/runtime-assembly-handoff.svg)

**See:** the composition kernel's output boundary. RuntimeAssembly is a handoff to a host, not an application, game loop, or execution engine.

**Deep dive:** [Runtime assembly](RUNTIME_ASSEMBLY.md) · [WebPage integration](WEBPAGE_INTEGRATION.md).

## 06 — MicroBundle as a composable unit

![MicroBundle cartridge](assets/microbundle-cartridge.svg)

**See:** the conceptual unit of reusable capability that the composition kernel resolves and loads.

**Deep dive:** [Consuming MicroBundles](CONSUMING_MICROBUNDLES.md) · [Theory](THEORY.md).

## 07 — The crane metaphor

![FSM_COS crane illustration](assets/fsm-cos-crane.svg)

**See:** the assembly-crane metaphor: FSM_COS composes the requested capability set and hands it to the host. The animated variant is a visual metaphor, not an executing runtime trace.

**Deep dive:** [Architecture](ARCHITECTURE.md) · [Living documentation audit](LIVING_DOCUMENTATION_AUDIT.md).

## Gallery evidence rules

- A diagram describes a contract only to the extent supported by source and tests.
- Conceptual metaphors must remain labeled as illustrations.
- Do not call animation live execution unless a real runtime data/control loop drives it.
- Keep this page short; use the linked documents for complete explanations, implementation details, and reproduction steps.

---

[Shared Gallery standard](DOCUMENTATION_STANDARD.md#gallery-standard--one-index-per-repository) · [Documentation index](DOCUMENTATION_INDEX.md)
