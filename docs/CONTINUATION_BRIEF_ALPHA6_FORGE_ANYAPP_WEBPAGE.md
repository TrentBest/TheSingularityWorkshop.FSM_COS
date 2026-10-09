# Continuation Brief: FSM_COS alpha.6, Forge, AnyApp, and WebPage

> **Last updated:** 2026-10-09  
> **Purpose:** Reset/priming document for continuing the Workshop ecosystem integration without losing the release target, architectural boundaries, current evidence, or safety rules.  
> **Status:** Work-in-progress checklist. It is not a release approval, merge instruction, or claim that every listed integration is complete.

## Immediate objective

Prepare and verify the next FSM_COS release, currently targeted as **0.1.0-alpha.6**, then align the Forge and AnyApp consumers to the released package and prove their real runtime paths. The user has confirmed alpha.6 is the intended next version and has asked another LLM to make the version update; the source version bump is also recorded below. Treat the source/repo state as authoritative and re-check it after any parallel LLM work lands.

The goal is to preserve and connect useful existing work—not rewrite working branches or add host-specific features to FSM_COS without a failing test that proves a missing kernel contract.

## Non-negotiable guardrails

- **Do not publish any NuGet package without explicit final approval.** “We plan to publish today” is a target, not permission to execute publication without the final release review.
- Keep every NuGet publish job explicitly disabled by default with `&& false`. Only change this after explicit approval for a specific publication, and restore the safeguard immediately afterward.
- Do not merge PRs, close useful branches, or rebase/overwrite branches merely to simplify the audit.
- Keep changes small, reviewable, and on the existing intended branch. Prefer `master` as stable and `development` as integration; avoid unnecessary branches.
- Report exact repo, branch, commit, PR, and verified CI result. Distinguish source inspection, documentation, local tests, CI tests, package availability, and actual end-to-end execution.
- Do not silently target an unpublished package version in a consumer project. Update consumers only when the actual package is available or a clearly isolated source-based integration test is intentionally being used.
- Keep FSM_COS platform-neutral: no Forge UI/files, WPF, browser lifecycle, GUI/rendering, repository transport, REST, storage, or scheduling responsibilities in the composition kernel unless a demonstrated cross-host contract truly belongs there.
- Lower-level packages must not depend upward on FSM_COS.
- Avoid changing unrelated APIs during the release window.

## Current FSM_COS release candidate

