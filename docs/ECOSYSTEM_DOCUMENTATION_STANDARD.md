# Ecosystem Documentation Standard

**Status:** proposed shared standard for review  
**Scope:** README files and package documentation across The Singularity Workshop ecosystem  
**Purpose:** make every repository easier to recognize, understand, try, and investigate without forcing unrelated packages to depend on one another.

This document standardizes the *reader's journey*, not the architecture or vocabulary of every package. Each repository remains responsible for explaining its own domain accurately.

## 1. The governing idea

A reader should be able to answer these questions in order:

1. **What is this?**
2. **Why does it exist?**
3. **Can I see it work quickly?**
4. **How do I use it?**
5. **Where are its boundaries, and how does it relate to other parts of the ecosystem?**
6. **Where do I go next, depending on my background and goals?**

The documentation should reduce the amount of interpretation a reader must do before they can learn. It should *edify, not mystify*.

## 2. Standard README sequence

Use these numbered sections as the default order. Keep the leading numbers even if visual styling is unavailable or the page is printed in black and white. The number is the durable identifier; color is only a supplementary visual cue.

| Number | Section | Reader's question | Required content |
|---|---|---|---|
| **01** | **What It Is** | What am I looking at? | A plain-language definition, the primary responsibility, and what the package produces or enables. Explain acronyms on first use. |
| **02** | **Why It Exists** | What problem does this solve? | The problem, the reason this package is a separate boundary, and the benefit it aims to provide. Avoid claims that have not been measured or demonstrated. |
| **03** | **See It Work in 60 Seconds** | Can I experience it instead of just reading about it? | The shortest honest, repeatable demonstration. Include prerequisites, exact steps, expected output or observable behavior, and a recovery note for common failure. If 60 seconds is not realistic, say so and give the shortest verified path. |
| **04** | **Get Started** | How do I try or adopt it? | Installation, prerequisites, a minimal working example, and the path from a blank project to a useful result. Prefer a source/Visual Studio path when appropriate, and include package-manager instructions when supported. |
| **05** | **How It Works** | What is happening underneath? | The key concepts and a simple flow or diagram. Introduce terms before relying on them. Link to deeper explanations instead of making the README an encyclopedia. |
| **06** | **Boundaries and Relationships** | What does it own, and what does it not own? | Responsibilities, non-goals, dependency direction, integration points, and what belongs to neighboring packages. Explain relationships without implying a compile-time dependency where none exists. |
| **07** | **Usage and Examples** | How do I apply this to my situation? | Common tasks, representative examples, configuration, and meaningful variations. Include language- or platform-specific examples only when supported. |
| **08** | **Reference and Deep Dives** | Where can I inspect the details? | API/reference documentation, design notes, diagrams, decisions, benchmarks, and focused technical articles, each with a clear description of what it covers. |
| **09** | **Development and Verification** | How is this maintained, and how can I contribute? | Build/test commands, supported targets, repository layout when useful, contribution guidance, and the current development/release state. Distinguish verified facts from plans. |
| **10** | **Resources and Support** | Where do I go next or ask for help? | Relevant issues, discussions, related repositories/packages, support paths, and the shared Workshop footer. Keep links current and explain why a reader might follow them. |

This is a default, not a demand to invent content. If a section does not apply, omit it only when its absence is obvious; otherwise state **Not applicable** with a short reason. Do not pad a README with empty headings merely to satisfy a checklist.

## 3. The 60-second demonstration is a product feature

The quick demonstration is not filler and must not be reduced to an installation badge or a code fragment that cannot run. It is the reader's first proof that the package is real.

A useful demonstration should:

- begin with a clean, explicit prerequisite list;
- use commands and files that actually exist in the repository or a supported package;
- show the expected result, including what the reader should see or learn;
- avoid relying on unmentioned local state, credentials, unpublished packages, or hidden setup;
- state when a step is illustrative rather than verified;
- be checked whenever the relevant implementation or package version changes.

When a package is primarily a contract, library, or infrastructure component, the demonstration can show a tiny consumer, a test, a data transformation, or a visible result. Choose the smallest example that proves the package's particular value.

## 4. Explain the package for different readers

Do not make the reader guess whether a page is intended for them. Keep one truthful source of information, then provide clearly signposted reading paths.

- **New to the concept / “for dummies” path:** plain-language purpose, concrete analogy where helpful, prerequisites, the 60-second experience, and a glossary of unavoidable terms. “For dummies” means *minimum prior knowledge required*, not reduced respect or rigor.
- **New to coding or new to C#:** explain where files go, how to run the example, what output means, and which language concepts are being used. Never assume every reader is a C# developer.
- **Working developer path:** install, copyable minimal example, API usage, configuration, error handling, and supported versions.
- **Advanced / “deep dive” path:** design rationale, invariants, trade-offs, extension points, performance measurements, failure modes, and links to source/tests.

