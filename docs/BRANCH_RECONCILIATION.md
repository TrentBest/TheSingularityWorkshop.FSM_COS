# FSM_COS Branch Reconciliation — Sith Policy

**Policy:** the intended persistent branches are exactly `master` and `development`. Preserve useful work before branch removal. Do not force-reset or merge divergent histories merely to make branch counts look tidy. Branch deletion is an owner action after reviewing this ledger.

**Audit date:** 2026-10-22 (GitHub refs queried during this audit).

## Persistent branches

| Branch | Role | Action |
|---|---|---|
| `master` | Published/release line | Preserve. Its release workflow and package compatibility protections must not be overwritten casually. |
| `development` | Active integration and documentation line | Preserve. Current development head at the end of this audit: `11b266f5971392577d1eb685035745be05bc3eb6`. |

## Surplus branch inventory

| Branch | Evidence | Reconciliation decision |
|---|---|---|
| `work/alpha7-educational-docs` | Its PR #36 is merged into `development` at `afd422b6e5d713776125bbce1427caa2875b7e2c`. Comparing `development` to this branch returned `ahead_by=0`, `behind_by=7`: it has no commits missing from development and is seven commits behind. | **Safe for owner-reviewed manual deletion.** The useful work is already in development. No cherry-pick or merge is indicated by the current comparison. |

There were three branches in the branch-search result: `master`, `development`, and `work/alpha7-educational-docs`. The extra branch is the only identified Sith-policy exception in that result.

## Do not conflate branch cleanup with release integration

Draft PR #35 (`development` → `master`) remains open and must stay draft while the two release histories are reconciled. The latest comparison reported **844 commits ahead and 48 behind** for `development` relative to `master`. This is a substantial divergence, not a routine merge. Master has release workflow, public-package compatibility, release-history, and documentation changes that must be preserved deliberately. In particular, retain the master-only publication safety guard; do not publish from this cleanup.

The last candidate-specific green CI evidence recorded in PR #35 was for `6dabffc5d9be6c282b7ca47d2f2dd4ad1b8fe04e`, not the current development head. It is historical evidence only, not proof that the current head passed those gates.

## Owner checklist

- [ ] Review this ledger and confirm `work/alpha7-educational-docs` can be deleted manually.
- [ ] Remove the surplus branch in GitHub only after confirming the merged PR and this ledger.
- [ ] Keep `master` and `development`; do not force-push either as part of branch cleanup.
- [ ] Reconcile PR #35 and master-only release safeguards as a separate, evidence-driven task.
- [ ] Do not publish a package without explicit owner authorization.

This ledger records observed state, not an automatic branch-deletion action or release-readiness certification.
