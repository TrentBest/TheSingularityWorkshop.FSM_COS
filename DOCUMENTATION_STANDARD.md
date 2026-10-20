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

## 5A. Per-source-file and per-type documentation contract

For maintained C# source, keep the source file, its focused explanation, and its relationship/use-case view distinct but linked. This is the default for production code; generated code, trivial declarations, test fixtures, and files whose contents are better documented as one cohesive unit may use a documented exception rather than producing empty boilerplate.

For a maintained source file such as `RuntimeManifest.cs`, the companion set is:

- **`RuntimeManifest.cs`** — authoritative implementation.
- **`RuntimeManifest.md`** — focused type/file explanation. Begin with the repository's moniker/identity visual, explain the purpose and responsibility boundary in plain language, include the source-grounded UML class card for the focus type, then explain the class's key members/contracts, inputs and outputs, invariants, failure behavior, and links to relevant guides/tests. The class card belongs here because it is the fast, immediately useful view of the type.
- **`RuntimeManifestUML.md`** — the wider UML relationship view for that source/type. Precede `UML.md` with the source/type stem so the companion is discoverable next to its source and primary explanation. This page contains the class diagram and a **Use cases** section; it should not duplicate the primary page's full member-by-member explanation.

Use the actual source stem and repository naming conventions consistently. When one source file declares multiple maintained types, create an explicit page/card for each maintained type or document a deliberate grouped-page exception. Never silently choose the first declaration and imply the rest are covered. A generated inventory should make omissions visible.

### Class diagram layout and relationship semantics

Center the **focus class** as the visual anchor. Place classes that use or depend on the focus above it, with their relationship connectors directed down toward the focus where that direction accurately expresses the modeled dependency. Place classes the focus uses or depends on below it, with connectors directed from the focus toward those dependencies. Keep inheritance, interface realization, association, ownership/composition, and dependency visually distinct and label multiplicity or navigability only when the source/model supports it.

This is a reading convention, not permission to falsify a relationship to force a layout. If a type participates in several relationship kinds or cycles, preserve the real semantics, label the edge, and adjust placement as needed for legibility. Avoid connectors that cross class compartments or appear to attach to the wrong type. Every edge must come from an explicit source/model fact or be clearly marked as a human-authored conceptual relationship.

### Use cases: intentional human input

The Use cases section records the goals actors have when using the system/type and the observable outcomes they need. Reflection, syntax trees, compiler symbols, and an AST can expose declared structure and some relationships; they cannot reliably infer user intent, actor goals, value, or whether a scenario is a supported use case. These require maintainer/user direction, with the LLM helping elicit, organize, challenge, and document that input—not inventing it as fact.

For each relevant use case, record:
- **Actor / stakeholder** and the goal they want to accomplish.
- **Trigger and preconditions**, including required host/context where relevant.
- **Main success path** and the observable result.
- **Important alternatives and failure paths**, including rejection, cancellation, or non-convergence where applicable.
- **Evidence/status**: implemented and tested, implemented but unverified, proposed, or explicitly unsupported; link to the source, tests, scenario, and any sequence/activity/state diagram that actually supports the claim.

Use a UML use-case diagram when it clarifies actor-to-goal relationships across a meaningful system boundary. Keep the written use-case scenarios authoritative enough to review; a diagram alone cannot capture preconditions, outcomes, or evidence. Do not create a use-case diagram for every class by rote. Some low-level types have no meaningful actor-facing use case of their own; link to the containing subsystem's use cases and explain the type's contribution instead.

### Source-derived model and generation boundary

Maintain a source-derived inventory/model of the domain where practical, rather than treating each SVG as a hand-maintained island. The model may be built from reflection over loaded assemblies, compiler/Roslyn analysis, or another repository-appropriate source of truth; document the chosen input and its limits. A prior Unity workflow used reflection to derive an AST across the domain, which is a useful precedent, not a mandate to assume reflection alone can recover source semantics or user intent.

Keep three concerns separate:
1. **Structural facts** — declarations, signatures, references, and compiler-resolved relationships that tooling can derive and verify.
2. **Human-authored intent** — use cases, actor goals, design rationale, and conceptual relationships that require maintainer/user direction.
3. **Presentation** — class cards, relationship diagrams, use-case diagrams, and SVG/Markdown output.

Prefer deterministic generation from a maintained inventory/model, with CI detecting stale or missing outputs. Every generated artifact must identify or link to the source/model inputs and generation contract. If a diagram is manually curated, label it honestly; if generated output omits a relationship or type, state the limitation instead of implying complete coverage. Do not make GUI.SVG parse source code by assumption: source analysis, semantic modeling, renderer-neutral diagram data, and SVG rendering are separate responsibilities with an explicit supported API boundary.

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

## 8A. Living visuals and visual coverage

Use a living visual wherever it materially improves comprehension: architecture and ownership maps, request/response flows, dependency ordering, state transitions, arbitration/convergence, failure paths, and real examples of user-visible behavior. Prefer an accurate working GUI or implementation-backed demonstration when it adds real evidence; use animated SVG when semantic motion teaches something; use a clear static SVG when motion adds no value. Keep text diagrams and tables where they are the clearest form.

A visual is not “living” merely because it is colorful or animated. It should express the real concept, behavior, or relationship. Animate the concept, not the decoration. Preserve a static and accessible explanation; never make motion essential to understanding. Add descriptive alt text and a caption when the visual's purpose would otherwise be unclear. Check ordinary GitHub width, mobile layout, light/dark contrast, and reduced-motion implications.

