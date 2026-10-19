# Living Documentation Visual Audit

**Purpose:** make FSM_COS documentation easier to understand through accurate diagrams, animated SVGs, interactive examples, and other living visuals where they materially teach the subject.

**Current status:** the Markdown inventory and inline local-link pass are complete; source-to-diagram accuracy and hosted/accessibility verification remain open. An SVG existing in `docs/assets` does not mean it is embedded in the relevant document, and an embedded picture does not automatically qualify as a useful living visual.

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

Inventory snapshot from the repository tree on `development`:

| Metric | Count | Interpretation |
|---|---:|---|
| Tracked Markdown documents | 30 | Every document needs an explicit visual decision, not necessarily an image |
| SVG assets outside the divider/marker directories | 13 | Candidate semantic diagrams/illustrations; asset existence alone does not prove document coverage |
| Section-divider SVGs | 13 | Navigation/branding assets, not technical subject diagrams |
| Section-marker SVGs | 4 | Navigation/branding assets, not technical subject diagrams |
| C# source files in `src/FSM_COS` | 16 | Current implementation surface to map against the diagrams |
| C# test files in `tests/FSM_COS.Tests` | 5 | Behavioral evidence for diagram claims |
| Dedicated UML class/sequence diagram assets | 0 identified | No committed UML-specific diagram format or class/sequence visual was found in the repository tree |

These are **inventory counts, not a coverage score**. The next audit pass must measure per-document use separately: (1) visual embedded directly, (2) visual shared from another document, (3) source-generated visual, or (4) deliberately text-only with rationale. A shared overview should not be counted as equivalent to a subject-specific diagram when the document teaches a more complex contract.

### UML candidates to prioritize

Use actual public/internal type definitions and tests as the source of truth; show visibility and relationships honestly. Do not imply that a conceptual box is an implemented class.

1. **Composition contract class diagram** — `FsmCos`, `IFsmCos`, `IMicroBundleCatalog`, `IMicroBundleConfigurationSource`, `RuntimeManifest`, `RuntimeAssembly`, `ArbitrationContext`, and the domain-owned `IMicroBundle` boundary. This should make constructor-injected dependencies and the host handoff explicit.
2. **Manifest and dependency-plan class diagram** — `RuntimeManifest`, `MicroBundleManifestEntry`, `MicroBundleReference`, `RuntimeManifestEntry`, `RuntimeManifestDependency`, `RuntimeManifestSchedule`, `RuntimeManifestLoadPlan`, and `ManifestLoadStage`. Distinguish value/record types from stateful plan objects and show ownership/cardinality only where the code supports it.
3. **Execute sequence diagram** — caller/host → `IFsmCos.Execute` / `FsmCos.Execute` → catalog/configuration boundaries → preflight and dependency-first load → bounded arbitration → `RuntimeAssembly` or propagated failure. This is the best candidate for an animated highlight path.
4. **Staged metadata versus actual loading sequence** — show validation/state transitions separately from repository localization and bundle loading. This is a high-risk teaching point because the code explicitly does not implement a repository or host localization pipeline.
5. **Arbitration sequence/state view** — per-round `Arbitrate` calls, change aggregation, convergence, configured limit, and failure. Keep the zero-based convergence index distinct from the number of calls.

Until the GUI.SVG class-definition-to-diagram capability is available, treat these as **diagram requirements**, not claims that generated UML or animation already exists. Once available, prefer generation from code definitions plus a small, reviewable semantic overlay; generated type boxes alone cannot prove control flow or runtime behavior.

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

## Coverage checklist

Use the status labels consistently:
- **Source reviewed; hosted checks pending:** the asset, caption, and linked contract have been compared at source level; this does not establish hosted rendering or assistive-technology behavior.


