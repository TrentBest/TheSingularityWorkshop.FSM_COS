<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# FSM_COS Documentation Audit — Working Decision Register

**Purpose:** Rebuild the documentation around the product a developer is trying to understand and use. This is a maintainer working note, not product documentation and not a claim that the audit is complete.

**Standard:** No sentence, paragraph, page, diagram, or historical artifact is protected merely because it already exists. Preserve only material that explains a real contract, supports a real task, records important history, or provides evidence that cannot be cheaply recreated. Rewrite, merge, relocate, archive, or delete the rest after checking dependencies and source truth.

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
| `README.md` | Rewrite the front door around the Compute Operating System definition, why to use it, ecosystem responsibilities, and a concrete runnable example. | Rewritten on `development`; commit `82f58ce5aab0efffa1a9674004bf3de95347dc76`. |
| `NuGet.README.md` | Make the package page answer what the library does, why to use it, how to run the example, and how to check real publication status. | Rewritten on `development`; commit `9dead24a8505f7b8f9224944ee5b1c1aa65814bb`. |
| `docs/WHAT_IS_FSM_COS.md` | Replace the introductory explanation with a direct product definition, use cases, contract boundaries, and runnable proof. | Rewritten on `development`; commit `9d54a27f377f8a9c90c1e9b4c615bb267405db68`. |
| `DOCUMENTATION_INDEX.md` | Organize links by reader goal instead of a flat topic inventory. | Rewritten on `development`; commit `483b0f0005d1c78c9095000df19c9e123c38f05f`. |

These are content edits, not a verified release. They still need local-link, Markdown, package-rendering, and source-claim checks.

## Public documentation inventory and disposition

| Path | Intended role | Audit decision / next action | Status |
|---|---|---|---|
| `README.md` | First-contact product explanation | Keep as the concise front door; validate every claim and link against source and package state. | Rewritten; verify |
| `NuGet.README.md` | Package consumer landing page | Keep shorter than the repository README; ensure it renders correctly in NuGet and never implies unpublished versions are available. | Rewritten; verify |
| `DOCUMENTATION_INDEX.md` | Reader navigation | Keep task-oriented; every link must resolve and the index must not expose internal planning as a user guide. | Rewritten; verify |
| `docs/WHAT_IS_FSM_COS.md` | Beginner-level concept and use cases | Keep only as a deeper plain-language introduction; eliminate repetition with README/learning guide during next pass. | Rewritten; compare |
| `docs/LEARNING_GUIDE.md` | One end-to-end mental model | Keep if it teaches by following one real example; compare against sample and remove repeated conceptual paragraphs. | Review next |
| `docs/CONSUMING_MICROBUNDLES.md` | Consumer implementation recipe | High-value public guide; validate code signatures, steps, failure modes, and output against current source/tests. | Review next |
| `docs/ARCHITECTURE.md` | Implementation map | Keep the source-derived relationships and runtime flow; remove repeated theory and ensure diagrams don't overstate staged loading. | Review next |
| `docs/DOMAIN.md` | Responsibility and ownership reference | Keep only the domain boundaries unique to this page; link to the external MicroBundleDomain contract rather than restating it. | Review next |
| `docs/RUNTIME_MANIFEST.md` | Manifest contract | Keep as an exact reference; prominently distinguish roots from optional planning metadata and clarify what is actually validated. | Review next |
| `docs/RUNTIME_ASSEMBLY.md` | Result/handoff contract | Keep if it explains public result semantics, snapshot behavior, and host responsibilities not already obvious in API docs. | Review next |
| `docs/ARBITRATION.md` | Convergence contract | Keep the exact round semantics, failure conditions, and limits; trim introductory repetition. | Review next |
| `docs/RUNTIME_BOUNDARY.md` | Kernel-versus-host responsibilities | Candidate for consolidation with Architecture/Domain if it repeats the same lists without a distinct reader task. | Review next |
| `docs/STAGED_MANIFEST_LOADING.md` | Current planning metadata and non-features | Keep only if this feature surface exists publicly; name it so readers do not mistake metadata for active deferred loading. | Review next |
| `docs/MANIFEST_THEORY.md` | Why the manifest is a published request | Merge into the canonical theory or Runtime Manifest reference if it adds no distinct insight. | Review next |
| `docs/THEORY.md` | Architectural rationale | Keep the unique, durable design rationale; move implementation detail to contract references and label future ideas. | Review next |
| `docs/WEBPAGE_INTEGRATION.md` | Host integration example/boundary | Keep only as a useful host recipe; make the absence of a shipped WebPage adapter clear without making the page mostly disclaimers. | Review next |
| `docs/GALLERY.md` | Visual tour | Keep concise; each exhibit must add meaning, link to its authoritative explanation, and distinguish concept art from live evidence. | Review next |
| `docs/UML_VISUAL_STYLE.md` | Diagram authoring standard | Maintainer-facing standard, not a required end-user read; decide whether this belongs under internal notes or a cross-repo docs standard. | Review next |
| `docs/UML/README.md` | Type/source navigation | Keep if it materially improves source navigation; verify every companion path and label manually maintained material honestly. | Review next |
| `docs/UML/ClassDiagram/*.md` | Focused class cards/use cases | Keep one canonical card per meaningful public class; validate class names, members, relationships, and use cases against source. | Review next |
| `docs/UML/src/FSM_COS/*.md` | Per-source-type companion pages | Validate one-to-one correspondence with current `.cs` files; remove stale members and ensure each page has purpose, moniker, class card, and use cases. | Review next |
| `docs/UML/tests/FSM_COS.Tests/*.md` | Test/use-case reference | Keep only if it helps readers learn usage or contract coverage; otherwise link to the actual test files from a smaller test map. | Review next |
| `docs/releases/0.1.0-alpha.7.md` | Version-specific release notes | Keep as historical/candidate record; distinguish verified candidate behavior from publication status. | Review next |
| `docs/releases/0.1.0-alpha.6.md` | Prior release history | Preserve only as accurate history; do not duplicate current instructions or imply it is current. | Review next |
| `SECURITY.md` | Security contract and report guidance | Keep the actionable threat model and enforced limits; verify every numeric limit and claim against implementation/tests. | Review next |
| `docs/BRANCH_RECONCILIATION.md` | Sith/maintainer branch ledger | Not end-user documentation. Move to `.github/notes/` after verifying the latest ledger and update links before deleting the public-path copy. | Relocate |
| `docs/assets/*.svg`, `*.gif` | Visual explanations | Audit each against source and accessible alt text; remove duplicates/stale art, keep only diagrams that teach a distinct concept. | Review next |

## Internal notes and other design artifacts

| Path | Decision |
|---|---|
| `.github/notes/AI_COMPOSITION.md` | Retain as internal/future architecture only if still useful to the Workshop vision. It explicitly says it is not the current FSM_COS contract. Keep out of public product claims and package contents. |
| `.github/notes/LIVING_SVG.md` | Retain as maintainer evidence for diagram generation and animation boundaries; verify pinned source/workflow claims and keep out of the end-user journey. |
| `.github/DEVELOPMENT.md` | Internal contributor instructions. Keep only current commands and repository conventions; remove instructions that assume missing sibling repositories or obsolete workflows. |
| `.github/release-readiness/0.1.0-alpha.7.md` | Release governance evidence, not product documentation. Preserve the exact-candidate rule; refresh every gate against one exact SHA before making a readiness claim. |
| `docs/BRANCH_RECONCILIATION.md` | Historical/maintainer artifact, not a public user guide; relocation above preserves the work while fixing its audience. |

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
