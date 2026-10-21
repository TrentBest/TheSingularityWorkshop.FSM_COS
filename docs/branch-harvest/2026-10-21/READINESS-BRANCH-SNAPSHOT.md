# Preserved readiness branch snapshots — docs/alpha7-release-readiness

Verbatim branch snapshots from 2026-10-21. Compare against canonical development before using; readiness status can become stale as soon as commits change.


---

## docs/0.1.0-alpha.7-READINESS.md
Source blob: `83074467355d1823808ad65f872b5875d53a25e4`

# FSM_COS 0.1.0-alpha.7 — Release Readiness and Remaining Work

**Candidate source:** `development`  
**Candidate version declared by the project:** `0.1.0-alpha.7`  
**Publication status:** not established by this document. Check NuGet and the exact GitHub Actions run independently.  
**Purpose:** one release gate for deciding whether the alpha.7 composition-kernel contract is coherent, tested, correctly packaged, and safe to publish.

> This is a checklist, not a claim that every gate has already passed. A result is valid only for the exact commit it tested.

## Release rule

Alpha.7 is a contract-hardening release. Its defining change is that public collections representing a validated plan or assembled order are read-only snapshots rather than mutable backing lists or arrays. Do not use this release to imply that MicroBundle instances themselves are immutable or that deferred composition features exist.

- Keep the package platform-neutral and focused on composition.
- Preserve the boundary: MicroBundleDomain defines bundles; a supplied catalog/resolver discovers them; FSM_COS plans, loads, arbitrates, and hands off; the host owns application behavior and lifecycle.
- Do not update dependency versions merely because newer source exists. Update the owning dependency first if a real contract change requires it.
- Do not publish until every P0 gate is satisfied and the owner explicitly authorizes publication.

## Priority legend

- **P0 — release blocker:** required before alpha.7 can ship.
- **P1 — readiness decision:** resolve or explicitly record why it is deferred before release review.
- **P2 — follow-up:** worthwhile future work, not an implied alpha.7 promise.

## 0. Establish the exact candidate

### P0

- [ ] Record the exact candidate commit SHA and link the build/test workflow run for that SHA.
- [ ] Confirm the release candidate is based on `development`, with no unreviewed source or dependency changes omitted from the candidate.
- [ ] Confirm the project version, release notes, README, NuGet README, documentation index, workflow/package checks, and intended package version all agree on `0.1.0-alpha.7`.
- [ ] Confirm the NuGet package page independently. Source metadata and a successful pack do not prove publication.
- [ ] Keep publication as a separate explicit owner decision; this document does not authorize it.

## 1. Confirm the alpha.7 contract

### P0

- [ ] Verify `RuntimeAssembly.Bundles` exposes a read-only snapshot of the composed bundle order.
- [ ] Verify `RuntimeManifestSchedule` exposes read-only snapshots for entries, dependency edges, independent entries, and dependent entries.
- [ ] Verify `RuntimeManifestLoadPlan.Entries` exposes a read-only snapshot.
- [ ] Keep regression tests that attempt mutation through `IList<T>` casts and prove mutation is rejected.
- [ ] Verify the snapshot boundary is described accurately: collection structure is protected, but the MicroBundle instances inside a collection remain domain-owned objects and may have mutable behavior.
- [ ] Ensure dependency planning, dependency ordering, shared-dependency deduplication, cycle detection, loading, configuration delivery, and bounded arbitration remain consistent with their documented contracts.
- [ ] Review failure paths against the API and tests; do not claim a successful stable handoff after resolution, planning, loading, or arbitration failure.

## 2. Dependency and ownership gate

### P0

The source candidate currently declares these direct package dependencies:

| Package | Declared version | Gate |
|---|---:|---|
| `TheSingularityWorkshop.FSM_API` | `1.0.13` | Keep unless a demonstrated FSM_COS requirement needs a dependency update |
| `TheSingularityWorkshop.FSM_UserIO` | `0.1.0-alpha.1` | Verify published availability and compatibility; keep alpha status explicit |
| `TheSingularityWorkshop.MicroBundleDomain` | `1.0.1` | Owns the MicroBundle contract; update it first if that contract must change |

- [ ] Confirm all declared package versions are available and restore from the public NuGet feed.
- [ ] Pass the public-NuGet compatibility job without silently resolving a local source package.
- [ ] Pass the development-source integration job using the intended sibling repositories and branches.
- [ ] Verify package-source mapping selects the intended MicroBundleDomain artifact in development mode.
- [ ] If a dependency contract must change, implement, test, package, and publish the owning dependency before updating FSM_COS to depend on its new public version.
- [ ] Confirm FSM_COS has not acquired GUI, transport, repository-storage, serialization, or application-loop responsibilities.

## 3. Prove the consumer path

### P0

- [ ] Run `dotnet run --project samples/FSM_COS.MinimalConsumer/FSM_COS.MinimalConsumer.csproj` against the candidate source.
- [ ] Confirm the consumer prints `Assembly order: 2 -> 1`, demonstrating that the dependency loads before the dependent bundle.
- [ ] Confirm the sample's catalog and MicroBundles are identified as sample-owned code, not public FSM_COS APIs.
- [ ] Confirm the sample passes in the repository's development-source integration path and that the package can independently restore/build from the public feed.
- [ ] Ensure the README describes the sample as source-level evidence, not proof of a published package.

