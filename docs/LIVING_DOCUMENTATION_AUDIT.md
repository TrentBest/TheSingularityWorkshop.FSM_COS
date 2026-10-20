# Living Documentation Visual Audit

**Purpose:** make FSM_COS documentation easier to understand through accurate diagrams, animated SVGs, interactive examples, and other living visuals where they materially teach the subject.

**Current status:** the Markdown inventory and inline local-link pass are complete, and explicit visual decisions are now recorded for all 36 tracked Markdown documents. Source-to-diagram accuracy and hosted/accessibility verification remain open. An SVG existing in `docs/assets` does not mean it is embedded in the relevant document, and an embedded picture does not automatically qualify as a useful living visual.

## Standard

A document should use a visual when it materially clarifies at least one of these:

- **Structure:** ownership, layers, boundaries, dependencies, or ecosystem relationships.
- **Sequence:** requests, resolution, validation, loading, arbitration, and handoff.
- **State or change:** convergence, failure paths, transitions, or lifecycle.
- **Choice:** package-versus-MicroBundle decisions, host responsibilities, or alternative paths.
- **Proof:** a source-linked example, test result, executable sample, or interactive GUI that demonstrates actual behavior.

Prefer, in order where appropriate: an accurate living GUI or working example; an animated SVG that communicates semantic motion; a clear static SVG; a compact text diagram or table. Keep text diagrams when they are the clearest representation; do not add images as decoration or duplicate the same explanation repeatedly.

Every visual must:
- match the current source contract and distinguish implemented behavior from proposed design;
- include useful alt text and, where needed, a caption stating what it teaches or proves;
- remain legible on GitHub in light/dark themes and at narrow widths;
- respect reduced-motion/accessibility expectations where animation is used;
- have a canonical source asset and a clear place to update it when the contract changes;
- avoid requiring JavaScript or remote runtime services just to display ordinary documentation.

## Measurable baseline and UML coverage

Inventory snapshot from the integrated `work/alpha7-living-docs-integration` candidate tree; counts must be refreshed when files are added or removed:

| Metric | Count | Interpretation |
|---|---:|---|
| Tracked Markdown documents | 36 | Every document needs an explicit visual decision, not necessarily an image |
| SVG assets outside the divider/marker directories | 13 | Candidate semantic diagrams/illustrations; asset existence alone does not prove document coverage |
| Section-divider SVGs | 13 | Navigation/branding assets, not technical subject diagrams |
| Section-marker SVGs | 4 | Navigation/branding assets, not technical subject diagrams |
| C# source files in `src/FSM_COS` | 16 | Current implementation surface to map against the diagrams |
| C# test files in `tests/FSM_COS.Tests` | 5 | Behavioral evidence for diagram claims |
| Mermaid UML-bearing guides | 6 | Five contract guides contain simplified class diagrams; `docs/EXECUTION_SEQUENCE.md` adds the source-derived `FsmCos.Execute` sequence. These are reviewable Markdown diagrams, not GUI.SVG-generated UML. |

These are **inventory counts, not a coverage score**. The next audit pass must measure per-document use separately: (1) visual embedded directly, (2) visual shared from another document, (3) source-generated visual, or (4) deliberately text-only with rationale. A shared overview should not be counted as equivalent to a subject-specific diagram when the document teaches a more complex contract.

### UML coverage and next priorities

Use actual public/internal type definitions and tests as the source of truth; show visibility and relationships honestly. Do not imply that a conceptual box is an implemented class.