“Package Name for Geniuses” is not a useful assumption about the reader. Prefer **Deep Dive**, **Design Notes**, or **Advanced Reference**: expertise is contextual, and advanced material should explain its prerequisites rather than imply a hierarchy of intelligence.

## 5. Numbering and color

### Numbering is the stable identity

- Use two-digit section identifiers: **01, 02, 03 … 10**.
- Preserve the same number-to-purpose mapping across package READMEs.
- Use the number in the heading itself, not only in an image, badge, or table of contents.
- If a section needs subsections, use ordinary Markdown hierarchy beneath it; do not renumber the shared top-level sections to fit local preferences.
- Keep **01 = What**, **02 = Why**, and **03 = See It Work in 60 Seconds** consistent. These are the essential opening sequence.

### Color is an accessibility aid, not the meaning

A shared color palette has **not yet been established**. Do not independently invent or silently standardize one in individual repositories.

When a palette is agreed, it must meet all of these rules:

- the same section number always receives the same color across the ecosystem;
- color must never be the only way to distinguish a section;
- heading text and numbers must remain legible with adequate contrast;
- the document must remain understandable in monochrome, printed form, plain-text views, and for readers who cannot distinguish the colors;
- use a consistent implementation supported by the target platform. GitHub Markdown does not provide arbitrary text-color styling, so do not pretend raw HTML or decorative emoji is a reliable universal color system.

Until a palette and rendering method are approved, use the stable numbers and headings. Color styling is a later presentation layer, not a reason to delay consistent structure.

## 6. References: remove bibliography ambiguity

Do not add a bare bibliography whose entries force readers to infer what each source is or why it matters. Prefer a **Reference Guide** or **Further Reading** list where every entry includes:

- **What it is:** official specification, API reference, tutorial, research paper, design note, example, benchmark, or other source type;
- **Publisher / author:** use the source's own attribution when available;
- **Why it is here:** the specific question it helps answer;
- **Relationship to this package:** required, recommended, background, historical, or comparative;
- **Link and version/date:** where applicable, so readers can identify the exact source.

Prefer authoritative, publisher-maintained references for specifications and APIs. The package author is responsible for selecting and classifying references that are relevant to the package. If a formal citation style is required for academic work, follow the institution or publisher's specified style rather than inventing a local one.

## 7. Describe ecosystem relationships without creating coupling

The Workshop shares documentation conventions, not a requirement that every package know about every other package.

- Describe each repository's own **what**, **why**, and **how**.
- Explain a neighboring package only to clarify a real boundary, supported integration, or next step.
- Label optional integrations as optional; do not present them as required dependencies.
- Link to the neighboring package's own documentation for its detailed behavior rather than copying its whole explanation.
- Do not introduce a package reference, runtime dependency, or shared contract merely to make documentation look consistent.
- Keep version numbers, compatibility claims, installation commands, and release status tied to evidence in the repository or verified package metadata.

## 8. Visual and editorial rules

- Lead with meaning, then detail. A diagram should clarify a real relationship or flow, not decorate an otherwise unclear explanation.
- Prefer small, labeled diagrams and examples that can be understood without specialist knowledge.
- Explain every diagram's key takeaway in prose; never make the image the only carrier of essential information.
- Use consistent terminology within a repository and explain intentional differences between repositories.
- Prefer direct language and concrete verbs. Expand acronyms on first use.
- Separate **current behavior**, **verified capability**, **planned work**, and **aspiration**.
- Do not claim performance, memory, energy, security, or compatibility benefits without an appropriate measurement or source.
- Use the shared Workshop resources/support footer consistently, while allowing package-specific support and links.

## 9. Review checklist

Before considering a README aligned, verify:

- [ ] Sections appear in the shared order, beginning **01 What**, **02 Why**, **03 See It Work in 60 Seconds**.
- [ ] The 60-second path is runnable, repeatable, and honest about prerequisites and verification.
- [ ] A reader without prior coding or C# knowledge has a clear starting path.
- [ ] A developer can find installation, a minimal example, and supported usage quickly.
- [ ] Advanced readers can find design rationale and deeper references without overwhelming the introduction.
- [ ] Responsibilities, non-goals, and dependency direction are explicit.
- [ ] References are classified and annotated rather than left as ambiguous bare citations.
- [ ] Links, package versions, commands, and release claims have been checked.
- [ ] Color is not required to understand the section numbering.
- [ ] The README explains this package without duplicating the neighboring packages' documentation.

## 10. Rollout policy

Adopt this document as the shared starting point for README standardization. Apply it incrementally:

1. Agree on this semantic section order and reader paths.
2. Inventory existing README work and preserve valuable package-specific explanations.
3. Align content and verify quick demonstrations before applying visual styling.
4. Propose a shared color palette and platform-compatible rendering method separately for review.
5. Review branches for divergence before merging; do not discard useful changes simply to make files look alike.

This standard does not authorize package publication, version changes, or merges. It is a documentation convention only.
