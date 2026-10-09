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
