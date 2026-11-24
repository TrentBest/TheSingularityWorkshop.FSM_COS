<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# MicroBundlePreparationContext

**Purpose.** Internal FSM_COS adapter implementing the domain-owned preparation context over the validated resolved dependency closure and a host-supplied typed provider resolver.

**Declaration.** `internal sealed class MicroBundlePreparationContext : IMicroBundlePreparationContext` in `src/FSM_COS/MicroBundlePreparationContext.cs`.

## Contract card

| Member | Meaning |
|---|---|
| Constructor(runtimeId, bundles, providerResolver) | Captures the runtime ID, resolved bundle set, and optional host resolver. |
| `RuntimeId` | Runtime currently being composed. |
| `Providers` | Immutable deterministic snapshot of version-pinned provider declarations across the resolved closure, ordered by bundle ID then provider ID. Legacy unversioned declarations are omitted. |
| `TryGetProvider<TProvider>(reference, out provider)` | Resolves only when the reference is valid, its owner is in the closure, bundle version matches, and provider ID/contract version are declared exactly. |

## Resolution rules

1. The reference must have non-zero owner ID and non-empty owner version, provider ID, and contract version.
2. The owner bundle must be in the resolved dependency closure.
3. The owner bundle's descriptor version must match exactly.
4. Its descriptor must declare the exact provider ID and contract version.
5. A host resolver must be present and return a non-null object assignable to the requested type.
6. Failed resolution returns `false` and a null output.

Preparation is invoked for all `IMicroBundlePreparable` bundles in deterministic bundle-ID order after dependency validation and before any `Load` callback. The context is read-only; it does not execute providers or own their lifecycle.

## Use cases

- **Actor:** a preparable MicroBundle. **Goal:** discover provider declarations and request a typed capability handle before load.
- **Unknown, unversioned, or stale reference:** fail closed without calling the host resolver.
- **No resolver:** provider discovery remains available, but typed resolution returns false.

## Evidence

- Source: [MicroBundlePreparationContext.cs](../../../../src/FSM_COS/MicroBundlePreparationContext.cs)
- Domain contract: [IMicroBundlePreparationContext](../../../../_dependencies/MicroBundleDomain/IMicroBundlePreparationContext.cs) (development-source integration only; the public package dependency is not yet advanced)
- Tests: [ProviderPreparationTests.cs](../../../../tests/FSM_COS.Tests/ProviderPreparationTests.cs)
- Provenance: manually authored and checked against the development source.

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
