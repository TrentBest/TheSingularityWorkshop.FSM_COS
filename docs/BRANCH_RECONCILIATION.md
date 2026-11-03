<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# FSM_COS Branch Reconciliation Ledger

**Maintainer-only release record.** This ledger documents branch policy and the current observed state; it is not consumer documentation.



## Live Sith activity audit — 2026-11-03

This snapshot was refreshed from the live GitHub branch inventory and commit metadata on 2026-11-03. It supersedes the 2026-10-22 inventory above for current branch-count/activity purposes; the earlier release evidence remains historical and SHA-scoped.

### Branch activity — newest first

| UTC committer time | Branch | Author | Latest subject | Head SHA |
|---|---|---|---|---|
| 2026-11-01 05:19:32 | [development](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/tree/development) | The Singularity Workshop | Document the AI package lineage and agent onboarding | [cad5775fe55a6a8d0b6df8bbd22017c1179141c4](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/commit/cad5775fe55a6a8d0b6df8bbd22017c1179141c4) |
| 2026-10-25 05:56:43 | [master](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/tree/master) | The Singularity Workshop | Define canonical Workshop README badge order and health reporting | [a4ed975882313b9a490eeb5eb82e291d04d866d0](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/commit/a4ed975882313b9a490eeb5eb82e291d04d866d0) |

### Reconciliation and verification

- Live inventory: exactly two branches, `development` and `master`; no extra branches were found.
- Open pull requests: none at audit time.
- Divergence: `development` is 155 commits ahead of `master`; `master` is 2 commits ahead of `development`. The refs have diverged from merge base `56ad34dbe4722ba1edf9a4da6a00812eeaa29101`.
- The two master-only commits produce a file-level difference in `DOCUMENTATION_STANDARD.md` (52 additions, no deletions in the compare summary). This is a preservation/review item, not permission to merge legacy master into development. Inspect and selectively reconcile any still-authoritative material before a future owner-authorized master replacement.
- Exact-head verification: GitHub combined commit-status lookup reported no statuses for either current head. The commit workflow lookup for the current development SHA returned no PR-triggered workflow runs. Therefore **neither current head is verified by this audit as passing CI**. An empty status response is not a pass.
- Release baseline: `master` remains the designated prior-release baseline under repository policy, but this audit did not establish a current public package/release-to-SHA mapping. Do not claim that the current master SHA is verified as the last published package without that evidence.
- No merge, branch replacement, branch deletion, NuGet publication, or deployment was performed.

### Interpretation

The branch count is clean, but the repository is not at a publish gate: the branches are not equal, the development head has no CI evidence in the queried status sources, and master contains two commits of unique history that need deliberate review. The branch timestamps show when refs were last modified; they do not establish correctness.

## Canonical branch policy

Exactly two persistent branches are intended:

- development — authoritative integration branch and source of truth for the next release.
- master — prior-release baseline until the owner replaces it with the verified development candidate.

Do not merge legacy master into development for release preparation. Do not create new branches or delete branches on the owner's behalf. Branch deletion is an owner decision after any unique work has been reviewed and preserved.

## Current observed state — 2026-10-22

| Branch | Observed head | Disposition |
|---|---|---|
| development | 826b011498942214f8f75c8f2f4406acfa1442a7 | Preserve; active alpha.7 candidate. |
| master | 732690a2b37ac23971b3f9aea1cc595fd68c0a7f | Preserve as prior-release baseline until owner-authorized replacement. |

The live branch inventory at this checkpoint contained exactly these two branches. No branch was deleted or rewritten as part of this record.

## Verification evidence

The [pack-and-publish workflow run 1114](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/actions/runs/39866658509) ran on development candidate 826b0114: build/test, public-package compatibility, and source integration succeeded; NuGet publication was skipped by the hard gate. This evidence belongs to that SHA only and does not authorize publication or branch replacement.

## Reconciliation rules

1. Compare any noncanonical branch's commits and file changes against current development.
2. Inspect actual source, tests, documentation, workflow changes, and related pull-request history.
3. Record work already integrated, unique useful work, conflicts, and obsolete or unsafe material.
4. Port useful work selectively and coherently to development; do not blindly merge stale snapshots.
5. Run relevant verification against the resulting candidate.
6. Tell the owner which branch is ready for manual deletion and why. Never delete it for the owner.

An old inventory or pull-request comment is not proof that a branch still exists or that its work has been reconciled. Refresh live refs and inspect current files before making a disposition decision.

## Release boundary

Branch replacement and NuGet publication are separate actions. Keep the publication job hard-gated by && false unless the owner explicitly authorizes the exact package/version publication attempt.

---
<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
