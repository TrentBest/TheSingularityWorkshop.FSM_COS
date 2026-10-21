![The Singularity Workshop animated moniker](https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Renderer/development/docs/assets/moniker.svg)

# RuntimeManifestDependency

**Source:** [`src/FSM_COS/RuntimeManifestDependency.cs`](../../../src/FSM_COS/RuntimeManifestDependency.cs)  
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

1. Represent and validate published identity or dependency metadata.
2. Keep artifact resolution and runtime policy at the appropriate repository or composition boundary.

## Verification status

Source-reviewed against the recorded baseline. Mermaid rendering, automated link checking, and exact-HEAD CI have not been verified.
