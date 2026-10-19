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
| `README.md` | One-screen composition overview | Preserve overview SVG; verify it is accurate and not merely decorative | Embedded / verify |
| `docs/WHAT_IS_FSM_COS.md` | Request → composition → host; newcomer mental model | Added the overview SVG with an explanatory caption; validate visual semantics and accessibility | Embedded / verify |
| `docs/ARCHITECTURE.md` | Kernel boundary, catalog, dependency and host relationships | Verify existing system SVG and reuse focused diagrams only where they add clarity | Embedded / verify |
| `docs/THEORY.md` | Why composition is distinct; invariants and non-goals | Add or embed the canonical composition-boundary visual and a compact lifecycle visual | Add / improve |
| `docs/DOMAIN.md` | What FSM_COS owns versus adjacent domains | Added domain map with caption; verify every edge against current ownership | Embedded / verify |
| `docs/DOMAIN_COMPLETION_ROADMAP.md` | Implemented, partial, deferred, and gated capabilities | Add a visual capability/status map grounded in the checklist | Add / improve |
| `docs/RUNTIME_MANIFEST.md` | Caller request and manifest contents | Embed manifest-pipeline visual with labeled input/output | Add / improve |
| `docs/MANIFEST_THEORY.md` | Semantic request versus serialized representation | Add request/representation distinction visual | Add / improve |
| `docs/STAGED_MANIFEST_LOADING.md` | Preflight, planning, validation, loading, failure boundaries | Added dependency-resolution SVG with an explicit caveat that it does not claim end-to-end deferred loading | Embedded / verify |
| `docs/CONSUMING_MICROBUNDLES.md` | Consumer flow, catalog responsibility, dependency ordering | Added dependency-resolution SVG and caption; consumer example coverage still needs review | Embedded / verify |
| `docs/ARBITRATION.md` | Iterations, convergence, limit, non-convergence | Embed arbitration-convergence visual; verify round limit and meaning | Add / improve |
| `docs/RUNTIME_ASSEMBLY.md` | Result shape and handoff | Embed assembly-handoff visual | Add / improve |
| `docs/RUNTIME_BOUNDARY.md` | Explicitly excluded responsibilities | Embed boundary diagram and responsibility comparison | Add / improve |
| `docs/AI_COMPOSITION.md` | AI-oriented composition contracts and current limits | Added AI-composition SVG and explicit conceptual/status caveat; verify diagram labels against current domains | Embedded / verify |
| `docs/ECOSYSTEM_INTEGRATION_MAP.md` | Package roles and dependency direction | Verify domain map; visually distinguish confirmed integrations from candidates | Embedded / verify |
| `docs/WEBPAGE_INTEGRATION.md` | Kernel/host separation and WebPage handoff | Add a host-handoff diagram grounded in current integration status | Add / improve |
| `docs/DEVELOPMENT.md` | Build/test/release verification path | Consider a small verification flow; avoid ornamental graphics | Review |
| `docs/0.1.0-alpha.7-READINESS.md` | Release gates, evidence, exact candidate/package identity | Keep checklist authoritative; consider a compact gate flow only if it improves scanability | Review |
| `docs/releases/0.1.0-alpha.7.md` | Scope and behavioral change | Link to the relevant contract visuals; avoid duplicating the README hero | Review |
| `DOCUMENTATION_INDEX.md` | Navigation by reader goal | Add this audit to the document map; avoid a visual on every index row | Review |
| `DOCUMENTATION_STANDARD.md` | Shared rules for visual, living documentation | Extend standard with explicit coverage and maintenance requirements | Add / improve |

This is an initial prioritized pass, not a claim that the remaining documents have been exhaustively inspected. Add rows for any further document where a visual would genuinely improve comprehension.

## Progress recorded on 2026-10-19

- [x] Created and indexed this coverage audit; standardized the living-visual requirements in `DOCUMENTATION_STANDARD.md`.
- [x] Embedded explanatory SVGs in `WHAT_IS_FSM_COS.md`, `CONSUMING_MICROBUNDLES.md`, `STAGED_MANIFEST_LOADING.md`, `DOMAIN.md`, and `AI_COMPOSITION.md`.
- [ ] Verify the latest documentation CI runs for those changes.
- [ ] Audit all remaining Markdown files and verify existing diagrams against implementation.
- [ ] Establish which visuals can be made genuinely living (semantic animation or source-backed GUI) and verify rendering/accessibility rather than assuming a static SVG is animated.

**Important distinction:** the current SVGs inspected in this pass are static diagrams. They improve visual coverage but do not, by themselves, satisfy the stronger goal of animated or interactive living documentation. That remains an explicit follow-up, not a completed claim.

## Execution order

### Pass 1 — canonical visuals in high-value contract docs
- [ ] Verify existing SVGs against current source and each document's exact topic.
- [ ] Embed the existing relevant SVG in the best canonical home instead of creating near-duplicates.
- [ ] Add concise alt text and a caption that explains the visual's point.
- [ ] Ensure relative asset links resolve on `development`.

### Pass 2 — living behavior, not just static architecture
- [ ] Identify the best candidate for an animated, semantic SVG or a small working GUI.
- [ ] Animate only meaningful behavior (for example, dependency travel, arbitration rounds, convergence, or handoff).
- [ ] Keep a static accessible explanation available; do not make animation essential to understanding.
- [ ] Where feasible, link diagrams to the exact source, sample, or test that substantiates them.

### Pass 3 — coverage verification
- [ ] Inspect every Markdown document in the repository, including release notes and working/continuation documents.
- [ ] For each document, record **visual added**, **existing visual verified**, or **no visual needed (with reason)**.
- [ ] Validate all local image/asset links and alt text.
- [ ] Confirm no visual implies an unimplemented feature or contradicts source/tests.
- [ ] Keep this checklist updated in the same documentation pass as visual changes.

## Definition of done

This audit is complete only when every documentation file has been considered, every visual that materially helps is present and source-aligned, every local asset reference is valid, and remaining text-only documents have a recorded reason. “There are many SVGs in the repository” is not a completion criterion.
