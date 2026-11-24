<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# MicroBundlePreparationContext — UML neighborhood

**Evidence class:** source-checked adapter structure and authored resolution flow. **Target:** `TheSingularityWorkshop.FSM_COS.MicroBundlePreparationContext`. **Source revision:** `development`; manually maintained.

```mermaid
flowchart TB
    Bundle["IMicroBundlePreparable"]
    Contract["Domain IMicroBundlePreparationContext"]
    Target["MicroBundlePreparationContext"]
    Closure["Resolved MicroBundle dependency closure"]
    Resolver["Host IMicroBundleProviderResolver"]
    Declarations["Version-pinned provider snapshot"]
    Handle["Typed provider handle"]
    Bundle -->|"Prepare(context)"| Contract
    Target -.->|"implements"| Contract
    Closure -->|"descriptor scan"| Target
    Target -->|"Providers"| Declarations
    Target -->|"exact reference lookup"| Resolver
    Resolver -->|"typed resolution"| Handle
```

## Resolution sequence

```mermaid
sequenceDiagram
    participant Fsm as FsmCos
    participant Context as MicroBundlePreparationContext
    participant Bundle as Preparable MicroBundle
    participant Resolver as Host provider resolver
    Fsm->>Context: create after dependency closure validation
    Fsm->>Bundle: Prepare(context), stable bundle-ID order
    Bundle->>Context: inspect Providers
    Bundle->>Context: TryGetProvider<T>(exact reference)
    Context->>Context: validate owner/version/declaration
    alt exact declaration and resolver available
        Context->>Resolver: TryResolve<T>(reference)
        Resolver-->>Context: typed handle or no match
    else invalid / undeclared / no resolver
        Context-->>Bundle: false + null
    end
    Fsm->>Fsm: after all Prepare callbacks, begin Load phase
```

## Limits

This adapter does not define `IDiegeticGUI`, GUI rendering, creator trust, or provider execution. Those belong to the GUI/authoring host and its adapters. The source-mode contract is intentionally separate from the public package build until the MicroBundleDomain package dependency is advanced.

See [source companion](MicroBundlePreparationContext.md), [composition sequence](../../SequenceDiagrams/FsmCos-Execute.md), and [typed provider integration](../../../CONSUMING_MICROBUNDLES.md).

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
