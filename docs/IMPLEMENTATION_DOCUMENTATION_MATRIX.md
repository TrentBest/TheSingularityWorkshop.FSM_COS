<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# FSM_COS Implementation-to-Documentation Matrix

**Status: initial source/test mapping; not yet a completed release audit.** This file is the working bridge between the public explanation and the implementation. Its purpose is to make undocumented behavior, unsupported claims, and untested contracts visible. A row marked as mapped means a likely source/test location has been identified; it does not mean every edge case has been audited.

## Build-mode parity is part of the contract audit

The project has two dependency modes. A normal package build references published FSM_API, FSM_UserIO, and MicroBundleDomain packages; a repository-source build is enabled with `WorkshopUseRepositorySources=true` and defines `WORKSHOP_USE_REPOSITORY_SOURCES`, which conditionally changes parts of the public model (including pre-resolved arbitration/configuration members). These modes must not be treated as identical merely because the same source files compile.

The release audit must compare both modes explicitly: public API shape, dependency versions, package contents, behavior, and consumer examples. Repository-source integration tests alone are insufficient evidence for the public package contract.

## The product contract

FSM_COS composes a runtime from a versioned request and a host-provided catalog. Its current core path is:

```text
RuntimeManifest
    -> validate request and resource bounds
    -> resolve requested roots and dependency closure through catalog
    -> validate identity, versions, dependency edges, and cycles
    -> load prerequisites before dependents
    -> supply resolved configuration
    -> bounded arbitration / convergence
    -> RuntimeAssembly
    -> host owns execution, presentation, and lifecycle
```

A failure in the composition path must not be described as a successful assembly. The host owns artifact acquisition and delivery, execution scheduling, UI/rendering, and application lifecycle.

## Source, documentation, and tests

| Contract / responsibility | Primary implementation | Current documentation home | Test evidence to inspect |
|---|---|---|---|
| Composition entry points and phase-aware result | [FsmCos.cs](../src/FSM_COS/FsmCos.cs), [IFsmCos.cs](../src/FSM_COS/IFsmCos.cs), [RuntimeCompositionResult.cs](../src/FSM_COS/RuntimeCompositionResult.cs) | [Architecture](ARCHITECTURE.md), [Composition Failures](COMPOSITION_FAILURES.md) | [FsmCosTests.cs](../tests/FSM_COS.Tests/FsmCosTests.cs) |
| Manifest roots, versions, and validation | [RuntimeManifest.cs](../src/FSM_COS/RuntimeManifest.cs), [RuntimeManifestEntry.cs](../src/FSM_COS/RuntimeManifestEntry.cs), [RuntimeManifestDependency.cs](../src/FSM_COS/RuntimeManifestDependency.cs) | [Runtime Manifest](RUNTIME_MANIFEST.md), [Manifest Theory](MANIFEST_THEORY.md) | [FsmCosTests.cs](../tests/FSM_COS.Tests/FsmCosTests.cs), [RuntimeManifestPlanningTests.cs](../tests/FSM_COS.Tests/RuntimeManifestPlanningTests.cs) |
| Catalog resolution and bundle loading | [IMicroBundleCatalog.cs](../src/FSM_COS/IMicroBundleCatalog.cs), [FsmCos.cs](../src/FSM_COS/FsmCos.cs) | [Consuming MicroBundles](CONSUMING_MICROBUNDLES.md), [Architecture](ARCHITECTURE.md) | [FsmCosTests.cs](../tests/FSM_COS.Tests/FsmCosTests.cs) |
| Dependency schedule, cycles, readiness, and resource bounds | [RuntimeManifestSchedule.cs](../src/FSM_COS/RuntimeManifestSchedule.cs), [RuntimeManifestLoadPlan.cs](../src/FSM_COS/RuntimeManifestLoadPlan.cs) | [Staged Manifest Loading](STAGED_MANIFEST_LOADING.md), [Runtime Manifest](RUNTIME_MANIFEST.md) | [RuntimeManifestPlanningTests.cs](../tests/FSM_COS.Tests/RuntimeManifestPlanningTests.cs), [FsmCosTests.cs](../tests/FSM_COS.Tests/FsmCosTests.cs) |
| Configuration resolution, ownership, and snapshot isolation | [IMicroBundleConfigurationSource.cs](../src/FSM_COS/IMicroBundleConfigurationSource.cs), [FsmCos.cs](../src/FSM_COS/FsmCos.cs), [RuntimeAssembly.cs](../src/FSM_COS/RuntimeAssembly.cs) | [Consuming MicroBundles](CONSUMING_MICROBUNDLES.md), [RuntimeAssembly](RUNTIME_ASSEMBLY.md) | [FsmCosTests.cs](../tests/FSM_COS.Tests/FsmCosTests.cs), [ResolvedRuntimeTests.cs](../tests/FSM_COS.Tests/ResolvedRuntimeTests.cs), [SnapshotIsolationTests.cs](../tests/FSM_COS.Tests/SnapshotIsolationTests.cs) |
| Bounded arbitration and convergence | [ArbitrationContext.cs](../src/FSM_COS/ArbitrationContext.cs), [FsmCos.cs](../src/FSM_COS/FsmCos.cs) | [Arbitration](ARBITRATION.md), [Theory](THEORY.md) | [FsmCosTests.cs](../tests/FSM_COS.Tests/FsmCosTests.cs) |
| Load compensation / rollback contract | [IMicroBundleRollback.cs](../src/FSM_COS/IMicroBundleRollback.cs), [FsmCos.cs](../src/FSM_COS/FsmCos.cs) | [Consuming MicroBundles](CONSUMING_MICROBUNDLES.md), [Composition Failures](COMPOSITION_FAILURES.md) | [FsmCosTests.cs](../tests/FSM_COS.Tests/FsmCosTests.cs) |
| Successful assembly lookup and configuration access | [RuntimeAssembly.cs](../src/FSM_COS/RuntimeAssembly.cs), [MicroBundleReference.cs](../src/FSM_COS/MicroBundleReference.cs) | [RuntimeAssembly](RUNTIME_ASSEMBLY.md) | [ResolvedRuntimeTests.cs](../tests/FSM_COS.Tests/ResolvedRuntimeTests.cs), [FsmCosTests.cs](../tests/FSM_COS.Tests/FsmCosTests.cs) |
| Staged load metadata and deferred promotion evaluation | [RuntimeManifestLoadPlan.cs](../src/FSM_COS/RuntimeManifestLoadPlan.cs), [RuntimeManifestSchedule.cs](../src/FSM_COS/RuntimeManifestSchedule.cs), [ManifestLoadStage.cs](../src/FSM_COS/ManifestLoadStage.cs), [IManifestLoadEvaluator.cs](../src/FSM_COS/IManifestLoadEvaluator.cs) | [Staged Manifest Loading](STAGED_MANIFEST_LOADING.md) | [RuntimeManifestPlanningTests.cs](../tests/FSM_COS.Tests/RuntimeManifestPlanningTests.cs) |
| Multi-experience registration, selection, and host handoff | [RuntimeExperienceHost.cs](../src/FSM_COS/RuntimeExperienceHost.cs), [RuntimeExperienceRegistry.cs](../src/FSM_COS/RuntimeExperienceRegistry.cs) | [Runtime Boundary](RUNTIME_BOUNDARY.md), [RuntimeAssembly](RUNTIME_ASSEMBLY.md) | [RuntimeExperienceRegistryTests.cs](../tests/FSM_COS.Tests/RuntimeExperienceRegistryTests.cs) |
| Dependency-direction / optional AI and GUI capability composition | [FSM_COS.csproj](../src/FSM_COS/FSM_COS.csproj) and test fixture types | [Architecture](ARCHITECTURE.md), [Domain](DOMAIN.md) | [AiGuiCapabilityHandoffTests.cs](../tests/FSM_COS.Tests/AiGuiCapabilityHandoffTests.cs) |
| Public use-case and source relationship diagrams | [src/FSM_COS](../src/FSM_COS), [tests/FSM_COS.Tests](../tests/FSM_COS.Tests) | [UML index](UML/README.md), [UML inventory](UML/INVENTORY.md) | Diagram coverage and generation scope must be checked against the current source/test tree |

