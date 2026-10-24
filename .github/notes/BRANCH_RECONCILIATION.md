<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# FSM_COS Branch Reconciliation — Sith Policy

**Audience:** maintainers only. This is not product documentation.

**Policy:** the intended persistent branches are exactly `master` and `development`. Preserve useful work before branch removal. Do not force-reset or merge divergent histories merely to make branch counts look tidy. Branch deletion is an owner action after reviewing this ledger.

**Audit date:** 2026-10-24 (06:30 UTC).

## Persistent branches — live snapshot

| Branch | Observed head | Role | Action |
|---|---|---|---|
| `master` | `43892fde0703f27146dd81ea33dd8ce4a9ad53b2` | Prior release baseline | Preserve until owner-authorized replacement. Preserve publication and public-package compatibility safeguards. |
| `development` | `3c0ca60a0baae665b8871b2a2aacacf8827976c9` | Active integration / alpha.7 candidate | Preserve as current source of truth. Candidate verification evidence is exact-SHA-specific. |

The live branch inventory returned **exactly two branches: `master` and `development`**. No branch deletion, ref movement, merge, or history rewrite was performed.

## Current divergence and release integration

Draft PR [#35](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/pull/35) (`development` → `master`) remains open and non-mergeable because the histories diverge. The live comparison reports **1,071 commits ahead and 51 behind** for development relative to master. Merge base: `e9c9968b9cdbec0478e77d9f89b4e74a0e404d3d`.

This is not a routine merge. Do not merge legacy `master` into `development`, force-reset either branch, or delete anything to tidy the branch list. The intended end state is owner-controlled replacement of the old master baseline only after selective reconciliation and exact-candidate verification.

## Exact-SHA candidate evidence

Candidate SHA: `3c0ca60a0baae665b8871b2a2aacacf8827976c9`. The package and CodeQL workflows below are green on this exact SHA; if this ledger update creates a new development commit, the new HEAD becomes the candidate and must be reverified.

- Pack/build/test workflow [40067625575](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/actions/runs/40067625575): completed successfully on this exact SHA. Build/test, public-package compatibility, and source-integration jobs succeeded; `publish_nuget` was skipped.
- CodeQL [40067625528](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/actions/runs/40067625528): completed successfully on the same SHA. This establishes workflow success for the candidate; the earlier candidate's SARIF result is not reused as current evidence.
- Development-source package artifact: workflow artifact ID `12550091701`, size 133,750 bytes, artifact digest `sha256:c29ebe637ceadb4c1e2c91e9e1932389ae21959e151d228846ba8bbded4fc34a`. It was produced by the exact-SHA candidate workflow and is built against the development-source dependency graph. The inner `.nupkg` metadata and hash still need direct inspection before release approval.
- Public-feed compatibility package artifact: workflow artifact ID `12550071690`, size 133,751 bytes, artifact digest `sha256:b5953e85b1781c2a3861bc5c041a09fd4c631d721a68f4e04f9d2f1ec007aa93`. This is the intended artifact from the public-package compatibility job and the relevant candidate for the existing publish workflow; its inner `.nupkg` metadata and hash still need direct inspection before release approval.
- The earlier candidate's two packages were **not byte-identical**: the `lib/net8.0/FSM_COS.dll` entries differed because the jobs restore different dependency graphs (development MicroBundleDomain source versus published NuGet dependencies). Do not carry the old inner-package comparison forward as if it inspected these new artifact bytes. The publish workflow's selected artifact/source must remain the public-feed compatibility package, with its provenance preserved.
- The earlier candidate's package metadata recorded target `net8.0` and dependencies `FSM_API 1.0.13`, `FSM_UserIO 0.1.0-alpha.1`, and `MicroBundleDomain 1.0.1`, but that inspection does **not** establish the metadata of the artifacts for `3c0ca60...`. Inspect the current candidate's downloaded packages before final approval.
- CiAi observer contract verification [40066974590](https://github.com/TrentBest/TheSingularityWorkshop.CiAi/actions/runs/40066974590) passed after the workflow was changed to require an explicit candidate SHA. The observer has **not yet been manually dispatched** against this FSM_COS SHA; its end-to-end JSON observation artifact remains outstanding.
- Workflow action references are pinned; NuGet publishing remains explicitly gated and was not performed.

This evidence validates the **development candidate only**. Any reconciliation commit changes the candidate SHA and requires applicable checks to run again. The package artifacts above are not proof that a final `master` candidate has passed.
## Master-only documentation review

The following four documents exist on current `master` and are absent from current `development`. They have been inspected at a high level, but **have not yet been ported**. Each should be selectively harvested only after claims, links, and examples are checked against alpha.7 source and current package metadata.

| Path on master | Initial disposition | Specific review cautions |
|---|---|---|
| `docs/COMPUTE_SCALE.md` | Review for selective preservation | Clearly distinguish the current in-process composition boundary from proposed remote/distributed execution; don't imply distributed execution is implemented. |
| `docs/DEPENDENCIES.md` | Strong candidate to port after version/API verification | Describes FSM_API, FSM_UserIO, MicroBundleDomain, and deliberate non-dependencies; verify current API names and keep optional integration claims explicit. |
| `docs/ECOSYSTEM_INTEGRATION_MAP.md` | Preserve as a clearly labeled working/architecture document if still useful | Snapshot is dated 2026-10-08 and includes older alpha versions and mismatches; refresh the facts table or clearly retain it as historical, never treat it as current release evidence. |
| `docs/PERFORMANCE_AND_EFFICIENCY.md` | Review for selective preservation | FSM_API measurements are not FSM_COS benchmarks; keep the energy-efficiency section explicitly hypothetical and label AnyApp footprint figures as historical/approximate. |

Other master-only source, tests, workflow, and documentation changes still require file-level comparison; this four-file review is not a claim that the entire 51-commit master-only side has been exhaustively reconciled. The release workflow and hard-gated publication safeguards must survive the eventual candidate selection.

## Accessibility / host-integration gate

CI verifies source/package structure and generated SVG consistency, but does not by itself prove the hosted browser rendering, accessibility tree, keyboard operation, or screen-reader experience. Determine applicability for the core .NET package versus the consuming host. If host-level behavior is part of the release claim, record browser/device/assistive-technology evidence; otherwise scope the limitation explicitly rather than claiming it passed.

## Owner checklist

- [x] Confirm the live persistent branch inventory is exactly `master` and `development`.
- [x] Refresh both branch heads, ahead/behind counts, and merge base.
- [x] Verify pack/build/test, public-package compatibility, source integration, and CodeQL on one exact development SHA (`3c0ca60...`; must be repeated if the ledger update advances `development`).
- [ ] Download and inspect the current candidate's test results and both package artifacts; distinguish source-dependency package from public-feed candidate.
- [ ] Manually dispatch CiAi observer for exact candidate SHA and inspect the resulting JSON artifact.
- [x] Identify four master-only documentation candidates and record review cautions.
- [ ] Inspect remaining master-only commits/files for unique source, tests, docs, and workflow protections.
- [ ] Selectively port approved useful work to development and update this ledger with each disposition.
- [ ] Run all required checks on the resulting new exact development SHA.
- [ ] Verify applicable hosted SVG/accessibility behavior or explicitly scope it out.
- [ ] Select the final master candidate and repeat applicable verification on that exact SHA.
- [ ] Obtain explicit owner authorization before any NuGet publication.

This ledger records observed state, not merge approval, branch replacement approval, or release authorization.

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
