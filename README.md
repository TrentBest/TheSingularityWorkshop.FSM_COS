![The Singularity Workshop animated moniker](https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Renderer/development/docs/assets/moniker.svg)

# FSM_COS — The Compute Operating System for Composable Runtime Systems

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![NuGet version](https://img.shields.io/nuget/v/TheSingularityWorkshop.FSM_COS?style=flat-square&logo=nuget&logoColor=white)](https://www.nuget.org/packages/TheSingularityWorkshop.FSM_COS)
[![NuGet downloads](https://img.shields.io/nuget/dt/TheSingularityWorkshop.FSM_COS?logo=nuget&style=flat-square)](https://www.nuget.org/packages/TheSingularityWorkshop.FSM_COS)
[![Build Status](https://img.shields.io/github/actions/workflow/status/TrentBest/TheSingularityWorkshop.FSM_COS/package.yml?branch=master&style=flat-square&logo=github)](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/actions/workflows/package.yml)
[![Code Coverage](https://codecov.io/gh/TrentBest/TheSingularityWorkshop.FSM_COS/graph/badge.svg)](https://codecov.io/gh/TrentBest/TheSingularityWorkshop.FSM_COS)

<p align="center">
  <img src="docs/assets/fsm-cos-overview.svg" alt="A runtime manifest requests capabilities; FSM_COS resolves dependencies and assembles MicroBundles into a RuntimeAssembly for a host." width="1000">
</p>

**When every host must hard-code how capabilities fit together, reuse becomes another source of complexity. FSM_COS gives the host one composition boundary for resolving dependencies and assembling the requested runtime.**

FSM_COS **directly depends on [FSM_API](https://github.com/TrentBest/FSM_API)** for finite-state-machine and state/context primitives used by the Workshop runtime model. The COS layer has a different job: **composition**. FSM_COS does not schedule every process, run the application loop, render the GUI, or become the finished application.

**[Run the minimal consumer](#-04-see-it-work)** · **[Why use FSM_COS?](#-01-why-use-a-compute-operating-system)** · **[Browse the living Gallery](docs/GALLERY.md)** · **[Read the consumer guide](docs/CONSUMING_MICROBUNDLES.md)**

## 🟣 01 — Why use a Compute Operating System?

That cost grows as applications add combinations, versions, and prerequisites. Each host otherwise has to duplicate dependency knowledge or invent its own rules for ordering and validation.

**FSM_COS moves that composition problem into a reusable runtime boundary.** A host declares the capabilities it wants; FSM_COS resolves the required MicroBundles, checks the composition, loads prerequisites before dependents, and returns a `RuntimeAssembly` for the host to use.

<p align="center">
  <img src="docs/assets/dependency-resolution.svg" alt="A requested MicroBundle expands into its dependencies, which are resolved before the requested capability is assembled." width="100%">
</p>

That separation gives an application a few important advantages:

- **Compose from intent.** Describe the requested capabilities and versions instead of embedding every combination in host-specific code.
- **Resolve dependencies once.** Share one dependency-resolution and load-order contract across websites, desktop applications, services, simulations, and headless hosts.
- **Fail before handoff.** Missing dependencies, invalid relationships, cycles, or failed bounded arbitration prevent a successful assembly.
- **Keep execution where it belongs.** FSM_COS assembles the runtime; the host owns application behavior, scheduling, user interface, lifecycle, and presentation.
- **Choose the boundary deliberately.** FSM_COS is useful when runtime composition is a real problem—not an extra requirement for every small application.

The key distinction is simple: **the manifest says what is wanted, FSM_COS assembles what must exist together, and the host decides what happens next.**

**Explore the Workshop:** [Find The Singularity Workshop packages on NuGet](https://www.nuget.org/packages?q=TheSingularityWorkshop) · [Browse the Workshop repository guide](docs/WORKSHOP_REPOSITORIES.md)

## 🟦 02 — What does FSM_COS do?

A runtime manifest says which capabilities and versions a host wants. A host-supplied catalog makes those MicroBundles available for resolution. FSM_COS works out the required dependency closure, validates the composition, loads prerequisites before dependents, and runs bounded arbitration. If composition succeeds, the host receives a `RuntimeAssembly`.

```text
Host / authoring system
        │
        │ RuntimeManifest + catalog (+ optional configuration)
        ▼
     FSM_COS
        ├─ resolve requested MicroBundles
        ├─ discover and validate dependencies
        ├─ load prerequisites before dependents
        ├─ arbitrate within a configured bound
        ▼
  RuntimeAssembly
        │
        ▼
Host executes, presents, or integrates the result
```

A MicroBundle is a domain-defined unit of capability or behavior. The MicroBundle contract belongs to [MicroBundleDomain](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleDomain); FSM_COS consumes that contract and defines how the requested pieces are composed. A `RuntimeAssembly` is the result of composition—not a promise that every bundle is compatible, an application is running, or a user interface has appeared.

## 🩵 03 — Where FSM_COS fits

| Component | Responsibility |
|---|---|
| [FSM_API](https://github.com/TrentBest/FSM_API) | Finite-state-machine and state/context primitives used by the Workshop ecosystem. |
| [MicroBundleDomain](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleDomain) | Defines the MicroBundle domain contract consumed by FSM_COS. |
| **FSM_COS** | Resolves, validates, loads, and arbitrates a requested MicroBundle composition; returns `RuntimeAssembly`. |
| Host / application | Supplies the catalog and optional configuration source; owns execution, lifecycle, UI, and presentation. |
| [FSM_Serialization](https://github.com/TrentBest/TheSingularityWorkshop.FSM_Serialization) | Handles representation/serialization concerns; it is not the composition kernel. |

FSM_COS does not download artifacts, verify artifact content hashes, activate deferred bundles in a running assembly, schedule the application, or render an experience. Those responsibilities belong to the artifact provider and host. See [Architecture](docs/ARCHITECTURE.md), [Runtime Boundary](docs/RUNTIME_BOUNDARY.md), and [Staged Manifest Loading](docs/STAGED_MANIFEST_LOADING.md) for the exact implementation boundaries.

## 🟢 04 — See it work

You need Git and the **.NET 8 SDK**. Run the repository's minimal consumer:

```bash
git clone https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS.git
cd TheSingularityWorkshop.FSM_COS
dotnet run --project samples/FSM_COS.MinimalConsumer/FSM_COS.MinimalConsumer.csproj
```

The sample requests a report MicroBundle that depends on a data-source MicroBundle. Its in-memory catalog supplies both. FSM_COS discovers the prerequisite and composes it first.

Expected final line:

```text
Assembly order: 2 -> 1
```

That output is a small, concrete proof of the central behavior: the manifest requests the report, but the assembled runtime includes its prerequisite first. The sample defines its own catalog and example MicroBundles; they are not built-in types supplied by FSM_COS. For a line-by-line explanation, see the [minimal consumer recipe](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/samples/FSM_COS.MinimalConsumer/Program.md).

The project currently targets **.NET 8** and declares version **0.1.0-alpha.7**. A project version is not proof that the same version is available on the public feed; the [NuGet package page](https://www.nuget.org/packages/TheSingularityWorkshop.FSM_COS) is authoritative for published versions. To understand the package contract, continue with [Consuming MicroBundles](docs/CONSUMING_MICROBUNDLES.md).

## 🟪 05 — Choose your next step

- **New here?** [What Is FSM_COS?](docs/WHAT_IS_FSM_COS.md) — the problem, model, and terminology in plain language.
- **Build a consumer:** [Consuming MicroBundles](docs/CONSUMING_MICROBUNDLES.md) — provide a catalog, manifest, configuration source, and MicroBundles.
- **Understand the implementation:** [Architecture](docs/ARCHITECTURE.md), [Runtime Manifest](docs/RUNTIME_MANIFEST.md), [RuntimeAssembly](docs/RUNTIME_ASSEMBLY.md), and [Arbitration](docs/ARBITRATION.md).
- **Understand current limits:** [Runtime Boundary](docs/RUNTIME_BOUNDARY.md) and [Staged Manifest Loading](docs/STAGED_MANIFEST_LOADING.md).
- **See the system visually:** [Gallery](docs/GALLERY.md) — source-backed diagrams with links to the underlying contracts.
- **Find any guide:** [Documentation Index](DOCUMENTATION_INDEX.md).
- **Choose another Workshop project:** [Repository guide](docs/WORKSHOP_REPOSITORIES.md).
- **Review security assumptions:** [Security](SECURITY.md).

---

## 🔗 Resources & Support

### Project & code
- **FSM_COS:** [Source code](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS) · [NuGet](https://www.nuget.org/packages/TheSingularityWorkshop.FSM_COS)
- **FSM_API:** [Source code](https://github.com/TrentBest/FSM_API)
- **MicroBundleDomain:** [Source code](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleDomain)
- **FSM_Serialization:** [Source code](https://github.com/TrentBest/TheSingularityWorkshop.FSM_Serialization)
- **WebPage:** [Source code](https://github.com/TrentBest/WebPage)
- **The Singularity Workshop:** [Repository guide](docs/WORKSHOP_REPOSITORIES.md) · [GitHub profile](https://github.com/TrentBest) · [Coder Legion profile](https://coderlegion.com/user/The%2BSingularity%2BWorkshop)

### Support The Singularity Workshop
- [Patreon](https://www.patreon.com/c/TheSingularityWorkshop)
- [PayPal](https://www.paypal.com/donate/?hosted_button_id=3Z7263LCQMV9J)
- [Coder Legion community](https://coderlegion.com/)

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
