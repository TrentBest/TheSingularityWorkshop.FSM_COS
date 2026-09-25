# The Singularity Workshop — FSM_COS

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![NuGet version](https://img.shields.io/nuget/v/TheSingularityWorkshop.FSM_COS?style=flat-square&logo=nuget&logoColor=white)](https://www.nuget.org/packages/TheSingularityWorkshop.FSM_COS)
[![NuGet downloads](https://img.shields.io/nuget/dt/TheSingularityWorkshop.FSM_COS?logo=nuget&style=flat-square)](https://www.nuget.org/packages/TheSingularityWorkshop.FSM_COS)

[![Build Status](https://img.shields.io/github/actions/workflow/status/TrentBest/TheSingularityWorkshop.FSM_COS/package.yml?branch=master&style=flat-square&logo=github)](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/actions/workflows/package.yml)
[![Last commit](https://img.shields.io/github/last-commit/TrentBest/TheSingularityWorkshop.FSM_COS/master)](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/commits/master)
[![Code Coverage](https://codecov.io/gh/TrentBest/TheSingularityWorkshop.FSM_COS/graph/badge.svg)](https://codecov.io/gh/TrentBest/TheSingularityWorkshop.FSM_COS)

**FSM_COS is the composition system.**

It takes a runtime manifest and assembles the MicroBundles, dependencies, configuration, and runtime components required by that manifest.

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

> **FSM_COS is the crane that assembles the machine. It does not become the machine.**

## Why this repository exists

FSM_COS is the repository for the **composition-of-systems boundary** in The Singularity Workshop architecture.

It is deliberately separate from:

- **FSM_API** — behavioral/state-machine primitives.
- **TheSingularityWorkshop.GUI** — semantic GUI construction and platform manifestation.
- **WebPage / WebForge** — a browser host and proving ground.
- **Unity-facing adapters or packages** — platform integration, not the composition kernel.
- **Warehouse infrastructure** — storage and delivery of data.
- **Experiences** — things a user encounters and executes.

FSM_COS answers one narrower question:

> **Given a published runtime manifest, what must be assembled before that runtime can be handed to a host?**

The repository therefore owns the composition contracts, dependency closure, configured loading, arbitration, convergence, and the resulting RuntimeAssembly.

## Current alpha boundary

The current 0.1.0-alpha.1 implementation is intentionally a small vertical slice:

~~~text
RuntimeManifest
    ↓
BundleRequest
    ↓
MicroBundleCatalog
    ↓
dependency closure
    ↓
configured Load()
    ↓
Arbitrate() rounds
    ↓
RuntimeAssembly
~~~

It does **not** yet own:

- host execution scheduling;
- GUI rendering;
- browser, desktop, or Unity lifecycle;
- Warehouse allocation;
- telemetry or metaDev adaptation;
- networking;
- Experience presentation;
- general application configuration.

Those concerns can become inputs, providers, or later composition layers without turning FSM_COS into an application framework.

## The manifest is the center

A RuntimeManifest is a **published runtime request**, not an application configuration file.

Editor/tooling systems may know rich information:

- human-readable names;
- ontology relationships;
- variants;
- dependency relationships;
- provenance;
- visual/editor structure;
- authoring metadata.

That information can be validated and baked into compact machine-oriented IDs and opaque configuration before runtime.

FSM_COS consumes the published representation.

See [Runtime Manifest Theory](docs/MANIFEST_THEORY.md).

## MicroBundles

A MicroBundle is **micro in focus, not necessarily in byte size**.

FSM_COS does not care whether a bundle is physically tiny or enormous. It cares that the bundle has a focused composition responsibility and exposes the contract required for assembly.

The current contract is:

~~~csharp
public interface IMicroBundle
{
    ulong Id { get; }
    IReadOnlyList<BundleRequest> Dependencies { get; }
    void Load(MicroBundleLoadContext context);
    bool Arbitrate(ArbitrationContext context, int roundIndex);
}
~~~

A bundle can therefore participate in composition without knowing whether it will ultimately manifest in WebForge, AnyApp, Unity, Desktop Forge, or another host.

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

BundleRequest carries a bundle ID plus opaque configuration bytes.

That matters because a dependency can be **entangled** with the request that caused it to load:

~~~text
A requests B + configuration X
B requests C + configuration Y
~~~

FSM_COS transports the configuration. The bundle that owns it interprets it.

This keeps the composition engine independent from the serialization format and domain meaning of configuration.

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

See [Arbitration and Convergence](docs/ARBITRATION.md).

## RuntimeAssembly is the handoff

RuntimeAssembly is the result of composition.

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
│       ├── BundleRequest.cs
│       ├── IMicroBundle.cs
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

## Documentation

This repository carries its own architecture and theory. The documents here describe **FSM_COS itself**, rather than asking another repository to explain its internals.

- [Theory](docs/THEORY.md) — why the composition boundary exists.
- [Architecture](docs/ARCHITECTURE.md) — contracts and runtime flow.
- [Runtime Manifest Theory](docs/MANIFEST_THEORY.md) — the published request model.
- [Arbitration and Convergence](docs/ARBITRATION.md) — reconciliation semantics.
- [Runtime Boundary](docs/RUNTIME_BOUNDARY.md) — what belongs here versus in hosts and neighboring systems.
- [Development](docs/DEVELOPMENT.md) — how to evolve and verify the repository.

## Package

**Package:** TheSingularityWorkshop.FSM_COS  
**Version:** 0.1.0-alpha.1  
**Target:** .NET 8  
**License:** MIT

The package is published to The Singularity Workshop's GitHub Packages feed and NuGet.org by the repository's Actions workflow.

## Design invariant

~~~text
FSM_API      → behavior
FSM_COS      → composition
MicroBundle  → focused capability/content/behavior
Warehouse    → storage and delivery
Host         → execution / manifestation
Experience   → what is encountered
~~~

The boundaries can evolve. The responsibility of FSM_COS should remain clear:

> **Assemble what was requested. Return a stable composition. Hand it to the host.**