- **Composition and manifest contracts — present in draft:** Draft PR #34 integrates simplified, source-derived Mermaid class diagrams across `docs/ARCHITECTURE.md`, `docs/RUNTIME_MANIFEST.md`, `docs/RUNTIME_ASSEMBLY.md`, `docs/ARBITRATION.md`, and `docs/STAGED_MANIFEST_LOADING.md`. They are manually authored Markdown diagrams, not GUI.SVG-generated UML; abbreviated signatures are not substitutes for source/XML documentation.
- **Execute sequence — present in draft:** `docs/EXECUTION_SEQUENCE.md` traces caller/host → `FsmCos.Execute` → catalog and configuration boundaries → complete dependency-graph preflight → optional schedule validation → depth-first dependency loading → bounded arbitration → `RuntimeAssembly` return or failure. It was checked against the current method ordering; hosted Mermaid rendering remains unverified.
- **Next: staged metadata versus actual loading:** add a focused state/sequence view only if it clarifies that load-plan/schedule metadata is validated but does not implement repository localization or automatic deferred loading.
- **Next: arbitration convergence:** a sequence/state view may clarify change aggregation, the zero-based round index, the configured call limit, and non-convergence. Reuse `arbitration-convergence.svg` if it already teaches the contract; do not create a duplicate merely to increase diagram count.

The next audit step is not to add visuals indiscriminately: verify each existing class diagram against its type declarations, and check whether the current staged-loading and arbitration visuals already explain their contracts sufficiently. Generated type boxes alone cannot prove control flow or runtime behavior.

## Existing visual assets discovered

The repository already contains a useful SVG library, including:

- `docs/assets/fsm-cos-overview.svg`
- `docs/assets/fsm-cos-system.svg`
- `docs/assets/fsm-cos-domain-map.svg`
- `docs/assets/fsm-cos-crane.svg`
- `docs/assets/composition-boundary.svg`
- `docs/assets/runtime-boundary.svg`
- `docs/assets/runtime-manifest-pipeline.svg`
- `docs/assets/fsm-cos-preflight.svg` (generated from `samples/DocumentationVisuals/Program.cs` with GUI.SVG)
- `docs/assets/dependency-resolution.svg`
- `docs/assets/ai-composition.svg`
- `docs/assets/arbitration-convergence.svg`
- `docs/assets/runtime-assembly-handoff.svg`
- `docs/assets/microbundle-cartridge.svg`

There are also section divider/marker assets. These are a starting library, not proof that every useful document already has its own appropriate visual.

## GUI.SVG-generated source and package boundary

The canonical scene source for `docs/assets/fsm-cos-preflight.svg` is `samples/DocumentationVisuals/Program.cs`. On `development`, the package workflow checks out a pinned GUI source revision, regenerates the SVG, compares it with the committed asset, and then packages the static file.

This is a **build-time documentation-tooling dependency only**. `FSM_COS.csproj` does not reference GUI.SVG, and the generated SVG does not introduce a GUI package into the FSM_COS NuGet dependency graph. The NuGet package contains the SVG file itself. Update the pinned GUI revision and committed asset together when the generator implementation changes.

The generated image is a contract map, not a live runtime trace. It must continue to distinguish complete dependency-closure preflight from runtime load or arbitration failures, and must not imply that staged metadata triggers deferred execution or that failed composition rolls back arbitrary bundle side effects.

### GUI.SVG source-derived UML: current capability boundary

The current GUI source-UML generator is a first vertical slice, not yet an authoritative semantic documentation pipeline. It currently emits only the first top-level type per invocation, and base-list references are textual rather than compiler-resolved relationship connectors. Therefore, the five Mermaid class diagrams in this candidate remain manually authored and reviewable; they must not be described as GUI-generated UML or as exhaustive type coverage.

Before FSM_COS can adopt generated UML as authoritative documentation, the consumer contract still needs a versioned, stable semantic model for type identity and members, resolved relationship kinds and endpoints, supported multiplicity/navigability, multi-type and cross-project coverage, deterministic regeneration, and a visible inventory of missing/unsupported types. Structural facts should be generated from source analysis; use cases, actor goals, and conceptual intent remain explicit human-authored/reviewed inputs. The GUI generator's current limitations are a dependency coordination item, not something FSM_COS documentation can silently assume away.

## Coverage checklist

