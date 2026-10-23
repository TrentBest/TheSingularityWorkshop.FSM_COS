# GUILLM.md — GUI / FSM_COS UML Coordination

**Purpose:** Define the FSM_COS consumer requirements for GUI.SVG living UML work and keep the documentation gate tied to source, generated output, and verifiable CI evidence.

## Decision

The source-grounded UML requirement is **partially satisfied in design and documentation, not complete in implementation**. Keep the per-type UML coverage gate open until the evidence below is available on the exact integrated candidate. A manually authored Mermaid diagram or the existence of a GUI renderer is not, by itself, a source-derived UML pipeline.

The FSM_COS UML catalog at [docs/UML/Standard/ARTIFACT_CATALOG.md](docs/UML/Standard/ARTIFACT_CATALOG.md) is the concrete artifact checklist. GUI's [per-type documentation standard](https://github.com/TrentBest/TheSingularityWorkshop.GUI/blob/development/docs/CLASS_DOCUMENTATION_STANDARD.md) is the cross-repository reference for per-declaration coverage and target-centered layout.

## Consumer boundary

- **Source analysis:** C# parsing, compilation, symbol resolution, source spans, declaration selection, and source inventory belong in a build-time/documentation tool or code-domain adapter. They must not become dependencies of the GUI runtime renderer or FSM_COS runtime kernel.
- **Normalized model:** project declarations into a renderer-neutral, stable-identity model. Preserve declaration kind, fully qualified identity, source revision/path/span, members/signatures, relationship kind and endpoints, analysis scope, provenance, unresolved/external edges, and generator version.
- **Rendering:** GUI.SVG consumes the normalized diagram model and produces accessible SVG. Rendering must not invent relationships or act as the source of truth.
- **FSM_COS:** keep the integration consumer-owned. Do not reference unmerged staging internals, add GUI/Roslyn dependencies to the FSM_COS kernel, or claim a supported package contract until the exact GUI API and package/version are verified.
- **Human intent:** use cases and explanatory narrative remain authored and source-grounded. AST/reflection alone cannot infer a user's goal.
- **Runtime behavior:** a source diagram is not a runtime trace. Sequence and lifecycle diagrams must be labeled according to whether they are source-shaped, authored, test-backed, or captured from an identified run.

## Required class-view contract

For each maintained in-scope production declaration (including class, interface, struct, record, and enum; include nested/internal types unless explicitly excluded with a reason):

1. Generate or maintain one dedicated type page and one class-diagram SVG for that declaration.
2. Identify the fully qualified type and defining source location. Multiple declarations in one file require explicit target selection; never silently document only the first type.
3. Show the focal type as the anchor. Place verified consumers above, the focal declaration in the center, direct dependencies and result types below, and verified project/package/framework references to the right.
4. Show members and signatures with type, parameters, return value, and meaningful modifiers. Preserve recursive ownership in nested data cards; keep implementation detail subordinate to the contract.
5. Resolve inheritance, interface realization, and meaningful member/type dependencies from compiler symbols when available. Label unresolved syntax references honestly; do not present text-only base-list names as verified relationships.
6. Use explicit, labeled relationship kinds. Do not infer composition or ownership merely because one type calls another.
7. Include accessible SVG title/description, legible contrast, text fallback, stable identities, source links, and provenance.
8. Keep the type page linked to relevant source, tests, neighborhood map, and behavior/use-case views where applicable.

## Additional UML families

Do not generate every UML family for every class. Generate each family where its subject and evidence exist:

- **Class neighborhoods:** consumers, focal declaration, collaborators/results, and package edges.
- **Sequence diagrams:** calls, returns, alternatives, loops, exceptions, and handoffs for a named scenario.
- **Activity/control-flow diagrams:** algorithm paths, branches, loops, and exceptional exits.
- **State/lifecycle diagrams:** defined states, transitions, guards, and lifecycle ownership.
- **Use cases:** one file per scenario, grouped by User Goals, API/Contract, Data/Serialization, Operational, Integration, Performance, Security/Abuse, and Maintenance/Evolution where applicable.
- **Architecture/component and deployment/topology:** responsibility boundaries, dependencies, hosts, processes, storage, network, and trust boundaries.
- **Data models/lineage:** data shapes, ownership, transformations, serialization and provenance.
- **MicroBundles/manifests:** every locally defined MicroBundle, identity/version, dependencies, configuration, failure behavior and actual composition path.
- **Traceability:** connect important requirements/contracts to source, tests, diagrams and decisions.
- **Complexity/performance:** keep static Big-O and space analysis separate from measured benchmark results and captured runtime evidence.

Mark genuinely inapplicable views with a reason; do not create empty artifacts merely to claim coverage.

## Verification required before closing the gate

- [ ] A deterministic inventory matches actual production declarations and identifies exclusions with reasons.
- [ ] Every maintained in-scope declaration has its own page and SVG; missing or duplicate outputs fail validation.
- [ ] A source fixture with multiple declarations proves explicit target selection.
- [ ] Compiler-backed fixtures prove inheritance/interface/member relationships and the renderer draws correctly typed, labeled connectors.
- [ ] A source mutation changes the normalized model and generated SVG predictably; stale committed output fails CI.
- [ ] Malformed source and unresolved symbols fail safely or produce explicit diagnostics without inventing relationships.
- [ ] SVG structure, accessible title/description, labels, bounds and text legibility are checked; a rendered artifact receives visual inspection.
- [ ] Required checks pass on the exact candidate SHA. Record GUI SHA, FSM_COS SHA, workflow run URLs and produced artifact.
- [ ] The exact GUI consumer API/package version is supported and verified; FSM_COS does not depend on an unmerged staging branch.
- [ ] The FSM_COS UML index and artifact catalog match actual coverage. No repository-wide generation or stale-check claim is made until CI proves it.

A passing SVG generation workflow proves only the checks it actually runs. It does not by itself prove semantic relationship correctness, per-type completeness, visual quality, integration, or release readiness.

## Current repository statement

The FSM_COS UML index currently describes a limited set of consumer-first SVG diagrams and manually maintained type companions, and explicitly notes that most declarations do not yet have individual SVG class diagrams. The artifact catalog records the expected coverage and evidence states. This coordination file is a consumer acceptance contract, not proof that the generator, complete inventory, or stale-output gate already exists.

No merge, publication, master-branch change, branch deletion, or weakening of required checks is authorized by this document.

