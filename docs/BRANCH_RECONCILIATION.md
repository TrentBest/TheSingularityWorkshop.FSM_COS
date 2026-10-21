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
