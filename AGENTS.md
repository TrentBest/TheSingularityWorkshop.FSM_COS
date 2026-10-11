# Repository Instructions for AI Agents

## Branch policy — mandatory

The permanent branch model for this repository is exactly:

- `master` — stable, reviewed, release-ready history.
- `development` — active integration and verification.

Do not create additional persistent branches for features, documentation, experiments, release preparation, or agent handoffs. Work directly on `development` unless the owner explicitly authorizes a short-lived exploratory branch. Any explicitly authorized temporary branch must be reconciled and retired promptly; the normal target remains exactly two branches.

This rule applies to every contributor and every LLM/agent. A task-specific branch is not permission to abandon the two-branch policy.

## Integration before cleanup

Preserve and extend useful work. Never delete or recommend deleting a branch merely because its name looks old, its history diverged, or its work appears superseded.

For every noncanonical branch:
1. Compare its commits and file changes against `development`.
2. Read the actual source, tests, documentation, workflow changes, and relevant commit/PR history.
3. Identify what is already integrated, what remains useful, what conflicts with current architecture, and what is obsolete or unsafe.
4. Integrate useful, non-conflicting work into `development` in coherent, reviewable increments; adapt rather than blindly merge stale snapshots.
5. Run the relevant tests/builds and inspect workflow/release gates.
6. Record the disposition and evidence in `docs/BRANCH_RECONCILIATION.md`.
7. Tell the repository owner which exact branch is clear to delete and why. **The owner performs branch deletion.** Do not delete branches on the owner's behalf.

If the work is ambiguous or changes architectural direction, present the evidence and ask the owner for a directive. The default is to preserve useful, non-conflicting functionality—not to discard it and not to merge blindly.

## Architectural and release constraints

- Preserve dependency direction: lower-level packages must not depend on FSM_COS.
- Keep FSM_COS host-neutral and limited to composition/runtime responsibilities.
- Keep optional capabilities optional; do not add a dependency solely because a package exists.
- Do not publish to NuGet unless the repository owner explicitly authorizes that specific release.
- Never weaken or bypass the repository's disabled-by-default NuGet publish gate.
- Do not claim work is integrated, tested, deployed, or published without evidence.

## Before starting a task

Read `docs/PAUSE_POINT.md`, `docs/BRANCH_RECONCILIATION.md`, and the relevant working-task or architecture documents. Check the current branch heads and working CI status rather than relying on a prior session summary.
