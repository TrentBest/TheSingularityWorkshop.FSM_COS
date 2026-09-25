# The Singularity Workshop — FSM_COS

**FSM_COS is the composition system.**

It takes a runtime manifest and assembles the MicroBundles, dependencies, configuration, and runtime components required by that manifest.

    FSM_COS.Execute(manifest);

The same semantic manifest can be assembled by WebForge/WebPage, AnyApp, Desktop Forge, or another host.

## The assembly boundary

    Runtime Manifest
          │
          ▼
       FSM_COS
          │
          ├── resolve MicroBundles
          ├── resolve dependencies
          ├── propagate configuration
          ├── load/install bundles
          ├── arbitrate toward convergence
          ▼
     RuntimeAssembly

FSM_COS is the crane that assembles the machine. It does not become the machine.

## How it uses the surrounding architecture

| System | Relationship to FSM_COS |
|---|---|
| FSM_API | behavioral substrate used by assembled runtimes |
| FSM_Layer | higher-level composition of FSM primitives when present |
| MicroBundle | focused unit supplied to the composition |
| Warehouse | storage/delivery source that can back resolution |
| Experience | runtime environment represented or made available by the assembly |
| WebForge | browser manifestation of the assembled result |
| AnyApp | local host of an assembled runtime |
| MyVR / Domain | environment in which Experiences are encountered |

FSM_COS does not reimplement these systems. It connects the pieces required by a manifest.

## The manifest is the center

The RuntimeManifest is the published request for a runtime.

Authoring systems may know rich names, ontology, variants, dependencies, provenance, and visual/editor relationships. Those can be validated and baked into compact IDs and configuration before runtime.

FSM_COS consumes the resulting machine-oriented request.

See [Runtime Manifest Theory](docs/MANIFEST_THEORY.md).

## Composition

FSM_COS owns this sequence:

1. resolve requested bundles;
2. resolve their dependency closure;
3. propagate request configuration;
4. load each bundle after its dependencies;
5. arbitrate across the complete loaded set;
6. return RuntimeAssembly.

Configuration is opaque to FSM_COS. The bundle that owns a payload interprets it. This is the foundation for entangled dependency composition.

See [Architecture](docs/ARCHITECTURE.md) and [Arbitration](docs/ARBITRATION.md).

## Convergence

Loading establishes the initial composition. Arbitration lets the installed set reconcile itself.

    load → round → changed? → round → stable

The current maximum is ten rounds. If the composition does not converge, FSM_COS fails rather than returning an unstable assembly.

See [FSM_COS Theory](docs/THEORY.md).

## Host independence

    RuntimeManifest → FSM_COS → RuntimeAssembly
                              │
                 ┌────────────┼────────────┐
                 ▼            ▼            ▼
              WebForge      AnyApp     MyVR / Domain

Composition is not manifestation. A host decides how the assembled runtime is executed and encountered.

## Alpha 1 boundary

    manifest
      → bundle resolution
      → dependency resolution
      → configured loading
      → arbitration
      → RuntimeAssembly

Execution scheduling, Warehouse allocation, MetaDev adaptation, rendering, networking, and domain-specific behavior remain outside the core until their composition contracts require them.

FSM_COS should remain the assembly system, not become a general-purpose application framework.

## Documentation

- [FSM_COS Theory](docs/THEORY.md)
- [Architecture](docs/ARCHITECTURE.md)
- [Runtime Manifest Theory](docs/MANIFEST_THEORY.md)
- [Arbitration and Convergence](docs/ARBITRATION.md)

## Package

`TheSingularityWorkshop.FSM_COS` · .NET 8 · MIT