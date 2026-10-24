<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# Class and Type Diagrams

## Purpose
Answer: **What is this declaration, what is its contract, and what source-backed types relate to it?**

## Coverage
Inventory each in-scope class, interface, struct, record, enum, delegate, nested type, generic declaration, and other relevant declared type. A source file with multiple declarations gets a distinct target identity for each declaration. File name alone is not a sufficient identity.

## Canonical composition
Use the Workshop target-centered layout: verified consumers above; focal declaration centered; relevant dependencies and results below or to the sides; package/project/framework references on the right. Label each relationship (implements, inherits, constructs, invokes, contains, parameter type, return type, or another precise relation). Do not confuse containment with dependency or imply execution order from static structure.

Each page identifies namespace, declaration kind, generic parameters, relevant modifiers, public contract, meaningful members, source/test links, and analysis scope. Nested declarations remain separately addressable and may also appear as nested cards.

## Provenance
Prefer Roslyn syntax and semantic models for declaration identity, signatures, symbol relationships, and source spans. Reflection can supplement compiled metadata but cannot replace source analysis for full source coverage. Identify generated, partial, conditional, external, and unresolved declarations.

## Acceptance
- One stable identity per declaration; source spans or equivalent source navigation.
- Relationships labeled and backed by evidence.
- Missing/external/unresolved relationships visible, not fabricated.
- Per-type companion and diagram indexes report gaps.
- Manual/conceptual diagrams are explicitly labeled as such.

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
