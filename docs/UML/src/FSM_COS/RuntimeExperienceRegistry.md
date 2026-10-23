# RuntimeExperienceRegistry

**Purpose.** Thread-safe registry retaining composed runtime assemblies and tracking a host-selected runtime identity.

**Declaration.** `public sealed class RuntimeExperienceRegistry` in `src/FSM_COS/RuntimeExperienceRegistry.cs`.

## Contract card

| Member | Meaning |
|---|---|
| `ActiveRuntimeId` | Currently selected ID, or null. |
| `Assemblies` | Read-only snapshot of retained assemblies keyed by runtime ID. |
| `TryRegister(assembly)` | Adds an assembly unless its ID already exists; does not activate it. |
| `TryGet(runtimeId, out assembly)` | Looks up a retained assembly. |
| `TryActivate(runtimeId)` | Selects an existing ID; failed selection leaves the current selection unchanged. |
| `ClearActive()` | Clears selection while retaining assemblies. |
| `TryRemove(runtimeId)` | Removes registry metadata and clears selection if needed. |

## Synchronization and ownership

Public state and dictionary operations are guarded by a private lock. The `Assemblies` property returns a copied dictionary wrapped in a read-only view, rather than exposing the live dictionary. Removing an entry does not unload or dispose MicroBundles; no general unload contract is defined here.

## Use cases

- **Actor:** host application. **Goal:** retain multiple prepared experiences and select one by stable runtime ID.
- **Unknown ID:** lookup/activation/removal returns false; activation does not change the previous selection.
- **Removal of active ID:** the registry clears the active selection but does not dispose the assembly's MicroBundles.

## Evidence

- Source: [RuntimeExperienceRegistry.cs](../../../../src/FSM_COS/RuntimeExperienceRegistry.cs)
- Provenance: source-checked against `development`; manually authored documentation.
- Related standard: [state/lifecycle](../../Standard/STATE_LIFECYCLE.md), [data models and lineage](../../Standard/DATA_MODELS_AND_LINEAGE.md).
