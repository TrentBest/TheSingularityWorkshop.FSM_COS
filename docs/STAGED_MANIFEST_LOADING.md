# Staged Manifest Loading and Dependency Planning

**Status:** first compatible contract slice is present on `development`; host localization and staged composition are not yet integrated. Do not describe deferred loading as a working end-to-end runtime feature until the host path is implemented and tested.

## UML class diagram: staged metadata and load-plan state

This is a simplified, source-derived overview of the current metadata and state-tracker types, not a claim that staged composition is integrated end to end. Method signatures are abbreviated; the source and the staged-loading contract below define exact signatures and behavior.

~~~mermaid
classDiagram
    class RuntimeManifest {
        +Bundles: IReadOnlyList~MicroBundleManifestEntry~
        +LoadPlan: IReadOnlyList~RuntimeManifestEntry~
        +Schedule: RuntimeManifestSchedule
        +ValidateStagedPlan() void
    }
    class RuntimeManifestLoadPlan {
        +Entries: IReadOnlyList~RuntimeManifestEntry~
        +GetState(bundleId) MicroBundleRuntimeState
        +MarkLocalized(bundleId) void
        +MarkLoaded(bundleId) void
        +EvaluatePromotions(evaluator, context, schedule) IEnumerable
    }
    class RuntimeManifestEntry {
        +Reference: MicroBundleReference
        +Stage: ManifestLoadStage
        +IsValid: bool
    }
    class RuntimeManifestSchedule {
        +Entries: IReadOnlyList~RuntimeManifestEntry~
        +Dependencies: IReadOnlyList~RuntimeManifestDependency~
        +IsDependencyReady(bundleId, isLoaded) bool
    }
    class RuntimeManifestDependency {
        +BundleId: ulong
        +DependencyId: ulong
        +IsValid: bool
    }
    class IManifestLoadEvaluator {
        <<interface>>
        +ShouldLoad(entry, experienceContext) bool
    }
    class ManifestLoadStage {
        <<enumeration>>
        Bootstrap
        Resident
        Deferred
    }
    class MicroBundleRuntimeState {
        <<enumeration>>
        Published
        Localized
        Loaded
    }

    RuntimeManifest o-- "0..*" RuntimeManifestEntry : optional plan
    RuntimeManifest o-- RuntimeManifestSchedule : optional schedule
    RuntimeManifestLoadPlan o-- "1..*" RuntimeManifestEntry : tracked entries
    RuntimeManifestLoadPlan --> RuntimeManifestSchedule : optional dependency gate
    RuntimeManifestLoadPlan ..> IManifestLoadEvaluator : asks for deferred promotion
    RuntimeManifestEntry --> ManifestLoadStage : intended stage
    RuntimeManifestLoadPlan ..> MicroBundleRuntimeState : tracks availability
    RuntimeManifestSchedule o-- "0..*" RuntimeManifestDependency : edges
~~~

This diagram is deliberately not an execution promise. The load-plan type tracks Published → Localized → Loaded state and can evaluate promotion candidates, but a host/repository must perform localization and actual loading. ManifestLoadStage labels intent; it does not automatically make FsmCos defer or promote bundles.

## Four responsibilities, four boundaries

![Dependency resolution and prerequisite relationships used during composition](assets/dependency-resolution.svg)

*The animated outline illustrates one valid dependency-first order for the example graph (`C → B → D → A`); it is not a live trace of a running process. The graph is illustrative, while actual dependencies come from each MicroBundle's declarations. This visual does not claim host localization or end-to-end deferred loading is implemented. See [`FsmCos.Execute` / `LoadRoot`](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/src/FSM_COS/FsmCos.cs) and the [dependency-order test](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/tests/FSM_COS.Tests/FsmCosTests.cs). The ownership table below remains authoritative for the current boundary.*


The staged-loading work must keep these concerns distinct:

| Concern | Owner | Responsibility |
|---|---|---|
| Dependency plan | Published manifest / FSM_COS contract | Describes which MicroBundles require which prerequisites |
| Process scheduling | FSM_API | Owns sequential process-group scheduling and execution semantics |
| Localization | Host/repository/cache adapter | Makes immutable published artifact bytes available locally and verifies identity/hash |
| Composition | FSM_COS | Resolves the current domain-owned MicroBundle contract, loads dependency closure, arbitrates, and returns a RuntimeAssembly |

