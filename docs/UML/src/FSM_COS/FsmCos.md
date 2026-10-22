![The Singularity Workshop animated moniker](https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Renderer/development/docs/assets/moniker.svg)

# FsmCos

**Source:** [`src/FSM_COS/FsmCos.cs`](../../../src/FSM_COS/FsmCos.cs)  
**Interface:** [`IFsmCos.cs`](../../../src/FSM_COS/IFsmCos.cs)  
**Source baseline:** `326a021498156f2bb7dc7a31219d6118d3427e37`  
**Source file blob:** `42b016f84c75ea6e599259d5b02ee8aede95a895`  
**Authorship:** manually authored and checked against the current `development` source; not GUI.SVG-generated.

## Purpose

`FsmCos` is the composition engine. It takes a runtime manifest, resolves requested MicroBundles and their dependency closure through the domain-owned catalog, validates identity and dependency constraints, performs bounded arbitration, and returns a `RuntimeAssembly` when composition succeeds.

## UML class card

~~~mermaid
classDiagram
    class IFsmCos {
        <<interface>>
        +Execute(manifest, configurationSource) RuntimeAssembly
    }
    class FsmCos {
        +DefaultMaximumArbitrationRounds$
        +DefaultMaximumBundleCount$
        +DefaultMaximumDependencyRequestCount$
        +DefaultMaximumConfigurationBytes$
        +FsmCos(catalog, maximumArbitrationRounds)
        +FsmCos(catalog, maximumArbitrationRounds, maximumBundleCount, maximumDependencyRequestCount)
        +FsmCos(catalog, maximumArbitrationRounds, maximumBundleCount, maximumDependencyRequestCount, maximumConfigurationBytes)
        +Execute(manifest, configurationSource) RuntimeAssembly
    }
    class IMicroBundleCatalog {
        <<interface>>
        +TryResolve(bundleId, version, bundle) bool
        +TryResolve(bundleId, bundle) bool
    }
    class IMicroBundleConfigurationSource {
        <<interface>>
        +TryGetConfiguration(runtimeId, bundleId, version, configuration) bool
    }
    class RuntimeManifest
    class RuntimeAssembly
    class IMicroBundle

    FsmCos ..|> IFsmCos
    FsmCos --> IMicroBundleCatalog : required catalog
    FsmCos ..> IMicroBundleConfigurationSource : optional configuration
    FsmCos ..> RuntimeManifest : consumes
    FsmCos ..> RuntimeAssembly : returns
    RuntimeAssembly o-- "0..*" IMicroBundle : assembled bundles
~~~

The diagram abbreviates parameter and nullable annotations for readability. For exact signatures, the linked C# source is authoritative.

## Important boundaries

- **Catalog vs. composition:** `IMicroBundleCatalog` resolves domain-owned bundles. `FsmCos` validates and composes them; it does not own the domain's MicroBundle implementation.
- **Configuration:** the optional `IMicroBundleConfigurationSource` provides per-runtime, per-bundle configuration. It is not a repository or artifact-localization API.
- **Arbitration:** the default maximum is 10 rounds. Bounded convergence is a resource/safety boundary, not proof that arbitrary bundle semantics are compatible.
- **Returned assembly:** `RuntimeAssembly` is a result model for the host. It is not a UI renderer, process scheduler, or rollback boundary.

## Use cases

1. A host requests a runtime by supplying a `RuntimeManifest`.
2. A host supplies optional configuration for a particular runtime and resolved bundle version.
3. A test injects a catalog and deliberately small resource limits to verify rejection of pathological inputs.

## Verification status

This companion is source-reviewed against the baseline recorded above. Its Mermaid rendering, automated link checking, and inclusion in the package's generated documentation artifacts have **not** been verified yet.

---

## 🔗 Resources & Support

- [FSM_COS](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS) · [FSM_API](https://github.com/TrentBest/FSM_API) · [FSM_Serialization](https://github.com/TrentBest/TheSingularityWorkshop.FSM_Serialization) · [WebPage](https://github.com/TrentBest/WebPage)
- [GitHub](https://github.com/TrentBest) · [Coder Legion](https://coderlegion.com/) · [Profile](https://coderlegion.com/user/The%2BSingularity%2BWorkshop) · [Patreon](https://www.patreon.com/c/TheSingularityWorkshop) · [PayPal](https://www.paypal.com/donate/?hosted_button_id=3Z7263LCQMV9J)

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar and brand image" width="200" /></a></p>
<p align="center"><em>The Singularity Workshop — Tools for the curious, the bold, and the systemically inclined.</em><br /><strong>Because state shouldn't be a mess.</strong><br /><em>And because static boundaries are invitations to cause trouble.</em></p>
