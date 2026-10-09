# Staged Manifest Loading and Dependency Planning

**Status:** first compatible contract slice is present on `development`; host localization and staged composition are not yet integrated. Do not describe deferred loading as a working end-to-end runtime feature until the host path is implemented and tested.

## Four responsibilities, four boundaries

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

The current `FsmCos.Execute` path still resolves domain-owned `IMicroBundle` instances through `IMicroBundleCatalog`, traverses their dependency declarations, loads dependencies before dependents, detects cycles, and arbitrates to convergence. The optional staged metadata currently validates identity/version consistency and provides a testable dependency/state model; it does **not** yet change that execution path or perform asynchronous localization.

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

The published manifest should eventually describe the dependency graph established by the Forge. The current runtime dependency traversal remains the compatibility behavior until the published graph has an authoritative producer, consistency checks against resolved MicroBundles, and end-to-end tests. Do not silently treat the published graph as authoritative before those checks exist.

## Verification still required

- Verify the new planning contracts and tests on the current `development` dependency graph.
- Add consistency tests comparing published dependency edges with the resolved MicroBundle dependency contract.
- Introduce a host/repository localization abstraction only when its contract can be exercised by a real host.
- Integrate deferred promotion without losing configuration precedence, dependency closure, cycle detection, arbitration convergence, or RuntimeAssembly behavior.
- Demonstrate bootstrap entry while localization continues in the background.
- Keep NuGet publishing disabled unless the owner explicitly authorizes it.
