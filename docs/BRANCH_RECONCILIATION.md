# FSM_COS Branch Reconciliation Ledger

**Owner's directive (2026-10-09):** preserve useful ecosystem work, make it work together, and reduce the repository to exactly `master` and `development`. The owner will delete branches only after being told that a specific branch is clear to delete. When architectural intent is ambiguous, present the evidence and request a directive rather than discarding work or merging blindly.

**Status:** first-pass inventory. No extra branch is yet declared safe to delete. This ledger must be updated as branches are inspected, changes are integrated, and verification passes.

## Mandatory operating rule

See root [AGENTS.md](../AGENTS.md). Do not create more persistent branches. Do not delete branches. Reconcile each branch into `development` where useful and non-conflicting, verify it, then tell the owner when that exact branch is safe for owner-controlled deletion. No NuGet publishing without explicit owner authorization.

## Canonical branches

- `master` — stable/release-ready history.
- `development` — active integration.

## First-pass branch inventory

All comparisons below are against the current `development` ref at the time of the 2026-10-09 audit. “Ahead” means commits reachable from that branch but not from `development`; it does not prove every commit is still unique in meaning. Changed-file lists are initial leads, not a substitute for reading the actual diffs.

| Branch | Ahead / behind | Initial contents indicated by comparison | Initial disposition |
|---|---:|---|---|
| `arbitration-weapon-magic` | 4 / 150 | `ArbitrationContext.cs`; new weapon/magic arbitration guide and tests; README note | Inspect behavior/tests and whether domain-specific example belongs in kernel tests/docs. |
| `architecture/domain-owned-microbundle-contract` | 20 / 110 | Large README/architecture/MicroBundles/development updates; new compute-scale, dependencies, performance docs; workflow and project changes | High-value architecture/doc candidate; inspect current accuracy, dependency direction, and release gate before selectively integrating. |
| `chore/standardize-ci-fsm-cos` | 1 / 71 | Package workflow only | Compare workflow carefully; retain useful CI changes without weakening the disabled NuGet publish gate. |
| `docs/ecosystem-documentation-standard` | 4 / 37 | New documentation index and ecosystem documentation standard; README links | Inspect against current documentation standard/index; likely selective documentation reconciliation. |
| `docs/master-readme-standard` | 55 / 110 | Large README and docs set, compute-scale/dependencies/performance/releasing docs, runtime source/tests, project/workflow changes | Broad mixed branch; do not merge wholesale. Review source/test changes separately from docs and workflow. |
| `docs/readme-standard` | 16 / 71 | README/docs revisions; new FSM_API integration guide; workflow changes | Compare against current docs and other README branches to deduplicate and preserve unique useful guidance. |
| `feature/ai-exchange-foundation` | 4 / 136 | New AI composition guide and SVG; README link | Inspect whether guide describes current optional capability contract accurately. |
| `feature/ai-gui-capability-handoff` | 4 / 133 | AI composition guide edits; new AI/GUI handoff tests and test project reference changes | Inspect tests and package references; preserve optionality and prevent production dependency creep. |
| `feature/runtime-assembly-capability-handoff` | 2 / 135 | RuntimeAssembly API changes and composition tests | Inspect carefully for API value, compatibility, and overlap with current RuntimeAssembly contract. |
| `forge/staged-manifest-load-plan` | 19 / 132 | Staged manifest loading types, evaluator, schedule/load-plan types, tests, design guide, workflow edits | Significant architectural feature. Compare with current manifest design and owner direction before deciding scope; do not silently discard. |
| `release/fsm-cos-alpha3-capability-handoff` | 2 / 134 | README and project/package metadata changes | Inspect version/dependency changes and preserve publish gate; likely release-history context rather than a merge target. |
| `release-prep/fsm-cos-1.0.0` | 24 / 132 | Staged-loading feature plus release guide/SVG, project metadata, workflow changes | Overlaps staged-load branch and includes release-sensitive workflow changes. Reconcile feature/docs separately from package/release configuration. |

## Reconciliation record

