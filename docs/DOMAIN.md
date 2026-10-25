<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# FSM_COS Domain and Ownership

**FSM_COS owns the composition of computation. It does not own the application that consumes the result.**

This page defines the domain boundary of FSM_COS. It is not the implementation walkthrough; see [Architecture](ARCHITECTURE.md) for how the contracts cooperate and [Consuming MicroBundles](CONSUMING_MICROBUNDLES.md) for how to use them.

## 01 — What belongs to the FSM_COS domain?

### The domain problem

A host asks for a computation by naming root MicroBundles and versions. Those roots may depend on other MicroBundles. FSM_COS must resolve the reachable dependency closure, validate that it is coherent, load prerequisites before dependents, give the composition a bounded arbitration phase, and return a `RuntimeAssembly` only when composition succeeds.

![FSM_COS domain map showing composition responsibilities and neighboring system boundaries](assets/fsm-cos-domain-map.svg)

The word **system** means the computation being assembled, not a particular application category. The same composition boundary can serve a website, desktop program, service, simulation, or headless host. Those are possible consumers, not built-in FSM_COS application types.

### What FSM_COS owns

- the runtime composition request contract (`RuntimeManifest`);
- root MicroBundle selection by identity and requested version;
- dependency-closure resolution and validation;
- composition-time loading and optional configuration delivery;
- bounded arbitration and convergence handling;
- the `RuntimeAssembly` result and handoff boundary.

The kernel owns the algorithm that composes the requested set. It does not own the domain meaning of every bundle.

### What FSM_COS consumes

| Contract or input | Owner | How FSM_COS uses it |
|---|---|---|
| Finite-state-machine and state/context primitives | [FSM_API](https://github.com/TrentBest/FSM_API) | Supplies foundational state/context types used by the runtime model. FSM_COS does not become the FSM_API process scheduler. |
| MicroBundle identity and runtime contract | [MicroBundleDomain](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleDomain) | Provides the domain-owned MicroBundle contract that FSM_COS resolves, loads, and arbitrates. |
| Optional semantic intent | [FSM_UserIO](https://github.com/TrentBest/FSM_UserIO) | Carries a platform-neutral intent value through the composition boundary when supplied. |
| Catalog / resolver | Host or repository integration | Makes the requested MicroBundle instances available by identity and version. |
| Configuration source | Host or repository integration | Supplies optional configuration bytes for a particular runtime, bundle, and version. |

The catalog and configuration source are injected boundaries. FSM_COS does not require a specific database, blob store, network protocol, artifact repository, or editor.

## 02 — Why keep ownership explicit?

### The design invariant

**The manifest says what is requested. FSM_COS assembles what must exist together. The host decides what happens next.**

Keep this boundary intact when adding integrations: FSM_COS should consume neighboring contracts rather than absorbing their domains.

## 03 — How does composition cross the domain boundary?

### Composition lifecycle

```text
RuntimeManifest
      │
      ▼
host-supplied catalog
      │
      ▼
resolve + validate dependency closure
      │
      ▼
load prerequisites before dependents
      │
      ▼
bounded arbitration
      │
      ├── no convergence / invalid composition → fail
      ▼
RuntimeAssembly
      │
      ▼
host-owned execution and presentation
```

Configuration is separate from the manifest. The manifest says **which roots and versions are requested**; the configuration source optionally provides bytes; each MicroBundle interprets its own configuration. FSM_COS does not read configuration files or impose a serialization format.

### What remains outside the domain

FSM_COS does not own:

- locating or downloading artifacts independently of the supplied catalog;
- verifying the actual artifact bytes represented by a content hash;
- converting arbitrary libraries into MicroBundles;
- running an application's main loop or scheduling its processes;
- GUI rendering, browser/desktop/Unity lifecycle, or user interaction policy;
- storage, networking, credential management, or platform permissions;
- in-place replacement, deferred activation, or retirement of a running assembly.

A host can combine FSM_COS with systems that provide those capabilities. Their existence elsewhere in the Workshop ecosystem does not mean they are implemented inside this package.

## 04 — See it in a minute

Run the [minimal consumer recipe](../samples/FSM_COS.MinimalConsumer/Program.md) to observe a real dependency-first composition and its expected output. This proves the composition path, not application execution or presentation.

## 05 — Choose your next step

### Related documents

- [Architecture](ARCHITECTURE.md) — principal types and execution path.
- [Runtime Boundary](RUNTIME_BOUNDARY.md) — explicit integration and non-feature boundaries.
- [Runtime Manifest](RUNTIME_MANIFEST.md) — request contract.
- [RuntimeAssembly](RUNTIME_ASSEMBLY.md) — successful handoff contract.
- [FSM_COS Theory](THEORY.md) — rationale for composition as a distinct operation.

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
