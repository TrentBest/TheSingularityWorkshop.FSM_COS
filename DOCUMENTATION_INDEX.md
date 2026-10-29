<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# FSM_COS Documentation

This is the map for learning, using, and verifying FSM_COS. Follow the five-part reader journey below; you do not need to read every reference before building a consumer.

**Product contract in one sentence:** FSM_COS accepts a `RuntimeManifest`, resolves requested MicroBundles through a host-supplied catalog, validates dependency closure, loads prerequisites before dependents, performs bounded arbitration, and returns a `RuntimeAssembly` to the host.

## 01 — What is it?

Start with the concept before learning the vocabulary.

- [What Is FSM_COS?](docs/WHAT_IS_FSM_COS.md) — the product in plain language, use cases, and explicit non-goals.
- [Architecture at a Glance](docs/ARCHITECTURE.md) — the major components and their responsibilities.
- [Domain Boundary](docs/DOMAIN.md) — what belongs to FSM_COS versus MicroBundleDomain.
- [Runtime Boundary](docs/RUNTIME_BOUNDARY.md) — the contract between composition and the host.
- [Workshop Repository Guide](docs/WORKSHOP_REPOSITORIES.md) — which neighboring package owns which concern.

## 02 — Why is it designed this way?

Read these when you need the reasoning behind the boundaries, rather than another description of the API.

- [FSM_COS Theory](docs/THEORY.md) — the composition model and its design rationale; use it as the starting point for the domain's textbook-style learning path.
- [Runtime Manifest Theory](docs/MANIFEST_THEORY.md) — why the published request is narrower than a rich authoring model.
- [Security and Trust Boundaries](SECURITY.md) — trust assumptions, resource limits, and what in-process MicroBundles do not guarantee.
- [Composition Failures](docs/COMPOSITION_FAILURES.md) — why failures are phase-aware and prevent a successful handoff.
- [Staged Loading: Intent and Limits](docs/STAGED_MANIFEST_LOADING.md) — what the planning model means and why it must not be confused with a live deployment engine.

## 03 — How do I use or change it?

Choose the task you are trying to complete. These are the practical guides and exact contract references.

**Build a consumer**
- [Minimal consumer: runnable recipe](samples/FSM_COS.MinimalConsumer/Program.md) — source, command, and expected output.
- [Consuming MicroBundles](docs/CONSUMING_MICROBUNDLES.md) — provide a catalog, request capabilities, supply configuration, and consume the result.
- [Learning Guide](docs/LEARNING_GUIDE.md) — follow a request through the composition pipeline.
- [WebPage Integration](docs/WEBPAGE_INTEGRATION.md) — integration responsibilities for a browser-hosted application.

**Look up a specific contract**
- [Runtime Manifest](docs/RUNTIME_MANIFEST.md) — request roots, versions, and supported metadata.
- [RuntimeAssembly](docs/RUNTIME_ASSEMBLY.md) — what a successful composition returns.
- [Arbitration](docs/ARBITRATION.md) — bounded reconciliation, convergence, and failure.
- [Composition Outcomes and Diagnostics](docs/COMPOSITION_FAILURES.md) — phase and error interpretation.
- [Staged Manifest Loading](docs/STAGED_MANIFEST_LOADING.md) — staged metadata, validation, state transitions, and explicit non-guarantees.

**Maintain the implementation**
- [Architecture](docs/ARCHITECTURE.md) — component boundaries and data flow.
- [Documentation standard](DOCUMENTATION_STANDARD.md) — shared Workshop requirements for textbook-style theory chapters, evidence, accuracy, visuals, and type documentation.

## 04 — See it in a minute

Prefer a concrete result before reading the theory.

- [Run the minimal consumer](samples/FSM_COS.MinimalConsumer/Program.md) — executable example with expected output.
- [Living Gallery](docs/GALLERY.md) — diagrams that teach the composition flow. Visuals are explanatory unless explicitly identified as captured runtime evidence.
- [Documentation Visuals sample](samples/DocumentationVisuals/Program.md) — how the repository's generated documentation visuals are produced.
- [UML and relationship maps](docs/UML/README.md) — source-oriented type views and their declared scan limits.

The minimal consumer demonstrates dependency-first composition. It does **not** demonstrate artifact downloading, content-hash verification, deferred activation, browser rendering, or a production deployment pipeline.

## 05 — Choose your next step

- **New consumer:** start with [Consuming MicroBundles](docs/CONSUMING_MICROBUNDLES.md).
- **Need a precise behavior or failure contract:** use the references in section 03.
- **Need to understand a design decision:** start with [Theory](docs/THEORY.md), then follow its links to the specific contract.
- **Need a visual explanation:** open the [Gallery](docs/GALLERY.md).
- **Changing the package:** read [Architecture](docs/ARCHITECTURE.md), inspect the relevant source and tests, and check the maintainer audit register before editing.
- **Evaluating a release candidate:** read [Alpha.7 release notes](docs/releases/0.1.0-alpha.7.md) and the current [release evidence checklist](.github/release-readiness/0.1.0-alpha.7.md). These are snapshots/checklists, not proof that the current HEAD has passed.
- **Maintaining UML:** start at the [UML index](docs/UML/README.md), then inspect the [coverage inventory](docs/UML/INVENTORY.md) and [reconciliation plan](docs/UML/RECONCILIATION_PLAN.md).

## Project links

- [FSM_COS source](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS) · [NuGet package page](https://www.nuget.org/packages/TheSingularityWorkshop.FSM_COS)
- [FSM_API](https://github.com/TrentBest/FSM_API) · [MicroBundleDomain](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleDomain) · [FSM_Serialization](https://github.com/TrentBest/TheSingularityWorkshop.FSM_Serialization) · [WebPage](https://github.com/TrentBest/WebPage)

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
