<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# Ecosystem Integration Audit — Refactor Priorities

**Status:** Initial source-checked audit; not a claim that every repository or every active branch has been exhaustively reviewed.  
**Purpose:** Identify where Workshop functionality is still implemented through transitional host-specific machinery, duplicated runtime paths, or compiled-in bundle inventories instead of flowing through the intended technology boundaries. Order the work from structural refactors to smaller integration and proof tasks.  
**Target:** A coherent end-to-end path from published Experience intent to immutable MicroBundle artifacts, FSM_COS composition, and host-owned execution/presentation.

## 01 — The architecture we are integrating

The intended responsibility chain is:

```text
FSM_API
  FSM/state/context primitives and the supported execution machinery
        |
        v
FSM_COS
  resolve requested MicroBundles -> validate dependencies -> load -> arbitrate
        |
        v
RuntimeAssembly
        |
        +----------------------------+
        |                            |
        v                            v
Experience / MicroBundleDomain      Host
  what a bundle provides             execution lifecycle, permissions,
  and how it participates             UI, platform resources, manifestation
        ^
        |
MicroBundleRepository
  immutable artifact delivery and content verification
        ^
        |
Experiences / authoring / publication
  define and publish versioned Experience and bundle identities
```

The exact contract matters. FSM_COS is a **composition boundary**, not the repository, application scheduler, GUI, or finished host. Its current synchronous `FsmCos.Execute` path returns a `RuntimeAssembly`; staged metadata does not automatically download artifacts, verify their bytes, activate deferred bundles, or hot-swap a running assembly. The host must explicitly connect the composed result to its real Experience lifecycle.

This audit does **not** recommend putting every responsibility inside FSM_COS. It recommends using each existing technology for the responsibility it already owns and keeping only genuinely host-specific behavior in WebPage, AnyApp, and other hosts.

## 02 — Priority order

Priority describes dependency order and architectural impact, not blame. Do not remove transitional implementations until their behavior has been transferred and verified.

### P0 — Establish one authoritative composition-to-execution path

**Why this is first:** Until a manifest-selected Experience has one clear composed identity and lifecycle path, repository delivery and host portability can only feed a partly duplicated runtime.

**Source-checked evidence**

