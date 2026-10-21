# Host integration map: AnyApp, WebApp, and WebPage

This guide answers one question: **how does a host use FSM_COS, and what is actually proven today?** It is a navigation and status map, not a second definition of the FSM_COS API. The authoritative kernel contract remains in [Consuming MicroBundles](CONSUMING_MICROBUNDLES.md); host-specific implementation details belong to the host repositories linked below.

> **Boundary in one sentence:** the host chooses the requested Experience and supplies a catalog; FSM_COS resolves and composes the requested MicroBundles into a `RuntimeAssembly`; the host then owns execution, lifecycle, and manifestation through its own runtime contracts.

## Follow the reader's goal

- **I want to understand or write an FSM_COS consumer.** Start with [Consuming MicroBundles](CONSUMING_MICROBUNDLES.md), then run the [Minimal Consumer sample](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/samples/FSM_COS.MinimalConsumer/Program.cs).
- **I want to see a native desktop host.** Read the [AnyApp README](https://github.com/TrentBest/AnyApp/blob/development/README.md), then its [CI dependency modes](https://github.com/TrentBest/AnyApp/blob/development/docs/CI-DEPENDENCY-MODES.md).
- **I want to understand the intended browser host.** Read the [WebApp README](https://github.com/TrentBest/TheSingularityWorkshop.WebApp/blob/development/README.md) and its [architecture contract](https://github.com/TrentBest/TheSingularityWorkshop.WebApp/blob/development/docs/ARCHITECTURE.md).
- **I want to see the Workshop's existing browser proving ground.** Read [WebPage's README](https://github.com/TrentBest/WebPage/blob/development/README.md) and its [FSM_COS usage guide](https://github.com/TrentBest/WebPage/blob/development/docs/FSM_COS_USAGE.md).
- **I want to understand the kernel's stopping point.** Read [Architecture](ARCHITECTURE.md), [Runtime Boundary](RUNTIME_BOUNDARY.md), and [RuntimeAssembly](RUNTIME_ASSEMBLY.md).

## The shared composition boundary

```text
host chooses an Experience / manifest
                  |
                  v
      host-owned MicroBundle catalog
                  |
                  v
             FSM_COS
       resolve dependency closure
       validate before loading
       load dependencies first
       arbitrate to convergence
                  |
                  v
           RuntimeAssembly
                  |
                  v
     canonical host runtime + UI
```

The catalog is the resolution seam, not the owner of dependency traversal. FSM_COS does not become a GUI, web server, repository, artifact materializer, application loop, or lifecycle manager. A successful composition proves that an assembly was produced; it does **not by itself** prove that a host executes the Experience correctly. See [Consuming MicroBundles](CONSUMING_MICROBUNDLES.md) for the inputs, guarantees, and failure boundaries.

## Host comparison: source facts versus remaining work

| Host | Role | What current development documentation establishes | What must not be claimed yet |
|---|---|---|---|
| **AnyApp** | Native desktop host and first native RuntimeAssembly consumer | The README documents manifest selection, FSM_COS composition, RuntimeAssembly handoff, and WPF presentation. Its development CI checks sibling dependency sources; its package-compatibility path restores from public NuGet. | Source integration and published-package compatibility are separate gates. The AnyApp CI dependency guide explicitly records missing public NuGet packages as a package-mode blocker. Do not treat a source-mode pass as package-mode proof. |
| **WebApp** | Browser-facing host role: translate web input into application requests and present observable results | The architecture guide defines WebApp as AnyApp's web manifestation—not a client/server wrapper around the WPF executable—and identifies the request/result seam. | The current WebApp code is documented as transitional: `WebAppCosRuntime` composes only its local Moniker MicroBundle, while `WebAppExperienceRuntime` creates and ticks page/actor FSMs on a private heartbeat. This is not yet proof of canonical, manifest-driven Experience execution. |
| **WebPage** | Existing browser experience and proving ground | Its README documents a real local browser journey and identifies the current manifest/FSM_COS path, along with transitional presentation paths. | WebPage is not the architectural definition of WebApp. A working page or successful composition alone does not prove that every visible behavior is driven by the canonical RuntimeAssembly and execution owner. |

These status notes were checked against the repositories' `development` documentation and project files on 2026-10-21. They are a source review, **not a fresh build/test run**. Check the linked host CI and exact commit before making a release or runtime-correctness claim.

## AnyApp: the desktop consumer

The documented native path is:

1. AnyApp selects an Experience manifest (the README distinguishes a local development manifest from an immutable repository-backed manifest).
2. It constructs the host-side resolution path and passes the request through FSM_COS.
3. FSM_COS returns a `RuntimeAssembly`.
4. AnyApp makes the composition observable through its native runtime and GUI.WPF.

The development workflow selects checked-out sibling repository sources; the package-compatibility workflow restores from the public NuGet feed. Those modes answer different questions and must stay separate. The package-mode guide currently records unavailable published dependencies, so do not call the released-package graph verified until that gate passes.

For commands, manifest details, and implementation-specific behavior, use the [AnyApp README](https://github.com/TrentBest/AnyApp/blob/development/README.md), not a copied command list here.

## WebApp: the browser host contract

WebApp's intended job is to accept web-authored or web-delivered Experience content, validate its request and artifact identity, compose through FSM_COS, and present observable results in the browser. It must not require the desktop AnyApp executable to be running and must not invent a competing semantic tick loop.

The current architecture notes identify an additional unresolved boundary: a repository can return hash-verified artifact bytes, while FSM_COS's catalog contract resolves an already-instantiated `IMicroBundle`. Those are not the same operation. A reviewed materialization/registration contract must bridge them while preserving version and content identity; arbitrary downloaded assembly bytes must not be blindly loaded or executed.

The authoritative migration requirements and tests belong in the [WebApp architecture guide](https://github.com/TrentBest/TheSingularityWorkshop.WebApp/blob/development/docs/ARCHITECTURE.md). This map intentionally does not duplicate that guide's full security and implementation plan.

## WebPage: evidence, not the target architecture

WebPage is useful because it offers a browser-facing place to observe the Workshop. Its own documentation distinguishes the current working vertical slice from the desired fully manifest-driven path. Preserve that distinction when writing examples: a visible effect can prove presentation works, but it cannot alone prove which runtime owns the effect or that a composed Experience is executing canonically.

Use [FSM_COS WebPage Integration](WEBPAGE_INTEGRATION.md) for the kernel-side boundary and WebPage's own [FSM_COS usage guide](https://github.com/TrentBest/WebPage/blob/development/docs/FSM_COS_USAGE.md) for host-side implementation details. If the two disagree, verify both against the current source and tests rather than silently copying one account over the other.

## Evidence rules for future updates

Before upgrading any status from transitional to proven, link the exact evidence:

- **Composition:** a deterministic test shows the intended manifest and dependency closure produce the expected RuntimeAssembly.
- **Execution:** an integration test demonstrates that the canonical execution owner advances the composed Experience.
- **Presentation:** a host test observes the returned state/result without a parallel semantic lifecycle.
- **Artifact trust:** tests prove identity/hash validation and safe materialization into the catalog's `IMicroBundle` contract.
- **CI mode:** record source-integration and public-package compatibility separately, each against the exact candidate commit.
- **Visual teaching:** diagrams explain the verified path and mark proposed/future steps as such. Animation is not live telemetry unless it is actually connected to runtime data.

Historical planning notes are useful context, not current contracts or release evidence. Preserve them when they explain decisions, but keep the active route short and make the document's status and authority explicit.

---

[Documentation index](../DOCUMENTATION_INDEX.md) · [Release readiness gate](0.1.0-alpha.7-READINESS.md)
