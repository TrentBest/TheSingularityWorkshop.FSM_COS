<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# PlatformMicroBundleDomainBinding — UML neighborhood

**Evidence class:** source-checked relationships and authored use cases. **Target:** `TheSingularityWorkshop.FSM_COS.PlatformMicroBundleDomainBinding` and `PlatformMicroBundleDomainBindingSet`. **Source revision:** `development`; manually maintained.

```mermaid
flowchart TB
    Portable["Canonical ontology identity"]
    Slot["PlatformMicroBundleSlot"]
    Domain["MicroBundleDomain IMicroBundle"]
    Descriptor["MicroBundleDescriptor"]
    Binding["PlatformMicroBundleDomainBinding"]
    Set["PlatformMicroBundleDomainBindingSet"]
    ByOntology["Ontology lookup"]
    BySlot["Platform-slot lookup"]
    ById["Domain-ID lookup"]
    Portable --> Binding
    Slot --> Binding
    Domain --> Binding
    Domain --> Descriptor
    Descriptor -->|"exact ID + version validation"| Binding
    Binding --> Set
    Set --> ByOntology
    Set --> BySlot
    Set --> ById
```

## Use cases

1. Materialize a verified portable capability and explicitly record its platform slot and Domain numeric ID.
2. Resolve the same binding by ontology, platform slot, or Domain ID without treating those values as interchangeable.
3. Reject duplicate or mismatched identities before the binding set enters a staged runtime.

## Limits

This file records the identity projection boundary. It does not resolve portable artifacts, allocate slots, load MicroBundles, or establish that an artifact is trusted.

See [source companion](PlatformMicroBundleDomainBinding.md), [slot-index companion](PlatformMicroBundleSlotIndex.md), and [runtime boundary](../../RUNTIME_BOUNDARY.md).

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
