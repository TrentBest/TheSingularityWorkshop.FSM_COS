# The Singularity Workshop — FSM_COS

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![NuGet version](https://img.shields.io/nuget/v/TheSingularityWorkshop.FSM_COS?style=flat-square&logo=nuget&logoColor=white)](https://www.nuget.org/packages/TheSingularityWorkshop.FSM_COS)
[![NuGet downloads](https://img.shields.io/nuget/dt/TheSingularityWorkshop.FSM_COS?logo=nuget&style=flat-square)](https://www.nuget.org/packages/TheSingularityWorkshop.FSM_COS)
[![Build Status](https://img.shields.io/github/actions/workflow/status/TrentBest/TheSingularityWorkshop.FSM_COS/package.yml?branch=master&style=flat-square&logo=github)](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/actions/workflows/package.yml)
[![Code Coverage](https://codecov.io/gh/TrentBest/TheSingularityWorkshop.FSM_COS/graph/badge.svg)](https://codecov.io/gh/TrentBest/TheSingularityWorkshop.FSM_COS)

**FSM_COS is the composition kernel.**

It turns a published runtime request into a stable **RuntimeAssembly** by resolving MicroBundles, closing their dependency graph, carrying opaque configuration, loading the required capabilities, and arbitrating until the composition converges.

<p align="center">
  <img src="docs/assets/fsm-cos-crane.gif" alt="Animated industrial composition crane lifting a runtime assembly">
</p>

<p align="center"><em>Request → resolve → load → arbitrate → converge → hand off</em></p>

## The one-sentence definition

> **FSM_COS assembles independently defined capabilities into a stable runtime composition without becoming the host, the GUI, the repository, or the domain of those capabilities.**

That boundary is the reason this package exists.

---

## What FSM_COS owns

Given a `RuntimeManifest`, FSM_COS owns the composition operation:

```text
RuntimeManifest
      │
      ▼
   FSM_COS
      │
      ├── resolve requested MicroBundles
      ├── resolve dependency closure
      ├── carry configuration
      ├── load/install capabilities
      ├── arbitrate the installed composition
      ├── require convergence
      ▼
RuntimeAssembly
      │
      ▼
host / manifestation
```

FSM_COS therefore owns:

- manifest execution;
- dependency traversal and closure;
- deterministic installation order;
- configuration propagation without interpreting domain bytes;
- MicroBundle loading;
- bounded arbitration;
- convergence and non-convergence failure;
- the RuntimeAssembly handoff.

FSM_COS does **not** own:

- MicroBundle domain identity or metadata;
- artifact storage or delivery;
- serialization formats;
- GUI rendering;
- browser, desktop, or Unity lifecycle;
- Experience execution;
- application scheduling;
- Warehouse allocation;
- telemetry or metaDev adaptation.

The important rule is:

> **FSM_COS assembles. The host executes and manifests.**

See [Runtime Boundary](docs/RUNTIME_BOUNDARY.md).

---

## The dependency direction

FSM_COS is deliberately above the contracts it consumes.

```text
FSM_API
   │
   │ state/context primitive
   ▼
FSM_COS  ◄──── MicroBundleDomain
   │
   │ composition
   ▼
RuntimeAssembly
   │
   ▼
Host / Experience
```

The arrows here mean **dependency direction**: FSM_COS consumes both packages. Neither package needs to depend on FSM_COS merely to be useful to it.

### Runtime dependencies

| Package | Why FSM_COS uses it | What FSM_COS does not take from it |
|---|---|---|
| **FSM_API 1.0.13** | Supplies the existing state/context abstraction used at the composition boundary. | FSM_COS does not become an FSM host or redefine FSM_API behavior. |
| **MicroBundleDomain 1.0.1** | Supplies the canonical MicroBundle runtime contract: identity, version/providers, dependency requests, load context, and arbitration context. | FSM_COS does not redefine MicroBundle domain semantics. |

That distinction is important: **a dependency should be explained by the responsibility FSM_COS actually consumes, not by copying the dependency's documentation.**

### Deliberately absent runtime dependencies

FSM_COS does not require:

- FSM_Serialization — serialization is a neighboring representation boundary;
- MicroBundleRepository — discovery/materialization is supplied through a catalog boundary;
- FSM_REST — transport is outside composition;
- GUI packages — manifestation is outside composition;
- ProtocolAi / GrammarAi — capabilities may be composed, but AI protocol/grammar are not kernel dependencies;
- WebPage, AnyApp, Unity, Blazor, WPF, or another host — hosts consume FSM_COS rather than the reverse.

See [Dependency & Boundary Guide](docs/DEPENDENCIES.md).

---

## MicroBundles: the contract comes from MicroBundleDomain

FSM_COS **consumes** the MicroBundle contract. It does not define a competing one.

The canonical runtime contract is owned by [MicroBundleDomain](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleDomain):

```csharp
public interface IMicroBundle
{
    MicroBundleDescriptor Descriptor { get; }
    IReadOnlyList<MicroBundleDependencyRequest> Dependencies { get; }

    void Load(IMicroBundleLoadContext context);

    bool Arbitrate(
        IMicroBundleArbitrationContext context,
        int roundIndex);
}
```

The exact domain surface belongs to MicroBundleDomain. This README shows only the portion needed to understand FSM_COS's relationship to it.

For the complete contract, definitions, examples, and authoring model, read [MicroBundleDomain](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleDomain).

For FSM_COS's use of that contract, read [MicroBundles in FSM_COS](docs/MICROBUNDLES.md).

---

## The composition lifecycle

### 1. Request

A Runtime Manifest names the root capabilities required for a runtime.

### 2. Resolve

FSM_COS asks its supplied catalog for each requested MicroBundle and recursively follows the dependency requests declared by those bundles.

### 3. Close the graph

The reachable dependency graph is resolved before arbitration begins.

For:

```text
A
├── B
│   └── C
└── D
```

FSM_COS can install:

```text
C → B → D → A
```

A missing capability or dependency cycle is a composition failure. FSM_COS does not guess around either condition.

### 4. Load

FSM_COS supplies the appropriate `IMicroBundleLoadContext`. Configuration remains opaque to the kernel; the capability that owns the configuration owns its meaning.

### 5. Arbitrate

All installed participants can inspect the shared composition through the domain-owned arbitration context.

A participant returns `true` only when its participation changed the composition enough to require another round.

### 6. Converge

FSM_COS repeats arbitration up to its configured safety bound. A complete round with no changes is convergence. Failure to converge is an error.

### 7. Hand off

A successful run produces a **RuntimeAssembly**. The host decides what execution or manifestation means for that assembly.

---

## Why arbitration exists

Dependency resolution answers:

> **What must exist?**

Arbitration answers:

> **Now that these capabilities exist together, can their shared composition settle into a stable state?**

This is the difference between a loader and a composition system.

Arbitration is **not** a universal priority system and does not select a winner. Independent participants observe the same composition and can express consequences of that composition until the system reaches a stable point.

See [Arbitration and Convergence](docs/ARBITRATION.md).

---

## Configuration is intentionally opaque

FSM_COS carries configuration but does not define what configuration means.

```text
authoring / publication
        │
        ▼
opaque configuration
        │
        ▼
     FSM_COS
        │
        ▼
owning MicroBundle
        │
        ▼
domain interpretation
```

If the bytes need a concrete serialization format, that concern belongs to [FSM_Serialization](https://github.com/TrentBest/TheSingularityWorkshop.FSM_Serialization), not to the composition algorithm.

This lets serialization evolve independently from dependency resolution and convergence semantics.

See [Runtime Manifest](docs/RUNTIME_MANIFEST.md).

---

## RuntimeAssembly is the boundary

RuntimeAssembly is not an application object.

It is the statement:

> **The requested composition was assembled and reached the required stable arbitration result.**

```text
             RuntimeAssembly
                    │
       ┌────────────┼────────────┐
       ▼            ▼            ▼
    WebPage       AnyApp       another host
       │            │
   manifestation  execution
```

A host can consume the same composition without requiring FSM_COS to know how that host works.

See [RuntimeAssembly](docs/RUNTIME_ASSEMBLY.md).

---

## Scale: the composition boundary is compute-environment independent

FSM_COS is not tied to a particular machine, operating system, renderer, process, or network topology. **It is the boundary alignment for functionality.**

The same composition model can feed radically different execution environments:

```text
                         Runtime Manifest
                                │
                                ▼
                           +---------+
                           | FSM_COS |
                           +---------+
                                │
                         RuntimeAssembly
                                │
          +---------------------+---------------------+
          │           │           │          │        │
          ▼           ▼           ▼          ▼        ▼
       AnyApp      WebPage      WebApp      MyVR   DistributedApp
       desktop      browser      service    VR      shared compute
```

These are **siblings, not layers**. FSM_COS composes the functionality; the execution environment determines where and how that composition is encountered or computed.

That scale is intentional. A capability composed for AnyApp should not become a different capability merely because it is later encountered through WebPage, MyVR, or a distributed execution topology.

### DistributedApp: computational sharing, not another kernel

[DistributedApp](docs/COMPUTE_SCALE.md) is a proposed sibling execution model for sharing computation across participating environments. It does **not** sit above FSM_COS and it does not replace the MicroBundle contract.

This distinction matters for relationships such as MyVR using computation hosted by an AnyApp process. We do **not** want:

```text
MyVR → AnyApp
```

as a hard architectural dependency.

We want the participating environments to meet through a distributed computation boundary:

```text
              MyVR
                │
                │ participates
                ▼
         DistributedApp
                │
       computational sharing
                │
                ▼
             AnyApp
```

In other words: **MyVR does not need to use AnyApp as an application dependency. MyVR can participate in a distributed computation in which an AnyApp host is one available compute participant.**

This preserves replaceability. The other participant could eventually be another desktop, a server, a WebApp, a cloud process, a specialized machine, or an execution environment we have not invented yet.

The composition question and the placement question remain separate:

> **FSM_COS answers what must exist together. DistributedApp can answer where computation happens.**

See [Compute Scale and Distributed Execution](docs/COMPUTE_SCALE.md).

---

## Quick start

A composition host supplies an `IMicroBundleCatalog` that can resolve the domain-owned `IMicroBundle` instances.

Conceptually:

```csharp
var manifest = new RuntimeManifest(
    RuntimeId: 1001,
    Bundles:
    [
        /* root MicroBundle dependency requests */
    ]);

var assembly = fsmCos.Execute(manifest, catalog);
```

The catalog is intentionally an input boundary. It can be backed by an in-memory registry, generated registry, cache, Warehouse adapter, or another discovery system without changing the composition algorithm.

For the complete runtime flow, see [Architecture](docs/ARCHITECTURE.md).

---

## Documentation map

This repository documents **FSM_COS itself**. Neighboring packages document their own domains.

### Start here

- [Architecture](docs/ARCHITECTURE.md) — how the kernel works.
- [Dependency & Boundary Guide](docs/DEPENDENCIES.md) — why each package is inside or outside the kernel.
- [Runtime Manifest](docs/RUNTIME_MANIFEST.md) — the request FSM_COS consumes.
- [MicroBundles](docs/MICROBUNDLES.md) — how FSM_COS uses the domain-owned capability contract.
- [Arbitration and Convergence](docs/ARBITRATION.md) — the stability model.
- [RuntimeAssembly](docs/RUNTIME_ASSEMBLY.md) — the handoff contract.
- [Runtime Boundary](docs/RUNTIME_BOUNDARY.md) — what stops at the kernel boundary.
- [Theory](docs/THEORY.md) — why the composition boundary exists.
- [Manifest Theory](docs/MANIFEST_THEORY.md) — why publication is separate from authoring.
- [Development](docs/DEVELOPMENT.md) — repository and verification discipline.
- [WebPage Integration](docs/WEBPAGE_INTEGRATION.md) — one concrete host integration.

### Neighboring package documentation

- [FSM_API](https://github.com/TrentBest/FSM_API) — state-machine behavior.
- [MicroBundleDomain](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleDomain) — the canonical MicroBundle contract.
- [FSM_Serialization](https://github.com/TrentBest/TheSingularityWorkshop.FSM_Serialization) — representation and the byte boundary.

**Read a dependency's documentation for its domain. Read this repository for the way FSM_COS uses that dependency.**

---

## Current package

**Package:** `TheSingularityWorkshop.FSM_COS`  
**Version:** `0.1.0-alpha.5`  
**Target:** .NET 8  
**License:** MIT

This alpha deliberately remains a composition kernel rather than an application framework.

The package is published through the repository's explicit trusted-publishing workflow. Publication is intentionally separate from verification.

---

## Repository structure

```text
TheSingularityWorkshop.FSM_COS/
├── README.md
├── LICENSE.txt
├── docs/
│   ├── ARCHITECTURE.md
│   ├── DEPENDENCIES.md
│   ├── RUNTIME_MANIFEST.md
│   ├── MICROBUNDLES.md
│   ├── ARBITRATION.md
│   ├── RUNTIME_ASSEMBLY.md
│   ├── RUNTIME_BOUNDARY.md
│   ├── THEORY.md
│   ├── MANIFEST_THEORY.md
│   ├── DEVELOPMENT.md
│   ├── COMPUTE_SCALE.md
│   └── WEBPAGE_INTEGRATION.md
├── src/
│   └── FSM_COS/
└── tests/
    └── FSM_COS.Tests/
```

---

## The invariant

```text
MicroBundleDomain → capability contract
FSM_API            → state/context primitive
FSM_COS            → composition
Repository/Catalog  → discovery/materialization
FSM_Serialization  → representation
Host                → execution / manifestation
Experience          → what is encountered
```

> **Assemble what was requested. Return a stable composition. Hand it to the host.**

---

## 🔗 The Singularity Workshop

FSM_COS is one layer in a deliberately modular ecosystem:

- **[FSM_API](https://github.com/TrentBest/FSM_API)** — behavior and state.
- **[MicroBundleDomain](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleDomain)** — capability contract.
- **[FSM_COS](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS)** — composition and runtime assembly.
- **[FSM_Serialization](https://github.com/TrentBest/TheSingularityWorkshop.FSM_Serialization)** — representation and the byte boundary.
- **[WebPage](https://github.com/TrentBest/WebPage)** — browser manifestation and proving ground.
- **[AnyApp](https://github.com/TrentBest/AnyApp)** — host/application proving ground.

<p align="center">
  <a href="https://github.com/TrentBest/FSM_API">
    <img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" height="180">
  </a>
</p>

<p align="center">
  <em>The Singularity Workshop — Tools for the curious, the bold, and the systemically inclined.</em><br>
  <strong>Because state shouldn't be a mess.</strong>
</p>
