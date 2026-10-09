# FSM_COS Branch Reconciliation Ledger

**Owner's directive (2026-10-09):** preserve useful ecosystem work, make it work together, and reduce the repository to exactly `master` and `development`. The owner will delete branches only after being told that a specific branch is clear to delete. When architectural intent is ambiguous, present the evidence and request a directive rather than discarding work or merging blindly.

**Status:** first-pass inventory. No extra branch is yet declared safe to delete. This ledger must be updated as branches are inspected, changes are integrated, and verification passes.

## Mandatory operating rule

See root [AGENTS.md](../AGENTS.md). Do not create more persistent branches. Do not delete branches. Reconcile each branch into `development` where useful and non-conflicting, verify it, then tell the owner when that exact branch is safe for owner-controlled deletion. No NuGet publishing without explicit owner authorization.

## Canonical branches

- `master` — stable/release-ready history.
- `development` — active integration.

## First-pass branch inventory

All comparisons below are against the current `development` ref at the time of the 2026-10-09 audit. “Ahead” means commits reachable from that branch but not from `development`; it does not prove every commit is still unique in meaning. Changed-file lists are initial leads, not a substitute for reading the actual diffs.

| Branch | Ahead / behind | Initial contents indicated by comparison | Initial disposition |
|---|---:|---|---|
| `arbitration-weapon-magic` | 4 / 150 | `ArbitrationContext.cs`; new weapon/magic arbitration guide and tests; README note | Inspect behavior/tests and whether domain-specific example belongs in kernel tests/docs. |
| `architecture/domain-owned-microbundle-contract` | 20 / 110 | Large README/architecture/MicroBundles/development updates; new compute-scale, dependencies, performance docs; workflow and project changes | High-value architecture/doc candidate; inspect current accuracy, dependency direction, and release gate before selectively integrating. |
| `chore/standardize-ci-fsm-cos` | 1 / 71 | Package workflow only | Compare workflow carefully; retain useful CI changes without weakening the disabled NuGet publish gate. |
| `docs/ecosystem-documentation-standard` | 4 / 37 | New documentation index and ecosystem documentation standard; README links | Inspect against current documentation standard/index; likely selective documentation reconciliation. |
| `docs/master-readme-standard` | 55 / 110 | Large README and docs set, compute-scale/dependencies/performance/releasing docs, runtime source/tests, project/workflow changes | Broad mixed branch; do not merge wholesale. Review source/test changes separately from docs and workflow. |
| `docs/readme-standard` | 16 / 71 | README/docs revisions; new FSM_API integration guide; workflow changes | Compare against current docs and other README branches to deduplicate and preserve unique useful guidance. |
| `feature/ai-exchange-foundation` | 4 / 136 | New AI composition guide and SVG; README link | Inspect whether guide describes current optional capability contract accurately. |
| `feature/ai-gui-capability-handoff` | 4 / 133 | AI composition guide edits; new AI/GUI handoff tests and test project reference changes | Inspect tests and package references; preserve optionality and prevent production dependency creep. |
| `feature/runtime-assembly-capability-handoff` | 2 / 135 | RuntimeAssembly API changes and composition tests | Inspect carefully for API value, compatibility, and overlap with current RuntimeAssembly contract. |
| `forge/staged-manifest-load-plan` | 19 / 132 | Staged manifest loading types, evaluator, schedule/load-plan types, tests, design guide, workflow edits | Significant architectural feature. Compare with current manifest design and owner direction before deciding scope; do not silently discard. |
| `release/fsm-cos-alpha3-capability-handoff` | 2 / 134 | README and project/package metadata changes | Inspect version/dependency changes and preserve publish gate; likely release-history context rather than a merge target. |
| `release-prep/fsm-cos-1.0.0` | 24 / 132 | Staged-loading feature plus release guide/SVG, project metadata, workflow changes | Overlaps staged-load branch and includes release-sensitive workflow changes. Reconcile feature/docs separately from package/release configuration. |

## Reconciliation record

For each branch, add:
- relevant commits/files reviewed;
- work already present in `development`;
- useful work selected for integration and resulting commit(s);
- work intentionally not integrated, with a reason;
- build/test/workflow evidence;
- open questions or owner directive required;
- final state: **not reviewed**, **reviewed—work remains**, **integrated—verification pending**, **verified—owner may delete**, or **retained by owner direction**.

A branch may only be marked **verified—owner may delete** when all useful work is preserved or consciously dispositioned, no required work remains only on that branch, relevant verification has passed, and any associated open PR has been checked.

## Evidence links

- [All repository branches](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/branches/all)
- [Compare each branch against development](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/compare/development...master)
- [Open repository issues and design records](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/issues)
- [Pause point and pre-WebPage kernel gate](PAUSE_POINT.md)

## Release safety

This cleanup and integration work does not authorize a NuGet release. Do not publish any package until the owner explicitly says to do so.