## 4. CI, package contents, and documentation

### P0

- [ ] Pass restore, build, tests, coverage collection/upload, and package creation on the exact candidate commit.
- [ ] Pass the public-package compatibility and development-source integration jobs on that same commit.
- [ ] Pass both package-content verification paths where configured.
- [ ] Inspect the actual `.nupkg`: package ID/version, dependency metadata, target assembly, XML documentation, README, license, release notes, and required living SVG assets.
- [ ] Confirm the package verifier does not accidentally treat an uploaded artifact as a published release.
- [ ] Verify public XML documentation and focused docs describe implemented behavior, not planned behavior.
- [ ] Verify the living diagrams distinguish current kernel responsibility, deferred work, and host/neighbor ownership.
- [ ] Record any remaining documentation mismatch as a blocker, not as an implicit promise for a future release.

## 5. Explicitly deferred — not alpha.7 functionality

These omissions must remain visible and must not be described as implemented:

- Repository localization and content-addressed artifact verification.
- Bootstrap-only initial composition.
- Background/asynchronous localization and its cancellation/error contract.
- Live promotion of Deferred bundles into an already-running `RuntimeAssembly`.
- Automatic application execution, UI/presentation, persistence, transport, and host lifecycle management.
- Immutability of the MicroBundle objects themselves.

A deferred capability moves into scope only after its contract, owner, failure behavior, compatibility/evolution rules, and minimum test evidence are defined.

## 6. Follow-up readiness work

### P1 — resolve or explicitly defer

- [ ] Ensure every public runtime type and meaningful failure mode maps to source, focused documentation, and relevant tests.
- [ ] Review tests for resolution misses, missing dependencies, cycles, load failure, arbitration non-convergence, and configuration absence/failure where these behaviors are part of the contract.
- [ ] Document deterministic behavior for duplicate requests, version conflicts, dependency/version conflicts, and ordering.
- [ ] Record what diagnostics a host can inspect without turning FSM_COS into a logging framework.
- [ ] Confirm package docs include the same caveats as source docs, including the mutable-object distinction.
- [ ] Review the current dependency set and decide explicitly whether any dependency release is required; do not bump packages speculatively.

### P2 — later work, not a release promise

- Broader repository-backed consumer scenarios.
- Additional staged-loading capabilities after their host/repository boundaries are proven.
- Further conformance and living-documentation automation where it adds executable evidence rather than decorative complexity.

## 7. Publication gate

- [ ] Verify the exact commit SHA intended for release.
- [ ] Verify all P0 gates and any P1 release decisions.
- [ ] Inspect the exact generated package and its dependency metadata.
- [ ] Obtain explicit owner approval for publication.
- [ ] Deliberately dispatch `.github/workflows/package.yml` from `development` with the required `publish` input set to `true`; do not treat ordinary push/PR CI as release authorization.
- [ ] Verify the publish job itself succeeded and the exact version is visible on NuGet.
- [ ] Record the published version, source commit, workflow run, and package URL.
- [ ] Confirm no follow-up workflow or branch change leaves publication unintentionally enabled.

A passing build, passing tests, green package verifier, or uploaded `.nupkg` artifact is not by itself evidence that publication occurred.

## Exit condition

Alpha.7 is ready only when the exact candidate has a verified composition contract, green consumer and dependency paths, inspected package contents, aligned documentation, and explicit release approval. If a gate remains open, preserve it as open; do not turn a known limitation into a claim.

## Source of truth

- [Domain completion roadmap](DOMAIN_COMPLETION_ROADMAP.md)
- [Alpha.7 release notes](releases/0.1.0-alpha.7.md)
- [Architecture](ARCHITECTURE.md)
- [Consuming MicroBundles](CONSUMING_MICROBUNDLES.md)
- [Runtime Boundary](RUNTIME_BOUNDARY.md)
- [Development and CI dependency modes](DEVELOPMENT.md)
- [NuGet Publication Runbook](RELEASING.md)



---

## README.md
Source blob: `9f6c9f4d9324b5e43c771c8590675de2b18f176e`

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
- **Preparing the alpha.7 release?** [Release readiness and remaining work](docs/0.1.0-alpha.7-READINESS.md) tracks release blockers, dependency and consumer evidence, package verification, and the explicit publication gate.
- **What is in the domain, and what remains?** [Domain Completion and Release Roadmap](docs/DOMAIN_COMPLETION_ROADMAP.md) separates implemented behavior from deferred capabilities and defines evidence required to close them.
- **Preparing a release?** [Domain Completion and Release Roadmap](docs/DOMAIN_COMPLETION_ROADMAP.md) records the dependency gate, verification evidence, and release checklist.
- **Building or contributing?** [Development](docs/DEVELOPMENT.md) covers repository setup, tests, and workflows.
- **Looking for another topic?** Open the [Documentation Index](DOCUMENTATION_INDEX.md).

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

