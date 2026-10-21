![The Singularity Workshop animated moniker](https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Renderer/development/docs/assets/moniker.svg)

# Repository Instructions for AI Agents

## Branch policy — mandatory

This repository has exactly two permanent branches:

- `master` — stable, reviewed, release-ready history.
- `development` — the only active integration and verification branch.

Do not create feature, documentation, experiment, handoff, or release branches. Work directly on `development`. Do not merge to `master`, publish a package, or deploy anything without the owner's explicit approval for that action.

## Keep the repository understandable

- Prefer the simplest implementation that accurately expresses the current design.
- Work from the current source, tests, project metadata, and documentation. Do not substitute generic templates or invent a parallel implementation.
- Do not preserve speculative or obsolete work merely because it exists in an old commit, branch, checklist, or handoff document. If reconciliation is unclear or expensive, discard the stale approach and rebuild from the current understanding.
- Keep each repository responsible for its own domain. Explain neighboring dependencies briefly and link to their authoritative documentation instead of copying their manuals.
- Distinguish implemented behavior, intended architecture, and future work. Do not make claims that source, tests, CI, or published metadata cannot support.
- Keep publication disabled by default. The owner must explicitly authorize the specific package release; a green build is not permission to publish.

## Before changing code or documentation

1. Read this repository's `README.md`, `DOCUMENTATION_STANDARD.md`, and `DOCUMENTATION_INDEX.md`.
2. Inspect the relevant implementation, tests, package metadata, and workflows on the current branch.
3. Check the current CI result and verify any claimed cross-repository dependency against that repository's actual source or published package.
4. Make the smallest coherent change that improves correctness or clarity.
5. Verify links and package contents when changing documentation or packaging.
6. Report the exact commit(s), checks performed, remaining blockers, and whether anything was published or deployed.

Do not treat old pause-point, branch-reconciliation, continuation, or alignment notes as mandatory instructions. Check them only when the current task genuinely needs their historical context.

---

**The Singularity Workshop**  
[GitHub](https://github.com/TrentBest) · [Coder Legion](https://coderlegion.com/) · [Patreon](https://www.patreon.com/c/TheSingularityWorkshop) · [PayPal](https://www.paypal.com/donate/?hosted_button_id=3Z7263LCQMV9J)  
[ FSM_API ](https://github.com/TrentBest/FSM_API) · [ FSM_Serialization ](https://github.com/TrentBest/TheSingularityWorkshop.FSM_Serialization) · [ WebPage ](https://github.com/TrentBest/WebPage) · [ FSM_COS Documentation Standard ](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/DOCUMENTATION_STANDARD.md)  
**Because state shouldn't be a mess.**  
*And because static boundaries are invitations to cause trouble.*
