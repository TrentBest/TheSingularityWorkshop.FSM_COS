<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# MicroBundleLoadContext — UML neighborhood

**Target:** `TheSingularityWorkshop.FSM_COS.MicroBundleLoadContext`  
**Evidence class:** manually authored, source-checked structural view derived from the existing type companion.  
**Source page:** [MicroBundleLoadContext.md](MicroBundleLoadContext.md)  
**Source declaration:** [MicroBundleLoadContext.cs](../../../../src/FSM_COS/MicroBundleLoadContext.cs)  
**Generation status:** not generated; no runtime trace is implied.

## Purpose

FSM_COS implementation of the domain-owned load-context contract, exposing runtime identity and per-bundle configuration bytes.

## Target type model

~~~mermaid
classDiagram
    class MicroBundleLoadContext {
        +RuntimeId: ulong
        +Configuration: IReadOnlyDictionary~ulong, ReadOnlyMemory~byte~~
        +TryGetConfiguration(bundleId, configuration) bool
    }
    class IMicroBundleLoadContext
    MicroBundleLoadContext ..|> IMicroBundleLoadContext
~~~

## Consumers, dependencies, and boundary

The relationships shown are limited to those documented in the linked type companion. This page does not claim a repository-wide caller scan. Consumers not represented in the documented model remain unknown until a compiler-backed source scan is performed. Package references and external types must be confirmed against project metadata and semantic symbols before being treated as exhaustive.

## Use cases

1. Pass the runtime identity and optional configuration bytes into a MicroBundle's `Load` method.
2. Let the bundle interpret its own configuration format; FSM_COS transports the bytes but does not parse them.
3. Keep bundle initialization tied to composition rather than host-specific UI or application lifecycle.

## Coverage notes

- Manually maintained and source-checked, not generated from Roslyn or reflection.
- Structural type relationships do not prove invocation order or runtime data flow.
- Sequence, activity/control-flow, and measured complexity evidence remain separate deliverables where applicable.

See [class neighborhood standard](../../Standard/CLASS_NEIGHBORHOODS.md), [traceability standard](../../Standard/TRACEABILITY.md), and the [coverage inventory](../../INVENTORY.md).

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
