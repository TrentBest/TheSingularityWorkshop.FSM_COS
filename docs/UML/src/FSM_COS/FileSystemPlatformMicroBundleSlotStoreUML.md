<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# FileSystemPlatformMicroBundleSlotStore — UML neighborhood

**Evidence class:** source-checked type structure and persistence sequence. **Target:** `TheSingularityWorkshop.FSM_COS.FileSystemPlatformMicroBundleSlotStore`. **Source revision:** `development`; manually maintained.

```mermaid
flowchart TB
    Host["PlatformMicroBundleSlotIndex / host"]
    Contract["IPlatformMicroBundleSlotStore"]
    Target["FileSystemPlatformMicroBundleSlotStore"]
    Snapshot["PlatformMicroBundleSlotSnapshot"]
    Json["JSON snapshot file"]
    Lock["Exclusive lock file"]
    Temp["Unique temporary file"]
    Host -->|"Read / TryWrite"| Contract
    Contract -.->|"implemented by"| Target
    Target -->|"reads / validates"| Json
    Target -->|"serializes writers"| Lock
    Target -->|"serializes replacement"| Temp
    Target -->|"returns immutable snapshot"| Snapshot
```

## Persistence sequence

```mermaid
sequenceDiagram
    participant Index as PlatformMicroBundleSlotIndex
    participant Store as FileSystemPlatformMicroBundleSlotStore
    participant Lock as Lock file
    participant Disk as Snapshot file
    Index->>Store: TryWrite(expectedRevision, replacement)
    Store->>Lock: acquire exclusive file handle
    Store->>Disk: Read current snapshot
    alt revision mismatch
        Store-->>Index: false + current snapshot
    else revision matches
        Store->>Store: validate replacement revision = expected + 1
        Store->>Disk: write and flush unique temporary file
        Store->>Disk: atomic move over snapshot
        Store-->>Index: true + persisted snapshot
    end
    Store->>Lock: release handle
```

## Limits

The diagram represents the file-level implementation and its interface boundary. It does not claim a distributed transaction, artifact cache, encryption layer, or multi-machine shared-store guarantee beyond the filesystem's atomic rename and exclusive file-handle semantics.

See [source companion](FileSystemPlatformMicroBundleSlotStore.md), [slot-index companion](PlatformMicroBundleSlotIndex.md), and [data models and lineage](../../Standard/DATA_MODELS_AND_LINEAGE.md).

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