Maintain a repository-specific visual coverage checklist. Every Markdown document must be considered and classified as one of:
- a useful visual is embedded and source-verified;
- a useful visual is still needed and tracked;
- no visual is needed, with a brief reason.

An SVG's existence in an assets folder is not evidence that the relevant document uses it. Reuse canonical assets rather than cloning near-identical diagrams, and validate local links. Every visual claim must match source, tests, and release status; distinguish implemented behavior from design intent. The checklist is complete only after every document has been considered and outstanding visual gaps are either closed or explicitly justified.

For FSM_COS, the working checklist is [FSM_COS maintainer-only Living Documentation Visual Audit](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/docs/LIVING_DOCUMENTATION_AUDIT.md). Update it alongside the documentation changes it tracks.

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


## Gallery standard — one index per repository

Every repository should have a concise `docs/GALLERY.md`, linked from its README. The Gallery is a visual index, not another giant manual.

Each entry should contain:

1. **A visual or proof object.** Prefer an SVG, screenshot, generated report, or direct artifact link that helps a reader understand this package. If no truthful visual exists yet, describe the proposed proof explicitly instead of inventing an implementation.
2. **A brief explanation.** What is the reader seeing, and why does this matter?
3. **An evidence boundary.** Say whether the item is implemented output, a test-derived trace, an explanatory architecture illustration, a browser-only experiment, or a proposed future proof.
4. **One focused deep-dive link.** Each entry links to its own document/recipe that explains how to reproduce it, what code produces it, what to inspect, and what it proves. The Gallery stays short; the deep dive owns the detail.

### Pick a visual form that matches the package

- **Visual/rendering packages:** generated SVGs, geometry/lighting studies, frames, scene outputs, and clearly labeled backend captures.
- **Stateful/functional packages:** state-transition diagrams, input/output traces, before/after snapshots, contract tables, and test-generated timelines. A functionality package does not need to pretend it is a GUI.
- **Composition/runtime packages:** responsibility maps, dependency direction, lifecycle diagrams, invalidation paths, and a real composed fixture.
- **Web experiences:** screenshots or captures from the actual running Experience, with links into the architecture and simulation documents. Concept art must be labeled as concept art.
- **Protocol/serialization packages:** example payloads, identity maps, round-trip traces, and compatibility matrices derived from executable fixtures.

### Living evidence rules

- Prefer a visual driven by the same source data, test fixture, or runtime output that the deep-dive describes.
- Do not call an SVG animation “live” unless a real runtime input/control loop drives the state being animated.
- Keep CPU-side projections, SVG manifestation, GPU execution, and hardware verification distinct.
- For performance claims, include reproducible measurements and the workload/environment; visual complexity alone is not a benchmark.
- Keep the Gallery skimmable. Move full recipes, API detail, and theory into linked focused documents.
- Link `docs/GALLERY.md` from the README so the visual index is discoverable in every repository.

The Gallery is the Workshop's common answer to “show me”: each package chooses the visual form appropriate to its responsibility, and the linked recipe teaches the reader how to reproduce and verify it.

## Branch policy — the Sith policy

**There are exactly two long-lived branches in every Workshop repository: `master` and `development`. No third branch is a permanent home.** This is the Workshop's Sith policy: two branches, no branch sprawl.

- **`master` is the release line.** It contains the reviewed, release-ready state. Published packages and release artifacts must be built from the intended `master` state, and consumer-facing release claims must match what is actually published.
- **`development` is the integration line.** Ongoing changes converge here through pull requests and required verification. It may contain unreleased work and must not be represented as the published package state.
- **Feature, fix, experiment, and automation branches are temporary work branches, not additional long-lived branches.** Use them only to isolate a specific change and submit it for review. After the work is merged, abandoned, or superseded, delete the temporary branch. Do not retain permanent feature, staging, release-candidate, backup, or agent-specific branches.
- **Do not bypass review by pushing ordinary work directly to `master`.** Use a pull request to bring changes into the appropriate integration/release path, with CI and review evidence appropriate to the change. Do not merge or publish merely because a workflow started; verify its result and the intended commit.
- **Keep development dependencies and release dependencies distinct.** Development CI should consume the dependent repository's `development` build/artifact when testing unreleased integration. Release/publish CI should consume the intended published package or release artifact corresponding to `master`, not silently substitute a development build.
- **A green workflow is not permission to publish.** Publishing must be explicitly enabled by the repository's documented release control and must target the intended release commit. Documentation-only work must not accidentally publish a package.
- **Do not make a repository policy exception by habit.** If a repository genuinely cannot follow this model, document the concrete technical reason, scope, owner, and exit condition in a reviewed decision record. Convenience, an old branch left behind, or a tool's default behavior is not sufficient justification.

### Branch-policy review checklist

- [ ] The only long-lived branches are `master` and `development`.
- [ ] Any other branch is tied to a specific, active change and is intended to be deleted.
- [ ] Pull requests target the intended integration branch and are reviewed before merge.
- [ ] Development CI resolves dependent source/artifacts from `development`; release CI uses the intended `master`/published dependency contract.
- [ ] Release workflows are gated and the publish step's outcome is checked separately from build/test success.
- [ ] Documentation, badges, and release notes distinguish current development state from the published release.