- [WebPage WorkshopExperienceService.cs](https://github.com/TrentBest/WebPage/blob/development/TheSingularityWorkshop/Services/WorkshopExperienceService.cs) composes the primary manifest, hub items, and capabilities through `FsmCos.Execute`. It then separately activates a Hub bundle through `_hubRuntime.ActivateManifestBundle(candidateHubBundle)`.
- [WebPage HubRuntime.cs](https://github.com/TrentBest/WebPage/blob/development/TheSingularityWorkshop/Infrastructure/Hub/HubRuntime.cs) resets the Hub's manifest runtime, calls `Hub.LoadBundle(...)`, and runs `Hub.ExecuteArbitrationPipeline()`.
- [WebPage ExperienceExecutionRuntime.cs](https://github.com/TrentBest/WebPage/blob/development/TheSingularityWorkshop/Services/ExperienceExecutionRuntime.cs) checks that declared bundle IDs exist in the `RuntimeAssembly`, but its compatibility adapter still routes ticks and lifecycle signals to the preserved `PageFSM`.
- [WebPage PageFSM.cs](https://github.com/TrentBest/WebPage/blob/development/TheSingularityWorkshop/Services/PageFSM.cs) contains the existing page/Living GUI lifecycle and must be treated as behavior to preserve, not as disposable scaffolding.

These findings show two connected paths: FSM_COS composition and a separate Hub bundle activation/arbitration path. The host still owns execution, so the existence of a host scheduler is not itself a violation. The unresolved question is whether both paths are necessary, and which one owns bundle identity, activation, and lifecycle for each manifest role.

**Refactor**

1. Write a source-backed runtime contract defining which objects FSM_COS composes, which runtime component ticks them, and how an Experience lifecycle maps to the assembled bundles.
2. Make the manifest-selected `RuntimeAssembly` the authoritative composition result for the corresponding Experience. If the Hub needs a host execution adapter, it should consume that result instead of silently rebuilding an equivalent bundle runtime.
3. Keep `PageFSM` and Living GUI behavior until their responsibilities are explicitly mapped to the Experience lifecycle and regression-tested.
4. Remove duplicate activation/arbitration only after tests prove the replacement path preserves startup, entry, Living GUI population/freeze, Moniker presentation, gravity, navigation, runtime switching, and shutdown.

**Acceptance evidence**

- One manifest-to-assembly-to-Experience lifecycle trace with identity and ownership recorded at each step.
- Tests prove that each intended bundle is composed once per runtime generation and that the active lifecycle advances through the expected FSM_API-backed behavior.
- Failed candidate composition leaves the previous runtime usable; successful activation retires the previous generation once.
- Browser smoke testing confirms the real manifestation, not only unit tests.

**Status:** Confirmed integration seam; the exact amount of duplicated behavior still needs a complete call-path/test audit before code is removed.

### P1 — Replace host-compiled bundle inventories with a repository-backed catalog/provider

**Why this is next:** Composition cannot be portable if hosts must know every available bundle in their own source code.

**Source-checked evidence**

- [WebPage WorkshopCompositionCatalog.cs](https://github.com/TrentBest/WebPage/blob/development/TheSingularityWorkshop/Workshop/Composition/WorkshopCompositionCatalog.cs) has a fixed set of bundle-ID-to-factory registrations for Moniker, Living GUI, rendering, and AI-related bundles.
- [AnyApp's development `AnyAppMicroBundleCatalog`](https://github.com/TrentBest/AnyApp/blob/development/AnyAppMicroBundleCatalog.cs) still contains a compiled inventory, but [AnyAppRuntime.ComposeAsync](https://github.com/TrentBest/AnyApp/blob/development/AnyAppRuntime.cs) can take a repository-backed path when the manifest declares artifact identities. This is partial integration, not absence of integration.
- [AnyApp's development `RepositoryMicroBundleCatalog`](https://github.com/TrentBest/AnyApp/blob/development/RepositoryMicroBundleCatalog.cs) implements recursive preload, local content-addressed caching, version checks, and resolved configuration loading. The repository itself also provides a [version-aware RestMicroBundleCatalog](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleRepository/blob/development/src/REST/TheSingularityWorkshop.MicroBundleRepository.Rest/RestMicroBundleCatalog.cs). The core closure/materialization/resolution responsibility now exists in both places; AnyApp's cache/configuration behavior may be host-specific, but it should reuse or wrap the shared catalog behavior instead of maintaining a second closure loader.
- The [MicroBundleRepository development README](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleRepository/blob/development/README.md) defines the artifact delivery boundary and content-hash verification responsibility. Its current REST catalog and materializer form a real integration bridge, but they are not the only path in use. FSM_COS's [WebPage integration contract](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/docs/WEBPAGE_INTEGRATION.md) correctly leaves artifact discovery to a host-supplied catalog.

**Refactor**

1. Make the repository-backed path the canonical path for published manifests. Retain the compiled catalog only as an explicit local-development/test provider or compatibility path, not as a host's permanent inventory of Experiences.
2. Reuse the repository's version-aware `RestMicroBundleCatalog` and materializer in AnyApp. Preserve AnyApp's cache and configuration behavior, but separate those host policies from duplicate dependency-closure traversal and catalog resolution.
3. Connect WebPage's `WorkshopCompositionCatalog` to the same repository-backed catalog/provider contract while keeping local factories only for tests and explicit offline demos.
4. Resolve each manifest's exact `(BundleId, Version, ContentHash)` identity and create isolated bundle instances for independent runtime generations; do not reuse an already-owned mutable bundle object across assemblies.
5. Keep network, storage credentials, cache policy, and trust decisions in the provider/host layer; do not add Azure or HTTP concerns to FSM_COS.

**Acceptance evidence**

- An Experience can be selected by immutable identity on a clean host without its MicroBundle implementation being compiled into that host.
- A missing artifact, version mismatch, or content-hash mismatch fails before composition.
- A repeated request resolves the same immutable artifact identity while producing runtime-owned instances with independent lifetimes.
- Offline/local development and remote delivery pass the same catalog contract tests.

**Status:** AnyApp development has a working source-level repository path for manifests with artifact identities; WebPage development still uses a fixed factory catalog. The repository also has a version-aware REST catalog. The refactor is to converge these existing pieces, not invent a new provider.

### P1 — Make the Experience lifecycle a first-class contract, not a host-specific inference

**Why this matters:** A composed bundle graph describes what is present. It does not by itself define how a complete Experience starts, receives host capabilities, advances, reports state, and shuts down.

**Source-checked evidence**

- WebPage's [ExperienceExecutionRuntime.cs](https://github.com/TrentBest/WebPage/blob/development/TheSingularityWorkshop/Services/ExperienceExecutionRuntime.cs) currently describes itself as a compatibility adapter used until the Experience package exposes an executable lifecycle contract.
- [WebPage WebPageHostManifest.cs](https://github.com/TrentBest/WebPage/blob/development/TheSingularityWorkshop/Services/WebPageHostManifest.cs) already describes startup/running Experiences and their bundle identities, but the manifest alone is not the executable lifecycle.
- The [Experiences README](https://github.com/TrentBest/TheSingularityWorkshop.Experiences/blob/master/README.md) describes Experiences as separate from WebApp, AnyApp, FSM_COS, and presentation technology, with multiple manifestations sharing identity rather than implementation.

**Refactor**

1. Define the smallest platform-neutral Experience runtime contract in the owning Experience/domain package, not in WebPage and not inside FSM_COS.
2. Specify lifecycle entry/exit, context ownership, host capability access, state observation, cancellation/shutdown, and bundle ownership/disposal explicitly.
3. Make WebPage and AnyApp adapt that contract to browser and desktop manifestation. They must not infer Experience identity solely from a hardcoded list or bundle-ID coincidence.
4. Preserve FSM_API for actual state/lifecycle behavior where appropriate; do not replace it with another hand-written transition engine as part of this refactor.
5. Keep UI-specific rendering and input in each host, and keep semantic Experience behavior independent of WPF/Blazor types.

**Acceptance evidence**

- The same published Experience identity composes on WebPage and AnyApp without the Experience implementation being duplicated in either host.
- Host adapters may differ, but lifecycle and ownership conformance tests are shared.
- A host that lacks a required capability reports a concrete incompatibility instead of silently substituting another Experience.

**Status:** The compatibility adapter and intended cross-manifestation boundary are confirmed. The canonical lifecycle contract and its owning package need to be selected and verified against current source before implementation.

### P2 — Reconcile package/API version skew before consumers depend on newer contracts

**Source-checked evidence**

- FSM_COS `development` documents source candidate `0.1.0-alpha.7` in [WebPage integration](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/docs/WEBPAGE_INTEGRATION.md).
- [AnyApp.csproj on development](https://github.com/TrentBest/AnyApp/blob/development/AnyApp.csproj) uses the published FSM_COS `0.1.0-alpha.6` package when repository-source mode is disabled, while its source-integration mode references the checked-out FSM_COS source. [AnyApp master](https://github.com/TrentBest/AnyApp/blob/master/AnyApp.csproj) is further behind at `0.1.0-alpha.4`.
- AnyApp development also pins the published MicroBundleRepository REST package at `0.1.0-alpha.2`, while source-integration mode builds the repository checkout. These are distinct dependency graphs and should not be treated as equivalent validation.

This is deliberate source/package skew until coordinated releases are authorized, not an instruction to bump packages immediately. It means source-integration success does not prove the released package graph has the same behavior. Do not change package versions or publish anything as part of this audit.

**Refactor**

1. Record the exact source commit and package version each host is intended to consume.
2. Compare the consumer's used API against that package's public API and tests.
3. When the owner authorizes the dependency update, update the package pin, restore, build, test, and verify the actual package artifact.
4. Keep the NuGet publish gate closed unless the owner explicitly authorizes publication.

**Acceptance evidence:** A consumer/package compatibility table tied to immutable commit/version evidence; no claim that a source candidate is published merely because it exists in a repository.

### P1 — Complete publication, artifact delivery, and Experience registration

**Why this matters:** A discoverable Experience record is not proof that every bundle it requests is published, immutable, retrievable, and verifiable.

**Source-checked evidence**

- The [Experiences development publication workflow](https://github.com/TrentBest/TheSingularityWorkshop.Experiences/blob/development/.github/workflows/publish-microbundles.yml) has a manual Moniker publication path, but checks out the MicroBundleRepository `feature/microbundle-publisher` branch to run its publisher tool. That tool directory is absent from the repository's `development` tree, and the branch is outside the required long-lived `master`/`development` model.
- The workflow writes a `publication.json` artifact after publishing the bundle but does not register an Experience manifest in the repository's Experience catalog.
- [AnyApp's ExperiencePublisher](https://github.com/TrentBest/AnyApp/blob/development/Publishing/ExperiencePublisher.cs) resolves and uploads bundle/configuration artifacts through `IMicroBundleRepository`, then writes the resolved manifest to a local file. [AnyApp's own pipeline guide](https://github.com/TrentBest/AnyApp/blob/development/docs/EXPERIENCE_PUBLISH_PIPELINE.md) says its current Experience catalog API is read-only; manifest publication/registration still needs a write adapter.
- The [MicroBundleRepository development REST host](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleRepository/blob/development/src/REST/TheSingularityWorkshop.MicroBundleRepository.Rest.Host/Program.cs) exposes artifact retrieval and catalog listing endpoints. Bundle artifact publication exists, but the inspected paths do not complete the last step of publishing the resolved Experience manifest and making it discoverable through the catalog.

**Refactor**

1. Bring the publisher tool into the repository's canonical `development` branch, update the workflow to consume that maintained source, and retire the feature-branch dependency only after confirming the needed tool has been integrated and tested. Do not delete the branch automatically.
2. Add a supported write contract for the resolved Experience manifest and catalog publication pointer; keep bundle/configuration artifacts immutable and publish the discoverable manifest last.
3. Make the complete chain atomic from the consumer's perspective: bundle/configuration artifacts → resolved manifest with exact identities/dependencies/load order → manifest artifact → catalog pointer. Partial failures may leave unreferenced immutable blobs, but must not create a discoverable incomplete Experience.
4. Expand the manual workflow beyond Moniker only after the end-to-end path is proven for a multi-bundle Experience. Keep publication manually authorized; do not turn it into an automatic NuGet or artifact release on ordinary build.
5. Test from a clean consumer that has no local copy of the bundle implementation and verify the selected Experience is discoverable after publication.

**Acceptance evidence:** A clean-room publication-and-consumption test, including identity mismatch, missing artifact, missing dependency, and immutable-write behavior.

**Status:** Bundle artifact upload and local resolved-manifest generation are implemented in development. The end-to-end publication gap is manifest registration/discovery plus the publisher tool's dependency on a noncanonical feature branch. This is source-checked; the workflow was not executed.

### P3 — Remove transitional registries and demo scaffolding after replacement

Examples include WebPage's static `IdleExperienceCatalog`, `FlexExperienceCatalog`, and `WorkshopDemoBundle`. They should not be deleted simply because they are static: first identify the behavior each supplies, move it to the Experience/MicroBundle/registry contract, and prove parity. The WebPage branch's earlier handoff documents these as transitional; current development files must be checked again when this stage is scheduled.

**Acceptance evidence:** No required selection, startup, fallback, or presentation behavior depends on the old catalog/demo path; tests cover registry discovery and an unknown/unsupported capability.

### P4 — Finish integration observability and contract tests

This is the smallest stage, but it protects every prior stage from regression.

- Give every integration check a unique, actionable name.
- Test the full identity chain: Experience ID/version → requested bundle IDs/versions/hashes → resolved bundle instances → `RuntimeAssembly` → active Experience runtime.
- Add failure-path coverage for missing bundles, wrong versions/hashes, duplicate identity, dependency failure, arbitration non-convergence, activation failure, and disposal.
- Run browser-level checks for WebPage's actual rendered lifecycle and host-level launch tests for AnyApp; unit tests alone do not prove manifestation.
- Record source revision, package version, test command, result, and limitations. Do not claim an end-to-end path based solely on mocked catalogs or in-memory bundles.

## 03 — Recommended execution order and gates

| Order | Deliverable | Gate before proceeding |
|---|---|---|
| 1 | Runtime ownership map and duplicate-path call graph | Each runtime responsibility has one named owner; behavior inventory exists |
| 2 | Unified composition-to-Experience activation path | FSM_COS result is authoritative; legacy behavior is preserved by tests |
| 3 | Platform-neutral Experience lifecycle contract | WebPage and AnyApp can adapt it without owning the Experience implementation |
| 4 | Repository-backed immutable bundle provider/catalog | A clean host can resolve and verify a bundle without compiled-in bundle code |
| 5 | Manifest/artifact publication chain | Published manifests reference retrievable, immutable, hash-verified bundle artifacts |
| 6 | Consumer package/version reconciliation | Each consumer is built and tested against the intended published package version |
| 7 | Transitional catalog/demo removal | Parity proven; no orphaned behavior or fallback path |
| 8 | End-to-end CI evidence and documentation | Clean-room host-to-Experience test and real manifestation checks pass |

The sequence deliberately addresses runtime semantics before transport. Moving artifacts to a repository before the runtime ownership boundary is clear would make delivery more flexible while leaving duplicate lifecycle/composition behavior intact.

## 04 — Audit scope, evidence, and limitations

This initial pass source-checked these active-development snapshots where available, plus published/master baselines where version pins matter:

- [FSM_COS `development`](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/tree/development): architecture, runtime boundary, staged-loading limits, WebPage integration contract, repository instructions, documentation index, and current audit notes.
- [WebPage `development`](https://github.com/TrentBest/WebPage/tree/development): composition service/catalog, Hub activation adapter, Experience execution adapter, host manifest, page FSM, and related transitional bundle.
- [AnyApp `development`](https://github.com/TrentBest/AnyApp/tree/development): compiled and repository-backed catalog paths, resolved-manifest publisher, configuration cache, source/package build modes, and its own publication status notes.
- [Experiences `development`](https://github.com/TrentBest/TheSingularityWorkshop.Experiences/tree/development): bundle publication workflow, artifact identity generation, and current publication documentation.
- [MicroBundleRepository `development`](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleRepository/tree/development): REST catalog, artifact repository, host endpoints, build/source-integration workflow, and branch-dependent publisher integration.

This is **source-checked, not executed**. I did not clone/build the repositories, run their test suites, inspect every file or commit history, verify live browser behavior, validate a deployed repository endpoint, or verify current CI status. The full WebPage activation/rollback path, all Experience package variants, and runtime behavior across multiple manifests still require deeper passes. The publisher workflow's feature-branch dependency and missing development-branch tool path were checked against the repository trees; the workflow was not run.

The audit identifies the largest supported integration seams and a safe order for investigating them. It does not authorize broad deletion, package publication, or replacement of `master`.

## 05 — Repository evidence index

- [FSM_COS architecture](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/docs/ARCHITECTURE.md)
- [FSM_COS runtime boundary](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/docs/RUNTIME_BOUNDARY.md)
- [FSM_COS staged-loading limits](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/docs/STAGED_MANIFEST_LOADING.md)
- [FSM_COS consumer integration contract](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/docs/WEBPAGE_INTEGRATION.md)
- [WebPage composition service](https://github.com/TrentBest/WebPage/blob/development/TheSingularityWorkshop/Services/WorkshopExperienceService.cs)
- [WebPage composition catalog](https://github.com/TrentBest/WebPage/blob/development/TheSingularityWorkshop/Workshop/Composition/WorkshopCompositionCatalog.cs)
- [WebPage Experience execution adapter](https://github.com/TrentBest/WebPage/blob/development/TheSingularityWorkshop/Services/ExperienceExecutionRuntime.cs)
- [AnyApp runtime entry point](https://github.com/TrentBest/AnyApp/blob/development/AnyAppRuntime.cs)
- [AnyApp repository catalog](https://github.com/TrentBest/AnyApp/blob/development/RepositoryMicroBundleCatalog.cs)
- [AnyApp publisher](https://github.com/TrentBest/AnyApp/blob/development/Publishing/ExperiencePublisher.cs)
- [MicroBundleRepository catalog](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleRepository/blob/development/src/REST/TheSingularityWorkshop.MicroBundleRepository.Rest/RestMicroBundleCatalog.cs)
- [Experiences publication workflow](https://github.com/TrentBest/TheSingularityWorkshop.Experiences/blob/development/.github/workflows/publish-microbundles.yml)
- [Experiences repository overview](https://github.com/TrentBest/TheSingularityWorkshop.Experiences/blob/development/README.md)

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>
