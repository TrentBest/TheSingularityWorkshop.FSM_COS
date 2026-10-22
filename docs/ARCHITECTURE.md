<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop — animated workshop moniker" width="720" /></a></p>

# FSM_COS Architecture

> **Architecture answers how the composition kernel performs the theory.** For the deeper “why,” start with [FSM_COS Theory](THEORY.md).

FSM_COS is a small composition kernel. This document describes how its pieces cooperate rather than redefining concepts owned by neighboring systems.

## UML class diagram: composition kernel

This is a simplified, source-reviewed map of the principal composition contracts, not a complete per-type member inventory. Method signatures and collection element types are abbreviated for legibility; the XML-documented source and focused contract pages remain authoritative. The schedule is dependency metadata, not the FSM_API process scheduler. Staged metadata does not itself localize artifacts or activate deferred execution.

~~~mermaid
classDiagram
    class IFsmCos {
        <<interface>>
        +Execute(manifest, configurationSource) RuntimeAssembly
    }
    class FsmCos {
        +Execute(manifest, configurationSource) RuntimeAssembly
    }
    class IMicroBundleCatalog {
        <<interface>>
        +TryResolve(bundleId, version, bundle) bool
        +TryResolve(bundleId, bundle) bool
    }
    class IMicroBundleConfigurationSource {
        <<interface>>
        +TryGetConfiguration(runtimeId, bundleId, version, configuration) bool
    }
    class RuntimeManifest {
        +RuntimeId: ulong
        +Bundles: IReadOnlyList~MicroBundleManifestEntry~
        +ExperienceContext: IStateContext
        +LoadPlan: IReadOnlyList~RuntimeManifestEntry~
        +Schedule: RuntimeManifestSchedule
        +Intent: SemanticIntent
        +Empty(runtimeId, context, intent)$ RuntimeManifest
        +ValidateStagedPlan() void
    }
    class MicroBundleManifestEntry {
        <<record struct>>
        +BundleId: ulong
        +Version: string
    }
    class RuntimeManifestEntry {
        <<record>>
        +Reference: MicroBundleReference
        +Stage: ManifestLoadStage
        +IsValid: bool
    }
    class MicroBundleReference {
        <<record struct>>
        +BundleId: ulong
        +Version: string
        +ContentHash: string
        +IsValid: bool
        +Matches(entry): bool
    }
    class ManifestLoadStage {
        <<enumeration>>
        Bootstrap
        Resident
        Deferred
    }
    class RuntimeManifestSchedule {
        +Entries: IReadOnlyList~RuntimeManifestEntry~
        +Dependencies: IReadOnlyList~RuntimeManifestDependency~
        +IndependentEntries: IReadOnlyList~RuntimeManifestEntry~
        +DependencyEntries: IReadOnlyList~RuntimeManifestEntry~
        +DependenciesOf(bundleId)
        +IsDependencyReady(bundleId, isLoaded): bool
    }
    class RuntimeManifestDependency {
        <<record struct>>
        +BundleId: ulong
        +DependencyId: ulong
        +IsValid: bool
    }
    class RuntimeAssembly {
        +RuntimeId: ulong
        +Bundles: IReadOnlyList~IMicroBundle~
        +ArbitrationRounds: int
        +Intent: SemanticIntent
        +TryGetBundle(bundleId, bundle): bool
    }

    FsmCos ..|> IFsmCos
    FsmCos --> IMicroBundleCatalog : resolves
    FsmCos --> IMicroBundleConfigurationSource : optional input
    FsmCos --> RuntimeManifest : consumes
    FsmCos --> RuntimeAssembly : returns
    RuntimeManifest *-- "0..*" MicroBundleManifestEntry : root requests
    RuntimeManifest o-- "0..*" RuntimeManifestEntry : optional load plan
    RuntimeManifest o-- "0..1" RuntimeManifestSchedule : optional schedule
    RuntimeManifestEntry --> MicroBundleReference : published identity
    RuntimeManifestEntry --> ManifestLoadStage : intended stage
    RuntimeManifestSchedule o-- "0..*" RuntimeManifestEntry : entries
    RuntimeManifestSchedule o-- "0..*" RuntimeManifestDependency : dependency edges
~~~

The diagram describes the current type model; it does not claim that schedule construction performs repository localization or that deferred loading is active. See [Runtime Manifest](RUNTIME_MANIFEST.md) and [Staged Manifest Loading](STAGED_MANIFEST_LOADING.md) for the detailed contract and its limits.

