![The Singularity Workshop animated moniker](https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Renderer/development/docs/assets/moniker.svg)

# IManifestLoadEvaluator

**Source:** [`src/FSM_COS/IManifestLoadEvaluator.cs`](../../../src/FSM_COS/IManifestLoadEvaluator.cs)  
**Source baseline:** `30e437100d1f3b49d07111e87f76cb4811ba8465`  
**Source file blob:** `735a28a7ec2180330f20d600152f83c3f391c519`  
**Authorship:** manually authored against current `development`; not GUI.SVG-generated.

## Purpose

Experience-owned policy contract for deciding whether a deferred manifest entry is wanted.

## UML class card

~~~mermaid
classDiagram
    class IManifestLoadEvaluator {
        <<interface>>
        +ShouldLoad(entry, experienceContext) bool
    }
    class RuntimeManifestEntry
    class IStateContext
    IManifestLoadEvaluator ..> RuntimeManifestEntry : evaluates
    IManifestLoadEvaluator ..> IStateContext : optional Experience context
~~~

## Important boundaries

- The Experience supplies the meaning of the decision.
- The evaluator does not resolve dependencies, localize artifacts, or load bundles.

## Use cases

1. Let the host or Experience decide whether a deferred manifest entry is wanted in the current context.
2. Keep the decision policy separate from dependency resolution and artifact localization; the evaluator does not load a bundle.




---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
