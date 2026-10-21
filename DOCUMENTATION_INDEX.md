# FSM_COS Documentation

This index links to the current product guides and development instructions.

## Get started

- [Project overview](README.md) — purpose, architecture at a glance, and a runnable consumer.
- [What Is FSM_COS?](docs/WHAT_IS_FSM_COS.md) — introduction to runtime composition.
- [Minimal consumer](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/samples/FSM_COS.MinimalConsumer/Program.md) — working example with a local catalog and two MicroBundles.
- [Gallery](docs/GALLERY.md) — diagrams of the composition and handoff flow.

## Use FSM_COS

- [Consuming MicroBundles](docs/CONSUMING_MICROBUNDLES.md) — catalog, manifest, configuration, dependency resolution, loading, and consumer errors.
- [Architecture](docs/ARCHITECTURE.md) — runtime flow and responsibility boundaries.
- [Domain](docs/DOMAIN.md) — kernel ownership and the MicroBundle contract boundary.
- [Runtime Manifest](docs/RUNTIME_MANIFEST.md) — request structure and version selection.
- [Staged Manifest Loading](docs/STAGED_MANIFEST_LOADING.md) — load-plan and schedule metadata.
- [RuntimeAssembly](docs/RUNTIME_ASSEMBLY.md) — successful composition result and host handoff.
- [Arbitration](docs/ARBITRATION.md) — bounded reconciliation and convergence behavior.
- [Runtime Boundary](docs/RUNTIME_BOUNDARY.md) — responsibilities owned by the host and adjacent packages.
- [WebPage Integration](docs/WEBPAGE_INTEGRATION.md) — browser-host integration.

## Reference

- [Alpha.7 release notes](docs/releases/0.1.0-alpha.7.md) — version-specific API and behavior changes.
- [Security](SECURITY.md) — threat model, resource limits, trust assumptions, and security verification.
- [Development](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/docs/DEVELOPMENT.md) — build, test, and CI instructions.
- [Public API UML](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/docs/UML/README.md) — source-to-type diagrams and class cards.

## Repository verification

The implementation and tests define runtime behavior. The focused contract guides explain those contracts; examples demonstrate usage. Release evidence is valid only for the exact commit and package artifact that were tested.
