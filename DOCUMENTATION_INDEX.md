<!-- The Singularity Workshop moniker -->
<p align="center">
  <a href="https://github.com/TrentBest">
    <img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop — animated workshop moniker" width="720" />
  </a>
</p>

# FSM_COS Documentation

FSM_COS is creational tooling: describe the capabilities you want, supply a catalog that can resolve them, and let the composition kernel assemble a dependency-ordered runtime for your host.

**You do not need to read every document to use the package.** Start with the working example, then open a reference only when your implementation needs it.

## Create something with FSM_COS

1. **[Run the minimal consumer](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/samples/FSM_COS.MinimalConsumer/Program.md)** — a complete small example, with expected output.
2. **[Build your own consumer](docs/CONSUMING_MICROBUNDLES.md)** — provide your own MicroBundle catalog, request the capabilities you want with a `RuntimeManifest`, supply optional configuration, and receive a `RuntimeAssembly`.
3. **[Change the composition request](docs/LEARNING_GUIDE.md#change-the-request)** — see how changing the requested root changes what FSM_COS assembles.
4. **[Browse the Gallery](docs/GALLERY.md)** — use the diagrams to understand the composition flow and find the recipe that fits your next step.

## Understand the product

- [What is FSM_COS?](docs/WHAT_IS_FSM_COS.md) — the problem it solves and the model in plain language.
- [Architecture at a glance](docs/ARCHITECTURE.md) — how request, catalog, dependency resolution, loading, arbitration, and host handoff fit together.
- [Runtime boundary](docs/RUNTIME_BOUNDARY.md) — what the package does and what your host remains responsible for.
- [Workshop repository guide](docs/WORKSHOP_REPOSITORIES.md) — find the right neighboring package when your system needs more than composition.

## Look up a contract when you need it

These are references, not prerequisites. Most consumers can begin with the recipe above and return here for a specific question.

- [Runtime Manifest](docs/RUNTIME_MANIFEST.md) — requested roots, versions, and optional composition metadata.
- [RuntimeAssembly](docs/RUNTIME_ASSEMBLY.md) — the result returned after successful composition.
- [Arbitration](docs/ARBITRATION.md) — convergence behavior and failure conditions.
- [Staged Manifest Loading](docs/STAGED_MANIFEST_LOADING.md) — what the current planning metadata means and, importantly, what it does not implement.
- [Security](SECURITY.md) — trust assumptions, resource limits, and why in-process MicroBundles are not sandboxed.
- [Alpha.7 release notes](docs/releases/0.1.0-alpha.7.md) — candidate contract and version-specific changes.

## Maintainer and deeper design references

Use these when changing FSM_COS itself, reviewing the design, or working on documentation—not as a checklist for package consumers.

- [Domain](docs/DOMAIN.md) — ownership boundary between FSM_COS and MicroBundleDomain.
- [FSM_COS Theory](docs/THEORY.md) — deeper composition rationale.
- [Runtime Manifest Theory](docs/MANIFEST_THEORY.md) — the boundary between a rich authoring model and the published request.
- [WebPage Integration](docs/WEBPAGE_INTEGRATION.md) — browser-host integration responsibilities.
- [UML index](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/docs/UML/README.md) — source-derived public type diagrams (maintainer reference).
- [UML coverage inventory](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/docs/UML/INVENTORY.md) — current manual coverage and remaining gaps.\n- [Comprehensive UML Standard](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/docs/UML/Standard/README.md) — per-view requirements, including MicroBundle inventory, complexity analysis, lifecycle, data lineage, deployment, and traceability.
- [Living UML visual style](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/docs/UML_VISUAL_STYLE.md) — diagram conventions (maintainer reference).
- [Documentation Standard](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/DOCUMENTATION_STANDARD.md) — shared documentation and evidence rules for Workshop repositories (maintainer reference).

---

## Project links

- **FSM_COS:** [Source](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS) · [NuGet](https://www.nuget.org/packages/TheSingularityWorkshop.FSM_COS)
- **FSM_API:** [Source](https://github.com/TrentBest/FSM_API)
- **MicroBundleDomain:** [Source](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleDomain)
- **FSM_Serialization:** [Source](https://github.com/TrentBest/TheSingularityWorkshop.FSM_Serialization)
- **WebPage:** [Source](https://github.com/TrentBest/WebPage)



## Diagnosing composition

- [Composition Outcomes and Failure Diagnostics](docs/COMPOSITION_FAILURES.md) — phase-aware results for hosts that need to report failures without parsing exception messages.

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
