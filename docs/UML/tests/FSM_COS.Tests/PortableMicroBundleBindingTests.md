<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# PortableMicroBundleBindingTests

**Purpose.** Verifies that portable MicroBundle dependency declarations are reconciled with platform-local slots, Domain runtime IDs, descriptor dependencies, and runtime dependency requests before activation.

**Declarations.** PortableMicroBundleBindingTests in tests/FSM_COS.Tests/PortableMicroBundleBindingTests.cs.

## Verification matrix

| Test | Invariant |
|---|---|
| Execute_requires_explicit_binding_set_for_portable_bundles | Portable bundles cannot enter activation without an explicit binding set. |
| Execute_validates_portable_projection_before_loading_the_dependency_closure | A valid portable dependency closure maps to exact numeric dependencies and loads in dependency order. |
| Execute_rejects_runtime_configuration_that_differs_from_portable_declaration_before_loading | Runtime request configuration must byte-match the portable declaration. |
| Execute_rejects_an_unbound_portable_dependency_before_loading | Every portable dependency must map to a binding and appear in the resolved closure. |
| Execute_rejects_portable_digest_that_differs_from_materializer_verified_digest | An optional portable SHA-256 pin must match the materializer-verified digest. |

## Activation gate

The tests exercise the public FsmCos.Execute overload that accepts PlatformMicroBundleDomainBindingSet. Validation runs after dependency resolution and before provider preparation and bundle loading. Failure assertions require every affected bundle's LoadCalls to remain zero.

## Evidence

- Source: [PortableMicroBundleBindingTests.cs](../../../../tests/FSM_COS.Tests/PortableMicroBundleBindingTests.cs)
- Implementation: [PlatformMicroBundleDomainBinding.cs](../../src/FSM_COS/PlatformMicroBundleDomainBinding.cs)
- Related contract: [IPortableMicroBundle](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleDomain/blob/development/IPortableMicroBundle.cs)
- Provenance: authored against the development source; CI result is tracked by the repository workflow.

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
