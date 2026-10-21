# Preserved additional alpha.7 integration documentation (batch 1)

Verbatim snapshots from `work/alpha7-living-docs-integration`, reviewed 2026-10-21. Historical status statements may be stale. These are recovery copies, not merged/certified content.


---

## AGENTS.md
Source blob: `bd033f772a44422ae7ae5b63e066f0cbfa8cf3fb`

![The Singularity Workshop animated moniker](https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Renderer/development/docs/assets/moniker.svg)

# Repository Instructions for AI Agents

## Branch policy — mandatory

This repository has exactly two permanent branches:

- `master` — stable, reviewed, release-ready history.
- `development` — the only active integration and verification branch.

Do not create feature, documentation, experiment, handoff, or release branches. Work directly on `development`. Do not merge to `master`, publish a package, or deploy anything without the owner's explicit approval for that action.

## Keep the repository understandable

- Prefer the simplest implementation that accurately expresses the current design.
- Work from the current source, tests, project metadata, and documentation. Do not substitute generic templates or invent a parallel implementation.
- Do not preserve speculative or obsolete work merely because it exists in an old commit, branch, checklist, or handoff document. If reconciliation is unclear or expensive, discard the stale approach and rebuild from the current understanding.
- Keep each repository responsible for its own domain. Explain neighboring dependencies briefly and link to their authoritative documentation instead of copying their manuals.
- Distinguish implemented behavior, intended architecture, and future work. Do not make claims that source, tests, CI, or published metadata cannot support.
- Keep publication disabled by default. The owner must explicitly authorize the specific package release; a green build is not permission to publish.

## Before changing code or documentation

1. Read this repository's `README.md`, `DOCUMENTATION_STANDARD.md`, and `DOCUMENTATION_INDEX.md`.
2. Inspect the relevant implementation, tests, package metadata, and workflows on the current branch.
3. Check the current CI result and verify any claimed cross-repository dependency against that repository's actual source or published package.
4. Make the smallest coherent change that improves correctness or clarity.
5. Verify links and package contents when changing documentation or packaging.
6. Report the exact commit(s), checks performed, remaining blockers, and whether anything was published or deployed.

Do not treat old pause-point, branch-reconciliation, continuation, or alignment notes as mandatory instructions. Check them only when the current task genuinely needs their historical context.

---

