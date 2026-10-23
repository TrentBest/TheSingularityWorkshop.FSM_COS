# RuntimeManifestDependency — UML neighborhood

**Target:** `TheSingularityWorkshop.FSM_COS.RuntimeManifestDependency`  
**Evidence class:** manually authored, source-checked structural view derived from the existing type companion.  
**Source page:** [RuntimeManifestDependency.md](RuntimeManifestDependency.md)  
**Source declaration:** [RuntimeManifestDependency.cs](../../../../src/FSM_COS/RuntimeManifestDependency.cs)  
**Generation status:** not generated; no runtime trace is implied.

## Purpose

Immutable directed dependency edge indicating that one bundle ID requires another bundle ID.

## Target type model

~~~mermaid
classDiagram
    class RuntimeManifestDependency {
        +BundleId: ulong
        +DependencyId: ulong
        +IsValid: bool
    }
    RuntimeManifestSchedule o-- "0..*" RuntimeManifestDependency : validates dependency graph
~~~

## Consumers, dependencies, and boundary

Relationships are limited to those documented in the linked type companion. This page does not claim a repository-wide caller scan. Consumers not represented in the documented model remain unknown until a compiler-backed source scan is performed. Package references and external types must be confirmed against project metadata and semantic symbols before being treated as exhaustive.

## Use cases

1. Describe a dependency edge in a published runtime schedule.
2. Validate that each edge references known entries and that the schedule contains no dependency cycle.
3. Keep schedule metadata distinct from the dependency declarations on the resolved MicroBundle instances.

## Coverage notes

- Manually maintained and source-checked, not generated from Roslyn or reflection.
- Structural type relationships do not prove invocation order or runtime data flow.
- Sequence, activity/control-flow, and measured complexity evidence remain separate deliverables where applicable.

See [class neighborhood standard](../../Standard/CLASS_NEIGHBORHOODS.md), [traceability standard](../../Standard/TRACEABILITY.md), and the [coverage inventory](../../INVENTORY.md).
