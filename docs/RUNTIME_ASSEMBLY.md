<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop — animated workshop moniker" width="720" /></a></p>

# RuntimeAssembly

> RuntimeAssembly is where FSM_COS stops owning the journey. The deeper rationale is in [FSM_COS Theory](THEORY.md#9-runtimeassembly-is-the-handoff-object).

**RuntimeAssembly is the handoff object produced by FSM_COS.**

![RuntimeAssembly handoff: a manifest request enters FSM_COS, which returns a stable RuntimeAssembly to a host responsible for GUI, browser, Unity, and Experience behavior.](assets/runtime-assembly-handoff.svg)

*The graphic shows the implemented boundary, not an execution pipeline: `FsmCos.Execute` validates the manifest, resolves and loads the requested dependency closure, arbitrates to convergence, and returns `RuntimeAssembly`. The host-specific examples on the right are responsibilities outside this package.*

It is the point where composition ends and host-specific execution can begin.


## UML class diagram: assembly handoff

This is a simplified, source-derived overview of the handoff relationships, not a complete per-type API inventory. Method signatures are abbreviated for legibility; consult the source and contract sections below for exact generic overloads, parameter types, and nullability.

~~~mermaid
classDiagram
    class FsmCos {
        +Execute(manifest, configurationSource) RuntimeAssembly
    }
    class RuntimeManifest {
        +RuntimeId: ulong
        +Bundles: IReadOnlyList
        +Intent: SemanticIntent
    }
    class RuntimeAssembly {
        +RuntimeId: ulong
        +Bundles: IReadOnlyList~IMicroBundle~
        +LoadedBundles: IReadOnlyList~IMicroBundle~
        +ArbitrationRounds: int
        +Intent: SemanticIntent
        +TryGetBundle(bundleId, bundle) bool
        +TryGetBundle~TBundle~(bundleId, bundle) bool
        +TryGetBundle~TBundle~(bundle) bool
    }
    class IMicroBundle {
        <<MicroBundleDomain contract>>
        +Id: ulong
        +Descriptor
        +Load(context)
        +Arbitrate(context, roundIndex) bool
    }
    class Host {
        <<outside FSM_COS>>
    }

    FsmCos ..> RuntimeManifest : consumes
    FsmCos ..> RuntimeAssembly : returns after convergence
    RuntimeAssembly o-- "0..*" IMicroBundle : exposes loaded capabilities
    Host --> RuntimeAssembly : receives handoff
~~~

RuntimeAssembly is a snapshot of the loaded MicroBundle references and the converged round index. It does not start an Experience, tick FSM_API process groups, render UI, or roll back side effects from failed bundle code.

It is the point where composition ends and host-specific execution can begin.

## The alpha contract

The current implementation exposes:

```csharp
public sealed class RuntimeAssembly
{
    public ulong RuntimeId { get; }
    public IReadOnlyList<IMicroBundle> Bundles { get; }
    public IReadOnlyList<IMicroBundle> LoadedBundles { get; }
    public int ArbitrationRounds { get; }
    public SemanticIntent? Intent { get; }

    public bool TryGetBundle(ulong bundleId, out IMicroBundle? bundle);
    public bool TryGetBundle<TBundle>(ulong bundleId, out TBundle? bundle)
        where TBundle : class, IMicroBundle;
    public bool TryGetBundle<TBundle>(out TBundle? bundle)
        where TBundle : class, IMicroBundle;
}
```

The assembly exposes both `Bundles` and the compatibility alias `LoadedBundles`; both return the same read-only snapshot. The `TryGetBundle` overloads locate an already loaded bundle by ID, by ID plus expected CLR type, or by expected CLR type alone. They do not load bundles or resolve provider declarations.

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

### Provider declarations are not provider invocation

A loaded bundle's `MicroBundleDescriptor.Providers` advertises provider IDs. The descriptor does not contain the provider instances, their CLR contract types, semantic contract versions, or an invocation mechanism. `RuntimeAssembly.TryGetBundle(...)` locates a loaded MicroBundle; it does not resolve a provider ID declared by that bundle.

Therefore the host or a separately authored integration MicroBundle must explicitly resolve and invoke providers. It should validate the expected semantic contract and compatibility before invocation; matching strings alone are not proof of compatibility. Keep domain-specific policy—such as Physics contact evidence to a Gestures recovery recommendation—outside this composition kernel.

The provider descriptor and provider-resolution semantics belong to [MicroBundleDomain](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleDomain); FSM_COS does not define or invoke a provider merely because its ID appears in a descriptor.

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

The assembly is a stable handoff snapshot, not a live update manager. The current API does not provide in-place replacement, unload/dispose, generation reference tracking, or automatic retirement. The host owns those lifecycle decisions; they are outside the current FSM_COS contract.

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

---

## 🔗 Resources & Support

- **Project:** [FSM_COS](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS)
- **Workshop projects:** [FSM_API](https://github.com/TrentBest/FSM_API) · [FSM_Serialization](https://github.com/TrentBest/TheSingularityWorkshop.FSM_Serialization) · [WebPage](https://github.com/TrentBest/WebPage)
- **Connect:** [GitHub](https://github.com/TrentBest) · [Coder Legion profile](https://coderlegion.com/user/The%2BSingularity%2BWorkshop) · [Coder Legion community](https://coderlegion.com/)
- **Support:** [Patreon](https://www.patreon.com/c/TheSingularityWorkshop) · [PayPal](https://www.paypal.com/donate/?hosted_button_id=3Z7263LCQMV9J)

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
