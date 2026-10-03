# Consuming MicroBundles from FSM_COS

> **FSM_COS does not define the MicroBundle domain. It defines how a composition consumes MicroBundles.**

The canonical MicroBundle contract lives in **TheSingularityWorkshop.MicroBundleDomain**.

This document therefore does not attempt to explain what a MicroBundle means, how its ontology is modeled, or how its domain metadata should be designed. Those are responsibilities of the MicroBundle domain package.

Instead, this document answers the FSM_COS question:

> **How does a developer provide MicroBundles to FSM_COS and let the composition kernel use them?**

## Ownership map

```text
MicroBundleDomain
    │
    └── defines the MicroBundle contract
             │
             ▼
      MicroBundleRepository
             │
             └── locates / delivers artifacts
                         │
                         ▼
                       FSM_COS
                         │
                         ├── resolves requested roots
                         ├── discovers dependencies
                         ├── supplies optional configuration
                         ├── calls Load()
                         ├── runs arbitration
                         └── returns RuntimeAssembly
```

FSM_COS consumes the first two layers through explicit boundaries. It does not duplicate their domains.

---



## Why not just use classes?

Because ordinary classes answer a smaller question.

A class usually says:

> Here is some behavior or data.

A MicroBundle says:

> Here is a capability with a machine identity, known composition relationships, an installation boundary, configuration ownership, and a defined opportunity to reconcile with the larger runtime.

That additional contract is what makes a MicroBundle useful to a composition kernel.

The abstraction is not valuable because it wraps a class.

It is valuable because it gives a capability a **place in a larger system without requiring the capability to become the system**.

---

## The composition contract

The current domain-owned runtime contract is:

~~~csharp
public interface IMicroBundle
{
    MicroBundleDescriptor Descriptor { get; }
    ulong Id => Descriptor.Id;
    IReadOnlyList<MicroBundleDependencyRequest> Dependencies { get; }
    void Load(IMicroBundleLoadContext context);
    bool Arbitrate(IMicroBundleArbitrationContext context, int roundIndex);
}
~~~

The contract describes four responsibilities:

1. **Identity** — who is this capability?
2. **Dependencies** — what else must exist for this capability to participate?
3. **Installation** — how does the capability enter the assembled composition?
4. **Arbitration** — does this participant need the composition to change?

Notice what is absent.

There is no browser lifecycle.

There is no Unity scene lifecycle.

There is no GUI renderer.

There is no application scheduler.

There is no serializer.

That absence is intentional.

---

## Identity is composition identity

The Id is the machine-oriented identity used by FSM_COS.

Human-readable names may exist in authoring systems, catalogs, tooling, documentation, or presentation models. The composition kernel does not need to make human naming the primary runtime identity.

That distinction becomes increasingly important as the ecosystem grows.

A human may call something:

~~~text
Physics
Navigation
Forge
Workshop
~~~

The composition engine needs an identity that can be resolved deterministically.

This is one reason the Runtime Manifest is published rather than interpreted as a rich editor document.

---

## Dependencies are relationships, not ownership

A MicroBundle may request another MicroBundle:

~~~text
Workshop Experience
├── GUI
│   └── FSM_API
└── Physics
    └── FSM_API
~~~

The dependency says:

> **I require this capability to participate in the composition.**

It does not say:

> **I own this capability.**

That distinction is essential.

Ownership language tends to pull composition toward a tree of masters and subordinates. Dependency language instead describes what must coexist.

FSM_COS resolves the reachable dependency closure and prevents duplicate installation of the same identity.

A missing dependency is an explicit composition failure.

A dependency cycle is an explicit composition failure.

The kernel does not guess.

---

## Configuration

Configuration is separate from the manifest.

A host can provide an `IMicroBundleConfigurationSource`:

```csharp
public interface IMicroBundleConfigurationSource
{
    bool TryGetConfiguration(
        ulong runtimeId,
        ulong bundleId,
        string version,
        out ReadOnlyMemory<byte> configuration);
}
```

FSM_COS owns the handoff, not the storage format.

If configuration is absent, FSM_COS provides no external configuration and the MicroBundle uses its defaults.

```text
configuration exists → FSM_COS supplies it → MicroBundle interprets it

configuration absent → no external configuration → MicroBundle defaults
```

## Arbitration
 makes the MicroBundle interoperable

This is the part that deserves more attention than “bundle” usually receives.

