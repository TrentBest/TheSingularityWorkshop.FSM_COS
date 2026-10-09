# Documentation Standard

This document defines the shared documentation standard for The Singularity Workshop repositories. It is a quality standard, not a demand that every repository use identical prose or contain every possible document.

The goal is to **edify, not mystify**: explain the human problem and conceptual model, state the technical boundary precisely, show how to use the software, and provide evidence for claims about implementation and behavior.

## 1. README sequence and shared section identifiers

Unless the repository's purpose makes a section irrelevant, use the shared semantic identifiers below. The identifier belongs to the topic, not merely to its current position: preserve it across repositories so readers learn the visual language once. Omit irrelevant sections rather than renumbering later sections.

| ID | Canonical section | Marker color | Typical content |
|---|---|---|---|
| **00** | Identity and badges | Graphite `#475569` | Repository/package name, valid license/build/coverage/release badges |
| **01** | Definition | Cobalt `#2563EB` | One-sentence concrete definition |
| **02** | Visual identity | Teal `#0F766E` | Project image or architecture visual with useful alt text |
| **03** | Plain-language explanation | Blue `#1D4ED8` | Problem, motivation, central idea before implementation detail |
| **04** | Audience / choose your path | Orange `#EA580C` | Reader paths for newcomers, users, developers, architects, evaluators |
| **05** | At a glance | Violet `#7C3AED` | Package ID, status/version, frameworks, license, install and CI links |
| **06** | Responsibility boundary | Emerald `#047857` | What the project owns, excludes, and hands off |
| **07** | Architecture and ecosystem | Amber `#B45309` | Ownership, dependency direction, architecture diagrams |
| **08** | Quick start | Green `#15803D` | Shortest verified path to a meaningful result |
| **09** | Core concepts / how it works | Indigo `#4338CA` | Model, lifecycle, invariants, theory and contracts |
| **10** | Usage and examples | Cyan `#0E7490` | Realistic source/package-version-specific examples |
| **11** | Verification and development | Slate blue `#475569` | Build, tests, coverage, contribution guidance |
| **12** | Documentation map / further reading | Purple `#6D28D9` | Reader-oriented routes to authoritative deep dives |
| **13** | Related projects and Workshop footer | Warm gold `#A16207` | Relevant neighbors and consistent Workshop identity |

### How to apply the color system on GitHub

GitHub Markdown does not provide a dependable repository-wide stylesheet for arbitrary heading colors. Use the **number plus a small colored marker** as the portable visual anchor (for example, a small SVG marker checked into a shared documentation-assets location, or a consistent colored square emoji where assets are not practical). Keep the heading text, numeric ID, marker color, and meaning aligned. Do not rely on custom HTML/CSS rendering that may be sanitized or display differently across clients. The color supplements the number; it must never be the only way to distinguish a section.

Use headings such as `## 03 🔵 Plain-language explanation` and `## 04 🟠 Audience / choose your path` when shared SVG markers are not available. For deep-dive documents, use the same ID/color only when the section belongs to that canonical topic; otherwise use unnumbered local headings rather than assigning a misleading global ID.

This order is a default, not a checklist to pad every README. Deep theory, API detail, tutorials, and decision records belong in focused documents linked from the README.

## 1A. The reader journey: promise, proof, understanding, depth

The README is the public front door, not a manual that happens to be placed first in the repository. Its order must respect how a new reader learns: first identify the thing, then understand why it matters, then see evidence, then decide how deeply to explore.

Use this **reading order** when the topics apply:

1. **What is it?** Definition and clear identity.
2. **Why should I care?** The human or engineering problem, the cost/friction, and the outcome this package is designed to improve.
3. **Show me in about a minute.** The shortest meaningful, verified example or a direct route to a real demonstration. State prerequisites and show the expected result.
4. **What do I get?** A concise view of capabilities and the audience/use cases that benefit.
5. **How does it fit?** Responsibility boundary, neighboring packages, and a compact architecture visual.
6. **How does it work?** The mental model, normal lifecycle, important contracts, and core invariants.
7. **What else must I know?** Configuration, limitations, compatibility, measured performance, troubleshooting, development, and further reading.

The shared section IDs in the table below identify **topics**, not a mandatory numeric reading order. Keep an ID/color attached to its canonical topic across repositories, but place the quick-start topic near the top even though its canonical identifier is **08**. The reader should not have to pass through every architecture section before reaching the first proof.

