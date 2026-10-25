<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# FSM_COS Documentation Audit — Working Decision Register

**Purpose:** Rebuild the documentation around the product a developer is trying to understand and use. This is a maintainer working note, not product documentation and not a claim that the audit is complete.

**Standard:** No sentence, paragraph, page, diagram, or historical artifact is protected merely because it already exists. Preserve only material that explains a real contract, supports a real task, records important history, or provides evidence that cannot be cheaply recreated. Rewrite, merge, relocate, archive, or delete the rest after checking dependencies and source truth.

## Current checkpoint — 2026-10-25

The five-part reader journey is now aligned across `README.md`, `NuGet.README.md`, and `DOCUMENTATION_INDEX.md`:

1. What
2. Why
3. How
4. See it in a minute
5. Choose your next step

The documentation index is the public navigation layer. The new `docs/IMPLEMENTATION_DOCUMENTATION_MATRIX.md` is maintainer audit material and must not be linked as a packaged consumer document: the source tree and tests it links to are not shipped in the NuGet package. `AGENTS.md` now directs maintainers to the matrix and this audit register.

The first source/test map is in place, but the audit is **not complete**. A significant audit dimension is build-mode parity: normal package mode consumes published dependencies, while `WorkshopUseRepositorySources=true` defines `WORKSHOP_USE_REPOSITORY_SOURCES` and conditionally changes members such as pre-resolved arbitration/configuration fields. The public package API and repository-source API must be compared explicitly. Recent documentation commits triggered fresh Pack and CodeQL workflows; do not treat them as passed until the exact current HEAD is checked.

Immediate next passes:
- verify the current HEAD's package workflow and CodeQL results;
- compare the five-section structure and all local links in README, NuGet README, and index;
- complete the public-type/member inventory against both build modes;
- reconcile each contract page with implementation and tests, prioritizing composition failures, configuration ownership, rollback semantics, staged-plan metadata, and runtime-experience lifecycle;
- check whether each source companion/UML card/use-case is accurate and whether diagram generation is actually reproducible;
- record implementation gaps separately from documentation gaps before deciding what to implement.

## Product definition to keep consistent

FSM_COS is the Workshop's **Compute Operating System layer for runtime composition**. It takes a `RuntimeManifest`, resolves the requested MicroBundles through a host-supplied catalog, discovers and validates the dependency closure, loads prerequisites before dependents, performs bounded arbitration, and returns a `RuntimeAssembly` to the host.

The **FSM** name signals the FSM_API foundation and its finite-state-machine/state-context primitives. FSM_COS's own responsibility is composition; it is not a general-purpose operating system, an application scheduler, a GUI, an artifact repository, or the host application.

Every public document should answer the question appropriate to its role, use these boundaries consistently, distinguish current behavior from future ideas, and avoid repeating the entire product introduction.

## Decision rules

1. **Front door:** answer what it is, why a user would choose it, what it needs, and how to see it work before deep caveats or architecture vocabulary.
2. **Task guide:** give a reader a concrete goal, prerequisites, steps, expected output, failure interpretation, and next step.
3. **Contract reference:** state exact behavior and ownership; cite source/tests where possible; don't retell the README.
4. **Theory/design:** retain only if it explains a real architectural decision that is not already clear elsewhere. Label future/proposed behavior and keep it out of the product promise.
5. **Internal LLM/maintainer notes:** keep outside the public reader journey and outside the NuGet package. Preserve vision if useful, but label it as internal/proposed and prevent it from masquerading as implementation.
6. **UML/type cards:** useful when they help someone navigate source or understand relationships. Keep source links and explicit limitations; remove duplicates or generated-looking diagrams that are stale or don't improve understanding.
7. **Pictures:** every picture must teach something. Label conceptual art and generated contract maps honestly; animation is not runtime telemetry unless a real runtime feeds it.
8. **No speculative claims:** a source version is not proof of publication; staged metadata is not active deferred loading; a content-hash field is not hash verification; bounded arbitration is not proof of arbitrary semantic compatibility.
9. **No sunk-cost preservation:** consolidate duplicated explanations, remove stale claims, and retire artifacts whose only job was to guide an earlier implementation conversation. Before deleting unique material, decide whether its useful idea belongs in a canonical document.

## Changes completed in this pass

| Artifact | Decision | Result |
|---|---|---|
| `README.md` | Rewrite the front door around the Compute Operating System definition, why to use it, ecosystem responsibilities, and a concrete runnable example. | Rewritten and subsequently sharpened to lead with the user problem/payoff; linked repository guide is included in the NuGet package. |
| `NuGet.README.md` | Make the package page answer what the library does, why to use it, how to run the example, and how to check real publication status. | Rewritten and aligned to canonical footer; package link verification passed on the candidate above. |
| `docs/WHAT_IS_FSM_COS.md` | Replace the introductory explanation with a direct product definition, use cases, contract boundaries, and runnable proof. | Rewritten and aligned to canonical footer; verify in the full document pass. |
| `DOCUMENTATION_INDEX.md` | Organize links by reader goal instead of a flat topic inventory; keep internal branch history out of the public index. | Rewritten and iterated on `development`; latest package-local links were validated by the passing package workflow above. |

