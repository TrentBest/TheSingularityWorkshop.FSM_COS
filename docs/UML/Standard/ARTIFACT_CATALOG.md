# FSM_COS UML and Living-Documentation Artifact Catalog

This is the actionable inventory for the FSM_COS UML documentation system. It translates the focused view specifications in this directory into concrete artifacts, coverage rules, and evidence labels. The catalog is a target checklist—not a claim that every artifact or generator already exists.

## Canonical organization

Use these reader-facing families as the intended organization for new or reorganized artifacts:

- `docs/UML/Class Diagrams/` — one AST-oriented declaration diagram per class, struct, record, interface, enum, delegate, or other supported declared type.
- `docs/UML/Class Neighborhoods/` — one consumer-first neighborhood per in-scope focal declaration, showing actual consumers above, the focal target centered, collaborators/results below, and project/package/framework references to the right.
- `docs/UML/Sequence Diagrams/` — interaction timelines, calls and returns, branches, loops, exceptions, and measured trace overlays where a real run exists.
- `docs/UML/Use Cases/` — one file per use case, organized by stable category.
- `docs/UML/Architecture/` — system/component boundaries, dependency direction, composition and handoff.
- `docs/UML/Deployment/` — hosts, processes, storage, network, trust and runtime topology.
- `docs/UML/Data Models/` — data cards, schemas, ownership, transformations, serialization and lineage.
- `docs/UML/MicroBundles/` — locally defined MicroBundle inventory, manifests, versions, dependency edges, configuration and composition paths.
- `docs/UML/Traceability/` — requirement-to-contract-to-test-to-evidence links, decisions and coverage.
- `docs/UML/Indexes/` — generated or maintained navigation and artifact coverage inventories.

The existing `ClassDiagram/`, `src/`, and `tests/` companions remain valid current locations until artifacts are deliberately reconciled and links updated. Do not perform a broad move solely to make folder names match this target. Preserve history and working links during any later migration.

## Required artifact matrix

| Artifact family | Minimum coverage | Ground truth and evidence | Status rule |
|---|---|---|---|
| Type/class AST diagrams | One focal declaration per diagram; identify multi-declaration source files explicitly. | Compiler-backed syntax/semantic model; source path, revision, declaration identity and span. | Mark source-derived only when generated from source; otherwise manual. |
| Consumer-first class neighborhoods | Every in-scope focal type; real consumers above, target centered, dependencies/results below, packages/projects/frameworks to the right. | Source symbols, project references, tests, or clearly marked authored context. | Record scan scope; missing consumers in a partial scan are unknown, not absent. |
| Expandable UML data cards | Each card has a colored, thick border, clear header, readable body and nested detail; click/keyboard expand when supported. | Stable declaration identity and supported levels of detail. | Distinguish static card links from truly interactive cards. |
| Sequence diagrams | Important public workflows, key collaboration paths, failures, and bounded arbitration; calls, returns, guards/branches and loops. | Source-shaped flow or captured runtime trace, explicitly labeled. | Never imply captured execution when the sequence is authored or inferred. |
| Complexity/performance overlays | Relevant methods: static time/space complexity, allocation or risk notes; separately show benchmark/test observations. | Algorithm/source analysis for Big-O; reproducible benchmark output for measurements. | Never present Big-O as a latency promise or infer practical performance from complexity alone. |
| Activity/control-flow diagrams | Branches, loops, exceptions, early returns and meaningful algorithm paths for critical logic. | Compiler/source control flow or clearly marked authored algorithm model. | Distinguish a source-derived control-flow graph from a simplified teaching diagram. |
| State/lifecycle diagrams | Each type or workflow with meaningful states, transitions, guards, ownership and terminal/failure states. | Source transitions, tests, explicit contract, or captured runtime observations. | Do not invent states to fill a diagram. Mark missing lifecycle evidence. |
| Use cases | One file per scenario, with actor/consumer, goal, trigger/preconditions, success outcome, alternatives/failures and evidence/status. | Human/LLM-authored intent grounded in source, product requirements and tests. | AST/reflection alone cannot establish user intent. |
| Architecture/component diagrams | Composition flow, responsibility boundaries, dependency direction, public handoffs and non-goals. | Project files, package metadata, source contracts and integration tests. | Separate current implementation from intended architecture. |
| Deployment/runtime topology | Relevant host/process, storage, network, trust boundaries and external services. | Actual deployment configuration and verified integration, not hypothetical architecture alone. | Mark conceptual/proposed topologies explicitly. |
| Data models and lineage | Request/result/context models, ownership, transformations, configuration, serialization boundaries and provenance. | Source declarations, serializers, tests and real payload evidence where available. | Distinguish declared schema from observed payload and inferred meaning. |
| MicroBundle/manifest inventory | Every MicroBundle defined in this repository; identity, purpose, contract, version, dependencies, configuration, failure modes and composition use. | Source inventory, manifest/catalog implementation and composition tests. | An inventory entry does not prove runtime-discoverable integration. |
| Requirement/test/decision traceability | Connect each material requirement or contract to implementation, tests, diagrams and decisions. | Test identifiers/results, source locations, decisions and CI evidence. | Missing links remain visible as gaps; do not mark inferred coverage as verified. |
| Indexes and coverage ledger | Navigable links to all applicable artifacts, owners/targets and current coverage status. | File inventory and declared scan scope. | Empty folders are not completed coverage; every in-scope target has a status. |