## DOCUMENTATION_INDEX.md
Source blob: `3eae1f18be6b5d8eed76188c2a1b80163babf1e6`

# FSM_COS Documentation Index

Use this page to find the right level of explanation. The README is the orientation page; the documents below provide focused authority for specific topics.

## Choose a path

| If you want to... | Start here |
|---|---|
| Understand the purpose and boundary of FSM_COS | [README](README.md) |
| Start with no programming experience | [What Is FSM_COS?](docs/WHAT_IS_FSM_COS.md) |
| Understand how Workshop packages fit together, including package-versus-MicroBundle decisions and Profiles (source-repository working map) | [Ecosystem Integration Map](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/docs/ECOSYSTEM_INTEGRATION_MAP.md) |
| Understand the architecture and component ownership | [Architecture](docs/ARCHITECTURE.md) |
| Learn the conceptual model and rationale | [FSM_COS Theory](docs/THEORY.md) |
| Understand the request that enters the kernel | [Runtime Manifest](docs/RUNTIME_MANIFEST.md), then [Manifest Theory](docs/MANIFEST_THEORY.md) |
| Understand staged loading, dependency planning, and the current implementation boundary | [Staged Manifest Loading](docs/STAGED_MANIFEST_LOADING.md) |
| Understand the object handed to a host | [RuntimeAssembly](docs/RUNTIME_ASSEMBLY.md) |
| Understand the boundary between declared provider IDs and actual provider resolution | [Provider Resolution Contract](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleDomain/blob/development/docs/PROVIDER_RESOLUTION_CONTRACT.md) |
| Understand what stays outside the kernel | [Runtime Boundary](docs/RUNTIME_BOUNDARY.md) |
| Run the minimal consumer example | [Minimal Consumer sample](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/samples/FSM_COS.MinimalConsumer/Program.cs) |
| Learn how MicroBundles are resolved and consumed | [Consuming MicroBundles](docs/CONSUMING_MICROBUNDLES.md) |
| Understand MicroBundle arbitration and convergence | [Arbitration](docs/ARBITRATION.md) |
| Explore AI-oriented composition | [AI Composition](docs/AI_COMPOSITION.md) |
| Understand the composition domain FSM_COS owns | [Domain](docs/DOMAIN.md) |
| See implemented domain capabilities, explicit deferrals, dependency order, and release gates | [Domain Completion and Release Roadmap](docs/DOMAIN_COMPLETION_ROADMAP.md) |
| Integrate with WebPage | [WebPage Integration](docs/WEBPAGE_INTEGRATION.md) |
| Review alpha.7 scope and explicit limitations | [Release notes](docs/releases/0.1.0-alpha.7.md) |
| Prepare and verify the alpha.7 release candidate | [Alpha.7 release readiness and remaining work](docs/0.1.0-alpha.7-READINESS.md) |
| Contribute, build, and verify changes | [Development](docs/DEVELOPMENT.md) |

## Recommended reading order

For a first encounter:

1. [README](README.md) — identity, responsibility boundary, and quick orientation.
2. [What Is FSM_COS?](docs/WHAT_IS_FSM_COS.md) — a plain-language introduction for readers new to software composition.
3. Run the [Minimal Consumer sample](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/samples/FSM_COS.MinimalConsumer/Program.cs) to see dependency ordering.
4. [Architecture](docs/ARCHITECTURE.md) — how the pieces fit together and which layer owns each concern.
5. [Ecosystem Integration Map](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/docs/ECOSYSTEM_INTEGRATION_MAP.md) — cross-repository working snapshot of package responsibilities, integration gaps, and MicroBundle candidates.
6. [FSM_COS Theory](docs/THEORY.md) — why composition is a distinct operation.
7. [Runtime Manifest](docs/RUNTIME_MANIFEST.md) — what the caller requests.
8. [RuntimeAssembly](docs/RUNTIME_ASSEMBLY.md) — what the host receives.
9. [Runtime Boundary](docs/RUNTIME_BOUNDARY.md) — where FSM_COS hands off responsibility.
10. The relevant integration or development guide for the task at hand.

Experienced readers can jump directly to the contract or integration guide they need.

## Documentation authority

- The README is the public front door, not the exhaustive specification.
- Architecture and theory documents explain ownership and rationale.
- Focused contract guides describe the manifest, assembly, dependency, loading, and arbitration boundaries.
- Source code and tests determine the behavior of the current implementation. If prose and implementation disagree, record and resolve the discrepancy rather than silently assuming the prose is correct.
- The repository-wide documentation rules live in [Documentation Standard](DOCUMENTATION_STANDARD.md).

## Status discipline

FSM_COS is evolving. Read any version or status statement in context: a design target is not automatically implemented, and code present on a development branch is not automatically part of a published NuGet release.

---

<p align="center"><em>The Singularity Workshop — Tools for the curious, the bold, and the systemically inclined.</em><br><strong>Because state shouldn't be a mess.</strong><br><em>And because static boundaries are invitations to cause trouble.</em></p>

