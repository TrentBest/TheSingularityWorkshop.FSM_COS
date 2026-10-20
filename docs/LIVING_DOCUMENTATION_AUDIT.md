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
| `README.md` | One-screen composition overview | Overview SVG compared with the README and architecture boundary; host ownership after handoff is explicit | Source reviewed; hosted checks pending |
| `docs/WHAT_IS_FSM_COS.md` | Request → composition → host; newcomer mental model | Overview SVG and alt text reviewed against the plain-language contract and host handoff | Source reviewed; hosted checks pending |
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
| `SECURITY.md` | Trust boundary and adversarial verification matrix | The threat model and control/evidence matrix are clearest as explicit text; a diagram would not add a stronger security guarantee | No visual needed — security controls and residual risks are stated directly |
| `docs/GALLERY.md` | Visual navigation across the package's concepts | The gallery is itself the visual index and links to the detailed contract guides | No additional visual needed — another overview would duplicate the gallery's purpose |
| `docs/BRANCH_RECONCILIATION.md` | Historical branch inventory and disposition decisions | Archive note points to current authorities; the dated inventory remains in Git history | No visual needed — archived decision record is navigation only |
| `docs/DEPENDENCY_ALIGNMENT_CHECKLIST.md` | Historical cross-repository dependency pins and migration sequence | Archive note points to current FSM_COS authorities; the dated matrix remains in Git history | No visual needed — archived pin inventory is not current evidence |
| `docs/CONTINUATION_BRIEF_ALPHA6_FORGE_ANYAPP_WEBPAGE.md` | Historical handoff and integration constraints | Archived alpha.6 notes now direct readers to current release and architecture authorities | No visual needed — archived handoff is a navigation note, not a current specification |
| `docs/PAUSE_POINT.md` | Historical engineering pause decision | Archived pause note now directs readers to the maintained alpha.7 readiness gate and domain roadmap | No visual needed — archive notice and authority links are sufficient |
| `docs/LIVING_DOCUMENTATION_AUDIT.md` | Coverage, validation, and completion criteria | This document is the coverage record; a separate picture would duplicate the status table | No visual needed — checklist is the source of truth |

## Current verification boundary

The following findings are verified at the source-markup level:

- The 13 top-level SVG assets in `docs/assets` each include an embedded `<title>`, `<desc>`, `role="img"`, and `aria-labelledby`; 12 are substantive diagrams, while `fsm-cos-crane.svg` is a conceptual metaphor rather than a source-accurate flow diagram.
- The 13 section-divider SVGs are decorative and use `role="presentation"` with `aria-hidden="true"`.
- The four section-marker SVGs expose concise accessible labels.
- `dependency-resolution.svg` uses reduced-motion-aware animation to illustrate an example dependency-first order. It is not a live execution trace.
- The inventory covered all 31 tracked Markdown documents on the reviewed `development` snapshot and found no unresolved inline relative Markdown or image/asset targets after the release-runbook link was corrected. Recount the inventory when documents are added or removed.

These findings do **not** establish complete semantic accuracy for every visual, hosted SVG rendering, browser accessibility-tree exposure, keyboard behavior, screen-reader output, or external-link reachability. Those require their own evidence and must not be inferred from source markup or a green build.

## Remaining work

- [x] Source-review diagram claims against linked implementation/tests and clarify conceptual versus implemented behavior in adjacent text and alt descriptions.
- [ ] Check hosted rendering in GitHub and relevant light/dark and narrow-width views.
- [ ] Verify accessibility-tree exposure and screen-reader behavior for substantive and animated visuals.
- [ ] Check reference-style Markdown link definitions and external-link reachability.
- [x] Reconcile status labels where reviewed: source-reviewed visuals explicitly retain hosted checks as pending, while text-only and historical documents record why no visual is needed.
- [x] No additional interactive example is justified at this stage: the minimal consumer and regression tests demonstrate actual behavior more directly than a second illustrative GUI.

## Definition of done

This audit is complete only when every documentation file has been considered, each visual that materially helps is present and source-aligned, every local asset reference is valid, remaining text-only documents have a recorded reason, and the open verification boundaries above are closed or explicitly accepted with a documented rationale. Visual quantity is not a completion criterion.