## Use-case categories

Keep one use case per file and group by these categories when applicable:

1. **User Goals** — the outcome a host, developer, or operator needs.
2. **API / Contract** — public entry points, validation, outputs and failure semantics.
3. **Data / Serialization** — manifest, configuration, context, identity and result handling.
4. **Operational** — loading, recovery, limits, diagnostics and lifecycle.
5. **Integration** — catalog/provider/host interactions and responsibility handoffs.
6. **Performance** — scale-sensitive paths, cost drivers, limits and measured evidence.
7. **Security / Abuse** — invalid inputs, untrusted artifacts, resource exhaustion and boundary enforcement.
8. **Maintenance / Evolution** — version compatibility, migration and extension points.

Only include categories that have real scenarios; explicitly mark categories not applicable in the inventory rather than creating empty documents.

## Recursive card and relationship contract

A card progresses through the deepest level the evidence supports:

1. **Identity:** declaration kind and name.
2. **Contract:** members, signatures, types, parameters, return values and modifiers.
3. **AST/declaration:** compiler-backed syntax tree, nesting and member structure.
4. **Implementation:** method bodies or deeper source detail when available.

Relationships must have a stable source/target identity, a relationship kind, a label, and provenance. Use compiler-backed Roslyn analysis for syntax and symbol relationships; reflection for runtime-visible metadata; authored material for use-case intent; tests for verified behavior; and captured runs for runtime observations. These evidence sources complement one another but are not interchangeable.

## Evidence and completion states

Each artifact in the coverage ledger must use one of these states:

- **Required** — applicable and expected, but not yet evidenced as complete.
- **Not applicable** — with a concrete reason.
- **Manual** — authored and source-reviewed, not generated.
- **Source-derived** — produced from a declared source revision and analysis scope.
- **Runtime-captured** — backed by an identified execution/run.
- **Planned** — intended future artifact, not implemented.

For generated artifacts record target revision, source scope, declaration identity/source span where applicable, analysis method, generator/tool version, provenance, unresolved relationships, and regeneration path. For manual artifacts link source/tests and state the review basis.

## Minimum completion gate

- [ ] Inventory all in-scope source files and declared types, including multiple declarations in one file.
- [ ] Assign a status to every applicable artifact family and explain every not-applicable decision.
- [ ] Link each type page, AST diagram, neighborhood, relevant use cases, source and tests.
- [ ] Inventory every locally defined MicroBundle and map its real manifest/catalog/composition path.
- [ ] Separate static complexity from measured performance and source structure from runtime behavior.
- [ ] Validate relative links and artifact metadata.
- [ ] Regenerate source-derived outputs and detect missing/stale outputs where tooling exists.
- [ ] Report the exact scope of automation; do not claim repository-wide coverage or CI enforcement until verified.
- [ ] Keep the documentation index, README, and readiness report aligned with the evidence.
- [ ] Do not merge, publish, or change package behavior as a side effect of a documentation pass.

## Current FSM_COS coverage boundary

The current [UML index](../README.md) identifies manually maintained type companions and a limited set of consumer-first SVG diagrams. It explicitly states that most declarations do not yet have individual SVG class diagrams and that full-source generation/stale-output enforcement is not established by the documents alone. Treat this as a coverage gap to reconcile against the inventory, not as completed automation.