| Document | Visual intent | Initial action | Status |
|---|---|---|---|
| `AGENTS.md` | Repository-specific operating guardrails | Plain-language constraints are the point; a diagram risks obscuring authoritative instructions | No visual needed — operational rules are best kept explicit |
| `README.md` | One-screen composition overview | Re-audit found a direct RuntimeAssembly-to-Experience path that bypassed the Host; corrected on `docs/fsm-cos-visual-contract-audit`. Verify rendered arrow routing and compare the remaining overview claims before closing this row. | Source correction in review branch; hosted checks pending |
| `docs/WHAT_IS_FSM_COS.md` | Request → composition → host; newcomer mental model | Shares the overview SVG; its host-to-experience routing correction is in `docs/fsm-cos-visual-contract-audit` and remains pending review/render verification. | Source correction in review branch; hosted checks pending |
| `docs/ARCHITECTURE.md` | Kernel boundary, catalog, dependency and host relationships | Overview and dependency-resolution diagrams remain; the generated preflight visual shows identity/dependency validation, optional schedule checks, dependency-first loading, bounded arbitration, and separate failure semantics. Generator output is pinned and checked in CI. | Source reviewed; hosted checks pending |
| `docs/THEORY.md` | Why composition is distinct; invariants and non-goals | System SVG marks upstream authoring context as conceptual and identifies `RuntimeManifest` as the implemented input | Source reviewed; hosted checks pending |
| `docs/DOMAIN.md` | What FSM_COS owns versus adjacent domains | Domain map reviewed against kernel ownership, host responsibilities, and deferred-capability caveats | Source reviewed; hosted checks pending |
| `docs/DOMAIN_COMPLETION_ROADMAP.md` | Implemented behavior versus deferred capabilities and follow-on work | Existing domain map explains the composition boundary; the capability table distinguishes implemented behavior from deferred work without duplicating the release gate | No additional visual needed — the map plus status table is clearer than a second capability/status diagram |
| `docs/RUNTIME_MANIFEST.md` | Caller request and manifest contents | Pipeline SVG is explicitly conceptual; the text scopes the implemented contract to `RuntimeManifest` and `FsmCos.Execute` | Source reviewed; hosted checks pending |
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
- The inventory covered all 29 tracked Markdown documents and found no unresolved inline relative Markdown or image/asset targets after the release-runbook link was corrected.

These findings do **not** establish complete semantic accuracy for every visual, hosted SVG rendering, browser accessibility-tree exposure, keyboard behavior, screen-reader output, or external-link reachability. Those require their own evidence and must not be inferred from source markup or a green build.

## Findings from the current source-to-diagram re-audit

- The overview SVG previously drew a second path from `RuntimeAssembly` directly to `Experience`, bypassing the Host. This contradicted the documented handoff boundary. The branch `docs/fsm-cos-visual-contract-audit` removes that bypass and routes the arrow from Host to Experience. This is a review-branch correction, not yet a merged fix; hosted rendering and the rest of the diagram audit remain open.
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
| Composition inputs, dependency resolution, and host handoff | `FsmCosTests.cs`, `FsmCos.cs`, `RuntimeAssembly.cs`, `fsm-cos-overview.svg`, `runtime-assembly-handoff.svg` | Overview correction is in PR #31; the class diagram should show injected catalog/configuration boundaries and return of `RuntimeAssembly` to the host, not imply host execution is inside FSM_COS. |
| Dependency-first ordering, cycles, and complete preflight before loading | `FsmCosTests.cs`, `dependency-resolution.svg`, `fsm-cos-preflight.svg` generated from `samples/DocumentationVisuals/Program.cs` | Existing visuals cover the high-level flow; verify edge labels, failure branches, and source-generated asset parity. The diagram is not a live runtime trace. |
| Arbitration convergence, bounded rounds, and failure | `FsmCosTests.cs`, `arbitration-convergence.svg` | Sequence/state view should distinguish a zero-based convergence index from call count, stable round from failed non-convergence, and exceptions from ordinary change aggregation. |
| Staged manifest, schedule, promotion, and load-plan transitions | `RuntimeManifestPlanningTests.cs`, `SnapshotIsolationTests.cs`, `STAGED_MANIFEST_LOADING.md` | Dedicated state/sequence visual is justified because metadata validation, readiness, localization, and actual loading are distinct. The plan itself does not localize or load bundles. |
| Snapshot isolation and collection immutability | `SnapshotIsolationTests.cs`, `FsmCosTests.cs` | Prefer concise source-linked proof/tests and a compact ownership note over a standalone decorative diagram. Keep the distinction between read-only collection structure and mutable domain-owned bundle instances. |
| Public GUI capability handoff | `AiGuiCapabilityHandoffTests.cs` | Keep the clipboard/GUI capability contract tied to its tests; add a diagram only if the guide explains a multi-step handoff that prose and tests do not make clear. |

### Coverage claims and limits

- The current test source has dedicated coverage for pre-load rejection, dependency ordering and graph failures, configuration bounds/copying, catalog/configuration/load/arbitration failures, convergence/non-convergence, staged-plan and schedule consistency, promotion/readiness transitions, and collection snapshot behavior. These are source-level observations, not a substitute for the exact candidate's CI results.
- The count of methods marked `[Fact]` or `[Theory]` is not the same as the number of executed test cases: theories can expand into multiple cases, and a method count does not establish a passing run. Use the workflow for the exact candidate as the pass/fail authority.
- No UML generation or animated sequence-highlighting capability is claimed to exist yet. The next useful UML artifact is an execution sequence grounded in `FsmCos.Execute` and the corresponding tests, followed by a type diagram grounded in public declarations. Hosted rendering, accessibility-tree exposure, keyboard navigation, and screen-reader behavior remain unverified.

