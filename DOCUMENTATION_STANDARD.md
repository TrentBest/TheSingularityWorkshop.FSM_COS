# Documentation Standard

This document defines the shared documentation standard for The Singularity Workshop repositories. It is a quality standard, not a demand that every repository use identical prose or contain every possible document.

The goal is to **edify, not mystify**: explain the human problem and conceptual model, state the technical boundary precisely, show how to use the software, and provide evidence for claims about implementation and behavior.

## 1. README sequence and shared section identifiers

Unless the repository's purpose makes a section irrelevant, use the shared semantic identifiers below. The identifier belongs to the topic, not merely to its current position: preserve it across repositories so readers learn the visual language once. Omit irrelevant sections rather than renumbering later sections.

| ID | Opening section | Marker shape and color | Reader purpose |
|---|---|---|---|
| **00** | Title and identity | **Bright, high-contrast asterisk emoji ✳️** | Make the project identity immediately recognizable; keep valid badges close by |
| **01** | What | Blue square 🟦 | Define the thing plainly and name what it does |
| **02** | Why | Violet marker 🟣 | Explain the problem it solves and why this project exists |
| **03** | How | Teal marker 🩵 | Explain the working model and the boundaries that matter |
| **04** | See it in a minute | Bright green circle 🟢 | Give the simplest honest, verified first use a reader can try, with expected behavior |
| **05** | Available documentation and theory | Bright purple marker 🟪 | Route readers to short, focused guides and deeper explanations |

These six opening sections are a **reader journey**, not an exhaustive table of contents. They should appear in this order when applicable. Place a money-shot visual directly below the title/badges and before section 01. The image must explain or demonstrate the actual package; it must not be generic decoration.

After section 05, continue with concise, useful technical reference sections as needed. Do not keep assigning global numeric IDs to every deep technical heading: the first six IDs describe the common front-door sequence. Use clear local headings for additional detail, and preserve each repository's domain voice.

### Shape, color, and ordering

Put the marker **before** its number and heading text (for example, 🟦 01 What). For section 00, use the high-contrast asterisk emoji ✳️ before 00; do not use the low-contrast gray circle. A marker must remain recognizable in GitHub light and dark themes and must not be the only carrier of meaning—the number and words remain visible.

When a marker shape has an outline, border, or underline, that stroke must use the **same semantic color as the shape**. Make it unmistakable: use a substantially thicker stroke than a hairline (normally at least 4 SVG units at a 20-unit icon scale, proportionally thicker for larger shapes). Do not use a gray or neutral outline around a colored shape unless the neutral outline has a separate, explicitly documented meaning. Keep sufficient contrast against the page background.

The marker palette is intentionally vivid. Color creates quick recognition, while the repeated shape/position creates a stable visual rhythm. Use a small consistent palette rather than choosing a new decorative color for every heading. Do not rely on custom HTML/CSS for heading colors; GitHub Markdown does not provide dependable arbitrary heading-color styling. Check mobile and dark-mode readability, and never rely on color alone.

### How to apply the color system on GitHub

GitHub Markdown does not provide a dependable repository-wide stylesheet for arbitrary heading colors. Use the **number plus a small colored marker** as the portable visual anchor (for example, a small SVG marker checked into a shared documentation-assets location, or a consistent colored square emoji where assets are not practical). Keep the heading text, numeric ID, marker color, and meaning aligned. Do not rely on custom HTML/CSS rendering that may be sanitized or display differently across clients. The color supplements the number; it must never be the only way to distinguish a section.

Use headings such as `## 🟦 01 What`, `## 🟣 02 Why`, `## 🩵 03 How`, `## 🟢 04 See it in a minute`, and `## 🟪 05 Available documentation and theory` when shared SVG markers are not available. For deep-dive documents, use the same ID/color only when the section belongs to that canonical topic; otherwise use unnumbered local headings rather than assigning a misleading global ID.

This order is a default, not a checklist to pad every README. Deep theory, API detail, tutorials, and decision records belong in focused documents linked from the README.

## 1A. The reader journey: problem, context, depth, proof, next steps

The README is the public front door, not a manual compressed into one page. Its opening should follow the reader journey defined by sections 00–05 above:

1. **00 — Identity:** make the project and package recognizable.
2. **Money-shot visual:** show the real package concept or behavior before asking the reader to absorb detail.
3. **01 — What:** define the project plainly and state what it does.
4. **02 — Why:** name the problem and explain why this project is a distinct solution.
5. **03 — How:** give the reader the mental model and the essential responsibility boundaries.
6. **04 — See it in a minute:** provide the shortest real, verified use a reader can try, plus the expected result.
7. **05 — Available documentation and theory:** link to focused guides, with a brief explanation of what each teaches.

This sequence is intentional: it answers *why should I care?* before asking the reader to study the architecture, and it provides the document map before the README grows into the complete technical account. Supporting reference sections may follow. Avoid duplicating the same explanation just to satisfy a template.

A README should not overwhelm newcomers, but it must not hide the proof behind a long tutorial. A skimming reader should understand the promise; a practical reader should find a credible first success; a motivated reader should have a clear route into the complete theory and contracts.

### The first-minute proof

The quick start belongs in section 04, immediately after What, Why, and How. It must be the simplest use a reader can actually try—not a test-helper excerpt presented as though it were consumer code. A meaningful first proof must:

- demonstrate the package's real purpose rather than a generic language feature;
- match the current source API or clearly name the published package version it targets;
- include only the minimum setup and prerequisites;
- show a copyable example or a direct command/demo path plus its expected output or behavior;
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

## 1B. Explain the package's place in the ecosystem

A package README should explain not only how to use the package, but how it relates to the author's wider ecosystem—especially when the package is intended to be independently reusable.

Describe the following where relevant:

- **Standalone value:** what a reader can use without adopting other ecosystem projects.
- **Optional alignment:** which contracts or conventions make integration with neighboring projects easier, without implying that outside creators must adopt them.
- **Responsibility and dependency direction:** what this package owns, what it consumes, what consumes it, and which upward dependencies are prohibited.
- **Package versus runtime capability:** distinguish a NuGet library/contract from a MicroBundle or other artifact that can be discovered and composed at runtime. Do not assume every package should become a bundle.
- **Host relationship:** explain how an application or host uses the package and which presentation, execution, or platform responsibilities remain with that host.
- **Current versus intended integration:** name working integrations separately from proposed paths. If cross-host use (for example, through AnyApp) still needs adaptation or proof, say so explicitly.
- **Creator freedom:** welcome compatible alternatives and independent implementations. The goal is to make useful parts easier to combine, not to create lock-in or require adoption of the whole ecosystem.
- **Creator opportunity, when relevant:** it is appropriate to briefly signal the long-term possibility that compatible capabilities may become easier to discover, distribute, and monetize through the ecosystem. Keep this secondary to the technical value, frame it as intent rather than an existing feature, and never promise a marketplace, income, revenue share, or audience unless those mechanisms are real and documented.

A useful message is: *build with the parts that help you; compatible Workshop contracts can make the pieces fit together more naturally, while you remain free to build the rest yourself.* State this as design intent, then substantiate each concrete compatibility claim with contracts, tests, or a working host integration.

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
