# Staged Manifest Loading

The RuntimeManifest is becoming an execution structure rather than a flat list.

The important runtime distinction is:

```text
Published
   │
   ▼
Localized
   │
   ▼
Loaded
```

- **Published** — immutable MicroBundle bytes exist in a repository.
- **Localized** — those bytes are available in local storage/cache.
- **Loaded** — the MicroBundle has been instantiated into the active FSM_COS composition.

A localized MicroBundle therefore does **not** imply an in-memory MicroBundle.

## Manifest entry

A staged entry carries three things:

```text
RuntimeManifestEntry
├── MicroBundleReference
│   ├── BundleId
│   ├── Version
│   └── ContentHash
├── BundleRequest
│   └── opaque configuration
└── ManifestLoadStage
    ├── Resident
    └── Deferred
```

The reference is an immutable identity for the published bytes. FSM_COS carries it; a repository or cache layer resolves it.

The stage is an execution hint:

- **Resident** entries participate in initial preparation.
- **Deferred** entries remain available for later Experience-owned promotion.

## Experience-owned JIT meaning

FSM_COS deliberately does not know what makes a deferred capability necessary.

Instead:

```csharp
public interface IManifestLoadEvaluator
{
    bool ShouldLoad(
        RuntimeManifestEntry entry,
        IStateContext? experienceContext);
}
```

This means an Experience can decide that a capability has become necessary without teaching FSM_COS about the domain.

For example, FSM_COS does not need to know what “mortal”, “arcade game”, or “science instrument” means. Those are Experience semantics.

## Why this matters

The resulting execution model is:

```text
load manifest
      ↓
validate published identities
      ↓
localize useful bytes
      ↓
compose resident capabilities
      ↓
evaluate deferred capabilities
      ↓
promote only when necessary
      ↓
RuntimeAssembly
```

The opening/arrival Experience can provide the presentation window during which localization and composition occur, but the presentation is not the scheduler.

The next boundary is repository-backed asynchronous localization. That layer can obtain immutable bytes from the Experience/MicroBundle repositories without forcing storage concerns into FSM_COS.

## ProtocolAI boundary

ProtocolAI is not part of this runtime state machine.

It may translate human semantic authoring values to application-owned integer identities at the authoring/initialization boundary. The published runtime representation remains compact and application-owned.

```text
human semantic value
        ↕
    ProtocolAI
        ↕
application-owned identity
        ↕
immutable published layout
        ↕
FSM_COS load plan
```

FSM_COS receives the machine-oriented result.

## Page alignment belongs downstream

The manifest can eventually preserve stable ordering and layout information discovered during authoring. Page alignment and physically efficient retrieval belong to publication/storage, however, not to the composition kernel.

That separation lets the Warehouse optimize:

```text
one fetch
   ↓
nearby immutable data becomes local
   ↓
only the required MicroBundle is loaded
```

without making FSM_COS a storage engine.
