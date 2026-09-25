# FSM_COS Theory

## 1. The missing layer is assembly

FSM_API provides the behavioral substrate. Higher layers describe richer runtime behavior. MicroBundles provide focused units of capability, content, or behavior. The Warehouse stores and delivers data. Experiences describe what is encountered. Hosts provide environments.

FSM_COS exists between the request and those systems. Its question is:

> Given this runtime manifest, what must be installed, configured, reconciled, and assembled before the requested runtime exists?

It is neither the runtime nor the host. It is the assembly boundary.

## 2. The manifest is the instruction to the crane

FSM_COS does not decide what an application ought to be. The caller supplies a RuntimeManifest. The manifest is the published statement of required runtime composition.

    authoring intent
          ↓
    published manifest
          ↓
       FSM_COS
          ↓
 resolve → load → arbitrate → assemble
          ↓
     RuntimeAssembly

The manifest can become progressively more compact as editor-time information is baked away.

## 3. Assembly is ordered, composition is declarative

A manifest requests bundles declaratively. Assembly derives installation order from dependency relationships.

    A
    └── B
        └── C

becomes C → B → A before arbitration begins.

A dependency cycle is a composition error, not an ordering problem FSM_COS should guess around.

## 4. Configuration travels with composition

A dependency is not merely an identifier. A parent can require a dependency together with configuration that must exist when that dependency loads.

FSM_COS carries that configuration through the load context. The alpha contract keeps configuration opaque as bytes; the bundle that owns the data interprets it.

## 5. Loading is installation, not execution

Load establishes a MicroBundle inside the composition. It does not start an Experience, render UI, enter a world, schedule a process, or start an application lifecycle.

Those are runtime/host responsibilities. This distinction allows one semantic composition to be assembled for different manifestations.

## 6. Arbitration is convergence

Loading establishes the initial set. Arbitration allows installed bundles to react to the presence of the complete composition.

Each round asks every loaded bundle whether the composition changed. A round with no changes means convergence. The process is bounded by a maximum round count.

The goal is not that one bundle wins. The goal is that the composition reaches a stable state or explicitly fails to converge.

## 7. RuntimeAssembly is the boundary object

RuntimeAssembly is the result of composition. It records runtime identity, loaded MicroBundles, and arbitration rounds.

It does not pretend to be the application. A host can take the assembly and connect it to its own execution environment.

## 8. Same manifest, different manifestation

    RuntimeManifest → FSM_COS → RuntimeAssembly
                         │
             ┌───────────┼───────────┐
             ▼           ▼           ▼
          WebForge     AnyApp    MyVR / Domain

The composition request is not inherently web, desktop, Unity, or browser-specific.

## 9. What FSM_COS deliberately does not own

FSM_COS does not own rendering, browser or desktop lifecycle, Warehouse storage, performance adaptation, telemetry policy, domain rules, Experience presentation, arbitrary scheduling, or unrelated application configuration.

Those systems can participate through explicit contracts.

> The crane can carry the parts. It does not become the factory floor.

## 10. The invariant

The alpha implementation is intentionally small. Its lasting boundary is:

    FSM_COS assembles; other systems perform the assembled work.