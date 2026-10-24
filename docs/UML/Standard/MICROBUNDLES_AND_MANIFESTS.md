<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# MicroBundles and Manifests

## Purpose
Answer: **Which capability units does this repository define or consume, how are they identified and composed, and what contract does each expose?**

A MicroBundle is a domain-defined capability unit. It is not automatically synonymous with a project, assembly, package, deployment unit, or runtime process. Follow the actual MicroBundleDomain contract and source implementation.

## Inventory rule
Every MicroBundle defined locally in the repository must appear in a generated or maintained inventory and have a companion page. Identify separately:
- locally defined MicroBundles;
- external MicroBundle contracts/references;
- sample/demo MicroBundles;
- test-only/fake MicroBundles;
- requested manifest roots and transitive dependencies;
- unresolved or dynamically supplied identities.

For each local definition document:
- stable identity, name, version/compatibility policy, and owning source;
- capability purpose, contract, inputs/outputs, configuration, and side effects;
- dependency identities and why each dependency is required;
- load stage/order and lifecycle/state transitions where implemented;
- arbitration/convergence participation and bounds where applicable;
- failure behavior, validation, resource limits, and trust/security assumptions;
- tests, consumer/use cases, package/project references, and evidence revision;
- publication/distribution status, explicitly separating source presence from release availability.

## Manifest and graph views
Show requested roots separately from dependency closure. Label edges with evidence (manifest declaration, catalog resolution, source reference, test fixture, or inferred relation). Identify validation and cycle/duplicate handling, dependency ordering, failure propagation, and any bounded arbitration. A manifest describing a capability does not by itself prove artifact retrieval, content-hash verification, loading, activation, or execution.

## FSM_COS boundary
The host supplies a catalog and optional configuration and owns application execution, lifecycle, UI, and presentation. FSM_COS resolves and validates the requested dependency closure, composes dependencies in order, performs bounded arbitration, and returns a RuntimeAssembly. Do not diagram deferred-load metadata as if it implements background localization, downloading, promotion, or activation unless those behaviors are separately implemented and verified.

## Acceptance
The inventory accounts for every in-repository definition within its declared scan scope; all entries link to source/tests; unresolved identities are visible; local, external, example, and test-only bundles are distinguished; and CI reports coverage without silently treating unknowns as zero.

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
