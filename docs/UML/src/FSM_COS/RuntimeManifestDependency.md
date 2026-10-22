![The Singularity Workshop animated moniker](https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Renderer/development/docs/assets/moniker.svg)

# RuntimeManifestDependency

**Source:** [`src/FSM_COS/RuntimeManifestDependency.cs`](../../../../src/FSM_COS/RuntimeManifestDependency.cs)  
**Source baseline:** `342c6fe71aee5ae9089547348c140a612e083248`  
**Source file blob:** `2ad81443708f5ef9e4df7f96a6cc477ffb059778`  
**Authorship:** manually authored against current `development`; not GUI.SVG-generated.

## Purpose

Immutable directed dependency edge indicating that one bundle ID requires another bundle ID.

## UML class card

~~~mermaid
classDiagram
    class RuntimeManifestDependency {
        +BundleId: ulong
        +DependencyId: ulong
        +IsValid: bool
    }
    RuntimeManifestSchedule o-- "0..*" RuntimeManifestDependency : validates dependency graph
~~~

## Important boundaries

- `IsValid` rejects zero IDs and self-dependencies.
- Endpoint membership, duplicate edges, and graph cycles are validated by `RuntimeManifestSchedule`, not by this value type alone.

## Use cases

1. Describe a dependency edge in a published runtime schedule.
2. Validate that each edge references known entries and that the schedule contains no dependency cycle.
3. Keep schedule metadata distinct from the dependency declarations on the resolved MicroBundle instances.




---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
