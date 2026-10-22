<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# FSM_COS Branch Reconciliation Ledger

**Maintainer-only release record.** This ledger documents branch policy and the current observed state; it is not consumer documentation.

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
