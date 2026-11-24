<!-- The Singularity Workshop moniker -->

# Documentation Standard

This is the shared documentation standard for The Singularity Workshop repositories. The goal is to **edify, not mystify**: explain the human problem, why the design exists, the actual technical boundary, how to try the technology, and how to verify claims.

## Reader journey and teaching

Unless irrelevant to a repository, README sections follow this order: **00 Identity**, a real “money-shot” visual, **01 What**, **02 Why**, **03 How**, **04 See it in a minute**, and **05 Available documentation and theory**. Use the Workshop's consistent section markers where applicable (✳️, 🟦, 🟣, 🩵, 🟢, 🟪); markers supplement visible headings and are never the only carrier of meaning. Omit irrelevant sections rather than padding the README.

Teach from experience toward abstraction: show a recognizable problem; use a familiar analogy and state where it stops matching reality; define the concept plainly; explain why the design decision matters; show a diagram/example; offer a deeper explanation or exercise. For every major topic answer: **What is it? Why does it exist? How does it work? How can a reader verify it? What next?** Explain what a boundary costs, enables, and cannot guarantee. The goal is to help people understand and create software without requiring a computer-science degree.

Keep a focused document set: README for orientation and proof; learning/getting-started guide; theory and architecture; exact API/contracts; runnable tutorials; Gallery.md with a two-sentence overview and links to detailed recipes; per-type documentation; companion per-type UML/use-case pages; and verification/status docs. Prefer one authoritative explanation per topic. Gallery visuals should teach through real output, identify how to reproduce it, and state what they do and do not prove.

### Canonical five-part document shape

The README's five-part journey is a reusable information shape, not a README-only trick. Apply it to major public guides and package documentation so readers learn where to find each kind of answer:

1. **01 — What:** define the subject, scope, vocabulary, and contract in plain language.
2. **02 — Why:** explain the problem, motivation, trade-offs, and when the subject is or is not useful.
3. **03 — How:** describe the real process, API, ownership, and steps needed to use or change it.
4. **04 — See it in a minute:** provide the smallest meaningful example, diagram, output, test, or reproducible verification path.
5. **05 — Choose your next step:** link to the single authoritative deeper guide, exact reference, related task, or maintainer procedure.

Keep the five roles and their order stable; tailor the visible heading wording to the document's subject. Do not pad a document merely to fill a section: say briefly when a role is not applicable and route the reader to the canonical source. A contract reference may put its precise normative rules inside **How**; a tutorial may put prerequisites and expected output in **See it**; a Gallery uses the same roles to orient, explain the purpose of the exhibits, show how to reproduce them, and route to recipes. Avoid creating multiple authoritative homes for the same fact.

This five-part shape applies to substantive public guides, not blindly to every artifact. Generated UML/type cards, machine-readable reports, release evidence checklists, changelogs, security reporting instructions, and internal audit ledgers keep the format appropriate to their job, while still stating scope, evidence, limitations, and next action where relevant.


## Textbook-style theory chapters

Every repository should teach its own domain as a small, college-textbook-quality learning path—not merely provide an architecture overview, API inventory, or design rationale. The aim is to make the technology understandable to a curious newcomer, useful to an experienced engineer, and adaptable by an instructor who wants to reorder, replace, annotate, or extend individual chapters.

The repository's theory is **domain-specific**. Explain the problem this technology owns and the concepts needed to reason about it. Link to authoritative neighboring repositories for concepts they own rather than copying their manuals or claiming their theory. For example, FSM_API owns general finite-state-machine theory; a consuming package explains how it uses that theory at its own boundary.

### Chapter teaching sequence

Build a progressive path from familiar experience to precise, source-grounded understanding. A chapter may combine or omit a part when the subject genuinely does not need it, but should not skip evidence or limitations just to make the story persuasive.

