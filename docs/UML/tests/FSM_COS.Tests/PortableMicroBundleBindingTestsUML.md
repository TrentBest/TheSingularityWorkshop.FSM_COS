<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# PortableMicroBundleBindingTests — UML neighborhood

**Evidence class:** executable source tests and authored test relationships. **Target:** FSM_COS.Tests.PortableMicroBundleBindingTests. **Source revision:** development; manually maintained.

```mermaid
flowchart TB
    Portable["IPortableMicroBundle"] --> Declared["PortableIdentity + PortableDependencies"]
    Declared --> Bindings["PlatformMicroBundleDomainBindingSet"]
    Bindings --> Slots["Unique platform slots"]
    Bindings --> IDs["Unique Domain runtime IDs"]
    Declared --> Projection["Dependency projection validation"]
    Projection --> Descriptor["MicroBundleDescriptor.Dependencies"]
    Projection --> Requests["IMicroBundle.Dependencies"]
    Projection --> Config["Exact configuration bytes"]
    Projection --> Digest["Optional verified SHA-256"]
    Projection --> Gate{"All checks pass?"}
    Gate -->|"yes"| Activate["Provider preparation / Load"]
    Gate -->|"no"| Reject["Fail before activation"]
```

## Use cases

1. Execute a valid portable dependency closure using explicit slot-to-Domain bindings.
2. Reject an absent binding, identity mismatch, version mismatch, digest mismatch, or configuration mismatch.
3. Verify that rejected compositions invoke no bundle Load callback.

## Limits

These tests verify host-side projection and pre-activation validation. They do not verify repository retrieval, cryptographic publisher identity, or the materializer's byte-hashing implementation.

See [test companion](PortableMicroBundleBindingTests.md), [binding contract](../../src/FSM_COS/PlatformMicroBundleDomainBinding.md), and [binding UML](../../src/FSM_COS/PlatformMicroBundleDomainBindingUML.md).

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
