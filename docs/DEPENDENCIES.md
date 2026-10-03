# FSM_COS Dependencies and Boundaries

This document answers a deliberately narrow question:

> **Why does FSM_COS depend on the packages it references, and why does it deliberately not depend on the packages that surround it?**

The goal is not to restate another package's theory. Each neighboring package should explain its own domain. This document explains the **FSM_COS side of the relationship**.

## Runtime dependency graph

```text
                    MicroBundleDomain
                           │
                           │ canonical capability contract
                           ▼
FSM_API ───────────────► FSM_COS
 state/context            composition
                             │
                             ▼
                       RuntimeAssembly
                             │
                             ▼
                         host/runtime
```

Both runtime package references point **into** FSM_COS.

Neither dependency points back upward into the composition host.

That is the dependency direction we want across the ecosystem.

---

## FSM_API

**Package:** `TheSingularityWorkshop.FSM_API`  
**Current FSM_COS reference:** `1.0.13`

### Why FSM_COS uses it

FSM_COS uses the FSM_API state/context abstraction at the composition boundary where a host may supply state context to the arbitration environment.

The dependency gives FSM_COS a neutral way to carry compatible state context without making FSM_COS responsible for the FSM engine itself.

### What FSM_COS does not own

FSM_COS does not:

- define FSM states or transitions;
- tick FSM_API processing groups;
- manage FSM instances;
- replace FSM_API lifecycle semantics;
- require a particular application update loop.

Those responsibilities remain with FSM_API and its consumers.

**Read FSM_API for state-machine behavior. Read FSM_COS for how state context can participate at the composition boundary.**

## FSM_UserIO

**Package:** `TheSingularityWorkshop.FSM_UserIO`  
**Current FSM_COS reference:** `0.1.0-alpha.1`

### Why FSM_COS uses it

FSM_COS consumes the platform-neutral `SemanticIntent` boundary as part of the runtime request and assembly handoff. A host or authoring system can associate an application-owned semantic intent with a RuntimeManifest without forcing FSM_COS to own devices, GUI, datum, execution policy, or interaction mechanics.

FSM_COS deliberately carries the intent rather than interpreting it:

```text
semantic request
      │
      ▼
  FSM_UserIO
      │
      ▼
 RuntimeManifest
      │
      ▼
   FSM_COS
      │
      ▼
RuntimeAssembly
```

The resulting `SemanticIntent` is available from `RuntimeAssembly.Intent`. The host or manifestation layer decides what the intent means operationally.

### What FSM_COS does not own

FSM_COS does not:

- define device input;
- render GUI controls;
- decide how an intent is executed;
- assign interaction policy;
- redefine the semantic-intent contract.

**FSM_UserIO defines the semantic interaction boundary. FSM_COS carries that boundary through composition.**

---

## MicroBundleDomain

**Package:** `TheSingularityWorkshop.MicroBundleDomain`  
**Current FSM_COS reference:** `1.0.1`

### Why FSM_COS uses it

MicroBundleDomain owns the canonical capability contract consumed by a composition host:

- MicroBundle identity;
- version/provider metadata;
- dependency requests;
- load context;
- arbitration context.

FSM_COS needs those contracts because it has to resolve, load, and arbitrate capabilities.

### What FSM_COS does not own

FSM_COS does not redefine:

- `IMicroBundle`;
- `MicroBundleDescriptor`;
- `MicroBundleDependencyRequest`;
- `IMicroBundleLoadContext`;
- `IMicroBundleArbitrationContext`;
- the semantic meaning of a capability's domain metadata.

This separation is important.

A capability author should be able to reference MicroBundleDomain without referencing FSM_COS. A different composition host should be able to consume the same capability.

**MicroBundleDomain answers “what is this capability?” FSM_COS answers “how do these capabilities become one runtime?”**

---

## FSM_Serialization

**Package:** `TheSingularityWorkshop.FSM_Serialization`

### Why it is not a runtime dependency

FSM_COS carries configuration as opaque data. It does not need to know whether that data originated as JSON, binary, generated code, a database record, or another representation.

