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


## Branch activity and exact evidence reporting — mandatory

Whenever reporting repository health or preparing branch reconciliation:

1. Enumerate the complete live branch inventory from GitHub; do not rely on a remembered list or a partial recent-commit feed.
2. For every branch, resolve its current head and report the full SHA, committer timestamp in UTC, author, subject, and commit URL. Sort by newest activity first.
3. Report `development` and `master` separately. A branch timestamp proves only that its ref changed; it does not prove that the branch is tested or releasable.
4. Compare every noncanonical branch against current `development`; report ahead/behind counts, unique file changes, PR/review state, exact-head checks, and a reasoned disposition. Inspect actual file content before calling a change redundant or safe to retire.
5. Distinguish no checks, queued/in-progress checks, failures, and successful exact-head evidence. Never transfer a green result from an older SHA.
6. Treat `master` as the released baseline only when its exact SHA is supported by package/release evidence. Do not infer release alignment from the branch name.
7. Keep the permanent branch set at exactly `master` and `development`. A short-lived task branch requires explicit owner authorization, must start from current `development`, and must be reconciled promptly. It is never a third permanent branch.
8. Never merge, replace `master`, delete branches, publish packages, or deploy without the owner's explicit authorization for that exact action. Branch equality and green CI are evidence, not release authorization.

Refresh `docs/BRANCH_RECONCILIATION.md` during a branch audit. Keep old snapshots visibly historical and record exact heads and evidence scope.

## Architectural and release constraints

- Preserve dependency direction: lower-level packages must not depend on FSM_COS.
- Keep FSM_COS host-neutral and limited to composition/runtime responsibilities.
- Keep optional capabilities optional; do not add a dependency solely because a package exists.
- Do not publish to NuGet unless the repository owner explicitly authorizes that specific release.
- Never weaken or bypass the repository's disabled-by-default NuGet publish gate.
- Do not claim work is integrated, tested, deployed, or published without evidence.

## Before starting a task

Read `docs/PAUSE_POINT.md`, `docs/BRANCH_RECONCILIATION.md`, `docs/IMPLEMENTATION_DOCUMENTATION_MATRIX.md`, `.github/notes/DOCUMENTATION_AUDIT.md`, and the relevant working-task or architecture documents. For cross-repository or host-integration work, read [`ECOSYSTEM_INTEGRATION_AUDIT.md`](ECOSYSTEM_INTEGRATION_AUDIT.md) and [`CROSS_REPOSITORY_MISSED_INTEGRATIONS.md`](CROSS_REPOSITORY_MISSED_INTEGRATIONS.md), also read [`ECOSYSTEM_AUDIT_SCOPE_2026.md`](ECOSYSTEM_AUDIT_SCOPE_2026.md) to track coverage and remaining repositories, then re-check each affected repository's active branch before acting. Check the current branch heads and working CI status rather than relying on a prior session summary.


---



## Package-aware release branch semantics — mandatory

Branch meaning depends on whether this package has actually been published, not merely on the existence or name of a branch.

- **Never published:** `development` is the authoritative source of truth. `master` is pre-release scaffolding, not a released-package baseline, and may be identical to `development`. Before the first release, the owner may replace/reset the pre-release `master` tree to the verified, approved `development` candidate. Do not preserve artificial pre-release history as though it represented a NuGet release.
- **Previously published:** `master` must represent the exact source commit corresponding to the latest successfully published NuGet package. `development` remains the active forward-moving source. Do not move `master` merely to make it match newer development work.
- **First release transition:** after the exact candidate has passed its release gates and the owner explicitly authorizes publication, publish that approved candidate. Establish `master` at the exact released source commit. Afterward, development may advance independently.
- **Classification evidence:** verify publication history from the authoritative NuGet feed and release/publish evidence. Do not infer that a package was published from a project file, version string, tag, successful pack job, skipped publish job, or branch name.
- **Branch count:** the permanent target remains exactly `master` and `development`. For an unreleased package, equality is normal and desirable; for a released package, divergence is normal after subsequent development.
- **Authorization boundary:** this policy describes the intended release model; it is not blanket permission to publish, force-push, replace a branch, merge, delete branches, or deploy. Obtain explicit owner authorization for the specific operation. Never publish NuGet packages without explicit authorization.

When auditing, record the package's verified publication state as `never-published` or `published`, the latest published version and source SHA when applicable, both branch SHAs, and the evidence used to classify it. If publication state cannot be verified, mark it `unknown` and preserve both refs until clarified.

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
