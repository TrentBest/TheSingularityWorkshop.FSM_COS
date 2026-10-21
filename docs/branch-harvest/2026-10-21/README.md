# Operation Sith branch harvest — 2026-10-21

Preserved from `work/alpha7-living-docs-integration` to keep potentially valuable material recoverable before branch cleanup.

**Important:** historical status notes within these documents may be stale. This is a preservation archive, not proof that the archived work is integrated, tested on current `development`, or release-approved. Source blob hashes are recorded below.

- `docs/branch-harvest/2026-10-21/alpha7-integration-GUILLM.md` ← `GUILLM.md` (source blob `bcbf931fdbec907e5e035fafd5028024be7c1aba`, 28907 characters)
- `docs/branch-harvest/2026-10-21/EXECUTION_SEQUENCE.md` ← `docs/EXECUTION_SEQUENCE.md` (source blob `c1115f5aa12a2b7b2c0d1280f9bc711e3770c2ff`, 5014 characters)
- `docs/branch-harvest/2026-10-21/UML-README.md` ← `docs/UML/README.md` (source blob `4e52f32e651efa54ba471adc6fff5a6792a8eb43`, 10312 characters)
- `docs/branch-harvest/2026-10-21/DOCUMENTATION_STANDARD.md` ← `DOCUMENTATION_STANDARD.md` (source blob `8b2d63d5a1498895d51ace296ba87f2ff56ca2fa`, 35105 characters)
- `docs/branch-harvest/2026-10-21/RuntimeManifest-source-companion.md` ← `src/FSM_COS/RuntimeManifest.md` (source blob `53bc982ecda9270d6d2c80127a4defc59018fd25`, 4380 characters)
- `docs/branch-harvest/2026-10-21/RuntimeManifest-source-UML.md` ← `src/FSM_COS/RuntimeManifestUML.md` (source blob `32a6f91a31e28975aff6cb919aead6554111c5db`, 6659 characters)

- `docs/branch-harvest/2026-10-21/PER-TYPE-UML-PAGES.md` — ten per-type UML companion pages preserved with source blob hashes.
- `docs/branch-harvest/2026-10-21/SAMPLES-TESTS-AND-CI.md` — sample docs, test companions, documentation sample source, NuGet verification script, and proposed package workflow preserved as inert text.

This is a substantial recovery archive, but not a complete reconciliation of every changed file or all implementation diffs. The branch is not yet cleared for deletion; remaining source/project/workflow/documentation diffs still need a disposition.

- `docs/branch-harvest/2026-10-21/ADDITIONAL-DOCS-1.md` — preserved AGENTS, index, NuGet README, README, SECURITY, readiness, AI composition, arbitration, architecture, and branch ledger snapshots.
- `docs/branch-harvest/2026-10-21/ADDITIONAL-DOCS-2.md` — preserved MicroBundle consumption, continuation, dependency checklist, development, domain/roadmap, ecosystem map, Gallery, living-documentation audit, and manifest theory snapshots.
- `docs/branch-harvest/2026-10-21/ADDITIONAL-DOCS-3.md` — preserved pause point, runtime assembly/boundary/manifest, staged loading, theory, WebPage integration, FSM_COS overview, and release notes.
- `docs/branch-harvest/2026-10-21/ADDITIONAL-SVG-ASSETS.md` — preserved divergent arbitration and overview SVGs.

Branch comparison refreshed after the harvest: 97 ahead / 120 behind `development`. The active project file `src/FSM_COS/FSM_COS.csproj` is byte-identical across the branch and development despite the compare summary listing it as modified. The proposed branch package workflow is unsafe to merge as-is: its publish condition lacks the canonical `&& false` hard stop. It remains archived as inert text only. The integration branch's `DocumentationVisuals/Program.cs` and package-verifier differences are likewise archived, not adopted without regeneration/tests.

## Additional branch snapshots added in the accelerated pass

- `READINESS-BRANCH-SNAPSHOT.md` — readiness branch's unique readiness/README/index versions.
- `VISUAL-CONTRACT-BRANCH-SNAPSHOT.md` — visual-contract audit branch's audit and overview SVG.
- `CI-DEPENDENCY-BRANCH-SNAPSHOT.md` — CI dependency branch workflow and development guide, both archived as inert text.
- `LIVING-SVG-BRANCH-SNAPSHOT.md` — living-SVG branch's index, NuGet README, guide, preflight SVG, and generator snapshots.
- `UML-CONTRACT-BRANCH-SNAPSHOT.md` — UML branch's expanded arbitration, architecture, runtime assembly/manifest, and staged-loading docs.

The changed-file inventory from all six surplus FSM_COS branches has now been preserved in the archive as branch-specific snapshots, including code/tooling/workflow proposals in inert text where applicable. Byte-identical files are documented as such rather than duplicated as implementation changes. This materially protects the recoverable content if the owner deletes the source branches. It is not a claim that all archived material should be integrated, that branch commit metadata is preserved, or that current `development` passes CI.

- `docs/branch-harvest/2026-10-21/BRANCH-HEADS.md` — captured exact head SHAs and commit URLs for all six surplus branches before manual cleanup; warns that SHAs alone do not retain unreachable Git objects.

- `docs/branch-harvest/2026-10-21/FINAL-PRE-CLEANUP-DISPOSITION.md` — final six-branch disposition matrix, exact compare counts, implementation/workflow findings, open PR status, and exact-HEAD CI status as of the last round.
