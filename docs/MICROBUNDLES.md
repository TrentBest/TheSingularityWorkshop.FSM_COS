# MicroBundles in FSM_COS

> **FSM_COS consumes the MicroBundle contract; MicroBundleDomain owns that contract.**

This document explains what the composition kernel needs from a MicroBundle. It intentionally does not reproduce the full MicroBundleDomain theory.

For the complete capability model, authoring contract, definitions, and examples, see [MicroBundleDomain](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleDomain).

## The boundary

A MicroBundle is an independently defined capability.

FSM_COS needs only enough of that capability to perform composition:

```text
MicroBundleDomain
      │
      ▼
    IMicroBundle
      │
      ▼
    FSM_COS
      │
      ├── dependency closure
      ├── Load()
      └── Arbitrate()
```

The composition host should not need to understand the private meaning of every capability.

## The contract FSM_COS consumes

The domain-owned runtime contract is conceptually:

```csharp
public interface IMicroBundle
{
    MicroBundleDescriptor Descriptor { get; }

    IReadOnlyList<MicroBundleDependencyRequest> Dependencies { get; }

    void Load(IMicroBundleLoadContext context);

    bool Arbitrate(
        IMicroBundleArbitrationContext context,
        int roundIndex);
}
```

The exact contract belongs to MicroBundleDomain and may evolve there.

FSM_COS should not duplicate it.

### What each member means to FSM_COS

| Contract | FSM_COS use |
|---|---|
| `Descriptor` | Resolves/identifies the capability and exposes the domain-owned identity metadata needed at composition time. |
| `Dependencies` | Supplies the edges from which FSM_COS calculates dependency closure. |
| `Load(context)` | Gives the capability its installation opportunity after its required dependencies are available. |
| `Arbitrate(context, roundIndex)` | Lets the capability participate in bounded reconciliation of the assembled composition. |

FSM_COS orchestrates these calls. The MicroBundle owns the meaning of its own capability.

---

## Dependency requests

A MicroBundle declares what it needs using `MicroBundleDependencyRequest`.

```text
A
├── B
│   └── C
└── D
```

FSM_COS resolves the reachable closure:

```text
C → B → D → A
```

The important architectural distinction is:

> **The MicroBundle declares the relationship. FSM_COS resolves the relationship.**

A MicroBundle should not reach into the catalog and load its own dependencies.

That would turn a declarative capability contract into host-specific orchestration.

### Duplicate dependencies

If multiple paths reach the same MicroBundle identity, FSM_COS installs the identity once.

```text
A → C
B → C

A + B
  │
  └── C is installed once
```

### Cycles

Cycles are composition errors.

```text
A → B → C → A
```

FSM_COS detects the active resolution path and rejects the cycle rather than attempting to invent an ordering.

---

## Load is installation

`Load()` is not the application update loop.

It is the capability's opportunity to enter the assembled runtime after the composition engine has supplied its load context.

FSM_COS owns:

- when the load occurs;
- dependency ordering;
- the context it supplies.

The MicroBundle owns:

- what its capability needs to initialize;
- how it installs its own contribution;
- how it interprets its configuration.

That division keeps the kernel generic.

---

## Configuration is owned by the capability

Dependency requests can carry opaque configuration.

```text
A requests B
        │
        └── configuration bytes
                    │
                    ▼
              FSM_COS carries
                    │
                    ▼
                 B.Load()
                    │
                    ▼
              B interprets them
```

FSM_COS does not decide whether those bytes mean a material property, a GUI option, a protocol message, or anything else.

When representation becomes a serialization concern, use the separate [FSM_Serialization](https://github.com/TrentBest/TheSingularityWorkshop.FSM_Serialization) boundary.

---

## Arbitration is participation, not ownership

After loading, FSM_COS gives participating bundles access to the shared composition through `IMicroBundleArbitrationContext`.

A participant returns:

- `false` when its participation made no further composition change;
- `true` when another arbitration round is required.

The engine therefore provides:

```text
load
  ↓
arbitrate
  ↓
changed?
 ├── yes → another round
 └── no  → convergence
```

The current safety bound is ten rounds by default.

A MicroBundle should make arbitration idempotent where practical: once its requirement is satisfied, another pass should not manufacture a new change.

For the full convergence model, see [Arbitration and Convergence](ARBITRATION.md).

---

## What FSM_COS deliberately does not ask a MicroBundle to know

A MicroBundle does not need to know:

- whether the host is WebPage, AnyApp, native host, or another application;
- where its artifact was stored;
- which REST transport retrieved it;
- which GUI framework will manifest it;
- which serialization format represented its configuration;
- which application update loop will execute after assembly.

Those concerns remain outside the composition contract.

This is what makes the domain contract reusable.

---

## Authoring versus composition

MicroBundleDomain also provides a description/definition surface for tooling.

FSM_COS does not need to consume every authoring concept.

```text
MicroBundleDomain
├── runtime contract ─────► FSM_COS
└── description contract ─► tooling / Forge / GUI adapters
```

This is another deliberate boundary: **the composition kernel consumes runtime semantics; authoring tools can consume richer description semantics.**

---

## The practical rule

When adding a new MicroBundle:

1. implement the MicroBundleDomain contract;
2. declare dependencies through domain-owned dependency requests;
3. let the composition host resolve those dependencies;
4. keep configuration meaning inside the capability domain;
5. make arbitration changes explicit and convergent;
6. do not reference FSM_COS merely because FSM_COS is the current composition host.

> **MicroBundleDomain defines the participant. FSM_COS orchestrates the participants.**

