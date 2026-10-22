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

## Visual and editorial quality

Use contrast for priority, repetition for recognition, proximity for ownership, and concrete metaphors for abstract systems. Diagrams need descriptive alt text, explanatory captions, readable labels, and accurate boundaries. Avoid hype as a substitute for evidence. State prerequisites, poor-fit scenarios, maturity, limitations, and performance context honestly.

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
