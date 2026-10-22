<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# What Is FSM_COS? The Compute Operating System

FSM_COS is The Singularity Workshop's **Compute Operating System for assembling composable runtime systems**. A host declares what computation it wants; FSM_COS resolves the required MicroBundles, checks their dependency relationships, loads the composition in dependency order, runs bounded arbitration, and returns a `RuntimeAssembly` for the host to use.

“Operating system” here describes the **runtime composition layer** of the Workshop architecture. FSM_COS is not a general-purpose desktop operating system, a hardware kernel, or the application's execution loop. Its job is to turn a composition request into a validated, assembled result at a well-defined boundary.

The **FSM** in its name points to the [FSM_API](https://github.com/TrentBest/FSM_API) foundation: FSM_COS consumes finite-state-machine/state-context primitives as part of the wider Workshop runtime model. Its own responsibility is composition, not scheduling every process or running the finished experience.

## The problem it solves

Applications often need different combinations of capabilities. A reporting experience may need a data source; a WebPage may need several presentation and domain capabilities; a simulation may need a different set. Some capabilities depend on others, and those prerequisites must be present before the dependent capability is loaded.

Without a shared composition layer, each host tends to grow its own dependency resolution, validation, loading order, and readiness rules. That logic becomes duplicated and tightly coupled to the application.

FSM_COS centralizes that common work. The host remains free to choose its catalog, configuration source, runtime lifecycle, interface, and presentation.

## One request, one assembled result

![FSM_COS overview: a runtime manifest enters the composition kernel and a RuntimeAssembly is handed to a host](assets/fsm-cos-overview.svg)

```text
Host supplies:
  RuntimeManifest + MicroBundle catalog + optional configuration
                         │
                         ▼
                      FSM_COS
          resolve → validate → load → arbitrate
                         │
                         ▼
                  RuntimeAssembly
                         │
                         ▼
             host executes or presents it
```

The request names the root MicroBundles and requested versions. FSM_COS discovers their dependency closure through the supplied catalog. It validates the reachable composition, loads prerequisites before dependents, and runs arbitration for a bounded number of rounds. If a required step fails or arbitration does not converge, composition fails rather than returning a successful assembly.

A successful `RuntimeAssembly` is a composition handoff. It is not proof that all bundles are semantically compatible, and it does not start an application or render an interface.

## Why would I use it?

FSM_COS is worth using when you need one or more of these properties:

- **Combinations are selected at runtime.** Different manifests can request different roots without requiring a separate hard-wired host for every combination.
- **Dependencies matter.** A requested capability can bring in the prerequisites it declares.
- **Hosts should share composition rules.** A website, desktop app, service, simulation, or headless process can use the same composition algorithm while retaining its own execution model.
- **Failures must stop the handoff.** Missing dependencies, invalid dependency relationships, cycles, and failure to converge are treated as composition failures.
- **Composition should remain separate from infrastructure.** The catalog may use in-memory objects, local resources, or a repository integration supplied by the host; FSM_COS does not dictate that implementation.

FSM_COS is not mandatory for every program. It earns its place when reusable capabilities need to be assembled consistently across different runtime requests or hosts.

## Five important terms

| Term | Meaning |
|---|---|
| **Host** | The application or runtime that requests a composition and decides how to use the result. |
| **MicroBundle** | A unit that implements the MicroBundleDomain contract and can participate in composition. It is not merely any small library or NuGet package. |
| **RuntimeManifest** | The request: root MicroBundles and their requested versions, plus optional supported metadata. |
| **Catalog** | A host-supplied resolver that makes requested MicroBundles available to FSM_COS. It is not automatically an artifact downloader. |
| **RuntimeAssembly** | The successful composition result handed back to the host. |

The MicroBundle contract is owned by [MicroBundleDomain](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleDomain). FSM_COS consumes that domain contract and provides the composition algorithm.

## What FSM_COS currently does—and what it does not

**Current composition behavior:**
- resolves requested MicroBundles through a host-supplied catalog;
- discovers and validates dependency closure;
- checks for missing dependencies and dependency cycles before successful handoff;
- loads prerequisites before dependents;
- accepts optional configuration through a separate host-supplied source;
- performs bounded arbitration and returns a `RuntimeAssembly` only on convergence.

**Not implied by the current package:**
- downloading or localizing MicroBundle artifacts;
- verifying the bytes represented by a content hash;
- activating deferred bundles inside an already-running assembly;
- automatically refreshing, replacing, or retiring a running composition;
- scheduling application work, rendering UI, or owning the host lifecycle.

These distinctions separate implemented behavior from future architecture. See [Staged Manifest Loading](STAGED_MANIFEST_LOADING.md) and [Runtime Boundary](RUNTIME_BOUNDARY.md) for the precise current contract.

## Run the smallest example

Install Git and the **.NET 8 SDK**, then run:

```bash
git clone https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS.git
cd TheSingularityWorkshop.FSM_COS
dotnet run --project samples/FSM_COS.MinimalConsumer/FSM_COS.MinimalConsumer.csproj
```

The sample requests a report bundle that depends on a data-source bundle. Its in-memory catalog provides both. The expected final line is:

```text
Assembly order: 2 -> 1
```

The sample's catalog and bundles are example implementations, not built-in FSM_COS types. Read the [minimal consumer recipe](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/samples/FSM_COS.MinimalConsumer/Program.md) or continue to [Consuming MicroBundles](CONSUMING_MICROBUNDLES.md) to build your own.

## Where to go next

- [Consumer guide](CONSUMING_MICROBUNDLES.md) — build the catalog and manifest.
- [Architecture](ARCHITECTURE.md) — follow the responsibilities and composition algorithm.
- [Runtime Manifest](RUNTIME_MANIFEST.md) — inspect the request contract.
- [RuntimeAssembly](RUNTIME_ASSEMBLY.md) — inspect the handoff contract.
- [Arbitration](ARBITRATION.md) — understand the bounded reconciliation step.
- [Gallery](GALLERY.md) — view source-backed diagrams.
- [Documentation index](../DOCUMENTATION_INDEX.md) — browse the full documentation set.

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