The package workflow passed on `da9078d7cf95243083cb0b078fd97239a1a8ab9c`: build-and-test, source integration, and public-package compatibility succeeded; `publish_nuget` was skipped. CodeQL was still running at that point. This audit/ledger update creates a new exact head, so rerun applicable checks before any release-readiness claim.

## Public documentation inventory and disposition

| Path | Intended role | Audit decision / next action | Status |
|---|---|---|---|
| `README.md` | First-contact product explanation | Lead with the user problem and payoff, then define the composition contract and show it working. Keep the front door concise; validate every claim and link against source and package state. | Rewritten and iterated; verify |
| `NuGet.README.md` | Package consumer landing page | Keep shorter than the repository README; ensure it renders correctly in NuGet and never implies unpublished versions are available. | Rewritten; verify |
| `DOCUMENTATION_INDEX.md` | Reader navigation | Keep task-oriented; every link must resolve and the index must not expose internal planning as a user guide. | Rewritten; verify |
| `docs/WHAT_IS_FSM_COS.md` | Beginner-level concept and use cases | Keep only as a deeper plain-language introduction; eliminate repetition with README/learning guide during next pass. | Rewritten; compare |
| `docs/LEARNING_GUIDE.md` | One end-to-end mental model | Follow one real request from manifest to dependency-first order, bounded arbitration, and host handoff; compare every claim against the runnable sample. | Rewritten; verify |
| `docs/CONSUMING_MICROBUNDLES.md` | Consumer implementation recipe | Preserve the actionable catalog/configuration examples and troubleshooting table; remove duplicated domain statements and repeated headings. | Edited for concision; contract review remains |
| `docs/ARCHITECTURE.md` | Implementation map | Keep source-derived relationships and runtime flow; generic resource/footer boilerplate has been removed. Validate signatures and staged-loading claims against source. | Boilerplate pruned; verify claims |
| `docs/DOMAIN.md` | Responsibility and ownership reference | Keep as the ownership map, not another algorithm walkthrough; link to external contracts and avoid duplicating Architecture. | Rewritten; verify |
| `docs/RUNTIME_MANIFEST.md` | Manifest contract | Keep as an exact reference; distinguish roots from optional planning metadata and what is actually validated. Generic footer boilerplate has been removed. | Boilerplate pruned; verify contract |
| `docs/RUNTIME_ASSEMBLY.md` | Result/handoff contract | Keep result semantics, snapshot behavior, and host responsibilities; remove repeated ecosystem/resource footer. | Boilerplate pruned; verify contract |
| `docs/ARBITRATION.md` | Convergence contract | Keep exact round semantics, failure conditions, and limits; generic footer boilerplate has been removed. | Boilerplate pruned; verify claims |
| `docs/RUNTIME_BOUNDARY.md` | Kernel-versus-host responsibilities | Keep as the concise integration/non-feature checklist; Domain owns conceptual responsibilities and Architecture owns the algorithm. | Rewritten; verify |
| `docs/STAGED_MANIFEST_LOADING.md` | Current planning metadata and non-features | Keep only as a precise statement of available metadata and explicit non-features; generic footer boilerplate has been removed. | Boilerplate pruned; verify claims |
| `docs/MANIFEST_THEORY.md` | Why the manifest is a published request | Retain the distinct authoring-to-publication rationale for now; compare against Runtime Manifest to determine whether it earns a separate page. Generic footer boilerplate has been removed. | Boilerplate pruned; consolidation decision open |
| `docs/THEORY.md` | Architectural rationale | Keep durable composition rationale and distinguish intended authoring context from implemented runtime behavior. Generic footer boilerplate has been removed; theory still needs a redundancy and source-truth pass. | Boilerplate pruned; deep review remains |
| `docs/WEBPAGE_INTEGRATION.md` | Host integration example/boundary | Keep only as a useful host recipe; make the absence of a shipped WebPage adapter clear without making the page mostly disclaimers. | Review next |
| `docs/GALLERY.md` | Visual tour | Organize exhibits as a consumer journey; each image must teach a distinct concept and link to its authoritative explanation. Source-only sample links must work from the NuGet package too. | Expanded; verify |
| `docs/WORKSHOP_REPOSITORIES.md` | Workshop project discovery | A user-facing index of repositories grouped by what someone wants to build. Verify every repository URL and one-line product description against live repositories; it is included in the NuGet package because the README links to it locally. | Added; verify links and package inclusion |
| `docs/UML_VISUAL_STYLE.md` | Diagram authoring standard | Keep as a maintainer standard; remove repeated resource footer. Compare its source/semantic/accessibility guidance against current GUI.SVG tooling before treating it as a proven workflow. | Boilerplate pruned; workflow proof remains |
| `docs/UML/README.md` | Type/source navigation | Keep as source navigation and explicitly label diagrams as manually authored. Relative links across the UML index, 16 source companions, 4 test companions, and 2 class diagrams were manually checked. Source/signature and visual accessibility review remains. | Links checked; content review remains |
| `docs/UML/ClassDiagram/*.md` | Focused class cards/use cases | Keep the two focused source-grounded diagrams; remove duplicate resource footers and verify claims, SVG rendering, and accessibility separately. | Boilerplate pruned; source/visual review remains |
| `docs/UML/src/FSM_COS/*.md` | Per-source-type companion pages | Replaced generic use cases with type-specific tasks, corrected broken relative source links, and removed repeated verification/resource boilerplate. Source signatures and class cards still need a full comparison. | Links/use cases revised; source review remains |
| `docs/UML/tests/FSM_COS.Tests/*.md` | Test/use-case reference | Replaced generic use cases with suite-specific guidance, corrected broken relative source links, and removed repeated boilerplate. Verify documented test counts and coverage statements against current test files. | Links/use cases revised; coverage review remains |
| `docs/releases/0.1.0-alpha.7.md` | Version-specific release notes | Keep as historical/candidate record; distinguish verified candidate behavior from publication status. | Review next |
| `docs/releases/0.1.0-alpha.6.md` | Prior release history | Preserve only as accurate history; do not duplicate current instructions or imply it is current. | Review next |
| `SECURITY.md` | Security contract and report guidance | Keep the actionable threat model and enforced limits; verify every numeric limit and claim against implementation/tests. | Review next |
| `.github/notes/BRANCH_RECONCILIATION.md` | Sith/maintainer branch ledger | Maintainer-only history; moved out of the public documentation tree and retained in notes. | Relocated; verify live refs before acting |
| `docs/assets/*.svg`, `*.gif` | Visual explanations | Audit each against source and accessible alt text; remove duplicates/stale art, keep only diagrams that teach a distinct concept. | Review next |

