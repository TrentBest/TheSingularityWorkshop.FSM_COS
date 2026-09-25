# FSM_COS Architecture

FSM_COS is a small composition kernel. This document describes how its pieces cooperate rather than redefining concepts owned by neighboring systems.

## Runtime flow

    RuntimeManifest
          │
          ▼
       FsmCos.Execute
          │
          ├── resolve requested bundle
          ├── recursively resolve dependencies
          ├── pass configuration
          ├── Load()
          ├── Arbitrate() until stable
          ▼
    RuntimeAssembly

## Core contracts

### RuntimeManifest
Identifies the runtime being assembled and supplies root BundleRequest values. It is composition input, not application behavior.

### BundleRequest
Pairs a ulong BundleId with opaque configuration bytes. A manifest producer can bake richer editor-time structures into this representation.

### IMicroBundleCatalog
Resolves a bundle identity to the IMicroBundle required for installation. The catalog is supplied by the host, so future Warehouse-backed or generated resolvers do not require a different composition algorithm.

### IMicroBundle
Provides exactly what composition needs: identity, dependencies, Load, and Arbitrate. It does not expose a GUI, web server, or host lifecycle contract.

### MicroBundleLoadContext
Carries runtime identity and configuration available during installation. Configuration remains opaque to FSM_COS.

### ArbitrationContext
Exposes runtime identity and the currently loaded bundle set during convergence. It is composition state, not host state.

### RuntimeAssembly
The result surface of the composition pass: runtime identity, loaded bundles, and arbitration count.

## Dependency resolution

FSM_COS performs depth-first dependency resolution.

    requested A
       │
       ├── B
       │   └── C
       └── D

The resulting installation order is C, B, D, A.

Two guards are fundamental: already-loaded IDs are skipped, and IDs currently loading are tracked to detect cycles.

Missing bundles and dependency cycles are composition failures.

## Configuration propagation

Configuration belongs to the BundleRequest that caused installation. The load context is populated before a requested bundle and its dependency chain are loaded.

This is the foundation for entangled dependency composition.

## Arbitration

After reachable bundles are loaded, FSM_COS creates one ArbitrationContext.

    for each round
        for each loaded bundle
            changed |= bundle.Arbitrate(context, round)
        if no change → converged

If the configured maximum is reached without convergence, FSM_COS throws instead of returning an unstable assembly. The current default is ten rounds.

## Catalog boundary

The catalog stays outside the composition algorithm:

    FSM_COS ← Catalog ← in-memory / generated registry / Warehouse / cache / Domain resolver

The composition engine should not need to know where a bundle came from.

## Host boundary

    FSM_COS
       │
       ▼
RuntimeAssembly
       │
       ├── WebForge → GUI → browser
       ├── AnyApp → local runtime
       ├── Desktop Forge → native manifestation
       └── MyVR / Domain → encountered Experience

## Current alpha boundary

Alpha 1 stops after manifest → resolve → dependency resolution → configured load → arbitration → RuntimeAssembly.

Execution scheduling, resource allocation, Warehouse integration, and host lifecycle belong to later layers when their contracts are sufficiently clear.