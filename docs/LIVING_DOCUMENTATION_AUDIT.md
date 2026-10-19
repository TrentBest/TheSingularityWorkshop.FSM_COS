# Living Documentation Visual Audit

**Purpose:** make FSM_COS documentation easier to understand through accurate diagrams, animated SVGs, interactive examples, and other living visuals where they materially teach the subject.

**Status:** audit started; coverage is not yet complete. This checklist is intentionally evidence-based: an SVG existing in `docs/assets` does not mean it is embedded in the relevant document, and an embedded picture does not automatically qualify as a useful living visual.

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

## Existing visual assets discovered

The repository already contains a useful SVG library, including:

- `docs/assets/fsm-cos-overview.svg`
- `docs/assets/fsm-cos-system.svg`
- `docs/assets/fsm-cos-domain-map.svg`
- `docs/assets/fsm-cos-crane.svg`
- `docs/assets/composition-boundary.svg`
- `docs/assets/runtime-boundary.svg`
- `docs/assets/runtime-manifest-pipeline.svg`
- `docs/assets/dependency-resolution.svg`
- `docs/assets/ai-composition.svg`
- `docs/assets/arbitration-convergence.svg`
- `docs/assets/runtime-assembly-handoff.svg`
- `docs/assets/microbundle-cartridge.svg`

There are also section divider/marker assets. These are a starting library, not proof that every useful document already has its own appropriate visual.

## Coverage checklist

Use the status labels consistently:
- **Embedded / verify:** a visual appears to exist in the document; verify accuracy, accessibility, and that it explains the specific topic.
- **Add / improve:** an explanatory visual is likely useful and needs to be embedded, created, or materially improved.
- **Review:** inspect the full document before deciding; do not assume an image is necessary.

