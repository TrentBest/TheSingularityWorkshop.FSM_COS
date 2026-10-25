<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# Documentation Standard

This is the shared documentation standard for The Singularity Workshop repositories. The goal is to **edify, not mystify**: explain the human problem, why the design exists, the actual technical boundary, how to try the technology, and how to verify claims.

## Reader journey and teaching

Unless irrelevant to a repository, README sections follow this order: **00 Identity**, a real “money-shot” visual, **01 What**, **02 Why**, **03 How**, **04 See it in a minute**, and **05 Available documentation and theory**. Use the Workshop's consistent section markers where applicable (✳️, 🟦, 🟣, 🩵, 🟢, 🟪); markers supplement visible headings and are never the only carrier of meaning. Omit irrelevant sections rather than padding the README.

Teach from experience toward abstraction: show a recognizable problem; use a familiar analogy and state where it stops matching reality; define the concept plainly; explain why the design decision matters; show a diagram/example; offer a deeper explanation or exercise. For every major topic answer: **What is it? Why does it exist? How does it work? How can a reader verify it? What next?** Explain what a boundary costs, enables, and cannot guarantee. The goal is to help people understand and create software without requiring a computer-science degree.

Keep a focused document set: README for orientation and proof; learning/getting-started guide; theory and architecture; exact API/contracts; runnable tutorials; Gallery.md with a two-sentence overview and links to detailed recipes; per-type documentation; companion per-type UML/use-case pages; and verification/status docs. Prefer one authoritative explanation per topic. Gallery visuals should teach through real output, identify how to reproduce it, and state what they do and do not prove.

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

## README badge policy

Every repository README should use the same **small, predictable badge row**, directly below the repository identity/moniker and title, before the opening pitch. The default is five core badges, plus one conditional package badge pair. Badge order is deliberate: health first, then distribution, quality, and legal identity.

### Canonical order

1. **GitHub Actions / CI status** — first; point to the authoritative workflow that exercises the repository's meaningful validation path. Use the correct default/working branch and workflow filename.
2. **NuGet version** — for a repository that publishes a NuGet package; point to the exact package ID on nuget.org and show the published version, never a project-file version that has not shipped.
3. **NuGet downloads** — immediately after the version badge, for that same published package ID.
4. **Codecov coverage** — when coverage is actually uploaded to Codecov for this repository and the badge is connected to the correct repository. Coverage is a measurement, not proof that the tests are comprehensive.
5. **Last commit** — a maintenance/activity signal, linked to the repository or commit history. It is not a quality, freshness guarantee, or release-status badge.
6. **License** — last; use the license actually present in the repository and link to its authoritative text.

This is the Workshop default, not a quota. **Do not manufacture a badge to fill a slot.** If a badge is not applicable or cannot be verified, omit it and report why in the badge audit. Repositories that do not publish NuGet packages omit both NuGet badges. Repositories without a trustworthy coverage upload omit Codecov. Add other badges only when they answer a concrete reader question and have an authoritative source; do not add stars, forks, repository size, language guesses, generic quality scores, or vanity labels just to increase the count.

### Presentation

- Keep badges on one compact row when practical; on narrow screens, natural wrapping is fine. Use consistent Markdown badge syntax and a consistent style (Workshop default: flat-square where the provider supports it). Keep the README's animated moniker and title visually distinct from the badge row.
- Each badge has a meaningful accessible label, an authoritative image/source URL, and an appropriate click-through destination. Prefer dynamic source-backed values over hand-maintained claims.
- The row's **membership and order are standardized**; individual badge presence is conditional on applicability and verified health. No extra badge is added merely because it is available.
- Preserve existing valid badges during README edits. Do not silently delete, reorder, or replace them as collateral formatting cleanup. If a badge is broken or inaccurate, remove it from the reader-facing row only after recording it in the audit report; repair it when the correct source is established.
- When a workflow, package ID, default branch, coverage upload, license, or release process changes, revalidate all dependent badge definitions.
- A green CI badge means only that the linked workflow passed its own checks. It must not imply broader release readiness, package publication, or behavior that the workflow does not test.

### Badge health reporting (for the Workshop and eventually “The”)

Broken badges should **not remain in a public README**, but discovering one must not erase the evidence that it was broken. Badge audit is a reporting responsibility, not just a visual cleanup.

For each repository, a badge audit should record:
- repository and default branch;
- expected badge kind/order and applicability;
- rendered badge image/source URL and click-through URL;
- validation time and result (healthy, broken, misconfigured, not-applicable, or unverified);
- concise failure reason and affected README line/section;
- remediation recommendation, owner/next action, and issue URL if one is opened.

A healthy badge remains in the README. A confirmed broken or misleading badge is removed or repaired in the same change when safe; the report must preserve the finding and remediation trail. A transient network/provider error is **unverified**, not automatically a broken badge. Never silently suppress failures, and never treat an unreachable badge image as a reason to delete the entire badge row.

The eventual automated audit should compare README badges with repository metadata and actual workflow/package/coverage configuration, validate image and destination links, check the canonical order, and emit machine-readable results plus a human-readable summary. It should distinguish errors from warnings and applicability exceptions. Reports should be consumable by a Workshop-level roll-up and, eventually, “The”; repository-level findings must retain their source repo, evidence, timestamp, and issue/remediation state. Start with read-only reporting and issue creation; do not automatically publish packages, change release gates, or make destructive repository edits.

### Badge review checklist

- [ ] The canonical order is preserved; only applicable, verified badges are displayed.
- [ ] CI badge identifies the correct workflow and branch.
- [ ] NuGet badges refer to the exact package ID and published feed.
- [ ] Codecov badge corresponds to uploaded coverage for this repository.
- [ ] Last-commit and license badges have correct destinations and claims.
- [ ] Every broken/misleading badge is repaired or removed from the README **and recorded for follow-up**.
- [ ] The audit distinguishes confirmed failure from transient/unverified results and records remediation.
- [ ] README edits and workflow/package/branch changes trigger a badge integrity check.

## Review checklist

- [ ] README follows the shared reader journey where applicable.
- [ ] README retains a concise, verified badge row; badge links, labels, and claims remain accurate.
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

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
