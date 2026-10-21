# Preserved UML/class-diagram branch contract documentation

Verbatim snapshots from `work/fsm-cos-uml-class-diagrams`, 2026-10-21. These retain the branch's expanded contract explanations for reconciliation against implementation and canonical docs. Do not assume diagrams or contracts remain source-accurate without validation.


---

## docs/ARBITRATION.md
Source blob: `898faf1ba84788841fbd9eb70fc4fb32d7a827f2`

# Arbitration and Convergence

> **Arbitration gives a composition a bounded runtime reconciliation step; it does not automatically prove semantic compatibility.**

See [FSM_COS Theory](THEORY.md#8-arbitration-is-composition-negotiation) for the compact model. This document is the deeper treatment.

![Arbitration convergence: loaded bundles participate in complete rounds until a round reports no changes or the configured maximum is exhausted](assets/arbitration-convergence.svg)

*Implementation note: the successful round is recorded as a zero-based index in `RuntimeAssembly.ArbitrationRounds` (`0` means the first round converged). If every allowed round reports changes, `FsmCos.Execute` throws instead of returning an unstable assembly.*

## UML class diagram: arbitration participants

This is a simplified, source-derived overview of the arbitration relationships, not a complete per-type API inventory. Parameter types and nullability are abbreviated; the source and the contract above remain authoritative for exact signatures and failure behavior.

~~~mermaid
classDiagram
    class FsmCos {
        +Execute(manifest, configurationSource) RuntimeAssembly
    }
    class ArbitrationContext {
        +RuntimeId: ulong
        +ExperienceContext: IStateContext
        +Bundles: IReadOnlyList~IMicroBundle~
        +LoadedBundles: IReadOnlyList~IMicroBundle~
    }
    class IMicroBundle {
        <<MicroBundleDomain contract>>
        +Arbitrate(context, roundIndex) bool
    }
    class IMicroBundleArbitrationContext {
        <<MicroBundleDomain contract>>
        +RuntimeId: ulong
        +Bundles
        +ExperienceContext
    }
    class RuntimeAssembly {
        +RuntimeId: ulong
        +Bundles: IReadOnlyList~IMicroBundle~
        +ArbitrationRounds: int
    }

    FsmCos ..> ArbitrationContext : creates after loading
    ArbitrationContext ..|> IMicroBundleArbitrationContext
    IMicroBundle ..> IMicroBundleArbitrationContext : receives during each round
    FsmCos ..> RuntimeAssembly : returns only on convergence
    RuntimeAssembly o-- "0..*" IMicroBundle : loaded participants
~~~

The two interfaces shown as MicroBundleDomain contracts are external to this repository; FSM_COS supplies their concrete context and invokes the domain-owned arbitration callback. A returned assembly means a complete round reported no changes before the configured bound, not that all possible domain compatibility rules have been proven.

## The question arbitration actually answers

Dependency resolution asks:

> **What must exist?**

Arbitration asks:

> **Now that those things exist together, can the composition settle into a state in which their contracts remain satisfiable?**

That distinction is why FSM_COS is a composition system rather than a sophisticated loader.

A loader can establish an inventory. It cannot, by itself, establish that independently authored capabilities can coexist coherently once installed.

Arbitration creates a bounded reconciliation phase between **installation** and **handoff**.

~~~text
Runtime Manifest
      ↓
dependency closure
      ↓
configured installation
      ↓
ARBITRATION
      ↓
convergence
      ↓
RuntimeAssembly
~~~

The result is not a winner. The result is a **stable relationship between participants**.

---

## Why interoperability is normally difficult

Interoperability is often reduced to questions such as:

- Can component A call component B?
- Can they deserialize the same message?
- Do they share an interface?
- Can they load into the same process?

Those questions matter, but they describe mostly **pairwise compatibility**.

Real compositions are contextual.

~~~text
GUI          expects a presentation surface
Physics      expects spatial state
Navigation   expects traversable space
Identity     expects a visitor anchor
Telemetry    expects observable transitions
~~~

None of those capabilities necessarily owns the others.

Traditional application architecture often solves this by creating a larger owner:

~~~text
              MASTER APPLICATION
              /       |                  GUI     Physics   Navigation
~~~

That master becomes the place where every conflict is understood, every ordering rule is encoded, and every new capability eventually acquires another special case.

FSM_COS deliberately avoids making the composition kernel that master.

Arbitration provides another model:

~~~text
          GUI ─────────────┐
                           │
       Physics ────────────┼──► shared composition
                           │
      Navigation ──────────┤
                           │
       Identity ───────────┘
~~~

Each participant can respond to the composition it finds without requiring one universal application owner to understand every domain.

---

## Arbitration changes the interoperability concern

Without arbitration, interoperability tends to mean:

> **Can these modules be connected?**

With arbitration, interoperability can mean:

> **Can these modules participate in a shared composition and converge without requiring one module to become the universal owner?**

That is the architectural leap.

A may be compatible with B in isolation and compatible with C in isolation, while A+B+C creates a condition none of the pairwise interfaces can express.

Arbitration gives the assembled system a place to discover and reconcile that condition.

This is not merely a second initialization pass. It is the phase in which the composition answers whether it can **live together**.

---

## Arbitration is not priority arbitration

The name can suggest a judge selecting a winner. That is not the model.

FSM_COS does not define a universal hierarchy such as:

~~~text
highest priority wins
lowest priority loses
~~~

Instead:

~~~text
participant A changes something
        ↓
participant B observes the composition
        ↓
B may change something
        ↓
participant C observes the new composition
        ↓
repeat until a full round changes nothing
~~~

The composition itself is the shared object of negotiation.

A navigation bundle can establish that a route is impossible. A GUI bundle can establish that a surface must exist. A physics bundle can establish that a spatial constraint must hold. None of those facts automatically makes one bundle sovereign over the others.

Arbitration lets capabilities express consequences instead of demanding ownership.

---

## The convergence law

The alpha contract is deliberately small:

1. every reachable bundle is installed;
2. every installed bundle participates in a round;
3. a participant returns true when its participation changed the composition;
4. it returns false when it did not;
5. a complete round with no changes means convergence;
6. the engine stops at the configured maximum number of rounds;
7. failure to converge is an error.

~~~text
round 1 → changed
round 2 → changed
round 3 → unchanged
              ↓
          CONVERGED
~~~

The current ten-round bound is a safety boundary, not a universal constant. The `RuntimeAssembly.ArbitrationRounds` property records the **zero-based index** of the converging round: `0` means the first round completed without changes. Therefore, it is not the total count of calls made to `Arbitrate`; a successful assembly with value `0` still means every loaded bundle participated in one round.

The deeper invariant is:

> **No unstable composition crosses the RuntimeAssembly boundary as if it were stable.**

## What convergence proves—and what it does not

A successful arbitration pass establishes a narrow, observable fact: every loaded bundle participated, and a complete round reported no composition changes before the configured limit was reached.

It does **not**, by itself, prove that:

- the bundles' domain assumptions are correct;
- every semantic compatibility rule has been implemented;
- external resources are available;
- the host will execute or present the assembly correctly;
- a bundle has not incorrectly returned `false` while a requirement remains unsatisfied;
- the overall application is safe or correct.

Those stronger claims depend on the contracts and implementations supplied by the participants and host. A bundle must encode the compatibility conditions it knows how to evaluate, and tests must exercise those conditions. FSM_COS enforces the bounded round protocol; it cannot infer every domain-specific invariant on behalf of independently authored bundles.

**Precise guarantee:** FSM_COS does not return an assembly when the arbitration protocol reports continued change through the configured maximum. A returned assembly means the participants reported convergence—not that every possible form of interoperability has been proven.


## Example: a shared capability name is not compatibility

Imagine a tanker MicroBundle and a station MicroBundle both advertise **docking**. That shared word is discovery metadata, not evidence that the operations can safely compose.

A domain-owned docking contract might need to compare facts such as:

- docking-interface or protocol identity and compatible contract versions;
- the geometry and reference frame of each docking port;
- permitted approach and capture envelopes;
- size, mass, clearance, or load limits relevant to the operation;
- units, coordinate conventions, and timebase where those affect interpretation.

These are illustrative contract facts, **not fields that the current MicroBundle descriptor automatically supplies**. The bundle author or a domain-specific adapter must define which facts are required, where they come from, and how they are evaluated. A generic label, a matching provider name, or the fact that both bundles loaded is insufficient.

The composition should therefore distinguish three outcomes in its domain logic:

1. **Compatible:** the declared contracts and configuration provide enough evidence to permit this pairing.
2. **Incompatible:** a known contract condition fails; the pairing must not be treated as usable.
3. **Unresolved:** required evidence is absent or cannot be interpreted; absence of a known conflict is not proof of compatibility.

The runtime/experience owns the policy for what to do with an incompatible or unresolved pairing—reject it, request different bundles or configuration, or present the reason to the author. The bundles own the domain facts they can validate. The authoritative host or simulation still determines whether a permitted docking attempt is physically feasible and what actually happens.

### Keep the responsibilities separate

- **Runtime manifest:** selects MicroBundle identities and requested versions. It does not embed each bundle's configuration.
- **Configuration source:** supplies configuration separately for each selected MicroBundle. FSM_COS passes that configuration through; it does not interpret domain-specific fields.
- **MicroBundle contract and implementation:** define capabilities, requirements, and the domain-specific checks a participant can actually perform.
- **FSM_COS arbitration:** provides the bounded reconciliation phase. It does not infer a universal compatibility schema from capability names.
- **Host / experience / simulation:** owns the higher-level choice and the authoritative execution outcome.

The current arbitration callback reports whether a participant changed the composition; it is **not** a structured compatibility-diagnostics API.

The test suite now includes a deliberately domain-owned example in `FsmCosTests`: two bundles advertise the same `docking` provider label, while their test-only contract identities determine whether arbitration accepts the pairing or throws on a known conflict. This demonstrates the existing extension point, **not** a built-in FSM_COS compatibility validator or a production docking schema. The exception is a fail-closed fixture; machine-readable compatible / incompatible / unresolved results remain future contract work. If callers need machine-readable rejection categories or explanations, that needs an explicit contract extension or domain-owned result surface. Do not disguise a missing compatibility check as successful convergence, and do not add a universal schema here before real domain contracts establish what it must express.

---

## Why bounded convergence matters

An open-ended negotiation loop could permit pathological participants:

~~~text
A changes B
B changes A
A changes B
B changes A
...
~~~

Without a bound, the runtime has no principled stopping point.

With a bound:

~~~text
round 1
round 2
...
round N
  ├── stable → success
  └── unstable → failure
~~~

The host therefore gets a meaningful contract:

- RuntimeAssembly means the composition reached its stable point;
- failure means the requested composition could not establish that point.

The host does not need to invent its own interpretation of “probably stable enough.”

---

## The strongest participants are boring

A good arbitrator is not an application loop hidden inside a MicroBundle.

A good arbitrator is:

- deterministic;
- local in responsibility;
- explicit about what it changed;
- safe to run again;
- respectful of the shared composition;
- independent of the final host where possible.

The ideal shape is effectively:

~~~text
apply requirement
    ↓
already satisfied?
    ├── yes → no change
    └── no  → satisfy it → changed
~~~

That makes convergence legible and testing meaningful.

A second pass over an already-satisfied condition should not manufacture another change simply to keep the system busy.

---

## Structural composition versus behavioral reconciliation

Dependencies are usually represented as a graph:

~~~text
Experience
 ├── Navigation
 │    └── SpatialModel
 ├── GUI
 │    └── PresentationModel
 └── Identity
      └── VisitorModel
~~~

The graph establishes **what is installed**.

But Navigation may reveal a spatial interaction point. SpatialModel may expose a constraint that changes presentation. The GUI may reveal that an interaction requires an identity affordance. Identity may then change available presentation state.

The dependency graph establishes structural composition.

Arbitration establishes **behavioral reconciliation**.

That distinction is fundamental.

---

## Arbitration creates an interoperability seam

A traditional integration boundary often looks like:

~~~text
Provider API → Consumer API
~~~

FSM_COS adds a third space:

~~~text
Provider
    ↓
shared composition
    ↑
Consumer
~~~

Neither participant has to own the entire application. Both can contribute facts or constraints to the shared composition.

That creates a **negotiation seam**.

This seam becomes particularly valuable when capabilities come from different authors, packages, domains, or future versions of the ecosystem. The composition kernel provides a stable place for them to reconcile without forcing their internal implementations to know about one another.

That is why arbitration can become a major interoperability primitive rather than a minor lifecycle callback.

---

## What arbitration must not become

Arbitration must not become a back door for application infrastructure.

A MicroBundle should not use arbitration to:

- render UI;
- open a browser;
- start arbitrary background processes;
- mutate unrelated host state;
- perform network orchestration;
- become an application scheduler;
- encode every business rule in the composition kernel;
- secretly implement serialization;
- establish an undocumented universal priority hierarchy.

If a concern can be handled after RuntimeAssembly, it belongs after RuntimeAssembly.

The boundary is valuable precisely because not everything is allowed to cross it.

---

## Arbitration versus serialization

Serialization answers:

> **How do I represent semantic state as bytes?**

Arbitration answers:

> **Given these semantic participants, have we reached a stable composition?**

Those concerns can touch the same configuration data without being the same problem.

~~~text
FSM_Serialization
      ↓ representation
BundleRequest
      ↓
FSM_COS
      ↓ composition
Arbitration
      ↓
RuntimeAssembly
~~~

Keeping these boundaries separate means a serialization format can evolve without changing convergence semantics.

See [FSM_Serialization Theory](https://github.com/TrentBest/TheSingularityWorkshop.FSM_Serialization/blob/master/docs/THEORY.md).

---

## The interoperability thesis

The thesis is:

> **Interoperability is not only the ability to exchange data or invoke APIs. In a compositional runtime, interoperability is the ability of independently defined capabilities to participate in a shared state space and converge without requiring one of them to become the universal owner.**

FSM_COS does not claim that arbitration solves every interoperability problem.

It creates a place where one important class of problem can be addressed:

**composition-level compatibility.**

That is a meaningful architectural boundary.

---

## The contract at the edge

At the end of arbitration, the kernel should be able to say one of two things:

~~~text
SUCCESS
The requested composition converged.
RuntimeAssembly is valid for handoff.
~~~

or:

~~~text
FAILURE
The requested composition did not converge within the safety bound.
No stable RuntimeAssembly is produced.
~~~

That is stronger than “all DLLs loaded.”

It is stronger than “all constructors succeeded.”

It is stronger than “the host can probably figure it out.”

It gives the composition boundary an actual semantic completion condition.

---

## The short version

~~~text
LOAD
  ↓
What exists?
  ↓
ARBITRATE
  ↓
Can what exists live together?
  ↓
CONVERGE
  ↓
Is the composition stable?
  ↓
ASSEMBLE
  ↓
HAND OFF
~~~

> **Arbitration is not a fight for control. It is the mechanism by which independent capabilities discover whether they can become one stable system.**


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

## docs/ARCHITECTURE.md
Source blob: `1bd9acb25c92eefc1228be89d5978d1a0a7ddb4e`

# FSM_COS Architecture

> **Architecture answers how the composition kernel performs the theory.** For the deeper “why,” start with [FSM_COS Theory](THEORY.md).

FSM_COS is a small composition kernel. This document describes how its pieces cooperate rather than redefining concepts owned by neighboring systems.

## UML class diagram: composition kernel

This is a simplified, source-derived UML overview of the principal types and their relationships, not a complete per-type member inventory. Method signatures are abbreviated for legibility; use the XML-documented source and focused contract sections for exact parameter types, nullability, and full public members. External MicroBundle contracts are consumed from MicroBundleDomain; the schedule is dependency metadata, not the FSM_API process scheduler.

~~~mermaid
classDiagram
    class IFsmCos {
        <<interface>>
        +Execute(manifest, configurationSource) RuntimeAssembly
    }
    class FsmCos {
        +Execute(manifest, configurationSource) RuntimeAssembly
    }
    class IMicroBundleCatalog {
        <<interface>>
        +TryResolve(bundleId, version, bundle) bool
        +TryResolve(bundleId, bundle) bool
    }
    class IMicroBundleConfigurationSource {
        <<interface>>
        +TryGetConfiguration(runtimeId, bundleId, version, configuration) bool
    }
    class RuntimeManifest {
        +RuntimeId: ulong
        +Bundles: IReadOnlyList
        +ExperienceContext: IStateContext
        +LoadPlan: IReadOnlyList
        +Schedule: RuntimeManifestSchedule
        +Intent: SemanticIntent
        +ValidateStagedPlan()
    }
    class MicroBundleManifestEntry {
        <<record struct>>
        +BundleId: ulong
        +Version: string
    }
    class RuntimeManifestEntry {
        <<record>>
        +Reference: MicroBundleReference
        +Stage: ManifestLoadStage
        +IsValid: bool
    }
    class MicroBundleReference {
        <<record struct>>
        +BundleId: ulong
        +Version: string
        +ContentHash: string
        +IsValid: bool
        +Matches(entry): bool
    }
    class ManifestLoadStage {
        <<enumeration>>
        Bootstrap
        Resident
        Deferred
    }
    class RuntimeManifestSchedule {
        +Entries: IReadOnlyList
        +Dependencies: IReadOnlyList
        +IndependentEntries: IReadOnlyList
        +DependencyEntries: IReadOnlyList
        +DependenciesOf(bundleId)
        +IsDependencyReady(bundleId, isLoaded): bool
    }
    class RuntimeManifestDependency {
        <<record struct>>
        +BundleId: ulong
        +DependencyId: ulong
        +IsValid: bool
    }
    class RuntimeAssembly {
        +RuntimeId: ulong
        +Bundles: IReadOnlyList
        +ArbitrationRounds: int
        +Intent: SemanticIntent
        +TryGetBundle(bundleId, bundle): bool
    }

    FsmCos ..|> IFsmCos
    FsmCos --> IMicroBundleCatalog : resolves
    FsmCos --> IMicroBundleConfigurationSource : optional input
    FsmCos --> RuntimeManifest : consumes
    FsmCos --> RuntimeAssembly : returns
    RuntimeManifest *-- "0..*" MicroBundleManifestEntry : root requests
    RuntimeManifest o-- "0..*" RuntimeManifestEntry : optional load plan
    RuntimeManifest o-- RuntimeManifestSchedule : optional schedule
    RuntimeManifestEntry --> MicroBundleReference : published identity
    RuntimeManifestEntry --> ManifestLoadStage : intended stage
    RuntimeManifestSchedule o-- "0..*" RuntimeManifestEntry : entries
    RuntimeManifestSchedule o-- "0..*" RuntimeManifestDependency : dependency edges
~~~

The diagram is a map of type contracts, not a claim that constructing a schedule performs localization or that stage metadata activates deferred loading. See [Staged Manifest Loading](STAGED_MANIFEST_LOADING.md) for the implementation boundary.

## Runtime flow

![FSM_COS runtime flow](assets/fsm-cos-overview.svg)

![FSM_COS validation and composition path, generated with GUI.SVG](assets/fsm-cos-preflight.svg)

*Generated from `samples/DocumentationVisuals/Program.cs` using a pinned GUI.SVG source revision. This contract map shows the preflight gate, optional schedule validation, dependency-first loading, arbitration, and failure boundaries; it is not a live execution trace.*



## Core contracts

### RuntimeManifest
Identifies the runtime being assembled and supplies versioned root MicroBundle entries. It can also carry an optional host-provided `SemanticIntent`, an optional load-plan description, and an optional dependency schedule. Intent is passed through to the result; staged metadata is validated according to the current alpha contract and does not, by itself, activate repository localization or staged execution. The manifest is composition input, not application behavior. See [Runtime Manifest](RUNTIME_MANIFEST.md) and [Runtime Manifest Theory](MANIFEST_THEORY.md).

### MicroBundle manifest entries
A manifest entry identifies a requested MicroBundle and its requested version. Configuration is intentionally not part of the manifest. A separate configuration source may provide per-MicroBundle configuration at runtime.

### IMicroBundleCatalog
Resolves a bundle identity to the IMicroBundle required for installation. The catalog is supplied by the host, so future Warehouse-backed or generated resolvers do not require a different composition algorithm.

### IMicroBundle
Consumes the domain-owned `MicroBundleDescriptor` for identity/version/providers, while retaining composition-specific dependency configuration, Load, and Arbitrate behavior. FSM_COS does not redefine MicroBundle domain metadata. It does not expose a GUI, web server, or host lifecycle contract.

### MicroBundleLoadContext
Carries runtime identity and configuration available during installation. Configuration remains opaque to FSM_COS.

### ArbitrationContext
Exposes runtime identity, the currently loaded bundle set, and an optional FSM_API `IStateContext` supplied by the host. It is composition context, not host lifecycle state.

### RuntimeAssembly
The result surface of the composition pass: runtime identity, loaded bundles, the zero-based index of the round that reported convergence, and the optional `SemanticIntent` carried by the request. An `ArbitrationRounds` value of `0` means the first round converged; it is not the number of `Arbitrate` calls. Intent is passed through for the host; it does not make FSM_COS an input handler or presentation layer. See [RuntimeAssembly](RUNTIME_ASSEMBLY.md) and [FSM_COS Theory — RuntimeAssembly is the handoff object](THEORY.md#9-runtimeassembly-is-the-handoff-object).

![Dependency closure and installation order](assets/dependency-resolution.svg)

The conceptual reason for deriving this order rather than encoding it in the manifest is explained in [FSM_COS Theory — Composition starts with roots, not a giant object graph](THEORY.md#4-composition-starts-with-roots-not-a-giant-object-graph).

## Dependency resolution

FSM_COS first resolves and validates the complete reachable dependency closure before calling any bundle's `Load`. This rejects missing dependencies, malformed dependency contracts, identity mismatches, and cycles before bundle loading can create side effects. It snapshots the resolved bundle objects and dependency requests for the load pass, so the catalog is not queried a second time during loading. The subsequent depth-first installation order is iterative rather than recursive, avoiding call-stack exhaustion on deep graphs.

    requested A
       │
       ├── B
       │   └── C
       └── D

The resulting installation order is C, B, D, A.

Two guards remain fundamental during installation: already-loaded IDs are skipped, and IDs currently loading are tracked to detect cycles. The preflight graph validation independently detects cycles before installation begins.

Missing bundles and dependency cycles are composition failures.

## Configuration propagation

Configuration is an external runtime input. FSM_COS accepts configuration through an abstraction supplied by the host/repository layer; it does not read configuration files or interpret their format. If no configuration is available for a bundle, the bundle is loaded without external configuration and uses its defaults.

## Arbitration

FSM_COS resolves the catalog's actual reachable dependency graph and validates identity/dependency declarations before calling any MicroBundle `Load` method. When a manifest supplies a dependency schedule, it also checks the resolved closure, versions, and edges against that schedule before loading. This is a validation boundary, not a claim that FSM_COS localizes artifacts or implements staged/background loading. After reachable bundles are loaded, FSM_COS creates one ArbitrationContext.

    for each round
        for each loaded bundle
            changed |= bundle.Arbitrate(context, round)
        if no change → converged

If the configured maximum is reached without convergence, FSM_COS throws instead of returning an unstable assembly. The current default is ten rounds.

## Failure and partial side effects

Resolution, configuration lookup, bundle loading, and arbitration failures propagate to the caller; FSM_COS does not return a `RuntimeAssembly` when composition fails. Arbitration begins only after all reachable bundles have completed `Load`.

Composition is **not a transactional rollback boundary**. If a dependency completes `Load` and a later dependent bundle throws from `Load`, the earlier dependency's in-memory changes or external side effects cannot be generically undone by FSM_COS. The failing bundle is not added to the internal loaded set, and arbitration does not begin, but already-executed `Load` code may have changed state outside FSM_COS. Bundle authors and hosts must make loading safe for their lifecycle, and perform any required cleanup at the layer that owns those side effects.

The same principle applies to exceptions during arbitration: FSM_COS propagates the failure and does not hand off an assembly, but it cannot reverse arbitrary mutations performed by bundle code. Hosts should treat a failed `Execute` as a failed composition attempt, not as proof that all bundle-side effects were reverted.

### Diagnostic surface

The diagnostic contract is deliberately small: `Execute` either returns a converged `RuntimeAssembly` or throws. FSM_COS does not keep a logging pipeline, diagnostics registry, or partially successful assembly for the host to inspect. For failures it detects itself, exception messages identify the relevant bundle IDs and, where applicable, requested/resolved versions, dependency cycles, missing schedule entries, or mismatched dependency edges. Exceptions raised by the catalog, configuration source, `Load`, or `Arbitrate` are allowed to propagate so their owning layer retains the original failure details.

Hosts should catch failures at their composition boundary, preserve the exception and its context in their own logging/telemetry system, and apply cleanup according to their lifecycle policy. Message text is useful for troubleshooting, but consumers should not treat exact prose as a stable machine-readable error code.

## Catalog boundary

The catalog stays outside the composition algorithm:

    FSM_COS ← Catalog ← in-memory / generated registry / Warehouse / cache / Domain resolver

The composition engine should not need to know where a bundle came from.

## Host boundary

    FSM_COS
       │
       ▼
RuntimeAssembly
       │
       ├── WebPage → browser experience
       ├── AnyApp → desktop/local experience
       └── other hosts → their own manifestation

## Current development boundary

The active development line is refining the manifest/configuration boundary while preserving the small composition kernel:

```text
Manifest (bundle + version)
        ↓
resolver / catalog
        ↓
configuration source (optional)
        ↓
dependency closure
        ↓
Load()
        ↓
Arbitrate() until stable
        ↓
RuntimeAssembly
```

File I/O, repository transport, serialization, host lifecycle, and presentation remain outside the kernel.

Execution scheduling, resource allocation, Warehouse integration, and host lifecycle belong to later layers when their contracts are sufficiently clear.

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

## docs/RUNTIME_ASSEMBLY.md
Source blob: `217115fbbdce40d17656854342441a9b80bcf7fa`

# RuntimeAssembly

> RuntimeAssembly is where FSM_COS stops owning the journey. The deeper rationale is in [FSM_COS Theory](THEORY.md#9-runtimeassembly-is-the-handoff-object).

**RuntimeAssembly is the handoff object produced by FSM_COS.**

![RuntimeAssembly handoff: a manifest request enters FSM_COS, which returns a stable RuntimeAssembly to a host responsible for GUI, browser, Unity, and Experience behavior.](assets/runtime-assembly-handoff.svg)

*The graphic shows the implemented boundary, not an execution pipeline: `FsmCos.Execute` validates the manifest, resolves and loads the requested dependency closure, arbitrates to convergence, and returns `RuntimeAssembly`. The host-specific examples on the right are responsibilities outside this package.*


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

### Provider declarations are not provider invocation

A loaded bundle's `MicroBundleDescriptor.Providers` advertises provider IDs. The descriptor does not contain the provider instances, their CLR contract types, semantic contract versions, or an invocation mechanism. `RuntimeAssembly.TryGetBundle(...)` locates a loaded MicroBundle; it does not resolve a provider ID declared by that bundle.

Therefore the host or a separately authored integration MicroBundle must explicitly resolve and invoke providers. It should validate the expected semantic contract and compatibility before invocation; matching strings alone are not proof of compatibility. Keep domain-specific policy—such as Physics contact evidence to a Gestures recovery recommendation—outside this composition kernel.

See [MicroBundleDomain's Provider Resolution Contract](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleDomain/blob/development/docs/PROVIDER_RESOLUTION_CONTRACT.md) for the current descriptor boundary and the minimum checks proposed before standardizing a resolver.

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



---

## docs/RUNTIME_MANIFEST.md
Source blob: `d3753aeb315781243216ee81418f117d41eff29c`

# Runtime Manifest

> **The manifest says which MicroBundles and versions are requested. Configuration is a separate concern. FSM_COS composes the request.**

The **Runtime Manifest** is the machine-oriented composition request that crosses from authoring/tooling into FSM_COS.

It answers:

> **Which MicroBundles, at which requested versions, belong in this runtime composition?**

It deliberately does **not** answer:

- how a MicroBundle is configured;
- where a MicroBundle artifact is stored;
- how configuration is serialized;
- how the host presents the resulting Experience;
- how the assembled runtime is scheduled after composition.

![Conceptual authoring-to-manifest pipeline: authoring, validation, dependency closure, and baking produce a small versioned runtime request](assets/runtime-manifest-pipeline.svg)

*Scope note: this is the intended authoring/publication pipeline, not a claim that FSM_COS currently ships those editor, validation, closure-baking, or manifest-publication stages. The implemented contract begins with `RuntimeManifest`; `FsmCos.Execute` validates and composes that request.*

## UML class diagram: manifest and published plan

This is a simplified, source-derived UML overview, not a complete per-type API inventory. The manifest's root requests are distinct from optional published identity/stage metadata and dependency edges. The content hash is recorded in MicroBundleReference; this type does not itself fetch or verify artifact bytes. Exact signatures and nullability remain defined by the source and contract sections below.

~~~mermaid
classDiagram
    class RuntimeManifest {
        +RuntimeId: ulong
        +Bundles: IReadOnlyList~MicroBundleManifestEntry~
        +ExperienceContext: IStateContext
        +LoadPlan: IReadOnlyList~RuntimeManifestEntry~
        +Schedule: RuntimeManifestSchedule
        +Intent: SemanticIntent
        +Empty(runtimeId, context, intent)$ RuntimeManifest
        +ValidateStagedPlan() void
    }
    class MicroBundleManifestEntry {
        <<record struct>>
        +BundleId: ulong
        +Version: string
    }
    class RuntimeManifestEntry {
        <<record>>
        +Reference: MicroBundleReference
        +Stage: ManifestLoadStage
        +IsValid: bool
    }
    class MicroBundleReference {
        <<record struct>>
        +BundleId: ulong
        +Version: string
        +ContentHash: string
        +IsValid: bool
        +Matches(entry) bool
    }
    class ManifestLoadStage {
        <<enumeration>>
        Bootstrap
        Resident
        Deferred
    }
    class RuntimeManifestDependency {
        <<record struct>>
        +BundleId: ulong
        +DependencyId: ulong
        +IsValid: bool
    }
    class RuntimeManifestSchedule {
        +Entries: IReadOnlyList~RuntimeManifestEntry~
        +Dependencies: IReadOnlyList~RuntimeManifestDependency~
        +IndependentEntries: IReadOnlyList~RuntimeManifestEntry~
        +DependencyEntries: IReadOnlyList~RuntimeManifestEntry~
        +DependenciesOf(bundleId) IReadOnlyList
        +IsDependencyReady(bundleId, isLoaded) bool
    }

    RuntimeManifest *-- "0..*" MicroBundleManifestEntry : root requests
    RuntimeManifest o-- "0..*" RuntimeManifestEntry : optional load plan
    RuntimeManifest o-- "0..1" RuntimeManifestSchedule : optional dependency plan
    RuntimeManifestEntry --> MicroBundleReference : identity
    RuntimeManifestEntry --> ManifestLoadStage : intended stage
    RuntimeManifestSchedule o-- "0..*" RuntimeManifestEntry : entries
    RuntimeManifestSchedule o-- "0..*" RuntimeManifestDependency : edges
    RuntimeManifestDependency ..> RuntimeManifestEntry : links by BundleId
~~~

The schedule is a validated dependency plan. It does not localize artifacts, execute a process schedule, or by itself enable deferred loading. The diagram reflects the current source model, including that the root manifest requests a version while published references also carry ContentHash.

## The contract

The current development contract is intentionally small:

```csharp
public sealed record RuntimeManifest(
    ulong RuntimeId,
    IReadOnlyList<MicroBundleManifestEntry> Bundles,
    IStateContext? ExperienceContext = null,
    IReadOnlyList<RuntimeManifestEntry>? LoadPlan = null,
    RuntimeManifestSchedule? Schedule = null,
    SemanticIntent? Intent = null);
```

Each entry identifies one root MicroBundle and the version requested by the manifest:

```csharp
public readonly record struct MicroBundleManifestEntry(
    ulong BundleId,
    string Version);
```

The concrete implementation validates that the ID is non-zero and the version is present.

Conceptually:

```text
RuntimeManifest
├── RuntimeId
├── Bundles
│   ├── MicroBundle ID + requested version
│   ├── MicroBundle ID + requested version
│   └── MicroBundle ID + requested version
├── optional ExperienceContext
├── optional staged LoadPlan + Schedule
└── optional FSM_UserIO SemanticIntent
```

The manifest names **roots**. FSM_COS discovers the dependency closure from those roots.

## Optional intent and published dependency plan

`Intent` carries an optional `FSM_UserIO.SemanticIntent` to the returned `RuntimeAssembly`. It describes application-owned semantic intent; it does not grant device authority or cause an Experience to start.

`LoadPlan` and `Schedule` are optional published metadata. When a schedule is present, FSM_COS resolves the dependency closure before loading and verifies that the scheduled bundle IDs, resolved versions, and dependency edges match the domain-owned MicroBundle declarations. A mismatch fails before any bundle `Load` call. This is a consistency check, not artifact localization or hash verification.

## Example

```csharp
var manifest = new RuntimeManifest(
    RuntimeId: 1001,
    Bundles:
    [
        new MicroBundleManifestEntry(10, "1.2.0"),
        new MicroBundleManifestEntry(20, "3.1.0")
    ]);
```

This means:

- runtime `1001` is being assembled;
- MicroBundle `10` is requested at `1.2.0`;
- MicroBundle `20` is requested at `3.1.0`;
- configuration is **not** embedded in the manifest.

The catalog/resolver is responsible for locating a compatible artifact. FSM_COS verifies the resolved root reports the requested version before loading it.

### Conflicting requests for the same root identity

A manifest must not request the same MicroBundle ID at two different versions. For example, requesting bundle `10` at both `1.2.0` and `2.0.0` is ambiguous: one runtime composition cannot silently satisfy both root requests by choosing whichever catalog result happens to load first. FSM_COS detects conflicting versions for the same root ID and fails before loading the composition.

If two parts of an application genuinely require incompatible versions of one capability, model that incompatibility explicitly rather than relying on duplicate IDs in one manifest.

**An explicit root version also remains authoritative when that same MicroBundle is reached through another root's dependency graph.** FSM_COS resolves the distinct root ID/version requests before invoking any MicroBundle `Load` method, then reuses those selected root instances during dependency traversal. If a requested root version cannot be resolved, composition fails before loading begins. When a published schedule is supplied, its dependency-graph validation uses those same selected root instances, so the schedule check and actual composition cannot silently disagree about the version chosen for an explicitly requested root.

## Version is part of the request

Version belongs in the manifest because the manifest is a publication-level statement of **what composition was requested**.

```text
Manifest
    │
    ├── Bundle 10 → version 1.2.0
    └── Bundle 20 → version 3.1.0

Catalog / Repository
    │
    └── locate those requested artifacts

FSM_COS
    │
    └── compose the resolved artifacts
```

The manifest does not need to know whether an artifact came from an in-memory catalog, a repository, Azure Blob, a package cache, or another delivery mechanism.

## Configuration is deliberately outside the manifest

Configuration answers a different question:

> **How should this particular MicroBundle participate in this particular runtime?**

That information belongs in a separate configuration source.

```text
runtime 1001

MicroBundle 10
    └── configuration file exists
            ↓
        configuration bytes

MicroBundle 20
    └── no configuration file
            ↓
        use MicroBundle defaults
```

FSM_COS exposes the composition boundary through:

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

The interface is intentionally about **availability**, not storage.

The implementation might read a local configuration file, an Azure Blob, a repository artifact, generated resources, or another application-owned source.

FSM_COS does not choose among those.

### No configuration means defaults

This is an important semantic rule:

```text
configuration exists
        │
        ▼
provide bytes to the MicroBundle
        │
        ▼
MicroBundle interprets its own configuration

configuration absent
        │
        ▼
provide no external configuration
        │
        ▼
MicroBundle loads its defaults
```

There is no empty “default configuration file” requirement. Absence itself has meaning.

## Configuration format is not an FSM_COS concern

FSM_COS does not parse JSON, YAML, XML, binary records, generated C#, or any other configuration representation.

If configuration becomes a serialized byte-level contract, the serialization responsibility remains with the appropriate serialization layer.

```text
representation
     ↓
configuration source
     ↓
FSM_COS
     ↓
MicroBundle
```

## Roots versus dependencies

Suppose the requested root is:

```text
A
├── B
│   └── C
└── D
```

The manifest only needs:

```text
[A @ requested-version]
```

FSM_COS discovers:

```text
C → B → D → A
```

and loads dependencies before the root.

A dependency's domain-owned `MicroBundleDependencyRequest` may still carry dependency-specific configuration for compatibility with the MicroBundleDomain runtime contract. When an external configuration source supplies configuration for that dependency, the external value is used.

That compatibility detail does **not** move configuration into the manifest.

## What does not belong in a Runtime Manifest?

A manifest should not quietly become:

- an application configuration file;
- a GUI layout;
- a browser lifecycle description;
- a Unity scene;
- a Warehouse database;
- an Experience execution script;
- a serialized RuntimeAssembly;
- a repository API response.

The boundary is:

```text
Manifest
    = what MicroBundles + which versions

Configuration
    = how a MicroBundle is configured

Repository / Resolver
    = where the artifact comes from

FSM_COS
    = how the requested composition is assembled

RuntimeAssembly
    = what FSM_COS successfully assembled
```

## Authoring → publication → composition

A rich authoring environment may know far more than the runtime needs:

```text
rich authoring model
        │
        ├── names
        ├── ontology
        ├── variants
        ├── dependencies
        ├── provenance
        └── editor metadata
        │
        ▼
     validation
        │
        ▼
MicroBundle IDs + requested versions
        │
        ▼
 Runtime Manifest
        │
        ├─────────────── optional configuration source
        │                                  │
        ▼                                  ▼
 Catalog / Resolver ───────────────► FSM_COS
                                        │
                                        ▼
                                 RuntimeAssembly
```

This is a compression boundary: authoring can be rich without forcing the composition kernel to become an editor.

## Stability

A useful long-term property is:

> **The same semantic manifest, resolved against the same compatible catalog and versions, should produce the same composition.**

That does not require every host to render or execute the result identically.

It means the composition request remains meaningful when moved between hosts.

## Related documents

- [FSM_COS Architecture](ARCHITECTURE.md)
- [FSM_COS Theory](THEORY.md)
- [Consuming MicroBundles](CONSUMING_MICROBUNDLES.md)
- [RuntimeAssembly](RUNTIME_ASSEMBLY.md)
- [Arbitration and Convergence](ARBITRATION.md)
- [Development](DEVELOPMENT.md)

---

<p align="center"><em>The Singularity Workshop — Tools for the curious, the bold, and the systemically inclined.</em><br><strong>Because state shouldn't be a mess.</strong><br><em>And because static boundaries are invitations to cause trouble.</em></p>



---

## docs/STAGED_MANIFEST_LOADING.md
Source blob: `abc5012915235906402550b912eb43b6ac2b2939`

# Staged Manifest Loading and Dependency Planning

**Status:** first compatible contract slice is present on `development`; host localization and staged composition are not yet integrated. Do not describe deferred loading as a working end-to-end runtime feature until the host path is implemented and tested.

## UML class diagram: staged metadata and load-plan state

This is a simplified, source-derived overview of the current metadata and state-tracker types, not a claim that staged composition is integrated end to end. Method signatures are abbreviated; the source and the staged-loading contract below define exact signatures and behavior.

~~~mermaid
classDiagram
    class RuntimeManifest {
        +Bundles: IReadOnlyList~MicroBundleManifestEntry~
        +LoadPlan: IReadOnlyList~RuntimeManifestEntry~
        +Schedule: RuntimeManifestSchedule
        +ValidateStagedPlan() void
    }
    class RuntimeManifestLoadPlan {
        +Entries: IReadOnlyList~RuntimeManifestEntry~
        +GetState(bundleId) MicroBundleRuntimeState
        +MarkLocalized(bundleId) void
        +MarkLoaded(bundleId) void
        +EvaluatePromotions(evaluator, context, schedule) IEnumerable
    }
    class RuntimeManifestEntry {
        +Reference: MicroBundleReference
        +Stage: ManifestLoadStage
        +IsValid: bool
    }
    class RuntimeManifestSchedule {
        +Entries: IReadOnlyList~RuntimeManifestEntry~
        +Dependencies: IReadOnlyList~RuntimeManifestDependency~
        +IsDependencyReady(bundleId, isLoaded) bool
    }
    class RuntimeManifestDependency {
        +BundleId: ulong
        +DependencyId: ulong
        +IsValid: bool
    }
    class IManifestLoadEvaluator {
        <<interface>>
        +ShouldLoad(entry, experienceContext) bool
    }
    class ManifestLoadStage {
        <<enumeration>>
        Bootstrap
        Resident
        Deferred
    }
    class MicroBundleRuntimeState {
        <<enumeration>>
        Published
        Localized
        Loaded
    }

    RuntimeManifest o-- "0..*" RuntimeManifestEntry : optional plan
    RuntimeManifest o-- RuntimeManifestSchedule : optional schedule
    RuntimeManifestLoadPlan o-- "1..*" RuntimeManifestEntry : tracked entries
    RuntimeManifestLoadPlan --> RuntimeManifestSchedule : optional dependency gate
    RuntimeManifestLoadPlan ..> IManifestLoadEvaluator : asks for deferred promotion
    RuntimeManifestEntry --> ManifestLoadStage : intended stage
    RuntimeManifestLoadPlan ..> MicroBundleRuntimeState : tracks availability
    RuntimeManifestSchedule o-- "0..*" RuntimeManifestDependency : edges
~~~

This diagram is deliberately not an execution promise. The load-plan type tracks Published → Localized → Loaded state and can evaluate promotion candidates, but a host/repository must perform localization and actual loading. ManifestLoadStage labels intent; it does not automatically make FsmCos defer or promote bundles.

## Four responsibilities, four boundaries

![Dependency resolution and prerequisite relationships used during composition](assets/dependency-resolution.svg)

*The animated outline illustrates one valid dependency-first order for the example graph (`C → B → D → A`); it is not a live trace of a running process. The graph is illustrative, while actual dependencies come from each MicroBundle's declarations. This visual does not claim host localization or end-to-end deferred loading is implemented. See [`FsmCos.LoadBundle`](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/src/FSM_COS/FsmCos.cs) and the [dependency-order test](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/tests/FSM_COS.Tests/FsmCosTests.cs). The ownership table below remains authoritative for the current boundary.*


The staged-loading work must keep these concerns distinct:

| Concern | Owner | Responsibility |
|---|---|---|
| Dependency plan | Published manifest / FSM_COS contract | Describes which MicroBundles require which prerequisites |
| Process scheduling | FSM_API | Owns sequential process-group scheduling and execution semantics |
| Localization | Host/repository/cache adapter | Makes immutable published artifact bytes available locally and verifies identity/hash |
| Composition | FSM_COS | Resolves the current domain-owned MicroBundle contract, loads dependency closure, arbitrates, and returns a RuntimeAssembly |

FSM_COS must not become a storage engine or introduce a competing process scheduler. A dependency schedule is a description of prerequisites; it is not a substitute for FSM_API scheduling.

## Identity and lifecycle

A `MicroBundleReference` records:

- `BundleId`
- requested `Version`
- `ContentHash`

The reference is an identity claim, not proof that bytes have been downloaded or verified. The repository/localization boundary must validate the artifact against that identity.

The lifecycle remains:

```text
Published ──localize and verify──> Localized ──compose──> Loaded
```

- **Published** — the immutable artifact is identified by the published manifest.
- **Localized** — verified artifact bytes are available in the local cache.
- **Loaded** — the MicroBundle has been instantiated into the active composition.

Localization does not imply in-memory loading.

## Stages and dependency readiness

A manifest entry may be:

- **Bootstrap** — required to establish the initial visible/runnable Experience.
- **Resident** — participates in initial preparation but is not itself a bootstrap prerequisite.
- **Deferred** — eligible for later Experience-owned promotion.

Promotion candidates are considered in Bootstrap → Resident → Deferred order while preserving manifest order within each stage. Dependency readiness still wins: a bootstrap entry cannot be loaded until its prerequisites are loaded. This is planning/state-model behavior today; `FsmCos.Execute` does not yet run a bootstrap-only composition or continue localization in the background.

An `IManifestLoadEvaluator` lets the Experience decide *whether* a deferred capability is wanted. It does not decide whether prerequisites are ready: the dependency schedule/load plan must enforce that separately. A dependent entry cannot be marked Loaded until its prerequisites are Loaded.

Independent entries are entries with no prerequisite edges. Dependency-constrained entries have one or more prerequisites. This classification is useful for broad localization batches; actual composition still obeys dependency order.

## Compatibility with the current composition contract

The current manifest continues to use `MicroBundleManifestEntry` (bundle ID + requested version). Do not replace it with the old branch's `BundleRequest` model: that model lacks the current root-version contract and would conflict with the configuration-source boundary.

The current `FsmCos.Execute` path still resolves domain-owned `IMicroBundle` instances through `IMicroBundleCatalog`, traverses their dependency declarations, loads dependencies before dependents, detects cycles, and arbitrates to convergence. The optional staged metadata validates root identity/version consistency. When a `Schedule` is supplied, FSM_COS also preflights the catalog-resolved dependency closure and rejects scheduled bundle-ID, resolved-version, or dependency-edge drift before any `Load` side effects. It still does **not** execute bootstrap-only composition, perform asynchronous localization, verify content hashes, or promote Deferred entries into an already running RuntimeAssembly.

## Intended end-to-end direction

```text
Forge / publisher
  └─ immutable versioned manifest + dependency edges + stages
          ├─ localization clock → repository/cache batches
          ├─ composition clock  → dependency-ready MicroBundles
          └─ Experience clock   → bootstrap entry and continued operation
                                      │
                                      ▼
                              FSM_COS RuntimeAssembly
```

The three clocks should be able to progress independently. A host should be able to enter a minimum runnable Experience while other eligible artifacts continue localizing, without faking a delay or making the presentation layer act as the scheduler.

The published manifest should describe the dependency graph established by the Forge. The current runtime dependency traversal remains the compatibility behavior; the optional published schedule is now checked against the resolved graph before loading, but it does not yet dictate staged loading order or lifecycle. Do not treat stage metadata as an end-to-end staged runtime until localization and promotion have dedicated implementation and tests.

## Verification still required

- Verify the new planning contracts and tests on the current `development` dependency graph.
- Add consistency tests comparing published dependency edges with the resolved MicroBundle dependency contract.
- Introduce a host/repository localization abstraction only when its contract can be exercised by a real host.
- Integrate deferred promotion without losing configuration precedence, dependency closure, cycle detection, arbitration convergence, or RuntimeAssembly behavior.
- Demonstrate bootstrap entry while localization continues in the background.
- Keep NuGet publishing disabled unless the owner explicitly authorizes it.

---

<p align="center"><em>The Singularity Workshop — Tools for the curious, the bold, and the systemically inclined.</em><br><strong>Because state shouldn't be a mess.</strong><br><em>And because static boundaries are invitations to cause trouble.</em></p>