1. **Intuition and motivating problem.** Begin with a recognizable real-world or programming problem before introducing specialist vocabulary. Explain the cost of the problem and why the reader should care.
2. **Concepts and vocabulary.** Define terms plainly, give a concrete analogy, and state where the analogy stops being accurate. Separate related ideas that are easy to confuse.
3. **Theory and reasoning.** Explain the governing model, assumptions, invariants, trade-offs, alternatives, and conditions under which the idea is useful or a poor fit. Show the reasoning, not only the conclusion.
4. **Model or living visual.** Provide a diagram, worked model, animation, interactive example, or data flow that helps the reader reason about the concept. Include a caption and accessible text equivalent. Label conceptual illustrations honestly; visuals are not runtime proof merely because they animate.
5. **Implementation mapping.** Trace each important concept to real public types, members, source files, tests, and lifecycle stages. Explain why the implementation is shaped this way, what it guarantees, and which responsibilities remain elsewhere. Link rather than invent symbols or duplicate full API references.
6. **Worked example.** Walk through a small end-to-end case with inputs, intermediate steps, outputs, and important failure or alternative paths. Provide exact prerequisites and reproducible commands when executable.
7. **Evidence and limits.** State what was source-checked, built, tested, benchmarked, or observed, against which revision and scope. Distinguish current implementation, intended architecture, hypothesis, and future work. Record counterexamples, edge cases, unverified assumptions, and guarantees the system does not make.
8. **Practice and assessment.** Include useful review questions, a small exercise, or an investigation prompt. Where appropriate, ask the reader to predict an outcome before revealing it, compare alternatives, or find a counterexample.
9. **Further reading.** Link to the exact contracts, runnable recipes, related chapters, tests, source, and authoritative theory owned by neighboring repositories.
10. **Instructor/editor notes where useful.** Keep chapters modular and independently linkable. Put repository-specific extension points, suggested experiments, discussion prompts, and visual/source assets where an instructor can adapt them without rewriting the entire book.

Not every chapter needs ten separately titled sections. This is a content-completeness checklist and editorial target, not a rigid page template. A short chapter should still connect intuition, concept, implementation, and evidence in proportion to its subject.

### Research discipline: from implementation to theory

Theory must be more than a confident explanation of current code. Where the repository makes or explores a general claim, preserve the reasoning chain:

**implementation → observation → proposition → evidence → counterexample / open question**

- **Implementation:** identify the exact code, contract, or behavior under discussion.
- **Observation:** record what source inspection, tests, a trace, or a benchmark actually shows, including the conditions of observation.
- **Proposition:** state the general claim that might follow. Make it precise enough to challenge.
- **Evidence:** link repeatable tests, examples, measurements, or independent reasoning that supports or weakens the claim.
- **Counterexample / open question:** record the conditions that break the claim, where evidence is missing, and what experiment would resolve uncertainty.

Do not silently promote an observation into a universal law. Label assumptions and confidence. Big-O analysis, benchmark measurements, formal guarantees, conceptual models, and anecdotes are different kinds of evidence and must not be presented as interchangeable.

### Chapter organization and maintainability

- Provide a **theory index / learning path** that describes the intended order and links to standalone chapters.
- Give chapters stable, descriptive names and ordering prefixes where order matters. Keep links useful when a chapter is read independently.
- Make it possible for instructors and maintainers to insert, reorder, replace, or extend chapters without requiring edits to one giant monolithic document.
- Keep reusable source examples, visual assets, exercises, and answer/rationale notes clearly associated with the chapter that uses them.
- Prefer living, editable visuals when they materially improve understanding. Keep their source editable, explain how to regenerate them, and provide static fallbacks or text equivalents. State whether each visual is conceptual, source-derived, or captured from a named execution.
- Link implementation claims to the relevant source and tests. Avoid brittle line-number citations unless the repository has a reliable way to keep them current.
- Keep the learning path complementary to the README, getting-started guide, API reference, UML, and runnable examples. The theory chapter explains **why and how to reason**; API reference defines exact contracts; recipes teach a task; UML maps structure and relationships.
- Include limitations and poor-fit scenarios. The goal is understanding, not marketing.
- Do not invent a public type, feature, test result, performance claim, guarantee, or external dependency to complete a narrative.
- For a repository that has no meaningful domain theory, document that decision and link to the owning repository's theory instead of manufacturing chapters.

### Review standard

A theory chapter is ready when a reader can answer: What problem is this solving? What are the concepts and assumptions? Why does the model work? Where is it implemented? What evidence supports the explanation? What does it not guarantee? How could I reproduce, challenge, or extend it? What should I read next?

The theory index and chapter set should make these answers discoverable without requiring the reader to already know the implementation or the repository's internal vocabulary.

## Accuracy and evidence

