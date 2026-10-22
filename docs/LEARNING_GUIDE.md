<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# Learn FSM_COS by Following One Request

This guide follows one real example through the core FSM_COS job: a host requests a report capability, the report depends on a data source, and FSM_COS assembles both in the right order.

FSM_COS is the Workshop's **Compute Operating System layer for runtime composition**. It turns a `RuntimeManifest` into a `RuntimeAssembly` by resolving MicroBundles, validating their dependencies, loading prerequisites first, and running bounded arbitration. The host supplies the catalog and decides what to do with the result.

## The problem: the host requests one thing, but needs more

The minimal consumer requests only the report bundle (ID `1`). That report declares a dependency on the data-source bundle (ID `2`). The host should not have to duplicate the dependency algorithm by manually listing every prerequisite in the manifest.

![Dependency resolution example: a requested report depends on a data source, and dependencies are loaded before the bundles that require them](assets/dependency-resolution.svg)

The diagram's A/B/C/D graph is a teaching example. The runnable sample uses two bundles: report `1` and data source `2`.

## Follow the request

### 1. The host describes the requested root

In `samples/FSM_COS.MinimalConsumer/Program.cs`, the manifest requests the report:

```csharp
var manifest = new RuntimeManifest(
    RuntimeId: 1001,
    Bundles: [new MicroBundleManifestEntry(reportBundle.Id, "1.0.0")]);
```

The manifest names the requested root and version. The sample's catalog makes the report and its prerequisite available to FSM_COS.

### 2. FSM_COS resolves and validates the dependency closure

FSM_COS asks the catalog to resolve the requested bundle and follows the dependency declarations. It validates the reachable graph—including identity and dependency consistency, missing dependencies, and cycles—before invoking bundle loading.

If required dependencies cannot be resolved or the graph is invalid, composition fails. FSM_COS does not return a successful `RuntimeAssembly` for an incomplete graph.

### 3. Prerequisites load before dependents

The report needs the data source, so the data source is loaded first. FSM_COS determines this order from the dependency relationships; the sample does not hard-code an assembly order.

Expected final line:

```text
Assembly order: 2 -> 1
```

### 4. Arbitration must converge before handoff

After loading, FSM_COS runs bounded arbitration rounds through the MicroBundle contract. A complete round that reports no changes establishes convergence. If every allowed round reports changes, composition fails rather than handing the host an assembly that did not converge.

A successful `RuntimeAssembly` is the composition result. It is not a running application, GUI, or guarantee that arbitrary bundles are semantically compatible.

## Run the example

You need Git and the **.NET 8 SDK**.

```bash
git clone https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS.git
cd TheSingularityWorkshop.FSM_COS
dotnet run --project samples/FSM_COS.MinimalConsumer/FSM_COS.MinimalConsumer.csproj
```

The sample supplies its own in-memory `IMicroBundleCatalog` and two example MicroBundles. They are example code, not built-in FSM_COS bundle implementations. Read the [complete sample recipe](../samples/FSM_COS.MinimalConsumer/Program.md) for its source, type references, and reproduction notes.

## Change the request

Open `samples/FSM_COS.MinimalConsumer/Program.cs` and find the `RuntimeManifest` constructor.

1. Run the sample unchanged and note the assembly order `2 -> 1`.
2. Change the requested root from `reportBundle.Id` to `dataBundle.Id`.
3. Run it again. The assembly should contain only bundle `2`, because the request no longer asks for the report.
4. Restore the original line.

You changed **what the host requested**, not the dependency algorithm. FSM_COS composes the dependency closure reachable from the manifest roots.

## Keep the responsibilities straight

| Part | Responsibility |
|---|---|
| Host | Chooses the requested roots, supplies a catalog and optional configuration source, and owns execution/presentation. |
| `RuntimeManifest` | Describes the requested root MicroBundles and versions. |
| Catalog | Resolves identities to available MicroBundle instances. It need not be a network repository. |
| FSM_COS | Validates dependency closure, loads prerequisites first, arbitrates to convergence, and returns the assembly. |
| MicroBundleDomain | Defines the MicroBundle contract consumed by FSM_COS. |
| `RuntimeAssembly` | Hands the composed result back to the host. |

FSM_COS does not download artifacts, verify artifact bytes from content hashes, activate deferred bundles in an existing assembly, or run the host application. The current planning metadata and these limits are explained in [Staged Manifest Loading](STAGED_MANIFEST_LOADING.md).

## Where to go next

- **Build your own consumer:** [Consuming MicroBundles](CONSUMING_MICROBUNDLES.md).
- **Understand the type relationships:** [Architecture](ARCHITECTURE.md).
- **Inspect the request:** [Runtime Manifest](RUNTIME_MANIFEST.md).
- **Inspect the result:** [RuntimeAssembly](RUNTIME_ASSEMBLY.md).
- **Understand convergence:** [Arbitration](ARBITRATION.md).
- **Explore the visuals:** [Gallery](GALLERY.md).
- **Browse all guides:** [Documentation Index](../DOCUMENTATION_INDEX.md).

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