When a semantic object crosses a byte boundary, FSM_Serialization owns that representation concern.

The relationship is therefore:

```text
representation
      │
      ▼
FSM_Serialization
      │
      ▼
semantic/runtime request
      │
      ▼
FSM_COS
      │
      ▼
composition
```

FSM_COS may document or integrate with serialization at a boundary, but it should not absorb serialization into the kernel.

---

## MicroBundleRepository

**Package:** `TheSingularityWorkshop.MicroBundleRepository`

### Why it is not a runtime dependency

A repository answers:

> **Where can the capability artifact be found or materialized?**

FSM_COS answers:

> **Given available capabilities and a manifest, how are they composed?**

FSM_COS therefore consumes a catalog/resolution boundary rather than a particular storage implementation.

A catalog may eventually be backed by:

- memory;
- generated registries;
- caches;
- a MicroBundle repository;
- Warehouse infrastructure;
- another organization's repository.

The composition algorithm should not change because the source of the capability changes.

---

## FSM_REST

**Package:** `TheSingularityWorkshop.FSM_REST`

### Why it is not a runtime dependency

REST describes a transport/API boundary. FSM_COS should not require a network transport to perform composition.

A host or repository adapter can use REST to retrieve manifests, discover capabilities, or communicate with another service and then hand the resulting semantic inputs to FSM_COS.

```text
REST / transport
       │
       ▼
adapter / repository
       │
       ▼
   FSM_COS
```

Transport belongs above the composition kernel.

---

## GUI packages

GUI.Core, GUI.Blazor, GUI.WPF, and future GUI packages are **manifestation layers**, not composition prerequisites.

A GUI capability may itself be a MicroBundle and therefore participate in composition.

That does not make the GUI framework a dependency of FSM_COS.

```text
FSM_COS
   │
   ▼
RuntimeAssembly
   │
   ▼
GUI MicroBundle / host
   │
   ▼
GUI manifestation
```

This distinction prevents a browser, desktop, or rendering technology from becoming part of the composition kernel.

---

## ProtocolAi and GrammarAi

ProtocolAi and GrammarAi may be composed as capabilities.

They are not required to define the composition algorithm.

Therefore:

- FSM_COS can compose AI-related MicroBundles;
- AI packages can remain independently useful;
- FSM_COS does not become an AI framework;
- an AI package does not need to depend on FSM_COS merely to be composed.

This is the same dependency-direction rule applied to another domain.

---

## WebPage, AnyApp, Unity, and other hosts

These are consumers of the composition boundary.

```text
             FSM_COS
                │
                ▼
        RuntimeAssembly
                │
       ┌────────┼────────┐
       ▼        ▼        ▼
    WebPage   AnyApp   Unity/other
```

A host owns its lifecycle, execution model, rendering, interaction, and manifestation.

FSM_COS should not acquire a dependency on a host merely because that host is the first proving ground.

---

## The rule for future dependencies

Before adding a package reference to FSM_COS, ask:

1. **Does the composition algorithm require a contract from this package?**
2. **Is that contract genuinely part of the kernel boundary?**
3. **Could the concern instead be supplied through an input/adapter boundary?**
4. **Would adding the reference force a host, storage, transport, GUI, or domain implementation upward into the kernel?**
5. **Does the referenced package itself remain independent of FSM_COS?**

If the answer to the first two is no, the reference probably does not belong in FSM_COS.

If the dependency would require the referenced package to depend back on FSM_COS, stop. That is an architectural inversion.

---

## The intended shape

```text
                 domain contracts
                       │
                       ▼
                  ┌─────────┐
 state/context ──►│ FSM_COS │
                  └────┬────┘
                       │
                 composition result
                       ▼
              ┌──────────────────┐
              │ host / adapters  │
              └──────────────────┘
                 │      │      │
                 ▼      ▼      ▼
             storage  GUI   transport
```

The composition kernel should remain small because its responsibility is small.

> **A package belongs in FSM_COS when the kernel needs its contract to perform composition — not merely because a host currently uses it.**
