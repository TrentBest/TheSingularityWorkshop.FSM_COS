# FSM_COS Architecture

> **Architecture answers how the composition kernel performs the theory.** For the deeper “why,” start with [FSM_COS Theory](THEORY.md).

FSM_COS is a small composition kernel. This document describes how its pieces cooperate while leaving neighboring domains to their own packages.

## Runtime flow

![FSM_COS runtime flow](assets/fsm-cos-overview.svg)

```text
RuntimeManifest
      │
      ▼
   FsmCos.Execute
      │
      ├── resolve requested MicroBundle
      ├── recursively resolve dependencies
      ├── propagate opaque configuration
      ├── Load()
      ├── Arbitrate() until stable
      ▼
RuntimeAssembly
```

## Core contracts

### RuntimeManifest

Identifies the runtime being assembled and supplies root `MicroBundleDependencyRequest` values.

The manifest is composition input, not application behavior. See [Runtime Manifest](RUNTIME_MANIFEST.md) and [Runtime Manifest Theory](MANIFEST_THEORY.md).

### MicroBundleDependencyRequest

The root request is the domain-owned dependency request from [MicroBundleDomain](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleDomain).

FSM_COS does not create a second request type for its own convenience. It consumes the same request contract that MicroBundles use to declare dependencies.

This is important because the capability author and the composition host now speak the same request language.

### IMicroBundleCatalog

Resolves a bundle identity to the domain-owned `IMicroBundle` required for installation.

The catalog is supplied by the host, so a future Warehouse-backed, generated, cached, or in-memory resolver does not require a different composition algorithm.

### IMicroBundle

The canonical MicroBundle contract comes from MicroBundleDomain.

FSM_COS consumes:

- `Descriptor` for identity and domain metadata;
- `Dependencies` for dependency closure;
- `Load()` for installation;
- `Arbitrate()` for composition reconciliation.

FSM_COS does not redefine the MicroBundle domain model.

### IMicroBundleLoadContext

The domain-owned load context supplies the runtime-specific information a MicroBundle may need while it enters the composition.

FSM_COS implements/provides the context required by the domain contract while retaining ownership of the composition semantics.

### IMicroBundleArbitrationContext

The domain-owned arbitration context exposes the shared composition to participating bundles.

FSM_COS creates the context and drives the arbitration rounds. A MicroBundle decides what its own participation means.

### RuntimeAssembly

The result surface of composition: runtime identity, loaded MicroBundles, and arbitration result.

RuntimeAssembly is a handoff object, not a host application object.

See [RuntimeAssembly](RUNTIME_ASSEMBLY.md).

---

## Dependency resolution

FSM_COS performs depth-first dependency resolution.

```text
requested A
   │
   ├── B
   │   └── C
   └── D
```

The resulting installation order is:

```text
C → B → D → A
```

Two guards are fundamental:

1. already-loaded identities are not installed twice;
2. identities currently being resolved are tracked so dependency cycles can be rejected.

Missing bundles and dependency cycles are composition failures.

The manifest names roots. The graph determines the closure.

---

## Configuration propagation

Configuration belongs to the dependency request that caused installation.

FSM_COS carries the configuration through its load context without interpreting its domain meaning.

```text
request
  │
  ├── BundleId
  └── opaque configuration
          │
          ▼
       LoadContext
          │
          ▼
     owning bundle
```

If those bytes have a concrete representation, that representation belongs to the serialization boundary rather than the composition algorithm.

See [FSM_Serialization](https://github.com/TrentBest/TheSingularityWorkshop.FSM_Serialization).

---

## Arbitration

After reachable bundles are loaded, FSM_COS creates the domain-owned arbitration context and drives bounded rounds.

Conceptually:

```text
round
  │
  ├── bundle A → changed?
  ├── bundle B → changed?
  ├── bundle C → changed?
  │
  ├── no changes → CONVERGED
  └── changes → next round
```

The current default maximum is ten rounds.

A complete round with no changes is convergence.

If the maximum is reached without convergence, FSM_COS fails rather than returning an assembly it knows is unstable.

See [Arbitration and Convergence](ARBITRATION.md).

---

## Catalog boundary

The catalog stays outside the composition algorithm:

```text
FSM_COS ← catalog / resolver
              ▲
              │
       memory / cache /
       repository / Warehouse /
       generated registry
```

FSM_COS does not need to know where a capability came from.

This is the same boundary principle used throughout the ecosystem: **storage and delivery are not composition.**

---

## Host boundary

```text
FSM_COS
   │
   ▼
RuntimeAssembly
   │
   ├── WebPage → GUI → browser
   ├── AnyApp → local runtime
   ├── Desktop Forge → native manifestation
   └── another host / Experience
```

The host owns execution and manifestation.

FSM_COS owns the composition result.

---

## Current alpha boundary

The current `0.1.0-alpha.5` slice is:

```text
RuntimeManifest
    ↓
root requests
    ↓
dependency closure
    ↓
configured load
    ↓
bounded arbitration
    ↓
stable RuntimeAssembly
```

It deliberately stops before:

- host execution scheduling;
- GUI rendering;
- browser/desktop/native-host lifecycle;
- Warehouse implementation;
- networking;
- telemetry/metaDev adaptation;
- Experience presentation.

Those may consume the assembly later without becoming responsibilities of the kernel.

---

## Related documents

- [Dependency & Boundary Guide](DEPENDENCIES.md)
- [Runtime Manifest](RUNTIME_MANIFEST.md)
- [MicroBundles](MICROBUNDLES.md)
- [Arbitration and Convergence](ARBITRATION.md)
- [RuntimeAssembly](RUNTIME_ASSEMBLY.md)
- [Runtime Boundary](RUNTIME_BOUNDARY.md)
- [Theory](THEORY.md)