## Internal notes and other design artifacts

| Path | Decision |
|---|---|
| `.github/notes/AI_COMPOSITION.md` | Retain as internal/future architecture only if still useful to the Workshop vision. It explicitly says it is not the current FSM_COS contract. Keep out of public product claims and package contents. |
| `.github/notes/LIVING_SVG.md` | Retain as maintainer evidence for diagram generation and animation boundaries; verify pinned source/workflow claims and keep out of the end-user journey. |
| `.github/DEVELOPMENT.md` | Internal contributor instructions. Keep only current commands and repository conventions; remove instructions that assume missing sibling repositories or obsolete workflows. |
| `.github/release-readiness/0.1.0-alpha.7.md` | Release governance evidence, not product documentation. Preserve the exact-candidate rule; refresh every gate against one exact SHA before making a readiness claim. |
| `.github/notes/BRANCH_RECONCILIATION.md` | Historical/maintainer artifact, not a public user guide; retained under maintainer notes rather than the public documentation path. |

## Package metadata

The package description and tags in `src/FSM_COS/FSM_COS.csproj` now identify FSM_COS as the Compute Operating System for MicroBundle runtime composition rather than a generic platform-neutral library. This metadata change passed the public-package compatibility job on candidate `056adc64500a9a5d01552b4b760558d5b0657bbe`.

## Package metadata

`src/FSM_COS/FSM_COS.csproj` now describes FSM_COS as a Compute Operating System for MicroBundle runtime composition and adds search tags for that purpose. This passed package compatibility on candidate `056adc64500a9a5d01552b4b760558d5b0657bbe`; the latest package workflow result at `da9078d7cf95243083cb0b078fd97239a1a8ab9c` also passed, while CodeQL was still running at the time of this audit.

## Sample, code, and visual evidence

| Path family | Audit decision |
|---|---|
| `samples/FSM_COS.MinimalConsumer/Program.cs` and `Program.md` | Canonical runnable proof. README, NuGet README, and learning guide must agree with its behavior and expected output. |
| `samples/DocumentationVisuals/Program.cs` and `Program.md` | Documentation generator/evidence. Keep claims tied to the pinned GUI.SVG implementation and actual workflow. |
| `src/FSM_COS/*.cs` and tests | Source and tests are the authority for current behavior. Documentation must not substitute diagrams or plans for verification. |
| `docs/assets/section-dividers/*` and `section-markers/*` | Evaluate whether these add real navigation or simply inflate page length; do not keep merely because they look polished. |

## Required next passes

1. **Contract truth:** review all public guides against source/tests; correct or remove unsupported claims.
2. **Reader journey:** make the first 60 seconds coherent from README → runnable proof → consumer guide. Remove duplicate introductions.
3. **Document economics:** for each page, decide keep / rewrite / merge / relocate / archive / delete. Every kept page must have a distinct audience and purpose.
4. **UML audit:** verify all per-class and test companions against their source; ensure the public type index has no broken links.
5. **Visual audit:** verify SVG source provenance, text readability, reduced-motion behavior, alt text, and whether each picture earns its place.
6. **Automated checks:** run repository Markdown/link validation, tests, packaging checks, and package README inspection on the resulting exact commit.
7. **Release claims:** only after checks on one exact candidate SHA, update the release-readiness ledger. No percentage should be inferred from prose quality or historical CI.

**Non-negotiable:** this register is not permission to publish, merge PR #35, or delete branches. The documentation pass is on `development`; integration governance remains a separate owner-reviewed decision.

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
