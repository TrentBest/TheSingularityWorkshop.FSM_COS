<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# Repository Instructions for AI Agents


## Branch policy — mandatory

The permanent branch model for this repository is exactly:

- `development` — the source of truth for active work and the next release candidate.
- `master` — the previously released baseline until the owner-authorized release operation replaces it with the approved `development` candidate.

Do not create additional persistent branches for features, documentation, experiments, release preparation, or agent handoffs. Work directly on `development` unless the owner explicitly authorizes a short-lived exploratory branch. The release owner may direct replacement of `master` with the verified `development` candidate; this does not authorize NuGet publication. Any explicitly authorized temporary branch must be reconciled and retired promptly; the normal target remains exactly two branches.

This rule applies to every contributor and every LLM/agent. A task-specific branch is not permission to abandon the two-branch policy.

## Integration before cleanup

Preserve and extend useful work. Never delete or recommend deleting a branch merely because its name looks old, its history diverged, or its work appears superseded. For release preparation, however, do not treat legacy `master` as a source to merge into `development`: `development` is authoritative for the next release, and the release operation replaces the old `master` tree after verification.

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

Read `docs/PAUSE_POINT.md`, `docs/BRANCH_RECONCILIATION.md`, `docs/IMPLEMENTATION_DOCUMENTATION_MATRIX.md`, `.github/notes/DOCUMENTATION_AUDIT.md`, and the relevant working-task or architecture documents. For cross-repository or host-integration work, read [`ECOSYSTEM_INTEGRATION_AUDIT.md`](ECOSYSTEM_INTEGRATION_AUDIT.md) and [`CROSS_REPOSITORY_MISSED_INTEGRATIONS.md`](CROSS_REPOSITORY_MISSED_INTEGRATIONS.md), then re-check each affected repository's active branch before acting. Check the current branch heads and working CI status rather than relying on a prior session summary.


---
<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