Source is authoritative for implementation claims. Separate implemented behavior, current development state, intended architecture, and future work. Label examples as **Verified** (built/run against a named version/commit), **Source-checked** (checked but not executed), or **Conceptual** (illustrative, not runnable). Diagrams must be identified as source-derived, authored conceptual models, or captured runtime observations. Record target identity, source/revision, analysis scope, generation path, unresolved relationships, and regeneration/check instructions where applicable. A green CI badge is not an explanation of what was tested. Never imply that a static picture proves runtime behavior or that fixture-level generation proves repository-wide automation.

## Canonical recursive, target-centered UML and data cards

The Workshop diagram is **not conventional UML with cosmetic changes**. It is a recursive, target-centered map of a declared type, its users, dependencies, data types, and external package references. The focal type is the visual anchor.

### Layout

1. **Above: users/consumers.** Place source-supported types that use, construct, call, contain, inherit from, or implement the target above it. Label the actual relationship. State the scan scope (file, project, solution, repository, or known consumers); absence from a partial scan does not prove absence of a user.
2. **Center: target.** Center the selected class, struct, interface, record, enum, delegate, or other declaration. Show declaration kind and name; include namespace/context when needed.
3. **Below: target dependencies.** Show base types, interfaces, property/field types, parameter/return types, constructed types, and other verified dependencies below the target. Static structure does not imply execution order.
4. **Right: package references.** Show relevant packages, assemblies, projects, and frameworks. Distinguish direct references from transitive, inferred, and unresolved references. Never guess package ownership from a familiar type name.
5. **Card-edge type conduits.** Draw short colored stubs for represented type connections. Matching ends share stable type identity and palette; the receiving stub is rotated 180 degrees. Cover built-ins (bool, byte, char, integer widths, floating-point types, decimal, string, object), plus arrays, nullable forms, generics, tuples, and user-defined types. Labels and an accessible legend are required; color alone is insufficient.
6. **Relationship connectors.** Keep inheritance, interface realization, association, composition, aggregation, construction, invocation, and dependency distinct and labeled. A type conduit indicates represented type identity; it does not prove assignability or runtime data movement.
7. **External/unresolved types.** Show fully qualified identity and owning assembly/package when known. Mark unresolved relationships instead of inventing or silently omitting them.

Every connector must be supported by source/tests or clearly marked as an authored conceptual relationship. The picture should let a reader quickly answer: who uses this type, what does it use, which types appear at its boundaries, and which packages matter?

### Recursive levels of detail and interaction

A data card is the same entity at different levels, not multiple competing representations:

- **Collapsed:** declaration kind and name (for example, “interface IResolver”).
- **Contract:** meaningful public members, signatures, property/field types, parameters, return values, and relevant modifiers.
- **Declaration/AST:** syntax-tree shape, nested declarations, and member structure when compiler-backed source analysis provides it.
- **Implementation:** bodies and deeper details only when the underlying source model provides them and the reader requests them.

Expansion preserves stable identity and relationship context. Apply the same card language recursively to types, members, signatures, and nested declarations. When the platform supports input, collapsed cards must be clickable and keyboard-operable to expand/collapse; provide visible focus, accessible names, and a text equivalent. On static platforms, link to deeper docs/source instead. **Never claim a static SVG is interactive.**

Name the source of each detail: compiler-backed syntax/semantic analysis for AST and symbol relationships; reflection only for runtime-visible metadata; human-authored explanation for purpose/use cases; captured traces for runtime observations. Reflection is not a complete AST, and source structure is not a runtime trace.

### Per-type documentation and use cases

For each in-scope C# source file, document each declared type in a companion Markdown page where practical. If one file declares multiple types, identify each page's target. Each page explains the type's purpose and why it exists, responsibilities/non-goals, declaration kind, compact class/data card, key members, source-backed relationships and package references, examples/tests, current status/limits, and links to source, tests, and related docs.

Provide a companion `<TypeName>UML.md` with the wider target-centered diagram and a **Use cases** section. Each use case identifies actor/consumer, goal, trigger/preconditions, successful outcome, important alternatives/failures, and evidence/status. AST/reflection cannot infer user intent; use cases require human/LLM-authored direction grounded in product goals and source. The compact card belongs in the main type page; the companion UML page is the expanded neighborhood view.

### Stable palette, semantic model, and CI

Maintain a stable documented palette for built-in and compound types across diagrams. Preserve distinctions (byte ≠ int; string ≠ char; int ≠ long; a type parameter ≠ built-in type). Use labels, symbols, or line patterns as well as color. A matching conduit is not proof of legal conversion or execution-time transfer.

