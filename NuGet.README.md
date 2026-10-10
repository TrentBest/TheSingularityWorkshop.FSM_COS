# FSM_COS — Runtime Composition Kernel

![FSM_COS composition flow: a runtime manifest enters FSM_COS, which resolves and composes MicroBundles into a RuntimeAssembly for a host.](https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.FSM_COS/development/docs/assets/fsm-cos-overview.svg)

**FSM_COS composes requested capabilities into a `RuntimeAssembly`. It is a composition boundary—not an application, GUI, engine, repository, or execution loop.**

## 🟦 01 — What is FSM_COS?

A `RuntimeManifest` identifies the MicroBundles and versions a caller wants. FSM_COS resolves their dependencies, loads the composition, performs bounded arbitration, and returns a `RuntimeAssembly` for the host to use.

## 🟣 02 — Why does it exist?

Hosts should not need to duplicate the same composition and dependency logic for every experience. FSM_COS gives that responsibility a focused home while leaving application behavior and presentation to the host.

Compatibility is not automatic: the selected bundles, their contracts, and the host must work together.

## 🩵 03 — How does it work?

```text
RuntimeManifest → FSM_COS → RuntimeAssembly → Host
                    │
                    ├─ resolve MicroBundles and dependencies
                    ├─ load the composition
                    └─ arbitrate toward convergence
```

- **MicroBundleDomain** owns the MicroBundle contract.
- **The host or repository** supplies the catalog used to resolve bundles.
- **FSM_COS** composes the requested capabilities.
- **The host** owns execution, lifecycle, UI, and presentation.

## 🟢 04 — See it in a minute

Install the currently published version into a .NET project:

```bash
dotnet add package TheSingularityWorkshop.FSM_COS
```

FSM_COS needs a catalog supplied by the host, so installation alone does not create an application. To see dependency resolution proven by a runnable test, clone the source repository and run:

```bash
git clone https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS.git
cd TheSingularityWorkshop.FSM_COS
dotnet test tests/FSM_COS.Tests/FSM_COS.Tests.csproj --filter "FullyQualifiedName~Execute_loads_dependencies_before_requesting_bundle"
```

The test proves that when bundle `1` depends on bundle `2`, the resulting assembly orders them `2 → 1`. For a consumer's own catalog and bundles, follow [Consuming MicroBundles](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/docs/CONSUMING_MICROBUNDLES.md).

The package targets **.NET 8**. Its direct dependencies are:

- [FSM_API](https://www.nuget.org/packages/TheSingularityWorkshop.FSM_API) — state/context primitives.
- [MicroBundleDomain](https://www.nuget.org/packages/TheSingularityWorkshop.MicroBundleDomain) — the MicroBundle contract.
- [FSM_UserIO](https://www.nuget.org/packages/TheSingularityWorkshop.FSM_UserIO) — the platform-neutral `SemanticIntent` boundary.

This source currently declares version `0.1.0-alpha.6`. A version declaration in source or documentation is not proof that a package is published; check the [NuGet package page](https://www.nuget.org/packages/TheSingularityWorkshop.FSM_COS) for the version actually available.

## 🟪 05 — Available documentation and theory

- [Project overview](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/README.md) — the bigger picture and quick proof.
- [Theory](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/docs/THEORY.md) — why composition is a distinct operation.
- [Architecture](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/docs/ARCHITECTURE.md) — ownership and runtime flow.
- [Runtime Manifest](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/docs/RUNTIME_MANIFEST.md) — the composition request.
- [RuntimeAssembly](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/docs/RUNTIME_ASSEMBLY.md) — the host handoff.
- [Arbitration](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/docs/ARBITRATION.md) — reconciliation and convergence.
- [Documentation Index](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/DOCUMENTATION_INDEX.md) — all focused guides.
- [Development](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/docs/DEVELOPMENT.md) — build and verification.
- [Source and issues](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS)

---

[![The Singularity Workshop](https://github.com/TrentBest.png?size=200)](https://github.com/TrentBest)

[GitHub](https://github.com/TrentBest) · [Coder Legion](https://coderlegion.com/) · [Patreon](https://www.patreon.com/c/TheSingularityWorkshop) · [PayPal](https://www.paypal.com/donate/?hosted_button_id=3Z7263LCQMV9J) · [FSM_API](https://github.com/TrentBest/FSM_API) · [FSM_Serialization](https://github.com/TrentBest/TheSingularityWorkshop.FSM_Serialization) · [WebPage](https://github.com/TrentBest/WebPage)

*The Singularity Workshop — Tools for the curious, the bold, and the systemically inclined.*

**Because state shouldn't be a mess.**

*And because static boundaries are invitations to cause trouble.*