Use the status labels consistently:
- **Source reviewed; hosted checks pending:** the asset, caption, and linked contract have been compared at source level; this does not establish hosted rendering or assistive-technology behavior.


| Document | Visual intent | Initial action | Status |
|---|---|---|---|
| `GUILLM.md` | Cross-repository coordination and UML acceptance boundaries | Keep the explicit decision record and evidence contract as text; the current candidate state and limits are easier to audit in prose than a second diagram | No additional visual needed — this is a coordination record, not a runtime contract |
| `AGENTS.md` | Repository-specific operating guardrails | Plain-language constraints are the point; a diagram risks obscuring authoritative instructions | No visual needed — operational rules are best kept explicit |
| `SECURITY.md` | Trust boundary, adversarial inputs, and host-owned security responsibilities | The threat matrix and explicit “not a sandbox” warning are the primary safety explanation; consider a separate trust-boundary visual only if it adds clarity without implying isolation | No additional visual needed — the matrix and boundary prose are more precise for this security plan |
| `README.md` | One-screen composition overview | Re-audit found a direct RuntimeAssembly-to-Experience path that bypassed the Host; corrected on `work/alpha7-living-docs-integration`. Verify rendered arrow routing and compare the remaining overview claims before closing this row. | Source correction integrated in draft PR; hosted checks pending |
| `docs/GALLERY.md` | Discover existing visual exhibits and their deep dives | The document is itself the visual index and embeds the existing exhibit SVGs; avoid adding a second overview that duplicates its six exhibits | No additional visual needed — the gallery already is the visual navigation surface |
| `docs/WHAT_IS_FSM_COS.md` | Request → composition → host; newcomer mental model | Shares the overview SVG; its host-to-experience routing correction is integrated on `work/alpha7-living-docs-integration`; rendered arrow routing remains pending verification. | Source correction integrated in draft PR; hosted checks pending |
| `docs/LIVING_SVG.md` | Explain how the canonical scene source becomes the generated preflight SVG and what its motion means | Reuse the generated preflight SVG and the numbered source-backed sequence; keep the distinction between explanatory motion and live telemetry explicit | Existing generated visual is the subject; no duplicate pipeline diagram needed |
| `docs/EXECUTION_SEQUENCE.md` | Exact `FsmCos.Execute` ordering, failure boundaries, and host handoff | Mermaid sequence is checked against `FsmCos.Execute`; it distinguishes pre-load rejection from load side effects and arbitration non-convergence. Hosted rendering and accessibility checks remain open. | Source reviewed; hosted checks pending |
| `docs/ARCHITECTURE.md` | Kernel boundary, catalog, dependency and host relationships | Overview and dependency-resolution diagrams remain; the generated preflight visual shows identity/dependency validation, optional schedule checks, dependency-first loading, bounded arbitration, and separate failure semantics. Generator output is pinned and checked in CI. | Source reviewed; hosted checks pending |
| `docs/THEORY.md` | Why composition is distinct; invariants and non-goals | System SVG marks upstream authoring context as conceptual and identifies `RuntimeManifest` as the implemented input | Source reviewed; hosted checks pending |
| `docs/DOMAIN.md` | What FSM_COS owns versus adjacent domains | Domain map reviewed against kernel ownership, host responsibilities, and deferred-capability caveats | Source reviewed; hosted checks pending |
| `docs/DOMAIN_COMPLETION_ROADMAP.md` | Implemented behavior versus deferred capabilities and follow-on work | Existing domain map explains the composition boundary; the capability table distinguishes implemented behavior from deferred work without duplicating the release gate | No additional visual needed — the map plus status table is clearer than a second capability/status diagram |
| `src/FSM_COS/RuntimeManifest.md` | Source-level `RuntimeManifest` class card and field contract | Keep the interim Mermaid card labeled as manually authored; compare signatures and optional metadata semantics against `RuntimeManifest.cs` | Source-grounded interim card; GUI generation and hosted rendering remain open |
| `docs/RUNTIME_MANIFEST.md` | Caller request and manifest contents | Pipeline SVG is explicitly conceptual; the text scopes the implemented contract to `RuntimeManifest` and `FsmCos.Execute` | Source reviewed; hosted checks pending |
| `src/FSM_COS/RuntimeManifestUML.md` | Relationships among manifest, roots, optional plan/schedule, context, and intent | Keep data-reference semantics distinct from execution/loading order; use cases remain maintainer-authored and must not be inferred from reflection | Source-grounded interim Mermaid; verify signatures/relationship semantics and hosted rendering before closing |
| `docs/MANIFEST_THEORY.md` | Semantic request versus serialized representation | Pipeline SVG and text distinguish rich authoring context from the compact runtime request and separate configuration source | Source reviewed; hosted checks pending |
| `docs/STAGED_MANIFEST_LOADING.md` | Preflight, planning, validation, loading, failure boundaries | Dependency-resolution SVG illustrates dependency-first order, links to `FsmCos.LoadBundle` and tests, and explicitly disclaims a live trace or implemented host localization; reduced-motion fallback is present | Source reviewed; hosted checks pending |
| `docs/CONSUMING_MICROBUNDLES.md` | Consumer flow, catalog responsibility, dependency ordering | Dependency graph and alt text distinguish an illustrative graph from the actual dependency-before-dependent loading contract | Source reviewed; hosted checks pending |
| `docs/ARBITRATION.md` | Iterations, convergence, limit, non-convergence | Diagram and caption distinguish convergence from failure at the configured round limit and explain the zero-based convergence index | Source reviewed; hosted checks pending |
| `docs/RUNTIME_ASSEMBLY.md` | Result shape and handoff | Handoff SVG and caption match `RuntimeAssembly` and keep host execution outside the composition boundary | Source reviewed; hosted checks pending |
| `docs/RUNTIME_BOUNDARY.md` | Explicitly excluded responsibilities | Boundary SVGs and text distinguish kernel-owned composition from external catalog/configuration contracts and host-owned execution | Source reviewed; hosted checks pending |
| `docs/AI_COMPOSITION.md` | AI-oriented composition contracts and current limits | AI-composition SVG is explicitly conceptual and not chronological; text distinguishes proposed layers from implemented kernel behavior | Source reviewed; hosted checks pending |
| `docs/ECOSYSTEM_INTEGRATION_MAP.md` | Package roles and dependency direction | System SVG is a conceptual target; its dated package inventory is not current dependency or release evidence | Source reviewed; hosted checks pending |
| `docs/WEBPAGE_INTEGRATION.md` | Kernel/host separation and WebPage handoff | Overview SVG illustrates the kernel/host boundary; guide now matches the alpha.7 source declaration and disclaims proof of a fully `RuntimeAssembly`-driven experience | Source reviewed; hosted checks pending |
| `NuGet.README.md` | Package-first overview for NuGet consumers | Candidate package includes the overview SVG and current README; the moniker is remote, so hosted asset rendering remains a separate check | Source reviewed; hosted checks pending |
| `docs/DEVELOPMENT.md` | Build/test/release verification path | Mermaid flow compared with the three verification jobs and manual `workflow_dispatch` publish condition in `.github/workflows/package.yml` | Source reviewed; hosted checks pending |
| `docs/0.1.0-alpha.7-READINESS.md` | Release gates, evidence, exact candidate/package identity | Checklist is intentionally text-based; another flow would duplicate the Development diagram and obscure exact-candidate evidence | No visual needed — exact gates and evidence are clearest as a checklist |
| `docs/releases/0.1.0-alpha.7.md` | Scope and behavioral change | Mermaid view matches the read-only collection-structure contract and preserves the mutable-element caveat | Source reviewed; hosted checks pending |
| `docs/releases/0.1.0-alpha.6.md` | Historical release scope and contract | Historical release record; current contract is documented in alpha.7 notes | No visual needed — historical release notes do not need a duplicate diagram |
| `DOCUMENTATION_INDEX.md` | Navigation by reader goal | Text links and grouped navigation are appropriate; no diagram currently needed | No visual needed — navigation is the purpose |
| `DOCUMENTATION_STANDARD.md` | Shared rules for visual, living documentation | Text-first normative standard; examples and requirements are clearer than a decorative summary image | No visual needed — rules and checklists are the authoritative form |
| `docs/BRANCH_RECONCILIATION.md` | Historical branch inventory and disposition decisions | Archive note points to current authorities; the dated inventory remains in Git history | No visual needed — archived decision record is navigation only |
| `docs/DEPENDENCY_ALIGNMENT_CHECKLIST.md` | Historical cross-repository dependency pins and migration sequence | Archive note points to current FSM_COS authorities; the dated matrix remains in Git history | No visual needed — archived pin inventory is not current evidence |
| `docs/CONTINUATION_BRIEF_ALPHA6_FORGE_ANYAPP_WEBPAGE.md` | Historical handoff and integration constraints | Archived alpha.6 notes now direct readers to current release and architecture authorities | No visual needed — archived handoff is a navigation note, not a current specification |
| `docs/PAUSE_POINT.md` | Historical engineering pause decision | Archived pause note now directs readers to the maintained alpha.7 readiness gate and domain roadmap | No visual needed — archive notice and authority links are sufficient |
| `docs/LIVING_DOCUMENTATION_AUDIT.md` | Coverage, validation, and completion criteria | This document is the coverage record; a separate picture would duplicate the status table | No visual needed — checklist is the source of truth |

