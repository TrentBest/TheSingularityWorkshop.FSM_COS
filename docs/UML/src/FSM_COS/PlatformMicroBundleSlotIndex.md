<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# PlatformMicroBundleSlotIndex

**Purpose.** Allocates stable, platform-local numeric slots for canonical portable ontology identities using a durable compare-and-swap store.

**Declarations.** `PlatformMicroBundleSlot`, `PlatformMicroBundleSlotSnapshot`, `IPlatformMicroBundleSlotStore`, and `PlatformMicroBundleSlotIndex` in `src/FSM_COS/PlatformMicroBundleSlotIndex.cs`.

## Contract card

| Member | Meaning |
|---|---|
| `PlatformMicroBundleSlot(value)` | Non-zero platform-local slot value; zero is reserved. |
| `PlatformMicroBundleSlotSnapshot(revision, assignments)` | Immutable, revisioned assignment snapshot; rejects duplicate slot values and invalid ontology keys. |
| `IPlatformMicroBundleSlotStore` | Durable read and atomic compare-and-swap persistence boundary. |
| `ResolveOrAssign(ontology)` | Returns an existing slot or allocates the next slot; retries compare-and-swap conflicts up to 64 times. |
| `TryResolve(ontology, out slot)` | Looks up an existing assignment without allocating. |

## Invariants

- Ontology keys must be non-empty and have no leading or trailing whitespace.
- Slot zero is reserved; assignments are unique within one platform namespace.
- Repeated resolution returns the same slot without advancing the revision.
- New assignments increment the snapshot revision by exactly one.
- Compare-and-swap conflicts reload the winning snapshot and reuse an assignment if another writer created it.
- Platform slots are not portable identity, repository storage handles, or Domain MicroBundle IDs.

## Use cases

- **Actor:** platform runtime. **Goal:** map a portable ontology to a compact local index.
- **Restart:** a new index instance over the same store recovers existing assignments.
- **Concurrent assignment:** stale writers converge on the persisted winner or fail after the bounded retry count.

## Evidence

- Source: [PlatformMicroBundleSlotIndex.cs](../../../../src/FSM_COS/PlatformMicroBundleSlotIndex.cs)
- Tests: [PlatformMicroBundleSlotIndexTests.cs](../../../../tests/FSM_COS.Tests/PlatformMicroBundleSlotIndexTests.cs)
- Persistence adapter: [FileSystemPlatformMicroBundleSlotStore](FileSystemPlatformMicroBundleSlotStore.md)
- Identity adapter: [PlatformMicroBundleDomainBinding](PlatformMicroBundleDomainBinding.md)
- Provenance: manually authored and checked against the development source.

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
