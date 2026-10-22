<!-- The Singularity Workshop moniker -->
<p align="center">
  <a href="https://github.com/TrentBest">
    <img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop — animated workshop moniker" width="720" />
  </a>
</p>

# FSM_COS Documentation

FSM_COS is the Compute Operating System layer that composes requested MicroBundles into a `RuntimeAssembly` for a host. Use this index to move from the product idea to a working consumer, then into the contracts you need.

## I want to understand the product

- [README](README.md) — the product definition, why to use it, its place in the Workshop ecosystem, and a runnable example.
- [What Is FSM_COS?](docs/WHAT_IS_FSM_COS.md) — the problem it solves, the composition model, current behavior, and limits.
- [Learn by Following One Request](docs/LEARNING_GUIDE.md) — a picture-led walkthrough of dependency resolution, load order, arbitration, and handoff.
- [Gallery](docs/GALLERY.md) — visual overview with links to implementation-level explanations.

## I want to build a consumer

- [Minimal consumer recipe](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/samples/FSM_COS.MinimalConsumer/Program.md) — runnable sample, source notes, and expected output.
- [Consuming MicroBundles](docs/CONSUMING_MICROBUNDLES.md) — implement a catalog, provide a manifest and optional configuration, and understand failure behavior.
- [Runtime Manifest](docs/RUNTIME_MANIFEST.md) — the composition request and version selection.
- [RuntimeAssembly](docs/RUNTIME_ASSEMBLY.md) — the successful composition result and host handoff.
- [Arbitration](docs/ARBITRATION.md) — bounded reconciliation and convergence behavior.

## I want to understand the implementation

- [Architecture](docs/ARCHITECTURE.md) — principal contracts, runtime flow, and responsibility boundaries.
- [Domain](docs/DOMAIN.md) — FSM_COS ownership versus the external MicroBundleDomain contract.
- [Runtime Boundary](docs/RUNTIME_BOUNDARY.md) — what the kernel owns and what remains with the host.
- [Staged Manifest Loading](docs/STAGED_MANIFEST_LOADING.md) — current planning metadata and explicit limits; this is not a complete deferred-loading implementation.
- [FSM_COS Theory](docs/THEORY.md) — the deeper architectural rationale.
- [Runtime Manifest Theory](docs/MANIFEST_THEORY.md) — why the published request differs from the richer authoring model.
- [WebPage Integration](docs/WEBPAGE_INTEGRATION.md) — browser-host integration boundary and current responsibilities.

## I want to inspect the visuals and types

- [Gallery](docs/GALLERY.md) — source-backed diagrams and visual walkthroughs.
- [UML index](docs/UML/README.md) — public API and source-derived type diagrams.
- [Living UML visual style](docs/UML_VISUAL_STYLE.md) — diagram conventions, data-card layout, and accessibility guidance.

## I want release and security information

- [Alpha.7 release notes](docs/releases/0.1.0-alpha.7.md) — version-specific behavior and API notes.
- [Security](SECURITY.md) — threat model, trust assumptions, resource limits, and verification guidance.
- [Branch reconciliation ledger](docs/BRANCH_RECONCILIATION.md) — development-branch governance and preservation notes (maintainer-facing).

---

## Resources & support

- **FSM_COS:** [Source](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS) · [NuGet](https://www.nuget.org/packages/TheSingularityWorkshop.FSM_COS)
- **FSM_API:** [Source](https://github.com/TrentBest/FSM_API)
- **MicroBundleDomain:** [Source](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleDomain)
- **FSM_Serialization:** [Source](https://github.com/TrentBest/TheSingularityWorkshop.FSM_Serialization)
- **WebPage:** [Source](https://github.com/TrentBest/WebPage)
- **Workshop community:** [GitHub profile](https://github.com/TrentBest) · [Coder Legion](https://coderlegion.com/)
- **Support:** [Patreon](https://www.patreon.com/c/TheSingularityWorkshop) · [PayPal](https://www.paypal.com/donate/?hosted_button_id=3Z7263LCQMV9J)

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