Build a normalized, renderer-neutral model before rendering SVG or another view. Preserve source identities/spans, declaration kind, relationship kind, type identity, analysis scope, and provenance. SVG is a rendering, not the only semantic store, so facts can be presented at different detail levels and by different platforms.

A release-quality gate inventories in-scope source files/types; verifies required type pages and UML pages; validates links/metadata; regenerates diagrams; and detects missing/stale outputs. State exact scope and method (syntax-only, compiler-backed, reflection-derived, or manual). If automation covers only fixtures/staging, say so. Do not claim repository-wide generation until CI verifies repository-wide coverage.


## Comprehensive UML documentation system

UML documentation is a family of complementary views, not a single class-diagram deliverable. Each view answers a different question; do not force all information into one diagram. The normative detailed contracts are maintained as focused documents under [`docs/UML/Standard/`](docs/UML/Standard/README.md). Keep this root standard as the cross-repository policy and keep the FSM_COS visual-specific implementation rules in [Living UML Visual Style](docs/UML_VISUAL_STYLE.md).

### Required documentation inventory

Every repository adopting this standard must inventory applicable view types, mark each as **required**, **not applicable with reason**, **manual**, **source-derived**, **runtime-captured**, or **planned**, and identify coverage gaps. Do not represent a planned generator as an implemented one. For code repositories, assess all of these:

- **Class/type diagrams:** one focused source-linked diagram per declared type in scope, including class, interface, struct, record, enum, delegate, nested and generic declarations.
- **Class neighborhoods:** one focal type, verified consumers above, the focal type centered, dependencies and results below/around it, and relevant project/package references to the right. Include expandable UML data cards where the medium supports interaction.
- **Sequence diagrams:** caller-to-callee interactions for important workflows, including alternative/failure paths, with method/source traceability and complexity evidence.
- **Activity/control-flow diagrams:** branches, loops, early exits, exception paths, validation gates, and convergence conditions for nontrivial algorithms.
- **State/lifecycle diagrams:** states, transitions, triggers, guards, side effects, terminal states, invalid transitions, and ownership of transitions.
- **Use cases:** one file per use case, classified under a stable category. Separate user goals from implementation structure.
- **Architecture/component diagrams:** package/project boundaries, responsibility ownership, allowed dependency direction, external integrations, and the distinction between direct and transitive dependencies.
- **Deployment/runtime topology:** only where the system actually has deployment/runtime nodes; show process, host, service, storage, trust boundaries, network edges, and configuration ownership without implying an unimplemented topology.
- **Data models and data flow/lineage:** schemas and object graphs, ownership/mutability, serialization boundaries, transformations, provenance, validation, and where data enters/leaves a subsystem.
- **MicroBundle and manifest views:** where a repository defines, consumes, publishes, or composes MicroBundles, document each MicroBundle contract and the manifest/dependency/load relationship. Inventory every MicroBundle defined in the repository; distinguish local definitions, external contracts, examples, tests, and unresolved references. Include identity/version, capability purpose, dependencies, inputs/outputs, configuration, lifecycle/load stage, arbitration behavior, failure behavior, compatibility/security assumptions, source/tests, and publication status where applicable.
- **Requirement/test traceability:** link goals and contracts to tests and evidence. This may be a table or graph rather than a UML notation.
- **Decision/constraint records:** link architecture decision records and important invariants to affected diagrams and tests; do not hide rationale inside a picture.

A view is not mandatory merely because it is listed. For example, a library may have no deployment topology; mark it not applicable with a reason. The inventory itself is mandatory so omissions are deliberate and visible.

### Analysis provenance and confidence

Every generated or maintained artifact must declare its evidence class: **source-derived** (syntax/semantic model), **reflection-derived** (compiled runtime metadata), **runtime-captured** (trace or observation from a named run), **human-authored** (design intent/use cases), or **conceptual** (illustrative only). Mixed artifacts identify which facts came from which source. Include target revision, scope, tool/version when generated, and unresolved/ambiguous relationships.

Use Roslyn syntax trees and semantic models for C# declaration and source relationships, method bodies, invocation sites, control flow, and source spans. Reflection may enrich compiled type/member metadata and runtime attributes, but cannot reconstruct a complete AST, local variables, method-body control flow, or an authoritative static call graph. Tests and runtime traces can corroborate behavior but must not silently substitute for source structure.

### Complexity and performance analysis

