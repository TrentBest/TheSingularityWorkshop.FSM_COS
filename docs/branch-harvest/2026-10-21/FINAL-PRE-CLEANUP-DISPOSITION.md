# Operation Sith — final pre-cleanup disposition (2026-10-21)

## Decision summary

The six surplus FSM_COS branch tips have been captured in [BRANCH-HEADS.md](BRANCH-HEADS.md). Their divergent changed-file content has been preserved in the neighboring recovery snapshots indexed by [README.md](README.md). No branch has been deleted or rewritten by this work.

## Per-branch disposition

| Surplus branch | Current compare against development | Preserved content / disposition |
|---|---:|---|
| `docs/alpha7-release-readiness` | 4 ahead / 412 behind; 3 changed files | Readiness file, README and documentation-index versions preserved in `READINESS-BRANCH-SNAPSHOT.md`. |
| `docs/fsm-cos-visual-contract-audit` | 5 ahead / 139 behind; 2 changed files | Audit and overview SVG preserved in `VISUAL-CONTRACT-BRANCH-SNAPSHOT.md`. |
| `work/alpha7-living-docs-integration` | 97 ahead / 135 behind; 67 changed files | Expanded docs, UML companions, sample/test docs, generator/verifier snapshots, release notes, assets and workflow text preserved across the initial, `ADDITIONAL-*`, `SAMPLES-TESTS-AND-CI.md`, and `ADDITIONAL-SVG-ASSETS.md` archives. |
| `work/ci-development-dependencies` | 2 ahead / 442 behind; 2 changed files | Development guide and workflow snapshot preserved in `CI-DEPENDENCY-BRANCH-SNAPSHOT.md`. |
| `work/fsm-cos-living-svg-flow` | 8 ahead / 139 behind; 5 changed files | Index, NuGet README, living-SVG guide, preflight SVG and sample generator preserved in `LIVING-SVG-BRANCH-SNAPSHOT.md`. |
| `work/fsm-cos-uml-class-diagrams` | 11 ahead / 139 behind; 5 changed files | Arbitration, architecture, runtime assembly/manifest and staged-loading docs preserved in `UML-CONTRACT-BRANCH-SNAPSHOT.md`. |

## Implementation and safety findings

- The alpha.7 integration branch compare reports `src/FSM_COS/FSM_COS.csproj` as modified, but direct content/blob comparison showed it byte-identical to `development`. No runtime `.cs` implementation file appears in the current changed-file compare. Unique code/tooling content is in sample generator and package verifier, preserved verbatim in the archive; these were not silently adopted.
- Both divergent package workflow proposals are archived as text only. The alpha.7 proposal lacks the canonical `&& false` publication hard stop. Do not copy it over the active workflow.
- PR #34 remains open/draft; PR #35 remains open/draft. Neither was merged or closed. The alpha.7 integration branch has 97 commits ahead, but file snapshots do not preserve all commit ancestry.
- Captured branch SHAs are locators, not durable backups of unreachable Git objects. If commit history itself must survive deletion, retain reachable refs or make repository-native backups before deletion.
- No NuGet publication is authorized.

## Exact-HEAD release verification

Current `development` SHA at this update: `272b0a75c1fedc46fecaa847ce4dc3a730bbe161`.

- Pack and publish workflow: [run 39687643748](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/actions/runs/39687643748) — in progress.
- CodeQL: [run 39687643867](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/actions/runs/39687643867) — in progress.
- Current-HEAD CI certification: **not yet earned**. Historical 22/23 (96%) readiness applies only to its previously tested candidate and is not current certification.

This document records a content-recovery decision, not an instruction to merge, publish, or delete. Owner retains final branch-deletion authority.