## Current verification boundary

The following findings are verified at the source-markup level:

- The previous markup audit recorded 12 substantive SVG diagrams with embedded `<title>`, `<desc>`, `role="img"`, and `aria-labelledby`. The repository tree now shows 13 SVG assets outside the divider/marker directories; reconcile the classification and verify the thirteenth asset before treating the count as closed.
- The 13 section-divider SVGs are decorative and use `role="presentation"` with `aria-hidden="true"`.
- The four section-marker SVGs expose concise accessible labels.
- `dependency-resolution.svg` uses reduced-motion-aware animation to illustrate an example dependency-first order. It is not a live execution trace.
- The earlier inline-link pass covered 29 tracked Markdown documents. This integration adds `docs/LIVING_SVG.md` and `docs/EXECUTION_SEQUENCE.md`; validate the complete 31-document candidate and package-relative links in CI before closing the link-audit gate.

These findings do **not** establish complete semantic accuracy for every visual, hosted SVG rendering, browser accessibility-tree exposure, keyboard behavior, screen-reader output, or external-link reachability. Those require their own evidence and must not be inferred from source markup or a green build.

## Findings from the current source-to-diagram re-audit

- The overview SVG previously drew a second path from `RuntimeAssembly` directly to `Experience`, bypassing the Host. This contradicted the documented handoff boundary. The integrated draft branch `work/alpha7-living-docs-integration` removes that bypass and routes the arrow from Host to Experience. This is not yet a merged fix; hosted rendering and the rest of the diagram audit remain open.
- This finding shows that earlier source-level review status was too broad. The checklist below now leaves the complete source-to-diagram audit open until each substantive diagram has been checked against the current implementation and the diagram's explanatory text.

