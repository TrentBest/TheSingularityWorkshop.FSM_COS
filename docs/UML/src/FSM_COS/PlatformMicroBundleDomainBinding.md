<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# PlatformMicroBundleDomainBinding

**Purpose.** Records an explicit relationship between a portable ontology identity, a platform-local slot, a Domain-owned numeric MicroBundle ID, and the materialized MicroBundle object.

**Declarations.** `PlatformMicroBundleDomainBinding` and `PlatformMicroBundleDomainBindingSet` in `src/FSM_COS/PlatformMicroBundleDomainBinding.cs`.

## Contract card

| Member | Meaning |
|---|---|
| `PlatformMicroBundleDomainBinding(...)` | Validates canonical ontology, non-zero Domain ID, descriptor identity, and exact materialized version before retaining the binding. |
| `PlatformMicroBundleDomainBindingSet(bindings)` | Builds immutable lookup maps and rejects duplicate ontology identities, platform slots, or Domain IDs. |
| `TryGetByOntology(...)` | Looks up by canonical portable ontology. |
| `TryGetByPlatformSlot(...)` | Looks up by platform-local slot. |
| `TryGetByDomainBundleId(...)` | Looks up by Domain runtime identity. |
| `Bindings` | Returns a read-only snapshot of all bindings. |

## Identity invariants

- The platform slot and Domain ID belong to separate namespaces.
- The binding requires both `bundle.Id` and `bundle.Descriptor.Id` to equal the explicitly supplied Domain ID.
- The materialized descriptor version must exactly equal the requested version.
- The constructor does not rewrite the descriptor or infer a numeric identity from the portable ontology.
- The set provides three lookup indexes over the same immutable binding records.

## Use cases

- **Actor:** platform materializer. **Goal:** turn verified portable identity into an explicit runtime object binding.
- **Identity mismatch:** construction fails rather than silently equating the platform slot and Domain ID.
- **Duplicate assignment:** set construction rejects duplicate keys in any identity namespace.

## Evidence

- Source: [PlatformMicroBundleDomainBinding.cs](../../../../src/FSM_COS/PlatformMicroBundleDomainBinding.cs)
- Tests: [PlatformMicroBundleSlotIndexTests.cs](../../../../tests/FSM_COS.Tests/PlatformMicroBundleSlotIndexTests.cs)
- Related types: [PlatformMicroBundleSlotIndex](PlatformMicroBundleSlotIndex.md), [MicroBundleReference](MicroBundleReference.md)
- Provenance: manually authored and checked against the development source.

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
