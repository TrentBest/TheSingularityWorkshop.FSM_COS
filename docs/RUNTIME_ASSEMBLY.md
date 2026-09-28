# RuntimeAssembly

> RuntimeAssembly is where FSM_COS stops owning the journey. The deeper rationale is in [FSM_COS Theory](THEORY.md#9-runtimeassembly-is-the-handoff-object).

**RuntimeAssembly is the handoff object produced by FSM_COS.**

![RuntimeAssembly handoff](assets/runtime-assembly-handoff.svg)

It is the point where composition ends and host-specific execution can begin.

## The alpha contract

The current implementation exposes:

```csharp
public sealed class RuntimeAssembly
{
    public ulong RuntimeId { get; }
    public IReadOnlyList<IMicroBundle> Bundles { get; }
    public int ArbitrationRounds { get; }
}
```

The assembly records:

- the runtime identity;
- the loaded MicroBundles;
- how many arbitration rounds were required to converge.

It does not become a host, renderer, scheduler, or Experience.

## The handoff

The flow is intentionally simple:

```text
RuntimeManifest
      │
      ▼
   FSM_COS
      │
      ▼
RuntimeAssembly
      │
      ├── WebForge → GUI → browser
      ├── Desktop Forge → native host
      ├── Unity bridge → Unity
      └── another host → its own manifestation
```

The assembly is therefore a **semantic handoff**, not a platform-specific object.

## What the host can do

Once it receives the assembly, the host can:

- inspect the installed bundles;
- connect them to host-specific services;
- construct GUI or presentation layers;
- start an Experience;
- schedule execution;
- allocate resources;
- communicate with platform APIs.

Those actions are deliberately outside FSM_COS.

## What the assembly does not promise

A RuntimeAssembly does not promise:

- that an Experience has started;
- that a GUI exists;
- that a browser is open;
- that a Unity scene exists;
- that Warehouse resources have been allocated;
- that networking has begun.

It says:

> **The requested composition was assembled and reached a stable arbitration result.**

## Why this boundary matters

If FSM_COS returned a host-specific runtime object, the composition kernel would quickly accumulate platform dependencies.

Instead:

```text
composition semantics
        ↓
RuntimeAssembly
        ↓
host semantics
        ↓
observable experience
```

That separation is what allows a single composition request to be consumed by different manifestations.

## Related concepts

- [Runtime Manifest](RUNTIME_MANIFEST.md)
- [MicroBundles](MICROBUNDLES.md)
- [Runtime Boundary](RUNTIME_BOUNDARY.md)
