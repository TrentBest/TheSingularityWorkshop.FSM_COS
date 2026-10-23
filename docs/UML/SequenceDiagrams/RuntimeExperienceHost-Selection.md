# SEQ-FSMCOS-003 — Prepare, select, and retire runtime experiences

**Scope:** Source-reviewed sequence for `RuntimeExperienceHost` and `RuntimeExperienceRegistry`. This is a manually authored contract diagram, not a generated call graph or runtime trace.

## Successful replacement without an activation gap

```mermaid
sequenceDiagram
    actor App as Application Host
    participant Host as RuntimeExperienceHost
    participant Composer as IFsmCos
    participant Registry as RuntimeExperienceRegistry
    participant Assembly as RuntimeAssembly

    App->>Host: ComposeAndRegister(replacementManifest)
    Host->>Composer: Execute(manifest, configurationSource)
    Composer-->>Host: RuntimeAssembly(replacement)
    Host->>Registry: TryRegister(replacement)
    Registry->>Registry: lock; TryAdd(RuntimeId, assembly)
    Registry-->>Host: true
    Host-->>App: replacement assembly

    Note over Registry: Existing active RuntimeId is unchanged.
    App->>Host: TryActivate(replacement.RuntimeId)
    Host->>Registry: TryActivate(RuntimeId)
    Registry->>Registry: lock; verify identity exists
    Registry->>Registry: set ActiveRuntimeId
    Registry-->>Host: true
    Host-->>App: true
```

## Failure and retirement paths

```mermaid
sequenceDiagram
    actor App as Application Host
    participant Host as RuntimeExperienceHost
    participant Registry as RuntimeExperienceRegistry

    App->>Host: ComposeAndRegister(manifest)
    Note over Host: Composition returns an assembly with an existing RuntimeId.
    Host->>Registry: TryRegister(assembly)
    Registry->>Registry: lock; TryAdd returns false
    Registry-->>Host: false
    Host-->>App: InvalidOperationException

    App->>Host: TryActivate(unknownRuntimeId)
    Host->>Registry: TryActivate(unknownRuntimeId)
    Registry->>Registry: lock; identity absent
    Registry-->>Host: false (selection unchanged)
    Host-->>App: false

    App->>Registry: TryRemove(activeRuntimeId)
    Registry->>Registry: lock; remove entry and clear active ID
    Registry-->>App: true
```

## Contract notes

- `ComposeAndRegister` composes before registration. A duplicate identity therefore can incur composition work before the registration exception; the existing registered object is not replaced.
- Registration does not activate. This permits composing and retaining a replacement before switching selection.
- `TryActivate` is selection metadata only. It does not execute, present, dispose, or unload MicroBundles.
- `ClearActive` clears only the selected ID. `TryRemove` removes one retained assembly and clears selection only if that assembly was active.
- `Assemblies` returns a locked, copied, read-only dictionary snapshot. Registry operations use a private lock.
- Removing a registry entry is not unloading: the domain contract does not provide general MicroBundle unload/dispose semantics here.
- The registry stores `RuntimeAssembly` instances; each assembly exposes a read-only bundle-list snapshot and runtime identity. This does not make the MicroBundle objects themselves immutable.

## Complexity notes

Let (n) be the number of retained runtime assemblies and (b) the number of bundles in an assembly.

- Dictionary registration, lookup, activation membership check, and removal are expected (O(1)) operations; this is average-case dictionary behavior, not a worst-case guarantee.
- Reading `Assemblies` copies the registry dictionary and is (O(n)) time and space.
- Constructing `RuntimeAssembly` snapshots its bundle list and is (O(b)) time and space.

## Evidence and limits

- Source: [RuntimeExperienceHost.cs](../../src/FSM_COS/RuntimeExperienceHost.cs), [RuntimeExperienceRegistry.cs](../../src/FSM_COS/RuntimeExperienceRegistry.cs), [RuntimeAssembly.cs](../../src/FSM_COS/RuntimeAssembly.cs).
- Tests: [RuntimeExperienceRegistryTests.cs](../../../tests/FSM_COS.Tests/RuntimeExperienceRegistryTests.cs) covers switching while retaining entries, duplicate IDs, failed activation preserving selection, active removal, read-only snapshots, replacement preparation, and duplicate-host registration.
- Tests were inspected as source; they were **not run** for this documentation change.
- This sequence does not claim concurrent stress-test coverage, automatic diagram generation, stale-output enforcement, or runtime-captured behavior.