## Runtime flow

![FSM_COS runtime flow](assets/fsm-cos-overview.svg)

![FSM_COS validation and composition path, generated with GUI.SVG](assets/fsm-cos-preflight.svg)

*Generated from `samples/DocumentationVisuals/Program.cs` using a pinned GUI.SVG source revision. This contract map shows the preflight gate, optional schedule validation, dependency-first loading, arbitration, and failure boundaries; it is not a live execution trace.*



## Core contracts

### RuntimeManifest
Identifies the runtime being assembled and supplies versioned root MicroBundle entries. It can also carry an optional host-provided `SemanticIntent`, an optional load-plan description, and an optional dependency schedule. Intent is passed through to the result; staged metadata is validated according to the current alpha contract and does not, by itself, activate repository localization or staged execution. The manifest is composition input, not application behavior. See [Runtime Manifest](RUNTIME_MANIFEST.md) and [Runtime Manifest Theory](MANIFEST_THEORY.md).

### MicroBundle manifest entries
A manifest entry identifies a requested MicroBundle and its requested version. Configuration is intentionally not part of the manifest. A separate configuration source may provide per-MicroBundle configuration at runtime.

### IMicroBundleCatalog
Resolves a bundle identity to the `IMicroBundle` required for installation. The catalog is supplied by the host, so the host can change its resolution implementation without changing FSM_COS's composition algorithm.

### IMicroBundle
Consumes the domain-owned `MicroBundleDescriptor` for identity/version/providers, while retaining composition-specific dependency configuration, Load, and Arbitrate behavior. FSM_COS does not redefine MicroBundle domain metadata. It does not expose a GUI, web server, or host lifecycle contract.

### MicroBundleLoadContext
Carries runtime identity and configuration available during installation. Configuration remains opaque to FSM_COS.

### ArbitrationContext
Exposes runtime identity, the currently loaded bundle set, and an optional FSM_API `IStateContext` supplied by the host. It is composition context, not host lifecycle state.

