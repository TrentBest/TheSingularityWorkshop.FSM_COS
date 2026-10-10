# FSM_COS — Runtime Composition Kernel

![FSM_COS composition flow: a runtime manifest enters FSM_COS, which resolves and composes MicroBundles into a RuntimeAssembly for a host.](https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.FSM_COS/2fde7770e3536716bc2a110fcd1326af4c80a5f1/docs/assets/fsm-cos-overview.svg)

**FSM_COS composes requested capabilities into a `RuntimeAssembly`. It is a composition boundary—not an application, GUI, engine, repository, or execution loop.**

A host supplies a runtime manifest and a catalog capable of resolving the requested MicroBundles. FSM_COS resolves dependency closure, loads the composition, carries configuration through the relevant contracts, and arbitrates toward a stable result. The host decides how to execute or present that result.

## What it owns

- Resolving the MicroBundles requested by a `RuntimeManifest`.
- Resolving dependency closure and installation order.
- Loading the selected composition and running bounded arbitration.
- Returning the resulting `RuntimeAssembly` to the host.

FSM_COS does not own MicroBundle domain definitions, artifact storage, serialization, transport, GUI rendering, host lifecycle, or application scheduling.

## Install

```bash
dotnet add package TheSingularityWorkshop.FSM_COS --version 0.1.0-alpha.6
```

The package targets **.NET 8**. Its direct dependencies are:

- [FSM_API 1.0.13](https://www.nuget.org/packages/TheSingularityWorkshop.FSM_API) — state/context primitives.
- [MicroBundleDomain 1.0.1](https://www.nuget.org/packages/TheSingularityWorkshop.MicroBundleDomain) — the canonical MicroBundle contract.
- [FSM_UserIO 0.1.0-alpha.1](https://www.nuget.org/packages/TheSingularityWorkshop.FSM_UserIO) — the platform-neutral `SemanticIntent` boundary.

## Learn more

- [Project README and architecture overview](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/README.md)
- [Documentation index](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/DOCUMENTATION_INDEX.md)
- [Documentation standard](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/DOCUMENTATION_STANDARD.md)
- [Consuming MicroBundles](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/docs/CONSUMING_MICROBUNDLES.md)
- [Runtime Manifest](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/docs/RUNTIME_MANIFEST.md)
- [RuntimeAssembly](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/docs/RUNTIME_ASSEMBLY.md)
- [Development and verification](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/docs/DEVELOPMENT.md)
- [Source code, tests, and issues](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS)

The README image uses an absolute URL on GitHub's raw-content domain because NuGet.org does not render relative local image paths in package READMEs. The image is also included in the package's documentation assets for offline/reference use.
