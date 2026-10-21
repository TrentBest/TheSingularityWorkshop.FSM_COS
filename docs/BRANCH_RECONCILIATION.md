![The Singularity Workshop animated moniker](https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Renderer/development/docs/assets/moniker.svg)

# Live Branch Reconciliation Ledger

> **Snapshot:** 2026-10-20 18:23 UTC. Live refs and comparisons refreshed. This is an evidence ledger, not authorization to merge, publish, or delete branches.

## Sith policy: two durable branches

The intended durable branch set is exactly `master` and `development`.

- **`master`** is the release line; leave it untouched unless a separate, explicit release decision authorizes a reviewable transition.
- **`development`** is the integration line for ordinary work.
- Temporary branches are staging areas. Reconcile their useful changes into `development`, then classify unique content against the exact integrated tree.
- Never force either durable branch to match the other merely to simplify branch count.
- Do not delete branches, close useful PRs, update `master`, publish a package, or deploy without the applicable authorization.

## Live FSM_COS refs

| Ref | Observed state | Disposition |
|---|---|---|
| `master` | Existing release line; head not re-fetched in this snapshot | Preserve |
| `development` | `577b39ca286ac2bed401f01eb6c71949f922f6af` | Preserve as integration base |
| `work/alpha7-living-docs-integration` | `9e5ac133fb56af0e6ff6c4f587a3e7618018ad57` | Keep; draft PR [#34](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/pull/34) |

PR #34 is open, draft, and unmerged. GitHub compare reports candidate `work/alpha7-living-docs-integration` **80 commits ahead, 0 behind** `development` at the snapshot. The candidate Pack and publish workflow [#790](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/actions/runs/39472732154) and CodeQL [#94](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/actions/runs/39472732326) succeeded on candidate `9e5ac133...`; the publish job was skipped. These checks are candidate evidence, not evidence for a later integrated SHA.

## Temporary-branch unique-content audit

GitHub compare was refreshed against both `development` and the current PR #34 candidate. The file lists below are the paths changed by each branch's branch-unique commit range; counts are ancestry counts, not semantic proof of equivalence.

| Branch | Unique commits vs development | Behind development | Branch-unique paths | Current finding |
|---|---:|---:|---|---|
| `docs/alpha7-release-readiness` | 4 | 277 | `DOCUMENTATION_INDEX.md`, `README.md`, `docs/0.1.0-alpha.7-READINESS.md` | Candidate has a richer readiness section and links the completion roadmap. Final text comparison still required before discardable. |
| `docs/fsm-cos-visual-contract-audit` | 5 | 4 | `docs/LIVING_DOCUMENTATION_AUDIT.md`, `docs/assets/fsm-cos-overview.svg` | Candidate contains the overview correction and updated audit. Final text/asset equivalence check still required. |
| `work/ci-development-dependencies` | 2 | 307 | `.github/workflows/package.yml`, `docs/DEVELOPMENT.md` | Candidate workflow supersedes the older workflow's generic checkout and incorrect publish gate; final behavior check still required. |
| `work/fsm-cos-living-svg-flow` | 8 | 4 | `DOCUMENTATION_INDEX.md`, `NuGet.README.md`, `docs/LIVING_SVG.md`, `docs/assets/fsm-cos-preflight.svg`, `samples/DocumentationVisuals/Program.cs` | Candidate includes the living-SVG guide, scene, and generator/output work; final content and deterministic parity check still required. |
| `work/fsm-cos-uml-class-diagrams` | 11 | 4 | `docs/ARBITRATION.md`, `docs/ARCHITECTURE.md`, `docs/RUNTIME_ASSEMBLY.md`, `docs/RUNTIME_MANIFEST.md`, `docs/STAGED_MANIFEST_LOADING.md` | Five manually authored Mermaid diagrams are represented in candidate docs; they are not GUI.SVG-generated UML. Final text comparison still required. |
| `work/alpha7-living-docs-integration` | 80 | 0 | Active candidate, 24 changed files in PR #34 | Retain pending an authorized integration decision and post-integration verification. |

**No temporary branch is declared safe to delete yet.** The five branch-unique path sets are narrow and the candidate appears to supersede their purpose, but deletion readiness requires comparing exact file contents and assets against the final integrated `development` SHA, not only these current candidate comparisons.

## GUI.SVG source-derived UML dependency

GUI repo PR [#18](https://github.com/TrentBest/TheSingularityWorkshop.GUI/pull/18), branch `work/gui-svg-source-uml-generator`, remains draft. Latest head observed: `9cadb06a648082879f9b1dd278213a7e2346b7d6`; comparison to GUI `development` reports 56 commits ahead and 11 behind, merge base `8699c26fc821f89ba4bee340e955a564ce2323e7`.

- Source-derived diagrams workflow [#45](https://github.com/TrentBest/TheSingularityWorkshop.GUI/actions/runs/39473512921): passed.
- Package GUI workflow [#689](https://github.com/TrentBest/TheSingularityWorkshop.GUI/actions/runs/39473512854): passed; publication was skipped.

This verifies CI at the candidate head; it does **not** close the semantic UML contract. Compiler/project semantic resolution, canonical relationship emission and connectors, complete multi-type inventory, deterministic regeneration for the consumer contract, and hosted visual/accessibility checks remain open. FSM_COS correctly labels the interim Mermaid diagrams as manually authored rather than GUI-generated. Keep the dependency boundary honest and do not treat PR #18 as merged or published.

## Master/development release-line warning

The latest compare API result reports `development` as 628 commits ahead of `master`, and `master` as 44 commits ahead of `development`, with merge base `e9c9968b9cdbec0478e77d9f89b4e74a0e404d3d`. This is a substantial divergence. The compare includes release-era source, workflows, package metadata, README and documentation paths. Do not replace or force-update either branch as a branch-count cleanup shortcut. Inventory master-only changes and approve a release-transition strategy separately.

## Required sequence before branch-owner cleanup

1. Decide and authorize how PR #34 is to integrate into `development`; no merge is implied by this ledger.
2. After integration, capture the exact integrated SHA and rerun required CI, CodeQL/SARIF review, SVG regeneration/parity, package verification, and exact package inspection against that SHA. Do not publish unless separately authorized.
3. Complete final content/asset comparison for each of the five older branches against that exact tree; classify each as incorporated or discardable with evidence.
4. Independently resolve the `master`/`development` release-line divergence. Do not use branch deletion as a substitute.
5. Report the exact temporary branch names as safe for the owner to delete only after these gates are met.

Green CI is not permission to merge or publish. This ledger does not authorize branch deletion.

---

**The Singularity Workshop**  
[GitHub](https://github.com/TrentBest) · [Coder Legion](https://coderlegion.com/) · [Patreon](https://www.patreon.com/c/TheSingularityWorkshop) · [PayPal](https://www.paypal.com/donate/?hosted_button_id=3Z7263LCQMV9J)  
[ FSM_API ](https://github.com/TrentBest/FSM_API) · [ FSM_Serialization ](https://github.com/TrentBest/TheSingularityWorkshop.FSM_Serialization) · [ WebPage ](https://github.com/TrentBest/WebPage) · [ FSM_COS Documentation Standard ](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/DOCUMENTATION_STANDARD.md)  
**Because state shouldn't be a mess.**  
*And because static boundaries are invitations to cause trouble.*
