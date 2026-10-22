<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# FSM_COS Branch Reconciliation — Sith Policy

**Audience:** maintainers only. This is not product documentation.

**Policy:** the intended persistent branches are exactly `master` and `development`. Preserve useful work before branch removal. Do not force-reset or merge divergent histories merely to make branch counts look tidy. Branch deletion is an owner action after reviewing this ledger.

**Audit date:** 2026-10-22. Branch refs and comparisons were queried during the audit; recheck live state before acting.

## Persistent branches

| Branch | Role | Action |
|---|---|---|
| `master` | Published/release line | Preserve. Its release workflow and package compatibility protections must not be overwritten casually. |
| `development` | Active integration and documentation line | Preserve. The documentation revision is being made here; verify the live head before candidate-specific CI or release claims. |

## Surplus branch inventory

| Branch | Evidence | Reconciliation decision |
|---|---|---|
| `work/alpha7-educational-docs` | PR #36 is merged into `development` at `afd422b6e5d713776125bbce1427caa2875b7e2c`. The comparison observed `ahead_by=0`, `behind_by=7`: the branch had no commits missing from development and was seven commits behind. | **Safe for owner-reviewed manual deletion**, provided a fresh comparison still agrees. No cherry-pick or merge was indicated by that comparison. |

The branch-search result at audit time contained `master`, `development`, and `work/alpha7-educational-docs`. Re-query before deletion because this inventory is time-sensitive.

## Do not conflate branch cleanup with release integration

Draft PR #35 (`development` → `master`) remains open and should remain draft while the two release histories are reconciled. The comparison observed **844 commits ahead and 48 behind** for development relative to master. That is substantial divergence, not a routine merge. Master has release workflow, public-package compatibility, release-history, and documentation changes that must be preserved deliberately. In particular, retain the master-only publication safety guard; do not publish as part of documentation cleanup.

The last candidate-specific green CI evidence recorded in PR #35 was for `6dabffc5d9be6c282b7ca47d2f2dd4ad1b8fe04e`, not the current development head at the time of the documentation pass. It is historical evidence only, not proof that the current head passed those gates. The documentation commits must be verified on their own exact candidate SHA.

## Owner checklist

- [ ] Recheck live branches and confirm `work/alpha7-educational-docs` remains fully reconciled.
- [ ] Remove the surplus branch in GitHub manually only after confirming the merged PR and current comparison.
- [ ] Keep `master` and `development`; do not force-push either as part of branch cleanup.
- [ ] Reconcile PR #35 and master-only release safeguards as a separate, evidence-driven task.
- [ ] Do not publish a package without explicit owner authorization.

This ledger records observed state, not an automatic branch-deletion action or release-readiness certification.

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
