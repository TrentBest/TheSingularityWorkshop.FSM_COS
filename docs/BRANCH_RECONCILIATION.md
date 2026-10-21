# Archived Branch Reconciliation Ledger

> **Archive status:** point-in-time branch inventory from October 9, 2026. The branch names, ahead/behind counts, pull-request states, and proposed dispositions below were observations at that date; they are not a current repository inventory or deletion instruction.

## Current authorities

- [Development and CI modes](DEVELOPMENT.md) — current branch policy.
- [Alpha.7 release-readiness gate](0.1.0-alpha.7-READINESS.md) — exact-candidate release evidence.
- [Domain completion roadmap](DOMAIN_COMPLETION_ROADMAP.md) — kernel ownership and deferred capabilities.
- [Documentation index](../DOCUMENTATION_INDEX.md) — primary document for each concern.

## Durable reconciliation rules

- Keep `master` as the stable line and `development` as the integration line unless the owner directs otherwise.
- Preserve useful source, tests, and documentation; do not merge divergent histories wholesale merely to simplify the branch list.
- Do not delete branches, close useful pull requests, or overwrite history without explicit owner direction.
- A branch's commit count, successful build, or source diff does not establish that its unique behavior is still needed or that a release is ready.
- Verify current branch heads and exact-commit workflows before acting. Historical run links prove only the commits they actually tested.
- NuGet publication remains a separate action requiring explicit owner authorization.

## Historical inventory

The original inventory and detailed branch-by-branch rationale remain in repository history. It was intentionally removed from this working document because its dated counts and instructions were repeatedly mistaken for current state. Rebuild an inventory from live GitHub refs only when a current cross-repository branch reconciliation is explicitly in scope.


## Live Operation Sith audit — 2026-10-21

This section supersedes the archived October 9 inventory above for the six named working branches. It is a point-in-time observation of GitHub refs on October 21, 2026. It is **not** a deletion authorization. Owner's target is exactly `master` and `development`; the owner performs final branch deletion after reconciliation is confirmed.

### Live branch inventory

Eight branches were returned by GitHub:
- Retain: `master`, `development`.
- Reconcile before manual deletion: the six branches below.

| Branch | Ahead of `development` | Behind `development` | Unique diff visible against `development` | Disposition |
|---|---:|---:|---|---|
| `docs/alpha7-release-readiness` | 4 | 310 | `docs/0.1.0-alpha.7-READINESS.md` (144 added lines), plus small index/README edits | Preserve/review readiness content against current canonical gate before marking harvested. |
| `docs/fsm-cos-visual-contract-audit` | 5 | 37 | `docs/LIVING_DOCUMENTATION_AUDIT.md` (57 additions, 4 deletions), `docs/assets/fsm-cos-overview.svg` | Preserve/review audit findings and SVG changes against current files. |
| `work/alpha7-living-docs-integration` | 97 | 33 | Broad integration: package workflow, documentation standard/index, `GUILLM.md`, readiness, architecture/runtime contracts, reconciliation ledger, execution sequence, living docs, UML companion pages, SVGs, sample generator, package verifier, and source docs | Highest-risk branch. Do not wholesale merge; perform file-level harvest/reconciliation and retain master-only publication protections. |
| `work/ci-development-dependencies` | 2 | 340 | `.github/workflows/package.yml` (37 additions, 8 deletions), `docs/DEVELOPMENT.md` (16 additions) | Review workflow semantics against both canonical branch protections before harvesting. |
| `work/fsm-cos-living-svg-flow` | 8 | 37 | `docs/LIVING_SVG.md`, preflight SVG, documentation generator, small index/package README edits | Reconcile with integration branch and current generator/source so overlapping changes are not duplicated. |
| `work/fsm-cos-uml-class-diagrams` | 11 | 37 | Architecture, arbitration, runtime assembly/manifest/staged-loading contract documentation | Reconcile with integrated docs and verify each diagram against current source. |

Comparison metadata: the six branches are all reported as diverged from `development`; no branch should be treated as a safe fast-forward cleanup candidate based on these comparisons. Ahead/behind counts are relative to the live `development` head observed during this audit and will change as commits land.

### Canonical branch divergence and release guard

The current compare reported `development` as 661 commits ahead and 48 behind `master` (merge base `e9c9968b9cdbec0478e77d9f89b4e74a0e404d3d`). This is substantial divergence; no wholesale merge or reset is authorized. In particular, reconcile `master`-specific release history, public-dependency compatibility, and the master-only manual publication guard before any alpha.7 release integration.