This is progressive disclosure, not a demand to make documentation shallow. The first screen earns attention; the first example earns confidence; the deeper guides earn understanding. A skimming reader should leave with the right idea, a practical reader should find the first success quickly, and a motivated reader should have a clear route into the complete theory and contract.

### The first-minute proof

The quick start belongs immediately after the definition and reason to care, not after a long tour of architecture. A meaningful first proof must:

- demonstrate the package's real purpose rather than a generic language feature;
- match the current source API or clearly name the published package version it targets;
- include the minimum necessary setup and prerequisites;
- show code or a real demo path plus its expected output/behavior;
- explain the example in plain language;
- link to the complete usage guide for setup details and realistic scenarios.

A snippet is not a first proof merely because it is short. Verify the code against source, build/run it when feasible, and label examples as verified, source-checked, or conceptual. Never imply a class or helper is supplied by the package if it is only illustrative scaffolding.

Some composition kernels and infrastructure packages cannot show a meaningful result without a host or domain contract. In that case, give the shortest honest proof available: a tested composition fixture, a minimal in-memory example with all required contracts identified, or a link to a working consumer demonstration that names exactly what it proves. Do not fake a standalone capability to satisfy the template.

### The README is a map; the deep dives teach

Keep the README focused on orientation, value, first proof, boundaries, and navigation. Link to focused documentation for:

- **Usage:** installation, setup, common tasks, complete runnable examples, and troubleshooting.
- **Theory:** the problem behind the abstraction, mental models, assumptions, trade-offs, and adjacent concepts.
- **Architecture:** ownership, dependencies, lifecycle, handoffs, and invariants.
- **API/contracts:** exact signatures, inputs, outputs, validation, and guarantees.
- **Performance and verification:** reproducible measurements, test evidence, environment, and known limits.
- **Non-coder learning path:** concepts explained from observable problems, with examples and exercises that teach the reader how to reason about software.
- **Integration:** how this package is consumed by a host or neighbor without duplicating that neighbor's authoritative domain documentation.

Give each link a purpose statement: not just “Theory,” but what the reader will understand after opening it. Maintain one authoritative explanation per topic instead of copying the same manual into multiple repositories.

### Teaching software concepts to non-coders

A non-coder path should teach a useful mental model, not merely replace technical words with friendlier synonyms. Build from concrete experience toward abstraction:

1. Show a recognizable problem or behavior.
2. Use a familiar analogy and clearly state where it stops matching reality.
3. Name and define the software concept in plain language.
4. Explain what design decision the concept enables or protects.
5. Show the relationship in a diagram or small example.
6. Offer a deeper explanation, a practical exercise, or both.

Explain why a boundary exists, what it costs, what it makes easier, what it cannot guarantee, and when it may not be appropriate. The goal is to help readers think alongside the software—not to require them to arrive already knowing its vocabulary.

### Visual and literary craft

Visual hierarchy and prose are part of the teaching system. Use contrast to establish priority, repetition to create recognition, proximity to show relationships, and concrete metaphors to make abstract systems memorable. Apply these techniques in service of understanding, never as camouflage for missing evidence.

- Put the most important distinction first and use concise paragraphs with informative headings.
- Use architecture diagrams for responsibility and relationships; sequence diagrams for flow; screenshots or animation for actual product behavior; tables for compact comparisons.
- Use a small semantic color palette consistently. Color should signal meaning (such as definition, procedure, caution, or deeper theory), not decorate unrelated sections.
- Never rely on color alone. Pair color with text labels, shapes, line styles, or icons; check contrast, mobile readability, and dark/light rendering.
- Keep badge use restrained and factual. Avoid badge walls, emoji walls, and ornate separators that compete with the package's promise.
- Give meaningful visuals descriptive alt text and captions explaining what the visual proves.
- Keep SVGs legible at ordinary GitHub width and avoid imagery that implies behavior the software does not implement.

The Workshop should be compelling because its value is understandable and its claims are provable. Do not substitute hype, pressure, or unsupported superlatives for evidence. State poor-fit scenarios, prerequisites, limitations, maturity, and performance context honestly.

## 2. Required quality questions

Every README should answer, in language appropriate to its audience:

- What is this, in one sentence?
- What problem or missing boundary motivates it?
- Why does this project exist separately from adjacent projects?
- What does it own, and what does it explicitly not own?
- How does a user get started?
- Where can a reader learn the underlying theory or inspect the contracts?
- How can a developer build and verify it?
- What is implemented now, what is transitional, and what is only a design target?

If a question is irrelevant, omit it intentionally rather than inventing content.

## 3. Documentation layers

Use a small, deliberate documentation set rather than one enormous README.

