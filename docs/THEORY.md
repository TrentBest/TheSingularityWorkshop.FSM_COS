# FSM_COS Theory

<p align="center">
  <img src="assets/fsm-cos-system.svg" alt="FSM_COS composition kernel between authoring and host manifestation">
</p>

FSM_COS is the **platform-neutral computation and composition kernel** of The Singularity Workshop ecosystem.

It exists because there is a meaningful architectural operation between:

> **“Here is the runtime I am asking for.”**

and:

> **“Here is the system that will execute or manifest that computation.”**

That operation is **composition**.

FSM_COS takes a published request, discovers the MicroBundles required to satisfy it, accepts optional external configuration through a supplied boundary, loads the reachable composition, gives that composition an opportunity to reconcile itself, and returns a stable `RuntimeAssembly`.

It does not become the application.

It does not become the host.

It does not become the Experience.

It does not become the serializer.

It is the system that **assembles the system**.

For the architectural rationale behind this boundary, see this document together with [FSM_COS Architecture](ARCHITECTURE.md), [Runtime Manifest Theory](MANIFEST_THEORY.md), and [Runtime Boundary](RUNTIME_BOUNDARY.md).

---

## 1. What does “composition of systems” mean?

FSM_COS is intentionally **not an application type**. It does not assume that the computation being assembled is a WebPage, WebApp, desktop application, distributed service, game, spreadsheet, simulation, or any other particular product form.

The engineering abstraction is the computation and the common operational work required to assemble it. A host may later turn that computation into any of those things, or into something with no presentation layer at all.

The name **FSM_COS** is intentionally broader than “bundle loader.”

A loader answers:

> “Can I load this thing?”

A composition system answers:

> “Given a requested computation and its relationships, what complete set of components must exist together, in what dependency-respecting arrangement, with what optional configuration, before the result can be handed to something else?”

That distinction matters.

A MicroBundle is not useful merely because it can be loaded. It is useful because it can participate in a larger composition.

Conceptually:

```text
                 requested system
                       │
                       ▼
              ┌─────────────────┐
              │    FSM_COS       │
              │                  │
              │ resolve          │
              │ configure        │
              │ load             │
              │ reconcile        │
              │ stabilize        │
              └────────┬─────────┘
                       │
                       ▼
                RuntimeAssembly
```

FSM_COS therefore operates on **relationships between systems**, not just individual objects.

See [Runtime Manifest Theory](MANIFEST_THEORY.md) for the deeper distinction between a published request and the assembled result.

---

## 2. The missing layer is assembly

The word **system** here means the computation being assembled, not an application category. FSM_COS is useful precisely because the same assembly mechanics can support many kinds of software.

The broader ecosystem has several important responsibilities:

| Layer | Primary responsibility |
|---|---|
| **FSM_API** | behavioral/state-machine primitives |
| **FSM_COS** | composition and runtime assembly |
| **MicroBundle** | focused capability/content/behavior |
| **FSM_Serialization** | representation and byte boundary |
| **Warehouse** | storage and delivery |
| **Host** | execution and manifestation |
| **Experience** | what is encountered |

FSM_COS is not a replacement for any of those domains. This document only summarizes how FSM_COS consumes the contracts it needs; the authoritative explanation of each dependency remains in that dependency's repository.

It is the boundary that connects a **request for a composition** to a **stable assembled composition**.

The important word is **stable**.

The result should not be handed to a host while dependency resolution is incomplete, a required bundle is missing, a dependency cycle has gone unnoticed, or arbitration is still changing the composition.

That is why FSM_COS has a convergence phase rather than simply returning the list of loaded bundles.

---

## 3. Why the manifest exists

The [Runtime Manifest](RUNTIME_MANIFEST.md) is the published request entering the composition kernel.

It is deliberately smaller than the authoring world that produced it.

Authoring systems may know:

- human-readable names;
- ontology relationships;
- variants;
- visual/editor structure;
- provenance;
- dependency graphs;
- rich configuration models;
- source assets;
- authoring-only metadata.

FSM_COS does not need all of that information at runtime.

The publication boundary turns rich intent into a compact request:

```text
rich authoring model
        │
        ▼
    validation
        │
        ▼
dependency closure
        │
        ▼
 MicroBundle IDs + requested versions
        │
        ▼
 RuntimeManifest
```

This is not “throwing information away” accidentally.

It is **publishing only what the composition kernel needs**.

See [Runtime Manifest](RUNTIME_MANIFEST.md) and [Runtime Manifest Theory](MANIFEST_THEORY.md).

---

## 4. Composition starts with roots, not a giant object graph

A manifest names the requested roots.

Suppose the request is:

```text
A
├── B
│   └── C
└── D
```

The manifest does not need to duplicate the entire graph merely to tell FSM_COS that `A` was requested.

Conceptually:

```text
RuntimeManifest
      │
      ▼
      A
     / \
    B   D
    │
    C
```

FSM_COS discovers the reachable dependency closure.

The resulting installation order is an implementation consequence of those relationships:

```text
C → B → D → A
```

This gives the composition system a crucial property:

> **The request describes what is wanted; the composition kernel derives how it must be assembled.**

Missing bundles and dependency cycles are therefore composition failures, not opportunities for the kernel to guess.

See [Architecture — Dependency Resolution](ARCHITECTURE.md#dependency-resolution).

---

## 5. MicroBundles participate in composition

**MicroBundleDomain owns the MicroBundle domain. FSM_COS owns the composition of those participants.**

FSM_COS therefore does not define:

- what a MicroBundle means;
- how its ontology is modeled;
- how its domain metadata should be designed;
- how its implementation is authored.

FSM_COS consumes the domain-owned contract to perform composition work:

```text
domain-owned MicroBundle
          │
          ▼
     catalog / resolver
          │
          ▼
       FSM_COS
          │
     ┌────┼────┐
     ▼    ▼    ▼
   Load Arbitrate dependency closure
          │
          ▼
   RuntimeAssembly
```

The same MicroBundle domain can therefore be consumed by different composition hosts.

> **MicroBundleDomain explains the participant. FSM_COS explains the relationship between participants.**

For concrete developer usage, see [Consuming MicroBundles](CONSUMING_MICROBUNDLES.md). The document focuses on what an FSM_COS developer can do with the contract; it does not attempt to redefine the MicroBundle domain.

---

## 6. Configuration is external to the manifest

The manifest says which MicroBundles and requested versions belong in the composition.

A separate configuration source says how a particular MicroBundle is configured for this runtime.

FSM_COS may consume configuration through `IMicroBundleConfigurationSource`, but it does not read configuration files or define their representation.

If configuration is absent, the MicroBundle receives no external configuration and uses its defaults.

```text
Manifest
   │
   ├── identity + version
   │
   ▼
FSM_COS
   ▲
   │
optional configuration source
```

### The serialization boundary

When configuration crosses a concrete byte representation boundary, that representation belongs to **FSM_Serialization** rather than FSM_COS.

FSM_COS can consume the resulting bytes without becoming another serialization framework.

```text
FSM_Serialization
    ↓ representation

configuration source
    ↓ availability

FSM_COS
    ↓ composition

MicroBundle
    ↓ domain interpretation
```

---

## 7. Loading is installation, not execution

A successful `Load()` means that a MicroBundle has entered the composition.

It does **not** mean:

- an Experience has started;
- a GUI has rendered;
- a browser has opened;
- a Unity scene has loaded;
- a desktop application has entered its lifecycle;
- a process has begun scheduling;
- a user has encountered anything.

This distinction is what lets the same composition travel to different hosts.

```text
             composition
                  │
             RuntimeAssembly
                  │
       ┌──────────┼──────────┐
       ▼          ▼          ▼
     browser    Unity     desktop
```

FSM_COS stops at the handoff.

See [Runtime Boundary](RUNTIME_BOUNDARY.md).

---

## 8. Arbitration is composition negotiation

Dependency loading establishes the initial reachable set.

Arbitration asks a different question:

> **Now that all reachable bundles are installed, does the composition need to change?**

Every loaded MicroBundle receives the shared composition context and can report whether its participation changed the composition.

```text
load complete
     │
     ▼
round 1 ── changed ──► round 2
                         │
                         ├── changed ──► round 3
                         │
                         └── unchanged
                                │
                                ▼
                         stable assembly
```

This is not a universal priority system.

The goal is not to produce a winner.

The goal is **convergence**.

The current alpha implementation bounds arbitration at ten rounds. If the composition does not stabilize, FSM_COS fails rather than handing the host an assembly it already knows is unstable.

See [Arbitration and Convergence](ARBITRATION.md).

Stable here means the participants reported no changes during a complete arbitration round. It is a protocol-level convergence result, not an automatic proof that every domain-specific compatibility rule, external dependency, or host behavior is correct. See [Arbitration and Convergence](ARBITRATION.md) for the exact guarantee and its limits.

---

## 9. RuntimeAssembly is the handoff object

The [RuntimeAssembly](RUNTIME_ASSEMBLY.md) is the concrete result of the composition pass.

It says, in effect:

> “The requested composition has been resolved, installed, and brought to a stable arbitration state.”

It does not say:

> “The application is now running.”

The distinction is deliberate.

```text
RuntimeManifest
      │
      ▼
   FSM_COS
      │
      ▼
RuntimeAssembly
      │
      ▼
      HOST
```

The host decides what execution or manifestation means in its environment.

This is the point at which FSM_COS stops being the owner of the journey.

---

## 10. Same computation, different manifestation

A computation should not become web-specific merely because the first proving ground is WebPage.

The same semantic request can be assembled for multiple hosts:

```text
                 Runtime Manifest
                        │
                        ▼
                     FSM_COS
                        │
                        ▼
                 RuntimeAssembly
                 /        |        \
                /         |         \
           WebPage       Unity     Desktop
              │            │          │
           browser       scene      native host
```

The host-specific behavior begins **after** composition.

This is why WebPage integration belongs in a host document rather than being baked into the kernel.

See [WebPage Integration](WEBPAGE_INTEGRATION.md).

---

## 11. What FSM_COS deliberately does not own

The boundary becomes easier to understand by looking at what is explicitly excluded.

FSM_COS does not own:

- application lifecycle;
- GUI rendering;
- browser lifecycle;
- Unity scene/object lifecycle;
- desktop lifecycle;
- Warehouse persistence;
- storage policy;
- transport protocols;
- networking;
- telemetry policy;
- metaDev adaptation;
- Experience presentation;
- arbitrary scheduling;
- domain-specific business rules;
- serialization formats.

Those concerns may participate in a larger architecture.

They should not be smuggled into the composition kernel merely because the kernel can see them.

The principle is:

> **If a concern can remain outside composition without weakening the composition contract, keep it outside.**

See [Runtime Boundary](RUNTIME_BOUNDARY.md).

---

## 12. The catalog is deliberately outside the algorithm

FSM_COS needs to resolve a bundle identity to an implementation.

It should not need to know where that implementation came from.

The catalog can therefore be supplied from outside:

```text
FSM_COS
   ▲
   │
IMicroBundleCatalog
   ▲
   │
┌──┴──────────────────────────┐
│ in-memory registry          │
│ generated catalog           │
│ cache                       │
│ Warehouse-backed resolver   │
│ domain-specific resolver    │
└─────────────────────────────┘
```

This is another form of boundary discipline.

The composition algorithm owns **what it needs**.

The surrounding system owns **how that need is fulfilled**.

That allows the same composition engine to evolve from an in-memory alpha catalog toward richer delivery infrastructure without rewriting the composition model.

---

## 13. Determinism is a goal of composition

A useful long-term property is:

> **The same semantic request, resolved against the same compatible catalog and bundle versions, should produce the same composition.**

That does not mean every host must render the result identically.

It means the composition boundary itself should be predictable.

Determinism can be undermined by:

- hidden global state;
- unspecified dependency ordering;
- inconsistent bundle identity;
- mutable configuration;
- arbitrary host callbacks;
- unstable arbitration rules.

FSM_COS therefore keeps its alpha algorithm deliberately explicit:

```text
request
  ↓
resolve
  ↓
dependency closure
  ↓
load
  ↓
arbitrate
  ↓
converge
  ↓
assemble
```

The theory is more important than the exact alpha implementation: the kernel must have a clear answer for **when composition is complete**.

---

## 14. Composition is not serialization

This distinction deserves its own statement.

A serialized manifest is a **representation** of a request.

A RuntimeManifest is the **semantic runtime request** consumed by FSM_COS.

A RuntimeAssembly is the **assembled runtime composition** produced by FSM_COS.

Those are three different things:

```text
serialized representation
        │
        │ decode / reconstruct
        ▼
  RuntimeManifest
        │
        │ compose
        ▼
  RuntimeAssembly
        │
        │ hand off
        ▼
       Host
```

The byte representation may change.

The composition semantics should not have to change merely because the representation format changes.

This is precisely why [FSM_Serialization](https://github.com/TrentBest/TheSingularityWorkshop.FSM_Serialization) is a separate package.

---

## 15. Composition is not manifestation

The crane metaphor is useful because it makes the responsibility boundary physical.

The crane:

- receives the request;
- finds the parts;
- assembles them;
- stabilizes the assembly;
- hands it off.

The crane does not become:

- the factory;
- the warehouse;
- the vehicle;
- the building;
- the user experience.

That is FSM_COS.

The crane is intentionally static in the architecture documentation. It is a symbol for the composition boundary: receive the request, gather the parts, stabilize the assembly, and hand it off. The SVG remains the canonical explanatory artifact.

> **FSM_COS assembles the machine. It does not become the machine.**

---

## 16. The architectural equation

The entire kernel can be reduced to:

```text
published manifest
       +
reachable MicroBundles
       +
optional external configuration
       +
dependency ordering
       +
arbitration
       =
stable RuntimeAssembly
```

Or, more simply:

```text
FSM_COS = computation request → composition → stable assembly
```

Not:

```text
FSM_COS = application framework
```

Not:

```text
FSM_COS = serializer
```

Not:

```text
FSM_COS = host
```

Not:

```text
FSM_COS = Experience
```

Its value is precisely that it stops before those responsibilities.

---

## 17. The invariant

The alpha implementation is intentionally small.

The architectural boundary should remain recognizable even when the surrounding ecosystem becomes much larger:

```text
authoring
   │
   ▼
Runtime Manifest
   │
   ▼
┌───────────────────────────────┐
│            FSM_COS            │
│                               │
│ resolve → load → arbitrate    │
│            ↓                  │
│      RuntimeAssembly          │
└───────────────┬───────────────┘
                │
                ▼
              host
                │
                ▼
          manifestation
```

The invariant is:

> **FSM_COS assembles what was requested, returns a stable composition, and hands that composition to something else.**

That is the theory.


---

## Living architecture

The documentation follows the architecture it describes: **static definitions can participate in dynamic behavior**.

The visual system therefore has a semantic motion vocabulary:

| Motion | Meaning |
|---|---|
| **Lift / lower** | work being performed by composition |
| **Pulse** | an active capability or boundary |
| **Travel** | a request or dependency moving between layers |
| **Repetition** | arbitration rounds |
| **Settling** | convergence |
| **Docking / handoff** | composition becoming available to a host |

The rule is simple: **animate the concept, not the decoration**.

A static blueprint remains the canonical explanatory artifact. A living companion makes the same model easier to *feel*. They are two manifestations of one semantic model, not competing representations.


---

## 🔗 The Singularity Workshop

FSM_COS is one layer in a deliberately troublesome ecosystem:

- **[FSM_API](https://github.com/TrentBest/FSM_API)** — behavior and state.
- **[FSM_COS](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS)** — composition and runtime assembly.
- **[FSM_Serialization](https://github.com/TrentBest/TheSingularityWorkshop.FSM_Serialization)** — representation and the byte boundary.
- **[WebPage](https://github.com/TrentBest/WebPage)** — browser manifestation and proving ground.
- **[FSM_API_Unity](https://github.com/TrentBest/FSM_API_Unity)** — Unity manifestation.

<p align="center"><em>The Singularity Workshop — Tools for the curious, the bold, and the systemically inclined.</em><br><strong>Because state shouldn't be a mess.</strong><br><em>And because static boundaries are invitations to cause trouble.</em></p>