## Remaining work

- [ ] Complete a fresh source-to-diagram audit against linked implementation/tests; an audit pass found an overview arrow that bypassed the Host and required correction. Continue checking every substantive diagram before marking this complete.
- [ ] Check hosted rendering in GitHub and relevant light/dark and narrow-width views.
- [ ] Verify accessibility-tree exposure and screen-reader behavior for substantive and animated visuals.
- [ ] Check reference-style Markdown link definitions and external-link reachability.
- [x] Reconcile status labels where reviewed: source-reviewed visuals explicitly retain hosted checks as pending, while text-only and historical documents record why no visual is needed.
- [x] No additional interactive example is justified at this stage: the minimal consumer and regression tests demonstrate actual behavior more directly than a second illustrative GUI.

## Definition of done

This audit is complete only when every documentation file has been considered, each visual that materially helps is present and source-aligned, every local asset reference is valid, remaining text-only documents have a recorded reason, and the open verification boundaries above are closed or explicitly accepted with a documented rationale. Visual quantity is not a completion criterion.

## Asset classification and behavior-to-evidence mapping

The inventory above deliberately separates **semantic diagrams** from **conceptual illustrations**. In particular, `docs/assets/fsm-cos-crane.svg` is a conceptual metaphor, not a source-accurate flow diagram: its title and description explicitly frame the crane as a visual metaphor. Count it as a conceptual illustration, not as evidence that a sequence or class contract is diagrammed. Keep the inventory's raw asset count at 13, but report semantic-diagram coverage separately after reviewing each asset's claims and its use in the relevant document.

