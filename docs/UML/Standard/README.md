<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# UML Documentation Standard

This directory decomposes the Workshop's canonical UML/documentation policy into focused, maintainable specifications. The shared policy lives in the repository-root [Documentation Standard](../../../DOCUMENTATION_STANDARD.md); the FSM_COS visual implementation is described by [Living UML Visual Style](../../UML_VISUAL_STYLE.md).

## Cross-repository integration contract

- [GUI / FSM_COS UML coordination](../../../GUILLM.md) — consumer boundary, required source-to-SVG contract, and evidence needed before the UML gate can close.

## Actionable catalog

- [FSM_COS UML and Living-Documentation Artifact Catalog](ARTIFACT_CATALOG.md) — concrete artifact families, minimum coverage, use-case categories, evidence states, and completion gate. This is the implementation checklist; the focused files below define each view in detail.

## View specifications

- [Class / Type Diagrams](CLASS_DIAGRAMS.md) — one focused diagram per declaration.
- [Class Neighborhoods and Expandable Data Cards](CLASS_NEIGHBORHOODS.md) — consumers, focal type, dependencies, package edges, and recursive detail.
- [Sequence Diagrams and Complexity](SEQUENCE_DIAGRAMS_AND_COMPLEXITY.md) — call traces, complexity, allocations, and evidence limits.
- [Activity and Control Flow](ACTIVITY_CONTROL_FLOW.md) — branches, loops, exceptions, and algorithm paths.
- [State and Lifecycle Diagrams](STATE_LIFECYCLE.md) — state, transition, guard, and lifecycle ownership.
- [Use Cases](USE_CASES.md) — goal-oriented scenarios, one file per use case, stable categories.
- [Architecture and Components](ARCHITECTURE_COMPONENTS.md) — project/package boundaries and dependency direction.
- [Deployment and Runtime Topology](DEPLOYMENT_TOPOLOGY.md) — processes, hosts, storage, network and trust boundaries.
- [Data Models and Lineage](DATA_MODELS_AND_LINEAGE.md) — shapes, ownership, transformations, serialization, and provenance.
- [MicroBundles and Manifests](MICROBUNDLES_AND_MANIFESTS.md) — inventory every locally defined MicroBundle and document its contract.
- [Requirements, Tests, and Decisions](TRACEABILITY.md) — connect goals, contracts, tests, evidence, and architecture decisions.

## Inventory and status

Every repository adopting the standard maintains an inventory of applicable views and labels each **required**, **not applicable (with reason)**, **manual**, **source-derived**, **runtime-captured**, or **planned**. The inventory must distinguish an empty folder from completed coverage. Generated artifacts record target revision, analysis scope, provenance, unresolved relationships, generator/tool version, and regeneration instructions.

The standard describes the intended end state. Unless repository CI explicitly proves it, do not assume the generator, full-source inventory, complexity analyzer, MicroBundle scanner, or stale-output gate exists. See the repo's current [UML index](../README.md) for what is currently documented.

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
