# ✳️ 00 The Singularity Workshop — FSM_COS

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![NuGet version](https://img.shields.io/nuget/v/TheSingularityWorkshop.FSM_COS?style=flat-square&logo=nuget&logoColor=white)](https://www.nuget.org/packages/TheSingularityWorkshop.FSM_COS)
[![NuGet downloads](https://img.shields.io/nuget/dt/TheSingularityWorkshop.FSM_COS?logo=nuget&style=flat-square)](https://www.nuget.org/packages/TheSingularityWorkshop.FSM_COS)
[![Build Status](https://img.shields.io/github/actions/workflow/status/TrentBest/TheSingularityWorkshop.FSM_COS/package.yml?branch=master&style=flat-square&logo=github)](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/actions/workflows/package.yml)
[![Code Coverage](https://codecov.io/gh/TrentBest/TheSingularityWorkshop.FSM_COS/graph/badge.svg)](https://codecov.io/gh/TrentBest/TheSingularityWorkshop.FSM_COS)

<p align="center">
  <img src="docs/assets/fsm-cos-overview.svg" alt="A runtime manifest enters FSM_COS; the kernel resolves dependencies and composes MicroBundles into a RuntimeAssembly for a host." width="1000">
</p>

FSM_COS is The Singularity Workshop's **runtime composition kernel**. It takes a request for capabilities, resolves the MicroBundles and dependencies needed to satisfy it, and hands the assembled result to a host.

## 🟦 01 — What is FSM_COS?

Think of a runtime manifest as an order: it says which capabilities are wanted. FSM_COS works out what else must be present for that order to make sense, loads the composition, and checks whether its parts can reach a stable agreement.

The result is a `RuntimeAssembly`—a handoff to another system, not a finished application.

**In one line:** the manifest says what is requested; FSM_COS assembles what must exist together; the host decides what happens next.

## 🟣 02 — Why give composition its own boundary?

Without a distinct composition layer, each host can end up owning its own dependency logic and capability wiring. That makes reusable behavior harder to share and encourages application-specific concerns to leak into lower-level libraries.

FSM_COS gives that work a focused home. A browser app, desktop program, service, simulation, or headless tool can supply its own catalog and decide how to use the resulting assembly. The kernel does not require a particular interface or application type.

This is an architectural boundary, not a promise that every capability is automatically portable. Compatibility still depends on the contracts, dependencies, and abilities of the host.

## 🟦 03 — How does the composition flow work?

```text
Runtime Manifest
      │ requested MicroBundles and versions
      ▼
   FSM_COS
      ├── resolve requested bundles
      ├── discover dependencies
      ├── apply available configuration
      ├── load the composition
      └── arbitrate toward convergence
      ▼
RuntimeAssembly
      │ handoff
      ▼
Host / application / experience
```

Each part has a clear owner:

- **MicroBundleDomain** defines the MicroBundle contract.
- **The host or repository** supplies a catalog that can find the requested bundles.
- **FSM_COS** resolves dependencies, loads the composition, and runs bounded arbitration.
- **The host** owns application lifecycle, execution, user interface, and presentation.

FSM_COS is not a GUI, engine, artifact repository, transport layer, or application loop. For the exact contracts and responsibility boundaries, see [Architecture](docs/ARCHITECTURE.md) and [Runtime Boundary](docs/RUNTIME_BOUNDARY.md).

## 🟢 04 — See the core behavior

A repository test demonstrates the central dependency rule: when bundle `1` depends on bundle `2`, requesting bundle `1` produces an assembly ordered `2 → 1`.

```csharp
var catalog = new TestCatalog(
    new TestBundle(2),
    new TestBundle(1, MicroBundleDependencyRequest.Unconfigured(2)));

var assembly = new FsmCos(catalog).Execute(
    new RuntimeManifest(42, new[] { Entry(1) }));

Assert.Equal(new ulong[] { 2, 1 }, assembly.Bundles.Select(x => x.Id));
```

This is a **test-fixture excerpt**, not a standalone application: `TestCatalog`, `TestBundle`, and `Entry` are helpers from the test project. It comes from `Execute_loads_dependencies_before_requesting_bundle`.

To run the proof from a repository clone:

```bash
dotnet test tests/FSM_COS.Tests/FSM_COS.Tests.csproj --filter "FullyQualifiedName~Execute_loads_dependencies_before_requesting_bundle"
```

For consumer setup and the full catalog contract, use [Consuming MicroBundles](docs/CONSUMING_MICROBUNDLES.md). The current source targets **.NET 8**. The `0.1.0-alpha.6` package is a candidate and is not published yet; check the [NuGet package page](https://www.nuget.org/packages/TheSingularityWorkshop.FSM_COS) for the actual released version.

## 🟪 05 — Choose your next step

Pick the question you want answered; each document focuses on one topic.

- **New to the idea?** [FSM_COS Theory](docs/THEORY.md) explains why composition is a distinct operation and develops the mental model.
- **Need the system map?** [Architecture](docs/ARCHITECTURE.md) explains component ownership and runtime flow.
- **Building a consumer?** [Consuming MicroBundles](docs/CONSUMING_MICROBUNDLES.md) shows how to provide a catalog and bundles.
- **Defining a request?** [Runtime Manifest](docs/RUNTIME_MANIFEST.md) explains requested bundle identities and versions; [Manifest Theory](docs/MANIFEST_THEORY.md) explains the design rationale.
- **Receiving the result?** [RuntimeAssembly](docs/RUNTIME_ASSEMBLY.md) describes the handoff object.
- **Wondering how stability is reached?** [Arbitration](docs/ARBITRATION.md) covers reconciliation and convergence.
- **Integrating a host?** [WebPage Integration](docs/WEBPAGE_INTEGRATION.md) documents one browser-host integration without moving browser concerns into the kernel.
- **Checking what is implemented versus intended?** [Ecosystem Integration Map](docs/ECOSYSTEM_INTEGRATION_MAP.md) separates verified relationships from future direction.
- **Building or contributing?** [Development](docs/DEVELOPMENT.md) covers repository setup, tests, and workflows.
- **Looking for another topic?** Open the [Documentation Index](DOCUMENTATION_INDEX.md).

---

<p align="center">
  <a href="https://github.com/TrentBest">
    <img src="https://avatars.githubusercontent.com/u/10436537?v=4" alt="The Singularity Workshop on GitHub" width="200">
  </a>
</p>

<p align="center">
  <a href="https://github.com/TrentBest">GitHub</a> ·
  <a href="https://coderlegion.com/">Coder Legion</a> ·
  <a href="https://www.patreon.com/">Patreon</a> ·
  <a href="https://www.paypal.com/">PayPal</a>
</p>

<p align="center"><em>The Singularity Workshop — Tools for the curious, the bold, and the systemically inclined.</em><br><strong>Because state shouldn't be a mess.</strong><br><em>And because static boundaries are invitations to cause trouble.</em></p>
