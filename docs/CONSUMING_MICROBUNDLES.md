# Consuming MicroBundles

> This document is about **using MicroBundles with FSM_COS**, not defining the MicroBundle domain. The authoritative MicroBundle contract and domain model live in MicroBundleDomain.

> **FSM_COS does not define the MicroBundle domain. It defines how a developer composes MicroBundles into a runtime.**

The authoritative MicroBundle contract is supplied by **TheSingularityWorkshop.MicroBundleDomain**.

This document is intentionally narrow. It explains what an FSM_COS consumer supplies, what FSM_COS does with it, and how a developer can choose the surrounding infrastructure.

## The boundary

```text
your MicroBundle source
        │
        │ IMicroBundle
        ▼
 IMicroBundleCatalog
        │
        ▼
     FSM_COS
        │
        ├── resolve manifest roots
        ├── discover dependency closure
        ├── obtain optional configuration
        ├── load bundles
        ├── arbitrate to convergence
        ▼
 RuntimeAssembly
        │
        ▼
      your host
```

The source can be an in-memory catalog, a repository adapter, local artifacts, generated resources, remote delivery, or another implementation you control.

FSM_COS does not require a particular storage or delivery mechanism.

## What FSM_COS consumes

FSM_COS consumes three intentionally small inputs.

### 1. Runtime Manifest

The manifest identifies the runtime and the root MicroBundles it requests.

Each root carries:

```csharp
new MicroBundleManifestEntry(
    BundleId: 10,
    Version: "1.2.0");
```

The manifest does **not** contain configuration bytes.

See [Runtime Manifest](RUNTIME_MANIFEST.md).

### 2. MicroBundle catalog

The catalog is your resolution boundary.

For manifest roots, FSM_COS asks:

```csharp
bool TryResolve(
    ulong bundleId,
    string version,
    out IMicroBundle? bundle);
```

For domain-declared dependencies, FSM_COS asks:

```csharp
bool TryResolve(
    ulong bundleId,
    out IMicroBundle? bundle);
```

This lets you decide how resolution works.

You can resolve from:

- a dictionary;
- a package cache;
- a MicroBundle repository;
- Azure-backed delivery;
- generated code;
- a remote service;
- a test fixture.

FSM_COS only needs the catalog contract.

### 3. Optional configuration source

Configuration is supplied separately:

```csharp
bool TryGetConfiguration(
    ulong runtimeId,
    ulong bundleId,
    string version,
    out ReadOnlyMemory<byte> configuration);
```

The implementation decides where those bytes come from.

It might be a file, blob, repository artifact, generated resource, or another application-owned source.

FSM_COS does not read the file or interpret its format.

If no configuration exists, the bundle receives no external configuration and uses its own defaults.

## Minimal composition

A developer can start with an entirely in-memory composition.

```csharp
var catalog = new InMemoryCatalog(
    guiBundle,
    inputBundle,
    physicsBundle);

var manifest = new RuntimeManifest(
    RuntimeId: 1001,
    Bundles:
    [
        new MicroBundleManifestEntry(guiBundle.Id, "1.0.0"),
        new MicroBundleManifestEntry(inputBundle.Id, "1.0.0")
    ]);

var cos = new FsmCos(catalog);

RuntimeAssembly assembly = cos.Execute(manifest);
```

No repository is required.

No configuration source is required.

No GUI framework is required.

No serialization framework is required.

The composition kernel is useful with only the contracts it actually consumes.

## Adding configuration

Configuration can be added without changing the manifest.

```csharp
var configuration = new FileBackedConfigurationSource(
    configurationDirectory);

RuntimeAssembly assembly = cos.Execute(
    manifest,
    configuration);
```

The important architectural property is that the configuration source is replaceable.

The same manifest can therefore be composed using:

```text
local files
     ↓
configuration source

Azure Blob
     ↓
configuration source

repository artifact
     ↓
configuration source

generated defaults
     ↓
configuration source
```

FSM_COS sees the same contract.

## Using a repository

A repository adapter can implement `IMicroBundleCatalog` and provide the artifacts requested by the manifest.

```text
RuntimeManifest
      │
      ▼
repository-backed catalog
      │
      ├── locate root @ requested version
      ├── locate dependency
      └── materialize IMicroBundle
      │
      ▼
    FSM_COS
```

This is where **TheSingularityWorkshop.MicroBundleRepository** can participate.

FSM_COS does not need to know whether the catalog came from that repository, another repository, or an application's own implementation.

## Supplying your own MicroBundle

FSM_COS does not prescribe the internal implementation of a MicroBundle. For the complete domain definition, use MicroBundleDomain documentation. Here we only show the portion an FSM_COS developer must provide to the composition boundary.

It consumes the domain-owned contract:

```csharp
public interface IMicroBundle
{
    MicroBundleDescriptor Descriptor { get; }
    ulong Id { get; }
    IReadOnlyList<MicroBundleDependencyRequest> Dependencies { get; }

    void Load(IMicroBundleLoadContext context);

    bool Arbitrate(
        IMicroBundleArbitrationContext context,
        int roundIndex);
}
```

The implementation remains yours.

FSM_COS gives that implementation a composition lifecycle:

```text
resolve
   ↓
dependencies
   ↓
load
   ↓
arbitration rounds
   ↓
stable assembly
```

For the complete domain contract, use the MicroBundleDomain package documentation rather than duplicating it here.

## Dependency consumption

FSM_COS consumes dependencies as relationships exposed by the domain-owned contract. It resolves those relationships, loads the reachable closure, deduplicates repeated identities, and rejects missing or cyclic composition. The semantics of what a dependency means inside a MicroBundle remain the MicroBundle domain's responsibility.

## Dependencies

A root MicroBundle can declare dependencies through the domain contract.

For example:

```text
Experience
├── GUI
│   └── Input
└── Physics
```

The manifest needs only the requested root.

FSM_COS walks the reachable dependency graph and loads dependencies before their dependents.

The resulting order is an implementation detail of composition traversal; the important guarantee is that a dependency is loaded before the bundle that requires it.

Repeated identities are deduplicated.

Missing identities fail composition.

Cycles fail composition.

FSM_COS does not silently invent a substitute.

## Version selection

Version is a manifest concern for root requests.

```text
Manifest
    │
    └── GUI @ 1.2.0
             │
             ▼
       catalog / resolver
             │
             ▼
       resolved artifact
             │
             ▼
          FSM_COS
```

FSM_COS verifies that the resolved root reports the requested version.

This prevents a catalog from silently returning a different version than the published composition request named.

Dependency version policy remains a concern of the domain/repository contract that supplies those dependency relationships. FSM_COS does not invent a second versioning system inside the composition kernel.

## Arbitration

After loading, FSM_COS gives every loaded MicroBundle an opportunity to participate in reconciliation.

```csharp
bool changed = bundle.Arbitrate(
    arbitrationContext,
    roundIndex);
```

A `true` result means the composition may have changed.

FSM_COS runs another round.

A complete round with no changes is stable.

The default maximum is **10 rounds**.

If the composition still reports changes after the maximum, FSM_COS fails rather than returning an assembly it knows is unstable.

See [Arbitration and Convergence](ARBITRATION.md).

## RuntimeAssembly

Successful composition produces:

```csharp
RuntimeAssembly assembly = cos.Execute(manifest);
```

The assembly exposes:

- the runtime identity;
- the loaded MicroBundles;
- the number of arbitration rounds.

A host can retrieve a bundle by identity:

```csharp
if (assembly.TryGetBundle(bundleId, out var bundle))
{
    // host-specific use
}
```

Or by a host-known type:

```csharp
if (assembly.TryGetBundle<MyBundle>(bundleId, out var bundle))
{
    // host-specific use
}
```

FSM_COS stops at the assembly boundary.

The host decides what the assembly means operationally.

## Different developers, same kernel

The same FSM_COS package can support very different environments.

```text
Developer A
    manifest
      ↓
in-memory catalog
      ↓
FSM_COS
      ↓
desktop host

Developer B
    manifest
      ↓
repository catalog
      ↓
FSM_COS
      ↓
WebPage / Blazor

Developer C
    manifest
      ↓
remote catalog
      ↓
FSM_COS
      ↓
distributed host
```

The composition algorithm does not need to change because the delivery mechanism changed.

That is the point of the boundary.

## What FSM_COS does not consume

FSM_COS deliberately does not require:

- a GUI framework;
- a browser;
- Unity;
- a specific repository;
- a specific serialization format;
- a specific configuration file format;
- an application scheduler;
- a particular host;
- a particular rendering engine.

Those systems may feed the composition boundary through contracts, but they do not become part of the composition kernel.

## Dependency consumption summary

FSM_COS currently consumes two foundational package domains:

| Package | FSM_COS consumes | FSM_COS does not define |
|---|---|---|
| **FSM_API** | state/context primitives, including optional runtime experience context | state-machine semantics beyond what composition requires |
| **MicroBundleDomain** | MicroBundle identity, version metadata, dependencies, load context, arbitration context, and runtime contract | the meaning, ontology, authoring model, or storage of MicroBundles |

This is the intended documentation boundary:

> **A dependency's repository explains the dependency. FSM_COS explains how FSM_COS consumes it.**

## Related documents

- [Architecture](ARCHITECTURE.md)
- [Runtime Manifest](RUNTIME_MANIFEST.md)
- [RuntimeAssembly](RUNTIME_ASSEMBLY.md)
- [Arbitration and Convergence](ARBITRATION.md)
- [Runtime Boundary](RUNTIME_BOUNDARY.md)
- [Development](DEVELOPMENT.md)