| Document | Responsibility |
|---|---|
| `README.md` | Project introduction, orientation, quick start, and navigation |
| `docs/WHAT_IS_*.md` or equivalent | Accessible conceptual explanation for new readers |
| `docs/THEORY.md` | Rationale, mental model, invariants, and why the architecture is shaped this way |
| `docs/ARCHITECTURE.md` | Components, boundaries, dependency direction, and ownership |
| Contract/API guides | Exact interfaces, inputs, outputs, validation, and behavioral guarantees |
| Getting-started/tutorial guides | Reproducible step-by-step use |
| Development/verification guide | Build, test, coverage, workflows, and contribution process |
| Decision/status documents | Explicit decisions, unresolved questions, transitional implementation, and evidence |
| `DOCUMENTATION_INDEX.md` (or a README documentation map) | A reader-oriented map of the document set |

Not every repository needs every file. Prefer one authoritative explanation per topic and link to it from other places.

## 4. Accuracy and evidence

- **Source is authoritative for implementation claims.** Verify examples, signatures, target frameworks, dependencies, and behavior against the current branch.
- Separate **implemented behavior**, **current development state**, **intended architecture**, and **future work**. Label them clearly.
- Version-specific instructions must say whether they apply to current source or a published package.
- Do not invent release availability, performance benefits, compatibility, guarantees, test results, or security properties.
- Use test names, reproducible commands, CI, benchmarks, and concrete examples as evidence where relevant.
- A green badge is a live pointer, not a substitute for describing what is tested.
- Treat dependency direction as an architectural fact. Do not imply a package depends on a neighbor unless its project/package metadata confirms it.
- Do not copy a dependency's full documentation into the consumer README. Explain the consumer's reason for depending on it, then link to its authority.

## 5. Architecture visuals

Visuals should clarify the idea, not merely decorate the page.

- Prefer version-controlled SVGs for architecture diagrams where practical.
- Keep diagrams legible in GitHub and in a narrow viewport; use meaningful labels and direction.
- Provide descriptive alternative text and a short caption that states what the diagram demonstrates.
- Keep diagrams consistent with current code and written boundaries.
- Link to a focused diagram or deeper explanation rather than repeating a large image many times.
- Do not introduce an image dependency that can silently break package/repository documentation when a local asset would be appropriate.

## 6. Examples must be runnable or honestly illustrative

Every code example should be one of:
- **Verified example** — built/run against the named version or commit.
- **Source-shaped example** — checked against current source but not independently executed.
- **Conceptual pseudocode** — explicitly labeled and not presented as compilable code.

Include prerequisites, required packages/references, commands, and expected outcome when useful. Avoid examples that rely on undeclared types or omit important setup.

## 7. Links and navigation

- Check relative paths and external links when changing documentation.
- Use descriptive link text that tells readers where they will go.
- Provide both a short path for quick orientation and a deeper path for people who want the theory.
- Keep related-project lists relevant to the repository's role.
- Avoid duplicate tables of contents or multiple competing documents claiming to be the canonical guide.

## 8. Shared visual and editorial identity

Use consistent Workshop identity, badges, terminology, diagrams, and closing footer where appropriate, while preserving each repository's own domain voice.

Prefer:
- direct statements over hype;
- concrete examples before unexplained jargon;
- explicit boundaries over vague claims of flexibility;
- short sections with meaningful headings;
- tables for comparisons and responsibility boundaries;
- diagrams for flows and relationships;
- “what / why / how / evidence” over feature lists without context.

Do not force a package to use terms, dependencies, audiences, or architectural layers that do not belong to it.

## 9. Review checklist

Before considering a documentation pass complete:

- [ ] The opening sentence identifies the project accurately.
- [ ] The README sections follow the shared sequence where applicable.
- [ ] The intended audience can find an appropriate starting point.
- [ ] Project/package identifiers, versions, frameworks, badges, and license are accurate.
- [ ] Ownership and non-goals are explicit.
- [ ] Dependency direction and neighboring responsibilities are correct.
- [ ] Diagrams and alt text match the current architecture.
- [ ] Code and commands match the stated source/package version.
- [ ] Relative links and referenced files exist on the target branch.
- [ ] Implemented behavior is distinguished from design goals and future work.
- [ ] Build/test instructions and evidence are reproducible.
- [ ] The README links to the deeper theory and documentation map.
- [ ] No NuGet publication, release, or other operational action is implied by a documentation-only change.

The standard should raise consistency and truthfulness across the ecosystem without making every repository read like the same package.
