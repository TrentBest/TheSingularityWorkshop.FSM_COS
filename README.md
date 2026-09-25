# The Singularity Workshop — FSM_COS

**FSM_COS is the composition system.**

It takes a runtime manifest and assembles the MicroBundles, FSM infrastructure, dependencies, configuration, and execution components required by that manifest.

The intended host surface is deliberately small:

```csharp
FSM_COS.Execute(manifest);
```

The same semantic manifest should be executable by WebForge/WebPage, AnyApp, Desktop Forge, or another host.

## Position in the stack

```text
Runtime Manifest
      │
      ▼
   FSM_COS                 composition / assembly
      │
      ├── MicroBundles
      ├── FSM_Layer
      └── runtime resources
             │
             ▼
          FSM_API
             │
             ▼
      assembled runtime
```

FSM_COS is **not** the Experience, renderer, application, web server, or state-machine implementation. It is the crane/factory that assembles what the manifest requests.

## The manifest is the center

The manifest is the artifact that says: **this is the runtime I need; assemble it.**

The first alpha keeps the representation understandable. As the Warehouse and runtime architecture mature, the manifest can become progressively more compact and baked.

The intended path is:

```text
editor intent
    ↓
published manifest
    ↓
FSM_COS.Execute(manifest)
    ↓
resolve → load → configure → arbitrate → assemble
    ↓
RuntimeAssembly
```

Human-readable names belong primarily to authoring/editor time. The runtime path is intended to converge on IDs and compact data.

## MicroBundles

MicroBundles are loaded **because the manifest requires them**.

Composition owns:

1. resolving requested MicroBundles;
2. resolving dependencies;
3. passing configuration into dependencies;
4. installing/loading bundles;
5. running bounded arbitration/convergence;
6. producing the assembled runtime.

This is where the **entangled dependency** concept belongs: a dependency is not merely a name. A parent can request a dependency together with configuration that must be present when that dependency loads.

## Host independence

```csharp
var assembly = cos.Execute(manifest);
```

WebForge/WebPage can manifest that assembly in a browser. AnyApp can host it locally. Desktop Forge can assemble the same semantic runtime natively. MyVR/Domain can provide the environment in which the resulting Experience is encountered.

The composition contract remains the same.

## Design boundary

| System | Responsibility |
|---|---|
| FSM_API | state machines and processing primitives |
| FSM_Layer | composition of FSM primitives into higher-level runtime behavior |
| FSM_COS | assemble the requested runtime from its manifest |
| MicroBundle | focused capability/content/behavior unit |
| Warehouse | storage and delivery of runtime data |
| AnyApp | local host of an assembled Experience |
| WebForge | browser/web manifestation and proving ground |
| MyVR / Domain | environment in which Experiences are encountered |

FSM_COS should remain focused on **assembly**. It should not become a general-purpose application framework.

## Alpha 1 vertical slice

```text
manifest
  → bundle resolution
  → dependency resolution
  → configured loading
  → arbitration
  → RuntimeAssembly
```

Execution scheduling, Warehouse allocation, MetaDev adaptation, rendering, networking, and domain-specific behavior stay outside the core until the composition contract requires them.

## Package

`TheSingularityWorkshop.FSM_COS`

Target framework: .NET 8.

License: MIT.
