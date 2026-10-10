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
    public SemanticIntent? Intent { get; }
}
```

The assembly records:

- the runtime identity;
- the loaded MicroBundles;
- `ArbitrationRounds`: the zero-based index of the round that reported convergence (`0` means the first round converged), not the total number of `Arbitrate` calls;
- the optional application-owned semantic intent carried by the manifest, when supplied.

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
- [Consuming MicroBundles](CONSUMING_MICROBUNDLES.md)
- [Runtime Boundary](RUNTIME_BOUNDARY.md)


---

## 🔗 The Singularity Workshop

FSM_COS is one layer in a deliberately troublesome ecosystem:

- **[FSM_API](https://github.com/TrentBest/FSM_API)** — behavior and state.
- **[FSM_COS](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS)** — composition and runtime assembly.
- **[FSM_Serialization](https://github.com/TrentBest/TheSingularityWorkshop.FSM_Serialization)** — representation and the byte boundary.
- **[WebPage](https://github.com/TrentBest/WebPage)** — browser manifestation and proving ground.
- **[FSM_API_Unity](https://github.com/TrentBest/FSM_API_Unity)** — Unity manifestation.

<p align="center"><em>The Singularity Workshop — Tools for the curious, the bold, and the systemically inclined.</em><br><strong>Because state shouldn't be a mess.</strong><br><em>And because static boundaries are invitations to cause trouble.</em></p>
