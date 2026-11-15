<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# FSM_COS Branch Reconciliation Ledger

**Maintainer-only release record.** This ledger documents branch policy and the current observed state; it is not consumer documentation.

## Canonical branch policy

Exactly two persistent branches are intended:

- development — authoritative integration branch and source of truth for the next release.
- master — prior-release baseline until the owner replaces it with the verified development candidate.

Do not merge legacy master into development for release preparation. Do not create new branches or delete branches on the owner's behalf. Branch deletion is an owner decision after any unique work has been reviewed and preserved.

## Live branch activity audit — 2026-11-15 UTC

This inventory was captured before the documentation commit that added this snapshot. It supersedes the older 2026-10-22 inventory for branch-count and activity reporting; historical release evidence below remains scoped to its original SHA.

### Branch activity — newest first

| UTC committer time | Branch | Author | Latest subject | Head SHA |
|---|---|---|---|---|
| 2026-11-15 06:59:00 | [development](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/tree/development) | The Singularity Workshop | docs: record alpha.7 package artifact inspection | [401152dc5dbf80134ede4593db76ad82d06e3ece](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/commit/401152dc5dbf80134ede4593db76ad82d06e3ece) |
| 2026-11-14 11:27:04 | [master](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/tree/master) | github-actions[bot] | chore: refresh Sith policy evidence | [b42e505399d156bc9f80f027968f164149f5ba1f](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/commit/b42e505399d156bc9f80f027968f164149f5ba1f) |
| 2026-11-14 09:58:35 | [work/ciai-evidence-hardening](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/tree/work/ciai-evidence-hardening) | The Singularity Workshop | Fail closed on missing or ambiguous CiAi source runs | [0158272cd516312e41e8ffa6282882325ddde22a](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/commit/0158272cd516312e41e8ffa6282882325ddde22a) |
| 2026-11-03 21:05:08 | [policy/sith-branch-activity-audit](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/tree/policy/sith-branch-activity-audit) | The Singularity Workshop | Record live Sith branch activity and verification audit | [26fcd53770efdf01cedad8e9d884e0b3e13b8790](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/commit/26fcd53770efdf01cedad8e9d884e0b3e13b8790) |

### Reconciliation and verification

- Live inventory: four branches — `development`, `master`, `work/ciai-evidence-hardening`, and `policy/sith-branch-activity-audit`. Sith's exact-two-branch policy is **NON-COMPLIANT**.
- `development` is 166 commits ahead of `master`; `master` is 16 commits ahead of `development`; merge base `56ad34dbe4722ba1edf9a4da6a00812eeaa29101`.
- `work/ciai-evidence-hardening` is 2 commits ahead / 11 behind development. Its only changed file in the branch comparison is `.github/workflows/observe-commit.yml`; that file's blob SHA is identical on the branch and development (`2cbaaa0ca3946fca9128c7bec281ea9beccc9cc7`). The unique workflow behavior is therefore already present on development and was exercised successfully by [CiAi run 43679382105](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/actions/runs/43679382105) for source SHA `15188ee93572188194a2b2335e51dd805ab240ac`. Draft PR [#53](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/pull/53) is content-redundant; no merge is needed. **Ready to close as superseded; branch is a deletion candidate for the owner's eventual batch**, subject to a final whole-tree recheck immediately before deletion.
- `policy/sith-branch-activity-audit` is 2 commits ahead / 11 behind development. Its unique changes are `AGENTS.md` (+17/-1) and `docs/BRANCH_RECONCILIATION.md` (+27). The useful reporting requirements have been selectively incorporated into development while preserving explicit owner authorization for temporary branches. Its 2026-11-03 branch snapshot is stale and must not be copied. Draft PR [#52](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/pull/52) is now superseded by the adapted policy and current ledger; **ready to close as superseded, with the branch a deletion candidate for the owner's eventual batch**, subject to a final whole-tree recheck immediately before deletion.
- Latest exact-HEAD gates for `15188ee93572188194a2b2335e51dd805ab240ac`: package/build/source integration/public package compatibility, benchmarks, CiAi evidence, and CodeQL passed; Sith failed closed on the four-branch inventory. See the [release portfolio](../RELEASE_READINESS_PORTFOLIO.md) for exact run IDs and package hashes. These checks do not verify later documentation commits.
- `master` is 16 commits behind development but contains unique history; review those 16 commits before any owner-authorized replacement. Do not merge legacy master into development.
- No PR was merged, no branch was deleted, no branch ref was rewritten, and no NuGet package was published during this audit.

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