A MicroBundle is not merely a loadable artifact.

It is a participant in **composition negotiation**.

During arbitration, a bundle can inspect the shared composition and determine whether something it owns has become unsatisfied or whether another participant has created a condition that requires a response.

That means a MicroBundle can remain independently authored while still participating in a larger runtime.

The model becomes:

~~~text
bundle A ──┐
bundle B ──┼──► shared composition ◄── host later
bundle C ──┘
~~~

No single bundle has to understand every other bundle.

The composition kernel provides the shared reconciliation point.

For the deeper interoperability argument, see [Arbitration and Convergence](ARBITRATION.md).

---

## A MicroBundle should have a bounded opinion

A useful MicroBundle has an opinion about **its own capability**.

It should not have an opinion about everything.

Good:

> “My navigation capability requires a traversable spatial model.”

Dangerous:

> “I am navigation, therefore I control the GUI, identity, physics, storage, and application lifecycle.”

The first is composition.

The second is application architecture hiding inside a bundle.

The MicroBundle abstraction is strongest when each participant has a sharp boundary and arbitration allows those boundaries to interact without collapsing them.

---

## A sealed capability cartridge

The cartridge metaphor is useful:

- the catalog locates the cartridge;
- the manifest requests it;
- FSM_COS installs it;
- dependencies identify other cartridges required by the composition;
- configuration arrives with the request;
- arbitration lets the cartridge participate in reconciliation;
- RuntimeAssembly records the stable result;
- the host decides how the capability is manifested.

The cartridge does not become the crane.

The crane does not become the warehouse.

The assembled machine does not become the host.

Each metaphor is another way of describing responsibility boundaries.

---

## Micro does not mean disposable

A MicroBundle should not be interpreted as a temporary plugin or throwaway module.

The word means that the unit is intentionally bounded.

A mature ecosystem could contain:

~~~text
Experience
 ├── identity MicroBundle
 ├── navigation MicroBundle
 ├── physics MicroBundle
 ├── GUI MicroBundle
 ├── persistence MicroBundle
 └── domain capability MicroBundle
~~~

Each can evolve independently.

The composition can evolve by changing which units are requested, which versions are resolved, and which configuration is carried.

That is substantially different from rebuilding a monolithic application every time a capability changes.

---

## What a MicroBundle must not become

A MicroBundle should not become:

- a hidden application;
- a universal service locator;
- a GUI framework;
- a host lifecycle manager;
- a serialization framework;
- a network orchestrator;
- a global configuration registry;
- a replacement for FSM_API;
- a reason for FSM_COS to know domain-specific semantics.

If a feature needs one of those things, the architecture should ask whether it belongs in another layer rather than quietly expanding the MicroBundle contract.

---

## The larger equation

The MicroBundle sits between semantic capability and composition:

~~~text
semantic capability
        │
        ▼
    MicroBundle
        │
        ├── identity
        ├── dependencies
        ├── configuration
        ├── installation
        └── arbitration
        │
        ▼
     FSM_COS
        │
        ▼
 RuntimeAssembly
~~~

That is why the MicroBundle is one of the most important concepts in FSM_COS.

It is the unit at which a capability becomes **composable without becoming sovereign**.

---

## Related concepts

- [Runtime Manifest](RUNTIME_MANIFEST.md)
- [RuntimeAssembly](RUNTIME_ASSEMBLY.md)
- [Arbitration and Convergence](ARBITRATION.md)
- [FSM_COS Theory](THEORY.md)


---

## 🔗 The Singularity Workshop

FSM_COS is one layer in a deliberately troublesome ecosystem:

- **[FSM_API](https://github.com/TrentBest/FSM_API)** — behavior and state.
- **[FSM_COS](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS)** — composition and runtime assembly.
- **[FSM_Serialization](https://github.com/TrentBest/TheSingularityWorkshop.FSM_Serialization)** — representation and the byte boundary.
- **[WebPage](https://github.com/TrentBest/WebPage)** — browser manifestation and proving ground.
- **[FSM_API_Unity](https://github.com/TrentBest/FSM_API_Unity)** — Unity manifestation.

<p align="center"><em>The Singularity Workshop — Tools for the curious, the bold, and the systemically inclined.</em><br><strong>Because state shouldn't be a mess.</strong><br><em>And because static boundaries are invitations to cause trouble.</em></p>