For sequence/control-flow views, include a complexity table when algorithmic cost is relevant. For each hot or nontrivial method record: input-size variables, dominant operations, loops/recursion, collection or graph operations, expected/average and worst-case complexity where defensible, allocations/copies, I/O or blocking boundaries, and evidence confidence. Explicitly flag likely quadratic-or-worse paths (nested input-dependent loops, repeated scans, repeated sorting, or graph traversal without visited-set guarantees) and identify candidate linear paths/optimizations.

Big-O is not a wall-clock upper bound. Do not present a numeric time or memory bound without explicit assumptions about input limits, implementation, runtime, hardware, allocation behavior, and external dependencies. Separate static heuristic from measured benchmark; link benchmark inputs, environment, revision, and results. Where a reliable bound cannot be established, say **unknown / requires measurement**, explain why, and provide a measurement plan instead of inventing precision.

### MicroBundle inventory and evidence

A MicroBundle is a domain-level capability unit, not automatically the same thing as a project, assembly, NuGet package, or deployment unit. The MicroBundle domain contract and the repository's actual implementation determine the meaning. For every locally defined MicroBundle, provide a companion page and index entry; identify its stable identity, versioning rules, purpose, dependencies, inputs/outputs, configuration, state/lifecycle, arbitration/load behavior where applicable, failure modes, trust assumptions, tests, and owning source. Show dependency edges with their provenance. Mark catalog/provider behavior and remote retrieval/content-hash verification as external or planned unless implemented and verified in the target revision.

For FSM_COS specifically, diagrams must preserve this boundary: the host supplies the catalog and owns application execution, lifecycle, UI, and presentation; FSM_COS resolves and validates the requested dependency closure, composes in dependency order, performs bounded arbitration, and returns a `RuntimeAssembly`. Do not imply that a manifest entry alone downloads, verifies, activates, or executes a MicroBundle.

### CI and release gate

The documentation gate should compare the inventory against repository source and known MicroBundle definitions, verify required companions and links, regenerate source-derived artifacts, and fail on stale/missing output within its declared scope. Emit a coverage report with counts and exceptions. Keep generated output deterministic and preserve source spans/stable symbol identities. Treat the following as separate measures: source declaration coverage, relationship coverage, use-case coverage, MicroBundle coverage, and freshness. Passing one does not imply passing the others.

If generation or static analysis is not implemented, the documentation must say so plainly. A folder taxonomy and written standard are not proof of generated diagrams, complete inventories, complexity analysis, or CI enforcement.

## Visual and editorial quality

Use contrast for priority, repetition for recognition, proximity for ownership, and concrete metaphors for abstract systems. Diagrams need descriptive alt text, explanatory captions, readable labels, and accurate boundaries. Avoid hype as a substitute for evidence. State prerequisites, poor-fit scenarios, maturity, limitations, and performance context honestly.

## Workshop identity and canonical document footer

The Workshop's static brand mark is the canonical footer image for Markdown documents. Use this exact, repository-independent raw image URL:

`https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png`

Append the following block at the very end of each Markdown document, after all substantive content and links:

```html

```

This footer mark is distinct from the animated moniker used as a document header or the author's GitHub avatar. Do not substitute `Avatar.jpg`, a personal avatar, an SVG moniker, a repository-relative image, or a guessed mirror for this canonical asset. Keep the exact raw URL so the image resolves consistently across repositories. The footer block must appear once, at the end; do not duplicate it or add competing workshop-logo footers. Existing documents should be migrated to this exact block when they are next edited as part of documentation-standard alignment.

## Review checklist

- [ ] README follows the shared reader journey where applicable.
- [ ] A newcomer can find a first proof and a non-coder learning path.
- [ ] What/why/how, ownership, non-goals, and dependency direction are clear.
- [ ] Implemented behavior is distinguished from plans and conceptual illustrations.
- [ ] Target-centered UML places users above, target at center, dependencies below, packages/references to the right.
- [ ] Recursive cards expose collapsed and expanded levels; interaction is claimed only on capable platforms.
- [ ] Type conduits use stable semantic identities, labels, and accessible non-color cues.
- [ ] In-scope types have per-type docs and companion UML/use-case pages.
- [ ] Diagram scope, provenance, unresolved relationships, regeneration path, and stale checks are accurate.
- [ ] Examples, links, and verification instructions match the target branch.
- [ ] No publication, deployment, or release operation is implied by documentation-only work.




## SURPS participation, sourcebooks, and enrollment

