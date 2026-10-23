# Class Neighborhoods and Expandable UML Data Cards

## Purpose
Answer: **Who uses this focal type, what does it rely on, and where do its inputs and outputs go?**

A neighborhood is target-centered, not a general graph dump. Put source-supported consumers above the target, dependencies and results below/around it, and relevant project/package/framework references to the right. State scan scope (file, project, solution, repository, or named external consumers); a partial scan cannot prove that no other consumer exists.

## Data-card levels
1. **Collapsed identity:** declaration kind and name.
2. **Contract:** meaningful members, signatures, parameter/return/property/field types, and modifiers.
3. **Declaration/AST:** nested declarations and syntax structure when compiler-backed source analysis supplies it.
4. **Implementation:** bodies and implementation details only where the source model makes them available.

A data card is the same entity at different detail levels. Expansion must preserve stable identity, parent/owner, relationship context, and navigation. Where the medium supports interaction, cards should be clickable, keyboard-operable, focus-visible, and accessible by name. Static SVGs must link to deeper views and must never be described as interactive.

## Relationships and uncertainty
Distinguish inheritance, realization, association, composition, aggregation, construction, invocation, and type dependency. Show direct, transitive, inferred, conceptual, and unresolved links differently and label them. Do not infer ownership from a connector or runtime movement from matching type conduits.

## Acceptance
Every neighborhood names its target, purpose, scope, evidence source, source revision, known unresolved links, and companion source/tests. Use-cases and human intent are separate from source facts.