| Behavior / contract | Primary evidence inspected | Visual implication / remaining check |
|---|---|---|
| Composition inputs, dependency resolution, and host handoff | `FsmCosTests.cs`, `FsmCos.cs`, `RuntimeAssembly.cs`, `fsm-cos-overview.svg`, `runtime-assembly-handoff.svg` | Overview correction is integrated in draft PR #34; the class diagram should show injected catalog/configuration boundaries and return of `RuntimeAssembly` to the host, not imply host execution is inside FSM_COS. |
| Dependency-first ordering, cycles, and complete preflight before loading | `FsmCosTests.cs`, `dependency-resolution.svg`, `fsm-cos-preflight.svg` generated from `samples/DocumentationVisuals/Program.cs` | Existing visuals cover the high-level flow; verify edge labels, failure branches, and source-generated asset parity. The diagram is not a live runtime trace. |
| Arbitration convergence, bounded rounds, and failure | `FsmCosTests.cs`, `arbitration-convergence.svg` | Sequence/state view should distinguish a zero-based convergence index from call count, stable round from failed non-convergence, and exceptions from ordinary change aggregation. |
| Staged manifest, schedule, promotion, and load-plan transitions | `RuntimeManifestPlanningTests.cs`, `SnapshotIsolationTests.cs`, `STAGED_MANIFEST_LOADING.md` | Dedicated state/sequence visual is justified because metadata validation, readiness, localization, and actual loading are distinct. The plan itself does not localize or load bundles. |
| Snapshot isolation and collection immutability | `SnapshotIsolationTests.cs`, `FsmCosTests.cs` | Prefer concise source-linked proof/tests and a compact ownership note over a standalone decorative diagram. Keep the distinction between read-only collection structure and mutable domain-owned bundle instances. |
| Public GUI capability handoff | `AiGuiCapabilityHandoffTests.cs` | Keep the clipboard/GUI capability contract tied to its tests; add a diagram only if the guide explains a multi-step handoff that prose and tests do not make clear. |

### Coverage claims and limits

- The current test source has dedicated coverage for pre-load rejection, dependency ordering and graph failures, configuration bounds/copying, catalog/configuration/load/arbitration failures, convergence/non-convergence, staged-plan and schedule consistency, promotion/readiness transitions, and collection snapshot behavior. These are source-level observations, not a substitute for the exact candidate's CI results.
- The count of methods marked `[Fact]` or `[Theory]` is not the same as the number of executed test cases: theories can expand into multiple cases, and a method count does not establish a passing run. Use the workflow for the exact candidate as the pass/fail authority.
- The Mermaid class diagrams integrated in draft PR #34 are simplified, source-derived overviews, manually authored in Markdown; they are not generated by GUI.SVG. The `docs/EXECUTION_SEQUENCE.md` sequence is now integrated in draft PR #34 and grounded in `FsmCos.Execute` and the corresponding tests. Hosted rendering, accessibility-tree exposure, keyboard navigation, and screen-reader behavior remain unverified.



