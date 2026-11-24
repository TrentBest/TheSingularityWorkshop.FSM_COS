<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# FileSystemPlatformMicroBundleSlotStore

**Purpose.** Durable filesystem implementation of the platform-local slot compare-and-swap store. It persists identity metadata only; it does not store MicroBundle artifact bytes.

**Declaration.** `public sealed class FileSystemPlatformMicroBundleSlotStore : IPlatformMicroBundleSlotStore` in `src/FSM_COS/FileSystemPlatformMicroBundleSlotStore.cs`.

## Contract card

| Member | Meaning |
|---|---|
| Constructor(root) | Resolves and creates the storage directory; derives snapshot and lock paths. |
| `Read()` | Returns an empty revision-0 snapshot when no file exists; otherwise parses and validates the persisted JSON snapshot. |
| `TryWrite(expectedRevision, replacement, out current)` | Serializes writers with an exclusive lock, compares the persisted revision, requires exactly one revision increment, then atomically replaces the snapshot. |

## Persistence invariants

- The JSON document has only `Revision` and `Assignments` root properties.
- Revision is a non-negative integer; each assignment is an unsigned, non-zero slot.
- Duplicate JSON properties, duplicate ontology keys, unknown root properties, and invalid slot values are rejected.
- Writers acquire an exclusive lock file, re-read under the lock, and return the latest snapshot on compare-and-swap conflict.
- The replacement is serialized to a unique temporary file, flushed to disk, and moved over the snapshot on the same filesystem.
- The store persists platform-local identity mappings, not MicroBundle payloads or Domain IDs.

## Use cases

- **Actor:** platform host. **Goal:** retain ontology-to-slot assignments across process restarts.
- **Concurrent writers:** one writer commits the expected revision; stale writers receive the latest persisted snapshot and may retry through `PlatformMicroBundleSlotIndex`.
- **Corrupt or unsupported document:** reading fails closed with invalid-data diagnostics rather than silently resetting identity.

## Evidence

- Source: [FileSystemPlatformMicroBundleSlotStore.cs](../../../../src/FSM_COS/FileSystemPlatformMicroBundleSlotStore.cs)
- Tests: [PlatformMicroBundleSlotIndexTests.cs](../../../../tests/FSM_COS.Tests/PlatformMicroBundleSlotIndexTests.cs)
- Related contract: [IPlatformMicroBundleSlotStore](../../../../src/FSM_COS/PlatformMicroBundleSlotIndex.cs)
- Provenance: manually authored and checked against the development source.

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
