<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# IManifestLoadEvaluator — UML neighborhood

**Target:** `TheSingularityWorkshop.FSM_COS.IManifestLoadEvaluator`  
**Evidence class:** manually authored, source-checked structural view derived from the existing type companion.  
**Source page:** [IManifestLoadEvaluator.md](IManifestLoadEvaluator.md)  
**Source declaration:** [IManifestLoadEvaluator.cs](../../../../src/FSM_COS/IManifestLoadEvaluator.cs)  
**Generation status:** not generated; no runtime trace is implied.

## Purpose

Experience-owned policy contract for deciding whether a deferred manifest entry is wanted.

## Target type model

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

## Consumers, dependencies, and boundary

The relationships shown in the class model above are limited to the relationships already documented in the linked type companion. This page does not claim a repository-wide caller scan. Consumers not represented by the documented model are unknown until a compiler-backed source scan is performed. Package references and external types must be confirmed against the project file and semantic symbols before being treated as exhaustive.

## Use cases

1. Let the host or Experience decide whether a deferred manifest entry is wanted in the current context.
2. Keep the decision policy separate from dependency resolution and artifact localization; the evaluator does not load a bundle.

## Coverage notes

- The diagram is manually maintained and source-checked, not generated from Roslyn or reflection.
- Structural type relationships do not prove invocation order or runtime data flow.
- Sequence, activity/control-flow, and measured complexity evidence for this type remain separate deliverables where applicable.

See [class neighborhood standard](../../Standard/CLASS_NEIGHBORHOODS.md), [traceability standard](../../Standard/TRACEABILITY.md), and the [coverage inventory](../../INVENTORY.md).

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
