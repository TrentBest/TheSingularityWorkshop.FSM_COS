<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# FSM_COS Release Pause Point

This is the maintainer handoff for FSM_COS release work. The checkpoint below is a historical snapshot from 2026-10-22, not a live status report; refresh branch heads, artifacts, and workflows before relying on it. It is not consumer-facing product documentation.

## Last recorded checkpoint — 2026-10-22 (historical snapshot)

- Release: TheSingularityWorkshop.FSM_COS 0.1.0-alpha.7.
- Authoritative branch: development.
- Candidate commit: 826b011498942214f8f75c8f2f4406acfa1442a7.
- Verification: [Pack and publish workflow run 1114](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/actions/runs/39866658509) passed build-and-test, public-package-compatibility, and source-integration; publish_nuget was skipped as intended.
- Permanent branches: development and master. Do not merge the old master into development. The owner intends to replace master with the verified development tree as a separate release operation.
- Publication: not authorized. Keep the workflow's && false hard gate unchanged.

The documentation reorganization and implementation-to-documentation audit are active on `development`; use the current [documentation index](../DOCUMENTATION_INDEX.md) and [implementation/documentation matrix](IMPLEMENTATION_DOCUMENTATION_MATRIX.md) for that work. Do not treat this 2026-10-22 pause point or its workflow link as evidence for the current candidate.

## What remains before declaring release-ready

1. Finish the source-to-doc audit: every consumer-facing claim must match the implementation and tests.
2. Inspect the exact candidate package artifact and record package ID/version, embedded commit, dependencies, contents, and hash.
3. Verify the current candidate's CodeQL and any other required checks against its exact SHA; older-SHA evidence does not transfer.
4. Resolve documentation integrity issues, including links from AGENTS.md to this pause point and the branch reconciliation ledger.
5. Keep browser-hosted rendering and accessibility claims explicitly unverified unless tested in a real browser.
6. Only after verification, present the candidate for the owner's release decision. Do not publish or replace master automatically.

## Product boundary to preserve

FSM_COS resolves a host-supplied catalog, validates the reachable dependency closure, loads bundles sequentially in dependency-first order, performs bounded arbitration, and returns a RuntimeAssembly. The host owns application execution, delivery/localization, presentation, and lifecycle. Staged schedule/load-plan metadata does not implement artifact localization, content-hash verification, parallel loading, live promotion, refresh, or atomic replacement.

## Resume instructions

Read this file, [Branch Reconciliation](BRANCH_RECONCILIATION.md), [Architecture](ARCHITECTURE.md), [Composition Failures](COMPOSITION_FAILURES.md), [Runtime Manifest](RUNTIME_MANIFEST.md), [RuntimeAssembly](RUNTIME_ASSEMBLY.md), [Arbitration](ARBITRATION.md), [Consuming MicroBundles](CONSUMING_MICROBUNDLES.md), and [Staged Manifest Loading](STAGED_MANIFEST_LOADING.md). Re-check live branch heads and CI before relying on this checkpoint.

---
<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
