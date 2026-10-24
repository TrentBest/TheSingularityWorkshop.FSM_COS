<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# UC-FSMCOS-002 — Prepare and switch runtime experiences

**Category:** User Goals  
**Status:** Implemented host-facing selection metadata; manually documented from source and tests.

## Goal

As an application host, I want to compose and retain a replacement runtime experience before selecting it, so I can prepare the next experience without discarding the currently selected one.

## Actors and scope

- **Primary actor:** Application host.
- **Supporting types:** `RuntimeExperienceHost`, `IFsmCos`, `RuntimeExperienceRegistry`, `RuntimeAssembly`.
- **Out of scope:** Activating bundle behavior, presentation, scheduling execution, unloading/disposal, and deferred bundle promotion. Selection is registry metadata; the host remains responsible for actual execution and presentation.

## Preconditions

- The host has an `IFsmCos` composer.
- The requested manifest is acceptable to the composition implementation.
- The replacement's runtime identity is not already registered.

## Main success path

1. The host asks `RuntimeExperienceHost.ComposeAndRegister` to compose the replacement manifest.
2. The host delegates composition to `IFsmCos.Execute`.
3. The composer returns a `RuntimeAssembly`.
4. The host asks the registry to retain the assembly.
5. The registry atomically adds it under its `RuntimeId` if that identity is not already present.
6. The new assembly is returned while the existing active selection remains unchanged.
7. When ready to switch, the host calls `TryActivate` with the replacement runtime ID.
8. The registry verifies that the ID is retained and updates `ActiveRuntimeId`.
9. Both assemblies remain registered; the selected ID now points to the replacement.

## Alternate and failure paths

- **Composition fails:** `IFsmCos.Execute` throws; the host does not reach registration. The host-facing method does not convert this into a non-throwing result.
- **Duplicate identity:** composition has already returned, but `TryRegister` rejects the duplicate. `ComposeAndRegister` throws `InvalidOperationException`; the previously registered assembly remains unchanged.
- **Unknown activation ID:** `TryActivate` returns `false` and preserves the previous selection.
- **Clear selection:** `ClearActive` sets the selected ID to null but retains all registered assemblies.
- **Remove a retained experience:** `TryRemove` returns false for an unknown ID. On success, it removes that registry entry; if it was active, the selection is cleared. Other entries remain.
- **Inspect registry:** `Assemblies` provides a copied, read-only snapshot, not a live mutable view.

## Postconditions and boundaries

- A successfully registered runtime is retained by ID.
- Registration alone does not activate the runtime.
- Selection does not invoke bundle execution or presentation.
- Removing an entry does not unload or dispose its MicroBundles; the core domain contract does not define general unloading here.
- The registry's lock protects its own membership and active-ID operations. This use case does not assert that the MicroBundle implementations themselves are thread-safe.

## Related source, workflow, and tests

- Source: [RuntimeExperienceHost](../../../../src/FSM_COS/RuntimeExperienceHost.cs), [RuntimeExperienceRegistry](../../../../src/FSM_COS/RuntimeExperienceRegistry.cs), [RuntimeAssembly](../../../../src/FSM_COS/RuntimeAssembly.cs).
- Sequence: [SEQ-FSMCOS-003 — Prepare, select, and retire runtime experiences](../../SequenceDiagrams/RuntimeExperienceHost-Selection.md).
- Tests: [RuntimeExperienceRegistryTests.cs](../../../../tests/FSM_COS.Tests/RuntimeExperienceRegistryTests.cs).

The linked tests exercise multi-experience retention and switching, duplicate identity, failed activation, removal of the active experience, immutable membership snapshots, and composing a replacement before activation. They were reviewed as source, not executed as part of this documentation change.

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