For each branch, add:
- relevant commits/files reviewed;
- work already present in `development`;
- useful work selected for integration and resulting commit(s);
- work intentionally not integrated, with a reason;
- build/test/workflow evidence;
- open questions or owner directive required;
- final state: **not reviewed**, **reviewed—work remains**, **integrated—verification pending**, **verified—owner may delete**, or **retained by owner direction**.

A branch may only be marked **verified—owner may delete** when all useful work is preserved or consciously dispositioned, no required work remains only on that branch, relevant verification has passed, and any associated open PR has been checked.

## Evidence links

- [All repository branches](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/branches/all)
- [Compare each branch against development](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/compare/development...master)
- [Open repository issues and design records](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/issues)
- [Pause point and pre-WebPage kernel gate](PAUSE_POINT.md)

## Release safety

This cleanup and integration work does not authorize a NuGet release. Do not publish any package until the owner explicitly says to do so.


## Second-pass findings — pull requests and architectural questions (2026-10-09)

### Pull-request relationships

The following extra branches still back open PRs and cannot be considered cleanup candidates yet:

- `docs/ecosystem-documentation-standard` → [PR #24](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/pull/24), targets `development`.
- `docs/master-readme-standard` → [PR #23](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/pull/23), targets `master`.
- `docs/readme-standard` → [PR #22](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/pull/22), targets `development`.
- `chore/standardize-ci-fsm-cos` → [PR #21](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/pull/21), targets `development`.
- `development` → [draft integration PR #14](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/pull/14), targets `master`; it is not mergeable at the time of review and is explicitly not release approval.

Several other branches have PRs that were merged into `master` or `development` in the past: AI composition (#2), RuntimeAssembly capability handoff (#4), alpha.3 capability handoff (#5), AI/GUI capability handoff (#6), and domain-owned MicroBundle contract (#13 and #15). Their branch tips still need reconciliation against the *current* `development` source because a historical PR merge into one branch does not prove that the same behavior exists on the other branch.

The staged-load-plan PR #9 and release-prep PR #12 are closed without merging, but the staged-load design issue #8 remains open. Therefore the staged-loading work is not safe to classify as simply abandoned.

### Staged manifest loading — significant design choice

The `forge/staged-manifest-load-plan` branch contains:
- `MicroBundleReference` with ID/version/content hash;
- `RuntimeManifestEntry` pairing immutable identity with the existing bundle request and a Resident/Deferred stage;
- `RuntimeManifestLoadPlan` tracking Published → Localized → Loaded;
- `IManifestLoadEvaluator` for Experience-owned deferred promotion;
- `RuntimeManifestSchedule` and dependency-edge types to distinguish broad localization opportunities from dependency-constrained composition;
- tests and `docs/STAGED_MANIFEST_LOADING.md`.

None of those staged-load files currently exists on `development`. The design is host-neutral in intent, but it expands the current simple manifest contract and brings localization/cache lifecycle concepts close to the kernel boundary. The open issue #8 describes an even broader eventual flow with bootstrap/resident/deferred entries, background localization, and launch before full localization. Issue #20 separately calls for repository-backed resolution and deterministic local cache/preservation/update behavior.

The current pause-point document had previously prioritized proving the existing `RuntimeAssembly` handoff in WebPage before broadening FSM_COS. Reconcile these two directions explicitly before integrating the staged-load types into the near-term release. Do not silently merge them, and do not delete their source branch while the owner decides.

### Master/development divergence is substantive

The current `master` `RuntimeAssembly` includes a `SemanticIntent` property and a direct `FSM_UserIO` reference; current `development` does not. The AI/GUI handoff test exists on both branches but uses the different manifest request types matching each branch's MicroBundleDomain contract. This is evidence that some branch content has already landed in one canonical line while the two canonical lines still have materially different public APIs. Review the current `development`→`master` integration PR and dependency contract before declaring the older feature branches reconciled.

### CI workflow branch

The CI-standardization branch changes the workflow substantially. Current `development` builds a separate `MicroBundleDomain` architecture branch into a local package feed before building FSM_COS; the CI branch removes that special checkout and builds against the solution/package graph directly. It also disables publication with `&& false`. Do not take the workflow wholesale until confirming which dependency source is authoritative and that the test/pack path works from a clean checkout.

### Current safe-to-delete status

**No extra branch is cleared for deletion.** Open PR references, master/development API drift, and the unreviewed staged-load design are active reconciliation work. Continue preserving evidence and integrating selectively; the owner alone deletes branches.


## Scheduler / staged-loading branch — active integration review (2026-10-09)

**Owner's priority:** treat the substantial scheduler/load-plan branch as the first capability to reconcile. Preserve useful work and refactor it to fit the current ecosystem; do not delete the branch or merge it mechanically.

Source branch: [`forge/staged-manifest-load-plan`](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/tree/forge/staged-manifest-load-plan). Its closed PR #9 and open design issue #8 remain useful history. The source branch is 19 commits ahead of and 139 behind the current `development` ref in the current comparison; this is a diverged history, not a safe merge candidate.

### Valuable work to preserve

- Immutable `MicroBundleReference` with ID, version, and content hash.
- Explicit `Bootstrap` / `Resident` / `Deferred` stages.
- Distinct `Published` → `Localized` → `Loaded` lifecycle.
- An Experience-owned `IManifestLoadEvaluator`, so the kernel does not hard-code domain-specific reasons to promote deferred capabilities.
- A dependency graph/schedule, plus tests and a readable design document.
- The intended separation between localization/storage and in-memory composition.

### Compatibility findings

The implementation on that branch predates the current `development` contract and must be adapted:

1. Current `RuntimeManifest.Bundles` is `IReadOnlyList<MicroBundleManifestEntry>` (bundle ID + requested version). The old branch instead changes it to `IReadOnlyList<BundleRequest>` (ID + configuration), losing the current explicit root-version contract and bypassing the current configuration-source behavior. Do **not** replace the current contract with the old one.
2. Current `FsmCos.Execute` resolves domain-owned `IMicroBundle` instances through `IMicroBundleCatalog`, traverses `IMicroBundle.Dependencies`, loads dependencies before dependents, and arbitrates to convergence. The staged plan must complement that contract rather than introduce a second competing dependency resolver.
3. FSM_API already owns sequential process-group scheduling. FSM_COS should describe composition dependencies and hand runnable work to the appropriate lower-level scheduler; it must not duplicate FSM_API's process scheduler.
4. The old `RuntimeManifestSchedule` is a dependency-order analysis, not a process scheduler. Its `IsDependencyReady` helper is useful as a predicate, but the current loader does not enforce it; `EvaluatePromotions` alone can return a localized deferred dependent without checking prerequisite readiness. Any integrated promotion path must enforce dependency readiness or explicitly delegate it to the current dependency resolver.
5. The old identity/version/hash model and current ID/version manifest entry need one authoritative representation. Avoid carrying conflicting version/configuration copies that can drift. Content-hash verification belongs at the artifact localization/resolution boundary; FSM_COS should not pretend that carrying a hash verifies bytes.
6. Localization/cache/repository access is not currently implemented by the FSM_COS kernel. Keep that responsibility behind a host/repository-facing abstraction rather than adding storage, network, or cache implementation to the composition package.

### Integration direction

Proceed in small verified slices on `development`:

1. Preserve the old branch as source evidence while reviewing all implementation, tests, docs, workflow edits, and PR/issue context.
2. Adapt the manifest representation without regressing requested versions, opaque configuration, dependency closure, arbitration, or the existing public API.
3. Separate the concepts explicitly: **dependency plan** (what must precede what), **FSM_API scheduling** (when runnable process groups execute), **localization** (where immutable bytes become available), and **FSM_COS composition** (when resolved bundles are loaded/arbitrated into a RuntimeAssembly).
4. Add tests for identity/version consistency, dependency ordering/readiness, resident/deferred promotion, and backward compatibility with current composition behavior. Add integration only when the relevant host/localization boundary exists; do not claim background localization or live deferred loading works before it does.
5. Run the development build/test workflow and inspect its publish gate. NuGet publishing remains disabled unless the owner explicitly authorizes it.

**Disposition:** active review; useful work identified; integration and verification pending; **not safe to delete**.


### First compatible staged-planning slice — integrated, end-to-end loading still pending

The following has now been added to `development` without replacing the current `MicroBundleManifestEntry` contract:

- `MicroBundleReference`, `ManifestLoadStage`, and `RuntimeManifestEntry` record versioned immutable identity and Resident/Deferred intent.
- `RuntimeManifestDependency` and `RuntimeManifestSchedule` represent dependency prerequisites separately from FSM_API process scheduling; the schedule validates entry identity, duplicate edges, unknown dependencies, and cycles.
- `RuntimeManifestLoadPlan` tracks Published → Localized → Loaded. When a schedule is supplied, it rejects loading a dependent entry before its prerequisites are Loaded; promotion evaluation respects both Experience choice and dependency readiness. Bootstrap entries are considered before Resident, then Deferred entries, preserving manifest order within each stage.
- `RuntimeManifest` now accepts optional staged-plan/schedule metadata while retaining the existing versioned root entries. `FsmCos.Execute` validates consistency before resolving or loading any bundles.
- `tests/FSM_COS.Tests/RuntimeManifestPlanningTests.cs` covers current-manifest compatibility, version drift, independent/dependent classification, cycle rejection, dependency-gated promotion, and Bootstrap-first ordering.
- `docs/STAGED_MANIFEST_LOADING.md` documents the responsibilities and explicitly distinguishes implemented planning/state contracts from the not-yet-integrated host localization/deferred execution path; the document is linked from `DOCUMENTATION_INDEX.md`.

Key commits on `development` include planning-contract additions `b4a0ddc`, `84ab6d8`, `527b3fb`; manifest integration `e92c28a`, `c4eafa2`; tests `559124f`, `de78324`; and design/index documentation `0608fdc`, `102a421`.

**Verification evidence:** [GitHub Actions run 37984065276](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/actions/runs/37984065276) completed successfully for commit `de78324a68c8ed5e0c0b0de5987616f2ee598eca`: build, tests, and pack succeeded; the `publish_nuget` job was skipped. Bootstrap-first ordering was then added in commits `1d8fdba`, `52966ea`, and tested in `a5188b2`; the corresponding CI run was still in progress when this ledger update was prepared.

**Not yet implemented:** asynchronous repository/cache localization, content-hash verification against actual bytes, bootstrap entry while localization continues, and live deferred promotion into an existing RuntimeAssembly. The current `FsmCos.Execute` behavior remains the composition path. These require a host/localization integration contract and end-to-end tests; the presence of planning types alone is not proof of staged execution.

**Branch disposition remains:** active integration; more work remains; **`forge/staged-manifest-load-plan` is not safe to delete**.


### Related release-prep branch comparison

The `release-prep/fsm-cos-1.0.0` branch is not a replacement for the staged-load branch. Comparing it directly with `forge/staged-manifest-load-plan` shows the staged-load implementation is shared, while release-prep adds a release contract, README material, a release-frontier SVG, project/package metadata, and a different workflow.

Disposition of its unique work:

- **Preserve for later adaptation:** `docs/RELEASE_1_0.md` and `docs/assets/release-frontier.svg`. The release checklist and contract-stabilization rationale are useful, but the dependency diagram and text currently name MicroBundleDomain 1.0.0 while `development` uses 1.0.1; revise against the actual release candidate before using it.
- **Do not copy its project metadata yet:** it changes the package from the current alpha version to `1.0.0` and pins MicroBundleDomain 1.0.0. That is release-sensitive and not authorized by this task.
- **Do not copy its workflow:** the release-prep workflow permits NuGet publishing on a manually dispatched run when the input is true, without the repository's mandatory `&& false` safety latch. The owner must explicitly authorize any future temporary publish-gate change, and the gate must return to disabled afterward.
- **Do not merge the branch wholesale:** it contains the staged-load implementation already being adapted plus unrelated release-only changes.

The release-prep branch remains **not safe to delete** until the release guide/visual are adapted or consciously dispositioned, the workflow/project changes are resolved, and its open/closed PR history is accounted for.


### Forge handoff and alpha.6 compatibility assessment (2026-10-09)

The Forge's durable handoff/TODO belongs in the Forge repository; FSM_COS keeps only the runtime contract and integration acceptance criteria. The current source-level assessment in `docs/DEPENDENCY_ALIGNMENT_CHECKLIST.md` is consistent with the staged-load design and is the working checklist for the next release candidate.

**What FSM_COS already offers to the Forge:**
- Versioned manifest roots via `MicroBundleManifestEntry` (bundle ID + requested version).
- An optional host-owned `IMicroBundleConfigurationSource`; configuration remains external, and missing configuration means bundle defaults.
- Catalog resolution, dependency closure, cycle detection, bounded arbitration, and a `RuntimeAssembly` handoff.
- Optional staged identity/schedule metadata that can describe immutable artifact identity, dependency prerequisites, and Bootstrap/Resident/Deferred intent without adding a Forge, repository, storage, serializer, GUI, or host dependency to the kernel.

**What is not yet an end-to-end capability:** FSM_COS does not yet localize/verify repository bytes, compose only a Bootstrap subset, or promote Deferred bundles into an already-running assembly. The staged-plan types validate/describe state; they do not implement the three concurrent Forge/runtime clocks. Do not let the Forge pretend those operations exist just because the metadata types compile.

**Compatibility rule:** the Forge must compile its rich authoring model into the published machine-oriented manifest. It must migrate from the older `BundleRequest`/inline-configuration shape to versioned root entries plus its own configuration source. FSM_COS should not import Forge authoring concepts or serialization/storage code to ease that migration.

**Release target correction:** the next intended package is `0.1.0-alpha.6`, not alpha.5. The development project metadata now declares alpha.6. This is a candidate version only; branch/API reconciliation, clean public-package consumer verification, and explicit owner publication approval remain required. Keep the NuGet publish condition hard-disabled with `&& false`.

**Next order of work:** (1) finish reconciling master-only intent support against the development versioned-manifest/configuration contract; (2) test published dependency-graph consistency against resolved MicroBundleDomain dependencies; (3) define the host/repository localization boundary and prove it with a real adapter; (4) only then implement Bootstrap-first composition and deferred promotion end-to-end. Keep the Forge and release-prep branches until all unique work is preserved and verified.

**Disposition:** Forge-facing contract mostly exists; consumer migration and staged runtime execution remain open; alpha.6 is not cleared for publication.


### Alpha.6 candidate verification and master release-line blocker (2026-10-09)

**Verified code/package candidate:** [`77c05d68d70159e6f4ffb53bd2209a5ed6ed0abc`](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/commit/77c05d68d70159e6f4ffb53bd2209a5ed6ed0abc) on `development`. [Actions run 37988552679](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/actions/runs/37988552679) completed successfully for `build-and-test` and `public-package-compatibility`; `publish_nuget` was skipped. The run produced both local-dependency and public-feed package artifacts. The current development ref is three documentation-only commits beyond this code candidate.

**Release is not yet ready to trigger.** The branch comparison currently reports `development` 172 commits ahead / 40 behind `master` (candidate commit 77 was 169 ahead / 40 behind). Both project files declare `0.1.0-alpha.6`, but the public API differs: development has versioned manifest roots, external configuration source, and staged schedule validation; master still uses the older inline-configuration `MicroBundleDependencyRequest` manifest. Master’s `package.yml` also lacks the separate clean public-NuGet compatibility job present on development. Publishing either branch as-is risks the immutable alpha.6 version representing different APIs.

**Required disposition:** keep PR #14 draft and reconcile selectively. Port the verified development runtime contract, its regression tests, release notes, and public-feed compatibility workflow into the master release line without wholesale replacement of master-only documentation or loss of unique branch work. Re-run the full candidate workflow on the exact master candidate and inspect the resulting package before the final owner review. The publish condition remains hard-disabled with `&& false`; no branch has been declared safe to delete, no PR merged, and no package published.


### Selective master alpha.6 candidate staged for CI (2026-10-09)

Temporary branch `release/fsm-cos-alpha6-candidate` was created from `master` and opened as draft [PR #25](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/pull/25). It is 28 commits ahead / 0 behind master and selectively ports the tested development manifest/configuration contract, optional semantic intent, staged graph validation, regression tests, release notes, focused consumer documentation, and public-package compatibility workflow. This avoids merging the whole 172-ahead/40-behind development line.

PR #25 is now open, ready for review, mergeable, and unmerged. Its exact head `385bec07588185d7cc98471db1f1015e9da69e5c` passed [Actions run 37992111115](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/actions/runs/37992111115): both build/test/coverage/pack and public-feed compatibility succeeded, and the publish job was skipped. The package artifact was inspected: alpha.6 version/dependencies, README, license, release notes, and internal-doc exclusions are correct; the TRX artifact reports 39/39 passing tests. The .NET build reported zero compiler warnings/errors.

**Next:** owner review and approval to merge PR #25 into master. After merge, verify the exact master-head workflow and artifact before any separate publication approval. Keep the temporary branch until its unique work is accepted or explicitly dispositioned; it is not safe to delete now.


### Current release and branch state re-check (2026-10-11)

This section supersedes the *current-state* statements above where GitHub has since changed. Earlier commit counts and PR states remain useful as historical snapshots, not as present-tense facts.

**Observed repository state:**

- PR [#14](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/pull/14) remains open and is explicitly a draft integration checkpoint from `development` into `master`. It is not a narrow fix. The current comparison reports `development` **297 commits ahead and 40 behind** `master`.
- PR [#25](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/pull/25), the selective alpha.6 candidate, was **closed on 2026-10-10 without merging**. Its temporary branch `release/fsm-cos-alpha6-candidate` no longer appears among repository branches. Do not describe that PR as open or reviewable.
- The verified candidate commit `385bec07588185d7cc98471db1f1015e9da69e5c` still exists and compares as **33 commits ahead / 0 behind** `master`. It is recoverable source evidence, not the current `master` head and not proof that its changes were integrated.
- Both `master` and `development` project files currently declare `0.1.0-alpha.6`, but they do **not** expose the same public manifest contract. On `master`, `RuntimeManifest.Bundles` is still `IReadOnlyList<MicroBundleDependencyRequest>`; on `development`, it is `IReadOnlyList<MicroBundleManifestEntry>` with explicit requested versions plus optional staged-plan/schedule metadata. The development contract also supports external configuration-source resolution. The shared version string therefore does not make these source lines interchangeable.
- The `master` package workflow has the normal build/test/pack job and a hard-disabled publication job, but does not include the separate clean public-NuGet compatibility job present on `development`. Keep the publication condition hard-disabled with `&& false`.

**Evidence and caveats for the selective candidate:**

The recorded Actions run [37992111115](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/actions/runs/37992111115) succeeded for both build/test/pack and public-feed compatibility at the exact candidate head; publication was skipped. The recorded artifact inspection and 39/39 test result are useful evidence for that commit, but they do not validate later commits or the current `master` head.

Codecov's PR #25 report separately recorded **69.43% patch coverage, with 96 changed lines not covered**. The main uncovered areas included `FsmCos.cs`, `RuntimeManifest.cs`, `RuntimeManifestSchedule.cs`, `RuntimeManifestLoadPlan.cs`, and `MicroBundleReference.cs`. A green build is not the same as complete branch/edge coverage. Before treating the candidate as review-ready, inspect those gaps and add tests where they protect meaningful public contracts or failure paths; document any deliberately untested defensive code rather than optimizing for a percentage alone.

**Disposition:**

- Do **not** merge `master` wholesale into `development`, or `development` wholesale into `master`, merely to resolve the divergent history.
- Do **not** treat PR #14 as a release-ready integration without a selective contract review.
- The selective candidate's verified commit can be used to reconstruct a review branch, but its PR was closed and its branch deleted; restoring a review surface is a separate action, not an implicit merge.
- Alpha.6 remains a source candidate, not publication authorization. Before release, reconcile the public contract on the intended release line, close the meaningful coverage gaps, rerun clean public-feed compatibility against the exact release commit, inspect the produced package, and obtain explicit owner approval for any merge or publication.

