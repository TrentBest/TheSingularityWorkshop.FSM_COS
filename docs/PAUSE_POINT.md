# FSM_COS Pause Point — Kernel First, WebPage Next

> **Status:** proposed engineering pause gate, 2026-10-09. This is not a release approval and does not authorize publishing to NuGet.

**Continuation checklist:** [WebPage Working Task List](https://github.com/TrentBest/WebPage/blob/development/docs/WORKING_TASKS.md) is the durable cross-session handoff. On resumption, read it, verify current branch heads and workflow runs, then continue the highest-priority unfinished task.

## Decision

FSM_COS should stop broadening its scope once the current correctness patch passes CI. The next major block of work belongs in WebPage: prove that the host consumes the composed `RuntimeAssembly` for actual Experience behavior, then prepare the public site for launch.

The purpose of this pause is not to declare the Workshop ecosystem complete. It is to keep the composition kernel small and dependable while we prove its value in the browser.

## The kernel's minimum dependency rule

The current `development` project references only:

- `TheSingularityWorkshop.FSM_API` — foundational state/context contract.
- `TheSingularityWorkshop.MicroBundleDomain` — the authoritative MicroBundle contract.

That is the right starting point for a minimal composition kernel. ProtocolAi, GrammarAi, GUI, rendering, storage, REST, identity, and other capabilities should remain optional unless the kernel itself demonstrably needs their contracts. A package being published does not make it a mandatory runtime dependency.

The test project references AI and GUI packages to prove that capability implementations can be composed without adding those packages to the kernel's assembly references. Keep that separation.

## What the current kernel already owns

The current implementation provides the core composition sequence:

1. Accept a `RuntimeManifest` describing requested root bundle IDs and versions.
2. Resolve root bundles through the host-supplied catalog.
3. Resolve dependency closure before loading each dependent bundle.
4. Apply dependency-provided configuration and allow an external configuration source to override it.
5. Detect dependency cycles and reject unresolved or identity/version-mismatched roots.
6. Load each bundle once and arbitrate until stable, with a configurable maximum round count.
7. Return a `RuntimeAssembly` for the host to consume.

Existing tests cover these contracts, including dependency ordering, configuration, cycle detection, deduplication, arbitration convergence, empty manifests, host lookup, and optional AI/GUI capability composition.

## Final FSM_COS gate before pausing

- [x] CI passed for development head `e721e0b52fecbbd0aaafc84a8c8e5e1e2bd4f440` in [run 37979475866](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/actions/runs/37979475866); the `build-and-test` job succeeded.
- [x] Repeated requests for the same root ID and same version load once (covered by the passing regression suite).
- [x] A manifest requesting the same root ID at conflicting versions fails before any bundle is loaded (covered by the passing regression suite).
- [x] The package workflow still has NuGet publishing disabled by default and explicitly gated by `&& false`; the publish job was skipped in run 37979475866.
- [x] No package version was changed and no NuGet release was made.

**Pause gate passed for the verified commit above.** Pause broad FSM_COS feature work here. Reopen it only for a correctness blocker or a narrowly scoped change required to unblock WebPage; do not hold WebPage hostage to unrelated future capabilities.

## What remains outside this pause

FSM_COS is not a repository service, marketplace, browser host, rendering engine, GUI framework, or general application loop. It does not need to absorb those concerns to be useful.

The broader package/MicroBundle integration map remains a living audit. Candidates must be evaluated by responsibility and runtime need—not by the number of packages already published. Optional capabilities should be represented as MicroBundles where that provides real discovery/configuration/composition value without forcing domain contracts upward into FSM_COS.

## WebPage is the next proving ground

The current WebPage development project references FSM_COS and composes the manifest-declared primary root with its Moniker dependency. However, WebPage's own status documents still identify an important gap: browser execution of the Living GUI passes through transitional WebPage-local `PageFSM` / `LivingGuiFsm` code after composition.

That makes WebPage the next priority. The next phase should:

1. Verify the host manifest and composition against the current package contract.
2. Ensure the host uses the resulting `RuntimeAssembly` as the source of the active Experience, not merely as a startup check before a parallel local runtime takes over.
3. Preserve existing Living GUI behavior with tests before migrating ownership.
4. Keep browser-only presentation and input in WebPage; keep reusable behavior in its owning package/domain or MicroBundle.
5. Run the WebPage build/tests and inspect the public experience at multiple viewport sizes.
6. Publish/deploy the WebPage only after the public launch checks pass; do not publish NuGet packages as part of that step.

The public launch is not blocked on integrating every Workshop package. It is blocked only by issues that prevent a truthful, usable, safe public experience or make the demonstrated architecture materially misleading.

## Status language

- **Current:** implemented in the branch and supported by source/tests.
- **Verified:** the relevant build/test or runtime proof actually passed.
- **Direction:** intended architecture that still needs integration proof.
- **Future:** not part of the present contract.

Do not describe a package or MicroBundle as integrated merely because it exists, is listed in a dependency table, or compiles in a separate repository.


## Updated continuation priority — FSM_COS alpha.6 and Forge boundary (2026-10-09)

This addendum supersedes the older sequencing above until the alpha.6 source reconciliation is complete. Do not resume broad WebPage work yet.

1. Treat `0.1.0-alpha.6` as the intended next FSM_COS package candidate; the development project metadata now declares it. This is not publication authorization.
2. Reconcile `master` and `development` deliberately. Preserve development's requested-version manifest roots and external configuration-source contract; evaluate master-only `FSM_UserIO.SemanticIntent` as an additive capability with tests rather than replacing the newer manifest contract wholesale.
3. Continue reconciling the Forge staged-manifest work. Forge owns authoring and publish-time compilation; FSM_COS consumes the compact published result. The Forge must not become a runtime dependency of FSM_COS, and FSM_COS must not absorb Forge UI, serialization, storage, or repository transport.
4. The current staged metadata can describe immutable identity, dependency edges, and Bootstrap/Resident/Deferred intent, but does not yet implement artifact localization/hash verification, Bootstrap-only initial composition, or live deferred promotion. Do not claim those behaviors until a repository/host adapter and end-to-end tests prove them.
5. Verify dependency-graph agreement between the published manifest and the resolved MicroBundleDomain contract; then prove clean restore/build/test against public package versions without the CI-only local MicroBundleDomain feed.
6. Keep NuGet publication disabled with `&& false`. No publish, branch deletion, or merge of the draft integration PR is authorized by this checklist.

Return to WebPage once the source contract is reconciled, the alpha.6 package candidate passes its release-readiness checks, and the RuntimeAssembly host handoff can be evaluated against that stable candidate. A package publish still requires separate explicit owner approval.
