# The Singularity Workshop — FSM_COS

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![NuGet version](https://img.shields.io/nuget/v/TheSingularityWorkshop.FSM_COS?style=flat-square&logo=nuget&logoColor=white)](https://www.nuget.org/packages/TheSingularityWorkshop.FSM_COS)
[![NuGet downloads](https://img.shields.io/nuget/dt/TheSingularityWorkshop.FSM_COS?logo=nuget&style=flat-square)](https://www.nuget.org/packages/TheSingularityWorkshop.FSM_COS)

[![Build Status](https://img.shields.io/github/actions/workflow/status/TrentBest/TheSingularityWorkshop.FSM_COS/package.yml?branch=master&style=flat-square&logo=github)](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/actions/workflows/package.yml)
[![Last commit](https://img.shields.io/github/last-commit/TrentBest/TheSingularityWorkshop.FSM_COS/master)](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/commits/master)
[![Code Coverage](https://codecov.io/gh/TrentBest/TheSingularityWorkshop.FSM_COS/graph/badge.svg)](https://codecov.io/gh/TrentBest/TheSingularityWorkshop.FSM_COS)

**FSM_COS is a platform-neutral computation platform for composing computation.**

> **Engineering identity:** FSM_COS is deliberately a computation platform, not an application platform. It does not know whether the computation will become a WebPage, WebApp, AnyApp, DistributedApp, desktop tool, service, simulation, spreadsheet-like system, or something with no user interface at all.

Its job is the common scheduling and operational overhead required to turn a defined computation into an assembled runtime that another system can execute or manifest.

FSM_COS intentionally keeps its dependency floor small. It consumes **FSM_API** for the state/context primitives required by composition and **MicroBundleDomain** for the canonical MicroBundle contract. The current project also has a direct dependency on **FSM_UserIO** (`0.1.0-alpha.1`); whether semantic-intent exchange belongs in the mandatory composition floor remains an explicit audit item, so it should not be described as optional until source usage and the public contract are verified. FSM_COS is deliberately **not an application framework**: no browser, desktop UI, Unity runtime, renderer, database, warehouse, or product type is built into the kernel.

<p align="center">
  <img src="docs/assets/fsm-cos-crane.svg" alt="FSM_COS composition crane">
</p>

<p align="center"><em>Runtime request → composition → stable assembly → host manifestation</em></p>

It takes a [Runtime Manifest](docs/RUNTIME_MANIFEST.md), resolves the requested computation, and produces a stable [RuntimeAssembly](docs/RUNTIME_ASSEMBLY.md) for another system to execute or manifest.

~~~text
Runtime Manifest
      │
      ▼
   FSM_COS
      │
      ├── resolve MicroBundles
      ├── resolve dependencies
      ├── propagate configuration
      ├── load/install bundles
      ├── arbitrate toward convergence
      ▼
RuntimeAssembly
~~~

> **FSM_COS assembles the computation. It does not become the application.**

## What FSM_COS actually is

<p align="center">
  <img src="docs/assets/fsm-cos-system.svg" alt="FSM_COS composition kernel between authoring and host manifestation">
</p>

FSM_COS is not simply a bundle loader. It is the **composition kernel**: the layer that turns a published runtime request into a stable assembled composition.

The central distinction is:

> **The manifest says what is requested. FSM_COS determines what must exist together. RuntimeAssembly says that composition is ready for handoff. The host decides what happens next.**

The deeper explanation lives in [FSM_COS Theory](docs/THEORY.md). The README uses that theory as a map and links into it repeatedly so the implementation and the architectural model stay connected.

### The computation boundary

```text
computation request
      ↓
Runtime Manifest
      ↓
    FSM_COS
      ├── dependency closure
      ├── optional configuration
      ├── loading
      └── arbitration / convergence
      ↓
RuntimeAssembly
      ↓
execution / manifestation / next system
```

**FSM_COS assembles the system. It does not become the system.**

See [Theory — what “composition of systems” means](docs/THEORY.md#1-what-does-composition-of-systems-mean) and [Runtime Boundary](docs/RUNTIME_BOUNDARY.md).

## Why this repository exists

FSM_COS is the repository for the **common computation-composition boundary** in The Singularity Workshop architecture.

It is deliberately separate from:

- **FSM_API** — behavioral/state-machine primitives.
- **TheSingularityWorkshop.GUI** — semantic GUI construction and platform manifestation.
- **WebPage / WebForge** — a browser host and proving ground.
- **Unity-facing adapters or packages** — platform integration, not the composition kernel.
- **Warehouse infrastructure** — storage and delivery of data.
- **Experiences** — things a user encounters and executes.

FSM_COS answers one deliberately general question:

> **Given a published computation request, what must be assembled before the resulting computation can be handed to another system?**

The repository therefore owns the composition contracts, dependency closure, configured loading, arbitration, convergence, and the resulting RuntimeAssembly.

## Current development boundary

The current `development` line is the active architecture workstream. The project file currently declares `0.1.0-alpha.6`; this source declaration is not a claim that alpha.6 has been published to NuGet. The runtime contract and documentation are still being refined:

~~~text
RuntimeManifest
    ↓
MicroBundle identity + version
    ↓
MicroBundle catalog / resolver
    ↓
optional configuration source
    ↓
dependency closure
    ↓
Load()
    ↓
Arbitrate() rounds
    ↓
RuntimeAssembly
~~~

It does **not** own:

- application-specific execution scheduling;
- GUI rendering;
- browser, desktop, or Unity lifecycle;
- Warehouse allocation;
- telemetry or metaDev adaptation;
- networking;
- Experience presentation;
- general application configuration.

Those concerns can become inputs, MicroBundles, or host-side layers without turning FSM_COS into an application framework.

### The design target is deliberately broader than any current host

The same kernel can be used to assemble computation for a WebPage, WebApp, AnyApp, DistributedApp, service, tool, simulation, data processor, spreadsheet-like application, or a system that has no user interface at all.

The application is **not** the abstraction. The computation is.

FSM_COS therefore has no architectural preference for:

- web over desktop;
- desktop over distributed;
- interactive over headless;
- GUI over command line;
- game over business software;
- one runtime platform over another.

Those are manifestations or application choices made outside the kernel.

## Architecture visuals

Git is static. The architecture does not have to *feel* static.

FSM_COS uses static SVG architecture art as the canonical visual language. The crane is a deliberately simple symbol: it represents the composition boundary doing work, not a UI animation.

The rule for repository visuals is simple: **illustrate the concept, not the decoration**. Architecture diagrams should remain readable, versionable, and useful in GitHub, package documentation, and generated documentation.

## Visual map

The documentation diagrams are deliberately architecture-first: they show where responsibility lives, what crosses the FSM_COS boundary, and where composition stops.

- [FSM_COS system overview](docs/assets/fsm-cos-system.svg)
- [Composition boundary](docs/assets/composition-boundary.svg)
- [Composition overview](docs/assets/fsm-cos-overview.svg)
- [Runtime Manifest publication pipeline](docs/assets/runtime-manifest-pipeline.svg)
- [Dependency resolution](docs/assets/dependency-resolution.svg)
- [Arbitration and convergence](docs/assets/arbitration-convergence.svg)
- [Runtime boundary](docs/assets/runtime-boundary.svg)
- [Composition crane](docs/assets/fsm-cos-crane.svg)
- [MicroBundle cartridge](docs/assets/microbundle-cartridge.svg)
- [RuntimeAssembly handoff](docs/assets/runtime-assembly-handoff.svg)

## The manifest is the center

<p align="center">
  <img src="docs/assets/runtime-manifest-pipeline.svg" alt="Runtime Manifest publication pipeline">
</p>

A [RuntimeManifest](docs/RUNTIME_MANIFEST.md) is a **published composition request**, not an application configuration file. The manifest identifies the MicroBundles and their requested versions. Per-MicroBundle configuration is a separate concern and may be absent; absence means the MicroBundle uses its defaults.

Editor/tooling systems may know rich information:

- human-readable names;
- ontology relationships;
- variants;
- dependency relationships;
- provenance;
- visual/editor structure;
- authoring metadata.

That information is authored and validated by the systems that own those domains. FSM_COS consumes the resulting machine-oriented manifest and optional configuration source; it does not define the authoring model.

FSM_COS consumes the published representation.

See [Runtime Manifest Theory](docs/MANIFEST_THEORY.md) and the deeper [FSM_COS Theory](docs/THEORY.md#3-why-the-manifest-exists).

## Consuming MicroBundles

FSM_COS does not define what a MicroBundle means. **MicroBundleDomain owns that domain.**

FSM_COS consumes the domain-owned contract through an application- or repository-supplied catalog.

```text
MicroBundleDomain
      │ defines
      ▼
IMicroBundle
      │ consumed by
      ▼
FSM_COS
      │
      ├── resolve
      ├── load
      ├── arbitrate
      └── hand off
```

A developer is free to supply MicroBundles from memory, a repository, local storage, remote delivery, generated resources, or another source. FSM_COS only requires the composition boundary represented by `IMicroBundleCatalog`.

See [Consuming MicroBundles](docs/CONSUMING_MICROBUNDLES.md) for concrete usage patterns.

## Dependency resolution

The manifest declares roots. Dependencies are discovered from those roots.

For:

~~~text
A
├── B
│   └── C
└── D
~~~

FSM_COS installs:

~~~text
C → B → D → A
~~~

before arbitration begins.

Cycles are composition errors. Missing bundles are composition errors. FSM_COS does not guess around either condition.

## Configuration

Configuration is deliberately **outside the Runtime Manifest**.

~~~text
Manifest
  └── MicroBundle identity + requested version

Configuration source
  ├── MicroBundle A configuration
  ├── MicroBundle B configuration
  └── optional entries
~~~

FSM_COS may receive configuration through an abstraction supplied by the host or repository layer. It does not read files, choose a file format, or interpret domain configuration.

If a MicroBundle has no configuration entry, it is loaded with no external configuration and therefore uses its own defaults.

This keeps three responsibilities distinct:

- **Manifest** — which MicroBundles and versions are requested.
- **Configuration source** — how a particular MicroBundle is configured for this runtime.
- **FSM_COS** — how the resulting composition is resolved, loaded, and arbitrated.

When configuration becomes a **serialized representation**, FSM_COS deliberately points downward to **[TheSingularityWorkshop.FSM_Serialization](https://github.com/TrentBest/TheSingularityWorkshop.FSM_Serialization)** rather than defining another serializer here. See [FSM_COS Theory — Composition is not serialization](docs/THEORY.md#14-composition-is-not-serialization) and the [FSM_Serialization Theory](https://github.com/TrentBest/TheSingularityWorkshop.FSM_Serialization/blob/master/docs/THEORY.md).

## Arbitration

Loading establishes the initial composition.

Arbitration allows the installed set to reconcile itself:

~~~text
load
  ↓
round
  ↓
changed?
  ├── yes → another round
  └── no  → stable RuntimeAssembly
~~~

The current default maximum is **10 rounds**.

A bundle returns true when its participation changed the composition and false when it did not. A complete round with no changes is convergence.

Non-convergence is an error. FSM_COS does not return an assembly that it knows is unstable.

See [Arbitration and Convergence](docs/ARBITRATION.md) and [FSM_COS Theory — Arbitration is composition negotiation](docs/THEORY.md#8-arbitration-is-composition-negotiation).

## RuntimeAssembly is the handoff

[RuntimeAssembly](docs/RUNTIME_ASSEMBLY.md) is the result of composition.

<p align="center">
  <img src="docs/assets/runtime-assembly-handoff.svg" alt="RuntimeAssembly as the handoff between FSM_COS and a host">
</p>

It records the runtime identity, loaded MicroBundles, and arbitration result. It is not the application and it is not a renderer.

A host receives the assembled result and decides how to execute, present, or encounter it.

~~~text
                  RuntimeAssembly
                         │
          ┌──────────────┼──────────────┐
          ▼              ▼              ▼
       WebForge        AnyApp      another host
          │
        GUI
          │
       browser
~~~

**Composition is not manifestation.**

## Repository structure

~~~text
TheSingularityWorkshop.FSM_COS/
│
├── README.md
├── LICENSE.txt
├── TheSingularityWorkshop.FSM_COS.sln
├── TheSingularityWorkshop.FSM_COS.slnx
│
├── docs/
│   ├── THEORY.md
│   ├── ARCHITECTURE.md
│   ├── RUNTIME_MANIFEST.md
│   ├── CONSUMING_MICROBUNDLES.md
│   ├── RUNTIME_ASSEMBLY.md
│   ├── MANIFEST_THEORY.md
│   ├── ARBITRATION.md
│   ├── RUNTIME_BOUNDARY.md
│   └── DEVELOPMENT.md
│
├── src/
│   └── FSM_COS/
│       ├── FSM_COS.csproj
│       ├── IFsmCos.cs
│       ├── FsmCos.cs
│       ├── RuntimeManifest.cs
│       ├── IMicroBundleCatalog.cs
│       ├── MicroBundleLoadContext.cs
│       ├── ArbitrationContext.cs
│       └── RuntimeAssembly.cs
│
└── tests/
    └── FSM_COS.Tests/
        └── FsmCosTests.cs
~~~

There is intentionally **one canonical FSM_COS implementation project**: src/FSM_COS/FSM_COS.csproj.

The old root scaffold that contained only Class1.cs has been removed. The solution now points at the real project.

<p align="center">
  <img src="docs/assets/ai-composition.svg" alt="FSM_COS AI composition: ProtocolAI and GrammarAI assembled as runtime capabilities" width="1100">
</p>

<p align="center"><strong>AI capabilities are composed like any other capability; FSM_COS does not become the AI framework.</strong></p>

## Documentation

This repository follows the shared [Documentation Standard](docs/ECOSYSTEM_DOCUMENTATION_STANDARD.md). Use the [Documentation Index](docs/DOCUMENTATION_INDEX.md) to choose a reading path by goal; the README is the entry point, while focused documents remain authoritative for theory, architecture, and individual contracts.

This repository carries its own architecture and theory. The documents here describe **FSM_COS itself**, rather than asking another repository to explain its internals.

- [Theory](docs/THEORY.md) — why the composition boundary exists.
- [Architecture](docs/ARCHITECTURE.md) — contracts and runtime flow.
- [Runtime Manifest](docs/RUNTIME_MANIFEST.md) — the published request itself, with examples and the alpha contract.
- [Consuming MicroBundles](docs/CONSUMING_MICROBUNDLES.md) — the composable unit contract.
- [RuntimeAssembly](docs/RUNTIME_ASSEMBLY.md) — the stable handoff object.
- [Runtime Manifest Theory](docs/MANIFEST_THEORY.md) — the deeper theory behind publication.
- [Arbitration and Convergence](docs/ARBITRATION.md) — reconciliation semantics.
- [Runtime Boundary](docs/RUNTIME_BOUNDARY.md) — what belongs here versus in hosts and neighboring systems.
- [Development](docs/DEVELOPMENT.md) — how to evolve and verify the repository.
- [WebPage Integration](docs/WEBPAGE_INTEGRATION.md) — how the browser host consumes FSM_COS without pulling platform concerns into the kernel.

## Package

**Package:** TheSingularityWorkshop.FSM_COS  
**Version:** 0.1.0-alpha.5  
**Development dependency:** TheSingularityWorkshop.MicroBundleDomain 1.0.1  
**Target:** .NET 8  
**License:** MIT

The repository contains the packaging and trusted-publishing workflow for GitHub Packages and NuGet.org. Publishing is an explicit workflow-dispatch action; the current alpha package is published through the repository's trusted-publishing workflow.

## Host integration

WebPage is one host and proving ground for FSM_COS; it is not a special case inside the composition kernel. The intended flow is:

~~~text
WebPage published manifest
        ↓
     FsmCos
        ↓
IMicroBundleCatalog supplied by WebPage
        ↓
 RuntimeAssembly
        ↓
WebPage / GUI manifestation
        ↓
     Blazor/browser
~~~

WebPage should reference the TheSingularityWorkshop.FSM_COS package and implement the catalog/host boundary around it. Platform-specific lifecycle, browser APIs, GUI rendering, and Experience presentation remain outside FSM_COS.

The first AI/GUI vertical slice now follows the same boundary: WebPage supplies ProtocolAI, GrammarAI, and GUI-facing MicroBundles through its catalog; FSM_COS composes them and returns them through RuntimeAssembly; the WebPage host uses the shared GUI builder for manifestation and owns clipboard/provider interaction. This keeps the package reusable while proving that the semantic exchange can be assembled as ordinary runtime capability.

For the concrete integration contract, see [WebPage Integration](docs/WEBPAGE_INTEGRATION.md) and [FSM_COS Theory — Same composition, different manifestation](docs/THEORY.md#10-same-composition-different-manifestation).

## Design invariant

~~~text
FSM_API      → behavior/state primitives
FSM_COS      → computation composition + stable assembly
MicroBundle  → focused domain capability/content/behavior
Repository   → artifact discovery and delivery
Serialization→ representation
Host         → execution / manifestation
Application  → whatever the assembled computation is used to build
~~~

The boundaries can evolve. The responsibility of FSM_COS should remain clear:

> **Assemble the requested computation. Return a stable composition. Hand it to whatever executes or manifests it.**

---

## 🔗 Resources & Support

### 📦 Get the core packages

- **FSM_API:** [Core NuGet](https://www.nuget.org/packages/TheSingularityWorkshop.FSM_API) · [Source](https://github.com/TrentBest/FSM_API)
- **FSM_COS:** [NuGet](https://www.nuget.org/packages/TheSingularityWorkshop.FSM_COS) · [Source](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS)
- **FSM_Serialization:** [NuGet](https://www.nuget.org/packages/TheSingularityWorkshop.FSM_Serialization) · [Source](https://github.com/TrentBest/TheSingularityWorkshop.FSM_Serialization)

### 💖 Support The Singularity Workshop

- **Patreon:** [Support us on Patreon](https://www.patreon.com/c/TheSingularityWorkshop)
- **PayPal:** [Make a donation](https://www.paypal.com/donate/?hosted_button_id=3Z7263LCQMV9J)

<p align="center">
  <a href="https://github.com/TrentBest/FSM_API">
    <img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" height="200">
  </a>
</p>

<p align="center">
  <em>The Singularity Workshop — Tools for the curious, the bold, and the systemically inclined.</em><br>
  <strong>Because state shouldn't be a mess.</strong>
</p>


---

## 🔗 The Singularity Workshop

FSM_COS is one layer in a deliberately troublesome ecosystem:

- **[FSM_API](https://github.com/TrentBest/FSM_API)** — behavior and state.
- **[FSM_COS](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS)** — composition and runtime assembly.
- **[FSM_Serialization](https://github.com/TrentBest/TheSingularityWorkshop.FSM_Serialization)** — representation and the byte boundary.
- **[WebPage](https://github.com/TrentBest/WebPage)** — browser manifestation and proving ground.
- **[FSM_API_Unity](https://github.com/TrentBest/FSM_API_Unity)** — Unity manifestation.

<p align="center"><em>The Singularity Workshop — Tools for the curious, the bold, and the systemically inclined.</em><br><strong>Because state shouldn't be a mess.</strong><br><em>And because static boundaries are invitations to cause trouble.</em></p>