Repository: [TheSingularityWorkshop.FSM_COS](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS)  
Working branch: `development`  
Open PR: [#14](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/pull/14)

Known candidate commits:
- [`3610890a470f86eee7f83b781ee9de143f16300f`](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/commit/3610890a470f86eee7f83b781ee9de143f16300f) — set project version from alpha.5 to alpha.6.
- [`e2b846cbc80c1b400d729cb1bfc0548172bc8ded`](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/commit/e2b846cbc80c1b400d729cb1bfc0548172bc8ded) — document package/source version drift.
- [`aca0cecbc5c21c3c7fdfa40851411c56431ec4d9`](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/commit/aca0cecbc5c21c3c7fdfa40851411c56431ec4d9) — record alpha.6 candidate correction and Forge/AnyApp readiness in the checklist.

Primary live checklist: [docs/DEPENDENCY_ALIGNMENT_CHECKLIST.md](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/docs/DEPENDENCY_ALIGNMENT_CHECKLIST.md).

**Verification warning:** when this brief was written, the workflow lookup had not confirmed a CI run for the alpha.6 version-bump commit. Do not report candidate CI as green unless a run on the exact current candidate commit proves it. Re-check the current `development` HEAD first because parallel work may have added commits after this snapshot.

### FSM_COS contract in development source

Source inspection found these contracts already present:
- `RuntimeManifest(RuntimeId, Bundles, ExperienceContext)`.
- `MicroBundleManifestEntry(BundleId, Version)` for versioned root requests.
- `FsmCos.Execute(manifest, configurationSource)`; configuration source is optional.
- `IMicroBundleConfigurationSource.TryGetConfiguration(runtimeId, bundleId, version, out bytes)`; no supplied configuration means the bundle's defaults apply.
- Version-aware catalog/dependency resolution, bounded arbitration, and a returned `RuntimeAssembly`.
- Repeated roots with the same ID/version are loaded once; conflicting requested versions for one root identity are rejected during preflight before loading.

The likely release gap is **verification and consumer migration**, not a new Forge-/AnyApp-specific kernel API. Keep the older master intent-contract divergence in mind: earlier source comparison found `SemanticIntent` in master `RuntimeManifest`/`RuntimeAssembly`, while development's composition/configuration contract differs. Reassess whether intent is still a desired contract; do not remove FSM_UserIO or claim the dependency is unused based on metadata alone. Do not casually pull a master-only contract into the release candidate without tests and a deliberate decision.

## Ordered todo list

### P0 — Finish the alpha.6 candidate (do first)

**Verification checkpoint (2026-10-09):** candidate code/package commit `0319307831c431fed250063a5f53744309b09220` passed the full `build-and-test` job (restore, build, tests/coverage, pack) and the separate `public-package-compatibility` job (public NuGet restore/build/test/pack). The publish job was skipped. The public-feed package artifact was inspected: `0.1.0-alpha.6`, README and license included, dependencies FSM_API 1.0.13 + FSM_UserIO 0.1.0-alpha.1 + MicroBundleDomain 1.0.1; release notes included; internal branch/continuation planning docs excluded. The publish gate still contains `&& false`. Documentation-only commits followed this code/package verification; confirm their latest workflow finishes before finalizing the candidate.



Recent work on `development`: optional semantic-intent pass-through and pre-load schedule-vs-resolved-graph validation have been implemented and tested in source; the exact-commit workflow and public-feed dependency verification remain mandatory.

- [ ] Re-read current `development` HEAD and project version; reconcile any parallel LLM changes without overwriting them.
- [ ] Confirm `src/FSM_COS/FSM_COS.csproj` declares `0.1.0-alpha.6`, and that release notes, package metadata, and the intended package contents agree.
- [ ] Confirm `.github/workflows/package.yml` publish job still has the explicit `&& false` safeguard.
- [ ] Obtain a passing build/test/pack workflow on the exact candidate commit. If no workflow triggered, investigate/trigger the verification workflow using supported repository tooling; do not infer success from older runs.
- [ ] Add or confirm tests for: versioned root manifest execution; expected bundle present in `RuntimeAssembly`; configuration source receives the correct runtime ID, bundle ID, version and bytes; absent config uses defaults; duplicate same-version roots load once; conflicting-version roots fail before loading.
- [x] When `RuntimeManifest.Schedule` is supplied, preflight the resolved dependency closure against scheduled bundle IDs, resolved versions, and exact dependency edges before any bundle `Load`; tests cover a matching graph and mismatch rejection without load side effects.
- [ ] Inspect the packed artifact/version and package contents if CI exposes an artifact. Distinguish pack success from NuGet publication.
- [ ] Check README/release notes and candidate diff for unrelated changes and API claims that are not tested.
- [ ] Present the exact candidate commit, verified CI evidence, known caveats, and package version for final human review.
- [ ] **Wait for explicit final publication approval.** Until then, no publish, no temporary gate enablement.

### P1 — Align AnyApp after alpha.6 is actually available

Repository: [AnyApp](https://github.com/TrentBest/AnyApp)  
Known current development pin: FSM_COS `0.1.0-alpha.5`.

- [ ] After the released alpha.6 package is available, update the FSM_COS package pin in a dedicated consumer change.
- [ ] Run the Windows restore/build/test path and record the exact commit and result.
- [ ] Prove the repository-backed artifact path: resolve the requested artifact, verify identity/hash, materialize an `IMicroBundle`, compose through FSM_COS, and hand the resulting `RuntimeAssembly` to GUI.WPF.
- [ ] Test the local compatibility catalog/legacy-manifest path too; keep it only where it has a defined compatibility purpose.
- [ ] Verify the canonical Moniker MicroBundle/startup experience and avoid hard-coded duplicate moniker behavior in hosts where the manifest should choose the experience.
- [ ] Keep the installed/signed desktop app as the production path; do not depend on running a downloaded executable directly in a browser.
- [ ] Keep this migration separate from FSM_COS package preparation. No merge without review.

### P1 — Migrate The Forge after alpha.6 is actually available

Repository: [TheForge](https://github.com/TrentBest/TheForge)  
Working branch: `forge/native-experience-authoring`  
Open PR: [#9](https://github.com/TrentBest/TheForge/pull/9)  
Latest previously verified commit: [`ca8283b67266d30580a681fdedde1dc6066bb24d`](https://github.com/TrentBest/TheForge/commit/ca8283b67266d30580a681fdedde1dc6066bb24d). Its CI run [37982103784](https://github.com/TrentBest/TheForge/actions/runs/37982103784) passed build/test for that commit; re-check before relying on it after new changes.

- [ ] After alpha.6 is available, update Forge's FSM_COS pin from alpha.3 in a dedicated change.
- [ ] Migrate `ForgeExperience.Compile()` from old `BundleRequest` roots with inline configuration bytes to versioned `MicroBundleManifestEntry` roots and a Forge-owned `IMicroBundleConfigurationSource`.
- [ ] Keep the authored configuration document, validation/editor model, and persistence responsibilities in Forge; do not move Forge serialization or file concerns into FSM_COS.
- [ ] Test versioned manifest compilation, config lookup scoped to runtime/bundle/version, absent-config defaults, duplicate root handling, conflicting-version rejection, and runtime handoff.
- [ ] Mark typed editable-value tree, validation pipeline, domain codecs, durable source persistence, revision-scoped live preview, and publication as future work unless implemented and tested.
- [ ] Re-run Forge CI and record exact commit/results. Do not merge PR #9 without review.

### P1 — Prove WebPage's runtime path separately

Repository: [TheSingularityWorkshop.WebPage](https://github.com/TrentBest/TheSingularityWorkshop.WebPage) (confirm the exact repository name and current branch in GitHub before acting; do not guess if it resolves differently).

- [ ] Keep WebPage as the browser proving ground, but verify actual source and branch before modifying.
- [ ] Trace manifest → FSM_COS `RuntimeAssembly` → active Experience/GUI path.
- [ ] Identify remaining host-local `PageFSM` / `LivingGuiFsm` behavior and determine which parts are transitional versus intentional host presentation.
- [ ] Move only genuinely shared runtime composition/behavior into the appropriate lower-level package; do not make FSM_COS a GUI/layout engine.
- [ ] Preserve the desired manifest-driven hub and AnyApp handoff, canonical Moniker, and the one-hub-first priority. Do not reintroduce duplicate labels or static hard-coded feature menus.
- [ ] Build/run the real browser path and record observed behavior rather than treating docs or a passing compile as proof of integration.

### P2 — Continue ecosystem dependency reconciliation after the release is safe

Use [DEPENDENCY_ALIGNMENT_CHECKLIST.md](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/docs/DEPENDENCY_ALIGNMENT_CHECKLIST.md) as the detailed inventory; it includes prior branch/source findings. Do not try to solve every row before alpha.6 is verified.

- [ ] Check the optional `MicroBundleRepository.FSM_COS` adapter, which was observed pinning alpha.5, against the actual released contract. Prefer repository-provider-neutral Core contracts so the adapter does not require REST if Local/Azure/other providers are used.
- [ ] Reconcile `MicroBundleRepository` master/development divergence carefully; preserve complementary Local, Azure, REST, CLI, materialization-adapter, and publisher smoke-test work rather than choosing a branch wholesale.
- [ ] Investigate Core boundary: artifact address/identity, immutable bytes and hash verification should stay provider-neutral; serialization/envelope and FSM_COS materialization should live in explicit adapter/payload boundaries.
- [x] Resolve FSM_COS master-only `SemanticIntent` versus development versioned configuration contract additively: `RuntimeManifest.Intent` passes through to `RuntimeAssembly.Intent`; versioned roots and external configuration remain intact.
- [ ] Check MicroBundleRepository.Core / REST / adapter package pins against actual published versions before changing them.
- [ ] Keep Profiles provider-neutral and separate from FSM_COS unless a tested optional MicroBundle/adapter is designed; do not call it integrated merely because its domain package exists.
- [ ] Confirm MicroBundleIngestor stays an authoring/ingestion tool, not a runtime dependency; optional REST should remain opt-in.
- [ ] Preserve FSM_API's planned 2.0.0 string/integer backing work as a separate project; do not let it expand this alpha.6 release scope.
- [ ] Keep dependency closure lean: no accidental Azure, REST, GUI, WPF, rendering, storage, or unrelated domain dependencies in minimal hosts.

### P3 — Repository documentation and release hygiene

- [ ] Keep numbered topic taxonomy and visual/color guide consistent without deleting project-specific explanations.
- [ ] Make docs distinguish implemented behavior, intended architecture, and future work.
- [ ] Audit all package-producing workflows and branch variants; every default publish path must remain explicitly disabled with `&& false`.
- [ ] Keep branch-specific PRs and CI facts current. Do not assume master/development parity.
- [ ] Update this brief with completed checkboxes, exact commit IDs, links, CI results, blockers, and next action each time work changes materially.

## Ownership boundaries to preserve

- **FSM_API:** finite-state-machine primitives/runtime.
- **FSM_COS:** host-neutral composition boundary: manifest, bundle resolution/loading, dependency closure, arbitration, runtime assembly.
- **MicroBundleDomain:** domain-neutral MicroBundle contracts/metadata.
- **MicroBundleRepository:** artifact identity and retrieval/storage provider boundaries; storage/transport adapters optional.
- **Forge:** experience authoring, editable configuration, validation, persistence and compilation into runtime-facing descriptors/config sources.
- **AnyApp:** desktop host, repository-backed resolution/materialization integration, GUI.WPF manifestation and browser handoff.
- **WebPage:** browser/public experience and host presentation, using the same core composition path rather than duplicating runtime behavior.
- **Experiences (e.g. Moniker/Elements):** domain capabilities as loadable MicroBundles when their contracts support it; avoid turning every infrastructure NuGet into a MicroBundle.

## Definition of done for the alpha.6 release

1. Candidate version is unambiguous and source/release notes/package metadata agree.
2. Exact candidate commit has verified build, tests, and pack.
3. Critical manifest/configuration/version regression tests pass.
4. Publish workflow is still disabled until final explicit approval.
5. The user receives an accurate release review with links and any remaining caveats.
6. After actual package availability, AnyApp and Forge are migrated in separate commits with passing consumer tests; WebPage's real runtime path is tracked independently.
7. No PR is merged and no package is published without the required approval.

## Resume instruction

On reset, start by checking this document and the live FSM_COS development HEAD, then the current CI status for that exact SHA. Do **not** begin by re-auditing every repository. Finish P0 first; once the release candidate is verified, proceed to the separate consumer migrations in the order above.