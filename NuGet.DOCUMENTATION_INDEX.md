<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# FSM_COS Documentation Index

This index is designed for the **installed NuGet package**. It links to documentation shipped with the package; where a deeper source-repository resource is not packaged, it links to the corresponding repository page instead.

**Product contract:** FSM_COS accepts a `RuntimeManifest`, resolves requested MicroBundles through a host-supplied catalog, validates dependency closure, loads prerequisites before dependents, performs bounded arbitration, and returns a `RuntimeAssembly` to the host.

## 01 — What is FSM_COS?

- [What Is FSM_COS?](docs/WHAT_IS_FSM_COS.md) — the product in plain language, use cases, and explicit non-goals.
- [Architecture at a Glance](docs/ARCHITECTURE.md) — component responsibilities and data flow.
- [Domain Boundary](docs/DOMAIN.md) — what belongs to FSM_COS versus MicroBundleDomain.
- [Runtime Boundary](docs/RUNTIME_BOUNDARY.md) — the contract between composition and the host.
- [Workshop Repository Guide](docs/WORKSHOP_REPOSITORIES.md) — which neighboring package owns which concern.

## 02 — Why is it designed this way?

- [FSM_COS Theory](docs/THEORY.md) — the composition model and design rationale.
- [Runtime Manifest Theory](docs/MANIFEST_THEORY.md) — why the runtime request is narrower than a rich authoring model.
- [Security and Trust Boundaries](SECURITY.md) — trust assumptions, resource limits, and what in-process MicroBundles do not guarantee.
- [Composition Failures](docs/COMPOSITION_FAILURES.md) — phase-aware failures and the absence of a successful handoff.
- [Staged Loading: Intent and Limits](docs/STAGED_MANIFEST_LOADING.md) — what staged planning means and what it does not promise.

## 03 — How do I use it?

- [Consuming MicroBundles](docs/CONSUMING_MICROBUNDLES.md) — provide a catalog, request capabilities, supply configuration, and consume the result.
- [Learning Guide](docs/LEARNING_GUIDE.md) — follow a request through the composition pipeline.
- [Runtime Manifest](docs/RUNTIME_MANIFEST.md) — request roots, versions, and supported metadata.
- [RuntimeAssembly](docs/RUNTIME_ASSEMBLY.md) — what a successful composition returns.
- [Arbitration](docs/ARBITRATION.md) — bounded reconciliation, convergence, and failure.
- [WebPage Integration](docs/WEBPAGE_INTEGRATION.md) — host integration responsibilities.
- [Run the minimal consumer](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/samples/FSM_COS.MinimalConsumer/Program.md) — source, command, and expected output.
- [Browse source-repository UML and relationship maps](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/tree/development/docs/UML) — source-oriented type views and declared scan limits.

## 04 — See it in a minute

Run the minimal consumer from a source checkout:

```bash
git clone https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS.git
cd TheSingularityWorkshop.FSM_COS
dotnet run --project samples/FSM_COS.MinimalConsumer/FSM_COS.MinimalConsumer.csproj
```

The sample demonstrates dependency-first composition. It does **not** demonstrate artifact downloading, content-hash verification, deferred activation, browser rendering, or a production deployment pipeline. See the [minimal consumer recipe](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/samples/FSM_COS.MinimalConsumer/Program.md) for the source walkthrough.

## 05 — Choose your next step

- **New consumer:** start with [Consuming MicroBundles](docs/CONSUMING_MICROBUNDLES.md).
- **Need a precise behavior or failure contract:** use the references above.
- **Need a design rationale:** start with [Theory](docs/THEORY.md).
- **Need a visual explanation:** open the [Living Gallery](docs/GALLERY.md).
- **Evaluating a release candidate:** review the [published package page](https://www.nuget.org/packages/TheSingularityWorkshop.FSM_COS) and [source release notes](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/docs/releases/0.1.0-alpha.7.md). A source version or release note is not proof of publication.
- **Maintaining the source:** read the [repository documentation index](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/DOCUMENTATION_INDEX.md) and [documentation standard](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/DOCUMENTATION_STANDARD.md).

## Project links

- [FSM_COS source](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS) · [NuGet package page](https://www.nuget.org/packages/TheSingularityWorkshop.FSM_COS)
- [FSM_API](https://github.com/TrentBest/FSM_API) · [MicroBundleDomain](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleDomain) · [FSM_Serialization](https://github.com/TrentBest/TheSingularityWorkshop.FSM_Serialization)

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