| Document | Visual intent | Initial action | Status |
|---|---|---|---|
| `AGENTS.md` | Repository-specific operating guardrails | Plain-language constraints are the point; a diagram risks obscuring authoritative instructions | No visual needed — operational rules are best kept explicit |
| `README.md` | One-screen composition overview | Preserve overview SVG; verify it is accurate and not merely decorative | Embedded / verify |
| `docs/WHAT_IS_FSM_COS.md` | Request → composition → host; newcomer mental model | Added the overview SVG with an explanatory caption; validate visual semantics and accessibility | Embedded / verify |
| `docs/ARCHITECTURE.md` | Kernel boundary, catalog, dependency and host relationships | Verify existing system SVG and reuse focused diagrams only where they add clarity | Embedded / verify |
| `docs/THEORY.md` | Why composition is distinct; invariants and non-goals | Existing system SVG is embedded; verify it against current theory and assess whether a separate lifecycle visual adds value | Embedded / verify |
| `docs/DOMAIN.md` | What FSM_COS owns versus adjacent domains | Added domain map with caption; verify every edge against current ownership | Embedded / verify |
| `docs/DOMAIN_COMPLETION_ROADMAP.md` | Implemented, partial, deferred, and gated capabilities | Add a visual capability/status map grounded in the checklist | Add / improve |
| `docs/RUNTIME_MANIFEST.md` | Caller request and manifest contents | Added alt text and explicit scope note: the authoring/validation/closure/bake SVG is conceptual; implemented contract begins at `RuntimeManifest` and `FsmCos.Execute` | Existing visual verified with scope caveat |
| `docs/MANIFEST_THEORY.md` | Semantic request versus serialized representation | Existing manifest-pipeline SVG is embedded; verify that the editor-time/published-request distinction is clear | Embedded / verify |
| `docs/STAGED_MANIFEST_LOADING.md` | Preflight, planning, validation, loading, failure boundaries | Added dependency-resolution SVG with an explicit caveat that it does not claim end-to-end deferred loading | Embedded / verify |
| `docs/CONSUMING_MICROBUNDLES.md` | Consumer flow, catalog responsibility, dependency ordering | Verified dependency graph and depth-first dependency-before-dependent load order against `FsmCos.LoadRoot`; alt text distinguishes illustrative graph from runtime contract | Existing visual verified |
| `docs/ARBITRATION.md` | Iterations, convergence, limit, non-convergence | Verified against `FsmCos.Execute`; added alt text and a caption documenting the zero-based converging-round index and failure at the limit | Existing visual verified |
| `docs/RUNTIME_ASSEMBLY.md` | Result shape and handoff | Verified `runtime-assembly-handoff.svg` against `RuntimeAssembly` and `FsmCos.Execute`; added descriptive alt text and a caption separating the implemented composition boundary from host-owned execution | Existing visual verified |
| `docs/RUNTIME_BOUNDARY.md` | Explicitly excluded responsibilities | Verified both boundary SVGs against `FsmCos.Execute`, `IFsmCos`, and `RuntimeAssembly`; clarified that catalog/configuration are external contracts invoked by the kernel, not kernel-owned implementations | Existing visuals verified |
| `docs/AI_COMPOSITION.md` | AI-oriented composition contracts and current limits | Added AI-composition SVG and explicit conceptual/status caveat; verify diagram labels against current domains | Embedded / verify |
| `docs/ECOSYSTEM_INTEGRATION_MAP.md` | Package roles and dependency direction | Added system SVG to the conceptual target section with a caveat separating target architecture from verified package facts | Embedded / verify |
| `docs/WEBPAGE_INTEGRATION.md` | Kernel/host separation and WebPage handoff | Existing overview SVG is embedded; verify it explains the host asymmetry without implying full runtime integration | Embedded / verify |
| `NuGet.README.md` | Package-first overview for NuGet consumers | Overview SVG and moniker are embedded; verify remote asset rendering and package inclusion | Embedded / verify |
| `docs/DEVELOPMENT.md` | Build/test/release verification path | Added a Mermaid flow matching the three verification jobs and manual publish gate in `.github/workflows/package.yml` | Embedded / verify |
| `docs/0.1.0-alpha.7-READINESS.md` | Release gates, evidence, exact candidate/package identity | Checklist and evidence links are the clearest current proof; a separate flow may duplicate Development's diagram | Review |
| `docs/releases/0.1.0-alpha.7.md` | Scope and behavioral change | Added a Mermaid view of the read-only snapshot contract, including the explicit mutable-element caveat | Embedded / verify |
| `docs/releases/0.1.0-alpha.6.md` | Historical release scope and contract | Keep as a historical record; add a visual only if it explains a contract not already covered elsewhere | Review |
| `DOCUMENTATION_INDEX.md` | Navigation by reader goal | Text links and grouped navigation are appropriate; no diagram currently needed | No visual needed — navigation is the purpose |
| `DOCUMENTATION_STANDARD.md` | Shared rules for visual, living documentation | Text-first normative standard; examples and requirements are clearer than a decorative summary image | No visual needed — rules and checklists are the authoritative form |
| `docs/BRANCH_RECONCILIATION.md` | Branch inventory and disposition decisions | Existing comparison tables are the useful visual structure; a diagram would add little to the branch-by-branch evidence | No additional visual needed — inventory tables already serve the task |
| `docs/DEPENDENCY_ALIGNMENT_CHECKLIST.md` | Package/version drift and ordered remediation | Existing tables and ordered checklist make state and action explicit | No additional visual needed — tables/checklist are more precise |
| `docs/CONTINUATION_BRIEF_ALPHA6_FORGE_ANYAPP_WEBPAGE.md` | Historical handoff and integration constraints | Dated continuation brief; prioritize correcting stale instructions rather than adding a diagram | Review — historical operational claims may be stale |
| `docs/PAUSE_POINT.md` | Current pause gate and historical operational notes | Current status bullets and guardrails are the proof; inspect stale dated notes before adding visuals | Review — historical operational claims may be stale |
| `docs/LIVING_DOCUMENTATION_AUDIT.md` | Coverage, validation, and completion criteria | This document is the coverage record; a separate picture would duplicate the status table | No visual needed — checklist is the source of truth |

This is an initial prioritized pass, not a claim that the remaining documents have been exhaustively inspected. Add rows for any further document where a visual would genuinely improve comprehension.

## Progress recorded on 2026-10-19