## Follow-up source-to-diagram pass — 2026-10-20

The following source comparisons were performed against the integrated alpha.7 candidate, including `FsmCos.Execute`, `RuntimeManifest`, `RuntimeAssembly`, `ArbitrationContext`, the five contract guides, and the canonical GUI.SVG scene source.

- **Execute ordering:** the sequence's major boundaries match the method: bounded root/load-plan/schedule inputs and manifest snapshots; staged-plan/root validation; exact root resolution; complete dependency graph resolution and cycle validation; optional schedule-to-graph comparison; depth-first configuration and dependency-first `Load`; bounded arbitration; and assembly return only after a no-change round. The diagram is intentionally not a runtime trace.
- **Configuration semantics:** configuration is optional. A configuration source may override a dependency request's configuration; the request configuration remains the fallback. Configuration and load exceptions propagate, and prior load side effects are not rolled back.
- **Assembly/handoff:** `RuntimeAssembly` exposes a read-only snapshot of bundle references and the zero-based converged arbitration round. The host remains responsible for execution and experience manifestation.
- **Manifest/schedule semantics:** a manifest requests root IDs and versions. Published references add identity metadata; schedule edges are compared against the resolved graph. Neither a content hash field nor a stage label by itself fetches, verifies, localizes, or defers artifacts.
- **Arbitration context:** `ArbitrationContext` implements the external MicroBundleDomain arbitration-context contract. Its `ExperienceContext` is nullable in the implementation; the class diagram deliberately abbreviates nullability and points readers back to source/XML documentation.

### Visual corrections applied; regeneration verified on the prior candidate

- [x] **Overview SVG:** removed the two disconnected amber strokes that looked like an unexplained secondary path. The primary RuntimeAssembly → Host → Experience handoff remains unchanged.
- [x] **GUI.SVG preflight scene:** added static red branches from identity validation to preflight rejection and from arbitration to runtime failure. The runtime-failure panel now explicitly includes configuration-provider failures. The canonical source is `samples/DocumentationVisuals/Program.cs`; the committed SVG was aligned with the renderer's observed attribute ordering after the first parity check caught a mismatch.
- [x] **Generator parity (candidate SHA `54627edda04dce10ff7baac227a73711ada186ce`):** Pack and publish workflow [#757](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/actions/runs/39369457060) passed, including `Generate FSM_COS validation visual with GUI.SVG` and `Verify committed GUI.SVG visual is current`. The same run passed source-integration, development-source build/tests, security analyzers, coverage, NuGet package-content/metadata verification, and public-feed compatibility build/tests/package verification. CodeQL [#61](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/actions/runs/39369457074) also passed. The audit-only commit recording this evidence creates a new candidate SHA, so its own CI must pass before this gate is considered closed for the updated PR head.
- [ ] **Hosted verification:** these source comparisons do not close GitHub-rendered SVG/Mermaid, light/dark, narrow viewport, accessibility-tree, keyboard, screen-reader, or external-link checks.

This pass narrows the source review and records successful generator parity and CI for the candidate SHA above; it does **not** close the full source-to-diagram audit or hosted/accessibility gates. Keep the draft PR unmerged until the updated PR head passes CI and the remaining review gates are closed or explicitly accepted.


### Additional execution-sequence correction

The source review caught a meaningful failure-boundary distinction: dependency-request configuration payloads are bounded while resolving the complete graph, but configuration-provider overrides are fetched and byte-bounded as each bundle is entered by `LoadRoot`. A provider override can therefore exceed the remaining configuration budget after a prior dependency has already loaded. The execution sequence now records this separately from pre-load graph/resource rejection and preserves the no-transactional-rollback caveat. The GUI.SVG preflight's runtime-failure panel is updated to name provider-configuration failures too.
