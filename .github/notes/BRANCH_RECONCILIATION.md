<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# FSM_COS Branch Reconciliation — Sith Policy

**Audience:** maintainers only. This is not product documentation.

**Policy:** the intended persistent branches are exactly `master` and `development`. Preserve useful work before branch removal. Do not force-reset or merge divergent histories merely to make branch counts look tidy. Branch deletion is an owner action after reviewing this ledger.

**Audit date:** 2026-10-24 (05:48 UTC).

## Persistent branches — live snapshot

| Branch | Observed head | Role | Action |
|---|---|---|---|
| `master` | `43892fde0703f27146dd81ea33dd8ce4a9ad53b2` | Prior release baseline | Preserve until owner-authorized replacement. Preserve publication and public-package compatibility safeguards. |
| `development` | `67929891e141598ca5c7f17afeb2feb60612e18c` | Active integration / alpha.7 candidate | Preserve as current source of truth. Candidate verification evidence is exact-SHA-specific. |

The live branch inventory returned **exactly two branches: `master` and `development`**. No branch deletion, ref movement, merge, or history rewrite was performed.

## Current divergence and release integration

Draft PR [#35](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/pull/35) (`development` → `master`) remains open and non-mergeable because the histories diverge. The live comparison reports **1,070 commits ahead and 51 behind** for development relative to master. Merge base: `e9c9968b9cdbec0478e77d9f89b4e74a0e404d3d`.

This is not a routine merge. Do not merge legacy `master` into `development`, force-reset either branch, or delete anything to tidy the branch list. The intended end state is owner-controlled replacement of the old master baseline only after selective reconciliation and exact-candidate verification.

## Exact-SHA candidate evidence

Candidate SHA: `67929891e141598ca5c7f17afeb2feb60612e18c`.

- Pack/build/test workflow [40064639400](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/actions/runs/40064639400): completed successfully on this exact SHA. Build/test, public-package compatibility, and source-integration jobs succeeded; `publish_nuget` was skipped.
- CodeQL [40064639389](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/actions/runs/40064639389): completed successfully on the same SHA. The downloaded C# SARIF contains zero result entries. This is the observed SARIF result, not a substitute for checking the workflow's configured analysis scope.
- Development-source package artifact: outer artifact SHA-256 `f5a3a8a5a71c8f0913bef35754b28658e01b0a6ed36df143f8e9c157bb63bdc7`; contained `TheSingularityWorkshop.FSM_COS.0.1.0-alpha.7.nupkg` (138,238 bytes; inner package SHA-256 `9badd9ed9dc6f80a8307b1e603e6c287c2427946ff63a377fa2c9fedb283d804`). Built against the development source dependency graph.
- Public-feed compatibility package artifact: outer artifact SHA-256 `5028c3f986b43712985734c53fca3a0eb1c5118f1ed232a68584a233680ea4bc`; contained the same package ID/version (138,231 bytes; inner package SHA-256 `6b08f93cc899823139f4c708fdedb260e149570bdfb6f8f4c96605e3a1c3c94f`). This is the artifact from the public-package compatibility job and is the relevant candidate for the existing publish workflow.
- The two packages are **not byte-identical**: the `lib/net8.0/FSM_COS.dll` entries differ (same size, different SHA-256). The workflow intentionally restores different dependency graphs for these jobs (development MicroBundleDomain source versus published NuGet dependencies), so this difference is explainable and must not be misreported as an artifact-corruption finding. The publish workflow's selected artifact/source must remain the public-feed compatibility package, with its provenance preserved.
- Inspected package metadata records repository commit `67929891e141598ca5c7f17afeb2feb60612e18c`, target `net8.0`, and dependencies `FSM_API 1.0.13`, `FSM_UserIO 0.1.0-alpha.1`, and `MicroBundleDomain 1.0.1`. Package contains the NuGet README, license, XML API documentation, and documentation/assets.
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
- [x] Verify pack/build/test, public-package compatibility, source integration, and CodeQL on one exact development SHA.
- [x] Inspect test results and both package artifacts; distinguish source-dependency package from public-feed candidate.
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
