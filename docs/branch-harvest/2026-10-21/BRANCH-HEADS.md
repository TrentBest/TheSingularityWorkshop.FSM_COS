# Operation Sith — branch-head recovery manifest

Captured 2026-10-21 22:36 UTC before owner-directed branch cleanup. These immutable commit IDs let a future recovery locate each branch tip even if the branch name is deleted. **This manifest records tips; it does not preserve unreachable Git objects by itself.** Before deleting branches, prefer keeping the open PRs or creating local bundles/tags if full history must remain reachable.

| Branch | Captured tip commit | Commit URL | Notes |
|---|---|---|---|
| `docs/alpha7-release-readiness` | `308d5e6afeb7515ce94be91e6f768eaa9008c273` | https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/commit/308d5e6afeb7515ce94be91e6f768eaa9008c273 | Readiness branch |
| `docs/fsm-cos-visual-contract-audit` | `2a0245a21b7f3e88d2c74f18171b16581b75a5b8` | https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/commit/2a0245a21b7f3e88d2c74f18171b16581b75a5b8 | Visual contract audit |
| `work/alpha7-living-docs-integration` | `19567ca49d5a49f13e05286301105b250bcd9e87` | https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/commit/19567ca49d5a49f13e05286301105b250bcd9e87 | Broad living-doc/UML integration; PR #34 |
| `work/ci-development-dependencies` | `3efe0cc1a58544df0340368ed7dd671f72258ca1` | https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/commit/3efe0cc1a58544df0340368ed7dd671f72258ca1 | Workflow/dependency proposal; archive only |
| `work/fsm-cos-living-svg-flow` | `1595e1b769b80b381e8881bed0acaf3187bdaaff` | https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/commit/1595e1b769b80b381e8881bed0acaf3187bdaaff | Living SVG flow |
| `work/fsm-cos-uml-class-diagrams` | `192c8912fa99560b16a47bc4f0c628b84fa089e3` | https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/commit/192c8912fa99560b16a47bc4f0c628b84fa089e3 | UML/class-contract docs |

## Safety and disposition

- Content snapshots for changed files are indexed in [README.md](README.md) and preserved alongside this manifest.
- Divergent workflow files are inert snapshots only. Keep the canonical `&& false` NuGet publish guard; no NuGet publish is authorized.
- Captured SHAs are recovery identifiers, not tags and not a guarantee that GitHub will retain unreachable commits indefinitely.
- Current development-head CI was still running at capture time; this file does not certify a release.
- No branch deletion, PR merge/closure, ref rewrite, or package publication was performed.