SURPS participation is an explicit repository-level capability. A repository participates when it contributes a domain-owned sourcebook, rulebook, provider, ontology, scenario, or other declared output that SURPS can discover and compose. Merely linking to SURPS, mentioning roleplaying, or consuming an unrelated package does not qualify.

**Repository participation does not make SURPS mandatory for every experience.** SURPS is enabled per experience, not globally. When the experience's SURPS option is disabled, it runs without SURPS rulebook discovery or dependency; FSM_API, FSM_COS, and FSM_Foundry remain independently usable. When enabled, the host composes the SURPS master system and the explicitly selected rulebooks/providers for that experience. The README badge advertises repository outputs; it does not imply that every consumer or runtime must enable SURPS.

SURPS is the shared composition and publishing system; it does not take ownership of the participating repository's domain. Each participant remains authoritative for its own concepts, theory, implementation, evidence, and sourcebook content. SURPS owns shared ontology, rulebook identity and composition, CORE rules, LOD/campaign selection, and the cross-domain authoring and enrollment contract. Domain books link to neighboring authoritative theory rather than copying it.

### Enrollment manifest

A participating repository maintains one machine-readable manifest at `.surps/participation.json`. The manifest is the source of truth for its enrollment status and book catalogue. At minimum it declares:

- schema version and repository identity;
- participation status (**draft**, **active**, **suspended**, or **retired**);
- the owning domain and a concise statement of its SURPS contribution;
- each locally produced book's stable ID, title, category, LOD, version, status, canonical book path, descriptor path, and cover path;
- the book's source/evidence status and the repository revision or release to which claims apply;
- optional cross-repository dependencies by stable identity, never by copied book content.

A book may be listed only when its authoritative content and descriptor are present in the producing repository. A planned title may appear in a clearly separated **planned** section, but must never be presented as a published/available book. A participant with no book output may enroll as a provider/contributor, but its badge must say so rather than implying it publishes books.

### Book and cover enrollment gate