Open PRs observed on October 21 include:
- [PR #35 — Draft release integration](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/pull/35): explicitly draft; preserve the `master`-only publication safeguard and reconcile master-specific history before integration.
- [PR #34 — Living documentation and source-derived diagrams](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/pull/34): useful docs/UML work, but its description says current exact-head workflow verification is missing.
- [PR #33 — Reduced-motion living SVG](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/pull/33), [PR #32 — UML contract diagrams](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/pull/32), and [PR #31 — Overview handoff correction](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/pull/31): review their latest head and overlap before deciding which are already represented in `development`.

### Deletion gate

**Status: NOT READY for manual deletion yet.** This is a reconciliation checkpoint, not a declaration that unique work has been harvested. Before the owner deletes any of the six branches:

1. Compare current branch heads and PR heads again; these numbers are point-in-time.
2. Confirm the alpha.7 readiness gate and living-documentation/UML/SVG changes are fully represented on the intended canonical branch, with no unique content left behind.
3. Inspect package workflow changes side by side on `master` and `development`; preserve the rule that publishing requires deliberate manual dispatch from `master` and explicit owner authorization.
4. Run/check CI against the exact final `development` candidate and separately verify any intended `master` candidate. Historical green runs do not verify later commits.
5. Update this ledger with harvested commit/file evidence and the final per-branch disposition. Only then report branches as ready for the owner's manual deletion.

No branch was deleted, no PR was merged or closed, no canonical ref was force-updated, and no package was published as part of this audit.


## Follow-up workflow inspection — 2026-10-21

A direct fetch of the live workflow files adds an important qualification to the PR descriptions:

- On `master`, `.github/workflows/package.yml` currently defines the publish condition as `github.event_name == 'workflow_dispatch' && inputs.publish == true && false`. This is **hard-disabled**, so the workflow as currently committed cannot publish even on an authorized manual dispatch.
- On `development`, the condition is `github.event_name == 'workflow_dispatch' && inputs.publish == true && github.ref == 'refs/heads/master' && false`. This is also **hard-disabled**, and additionally includes the explicit `refs/heads/master` guard.
- The development workflow has additional source-integration, GUI.SVG reproducibility, package verification, dependency audit, analyzer, and minimal-consumer jobs. The master workflow differs materially; neither file should be copied wholesale over the other.
- PR #35's statement that the current master workflow permits publishing on manual dispatch is not consistent with the live master file fetched during this audit. Treat the actual workflow at the current branch head as authoritative and resolve this discrepancy before any release integration.
- Do not remove the hard-disable or enable publishing as part of branch cleanup. Any future change to the release gate must be a deliberate, separately reviewed owner-approved action.

The ledger update itself changed `development` after the readiness checklist's previous candidate evidence. Therefore, even if that checklist contains checked gates from a prior exact candidate, the current `development` HEAD must be re-verified before being described as the verified release candidate.


## Follow-up branch-by-branch verification — 2026-10-21

A fresh live compare against `development` confirms the same six surplus branches remain. Current ahead/behind counts are not deletion criteria; all six remain diverged. Open PRs #31–#35 are still visible in the repository search, and none was merged or closed during this pass.

### Verified SVG-flow gap (do not call harvested yet)

The living-SVG work is **not fully represented on `development`** at this point:

- Fetching `docs/LIVING_SVG.md` from `development` returns not found, while the file exists on `work/fsm-cos-living-svg-flow` and `work/alpha7-living-docs-integration`.
- The current development generator `samples/DocumentationVisuals/Program.cs` does not include the `System.Xml.Linq` post-processing used by the SVG-flow branch to add the connector stylesheet.
- The current development asset `docs/assets/fsm-cos-preflight.svg` has no stylesheet, animation, or `prefers-reduced-motion` rule. The SVG-flow branch asset does include CSS animation and a reduced-motion fallback.
- The branch documentation explicitly labels the animation as explanatory (not runtime telemetry) and says browser/assistive-technology verification remains open.

This is a concrete, bounded harvest candidate, but it must be integrated as a **single source/asset/documentation change**: generator, generated SVG, living-SVG explanation, and documentation index/package boundaries must agree. Do not copy only the SVG or claim the feature is integrated until the pinned GUI renderer can reproduce it and exact-head CI verifies it. PR #33 is relevant, but its latest head/check status must be checked before choosing the safest integration path.

### Workflow branch disposition

The fresh compare still shows `work/ci-development-dependencies` changes to `.github/workflows/package.yml` and `docs/DEVELOPMENT.md`. Its publish condition differs from the canonical development workflow and points at `refs/heads/development` (still hard-disabled by `&& false`). It is not safe to harvest the workflow wholesale; preserve the current source-integration/package-verification jobs and do not enable publication.

### Current gate

**Manual deletion remains NOT READY.** No new files have been marked harvested, because the living-SVG implementation needs a reproducible multi-file integration and the remaining divergent branches still require per-file reconciliation. No branch deletion, PR merge/closure, canonical ref rewrite, or package publication was performed in this pass.


### Exact-head check on the living-SVG PR — 2026-10-21

- PR #33 remains **open and draft**, based on `development`, with head `1595e1b769b80b381e8881bed0acaf3187bdaaff`.
- GitHub reports successful completed workflow runs for that exact PR head: Pack and publish run [39364227302](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/actions/runs/39364227302) and CodeQL run [39364227304](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/actions/runs/39364227304).
- This verifies CI for the PR head only. It does **not** mean the changes are on `development`, and it does not close the separately stated browser, reduced-motion, hosted rendering, or assistive-technology verification gaps.
- The live `development` HEAD after this ledger update has no reported commit statuses or PR-triggered workflow runs for its new ledger commit. The current development candidate is therefore not currently verified by this check.
- I have not merged or closed PR #33. The SVG generator, generated asset, explanatory document, and index still need to be reconciled as one source-consistent change before that branch can be marked harvested.


## Content-level comparison pass — 2026-10-21

The following comparisons were made against the live `development` files, rather than treating branch-level ahead/behind counts as proof of missing content.

| Surplus branch | Content-level result | Disposition |
|---|---|---|
| `docs/alpha7-release-readiness` | Its older readiness checklist (9,761 chars) is superseded by the current exact-HEAD evidence gate on `development` (10,633 chars). Its README/index variants are also older and omit current Gallery, UML map, and domain-roadmap navigation already present on `development`. | Do not copy these files over current versions. Inspect any isolated checklist requirement not represented in the current gate before clearing the branch. |
| `docs/fsm-cos-visual-contract-audit` | The living-documentation audit on this branch is 22,298 chars versus 14,630 chars on `development`. It adds measurable inventory/UML coverage, per-document visual decisions, and additional source-review notes. Its overview SVG differs in two connector paths; the current development version already uses a direct host-handoff path, so the branch SVG is not a safe replacement without diagram-contract review. | Audit document is a high-value, bounded harvest candidate. Merge its genuinely new sections into the current audit only after reconciling stale inventory counts (31 Markdown documents, 13 top-level SVGs, and 16 source files are snapshots, not live truths). Preserve the current document's more up-to-date visual decisions. Do not replace the overview SVG wholesale. |
| `work/alpha7-living-docs-integration` | `docs/UML/README.md` and `GUILLM.md` are substantial unique artifacts; `docs/EXECUTION_SEQUENCE.md` is source-derived. Its readiness checklist and documentation index are not automatically preferred over current development versions. The branch's `docs/LIVING_DOCUMENTATION.md` path returned 404, so it is not a reliable file candidate by that name. | Review individual UML companion pages and GUILLM coordination guidance for source accuracy and cross-repo staleness; no bulk merge. |
| `work/ci-development-dependencies` | Its `docs/DEVELOPMENT.md` is shorter (7,510 chars) than the current development version (10,002 chars) and contains stale wording that publication is allowed by dispatch with `publish=true`, contradicting the live `&& false` condition. Its workflow changes are not a safe replacement. | Keep current development guide and hard-disabled workflow; selectively recover only missing valid troubleshooting details, if any. |
| `work/fsm-cos-living-svg-flow` | Its living-SVG explanation, generator using `System.Xml.Linq`, and reduced-motion SVG are unique to that branch. The source labels motion as explanatory, not runtime telemetry, and explicitly leaves browser/accessibility verification open. | Integrate generator + regenerated asset + explanation + index as one coherent candidate; use PR #33's exact-head CI as evidence only for that PR commit, not development. |
| `work/fsm-cos-uml-class-diagrams` | The branch adds class-diagram sections to `docs/ARCHITECTURE.md` and `docs/RUNTIME_MANIFEST.md`, plus detailed UML/context expansions in arbitration, runtime assembly, and staged manifest loading. Development's architecture and manifest pages are shorter and do not contain those class-diagram sections; however, current development manifest docs have at least one lifecycle limitation paragraph absent from the branch. | Strong harvest candidate: port diagrams/clarifying content section-by-section while preserving current development lifecycle caveats and checking every type/member against current source. Do not replace entire contract pages. |

### Harvest order and acceptance criteria

1. **First:** port the source-reviewed UML sections from `work/fsm-cos-uml-class-diagrams` into current documents, preserving newer caveats and confirming every diagram node/member against the current source. Run link checks and exact-head CI after integration.
2. **Second:** reconcile the new measurable-baseline/per-document audit sections from `docs/fsm-cos-visual-contract-audit`; refresh all inventory counts from the live tree before committing.
3. **Third:** integrate the living-SVG branch as a multi-file change, checking the exact GUI renderer pin in the package workflow and regenerating the SVG with that pinned renderer.
4. **Then:** inspect remaining unique files and update this ledger with exact destination paths and commits. Do not mark a source branch harvested merely because a related PR has green checks.

**Deletion gate remains NOT READY.** This pass identified specific harvest targets but did not yet integrate or validate them. The six surplus branches remain; no branch or PR was deleted, merged, or closed. Current development HEAD still needs a fresh CI run before it can be designated the release candidate. Publication remains unauthorized and disabled.


### Verified harvest 1 — composition UML

- Source: `work/fsm-cos-uml-class-diagrams`, file `docs/ARCHITECTURE.md` (branch blob `1bd9acb25c92eefc1228be89d5978d1a0a7ddb4e`).
- Destination: `development:docs/ARCHITECTURE.md`; added a focused class diagram ahead of Runtime flow while retaining the current architecture text and caveats.
- Source cross-check: the diagram's principal types and signatures were checked against live development files `src/FSM_COS/FsmCos.cs`, `IFsmCos.cs`, `IMicroBundleCatalog.cs`, `RuntimeManifest.cs`, `RuntimeManifestSchedule.cs`, and `RuntimeAssembly.cs`. The diagram explicitly labels member signatures/collection element types as abbreviated and says schedule metadata does not activate deferred execution or localize artifacts.
- Integration commit: `9ede8b5b39aaf2241edb373ff45858098eb16fbc`. This is a bounded documentation harvest, not a branch merge and not release verification. Exact-head CI and Markdown/Mermaid rendering still need to pass.


### Verified harvest 2 — staged manifest/load-plan UML

- Source: `work/fsm-cos-uml-class-diagrams:docs/STAGED_MANIFEST_LOADING.md`, blob `abc5012915235906402550b912eb43b6ac2b2939`.
- Destination: `development:docs/STAGED_MANIFEST_LOADING.md`, integrated the bounded class-diagram section only; retained all existing development contract text and verification checklist.
- Cross-check: diagram types and behaviors were checked against `RuntimeManifest.cs`, `RuntimeManifestEntry.cs`, `RuntimeManifestSchedule.cs`, `RuntimeManifestLoadPlan.cs`, `IManifestLoadEvaluator.cs`, `ManifestLoadStage.cs`, and the in-file `MicroBundleRuntimeState` enum. The diagram's caveat explicitly states localization/loading remains host/repository responsibility and stage metadata is not an execution promise.
- Integration commit: `8bc87007964df4354efc216b4741cbac46474526`. Markdown/Mermaid rendering and exact-HEAD CI remain unverified.


### Verified harvests 3–4 — manifest and assembly UML

- `docs/RUNTIME_MANIFEST.md`: ported the bounded `manifest and published plan` diagram from `work/fsm-cos-uml-class-diagrams` (source blob `d3753aeb315781243216ee81418f117d41eff29c`). Kept the existing development contract text. The diagram calls out the distinction between root version requests and published references containing content hashes, and explicitly does not promise artifact retrieval or staged execution. Commit: `568e382a894499224212c59e1e4f28b3ff14600e`.
- `docs/RUNTIME_ASSEMBLY.md`: ported the bounded `assembly handoff` diagram from the same branch (source blob `217115fbbdce40d17656854342441a9b80bcf7fa`). Kept current development prose. The diagram identifies the host as outside FSM_COS and states that RuntimeAssembly is not a UI renderer, process scheduler, or rollback boundary. Commit: `7d682269092cc806a7262bf0ccba7f31b783cdc7`.
- These are content-level diagram harvests only, not wholesale file replacement. The exact source files were compared to development before insertion; Mermaid rendering, link validation, and exact-HEAD CI are still required.


### Verified harvest 5 — arbitration participants UML

- Source: `work/fsm-cos-uml-class-diagrams:docs/ARBITRATION.md`, blob `898faf1ba84788841fbd9eb70fc4fb32d7a827f2`.
- Destination: `development:docs/ARBITRATION.md`; inserted the diagram section without replacing current arbitration contract prose.
- Source cross-check: `ArbitrationContext.cs`, `RuntimeAssembly.cs`, and `FsmCos.cs` were inspected on development. The diagram labels MicroBundle interfaces as external MicroBundleDomain contracts and preserves the distinction between bounded convergence and proof of semantic compatibility.
- Integration commit: `18407454bcbc378f516589a220023d003ce9dfa9`. Rendering and exact-HEAD CI are still required.


### Follow-on inventory — UML companion pages require source-baseline review

The `work/alpha7-living-docs-integration` branch contains a second class of unique material beyond the five architecture/contract diagrams harvested from `work/fsm-cos-uml-class-diagrams`:

- 15 per-source companion documents under `docs/UML/src/FSM_COS/` (including FsmCos, RuntimeAssembly, RuntimeManifest, RuntimeManifestLoadPlan, RuntimeManifestSchedule, and the contracts).
- 5 test-companion documents under `docs/UML/tests/FSM_COS.Tests/`.
- 2 sample-companion documents under `docs/UML/samples/`.
- A branch-specific UML map README and a 28.9 KB `GUILLM.md` coordination artifact; neither exists at that path on current development.

These are promising harvest targets because they align with the repository's per-`.cs` documentation standard, but the branch README names an older source baseline (`ce43e8f5...`) while current development's UML inventory cites a newer source-tree snapshot. **Do not bulk-copy the companions as-is.** Next pass should inspect each page's stated source SHA/member list against current source and tests, then port only still-accurate pages or regenerate them. The test companions and GUILLM also need checks for obsolete claims and cross-repository instructions.

The UML documentation branch is therefore not yet fully reconciled despite five high-level diagrams now being integrated. Its branch-level changes and PR checks are not a substitute for source-baseline validation.


### Verified harvest 6 — current-source FsmCos companion

- The 15 existing companion pages on `work/alpha7-living-docs-integration` all reference old baseline `ce43e8f5bf3dfc229f28678fc2e0d944c5c8d38a`; this differs from the current development baseline and is not safe for wholesale copying.
- Created a new, explicitly source-reviewed `docs/UML/src/FSM_COS/FsmCos.md` against current baseline `326a021498156f2bb7dc7a31219d6118d3427e37` and source blob `42b016f84c75ea6e599259d5b02ee8aede95a895`. It includes a class card, source links, boundary caveats, use cases, and a verification-status statement. Commit: `82909a4399c17e207d7c3ffce8d93217ab0078f6`.
- Added the companion to `docs/UML/README.md` rather than importing the old branch's entire README. Commit: `f0ed2dd8c0b28c0a936fcbcd806256f92fa8aa0a`.
- This establishes a safe pattern for further companions; it does not mean the other 14 source pages, five test pages, or two sample pages are reconciled. Mermaid rendering, automated links, and exact-head CI remain open.


### Verified harvests 7–8 — current-source manifest and assembly companions

Created current-baseline companions rather than copying the older branch versions:
- `docs/UML/src/FSM_COS/RuntimeManifest.md` (source blob `c2b448da5e076f13209d68b1edd7ce50cbbd4d7a`), commit `d0a5907ae8db6e99ab99bf88236ba3358a1a99c6`.
- `docs/UML/src/FSM_COS/RuntimeAssembly.md` (source blob `6ef0c80aacce694fc513a7afaea76309d79fe6a5`), commit `46a532753cf3b0558af0319369f9f670d7e21d10`.
- Indexed both in `docs/UML/README.md`, commit `574f5ff168b15698260ac62592c1938b380f5815`.

Both pages explicitly document the host/repository boundaries and avoid claiming that manifest metadata loads bundles or that RuntimeAssembly provides rendering, scheduling, or rollback. Mermaid rendering and automated link checks remain unverified. This still leaves the other 12 source companions, five test companions, two sample companions, and branch-level docs to reconcile.


### Verified harvests 9–10 — current-source schedule and load-plan companions

Created two more current-source pages rather than copying their stale-baseline counterparts from `work/alpha7-living-docs-integration`:
- `docs/UML/src/FSM_COS/RuntimeManifestSchedule.md`, source blob `28f1c71f6a1f85eb177147740c4a15a1052d5cd8`, commit `6761f17c45bc2c68572a267c91487396be4c6db6`.
- `docs/UML/src/FSM_COS/RuntimeManifestLoadPlan.md`, source blob `484a0882e792bc75e9e766b5368691fba561d618`, commit `07d2ca14b1dc021b005078067c600d5419d006bc`.
- Added both to `docs/UML/README.md`, commit `ec603037cfd1e5d1e9716bb55aa9da98b0f9fa22`.

The schedule companion documents bounded input snapshots, dependency validation, and the explicit boundary that it is not a process scheduler. The load-plan companion distinguishes state tracking from host-owned localization/loading and automatic execution. Both pages point at current source and mark rendering, links, and exact-head CI as unverified.


### API accuracy follow-up — UML companion signatures

Rechecked the two new state/schedule companions against the full current source files. Updated the schedule diagram to include `DependencyEntries`, `DependenciesOf`, and `IsDependencyReady`; updated the load-plan diagram to reflect the actual `EvaluatePromotions(IManifestLoadEvaluator, IStateContext?, RuntimeManifestSchedule?)` signature and its `IEnumerable<RuntimeManifestEntry>` result. Commits: `e91d14cb0e862fc66f0350e9c32cb3977bc2d9cd`, `90439c648c31e713fa05ef71b0b93c7d167a9e1a`. This correction is recorded because generated Mermaid and exact-head CI have not yet independently validated the docs.


### Verified harvests 11–13 — current-source manifest identity companions

Created and indexed current-source companions:
- `docs/UML/src/FSM_COS/RuntimeManifestEntry.md` — source blob `acd95c2ce7ff7f10ce4696d3ff3c11dec70837ad`, commit `d7b0cb4f29be61e2b04f3a4ac12cb988edf8eecd`.
- `docs/UML/src/FSM_COS/MicroBundleReference.md` — source blob `931905da936669cb1fdad47f0fef8015f0e4d451`, commit `1c6ee02d9e2dff9156cd8c9bf199c9da914e49f6`.
- `docs/UML/src/FSM_COS/RuntimeManifestDependency.md` — source blob `2ad81443708f5ef9e4df7f96a6cc477ffb059778`, commit `202f91986e107bb6f28abe703056124d61baaae1`.
- Index commit: `97cb479651361c886992acec1dca2e966d0ea92e`.

The pages distinguish validity checks from artifact retrieval/hash verification and graph-level validation. The corresponding older branch pages use the old source baseline and were not copied. Rendering, automated link checks, and exact-head CI remain unverified.


### Verified harvests 14–21 — current-source composition contract companions

Created and indexed eight more pages against current development source:
- `docs/UML/src/FSM_COS/IManifestLoadEvaluator.md` — source blob `735a28a7ec2180330f20d600152f83c3f391c519`, commit `495cb6b8612d7d8c83902037f87658b3b20061cd`.
- `docs/UML/src/FSM_COS/IMicroBundleCatalog.md` — source blob `54925e70584fa414abc7c86669f8beb738004d6d`, commit `a7b2b8a541f3af3fc8f27e84cae4bea4649549f4`.
- `docs/UML/src/FSM_COS/IMicroBundleConfigurationSource.md` — source blob `eaad15c340817391e41bfb3d5e6dc9028e193e5a`, commit `354fc7d711d31b8901af85f533a06c5fca5afa20`.
- `docs/UML/src/FSM_COS/ManifestLoadStage.md` — source blob `e7313fb890ac15cc1f61648056edb885ac856a0f`, commit `2ac48ea1c1b400549d19154a6f7c9d730ddd14a7`.
- `docs/UML/src/FSM_COS/MicroBundleManifestEntry.md` — source blob `f53611126fde9a3eacb5f596d6fa13fc4e61ce6e`, commit `4483718751770e72af345cc2b089a38f48a8db61`.
- `docs/UML/src/FSM_COS/MicroBundleLoadContext.md` — source blob `4ede9c138d0fd5fca0638b135565e2724b283b62`, commit `1c309f64e9e3be9947d1ed691a8ce7a7e9841cc5`.
- `docs/UML/src/FSM_COS/IFsmCos.md` — source blob `6b3d18419609e364d00918b8ef632e443c93a31f`, commit `c032d1a1ba67e3a8c84b116766c4c931a42afda8`.
- `docs/UML/src/FSM_COS/ArbitrationContext.md` — source blob `e74a4d177f894de85cd2a34c4df00a182b973c07`, commit `1c77bc2ac29abb42d9a05c7d4f6e31e0830a6f89`.
- UML index update: `9b7b8c957b5a9b5397ee00552dba0bccfb720f27`.

The pages cover Experience-owned promotion policy, catalog resolution, optional configuration, intended load-stage metadata, root request identity, load context, composition entry interface, and arbitration context. Each states the boundary between its contract and artifact localization/loading. Rendering, automated links, and exact-head CI remain unverified.


### Verified harvests 22–25 — current-source test companions

Compared the old integration branch's UML/test inventory with current `development`. The four substantive test files were present in source but had no current UML companions. Created and indexed:
- `docs/UML/tests/FSM_COS.Tests/AiGuiCapabilityHandoffTests.md` — source blob `b28a6df5b7b5c4a7f8a6dc9ad9cf192b78c21f99`, 3 `[Fact]` tests, commit `f03b3fd1fc282cd86538b4a597db4cff6916c417`.
- `docs/UML/tests/FSM_COS.Tests/FsmCosTests.md` — source blob `6ee7f5e2b5feb0286f08fb8403b92f17edc31447`, 86 `[Fact]` tests, commit `5e516e45cd6d5fcf5d85643743d7debba0419328`.
- `docs/UML/tests/FSM_COS.Tests/RuntimeManifestPlanningTests.md` — source blob `8f4f1f8ee010f8a3b8bc7b68582998a57bccc940`, 33 `[Fact]` tests, commit `42f679e466ec6a6712a6bae9bb7a98233e5100b8`.
- `docs/UML/tests/FSM_COS.Tests/SnapshotIsolationTests.md` — source blob `4b878f7c350f764f1cb5ae9d8153065b5d79b8ac`, 3 `[Fact]` tests, commit `4bbdf6eff0a440040a6aa2e8dc5ca469e9e8bd5a`.
- Index commit: `5b984900542e5cac550f2c33465be6a96fe9bef5`.

The test docs summarize the actual current test themes; they are not assertions that tests passed. The test project must run on the exact integrated HEAD, and Mermaid/link verification remains outstanding. The old `GlobalUsings.md` is not being copied as a class companion because `GlobalUsings.cs` only declares `global using Xunit;`; it can be covered by a concise project-level note if needed.


### Living-SVG harvest — integrated as a coherent generator/asset/doc set

Compared `work/fsm-cos-living-svg-flow` with current `development`. The branch had only one file absent from development, `docs/LIVING_SVG.md`; its other unique value was the coordinated change across the generator, generated SVG, and documentation. The generator change was narrowly reviewed: add `System.Xml.Linq`, tag the seven connector lines with `flow-line`, and inject the CSS stylesheet after serialization. The reduced-motion media query disables motion and restores solid connectors. Existing scene labels and diagram intent match the current development asset.

Integrated:
- Generator commit: `2aaa0e68483e19b64b0571514c3a0c1f6eb92d99`
- Generated SVG commit: `12b6ae559dc844765de50ddaa9e283a751c10831`
- Living-SVG contract doc commit: `04d08fc67e72637b937f514f3cd59e2e4b174caa`
- README link commit: `43725b06218a888705e4ef74529104a67508fdc3`

The package workflow's existing pinned GUI.SVG checkout and regeneration/diff guard were preserved; no workflow or publishing condition was changed. The generated SVG was harvested from the matching generator branch, but exact-head workflow regeneration has not yet verified byte-for-byte reproducibility on the integrated development candidate. Browser rendering, hosted appearance, accessibility-tree exposure, and screen-reader behavior remain unverified. PR #33 and #34 remain open/draft pending a fresh check review.