### RuntimeAssembly
The result surface of the composition pass: runtime identity, loaded bundles, the zero-based index of the round that reported convergence, and the optional `SemanticIntent` carried by the request. An `ArbitrationRounds` value of `0` means the first round converged; it is not the number of `Arbitrate` calls. Intent is passed through for the host; it does not make FSM_COS an input handler or presentation layer. See [RuntimeAssembly](RUNTIME_ASSEMBLY.md) and [FSM_COS Theory — RuntimeAssembly is the handoff object](THEORY.md#9-runtimeassembly-is-the-handoff-object).

![Dependency closure and installation order](assets/dependency-resolution.svg)

The conceptual reason for deriving this order rather than encoding it in the manifest is explained in [FSM_COS Theory — Composition starts with roots, not a giant object graph](THEORY.md#4-composition-starts-with-roots-not-a-giant-object-graph).

## Dependency resolution

FSM_COS first resolves and validates the complete reachable dependency closure before calling any bundle's `Load`. This rejects missing dependencies, malformed dependency contracts, identity mismatches, and cycles before bundle loading can create side effects. It snapshots the resolved bundle objects and dependency requests for the load pass, so the catalog is not queried a second time during loading. The subsequent depth-first installation order is iterative rather than recursive, avoiding call-stack exhaustion on deep graphs.

    requested A
       │
       ├── B
       │   └── C
       └── D

The resulting installation order is C, B, D, A.

Two guards remain fundamental during installation: already-loaded IDs are skipped, and IDs currently loading are tracked to detect cycles. The preflight graph validation independently detects cycles before installation begins.

Missing bundles and dependency cycles are composition failures.

## Configuration propagation

Configuration is an external runtime input. FSM_COS accepts configuration through an abstraction supplied by the host/repository layer; it does not read configuration files or interpret their format. If no configuration is available for a bundle, the bundle is loaded without external configuration and uses its defaults.

## Arbitration

FSM_COS resolves the catalog's actual reachable dependency graph and validates identity/dependency declarations before calling any MicroBundle `Load` method. When a manifest supplies a dependency schedule, it also checks the resolved closure, versions, and edges against that schedule before loading. This is a validation boundary, not a claim that FSM_COS localizes artifacts or implements staged/background loading. After reachable bundles are loaded, FSM_COS creates one ArbitrationContext.

    for each round
        for each loaded bundle
            changed |= bundle.Arbitrate(context, round)
        if no change → converged

If the configured maximum is reached without convergence, FSM_COS throws instead of returning an unstable assembly. The current default is ten rounds.

## Failure and partial side effects

Resolution, configuration lookup, bundle loading, and arbitration failures propagate to the caller; FSM_COS does not return a `RuntimeAssembly` when composition fails. Arbitration begins only after all reachable bundles have completed `Load`.

Composition is **not a transactional rollback boundary**. If a dependency completes `Load` and a later dependent bundle throws from `Load`, the earlier dependency's in-memory changes or external side effects cannot be generically undone by FSM_COS. The failing bundle is not added to the internal loaded set, and arbitration does not begin, but already-executed `Load` code may have changed state outside FSM_COS. Bundle authors and hosts must make loading safe for their lifecycle, and perform any required cleanup at the layer that owns those side effects.

The same principle applies to exceptions during arbitration: FSM_COS propagates the failure and does not hand off an assembly, but it cannot reverse arbitrary mutations performed by bundle code. Hosts should treat a failed `Execute` as a failed composition attempt, not as proof that all bundle-side effects were reverted.

### Diagnostic surface

The diagnostic contract is deliberately small: `Execute` either returns a converged `RuntimeAssembly` or throws. FSM_COS does not keep a logging pipeline, diagnostics registry, or partially successful assembly for the host to inspect. For failures it detects itself, exception messages identify the relevant bundle IDs and, where applicable, requested/resolved versions, dependency cycles, missing schedule entries, or mismatched dependency edges. Exceptions raised by the catalog, configuration source, `Load`, or `Arbitrate` are allowed to propagate so their owning layer retains the original failure details.

Hosts should catch failures at their composition boundary, preserve the exception and its context in their own logging/telemetry system, and apply cleanup according to their lifecycle policy. Message text is useful for troubleshooting, but consumers should not treat exact prose as a stable machine-readable error code.

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
       ├── WebPage → browser experience
       ├── AnyApp → desktop/local experience
       └── other hosts → their own manifestation

## Runtime boundary

The current alpha performs a synchronous composition pass:

```text
Manifest (bundle + version)
        ↓
host-supplied catalog / resolver
        ↓
optional configuration source
        ↓
dependency closure and preflight validation
        ↓
Load() in dependency-first order
        ↓
bounded Arbitrate() rounds
        ↓
RuntimeAssembly
```

The kernel validates the reachable dependency graph before calling bundle load code. It then loads dependencies before dependents and returns a `RuntimeAssembly` only when a complete arbitration round reports no changes.

This package does not localize artifact bytes, verify content hashes, load independent bundles in parallel, or replace an active experience in a running application. The schedule and load-plan types carry validated metadata and state; they do not implement those execution and lifecycle behaviors.

The integrating system supplies the catalog and any configuration source, and owns artifact delivery, serialization, application-specific lifecycle, and presentation. FSM_COS owns the composition pass described above.

---

## 🔗 The Singularity Workshop

FSM_COS is one layer in a deliberately troublesome ecosystem:

- **[FSM_API](https://github.com/TrentBest/FSM_API)** — behavior and state.
- **[FSM_COS](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS)** — composition and runtime assembly.
- **[FSM_Serialization](https://github.com/TrentBest/TheSingularityWorkshop.FSM_Serialization)** — representation and the byte boundary.
- **[WebPage](https://github.com/TrentBest/WebPage)** — browser manifestation and proving ground.

<p align="center"><em>The Singularity Workshop — Tools for the curious, the bold, and the systemically inclined.</em><br><strong>Because state shouldn't be a mess.</strong><br><em>And because static boundaries are invitations to cause trouble.</em></p>

---

## 🔗 Resources & Support

- **Project:** [FSM_COS](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS)
- **Workshop projects:** [FSM_API](https://github.com/TrentBest/FSM_API) · [FSM_Serialization](https://github.com/TrentBest/TheSingularityWorkshop.FSM_Serialization) · [WebPage](https://github.com/TrentBest/WebPage)
- **Connect:** [GitHub](https://github.com/TrentBest) · [Coder Legion profile](https://coderlegion.com/user/The%2BSingularity%2BWorkshop) · [Coder Legion community](https://coderlegion.com/)
- **Support:** [Patreon](https://www.patreon.com/c/TheSingularityWorkshop) · [PayPal](https://www.paypal.com/donate/?hosted_button_id=3Z7263LCQMV9J)

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