**The Singularity Workshop**  
[GitHub](https://github.com/TrentBest) · [Coder Legion](https://coderlegion.com/) · [Patreon](https://www.patreon.com/c/TheSingularityWorkshop) · [PayPal](https://www.paypal.com/donate/?hosted_button_id=3Z7263LCQMV9J)  
[ FSM_API ](https://github.com/TrentBest/FSM_API) · [ FSM_Serialization ](https://github.com/TrentBest/TheSingularityWorkshop.FSM_Serialization) · [ WebPage ](https://github.com/TrentBest/WebPage) · [ FSM_COS Documentation Standard ](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/DOCUMENTATION_STANDARD.md)  
**Because state shouldn't be a mess.**  
*And because static boundaries are invitations to cause trouble.*



---

## DOCUMENTATION_INDEX.md
Source blob: `e98408b5d876cdb50aa806aa33ca5490f6e10772`

![The Singularity Workshop animated moniker](https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Renderer/development/docs/assets/moniker.svg)

# FSM_COS Documentation Index

This index is the navigation map. It does not duplicate the contracts below.

## Start here

| Reader goal | Read |
|---|---|
| Understand the package quickly | [README](README.md) |
| Learn the idea without assuming programming experience | [What Is FSM_COS?](docs/WHAT_IS_FSM_COS.md) |
| Run a concrete consumer | [Minimal consumer sample](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/samples/FSM_COS.MinimalConsumer/Program.cs) |
| Understand ownership and runtime flow | [Architecture](docs/ARCHITECTURE.md) |
| Understand the rationale and design invariants | [Theory](docs/THEORY.md) |
| Integrate the package | [Consuming MicroBundles](docs/CONSUMING_MICROBUNDLES.md) |
| Review the candidate's scope and limitations | [Alpha.7 release notes](docs/releases/0.1.0-alpha.7.md) |
| Review implemented domain capabilities, explicit deferrals, and follow-on engineering | [Domain Completion and Release Roadmap](docs/DOMAIN_COMPLETION_ROADMAP.md) |
| Verify release readiness | [Alpha.7 readiness gate](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/docs/0.1.0-alpha.7-READINESS.md) |
| Review trust assumptions and adversarial security coverage | [Security](SECURITY.md) |
| Build, test, and understand CI modes | [Development](docs/DEVELOPMENT.md) |

## Contract authority map

Use one primary document for each question. Related pages should explain context and link here rather than restating the same contract.

| Concern | Primary authority | Scope |
|---|---|---|
| What FSM_COS owns | [Domain](docs/DOMAIN.md) | Domain boundary, not API reference |
| How the implementation works | [Architecture](docs/ARCHITECTURE.md) | Current code flow and failure behavior |
| Follow the exact Execute success/failure path | [Execute Sequence](docs/EXECUTION_SEQUENCE.md) | Source-derived sequence for validation, dependency resolution, loading, arbitration, and host handoff |
| Why the composition boundary exists | [Theory](docs/THEORY.md) | Rationale and invariants |
| Request shape and root version selection | [Runtime Manifest](docs/RUNTIME_MANIFEST.md) | Public manifest contract |
| Staged metadata and its limits | [Staged Manifest Loading](docs/STAGED_MANIFEST_LOADING.md) | Planning/state metadata versus end-to-end localization |
| Host handoff | [RuntimeAssembly](docs/RUNTIME_ASSEMBLY.md) | Output contract and explicit non-promises |
| Runtime ownership limits | [Runtime Boundary](docs/RUNTIME_BOUNDARY.md) | What belongs to hosts and adjacent packages |
| Resolution, loading, and consuming bundles | [Consuming MicroBundles](docs/CONSUMING_MICROBUNDLES.md) | Consumer-facing use and errors |
| Arbitration and convergence | [Arbitration](docs/ARBITRATION.md) | Negotiation semantics and limits |
| AI-related composition concepts | [AI Composition](docs/AI_COMPOSITION.md) | Architectural exploration; not proof of provider integration |
| WebPage host integration | [WebPage Integration](docs/WEBPAGE_INTEGRATION.md) | Host-specific integration boundary |
| Release-specific behavior | [Alpha.7 release notes](docs/releases/0.1.0-alpha.7.md) | Version scope and deferrals |
| Release gate | [Alpha.7 readiness](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/docs/0.1.0-alpha.7-READINESS.md) | Evidence and release criteria |
| Security boundaries and adversarial test plan | [Security](SECURITY.md) | Threat model, mitigations, external baselines, and residual risks |

## Documentation layers

- **Public orientation:** `README.md` and `NuGet.README.md`. The root README is the repository entry point; the NuGet README is the package-consumer entry point.
- **Current contract and explanation:** the primary authority documents listed above.
- **Repository process:** [Development](docs/DEVELOPMENT.md), [Documentation Standard](DOCUMENTATION_STANDARD.md), and [AI agent instructions](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/AGENTS.md).
- **Working and historical records:** branch reconciliation, dependency alignment, ecosystem integration, continuation, and pause-point documents are planning records. They are not API specifications or release evidence; verify any current-state statement against source, tests, and current CI before acting on it. These records are intentionally excluded from the consumer package.

## Accuracy and release evidence

The shared [Documentation Standard](DOCUMENTATION_STANDARD.md) governs writing quality, source accuracy, and link hygiene. The [alpha.7 readiness gate](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/docs/0.1.0-alpha.7-READINESS.md) governs exact-candidate release evidence. Historical planning notes are context, not current specifications; verify present-tense claims against source, tests, and current CI.

## Living documentation

The diagrams are explanatory assets, not independent specifications. Each diagram must agree with its adjacent contract and current implementation. The [maintainer-only Living Documentation Visual Audit](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/docs/LIVING_DOCUMENTATION_AUDIT.md) tracks visual coverage and accessibility-source checks; [Execute Sequence](docs/EXECUTION_SEQUENCE.md) traces the current composition contract; [Living SVG](docs/LIVING_SVG.md) explains the GUI.SVG-generated preflight picture, reduced-motion behavior, and the boundary between explanatory motion and a future host-bound live view. Neither document claims browser or screen-reader verification unless that evidence is explicitly recorded.

---

<p align="center"><em>The Singularity Workshop — Tools for the curious, the bold, and the systemically inclined.</em><br><strong>Because state shouldn't be a mess.</strong></p>

## Source-type documentation pilots

The source-adjacent documentation pattern is being introduced incrementally. The first pilot is the `RuntimeManifest` type: [class explanation and UML class card](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/src/FSM_COS/RuntimeManifest.md), plus [relationship diagram and explicitly authored use cases](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/src/FSM_COS/RuntimeManifestUML.md). These pages are a human-maintained interim example; they are not yet proof that every source type has generated documentation or that the GUI UML generator is integrated.

---

**The Singularity Workshop**  
[GitHub](https://github.com/TrentBest) · [Coder Legion](https://coderlegion.com/) · [Patreon](https://www.patreon.com/c/TheSingularityWorkshop) · [PayPal](https://www.paypal.com/donate/?hosted_button_id=3Z7263LCQMV9J)  
[ FSM_API ](https://github.com/TrentBest/FSM_API) · [ FSM_Serialization ](https://github.com/TrentBest/TheSingularityWorkshop.FSM_Serialization) · [ WebPage ](https://github.com/TrentBest/WebPage) · [ FSM_COS Documentation Standard ](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/DOCUMENTATION_STANDARD.md)  
**Because state shouldn't be a mess.**  
*And because static boundaries are invitations to cause trouble.*



---

## NuGet.README.md
Source blob: `373c981656385f916c789b3f9c1d175b8351b28f`

![The Singularity Workshop animated moniker](https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Renderer/development/docs/assets/moniker.svg)

# ✳️ 00 The Singularity Workshop — FSM_COS

![FSM_COS composition flow: a runtime manifest enters FSM_COS, which resolves and composes MicroBundles into a RuntimeAssembly for a host.](https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.FSM_COS/development/docs/assets/fsm-cos-overview.svg)

**FSM_COS composes requested capabilities into a `RuntimeAssembly`. It is a composition boundary—not an application, GUI, engine, repository, or execution loop.**

*If a picture is worth a thousand words, then what is a living picture worth?* FSM_COS uses source-backed diagrams to explain dependency closure, preflight validation, failure boundaries, and the host handoff. They illustrate the real contract; they are not presented as live execution traces.

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

![FSM_COS validation and composition path, generated with GUI.SVG](https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.FSM_COS/fbe34dfa2f67019bd54fc9c47549425be6bc1d24/docs/assets/fsm-cos-preflight.svg)

*This is a source-backed contract map, not a live execution trace.*

- **MicroBundleDomain** owns the MicroBundle contract.
- **The host or repository** supplies the catalog used to resolve bundles.
- **FSM_COS** composes the requested capabilities.
- **The host** owns execution, lifecycle, UI, and presentation.

Alpha.7 exposes read-only snapshots for assembly and plan collections: callers cannot mutate the collection structure through those public views. This does **not** make the `MicroBundle` objects inside those collections immutable; their behavior remains owned by their domain contracts.

## 🟢 04 — See it in a minute

To install FSM_COS, choose an available version from the [NuGet package page](https://www.nuget.org/packages/TheSingularityWorkshop.FSM_COS). If you intend to use a prerelease, request prerelease versions explicitly:

```bash
dotnet add package TheSingularityWorkshop.FSM_COS --prerelease
```

Installation adds a composition library; it does not create an application. FSM_COS needs a catalog supplied by the host. To see the composition behavior running, clone the source repository and run its minimal consumer sample:

```bash
git clone https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS.git
cd TheSingularityWorkshop.FSM_COS
dotnet run --project samples/FSM_COS.MinimalConsumer/FSM_COS.MinimalConsumer.csproj
```

The sample creates a report bundle that depends on a data-source bundle. The expected result is `Assembly order: 2 -> 1`, proving the dependency is loaded before the requested bundle. It defines its own catalog and example bundles; these are not built-in FSM_COS types. The sample references the local source project so it can be verified before a candidate version is published.

For a complete consumer guide, see [Consuming MicroBundles](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/docs/CONSUMING_MICROBUNDLES.md). The package targets **.NET 8** and its direct dependencies are:

- [FSM_API](https://www.nuget.org/packages/TheSingularityWorkshop.FSM_API) — state/context primitives.
- [MicroBundleDomain](https://www.nuget.org/packages/TheSingularityWorkshop.MicroBundleDomain) — the MicroBundle contract.
- [FSM_UserIO](https://www.nuget.org/packages/TheSingularityWorkshop.FSM_UserIO) — the platform-neutral `SemanticIntent` boundary.

This source currently declares version `0.1.0-alpha.7`. A source version declaration alone does not establish publication status; check the [NuGet package page](https://www.nuget.org/packages/TheSingularityWorkshop.FSM_COS) for the version actually available.

## 🟪 05 — Available documentation and theory

- [Project overview](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/README.md) — the bigger picture and quick proof.
- [What Is FSM_COS?](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/docs/WHAT_IS_FSM_COS.md) — a plain-language introduction for readers new to software composition.
- [Theory](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/docs/THEORY.md) — why composition is a distinct operation.
- [Architecture](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/docs/ARCHITECTURE.md) — ownership and runtime flow.
- [Domain completion roadmap](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/docs/DOMAIN_COMPLETION_ROADMAP.md) — implemented capabilities, deferred work, dependency relationships, and follow-on engineering.
- [Runtime Manifest](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/docs/RUNTIME_MANIFEST.md) — the composition request.
- [RuntimeAssembly](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/docs/RUNTIME_ASSEMBLY.md) — the host handoff.
- [Arbitration](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/docs/ARBITRATION.md) — reconciliation and convergence.
- [Documentation Index](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/DOCUMENTATION_INDEX.md) — all focused guides.
- [Alpha.7 release notes](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/docs/releases/0.1.0-alpha.7.md) — included contracts and explicit limitations.
- [Development](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/docs/DEVELOPMENT.md) — build and verification.
- [Source and issues](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS)

---
<p align="center">
  <a href="https://github.com/TrentBest/WebPage" aria-label="Visit The Singularity Workshop">
    <img src="https://raw.githubusercontent.com/TrentBest/WebPage/development/TheSingularityWorkshop/wwwroot/Images/TheSingularityWorkshopLivingAvatar.svg" alt="The Singularity Workshop living avatar — a digital vortex of information converging on its central eye" width="112" />
  </a><br />
  <strong>The Singularity Workshop</strong><br />
  <em>If a picture is worth a thousand words, what is a living picture worth?</em><br />
  <a href="https://github.com/TrentBest">GitHub</a> ·
  <a href="https://coderlegion.com/">Coder Legion</a> ·
  <a href="https://www.patreon.com/c/TheSingularityWorkshop">Patreon</a> ·
  <a href="https://www.paypal.com/donate/?hosted_button_id=3Z7263LCQMV9J">PayPal</a><br />
  <a href="https://github.com/TrentBest/FSM_API">FSM_API</a> ·
  <a href="https://github.com/TrentBest/TheSingularityWorkshop.FSM_Serialization">FSM_Serialization</a> ·
  <a href="https://github.com/TrentBest/WebPage">WebPage</a><br />
  <strong>Because state shouldn't be a mess.</strong><br />
  <em>And because static boundaries are invitations to cause trouble.</em>
</p>

---

**The Singularity Workshop**  
[GitHub](https://github.com/TrentBest) · [Coder Legion](https://coderlegion.com/) · [Patreon](https://www.patreon.com/c/TheSingularityWorkshop) · [PayPal](https://www.paypal.com/donate/?hosted_button_id=3Z7263LCQMV9J)  
[ FSM_API ](https://github.com/TrentBest/FSM_API) · [ FSM_Serialization ](https://github.com/TrentBest/TheSingularityWorkshop.FSM_Serialization) · [ WebPage ](https://github.com/TrentBest/WebPage) · [ FSM_COS Documentation Standard ](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/DOCUMENTATION_STANDARD.md)  
**Because state shouldn't be a mess.**  
*And because static boundaries are invitations to cause trouble.*



---

## README.md
Source blob: `f001bc36be0ba741f22a339ab1da9e8a36f5312d`

![The Singularity Workshop animated moniker](https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Renderer/development/docs/assets/moniker.svg)

# ✳️ 00 The Singularity Workshop — FSM_COS

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![NuGet version](https://img.shields.io/nuget/v/TheSingularityWorkshop.FSM_COS?style=flat-square&logo=nuget&logoColor=white)](https://www.nuget.org/packages/TheSingularityWorkshop.FSM_COS)
[![NuGet downloads](https://img.shields.io/nuget/dt/TheSingularityWorkshop.FSM_COS?logo=nuget&style=flat-square)](https://www.nuget.org/packages/TheSingularityWorkshop.FSM_COS)
[![Build Status](https://img.shields.io/github/actions/workflow/status/TrentBest/TheSingularityWorkshop.FSM_COS/package.yml?branch=master&style=flat-square&logo=github)](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/actions/workflows/package.yml)
[![Code Coverage](https://codecov.io/gh/TrentBest/TheSingularityWorkshop.FSM_COS/graph/badge.svg)](https://codecov.io/gh/TrentBest/TheSingularityWorkshop.FSM_COS)

<p align="center">
  <img src="docs/assets/fsm-cos-overview.svg" alt="A runtime manifest enters FSM_COS; the kernel resolves dependencies and composes MicroBundles into a RuntimeAssembly for a host." width="1000">
</p>

*If a picture is worth a thousand words, then what is a living picture worth?* FSM_COS uses source-backed diagrams to explain dependency closure, preflight validation, failure boundaries, and the host handoff. They illustrate the real contract; they are not presented as live execution traces.

**[Gallery](docs/GALLERY.md)** — a visual index of composition, validation, arbitration, and host handoff.

FSM_COS is The Singularity Workshop's **runtime composition kernel**. It takes a request for capabilities, resolves the MicroBundles and dependencies needed to satisfy it, and hands the assembled result to a host.

## 🟦 01 — What is FSM_COS?

Think of a runtime manifest as an order: it says which capabilities are wanted. FSM_COS works out what else must be present for that order to make sense, loads the composition, and gives its bundles a bounded chance to reconcile their composition state. It hands the result to the host only if that arbitration process reports convergence.

The result is a `RuntimeAssembly`—a handoff to another system, not a finished application.

**In one line:** the manifest says what is requested; FSM_COS assembles what must exist together; the host decides what happens next.

## 🟣 02 — Why does it exist?

Without a distinct composition layer, each host can end up owning its own dependency logic and capability wiring. That makes reusable behavior harder to share and encourages application-specific concerns to leak into lower-level libraries.

FSM_COS gives that work a focused home. A browser app, desktop program, service, simulation, or headless tool can supply its own catalog and decide how to use the resulting assembly. The kernel does not require a particular interface or application type.

This is an architectural boundary, not a promise that every capability is automatically portable. Compatibility still depends on the contracts, dependencies, and abilities of the host.

## 🩵 03 — How does it work?

```text
Runtime Manifest
      │ requested MicroBundles and versions
      ▼
   FSM_COS
      ├── resolve roots and exact versions
      ├── preflight the full dependency closure
      ├── validate identities, requests, and cycles
      ├── load dependencies first (iterative DFS)
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

FSM_COS is not a GUI, engine, artifact repository, transport layer, or application loop. For the exact contracts and responsibility boundaries, see [Architecture](docs/ARCHITECTURE.md) and [Runtime Boundary](docs/RUNTIME_BOUNDARY.md). Security scope, explicit trust assumptions, and the adversarial verification matrix are documented in [Security](SECURITY.md).

Alpha.7 exposes read-only snapshots for assembly and plan collections: callers cannot mutate the collection structure through those public views. This does **not** make the `MicroBundle` objects inside those collections immutable; their behavior remains owned by their domain contracts.

## 🟢 04 — See it in a minute

The repository includes a tiny runnable consumer that demonstrates the package's actual purpose: assembling a requested bundle and its dependency. You need Git and the **.NET 8 SDK**.

```bash
git clone https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS.git
cd TheSingularityWorkshop.FSM_COS
dotnet run --project samples/FSM_COS.MinimalConsumer/FSM_COS.MinimalConsumer.csproj
```

**What should happen?** The sample loads bundle `2` (the data-source prerequisite) before bundle `1` (the report), then prints `Assembly order: 2 -> 1`. The sample defines its own in-memory catalog and two tiny MicroBundles; those are example code, not extra types supplied by FSM_COS.

This is a source-repository example and references the local FSM_COS project so it can be verified before a release is published. For the exact published package status, check the [NuGet package page](https://www.nuget.org/packages/TheSingularityWorkshop.FSM_COS). To build your own host, start with [Consuming MicroBundles](docs/CONSUMING_MICROBUNDLES.md), which explains the catalog, manifest, configuration, and bundle contracts.

The source targets **.NET 8** and currently declares version `0.1.0-alpha.7`. This is a source candidate, not proof of publication; verify availability on the [NuGet package page](https://www.nuget.org/packages/TheSingularityWorkshop.FSM_COS).
  
## 🟪 05 — Available documentation and theory

Pick the question you want answered; each document focuses on one topic.

- **New to software or this architecture?** [What Is FSM_COS?](docs/WHAT_IS_FSM_COS.md) introduces the problem and key terms without assuming programming experience. [FSM_COS Theory](docs/THEORY.md) develops the deeper rationale.
- **Need the system map?** [Architecture](docs/ARCHITECTURE.md) explains component ownership and runtime flow.
- **Building a consumer?** [Consuming MicroBundles](docs/CONSUMING_MICROBUNDLES.md) shows how to provide a catalog and bundles.
- **Defining a request?** [Runtime Manifest](docs/RUNTIME_MANIFEST.md) explains requested bundle identities and versions; [Manifest Theory](docs/MANIFEST_THEORY.md) explains the design rationale.
- **Receiving the result?** [RuntimeAssembly](docs/RUNTIME_ASSEMBLY.md) describes the handoff object.
- **Need the exact stopping point?** [Runtime Boundary](docs/RUNTIME_BOUNDARY.md) explains what FSM_COS owns and what remains with the host.
- **Wondering how stability is reached?** [Arbitration](docs/ARBITRATION.md) covers reconciliation and convergence.
- **Integrating a host?** [WebPage Integration](docs/WEBPAGE_INTEGRATION.md) documents one browser-host integration without moving browser concerns into the kernel.
- **Checking how Workshop packages fit together?** [Ecosystem Integration Map](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/docs/ECOSYSTEM_INTEGRATION_MAP.md) is a source-repository working snapshot; verify its point-in-time source facts before relying on them.
- **Checking alpha.7 scope and limitations?** [Release notes](docs/releases/0.1.0-alpha.7.md) describe the source candidate's contracts and explicit non-goals.
- **What is in the domain, and what remains?** [Domain Completion and Release Roadmap](docs/DOMAIN_COMPLETION_ROADMAP.md) separates implemented behavior from deferred capabilities and defines evidence required to close them.
- **Preparing a release?** The [alpha.7 Release Readiness checklist](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/docs/0.1.0-alpha.7-READINESS.md) is the sole authority for exact-candidate gates and evidence. The [Domain Completion and Release Roadmap](docs/DOMAIN_COMPLETION_ROADMAP.md) describes implemented behavior, deferred capabilities, and follow-on engineering—not candidate CI or package evidence.
- **Building or contributing?** [Development](docs/DEVELOPMENT.md) covers repository setup, tests, and workflows.
- **Looking for another topic?** Open the [Documentation Index](DOCUMENTATION_INDEX.md).

---

---

**The Singularity Workshop**  
[GitHub](https://github.com/TrentBest) · [Coder Legion](https://coderlegion.com/) · [Patreon](https://www.patreon.com/c/TheSingularityWorkshop) · [PayPal](https://www.paypal.com/donate/?hosted_button_id=3Z7263LCQMV9J)  
[ FSM_API ](https://github.com/TrentBest/FSM_API) · [ FSM_Serialization ](https://github.com/TrentBest/TheSingularityWorkshop.FSM_Serialization) · [ WebPage ](https://github.com/TrentBest/WebPage) · [ FSM_COS Documentation Standard ](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/DOCUMENTATION_STANDARD.md)  
**Because state shouldn't be a mess.**  
*And because static boundaries are invitations to cause trouble.*



---

## SECURITY.md
Source blob: `64a89e89a050865b539f8abeee60ad75da989948`

![The Singularity Workshop animated moniker](https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Renderer/development/docs/assets/moniker.svg)

# Security posture and adversarial test plan

FSM_COS is a composition kernel that invokes host-supplied catalog, configuration-provider, and MicroBundle code. This document separates guarantees enforced by FSM_COS from guarantees that belong to the host and deployment.

## Security boundary

FSM_COS validates identity, dependency declarations, graph consistency, and optional published schedule metadata before invoking any bundle's `Load` method. It snapshots the root/load-plan collections, resolved bundle instances, dependency request structures, and configuration bytes used during loading. Graph traversal is iterative and rejects manifests above the configured bundle and dependency-request limits (defaults: 10,000 bundles, 100,000 dependency requests, and 64 MiB of copied configuration data per composition); arbitration is bounded by the configured round limit.

The `latest-all` .NET analyzer gate treats all unsuppressed runtime-assembly warnings as errors. `CA1707` is intentionally suppressed because the published `FSM_COS` assembly and namespace names are compatibility contracts; do not rename them solely to satisfy a style rule. Other analyzer warnings are fixed or reviewed explicitly, not blanket-suppressed.

**FSM_COS is not a sandbox.** A catalog, configuration provider, `Load`, or `Arbitrate` implementation executes in-process with the process's permissions. Malicious code can access files, network, environment variables, other process memory available to it, or mutate shared state if the host grants those capabilities. Use process/OS isolation, least-privilege identities, containers, or virtualization when executing code that is not trusted. Do not treat a MicroBundle descriptor, hash string, or manifest as proof that executable code is trustworthy.

Composition is not transactional. If a bundle mutates external state and then throws, or an earlier dependency succeeds before a later bundle fails, FSM_COS cannot generically roll back those side effects. Hosts should own lifecycle cleanup, authorization, and audit logging.

## Adversarial verification matrix

| Attack / failure class | FSM_COS control | Evidence expected |
| --- | --- | --- |
| Invalid, zero, conflicting, or mismatched identities/versions | Reject before loading | Unit tests for malformed roots, catalog mismatch, descriptor mismatch, and dependency contract mismatch |
| Missing dependency or catalog exception | Resolve the complete closure before loading | Assert no bundle `Load` callback runs |
| Dependency cycle, self-cycle, duplicate edge/request | Reject during graph/schedule validation | Cycle, duplicate, and malformed-graph tests |
| Catalog time-of-check/time-of-use swap | Retain resolved instances; don't re-resolve during load | Adversarial catalog test with lookup count and object identity assertions |
| Mutating caller-owned root/load-plan/schedule/configuration buffers | Snapshot mutable inputs used by execution | Mutation-after-validation and mutation-during-callback tests |
| Deep graph / recursive stack exhaustion | Iterative graph validation and installation | Deep-chain stress tests |
| Non-convergent or throwing arbitration | Bounded rounds; propagate original exception; no assembly handoff | Non-convergence and callback-failure tests |
| Partial side effects from `Load`/`Arbitrate` failure | Explicitly no rollback guarantee | Failure tests and host cleanup contract |
| Malformed package or accidental GUI dependency | Package metadata/content verifier and runtime assembly boundary test | Verify exact candidate package, dependency list, and assembly references |
| Vulnerable transitive dependencies / unsafe code patterns | Must be checked by dependency audit and static analysis in CI | Record the tool, database date, findings, and disposition; a clean unit-test run is not evidence of a clean security scan |
| Mutable third-party GitHub Action tags | Pin each action to a verified full commit SHA; update pins deliberately and rerun workflows | Workflow source review plus successful exact-candidate CI |
| Untrusted bundle code / privilege abuse | Outside the in-process kernel's security boundary | Host isolation and least privilege; do not claim sandboxing |
| Resource exhaustion from extremely large manifests/graphs/configuration or pathological callbacks | Bounded enumeration for roots, schedules, and load plans; configurable bundle, dependency-request, and configuration-byte limits; iterative traversal. Arbitrary callbacks still need host execution policy | Tests for configured limits and deep graphs; host-level timeout/cancellation remains necessary |

## Release security checklist

Before treating a candidate as security-reviewed:

- [x] Run the exact candidate's full test suite and retain its TRX and coverage artifacts.
- [x] Confirm the current CI dependency audit reports no vulnerable direct or transitive NuGet packages; investigate any advisory rather than silently ignoring it.
- [x] Confirm the `latest-all` .NET analyzer job passes with warnings treated as errors, and review the documented `CA1707` naming suppression.
- [x] Review the CodeQL C# scan for new high/critical findings; the retained SARIF artifact for the candidate contains zero results.
- [x] Verify package identity, version, dependencies, file inventory, and that GUI.SVG is not a runtime dependency.
- [x] Inspect the exact packed assembly and XML API documentation, not just the source tree.
- [x] Pin third-party GitHub Actions to verified full commit SHAs; record the upstream major version beside each pin.
- [x] Confirm the workflow's publish gate is disabled unless explicitly authorized.
- [x] Verify generated SVG assets against the pinned GUI.SVG generator and inspect that they describe current implementation behavior.
- [x] Review the remaining limitations above, especially in-process trust and non-transactional side effects.

The CodeQL analysis workflow completed successfully for the candidate and uploaded SARIF results. The retained SARIF artifact was inspected and contains zero results; a successful analysis run alone would not have established that.

## External baselines

Use these as evolving review checklists, not as a claim that every category applies to this library:

- [OWASP Top 10:2025](https://top10.owasp.org/2025/) — access control, misconfiguration, supply chain, cryptography, injection, design, authentication, integrity, logging, and exceptional conditions.
- [MITRE CWE Top 25:2025](https://cwe.mitre.org/top25/) — recurring dangerous implementation weaknesses including input validation, null handling, resource limits, code injection, and unsafe deserialization.
- [Microsoft .NET security code-analysis rules](https://learn.microsoft.com/en-us/dotnet/fundamentals/code-analysis/quality-rules/security-warnings).
- [Microsoft secure coding guidelines for .NET](https://learn.microsoft.com/en-us/dotnet/standard/security/secure-coding-guidelines).

The relevant attack surface for this package is narrower than a web application, so apply each baseline to the actual code and boundaries instead of adding irrelevant web-specific controls to the kernel.

## Reporting a vulnerability

Please report suspected vulnerabilities privately to the repository maintainer rather than publishing an exploit before a fix is available. Do not include secrets, production data, or credentials in public issues.

---

**The Singularity Workshop**  
[GitHub](https://github.com/TrentBest) · [Coder Legion](https://coderlegion.com/) · [Patreon](https://www.patreon.com/c/TheSingularityWorkshop) · [PayPal](https://www.paypal.com/donate/?hosted_button_id=3Z7263LCQMV9J)  
[ FSM_API ](https://github.com/TrentBest/FSM_API) · [ FSM_Serialization ](https://github.com/TrentBest/TheSingularityWorkshop.FSM_Serialization) · [ WebPage ](https://github.com/TrentBest/WebPage) · [ FSM_COS Documentation Standard ](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/DOCUMENTATION_STANDARD.md)  
**Because state shouldn't be a mess.**  
*And because static boundaries are invitations to cause trouble.*



---

## docs/0.1.0-alpha.7-READINESS.md
Source blob: `6e61aaac09b53795524fe1f02b146c51a560f2f1`

![The Singularity Workshop animated moniker](https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Renderer/development/docs/assets/moniker.svg)

# FSM_COS 0.1.0-alpha.7 — Release Readiness Gate

**Purpose:** this is the single release-gate definition for alpha.7. It is deliberately not a running diary of every earlier CI run. Use GitHub Actions and the exact artifact as evidence; do not copy old results forward.

## Candidate identity

- **Package:** `TheSingularityWorkshop.FSM_COS`
- **Version declared by the project:** `0.1.0-alpha.7`
- **Current review candidate:** the live head of draft PR [#34](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/pull/34) on `work/alpha7-living-docs-integration`. Read the current head SHA from GitHub immediately before verification; this checklist must not treat a prior run as evidence for a later commit. This is not yet the `development` HEAD. After reconciliation, rerun the gate against the exact integrated `development` SHA.
- **Publication:** not authorized by this checklist. A successful build or package artifact does not publish a release.
- **Evidence rule:** every CI result and package inspection is valid only for the exact source commit that produced it. The final candidate is the `development` HEAD for which required CI passed and whose package artifact was inspected. Any subsequent source change invalidates that final-candidate designation until reverified.

## Source-of-truth rule

This file defines the **release evidence gates**, not a second API specification or a running history of prior attempts.

- Current behavior and failure semantics belong to source code, tests, and the primary contract documents linked from [the documentation index](../DOCUMENTATION_INDEX.md).
- Alpha.7's scope, changes, and explicit deferrals belong to the [release notes](releases/0.1.0-alpha.7.md).
- This gate records whether release evidence is sufficient. It must not paraphrase those contracts or copy forward evidence from a different commit.

## Release gates

### A. Source and contract

- [x] Review public API and XML documentation against implementation and tests.
- [x] Verify root-version precedence, dependency closure/order, staged-plan consistency, duplicate/missing IDs, cycles, resolution/version failures, and pre-load rejection.
- [x] Verify load/configuration/arbitration exceptions and non-convergence behavior, including the documented limits on rollback and diagnostics.
- [x] Verify alpha.7's read-only collection-snapshot contract for `RuntimeAssembly.Bundles`, schedule collections, and `RuntimeManifestLoadPlan.Entries`, including mutation-rejection regression tests. The collection structure is protected; the referenced MicroBundle instances are not thereby immutable.
- [x] Confirm direct dependency versions and ownership are intentional (`FSM_API` 1.0.13, `FSM_UserIO` 0.1.0-alpha.1, `MicroBundleDomain` 1.0.1); do not bump a dependency speculatively or before changing its owning contract.
- [x] Review [Security](../SECURITY.md): graph/resource limits, mutable configuration snapshots, trust boundary, dependency audit, and .NET security analyzers.
- [ ] Review CodeQL findings for the exact final `development` candidate SHA. CodeQL passed for the current PR merge candidate and its SARIF artifact contains zero results; rerun and inspect SARIF after integration to `development` before checking this item.
- [x] Confirm deferred capabilities are clearly labeled as deferred in every public-facing explanation and diagram.

### B. Repository and CI

- [x] Confirm `development` is the intended integration/release-preparation source. The current PR candidate is a separate staging branch; capture the exact `development` SHA again after reconciliation.
- [ ] On the exact final `development` candidate SHA, pass `build-and-test`, `public-package-compatibility`, and `source-integration`. The current PR merge candidate passed all three jobs; rerun after integration to `development` because that will be a different exact source SHA.
- [ ] Confirm both package-verification paths pass on the exact final `development` candidate. The corrected Gallery audit link passed both package-verification paths on the current PR merge candidate; repeat against the integrated `development` candidate. Confirm the publish job remains skipped.
- [ ] Review warnings, test output, workflow configuration, and package workflow inputs from the exact final candidate run—not just the overall green badge.
- [ ] Confirm the transitive dependency audit and .NET analyzer gate passed on the exact final candidate; review CodeQL findings separately.

### C. Exact package

- [ ] Download the `.nupkg` generated by a successful run for the exact final candidate SHA.
- [ ] Verify package ID/version, repository commit metadata, target framework, direct dependency versions, license, package README, XML docs, required documentation and visual assets, and exclusions for internal planning files in that exact artifact.
- [ ] Run the repository package verifier and independently inspect the package archive from the exact final candidate run.
- [ ] Record the exact workflow run, artifact name, package SHA-256, and source SHA in the release review outside this mutable checklist. Do not copy hashes from an earlier candidate.
- [ ] Confirm the public NuGet feed still reports the previous version until publication actually succeeds.

### D. Documentation and accessibility

- [ ] Resolve any mismatch between source, tests, the exact built package metadata, root README, NuGet README, release notes, focused contract guides, and the documentation index for the exact final candidate.
- [x] Check links, filenames, examples, API names, dependency versions, implementation status, and package inclusion/exclusion rules against the current tree.
- [ ] Regenerate the SVG from the pinned GUI.SVG source and confirm parity, then pass the package SVG accessibility verifier on the exact final `development` candidate. Both checks passed on the current PR merge candidate; repeat after integration.
- [ ] Verify hosted rendering, browser accessibility-tree exposure, keyboard behavior, and screen-reader behavior separately; these have not been independently verified.
- [x] Ensure the index points readers to one primary authority per concern and that historical planning documents are not presented as current specifications.

### E. Explicitly deferred capabilities and follow-up decisions

The following are not alpha.7 functionality and must not be implied by the diagrams, examples, or package description:

- Repository localization and content-addressed artifact verification.
- Bootstrap-only initial composition.
- Background/asynchronous localization and its cancellation/error contract.
- Live promotion of Deferred bundles into an already-running `RuntimeAssembly`.
- Automatic application execution, UI/presentation, persistence, transport, and host lifecycle management.
- Immutability of MicroBundle objects themselves; only the documented collection structure is protected by read-only snapshots.

A deferred capability moves into scope only after its contract, owner, failure behavior, compatibility/evolution rules, and minimum test evidence are defined.

Before the release review is closed, also resolve or explicitly defer these contract-quality questions where applicable:

- Map every public runtime type and meaningful failure mode to source, focused documentation, and relevant tests.
- Confirm deterministic behavior for duplicate requests, version conflicts, dependency/version conflicts, and ordering.
- Record what diagnostics a host can inspect without turning FSM_COS into a logging framework.
- Confirm package documentation retains the same caveats as source documentation, including the mutable-object distinction.
- Review whether a dependency release is actually required; do not bump packages speculatively.

### F. Publication authorization and post-publish verification (separate from readiness)

- [ ] Obtain explicit owner approval for publication.
- [ ] Deliberately dispatch `.github/workflows/package.yml` from `master` with the required `publish` input set to `true`, after verifying the exact intended master commit with public dependencies. Do not dispatch from `development`, and do not enable publication as a side effect of ordinary CI.
- [ ] Verify the publish job succeeded and the exact version appears on NuGet.
- [ ] Record the published version, source SHA, workflow run, package URL, and package hash.
- [ ] Recheck that publication is not unintentionally enabled for future pushes.

## Status calculation

**Pre-publication readiness** is 100% only when gates A–D are evidenced for the same exact candidate SHA, documentation and package agree with source, no known release blocker remains, and external verification claims are honestly bounded.

Section F is deliberately separate from the readiness percentage. Publishing requires explicit owner authorization; post-publication checks apply only after a publish actually occurs. An unauthorized or unpublished package can still be release-ready, but it must never be described as published.

The latest Actions run and its artifacts are the evidence source for the current candidate. This file does not duplicate run IDs or hashes. If the successful run's source SHA differs from the current `development` HEAD, or the exact package artifact has not been inspected, the exact-candidate gates remain open. Any source or documentation edit creates a new candidate and requires re-verification.

## Authority

- [Documentation index and authority map](../DOCUMENTATION_INDEX.md)
- [Alpha.7 release notes](releases/0.1.0-alpha.7.md)
- [Architecture](ARCHITECTURE.md)
- [Development and CI modes](DEVELOPMENT.md)


## Supplemental release checks retained during checklist reconciliation

These checks preserve useful explicit gates from the prior readiness checklist without duplicating the source/API contract above.

### P0 — candidate and consumer evidence

- [ ] Capture the exact candidate SHA and link the Actions run for that SHA; any later source or documentation change requires a new verification.
- [ ] Confirm the declared project version, release notes, root README, NuGet README, documentation index, workflow checks, and intended package version agree on `0.1.0-alpha.7`.
- [ ] Confirm public package dependencies restore from the public NuGet feed without silently substituting a local source package.
- [ ] Confirm the development-source integration job consumes the intended sibling repositories and branches, and that package-source mapping selects the intended MicroBundleDomain artifact.
- [ ] Run `dotnet run --project samples/FSM_COS.MinimalConsumer/FSM_COS.MinimalConsumer.csproj` against the candidate source; verify the expected `Assembly order: 2 -> 1` output and that the sample is described as source-level evidence, not proof of a published package.
- [ ] Inspect the actual `.nupkg` for package ID/version, dependency metadata, target assembly, XML docs, README, license, release notes, required living SVG assets, and exclusion of internal planning material.
- [ ] Confirm failure paths do not claim a stable handoff after resolution, planning, loading, or arbitration failure.

### P1 — resolve or explicitly defer before release review

- [ ] Review resolution misses, missing dependencies, cycles, load failure, arbitration non-convergence, and configuration absence/failure where those behaviors are part of the contract.
- [ ] Record deterministic behavior for duplicate requests, version conflicts, dependency/version conflicts, and ordering.
- [ ] Record diagnostics a host can inspect without turning FSM_COS into a logging framework.
- [ ] Confirm the public documentation preserves the distinction between read-only collection structure and potentially mutable MicroBundle instances.
- [ ] Review the direct dependency set and update an owning dependency first if its contract genuinely needs to change; do not bump package versions speculatively.

**Priority legend:** P0 is a release blocker; P1 must be resolved or explicitly deferred in the release review; P2 is follow-up work, not an implied alpha.7 promise.

**Publication branch invariant:** publication remains separately authorized and must be deliberately dispatched from `master` with `publish=true`, using the intended master commit and public dependencies. This current policy supersedes the older checklist wording that named `development` as the dispatch branch. Do not dispatch or publish as part of reconciliation.


## Release intent and boundary invariant

Alpha.7 is a contract-hardening release. Its defining change is read-only snapshots for public collections representing a validated plan or assembled order; it does **not** make the contained MicroBundle instances immutable and does not implement deferred composition capabilities by implication.

- Keep FSM_COS platform-neutral and focused on composition.
- MicroBundleDomain defines bundles; a supplied catalog/resolver discovers them; FSM_COS plans, loads, arbitrates, and hands off; the host owns application behavior and lifecycle.
- Do not update dependency versions merely because newer source exists. Change and verify the owning dependency contract first when a real requirement exists.
- Do not publish until all P0 evidence gates are satisfied and the owner explicitly authorizes publication. This checklist is not publication authorization.

---

**The Singularity Workshop**  
[GitHub](https://github.com/TrentBest) · [Coder Legion](https://coderlegion.com/) · [Patreon](https://www.patreon.com/c/TheSingularityWorkshop) · [PayPal](https://www.paypal.com/donate/?hosted_button_id=3Z7263LCQMV9J)  
[ FSM_API ](https://github.com/TrentBest/FSM_API) · [ FSM_Serialization ](https://github.com/TrentBest/TheSingularityWorkshop.FSM_Serialization) · [ WebPage ](https://github.com/TrentBest/WebPage) · [ FSM_COS Documentation Standard ](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/DOCUMENTATION_STANDARD.md)  
**Because state shouldn't be a mess.**  
*And because static boundaries are invitations to cause trouble.*



---

## docs/AI_COMPOSITION.md
Source blob: `d921205855e0efa6f145bf296705d647943a83bc`

![The Singularity Workshop animated moniker](https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Renderer/development/docs/assets/moniker.svg)

# AI Composition with FSM_COS

## Purpose

![AI composition responsibility stack for domain concepts, ProtocolAi, GrammarAi, exchange context, and provider adaptation](assets/ai-composition.svg)

*This is an architectural map, not a chronological pipeline or proof that every depicted layer is implemented. The status qualifications below and in the diagram must remain explicit.*


Hosts may compose ProtocolAi and GrammarAi MicroBundles through FSM_COS. The kernel should not absorb their responsibilities or take a hard dependency on their concrete packages.

The following is a conceptual responsibility stack, **not a chronological execution sequence**. The future-facing layers are architectural proposals, not a claim that every capability is implemented today:

```text
Domain
  |
  v
ProtocolAi          WHAT
  |
  v
GrammarAi           HOW
  |
  v
AI Exchange         WHAT + HOW + CONTEXT + REQUEST
  |
  v
Provider Adapter    transport / authentication
  |
  v
LLM
  |
  v
validated response
  |
  v
FSM_COS             composition
  |
  v
RuntimeAssembly
  |
  v
Host / Experience
```

The exact direction of the arrows may vary by host, but the ownership boundaries should remain recognizable.

---

## Why FSM_COS should consume them

FSM_COS already answers:

> What must be assembled before a host can receive a stable runtime?

ProtocolAi and GrammarAi can become MicroBundle capabilities that contribute:

- semantic vocabularies;
- grammar definitions;
- AI-facing exchange capabilities;
- provider adapters;
- contextual interaction services.

The composition kernel should assemble those capabilities without becoming an AI framework.

For example:

```text
RuntimeManifest
    |
    +-- ProtocolAi bundle
    |
    +-- GrammarAi bundle
    |
    +-- AI Exchange bundle
    |
    +-- GUI bundle
    |
    +-- Experience bundle
    |
    v
FSM_COS
    |
    v
RuntimeAssembly
```

A different runtime may request only ProtocolAi and GrammarAi.

Another may add a connected provider.

Another may use clipboard-only interaction.

The composition mechanism remains the same.

---

## Clipboard is a host capability, not a composition primitive

A clipboard round trip can demonstrate that the semantic protocol does not require a network connection.

```text
assembled context
      |
      v
copy
      |
      v
human-facing LLM
      |
     copy
      |
      v
paste
      |
      v
host validation
```

FSM_COS should assemble the capability required to produce and consume the exchange.

It should not call the clipboard itself.

Clipboard access belongs to the host manifestation layer.

---

## Connected providers

A connected interaction introduces another MicroBundle boundary:

```text
AI Exchange
    |
    v
Provider Adapter
    |
    +-- endpoint
    +-- model
    +-- credential reference
    |
    v
LLM
```

The provider adapter owns transport.

The host owns credential acquisition and policy.

FSM_COS assembles the adapter when the manifest requests it.

This allows a runtime to remain provider-neutral at the composition level.

---

## Credential boundary

An API key must never become ordinary RuntimeManifest content.

The manifest may identify a provider capability or a credential reference, but the secret itself belongs to a secure host-owned credential boundary.

```text
RuntimeManifest
    |
    | provider = X
    | credentialRef = local-user
    v
FSM_COS
    |
    v
Provider Adapter
    ^
    |
Host credential store
    |
    +--> actual secret
```

This is a future contract.

The alpha should not invent a credential store merely to make the diagram executable.

---

## Contextual interaction at speed

The architecture should distinguish **semantic context** from **transport payload**.

A contextual session may retain:

- active protocol definitions;
- active grammar;
- selected runtime objects;
- accepted prior exchanges;
- host capabilities;
- current state;
- current request.

The provider or clipboard transport can then receive a representation of that context.

```text
persistent semantic session
          |
          +-- stable context
          |
          +-- changing delta
          |
          v
     exchange builder
          |
      +---+---+
      |       |
 clipboard  provider
      |       |
      +---+---+
          |
          v
        LLM
```

The phrase **speed** should therefore be tested at multiple levels:

- human interaction latency;
- context construction time;
- serialization time;
- provider round-trip time;
- response validation time;
- token usage;
- repeated-turn payload growth.

FSM_COS should not claim performance until those measurements exist.

---

## The architectural stop line

FSM_COS assembles the AI capabilities.

It does not become:

- the LLM;
- a provider SDK;
- a prompt editor;
- a credential vault;
- a GUI;
- a clipboard manager;
- a conversation UI;
- an Experience.

That keeps the same invariant that governs the rest of the kernel:

> **Assemble what was requested. Return a stable composition. Hand it to the host.**

---

## Source-level host composition pattern

The source-level integration exercises the intended composition boundary:

```text
RuntimeManifest
    |
    +-- AI exchange capability
    |     |
    |     +-- ProtocolAi
    |     +-- GrammarAi
    |
    +-- GUI-facing capability
    |
    v
FSM_COS
    |
    v
RuntimeAssembly
    |
    v
host
    |
    +-- shared GUI builder
    +-- clipboard
    +-- provider transport
```

The source-level proof is limited to the composition contracts and test path; it does not establish that the visible WebPage experience is fully driven by `RuntimeAssembly`. See [WebPage Integration](WEBPAGE_INTEGRATION.md) for that explicit boundary.

The architectural point is that **FSM_COS does not need to reference the AI or GUI packages to compose them**.

A host supplies an `IMicroBundleCatalog`. The catalog resolves concrete capability bundles, while FSM_COS handles dependency ordering, loading, arbitration, and the stable handoff.

A GUI-capability bundle could build a platform-neutral semantic tree using `TheSingularityWorkshop.GUI.Core`; a host-specific renderer such as Blazor could manifest that tree. Treat this as the intended integration shape until the complete host path is demonstrated.

The exchange itself can be assembled from the actual ProtocolAi and GrammarAi definitions:

```text
PROTOCOL
  [integer-backed vocabulary]

GRAMMAR
  [integer-backed structure]
```

A host could expose that deterministic representation through an **Extract** action, send it to an LLM by clipboard or provider transport, and receive a response through the host's input surface. The end-to-end interaction remains a host-level integration concern.

FSM_COS remains deliberately unaware of:

- browser clipboard APIs;
- Blazor or WPF rendering;
- provider SDKs;
- API keys or credential stores;
- LLM inference;
- arbitrary response parsing.

This source-level path is an initial proof of the composition boundary. A full host integration must separately demonstrate that manifestation and transport consume the assembled result end to end.

---

## Future MicroBundle graph

A plausible future graph is:

```text
                    RuntimeManifest
                          |
          +---------------+----------------+
          |               |                |
          v               v                v
     ProtocolAi       GrammarAi       GUI / Host
          |               |
          +-------+-------+
                  |
                  v
             AI Exchange
                  |
          +-------+-------+
          |               |
          v               v
      Clipboard      Provider Adapter
                          |
                          v
                         LLM
```

This is deliberately a **composition hypothesis**, not an alpha API promise.

The first implementation task is to make the semantic contracts stable enough that FSM_COS can consume them without redefining them.



---

## Semantic layers: from meaning to application coordination

The AI architecture is broader than the exchange transport.

The current semantic model is:

```text
ProtocolAi
    WHAT exists
       ↓
GrammarAi
    HOW identities may organize
       ↓
CommandAI
    FUNCTION assembled from grammar
       ↓
OperatingSystemAI
    DISTRIBUTION / ROUTING of commands
       ↓
AppAI
    APPLICATION-LEVEL coordination
```

This should be understood as a **capability hierarchy**, not a requirement that every interaction traverse every layer.

### ProtocolAi — meaning

ProtocolAi is the foundation.

It provides stable mappings between integer identities and application-owned semantic values. It is deliberately unaware of command execution.

### GrammarAi — legal structure

GrammarAi takes protocol identities and describes which combinations form valid semantic statements.

It is the structural strainer above the vocabulary strainer.

### CommandAI — executable semantic composition

CommandAI is a candidate layer above GrammarAi.

Its concern is not merely that a statement is grammatically valid, but that valid grammar can be assembled into a meaningful unit of functionality.

For example:

```text
Tool registration

Buttons
  A

Actions
  Click

Command
  Click(A)
```

The actual GUI object, delegate, framework event, or memory reference remains host-owned. The AI-facing representation contains semantic identities and command structure.

### OperatingSystemAI — distribution and routing

If this layer proves necessary, it belongs **above CommandAI**, not beneath it.

CommandAI answers:

> What functionality does this command represent?

An OperatingSystemAI layer would answer questions such as:

> Which command-capable domain owns this functionality?

> Which available application, process, service, or experience should receive it?

> How should a command move between those domains?

That makes it a coordinator of command-capable resources rather than another command syntax.

The name is intentionally provisional. The architectural role matters before the package name does.

### AppAI — application-level orchestration

An AppAI layer can then describe interaction across application boundaries.

For example:

```text
Application A
    ↓
extract semantic data
    ↓
AppAI
    ↓
Application B
    ↓
invoke exposed CommandAI capability
```

This is where moving data between applications, selecting capabilities exposed by those applications, and composing cross-application workflows becomes meaningful.

Again, not every host needs this layer.

---

## The important consequence: layers are interaction-dependent

A user interacting with a tool does not inherently invoke the entire AI stack.

The tooling determines the highest semantic layer required by the interaction.

```text
Simple semantic selection
    ProtocolAi

Structured statement
    ProtocolAi + GrammarAi

Tool operation
    ProtocolAi + GrammarAi + CommandAI

Cross-tool / system routing
    ... + OperatingSystemAI

Cross-application workflow
    ... + AppAI
```

This is important because it prevents the architecture from turning every interaction into a giant prompt.

The host should expose the **smallest semantic surface that can express the requested interaction**.

That is also where the token-efficiency hypothesis becomes concrete: higher layers should add only the additional vocabulary and structure required for the interaction they govern.

---

## Operational-domain extraction

An **Extract to Clipboard** action should therefore extract a semantic snapshot of the current operational domain rather than a prose description of the UI.

A tool can introduce itself when initialized:

```text
I expose:

Buttons
  A

Actions
  Click
```

The registration can become integer-backed protocol identities, after which GrammarAi can describe legal composition and CommandAI can assemble the executable semantic unit.

The resulting exchange is conceptually closer to:

```text
[Actions #]
    [Click]
        [Button A]
```

than:

```text
"Click the button named A."
```

The first form gives the model a constrained semantic address space. The second asks the model to infer relationships from language.

The architecture therefore aims to move probabilistic inference toward deterministic validation:

```text
LLM probability
      ↓
Protocol identity
      ↓
Grammar constraint
      ↓
Command composition
      ↓
host validation / authorization
      ↓
deterministic execution
```

This is the central reason for keeping the layers separate.

---

<p align="center"><em>The Singularity Workshop — Tools for the curious, the bold, and the systemically inclined.</em><br><strong>Because state shouldn't be a mess.</strong><br><em>And because static boundaries are invitations to cause trouble.</em></p>

---

**The Singularity Workshop**  
[GitHub](https://github.com/TrentBest) · [Coder Legion](https://coderlegion.com/) · [Patreon](https://www.patreon.com/c/TheSingularityWorkshop) · [PayPal](https://www.paypal.com/donate/?hosted_button_id=3Z7263LCQMV9J)  
[ FSM_API ](https://github.com/TrentBest/FSM_API) · [ FSM_Serialization ](https://github.com/TrentBest/TheSingularityWorkshop.FSM_Serialization) · [ WebPage ](https://github.com/TrentBest/WebPage) · [ FSM_COS Documentation Standard ](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/DOCUMENTATION_STANDARD.md)  
**Because state shouldn't be a mess.**  
*And because static boundaries are invitations to cause trouble.*



---

## docs/ARBITRATION.md
Source blob: `4150384b368ff1a49acbb12aac1452030520bf87`

![The Singularity Workshop animated moniker](https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Renderer/development/docs/assets/moniker.svg)

# Arbitration and Convergence

> **Arbitration gives a composition a bounded runtime reconciliation step; it does not automatically prove semantic compatibility.**

See [FSM_COS Theory](THEORY.md#8-arbitration-is-composition-negotiation) for the compact model. This document is the deeper treatment.

![Arbitration convergence: loaded bundles participate in complete rounds until a round reports no changes or the configured maximum is exhausted](assets/arbitration-convergence.svg)

*Implementation note: the successful round is recorded as a zero-based index in `RuntimeAssembly.ArbitrationRounds` (`0` means the first round converged). If every allowed round reports changes, `FsmCos.Execute` throws instead of returning an unstable assembly.*

## UML class diagram: arbitration participants

This is a simplified, source-derived overview of the arbitration relationships, not a complete per-type API inventory. Parameter types and nullability are abbreviated; the source and the contract above remain authoritative for exact signatures and failure behavior.

~~~mermaid
classDiagram
    class FsmCos {
        +Execute(manifest, configurationSource) RuntimeAssembly
    }
    class ArbitrationContext {
        +RuntimeId: ulong
        +ExperienceContext: IStateContext
        +Bundles: IReadOnlyList~IMicroBundle~
        +LoadedBundles: IReadOnlyList~IMicroBundle~
    }
    class IMicroBundle {
        <<MicroBundleDomain contract>>
        +Arbitrate(context, roundIndex) bool
    }
    class IMicroBundleArbitrationContext {
        <<MicroBundleDomain contract>>
        +RuntimeId: ulong
        +Bundles
        +ExperienceContext
    }
    class RuntimeAssembly {
        +RuntimeId: ulong
        +Bundles: IReadOnlyList~IMicroBundle~
        +ArbitrationRounds: int
    }

    FsmCos ..> ArbitrationContext : creates after loading
    ArbitrationContext ..|> IMicroBundleArbitrationContext
    IMicroBundle ..> IMicroBundleArbitrationContext : receives during each round
    FsmCos ..> RuntimeAssembly : returns only on convergence
    RuntimeAssembly o-- "0..*" IMicroBundle : loaded participants
~~~

The two interfaces shown as MicroBundleDomain contracts are external to this repository; FSM_COS supplies their concrete context and invokes the domain-owned arbitration callback. A returned assembly means a complete round reported no changes before the configured bound, not that all possible domain compatibility rules have been proven.

## The question arbitration actually answers

Dependency resolution asks:

> **What must exist?**

Arbitration asks:

> **Now that those things exist together, can the composition settle into a state in which their contracts remain satisfiable?**

That distinction is why FSM_COS is a composition system rather than a sophisticated loader.

A loader can establish an inventory. It cannot, by itself, establish that independently authored capabilities can coexist coherently once installed.

Arbitration creates a bounded reconciliation phase between **installation** and **handoff**.

~~~text
Runtime Manifest
      ↓
dependency closure
      ↓
configured installation
      ↓
ARBITRATION
      ↓
convergence
      ↓
RuntimeAssembly
~~~

The result is not a winner. The result is a **stable relationship between participants**.

---

## Why interoperability is normally difficult

Interoperability is often reduced to questions such as:

- Can component A call component B?
- Can they deserialize the same message?
- Do they share an interface?
- Can they load into the same process?

Those questions matter, but they describe mostly **pairwise compatibility**.

Real compositions are contextual.

~~~text
GUI          expects a presentation surface
Physics      expects spatial state
Navigation   expects traversable space
Identity     expects a visitor anchor
Telemetry    expects observable transitions
~~~

None of those capabilities necessarily owns the others.

Traditional application architecture often solves this by creating a larger owner:

~~~text
              MASTER APPLICATION
              /       |                  GUI     Physics   Navigation
~~~

That master becomes the place where every conflict is understood, every ordering rule is encoded, and every new capability eventually acquires another special case.

FSM_COS deliberately avoids making the composition kernel that master.

Arbitration provides another model:

~~~text
          GUI ─────────────┐
                           │
       Physics ────────────┼──► shared composition
                           │
      Navigation ──────────┤
                           │
       Identity ───────────┘
~~~

Each participant can respond to the composition it finds without requiring one universal application owner to understand every domain.

---

## Arbitration changes the interoperability concern

Without arbitration, interoperability tends to mean:

> **Can these modules be connected?**

With arbitration, interoperability can mean:

> **Can these modules participate in a shared composition and converge without requiring one module to become the universal owner?**

That is the architectural leap.

A may be compatible with B in isolation and compatible with C in isolation, while A+B+C creates a condition none of the pairwise interfaces can express.

Arbitration gives the assembled system a place to discover and reconcile that condition.

This is not merely a second initialization pass. It is the phase in which the composition answers whether it can **live together**.

---

## Arbitration is not priority arbitration

The name can suggest a judge selecting a winner. That is not the model.

FSM_COS does not define a universal hierarchy such as:

~~~text
highest priority wins
lowest priority loses
~~~

Instead:

~~~text
participant A changes something
        ↓
participant B observes the composition
        ↓
B may change something
        ↓
participant C observes the new composition
        ↓
repeat until a full round changes nothing
~~~

The composition itself is the shared object of negotiation.

A navigation bundle can establish that a route is impossible. A GUI bundle can establish that a surface must exist. A physics bundle can establish that a spatial constraint must hold. None of those facts automatically makes one bundle sovereign over the others.

Arbitration lets capabilities express consequences instead of demanding ownership.

---

## The convergence law

The alpha contract is deliberately small:

1. every reachable bundle is installed;
2. every installed bundle participates in a round;
3. a participant returns true when its participation changed the composition;
4. it returns false when it did not;
5. a complete round with no changes means convergence;
6. the engine stops at the configured maximum number of rounds;
7. failure to converge is an error.

~~~text
round 1 → changed
round 2 → changed
round 3 → unchanged
              ↓
          CONVERGED
~~~

The current ten-round bound is a safety boundary, not a universal constant. The `RuntimeAssembly.ArbitrationRounds` property records the **zero-based index** of the converging round: `0` means the first round completed without changes. Therefore, it is not the total count of calls made to `Arbitrate`; a successful assembly with value `0` still means every loaded bundle participated in one round.

The deeper invariant is:

> **No unstable composition crosses the RuntimeAssembly boundary as if it were stable.**

## What convergence proves—and what it does not

A successful arbitration pass establishes a narrow, observable fact: every loaded bundle participated, and a complete round reported no composition changes before the configured limit was reached.

It does **not**, by itself, prove that:

- the bundles' domain assumptions are correct;
- every semantic compatibility rule has been implemented;
- external resources are available;
- the host will execute or present the assembly correctly;
- a bundle has not incorrectly returned `false` while a requirement remains unsatisfied;
- the overall application is safe or correct.

Those stronger claims depend on the contracts and implementations supplied by the participants and host. A bundle must encode the compatibility conditions it knows how to evaluate, and tests must exercise those conditions. FSM_COS enforces the bounded round protocol; it cannot infer every domain-specific invariant on behalf of independently authored bundles.

**Precise guarantee:** FSM_COS does not return an assembly when the arbitration protocol reports continued change through the configured maximum. A returned assembly means the participants reported convergence—not that every possible form of interoperability has been proven.


## Example: a shared capability name is not compatibility

Imagine a tanker MicroBundle and a station MicroBundle both advertise **docking**. That shared word is discovery metadata, not evidence that the operations can safely compose.

A domain-owned docking contract might need to compare facts such as:

- docking-interface or protocol identity and compatible contract versions;
- the geometry and reference frame of each docking port;
- permitted approach and capture envelopes;
- size, mass, clearance, or load limits relevant to the operation;
- units, coordinate conventions, and timebase where those affect interpretation.

These are illustrative contract facts, **not fields that the current MicroBundle descriptor automatically supplies**. The bundle author or a domain-specific adapter must define which facts are required, where they come from, and how they are evaluated. A generic label, a matching provider name, or the fact that both bundles loaded is insufficient.

The composition should therefore distinguish three outcomes in its domain logic:

1. **Compatible:** the declared contracts and configuration provide enough evidence to permit this pairing.
2. **Incompatible:** a known contract condition fails; the pairing must not be treated as usable.
3. **Unresolved:** required evidence is absent or cannot be interpreted; absence of a known conflict is not proof of compatibility.

The runtime/experience owns the policy for what to do with an incompatible or unresolved pairing—reject it, request different bundles or configuration, or present the reason to the author. The bundles own the domain facts they can validate. The authoritative host or simulation still determines whether a permitted docking attempt is physically feasible and what actually happens.

### Keep the responsibilities separate

- **Runtime manifest:** selects MicroBundle identities and requested versions. It does not embed each bundle's configuration.
- **Configuration source:** supplies configuration separately for each selected MicroBundle. FSM_COS passes that configuration through; it does not interpret domain-specific fields.
- **MicroBundle contract and implementation:** define capabilities, requirements, and the domain-specific checks a participant can actually perform.
- **FSM_COS arbitration:** provides the bounded reconciliation phase. It does not infer a universal compatibility schema from capability names.
- **Host / experience / simulation:** owns the higher-level choice and the authoritative execution outcome.

The current arbitration callback reports whether a participant changed the composition; it is **not** a structured compatibility-diagnostics API.

The test suite now includes a deliberately domain-owned example in `FsmCosTests`: two bundles advertise the same `docking` provider label, while their test-only contract identities determine whether arbitration accepts the pairing or throws on a known conflict. This demonstrates the existing extension point, **not** a built-in FSM_COS compatibility validator or a production docking schema. The exception is a fail-closed fixture; machine-readable compatible / incompatible / unresolved results remain future contract work. If callers need machine-readable rejection categories or explanations, that needs an explicit contract extension or domain-owned result surface. Do not disguise a missing compatibility check as successful convergence, and do not add a universal schema here before real domain contracts establish what it must express.

---

## Why bounded convergence matters

An open-ended negotiation loop could permit pathological participants:

~~~text
A changes B
B changes A
A changes B
B changes A
...
~~~

Without a bound, the runtime has no principled stopping point.

With a bound:

~~~text
round 1
round 2
...
round N
  ├── stable → success
  └── unstable → failure
~~~

The host therefore gets a meaningful contract:

- RuntimeAssembly means the composition reached its stable point;
- failure means the requested composition could not establish that point.

The host does not need to invent its own interpretation of “probably stable enough.”

---

## The strongest participants are boring

A good arbitrator is not an application loop hidden inside a MicroBundle.

A good arbitrator is:

- deterministic;
- local in responsibility;
- explicit about what it changed;
- safe to run again;
- respectful of the shared composition;
- independent of the final host where possible.

The ideal shape is effectively:

~~~text
apply requirement
    ↓
already satisfied?
    ├── yes → no change
    └── no  → satisfy it → changed
~~~

That makes convergence legible and testing meaningful.

A second pass over an already-satisfied condition should not manufacture another change simply to keep the system busy.

---

## Structural composition versus behavioral reconciliation

Dependencies are usually represented as a graph:

~~~text
Experience
 ├── Navigation
 │    └── SpatialModel
 ├── GUI
 │    └── PresentationModel
 └── Identity
      └── VisitorModel
~~~

The graph establishes **what is installed**.

But Navigation may reveal a spatial interaction point. SpatialModel may expose a constraint that changes presentation. The GUI may reveal that an interaction requires an identity affordance. Identity may then change available presentation state.

The dependency graph establishes structural composition.

Arbitration establishes **behavioral reconciliation**.

That distinction is fundamental.

---

## Arbitration creates an interoperability seam

A traditional integration boundary often looks like:

~~~text
Provider API → Consumer API
~~~

FSM_COS adds a third space:

~~~text
Provider
    ↓
shared composition
    ↑
Consumer
~~~

Neither participant has to own the entire application. Both can contribute facts or constraints to the shared composition.

That creates a **negotiation seam**.

This seam becomes particularly valuable when capabilities come from different authors, packages, domains, or future versions of the ecosystem. The composition kernel provides a stable place for them to reconcile without forcing their internal implementations to know about one another.

That is why arbitration can become a major interoperability primitive rather than a minor lifecycle callback.

---

## What arbitration must not become

Arbitration must not become a back door for application infrastructure.

A MicroBundle should not use arbitration to:

- render UI;
- open a browser;
- start arbitrary background processes;
- mutate unrelated host state;
- perform network orchestration;
- become an application scheduler;
- encode every business rule in the composition kernel;
- secretly implement serialization;
- establish an undocumented universal priority hierarchy.

If a concern can be handled after RuntimeAssembly, it belongs after RuntimeAssembly.

The boundary is valuable precisely because not everything is allowed to cross it.

---

## Arbitration versus serialization

Serialization answers:

> **How do I represent semantic state as bytes?**

Arbitration answers:

> **Given these semantic participants, have we reached a stable composition?**

Those concerns can touch the same configuration data without being the same problem.

~~~text
FSM_Serialization
      ↓ representation
BundleRequest
      ↓
FSM_COS
      ↓ composition
Arbitration
      ↓
RuntimeAssembly
~~~

Keeping these boundaries separate means a serialization format can evolve without changing convergence semantics.

See [FSM_Serialization Theory](https://github.com/TrentBest/TheSingularityWorkshop.FSM_Serialization/blob/master/docs/THEORY.md).

---

## The interoperability thesis

The thesis is:

> **Interoperability is not only the ability to exchange data or invoke APIs. In a compositional runtime, interoperability is the ability of independently defined capabilities to participate in a shared state space and converge without requiring one of them to become the universal owner.**

FSM_COS does not claim that arbitration solves every interoperability problem.

It creates a place where one important class of problem can be addressed:

**composition-level compatibility.**

That is a meaningful architectural boundary.

---

## The contract at the edge

At the end of arbitration, the kernel should be able to say one of two things:

~~~text
SUCCESS
The requested composition converged.
RuntimeAssembly is valid for handoff.
~~~

or:

~~~text
FAILURE
The requested composition did not converge within the safety bound.
No stable RuntimeAssembly is produced.
~~~

That is stronger than “all DLLs loaded.”

It is stronger than “all constructors succeeded.”

It is stronger than “the host can probably figure it out.”

It gives the composition boundary an actual semantic completion condition.

---

## The short version

~~~text
LOAD
  ↓
What exists?
  ↓
ARBITRATE
  ↓
Can what exists live together?
  ↓
CONVERGE
  ↓
Is the composition stable?
  ↓
ASSEMBLE
  ↓
HAND OFF
~~~

> **Arbitration is not a fight for control. It is the mechanism by which independent capabilities discover whether they can become one stable system.**


---

## 🔗 The Singularity Workshop

FSM_COS is one layer in a deliberately troublesome ecosystem:

- **[FSM_API](https://github.com/TrentBest/FSM_API)** — behavior and state.
- **[FSM_COS](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS)** — composition and runtime assembly.
- **[FSM_Serialization](https://github.com/TrentBest/TheSingularityWorkshop.FSM_Serialization)** — representation and the byte boundary.
- **[WebPage](https://github.com/TrentBest/WebPage)** — browser manifestation and proving ground.
- **[FSM_API_Unity](https://github.com/TrentBest/FSM_API_Unity)** — Unity manifestation.

<p align="center"><em>The Singularity Workshop — Tools for the curious, the bold, and the systemically inclined.</em><br><strong>Because state shouldn't be a mess.</strong><br><em>And because static boundaries are invitations to cause trouble.</em></p>

---

**The Singularity Workshop**  
[GitHub](https://github.com/TrentBest) · [Coder Legion](https://coderlegion.com/) · [Patreon](https://www.patreon.com/c/TheSingularityWorkshop) · [PayPal](https://www.paypal.com/donate/?hosted_button_id=3Z7263LCQMV9J)  
[ FSM_API ](https://github.com/TrentBest/FSM_API) · [ FSM_Serialization ](https://github.com/TrentBest/TheSingularityWorkshop.FSM_Serialization) · [ WebPage ](https://github.com/TrentBest/WebPage) · [ FSM_COS Documentation Standard ](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/DOCUMENTATION_STANDARD.md)  
**Because state shouldn't be a mess.**  
*And because static boundaries are invitations to cause trouble.*



---

## docs/ARCHITECTURE.md
Source blob: `3849331d34fa1055194786343dc1246300dbd4c4`

![The Singularity Workshop animated moniker](https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Renderer/development/docs/assets/moniker.svg)

# FSM_COS Architecture

> **Architecture answers how the composition kernel performs the theory.** For the deeper “why,” start with [FSM_COS Theory](THEORY.md).

FSM_COS is a small composition kernel. This document describes how its pieces cooperate rather than redefining concepts owned by neighboring systems.

## UML class diagram: composition kernel

This is a simplified, source-derived UML overview of the principal types and their relationships, not a complete per-type member inventory. Method signatures are abbreviated for legibility; use the XML-documented source and focused contract sections for exact parameter types, nullability, and full public members. External MicroBundle contracts are consumed from MicroBundleDomain; the schedule is dependency metadata, not the FSM_API process scheduler.

~~~mermaid
classDiagram
    class IFsmCos {
        <<interface>>
        +Execute(manifest, configurationSource) RuntimeAssembly
    }
    class FsmCos {
        +Execute(manifest, configurationSource) RuntimeAssembly
    }
    class IMicroBundleCatalog {
        <<interface>>
        +TryResolve(bundleId, version, bundle) bool
        +TryResolve(bundleId, bundle) bool
    }
    class IMicroBundleConfigurationSource {
        <<interface>>
        +TryGetConfiguration(runtimeId, bundleId, version, configuration) bool
    }
    class RuntimeManifest {
        +RuntimeId: ulong
        +Bundles: IReadOnlyList
        +ExperienceContext: IStateContext
        +LoadPlan: IReadOnlyList
        +Schedule: RuntimeManifestSchedule
        +Intent: SemanticIntent
        +ValidateStagedPlan()
    }
    class MicroBundleManifestEntry {
        <<record struct>>
        +BundleId: ulong
        +Version: string
    }
    class RuntimeManifestEntry {
        <<record>>
        +Reference: MicroBundleReference
        +Stage: ManifestLoadStage
        +IsValid: bool
    }
    class MicroBundleReference {
        <<record struct>>
        +BundleId: ulong
        +Version: string
        +ContentHash: string
        +IsValid: bool
        +Matches(entry): bool
    }
    class ManifestLoadStage {
        <<enumeration>>
        Bootstrap
        Resident
        Deferred
    }
    class RuntimeManifestSchedule {
        +Entries: IReadOnlyList
        +Dependencies: IReadOnlyList
        +IndependentEntries: IReadOnlyList
        +DependencyEntries: IReadOnlyList
        +DependenciesOf(bundleId)
        +IsDependencyReady(bundleId, isLoaded): bool
    }
    class RuntimeManifestDependency {
        <<record struct>>
        +BundleId: ulong
        +DependencyId: ulong
        +IsValid: bool
    }
    class RuntimeAssembly {
        +RuntimeId: ulong
        +Bundles: IReadOnlyList
        +ArbitrationRounds: int
        +Intent: SemanticIntent
        +TryGetBundle(bundleId, bundle): bool
    }

    FsmCos ..|> IFsmCos
    FsmCos --> IMicroBundleCatalog : resolves
    FsmCos --> IMicroBundleConfigurationSource : optional input
    FsmCos --> RuntimeManifest : consumes
    FsmCos --> RuntimeAssembly : returns
    RuntimeManifest *-- "0..*" MicroBundleManifestEntry : root requests
    RuntimeManifest o-- "0..*" RuntimeManifestEntry : optional load plan
    RuntimeManifest o-- RuntimeManifestSchedule : optional schedule
    RuntimeManifestEntry --> MicroBundleReference : published identity
    RuntimeManifestEntry --> ManifestLoadStage : intended stage
    RuntimeManifestSchedule o-- "0..*" RuntimeManifestEntry : entries
    RuntimeManifestSchedule o-- "0..*" RuntimeManifestDependency : dependency edges
~~~

The diagram is a map of type contracts, not a claim that constructing a schedule performs localization or that stage metadata activates deferred loading. See [Staged Manifest Loading](STAGED_MANIFEST_LOADING.md) for the implementation boundary.

## Runtime flow

![FSM_COS runtime flow](assets/fsm-cos-overview.svg)

![FSM_COS validation and composition path, generated with GUI.SVG](assets/fsm-cos-preflight.svg)

*Generated from `samples/DocumentationVisuals/Program.cs` using a pinned GUI.SVG source revision. This contract map shows the preflight gate, optional schedule validation, dependency-first loading, arbitration, and failure boundaries; it is not a live execution trace.*



## Core contracts

### RuntimeManifest
Identifies the runtime being assembled and supplies versioned root MicroBundle entries. It can also carry an optional host-provided `SemanticIntent`, an optional load-plan description, and an optional dependency schedule. Intent is passed through to the result; staged metadata is validated according to the current alpha contract and does not, by itself, activate repository localization or staged execution. The manifest is composition input, not application behavior. See [Runtime Manifest](RUNTIME_MANIFEST.md) and [Runtime Manifest Theory](MANIFEST_THEORY.md).

### MicroBundle manifest entries
A manifest entry identifies a requested MicroBundle and its requested version. Configuration is intentionally not part of the manifest. A separate configuration source may provide per-MicroBundle configuration at runtime.

### IMicroBundleCatalog
Resolves a bundle identity to the IMicroBundle required for installation. The catalog is supplied by the host, so future Warehouse-backed or generated resolvers do not require a different composition algorithm.

### IMicroBundle
Consumes the domain-owned `MicroBundleDescriptor` for identity/version/providers, while retaining composition-specific dependency configuration, Load, and Arbitrate behavior. FSM_COS does not redefine MicroBundle domain metadata. It does not expose a GUI, web server, or host lifecycle contract.

### MicroBundleLoadContext
Carries runtime identity and configuration available during installation. Configuration remains opaque to FSM_COS.

### ArbitrationContext
Exposes runtime identity, the currently loaded bundle set, and an optional FSM_API `IStateContext` supplied by the host. It is composition context, not host lifecycle state.

### RuntimeAssembly
The result surface of the composition pass: runtime identity, loaded bundles, the zero-based index of the round that reported convergence, and the optional `SemanticIntent` carried by the request. An `ArbitrationRounds` value of `0` means the first round converged; it is not the number of `Arbitrate` calls. Intent is passed through for the host; it does not make FSM_COS an input handler or presentation layer. See [RuntimeAssembly](RUNTIME_ASSEMBLY.md) and [FSM_COS Theory — RuntimeAssembly is the handoff object](THEORY.md#9-runtimeassembly-is-the-handoff-object).

![Dependency closure and installation order](assets/dependency-resolution.svg)

The conceptual reason for deriving this order rather than encoding it in the manifest is explained in [FSM_COS Theory — Composition starts with roots, not a giant object graph](THEORY.md#4-composition-starts-with-roots-not-a-giant-object-graph).

## Dependency resolution

FSM_COS first resolves and validates the complete reachable dependency closure before calling any bundle's `Load`. This rejects missing dependencies, malformed dependency contracts, identity mismatches, and cycles before bundle loading can create side effects. It snapshots the resolved bundle objects and dependency requests for the load pass, so the catalog is not queried a second time during loading. The subsequent depth-first installation order is iterative rather than recursive, avoiding call-stack exhaustion on deep graphs.

    requested A
       │
       ├── B
       │   └── C
       └── D

The resulting installation order is C, B, D, A.

Two guards remain fundamental during installation: already-loaded IDs are skipped, and IDs currently loading are tracked to detect cycles. The preflight graph validation independently detects cycles before installation begins.

Missing bundles and dependency cycles are composition failures.

## Configuration propagation

Configuration is an external runtime input. FSM_COS accepts configuration through an abstraction supplied by the host/repository layer; it does not read configuration files or interpret their format. If no configuration is available for a bundle, the bundle is loaded without external configuration and uses its defaults.

## Arbitration

FSM_COS resolves the catalog's actual reachable dependency graph and validates identity/dependency declarations before calling any MicroBundle `Load` method. When a manifest supplies a dependency schedule, it also checks the resolved closure, versions, and edges against that schedule before loading. This is a validation boundary, not a claim that FSM_COS localizes artifacts or implements staged/background loading. After reachable bundles are loaded, FSM_COS creates one ArbitrationContext.

    for each round
        for each loaded bundle
            changed |= bundle.Arbitrate(context, round)
        if no change → converged

If the configured maximum is reached without convergence, FSM_COS throws instead of returning an unstable assembly. The current default is ten rounds.

## Failure and partial side effects

Resolution, configuration lookup, bundle loading, and arbitration failures propagate to the caller; FSM_COS does not return a `RuntimeAssembly` when composition fails. Arbitration begins only after all reachable bundles have completed `Load`.

Composition is **not a transactional rollback boundary**. If a dependency completes `Load` and a later dependent bundle throws from `Load`, the earlier dependency's in-memory changes or external side effects cannot be generically undone by FSM_COS. The failing bundle is not added to the internal loaded set, and arbitration does not begin, but already-executed `Load` code may have changed state outside FSM_COS. Bundle authors and hosts must make loading safe for their lifecycle, and perform any required cleanup at the layer that owns those side effects.

The same principle applies to exceptions during arbitration: FSM_COS propagates the failure and does not hand off an assembly, but it cannot reverse arbitrary mutations performed by bundle code. Hosts should treat a failed `Execute` as a failed composition attempt, not as proof that all bundle-side effects were reverted.

### Diagnostic surface

The diagnostic contract is deliberately small: `Execute` either returns a converged `RuntimeAssembly` or throws. FSM_COS does not keep a logging pipeline, diagnostics registry, or partially successful assembly for the host to inspect. For failures it detects itself, exception messages identify the relevant bundle IDs and, where applicable, requested/resolved versions, dependency cycles, missing schedule entries, or mismatched dependency edges. Exceptions raised by the catalog, configuration source, `Load`, or `Arbitrate` are allowed to propagate so their owning layer retains the original failure details.

Hosts should catch failures at their composition boundary, preserve the exception and its context in their own logging/telemetry system, and apply cleanup according to their lifecycle policy. Message text is useful for troubleshooting, but consumers should not treat exact prose as a stable machine-readable error code.

## Catalog boundary

The catalog stays outside the composition algorithm:

    FSM_COS ← Catalog ← in-memory / generated registry / Warehouse / cache / Domain resolver

The composition engine should not need to know where a bundle came from.

## Host boundary

    FSM_COS
       │
       ▼
RuntimeAssembly
       │
       ├── WebPage → browser experience
       ├── AnyApp → desktop/local experience
       └── other hosts → their own manifestation

## Current development boundary

The active development line is refining the manifest/configuration boundary while preserving the small composition kernel:

```text
Manifest (bundle + version)
        ↓
resolver / catalog
        ↓
configuration source (optional)
        ↓
dependency closure
        ↓
Load()
        ↓
Arbitrate() until stable
        ↓
RuntimeAssembly
```

File I/O, repository transport, serialization, host lifecycle, and presentation remain outside the kernel.

Execution scheduling, resource allocation, Warehouse integration, and host lifecycle belong to later layers when their contracts are sufficiently clear.

---

## 🔗 The Singularity Workshop

FSM_COS is one layer in a deliberately troublesome ecosystem:

- **[FSM_API](https://github.com/TrentBest/FSM_API)** — behavior and state.
- **[FSM_COS](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS)** — composition and runtime assembly.
- **[FSM_Serialization](https://github.com/TrentBest/TheSingularityWorkshop.FSM_Serialization)** — representation and the byte boundary.
- **[WebPage](https://github.com/TrentBest/WebPage)** — browser manifestation and proving ground.
- **[FSM_API_Unity](https://github.com/TrentBest/FSM_API_Unity)** — Unity manifestation.

<p align="center"><em>The Singularity Workshop — Tools for the curious, the bold, and the systemically inclined.</em><br><strong>Because state shouldn't be a mess.</strong><br><em>And because static boundaries are invitations to cause trouble.</em></p>

---

**The Singularity Workshop**  
[GitHub](https://github.com/TrentBest) · [Coder Legion](https://coderlegion.com/) · [Patreon](https://www.patreon.com/c/TheSingularityWorkshop) · [PayPal](https://www.paypal.com/donate/?hosted_button_id=3Z7263LCQMV9J)  
[ FSM_API ](https://github.com/TrentBest/FSM_API) · [ FSM_Serialization ](https://github.com/TrentBest/TheSingularityWorkshop.FSM_Serialization) · [ WebPage ](https://github.com/TrentBest/WebPage) · [ FSM_COS Documentation Standard ](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/DOCUMENTATION_STANDARD.md)  
**Because state shouldn't be a mess.**  
*And because static boundaries are invitations to cause trouble.*



---

## docs/BRANCH_RECONCILIATION.md
Source blob: `83b0e7ef8203e6f0e345dd8dc6a62c02812efd1a`

![The Singularity Workshop animated moniker](https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Renderer/development/docs/assets/moniker.svg)

# Live Branch Reconciliation Ledger

> **Snapshot:** 2026-10-20 18:23 UTC. Live refs and comparisons refreshed. This is an evidence ledger, not authorization to merge, publish, or delete branches.

## Sith policy: two durable branches

The intended durable branch set is exactly `master` and `development`.

- **`master`** is the release line; leave it untouched unless a separate, explicit release decision authorizes a reviewable transition.
- **`development`** is the integration line for ordinary work.
- Temporary branches are staging areas. Reconcile their useful changes into `development`, then classify unique content against the exact integrated tree.
- Never force either durable branch to match the other merely to simplify branch count.
- Do not delete branches, close useful PRs, update `master`, publish a package, or deploy without the applicable authorization.

## Live FSM_COS refs

| Ref | Observed state | Disposition |
|---|---|---|
| `master` | Existing release line; head not re-fetched in this snapshot | Preserve |
| `development` | `577b39ca286ac2bed401f01eb6c71949f922f6af` | Preserve as integration base |
| `work/alpha7-living-docs-integration` | `9e5ac133fb56af0e6ff6c4f587a3e7618018ad57` | Keep; draft PR [#34](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/pull/34) |

PR #34 is open, draft, and unmerged. GitHub compare reports candidate `work/alpha7-living-docs-integration` **80 commits ahead, 0 behind** `development` at the snapshot. The candidate Pack and publish workflow [#790](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/actions/runs/39472732154) and CodeQL [#94](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/actions/runs/39472732326) succeeded on candidate `9e5ac133...`; the publish job was skipped. These checks are candidate evidence, not evidence for a later integrated SHA.

## Temporary-branch unique-content audit

GitHub compare was refreshed against both `development` and the current PR #34 candidate. The file lists below are the paths changed by each branch's branch-unique commit range; counts are ancestry counts, not semantic proof of equivalence.

| Branch | Unique commits vs development | Behind development | Branch-unique paths | Current finding |
|---|---:|---:|---|---|
| `docs/alpha7-release-readiness` | 4 | 277 | `DOCUMENTATION_INDEX.md`, `README.md`, `docs/0.1.0-alpha.7-READINESS.md` | Candidate has a richer readiness section and links the completion roadmap. Final text comparison still required before discardable. |
| `docs/fsm-cos-visual-contract-audit` | 5 | 4 | `docs/LIVING_DOCUMENTATION_AUDIT.md`, `docs/assets/fsm-cos-overview.svg` | Candidate contains the overview correction and updated audit. Final text/asset equivalence check still required. |
| `work/ci-development-dependencies` | 2 | 307 | `.github/workflows/package.yml`, `docs/DEVELOPMENT.md` | Candidate workflow supersedes the older workflow's generic checkout and incorrect publish gate; final behavior check still required. |
| `work/fsm-cos-living-svg-flow` | 8 | 4 | `DOCUMENTATION_INDEX.md`, `NuGet.README.md`, `docs/LIVING_SVG.md`, `docs/assets/fsm-cos-preflight.svg`, `samples/DocumentationVisuals/Program.cs` | Candidate includes the living-SVG guide, scene, and generator/output work; final content and deterministic parity check still required. |
| `work/fsm-cos-uml-class-diagrams` | 11 | 4 | `docs/ARBITRATION.md`, `docs/ARCHITECTURE.md`, `docs/RUNTIME_ASSEMBLY.md`, `docs/RUNTIME_MANIFEST.md`, `docs/STAGED_MANIFEST_LOADING.md` | Five manually authored Mermaid diagrams are represented in candidate docs; they are not GUI.SVG-generated UML. Final text comparison still required. |
| `work/alpha7-living-docs-integration` | 80 | 0 | Active candidate, 24 changed files in PR #34 | Retain pending an authorized integration decision and post-integration verification. |

**No temporary branch is declared safe to delete yet.** The five branch-unique path sets are narrow and the candidate appears to supersede their purpose, but deletion readiness requires comparing exact file contents and assets against the final integrated `development` SHA, not only these current candidate comparisons.

## GUI.SVG source-derived UML dependency

GUI repo PR [#18](https://github.com/TrentBest/TheSingularityWorkshop.GUI/pull/18), branch `work/gui-svg-source-uml-generator`, remains draft. Latest head observed: `9cadb06a648082879f9b1dd278213a7e2346b7d6`; comparison to GUI `development` reports 56 commits ahead and 11 behind, merge base `8699c26fc821f89ba4bee340e955a564ce2323e7`.

- Source-derived diagrams workflow [#45](https://github.com/TrentBest/TheSingularityWorkshop.GUI/actions/runs/39473512921): passed.
- Package GUI workflow [#689](https://github.com/TrentBest/TheSingularityWorkshop.GUI/actions/runs/39473512854): passed; publication was skipped.

This verifies CI at the candidate head; it does **not** close the semantic UML contract. Compiler/project semantic resolution, canonical relationship emission and connectors, complete multi-type inventory, deterministic regeneration for the consumer contract, and hosted visual/accessibility checks remain open. FSM_COS correctly labels the interim Mermaid diagrams as manually authored rather than GUI-generated. Keep the dependency boundary honest and do not treat PR #18 as merged or published.

## Master/development release-line warning

The latest compare API result reports `development` as 628 commits ahead of `master`, and `master` as 44 commits ahead of `development`, with merge base `e9c9968b9cdbec0478e77d9f89b4e74a0e404d3d`. This is a substantial divergence. The compare includes release-era source, workflows, package metadata, README and documentation paths. Do not replace or force-update either branch as a branch-count cleanup shortcut. Inventory master-only changes and approve a release-transition strategy separately.

## Required sequence before branch-owner cleanup

1. Decide and authorize how PR #34 is to integrate into `development`; no merge is implied by this ledger.
2. After integration, capture the exact integrated SHA and rerun required CI, CodeQL/SARIF review, SVG regeneration/parity, package verification, and exact package inspection against that SHA. Do not publish unless separately authorized.
3. Complete final content/asset comparison for each of the five older branches against that exact tree; classify each as incorporated or discardable with evidence.
4. Independently resolve the `master`/`development` release-line divergence. Do not use branch deletion as a substitute.
5. Report the exact temporary branch names as safe for the owner to delete only after these gates are met.

Green CI is not permission to merge or publish. This ledger does not authorize branch deletion.

---

**The Singularity Workshop**  
[GitHub](https://github.com/TrentBest) · [Coder Legion](https://coderlegion.com/) · [Patreon](https://www.patreon.com/c/TheSingularityWorkshop) · [PayPal](https://www.paypal.com/donate/?hosted_button_id=3Z7263LCQMV9J)  
[ FSM_API ](https://github.com/TrentBest/FSM_API) · [ FSM_Serialization ](https://github.com/TrentBest/TheSingularityWorkshop.FSM_Serialization) · [ WebPage ](https://github.com/TrentBest/WebPage) · [ FSM_COS Documentation Standard ](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/DOCUMENTATION_STANDARD.md)  
**Because state shouldn't be a mess.**  
*And because static boundaries are invitations to cause trouble.*

