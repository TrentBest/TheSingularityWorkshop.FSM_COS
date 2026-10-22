<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# FSM_COS Branch Reconciliation — Sith Policy

**Audience:** maintainers only. This is not product documentation.

**Policy:** the intended persistent branches are exactly `master` and `development`. Preserve useful work before branch removal. Do not force-reset or merge divergent histories merely to make branch counts look tidy. Branch deletion is an owner action after reviewing this ledger.

**Audit date:** 2026-10-22 (21:04 UTC). Live branch refs and both comparison directions were queried for this refresh.

## Persistent branches

| Branch | Role | Action |
|---|---|---|
| `master` | Published/release line | Preserve. Its release workflow and package compatibility protections must not be overwritten casually. |
| `development` | Active integration and documentation line | Preserve. The documentation revision is being made here; verify the live head before candidate-specific CI or release claims. |

## Surplus branch inventory

| Branch | Evidence | Reconciliation decision |
|---|---|---|
| `work/alpha7-educational-docs` | PR #36 is merged into `development` at `afd422b6e5d713776125bbce1427caa2875b7e2c`. A fresh comparison used `work/alpha7-educational-docs` as base and `development` as head; GitHub reported `ahead_by=67`, `behind_by=0`, `merge_base=33456c23d94e147741f27554d6328b19a58532f8`. Reverse comparison confirms `development` is 67 commits ahead and the surplus branch is 0 commits ahead. PR #36 is closed and merged at `afd422b6e5d713776125bbce1427caa2875b7e2c`; its head was `33456c23d94e147741f27554d6328b19a58532f8`. The branch has no unique commits or unmerged work. | **Safe for owner-reviewed manual deletion**, provided a fresh comparison still agrees. No cherry-pick or merge was indicated by that comparison. |

The fresh branch inventory contains exactly `master`, `development`, and `work/alpha7-educational-docs`. The educational docs are already merged into `development`; the remaining exception is only the stale branch ref. Re-query before owner deletion because this inventory is time-sensitive.

## Do not conflate branch cleanup with release integration

Draft PR #35 (`development` → `master`) remains open and should remain draft while the two release histories are reconciled. The fresh comparison observed **888 commits ahead and 48 behind** for development relative to master. That is substantial divergence, not a routine merge. Master has release workflow, public-package compatibility, release-history, and documentation changes that must be preserved deliberately. In particular, retain the master-only publication safety guard; do not publish as part of documentation cleanup.

The current development head at audit time was `8dbe9207d811678aac4d2efd54d9d7e87eeea802`; package workflow and CodeQL were green on that exact head. This is not release certification and does not resolve the separate master/development history divergence. The last recorded candidate-specific CodeQL/package evidence in PR #35 remains historical and must not be treated as current-head certification. It is historical evidence only, not proof that the current head passed those gates. The documentation commits must be verified on their own exact candidate SHA.

## Owner checklist

- [x] Recheck live branches and confirm `work/alpha7-educational-docs` is fully reconciled: PR #36 merged; 67 commits behind development; zero unique commits.
- [ ] Owner: remove the stale `work/alpha7-educational-docs` branch in GitHub manually; fresh comparison confirms it has no unique commits.
- [ ] Keep `master` and `development`; do not force-push either as part of branch cleanup.
- [ ] Reconcile PR #35 and master-only release safeguards as a separate, evidence-driven task.
- [ ] Do not publish a package without explicit owner authorization.

This ledger records observed state, not an automatic branch-deletion action or release-readiness certification.

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