- [x] Created and indexed this coverage audit; standardized the living-visual requirements in `DOCUMENTATION_STANDARD.md`.
- [x] Embedded explanatory SVGs in `WHAT_IS_FSM_COS.md`, `CONSUMING_MICROBUNDLES.md`, `STAGED_MANIFEST_LOADING.md`, `DOMAIN.md`, `AI_COMPOSITION.md`, and `ECOSYSTEM_INTEGRATION_MAP.md`.
- [x] Reconciled the audit with existing visuals in `THEORY.md`, `RUNTIME_MANIFEST.md`, `MANIFEST_THEORY.md`, `ARBITRATION.md`, `RUNTIME_ASSEMBLY.md`, `RUNTIME_BOUNDARY.md`, and `WEBPAGE_INTEGRATION.md`; these are recorded as **embedded / verify**, not as fully validated.
- [x] Verify documentation CI for the initial visual pass, audit reconciliation, and ecosystem-map update; workflows `39185334121`, `39185518082`, and `39185482305` all completed successfully. The latest Development workflow-diagram change is pending CI.
- [ ] Finish the full Markdown inventory and verify existing diagrams against implementation. Every tracked Markdown file now has a row or an explicit no-additional-visual rationale; source-accuracy verification is still open.
- [ ] Establish which visuals can be made genuinely living (semantic animation or source-backed GUI) and verify rendering/accessibility rather than assuming a static SVG is animated.
- [x] Added a Mermaid verification/release-gate flow to `docs/DEVELOPMENT.md`, based on the current package workflow's three required verification jobs and manual publish condition.
- [x] Added a source-aligned Mermaid diagram to the alpha.7 release notes showing collection snapshot protection and the limitation that MicroBundle instances remain domain-owned.

**Important distinction:** the current SVGs inspected in this pass are static diagrams. They improve visual coverage but do not, by themselves, satisfy the stronger goal of animated or interactive living documentation. That remains an explicit follow-up, not a completed claim.

## Execution order

### Pass 1 — canonical visuals in high-value contract docs
- [x] Verified `arbitration-convergence.svg` against `FsmCos.Execute`; documented the zero-based `RuntimeAssembly.ArbitrationRounds` value and no-assembly-on-nonconvergence behavior.
- [x] Verified `dependency-resolution.svg` against `FsmCos.LoadRoot`; documented dependency-first loading, duplicate-ID skipping, cycle detection, and that the pictured graph is illustrative.
- [x] Verified the `RUNTIME_MANIFEST.md` pipeline graphic against `RuntimeManifest` and `FsmCos.Execute`; clarified that editor authoring, closure baking, and publication are conceptual upstream stages, not shipped FSM_COS behavior.
- [x] Verified `runtime-assembly-handoff.svg` against `RuntimeAssembly` and `FsmCos.Execute`; added alt text and a caption that distinguishes the implemented assembly contract from host-owned execution.
- [x] Verified `runtime-boundary.svg` and `composition-boundary.svg` against `FsmCos.Execute`, `IFsmCos`, and `RuntimeAssembly`; clarified the difference between invoking an external contract and owning its implementation.
- [ ] Continue verifying remaining SVGs against current source and each document's exact topic.
- [ ] Embed the existing relevant SVG in the best canonical home instead of creating near-duplicates.
- [ ] Add concise alt text and a caption that explains the visual's point.
- [ ] Ensure relative asset links resolve on `development` (full-repository validation still pending).

### Pass 2 — living behavior, not just static architecture
- [ ] Identify the best candidate for an animated, semantic SVG or a small working GUI.
- [ ] Animate only meaningful behavior (for example, dependency travel, arbitration rounds, convergence, or handoff).
- [ ] Keep a static accessible explanation available; do not make animation essential to understanding.
- [ ] Where feasible, link diagrams to the exact source, sample, or test that substantiates them.

### Pass 3 — coverage verification
- [x] Inventory every Markdown document in the repository, including release notes and working/continuation documents (29 files; all 29 have a unique audit row). Source-accuracy and link validation remain open.
- [ ] For each document, record **visual added**, **existing visual verified**, or **no visual needed (with reason)**.
- [ ] Validate all local image/asset links and alt text.
- [ ] Confirm no visual implies an unimplemented feature or contradicts source/tests.
- [ ] Keep this checklist updated in the same documentation pass as visual changes.

## Definition of done

This audit is complete only when every documentation file has been considered, every visual that materially helps is present and source-aligned, every local asset reference is valid, and remaining text-only documents have a recorded reason. “There are many SVGs in the repository” is not a completion criterion.
