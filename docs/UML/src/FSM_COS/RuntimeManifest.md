![The Singularity Workshop animated moniker](https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Renderer/development/docs/assets/moniker.svg)

# RuntimeManifest

**Source:** [`src/FSM_COS/RuntimeManifest.cs`](../../../../src/FSM_COS/RuntimeManifest.cs)  
**Source baseline:** `9a5d9b72c42f1221e800926b09640d6e00f751ac`  
**Source file blob:** `c2b448da5e076f13209d68b1edd7ce50cbbd4d7a`  
**Authorship:** manually authored and checked against current `development`; not GUI.SVG-generated.

## Purpose

A machine-oriented request for a runtime composition. It carries the runtime identity, root MicroBundle/version requests, optional experience context, optional staged-load metadata, optional dependency schedule, and optional semantic intent.

## UML class card

~~~mermaid
classDiagram
    class RuntimeManifest {
        +RuntimeId: ulong
        +Bundles: IReadOnlyList~MicroBundleManifestEntry~
        +ExperienceContext: IStateContext
        +LoadPlan: IReadOnlyList~RuntimeManifestEntry~
        +Schedule: RuntimeManifestSchedule
        +Intent: SemanticIntent
        +Empty(runtimeId, context, intent)$ RuntimeManifest
        +ValidateStagedPlan() void
    }
    class MicroBundleManifestEntry
    class RuntimeManifestEntry
    class RuntimeManifestSchedule
    class IStateContext
    class SemanticIntent
    RuntimeManifest *-- "0..*" MicroBundleManifestEntry : root requests
    RuntimeManifest o-- "0..*" RuntimeManifestEntry : optional staged plan
    RuntimeManifest o-- "0..1" RuntimeManifestSchedule : optional schedule
    RuntimeManifest ..> IStateContext : optional context
    RuntimeManifest ..> SemanticIntent : optional intent
~~~

The diagram abbreviates nullable annotations and collection element detail. The C# source is authoritative.

## Important boundaries

- Root entries request bundle identities and versions; they are not themselves resolved bundle objects.
- The optional schedule is dependency metadata, not the FSM_API process scheduler.
- The optional load plan describes identity/stage metadata. Constructing a manifest does not localize or load artifacts.
- The host supplies context and intent; this type does not infer either.

## Use cases

1. Request one or more root MicroBundles by ID and version.
2. Supply optional host-owned state context or semantic intent when the host has those values.
3. Attach schedule/load-plan metadata for validation or host planning without assuming that it activates deferred loading.




---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
