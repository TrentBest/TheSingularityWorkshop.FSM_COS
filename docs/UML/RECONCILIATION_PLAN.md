# UML Standard Reconciliation Plan

**Repository:** TheSingularityWorkshop.FSM_COS  
**Working branch:** `development`  
**Purpose:** Lead the first full application of the Workshop's expanded UML and living-documentation standard.

This plan is an implementation ledger, not a claim that the work is already complete. Source and test code remain authoritative. Existing diagrams and companion pages are evidence to inspect and improve, not automatically accepted as compliant.

## Decision: reconcile in place, preserve useful work

Do not delete and recreate the UML tree wholesale. The current tree has companion Markdown pages for the 20 core C# source files, a limited set of SVG diagrams, focused view specifications, and a manual coverage inventory. Reuse verified content and migrate artifacts deliberately. Replace or retire an individual artifact only when its replacement is validated and its inbound links have been reconciled.

The canonical target families are defined in [the Artifact Catalog](Standard/ARTIFACT_CATALOG.md). Existing `ClassDiagram/`, `src/`, and `tests/` locations remain valid during reconciliation; avoid a broad move that creates link churn before the target inventory is real.

## Baseline observed

- The core project tree contains 20 C# source files.
- The current UML inventory reports companion pages for all 20 source files and UML-neighborhood companions for the declarations represented by those files.
- Dedicated SVGs exist for only a small subset of the source surface; presence alone does not prove that a diagram satisfies the new layout, semantic-relationship, provenance, accessibility, or visual-review requirements.
- Sequence, activity/control-flow, state/lifecycle, use-case, architecture, data-lineage, and traceability coverage is described as partial in the current inventory.
- The repository does not currently claim a repository-wide source-derived generator or stale-output CI gate.
- No build or CI result is implied by this manual tree inspection.

Recheck this baseline whenever the source revision changes. Do not treat the counts above as compiler-derived declaration coverage: one source file can contain multiple or nested declarations.

## Work sequence

### Phase 1 — Reconcile the inventory

- [ ] Enumerate every in-scope declaration, including nested and multiple declarations per file, from compiler-backed source analysis.
- [ ] Give each declaration a stable identity and source location.
- [ ] Match each declaration to its source companion, AST/class diagram, consumer-first neighborhood, relevant use cases, and tests.
- [ ] Record missing, stale, duplicate, mis-targeted, and unverifiable artifacts explicitly.
- [ ] Separate repository evidence from planned architecture and authored explanation.

**Exit evidence:** a reproducible declaration-level coverage report with its exact source revision and scan scope.

### Phase 2 — Prove the core composition path

Create or reconcile views in this priority order:

1. Composition entry point and dependency closure.
2. Manifest validation, dependency graph, and load schedule.
3. Sequential load and arbitration, including bounded rounds and failure paths.
4. Composition result and runtime assembly handoff.
5. Runtime experience registration and explicit selection.
6. Rollback contract and limits on external side-effect recovery.

For each workflow, link a consumer-facing use case to its contract, source, tests, and diagrams. Keep authored use-case intent distinct from source-derived structure and runtime-captured behavior.

### Phase 3 — Align artifacts to the expanded standard

- [ ] One focused AST/class diagram per supported declaration, explicitly targeting each declaration in multi-type files.
- [ ] Consumer-first neighborhood per in-scope declaration: actual consumers above, focal declaration centered, collaborators/results below, package/project/framework references to the right.
- [ ] Expandable, keyboard-accessible data cards where the rendering surface supports interaction; label noninteractive renderings honestly.
- [ ] Source-grounded sequence and control-flow views for critical paths, including branches, loops, early exits, and failures.
- [ ] Lifecycle/state diagrams only where contracts or tests establish meaningful states and transitions.
- [ ] Dedicated, categorized use-case files linked to source and tests.
- [ ] Manifest/MicroBundle inventory based on actual locally defined implementations, not inferred from interface names or test doubles.
- [ ] Data lineage and requirement-to-test traceability for critical contracts.

### Phase 4 — Establish source-derived generation with GUI.SVG

- [ ] Define a renderer-neutral model with declaration identity, source spans, relationship kinds, labels, and provenance.
- [ ] Resolve type relationships through compiler symbols; do not render unresolved names as verified connections.
- [ ] Keep source analysis in the build-time tool/adapter, not the GUI renderer or FSM_COS runtime kernel.
- [ ] Support explicit target selection and complete project-wide inventory, including nested/multiple declarations.
- [ ] Add fixtures for source mutation, missing declarations, malformed inputs, unresolved symbols, and stale output.
- [ ] Inspect rendered SVG for readability, contrast, labels, connector direction, and accessible semantics.
- [ ] Record generator/tool version, source revision, analysis scope, unresolved relationships, and regeneration command.
- [ ] Add deterministic coverage/freshness CI only after the tool and its failure cases are verified on the exact candidate SHA.

### Phase 5 — Close coverage and release evidence

- [ ] Validate relative links and reconcile navigation after any path migration.
- [ ] Report independent metrics for declarations, AST diagrams, neighborhoods, use cases, relationship resolution, tests/traceability, and freshness enforcement.
- [ ] Keep manual, source-derived, runtime-captured, planned, required, and not-applicable states distinct.
- [ ] Update the UML index and release-readiness evidence from the same exact source revision.
- [ ] Do not call the UML gate closed based on document count or historical CI results alone.

## Reporting rules

Every status update must state the assessed branch/commit where known, what was actually inspected or tested, the remaining critical gaps, and a grounded progress percentage. If coverage has not been measured, say **unverified** rather than inventing a percentage. Do not merge to `master`, publish packages, delete branches, or weaken required checks as part of this documentation effort.
