# Live Branch Reconciliation Ledger

> **Snapshot:** 2026-10-20 16:30 UTC. Point-in-time inventory only; refresh refs and candidate CI before acting. This is not deletion authorization.

## Sith policy: two durable branches

The durable branch set is exactly `master` and `development`.

- **`master`** is the stable/release line. Leave it untouched during routine preparation. Move a verified development state to master only through an explicit, reviewable release decision.
- **`development`** is the integration line and current source of truth for ordinary work.
- **Temporary branches** may isolate substantial or risky work, but are staging areas, not additional sources of truth. Reconcile useful changes into `development`; after verification and integration, classify them as incorporated or discardable.
- Never merge divergent histories wholesale merely to reduce branch count. Compare ancestry and unique content first.
- Do not delete branches, close useful PRs, update `master`, publish a package, or deploy without the applicable authorization.

## Current refs and verified comparison

| Ref | Observed head | Current disposition |
|---|---|---|
| `master` | `2b8619eca6ae8800a02462a4377123fa20603e2f` | Stable line; untouched |
| `development` | `577b39ca286ac2bed401f01eb6c71949f922f6af` | Integration line |
| `work/alpha7-living-docs-integration` | `bacb5235f077ad7919ba1678e0f22106fe280be6` | Draft PR [#34](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/pull/34); CI green for PR merge candidate; not integrated |

Live branch comparison against `development` (GitHub compare API, 2026-10-20):

| Branch | Commits unique to branch | Commits behind development | Preliminary disposition |
|---|---:|---:|---|
| `docs/alpha7-release-readiness` | 4 | 277 | Readiness doc is superseded in candidate, but compare unique wording/claims before marking discardable |
| `docs/fsm-cos-visual-contract-audit` | 5 | 4 | Audit and overview SVG evolved in candidate; final content reconciliation required |
| `work/ci-development-dependencies` | 2 | 307 | Older workflow/docs changes; verify no unique behavior remains |
| `work/fsm-cos-living-svg-flow` | 8 | 4 | Living SVG guide appears integrated; verify generator/output delta before final classification |
| `work/fsm-cos-uml-class-diagrams` | 11 | 4 | Five Mermaid diagrams integrated into PR #34; GUI.SVG UML-generator requirement is not satisfied by these diagrams |
| `work/alpha7-living-docs-integration` | 56 | 0 | Active staging branch for draft PR #34; retain until integration and verification |

The commit counts are ancestry counts, not proof that every unique change is useful or missing. No temporary branch is yet declared safe to delete.

## Release-line ancestry warning

`master` and `development` are diverged, not fast-forward-related. Current compare reports **44 commits unique to master and 628 unique to development** from merge base `e9c9968b9cdbec0478e77d9f89b4e74a0e404d3d`. The master-only delta includes release-era code, workflow, and documentation changes (including changes to `FsmCos.cs`, the package workflow, README, dependency guidance, and release docs). It is not safe to replace master with development or force-update the ref just to simplify the graph. Before the release transition, inventory all master-only commits and file deltas, decide which content remains relevant, and prepare an explicit, reviewable reconciliation strategy. `master` remains untouched.

## GUI.SVG UML requirement

The alpha.7 candidate's five class diagrams and Execute sequence are manually authored Mermaid. They are not GUI.SVG-generated UML.

GUI's `development` branch has renderer-neutral `DataCard` semantics and a static SVG data-card renderer, but the GUI guide explicitly calls for a **source-grounded, tested SVG class-diagram generator and fixture whose documentation image is generated and verified in CI** as its immediate milestone. The renderer currently consumes an already-built card model; it does not parse source code. Therefore the current diagrams do not meet the source/model-generated UML requirement. Do not close the living-documentation gate by relabeling Mermaid as GUI-generated. The next useful step is to implement or complete the GUI source-to-model-to-SVG generator and its deterministic verification, then adopt it in FSM_COS; otherwise record this as a deliberate release deferral rather than silently treating it as done.

## Current alpha.7 evidence

- PR #34 remains open and draft. Current head: `bacb5235f077ad7919ba1678e0f22106fe280be6`; base `development` head at last inspection: `577b39ca286ac2bed401f01eb6c71949f922f6af`.
- Pack workflow run [#768](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/actions/runs/39454926450) succeeded for the PR merge candidate: source integration, build/test, public-package compatibility and package verification passed; publish job skipped.
- CodeQL run [#72](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/actions/runs/39454926544) succeeded; inspected SARIF contained zero findings.
- The generated package artifact was independently inspected. It contains consumer docs/assets and excludes maintainer-only readiness/audit and historical planning files. Exact package hash and artifact evidence are retained in the release-preparation conversation; do not reuse that evidence as if it came from a later integrated `development` SHA.
- Gallery package-link verification now passes on this PR merge candidate. Hosted GitHub Markdown/Mermaid/SVG rendering, light/dark and narrow-width behavior, accessibility-tree exposure, keyboard behavior, and screen-reader behavior remain unverified.
- After any integration into `development`, rerun required CI, CodeQL/SARIF review, SVG regeneration/parity, package verification, and exact-package inspection against that exact final `development` SHA. Do not publish unless explicitly authorized.

## Next steps

1. Resolve the GUI.SVG source-grounded UML generator requirement (implement it or explicitly defer it with owner approval).
2. Complete hosted rendering and accessibility checks.
3. Reconcile PR #34 into `development` only after its release gates are resolved; verify the exact integrated SHA.
4. Compare each temporary branch's unique files/commits against the integrated `development` tree and classify it as **incorporated** or **discardable** only with evidence.
5. Separately review master-only history and select a release-transition strategy. Do not force-push or replace `master`.
6. Report the exact temporary branch list as ready for owner deletion only after those steps.

NuGet publication is a separate, explicitly authorized action. A green verification run is not permission to publish.