## Boundaries that must remain explicit

These are not implied by the existence of related names or planning metadata:

- A `RuntimeManifestLoadPlan` and `RuntimeManifestSchedule` do not, by themselves, implement artifact downloading, content-hash verification, parallel loading, or deferred activation in an already-running assembly.
- A `RuntimeAssembly` is a composition result and handoff, not a started application, process scheduler, browser page, or renderer.
- Bounded arbitration is not a general proof of semantic compatibility between arbitrary MicroBundles.
- In-process MicroBundles are not sandboxed merely because they pass through the composition kernel.
- A source version, release-note file, green historical workflow, or README badge does not prove that the current HEAD is release-ready or that a package version has been published.

## Audit completion checklist

The audit is complete only when every public claim and meaningful contract has been checked against implementation and tests at the same revision.

- [ ] Enumerate all public types and members from both normal package mode and `WorkshopUseRepositorySources=true` mode; explain any intentional API differences.
- [ ] Map every public member to one authoritative contract page or explicitly mark it intentionally undocumented.
- [ ] Verify every behavior claim against source and at least one relevant test, or label the claim source-checked / conceptual.
- [ ] Audit failure paths, validation, resource limits, exception/result behavior, and rollback/compensation semantics.
- [ ] Audit mutability, collection snapshots, byte ownership, concurrency/thread-safety, and lifecycle guarantees; document only guarantees actually provided.
- [ ] Audit every staged-loading state transition and distinguish planning/evaluation from actual activation.
- [ ] Run and record the current test suite and relevant CI against one exact commit SHA.
- [ ] Check all links, code snippets, diagrams, and generated artifacts; remove stale or duplicative material.
- [ ] Verify README, NuGet README, documentation index, Gallery, release notes, and public API tell the same story.
- [ ] Record unresolved implementation gaps separately from documentation gaps; do not use prose to paper over missing behavior.

## Next audit pass

Start with the source of truth, not the current prose: inspect `FsmCos.cs` and every public contract type; then walk the tests by behavior category. Update the matrix only when a source/test location has been verified. Where a contract is missing in the implementation, decide whether to implement it, narrow the public promise, or explicitly defer it.

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
