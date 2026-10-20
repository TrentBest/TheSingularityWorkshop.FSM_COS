# Live Branch Reconciliation Ledger

> **Snapshot:** 2026-10-20 UTC. This is a point-in-time inventory, not a deletion authorization. Refresh refs and candidate CI before acting on it.

## Sith policy: two durable branches

The durable branch set is exactly `master` and `development`.

- **`master`** is the stable/release line. Do not update it as part of routine preparation; move a verified `development` state into `master` only as an explicit release decision.
- **`development`** is the integration line and current source of truth for ordinary work.
- **Temporary branches** are allowed to isolate a substantial or risky change, but they are staging areas, not additional sources of truth. Reconcile useful changes into `development`; after verification and integration, close their PRs and delete them.
- Do not merge divergent branch histories wholesale simply to reduce the branch count. Compare unique file content and commit ancestry; preserve useful work and identify genuinely obsolete branches.
- Never delete branches, close useful PRs, merge to `master`, publish a package, or deploy without the applicable authorization.

## Current authoritative refs

| Ref | Observed head | Role / status |
|---|---|---|
| `master` | `2b8619eca6ae8800a02462a4377123fa20603e2f` | Stable line; untouched by this preparation |
| `development` | `577b39ca286ac2bed401f01eb6c71949f922f6af` | Integration line |
| `work/alpha7-living-docs-integration` | `80c447a81e14e2f15a87c526b582927c33bd1890` | Draft PR [#34](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/pull/34), base `development`; exact-head CI still pending |

## Temporary-branch inventory

These dispositions are preliminary. The current alpha.7 candidate must pass exact-head verification and be reconciled with `development` before branches are declared safe to delete.

| Temporary branch | Observed head | Initial reconciliation finding | Disposition |
|---|---|---|---|
| `docs/alpha7-release-readiness` | `308d5e6afeb7515ce94be91e6f768eaa9008c273` | Contains a readiness document plus README/index edits. The candidate has a different, integrated readiness-document version; audit the unique wording and claims before closing this branch out. | **Review unique content; likely superseded** |
| `docs/fsm-cos-visual-contract-audit` | `2a0245a21b7f3e88d2c74f18171b16581b75a5b8` | Holds the visual-contract audit and overview-SVG correction. Both areas have evolved on the alpha.7 candidate, so compare the final candidate against this source rather than merging its old tree wholesale. | **Review unique content; likely superseded by PR #34** |
| `work/ci-development-dependencies` | `3efe0cc1a58544df0340368ed7dd671f72258ca1` | Contains the development-source dependency workflow and a `docs/DEVELOPMENT.md` revision. Current development already documents the two-branch policy and the candidate workflow has advanced immutable action pins and additional verification; check for any unique documentation or CI behavior before disposal. | **Older implementation; verify no unique behavior remains** |
| `work/fsm-cos-living-svg-flow` | `1595e1b769b80b381e8881bed0acaf3187bdaaff` | Its `docs/LIVING_SVG.md` content is identical to the candidate; the preflight SVG and generator have since changed on the candidate. | **Superseded content appears integrated; final diff check required** |
| `work/fsm-cos-uml-class-diagrams` | `192c8912fa99560b16a47bc4f0c628b84fa089e3` | The candidate's `docs/ARCHITECTURE.md` matches this branch exactly; the five class-diagram guides are integrated into PR #34. | **Superseded content appears integrated; final diff check required** |

## Release-preparation gate

At this snapshot, PR #34 remains **open and draft**. The previous workflow run on the older candidate failed package verification because the packaged gallery linked to a maintainer-only audit file excluded from the NuGet package. The current candidate changes that gallery link to a repository URL and its 21 local Markdown links were checked against the package layout.

No pull-request-triggered workflow run or commit status is currently reported for exact head `80c447a81e14e2f15a87c526b582927c33bd1890`. Therefore the fix is **locally/repository-tree checked, not yet CI-verified**. Do not merge or publish based on the older run.

Next steps:

1. Obtain successful exact-head CI for PR #34, including package verification, GUI.SVG regeneration/parity, source and public-NuGet integration, tests, coverage, and security checks.
2. Inspect hosted Markdown/Mermaid/SVG rendering and accessibility at light/dark themes and narrow widths; source inspection alone does not prove hosted behavior.
3. Reconcile the verified candidate into `development`, then run the integration line's checks.
4. Review every temporary branch's unique commits/files against the integrated `development` state. Label each **incorporated** or **discardable** only when supported by that comparison.
5. Report the exact branch list as ready for owner deletion only after those checks. Keep `master` unchanged until an explicit release decision.

NuGet publication remains a separate, explicitly authorized action. A green verification run is not permission to publish.
