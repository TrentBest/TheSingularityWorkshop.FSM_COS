# Learn FSM_COS by Following One Request

This guide teaches the core idea behind FSM_COS by following a small example from request to handoff. You can read it without coding, then run the sample to see the same behavior for yourself.

## The problem: one capability needs another

Imagine a report capability that needs a data-source capability. A host wants the report, but the report cannot do its job unless its prerequisite is available too.

The host describes what it wants in a **runtime manifest**. It does not need to list every prerequisite itself.

![Dependency resolution example: a requested report depends on a data source, and dependencies are loaded before the bundles that require them](assets/dependency-resolution.svg)

In this illustration:

- **A** is the requested report capability.
- **B** and **D** are prerequisites of A.
- **C** is a prerequisite of B.
- FSM_COS discovers the reachable dependency graph, checks it, and loads prerequisites before their dependents.

The A/B/C/D graph is an example to teach the rule, not a list of built-in bundles. Your catalog supplies the actual MicroBundles.

## Follow the request

The composition process has four important stages.

![FSM_COS validation and composition path, from manifest and identity checks through dependency-first loading and bounded arbitration to the host handoff](assets/fsm-cos-preflight.svg)

### 1. Describe what you want

A `RuntimeManifest` identifies the runtime and the root MicroBundles requested by the host. A root request includes a bundle ID and version.

Think of this as an order: it names the requested capability, not every item needed to fulfill it.

### 2. Find and check the complete set

The catalog is supplied by your application or repository adapter. FSM_COS asks it to resolve the requested roots and their dependencies.

Before calling bundle load methods, FSM_COS checks the resolved composition for contract problems such as mismatched identities, missing dependencies, and dependency cycles. A request that cannot be composed correctly fails instead of quietly substituting a different capability.

### 3. Load prerequisites before dependents

For the report example, the data source must load before the report that needs it. With deeper dependency chains, FSM_COS follows the reachable graph and loads in dependency-first order.

The exact order among independent branches is not the point of the contract. The important rule is that a dependency is loaded before the bundle that requires it.

### 4. Let the composition settle, then hand it to the host

After loading, FSM_COS runs bounded **arbitration** rounds. Each participating MicroBundle can report whether it changed composition-relevant state. A complete round with no reported changes means the composition has converged.

If the composition does not converge within the configured bound, FSM_COS fails rather than returning it as a successful assembly.

When composition succeeds, FSM_COS returns a `RuntimeAssembly`. The host decides what to do with it: execute application behavior, show a screen, expose an endpoint, or use it in another host-specific way. FSM_COS does not create or run that experience itself.

## Try the example

You need Git and the .NET 8 SDK. From a terminal, run:

```bash
git clone https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS.git
cd TheSingularityWorkshop.FSM_COS
dotnet run --project samples/FSM_COS.MinimalConsumer/FSM_COS.MinimalConsumer.csproj
```

The sample defines a tiny in-memory catalog, a data-source bundle (ID `2`), and a report bundle (ID `1`) that depends on it. The expected final line is:

```text
Assembly order: 2 -> 1
```

Read that line from left to right: the prerequisite was loaded first, then the requested report. The sample's catalog and bundles are teaching examples supplied by the sample program, not built-in FSM_COS types.

For the full walkthrough, see the [minimal consumer recipe](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/samples/FSM_COS.MinimalConsumer/Program.md).

## Change the question

If you are comfortable editing the sample, find the `RuntimeManifest` in `samples/FSM_COS.MinimalConsumer/Program.cs`.

1. Run the sample unchanged and note the order `2 -> 1`.
2. Change the requested root from `reportBundle.Id` to `dataBundle.Id`.
3. Run it again. The assembly should now contain only bundle `2`, because the request no longer asks for the report.
4. Restore the original line.

You have changed **what the host requested**, not the dependency algorithm. FSM_COS composes the graph reachable from the new root.

## If something goes wrong

| What you see | What to check |
|---|---|
| A requested bundle cannot be found | Check that the catalog can resolve the requested ID and exact version. |
| A dependency cannot be found | Check that the catalog can resolve every dependency declared by the requested bundles. |
| A dependency cycle is reported | Follow the dependency chain and remove the cycle from the participating bundle declarations. |
| A bundle reports the wrong identity or version | Check the catalog mapping and the metadata returned by the resolved bundle. |
| Arbitration does not converge | Review whether each bundle reports a change only when it actually changed composition-relevant state. |
| A later load fails after an earlier bundle loaded | Remember that composition is not a transaction: FSM_COS cannot undo arbitrary side effects performed by bundle code. Put cleanup where those effects are owned. |

## Keep the responsibilities straight

- **The manifest** says what the host requests.
- **The catalog** supplies the requested MicroBundles.
- **FSM_COS** resolves, validates, loads, and arbitrates the composition.
- **The host** decides how the resulting assembly is used and presented.

That boundary lets desktop programs, websites, services, simulations, and other hosts use the same composition kernel without requiring FSM_COS to own their UI or application lifecycle.

## Where to go next

- [What Is FSM_COS?](WHAT_IS_FSM_COS.md) for the broader introduction.
- [Consuming MicroBundles](CONSUMING_MICROBUNDLES.md) for the catalog, manifest, configuration, and bundle contracts.
- [Runtime Manifest](RUNTIME_MANIFEST.md) for the request structure.
- [Arbitration and Convergence](ARBITRATION.md) for the bounded reconciliation stage.
- [RuntimeAssembly](RUNTIME_ASSEMBLY.md) for the successful handoff.
- [Gallery](GALLERY.md) for the visual overview of the composition flow.