Every published book output must have one canonical home under `docs/books/`, one machine-readable descriptor beside or linked from its book record, and one owned cover under `docs/books/covers/` (or the repository's documented equivalent during migration). The cover is part of the book's identity and enrollment—not a decorative afterthought.

The cover gate verifies that the asset exists in the producing repository, is linked from the book descriptor and catalogue, has a descriptive title/alt text and provenance, and visually identifies the book title, owning domain/publisher, and LOD/category where applicable. Prefer editable SVG with a static, accessible fallback where required. Do not fabricate a cover from a title, use an unrelated repository avatar, or claim that a conceptual cover depicts implemented content. A cover may be authored progressively, but the book remains **draft / cover in progress** until its required identity elements are present. Preserve the original source asset and document its generation/editing path.

### Automatically maintained README badge

Enrollment automatically adds a SURPS badge block to the repository README. The badge is generated from `.surps/participation.json`; it is not manually maintained text and must never be inferred from repository name, topics, or a link to SURPS.

Use these stable README markers exactly once:

```markdown
<!-- SURPS:PARTICIPATION:START -->
[![SURPS participation](docs/assets/badges/surps-participation.svg)](https://github.com/TrentBest/TheSingularityWorkshop.SingularityUniversalRoleplayingSystem)
<!-- SURPS:PARTICIPATION:END -->
```

The generated SVG lives at `docs/assets/badges/surps-participation.svg`. It identifies the repository as a SURPS participant and lists the actual book outputs by title and LOD. If the repository contributes providers but no books, the badge lists the contribution type instead. Long catalogues may use a compact first line plus a linked book-count/catalogue line; the full catalogue remains available at `docs/books/`. The badge must not imply a book is released when its manifest status is draft, planned, or under review.

The enrollment workflow must validate the manifest, verify every referenced book/descriptor/cover path, regenerate the SVG deterministically, and update only the README region between the markers. It must fail on duplicate book IDs, missing assets, unknown status/LOD values, malformed links, or a badge that does not match the manifest. It must not rewrite unrelated README content. A repository is not enrolled until this workflow and its output are verified on its active development branch.

The SURPS repository provides the first reference implementation on its active development branch: [enrollment manifest](https://github.com/TrentBest/TheSingularityWorkshop.SingularityUniversalRoleplayingSystem/blob/development/.surps/participation.json), [badge generator](https://github.com/TrentBest/TheSingularityWorkshop.SingularityUniversalRoleplayingSystem/blob/development/scripts/generate_surps_badge.py), [enrollment workflow](https://github.com/TrentBest/TheSingularityWorkshop.SingularityUniversalRoleplayingSystem/blob/development/.github/workflows/surps-enrollment.yml), and [generated badge](https://github.com/TrentBest/TheSingularityWorkshop.SingularityUniversalRoleplayingSystem/blob/development/docs/assets/badges/surps-participation.svg). Repositories adopting the standard should reuse this implementation or a compatible equivalent; the manifest remains authoritative.\n\n**Presentation ownership:** SURPS owns enrollment metadata, book identity, LOD policy, and content validation. GUI/GUI.SVG owns reusable semantic badge composition and document presentation; use the SURPS generator as an interim fixture until the supported GUI.SVG contract is available, then route the same manifest data through that shared renderer. Visualize owns general scene/illustration rendering and related source-to-output provenance. Do not create a competing badge renderer in every domain repository or make GUI Core depend on GitHub, Markdown, or filesystem APIs. See [GUI's semantic badge composition issue #86](https://github.com/TrentBest/TheSingularityWorkshop.GUI/issues/86) and [Visualize's theory/evidence work](https://github.com/TrentBest/TheSingularityWorkshop.Visualize/issues/37). The SURPS-to-Gauntlet integration is tracked in [Gauntlet issue #6](https://github.com/TrentBest/TheSingularityWorkshop.Gauntlet/issues/6).

### Shared book identity and visual system

Use the shared SURPS book identity/cover specification for descriptor fields, title hierarchy, publisher identity, LOD marks, and cover proportions. Domain identity remains visible; the SURPS mark communicates shared participation, not ownership or authorship of every book. A book can be referenced by multiple experiences, but its authoritative content has exactly one producing repository.

The first enrollment example is SURPS CORE — **Spying**, currently identified as a tabletop playtest draft. Its status must remain explicit; the badge must not imply completed group playtesting or digital runtime implementation.

### Gauntlet extension and conformance

The Gauntlet is a first-class SURPS proving ground. SURPS must extend Gauntlet so it can exercise composed domain rulebooks and providers as integrated experiences, rather than treating each domain's isolated demo as sufficient evidence.

The integration contract is layered:

1. **Gauntlet owns the proving-ground harness and its challenge/encounter progression.** It defines how a test run is staged, observed, recorded, replayed, and judged.
2. **SURPS owns the shared rulebook composition and campaign/LOD selection.** A SURPS-enabled Gauntlet run selects an explicit SURPS profile and the rulebooks/providers under test.
3. **Participating domains own their rules and capabilities.** Physics resolves physical consequences; Gestures supplies posture/motion/recognition/coordination; Dungeon owns underground environments; City owns civic systems; Laboratories owns experiments/apparatus/procedures; StoryBuilder owns narrative construction. Each contributes evidence at its own boundary.
4. **A Gauntlet scenario is a conformance fixture, not a substitute for domain tests.** It records the selected rulebook IDs/versions, manifest, initial conditions, actions/inputs, expected invariants, observed outputs, evidence references, and unresolved limitations.
5. **Offline tabletop equivalence remains a SURPS requirement.** A Gauntlet scenario may automate or visualize a procedure, but it cannot make the corresponding SURPS rule unintelligible or unplayable without the application.

The initial SURPS extension should be a thin adapter/fixture set in Gauntlet that proves opt-in composition, explicit LOD selection, provider discovery, cross-domain event flow, time/turn behavior, and evidence capture. Do not move the Gauntlet harness into SURPS or duplicate domain simulation code. Mark any adapter or scenario as planned until it exists and is verified.

### Enrollment and adoption checklist

- [ ] Repository owner and domain contribution are identified.
- [ ] `.surps/participation.json` exists and validates.
- [ ] Each listed book has one authoritative content path and descriptor.
- [ ] Each published book has its own owned, provenance-recorded cover.
- [ ] Book status and LOD are accurate; planned work is not advertised as published.
- [ ] README badge is generated from the manifest and lists actual outputs.
- [ ] Badge links to the SURPS catalogue and the local catalogue links back to the book.
- [ ] Domain ownership and cross-repository dependencies are explicit.
- [ ] Gauntlet scenarios exist or are explicitly marked planned, with reproducible evidence.
- [ ] Offline tabletop rules remain understandable independently of the digital harness.
- [ ] Existing links, Pages workflows, and CI are verified after adoption.

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