FSM_COS must not become a storage engine or introduce a competing process scheduler. A dependency schedule is a description of prerequisites; it is not a substitute for FSM_API scheduling.

## Identity and lifecycle

A `MicroBundleReference` records:

- `BundleId`
- requested `Version`
- `ContentHash`

The reference is an identity claim, not proof that bytes have been downloaded or verified. The repository/localization boundary must validate the artifact against that identity.

The lifecycle remains:

```text
Published ──localize and verify──> Localized ──compose──> Loaded
```

- **Published** — the immutable artifact is identified by the published manifest.
- **Localized** — verified artifact bytes are available in the local cache.
- **Loaded** — the MicroBundle has been instantiated into the active composition.

Localization does not imply in-memory loading.

## Stages and dependency readiness

A manifest entry may be:

- **Bootstrap** — required to establish the initial visible/runnable Experience.
- **Resident** — participates in initial preparation but is not itself a bootstrap prerequisite.
- **Deferred** — eligible for later Experience-owned promotion.

Promotion candidates are considered in Bootstrap → Resident → Deferred order while preserving manifest order within each stage. Dependency readiness still wins: a bootstrap entry cannot be loaded until its prerequisites are loaded. This is planning/state-model behavior today; `FsmCos.Execute` does not yet run a bootstrap-only composition or continue localization in the background.

An `IManifestLoadEvaluator` lets the Experience decide *whether* a deferred capability is wanted. It does not decide whether prerequisites are ready: the dependency schedule/load plan must enforce that separately. A dependent entry cannot be marked Loaded until its prerequisites are Loaded.

Independent entries are entries with no prerequisite edges. Dependency-constrained entries have one or more prerequisites. This classification is useful for broad localization batches; actual composition still obeys dependency order.

## Compatibility with the current composition contract

The current manifest continues to use `MicroBundleManifestEntry` (bundle ID + requested version). Do not replace it with the old branch's `BundleRequest` model: that model lacks the current root-version contract and would conflict with the configuration-source boundary.

The current `FsmCos.Execute` path still resolves domain-owned `IMicroBundle` instances through `IMicroBundleCatalog`, traverses their dependency declarations, loads dependencies before dependents, detects cycles, and arbitrates to convergence. The optional staged metadata validates root identity/version consistency. When a `Schedule` is supplied, FSM_COS also preflights the catalog-resolved dependency closure and rejects scheduled bundle-ID, resolved-version, or dependency-edge drift before any `Load` side effects. It still does **not** execute bootstrap-only composition, perform asynchronous localization, verify content hashes, or promote Deferred entries into an already running RuntimeAssembly.

## Intended end-to-end direction

```text
Forge / publisher
  └─ immutable versioned manifest + dependency edges + stages
          ├─ localization clock → repository/cache batches
          ├─ composition clock  → dependency-ready MicroBundles
          └─ Experience clock   → bootstrap entry and continued operation
                                      │
                                      ▼
                              FSM_COS RuntimeAssembly
```

The three clocks should be able to progress independently. A host should be able to enter a minimum runnable Experience while other eligible artifacts continue localizing, without faking a delay or making the presentation layer act as the scheduler.

The published manifest should describe the dependency graph established by the Forge. The current runtime dependency traversal remains the compatibility behavior; the optional published schedule is now checked against the resolved graph before loading, but it does not yet dictate staged loading order or lifecycle. Do not treat stage metadata as an end-to-end staged runtime until localization and promotion have dedicated implementation and tests.

## Verification still required

- Verify the new planning contracts and tests on the current `development` dependency graph.
- Add consistency tests comparing published dependency edges with the resolved MicroBundle dependency contract.
- Introduce a host/repository localization abstraction only when its contract can be exercised by a real host.
- Integrate deferred promotion without losing configuration precedence, dependency closure, cycle detection, arbitration convergence, or RuntimeAssembly behavior.
- Demonstrate bootstrap entry while localization continues in the background.
- Keep NuGet publishing disabled unless the owner explicitly authorizes it.

---

<p align="center"><em>The Singularity Workshop — Tools for the curious, the bold, and the systemically inclined.</em><br><strong>Because state shouldn't be a mess.</strong><br><em>And because static boundaries are invitations to cause trouble.</em></p>
