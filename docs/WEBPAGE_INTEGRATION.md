# Host Integration Boundary: WebPage

FSM_COS is a platform-neutral composition library. It does not ship a WebPage-specific catalog, Blazor adapter, application lifecycle manager, or rendering component. A WebPage host can consume FSM_COS by implementing the host-owned contracts and deciding how to use the resulting `RuntimeAssembly`.

## Responsibility boundary

- **WebPage / host** — owns the application lifecycle, user interface, routing, permissions, execution, and presentation.
- **Catalog / resolver** — locates and returns the MicroBundle instances requested by identity and version.
- **Configuration source** — optionally supplies per-bundle configuration.
- **FSM_COS** — validates the request and dependency closure, loads dependencies before dependents, runs bounded arbitration, and returns a `RuntimeAssembly` only after convergence.
- **MicroBundleDomain** — defines the MicroBundle contract and the domain behavior of each bundle.

FSM_COS does not discover artifacts by itself. A host-supplied `IMicroBundleCatalog` is the resolution boundary; the package contains no built-in Warehouse-backed or WebPage-specific resolver.

## Host integration sequence

1. Implement `IMicroBundleCatalog` for the host's source of MicroBundles.
2. Construct a `RuntimeManifest` with the requested root identities and versions.
3. Supply an optional `IMicroBundleConfigurationSource` when the host has external configuration.
4. Call `FsmCos.Execute`.
5. Use the returned `RuntimeAssembly` according to the host's own lifecycle and presentation rules.

The [minimal consumer sample](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/samples/FSM_COS.MinimalConsumer/Program.cs) demonstrates the current dependency-first composition path with an in-memory catalog. It is a source-repository sample, not a built-in WebPage integration.

## Package dependencies

The source candidate `0.1.0-alpha.7` targets .NET 8 and declares direct dependencies on FSM_API 1.0.13, MicroBundleDomain 1.0.1, and FSM_UserIO 0.1.0-alpha.1. FSM_UserIO supplies the optional `SemanticIntent` value exposed by `RuntimeManifest` and carried to `RuntimeAssembly`; consumers must restore that dependency even when they do not supply an intent.

A source project's declared version does not prove that the same version is published. Check the [NuGet package page](https://www.nuget.org/packages/TheSingularityWorkshop.FSM_COS) before selecting a package version.

## Current limits

The current kernel performs synchronous composition. It does not perform repository localization, verify artifact content hashes, continue loading in the background, promote deferred bundles into an already-running assembly, refresh a running manifest, replace bundle generations, or manage host lifecycle and retirement.

These limits apply to FSM_COS itself. A host may implement additional behavior around the returned assembly, but must define and test that behavior as a host-owned contract rather than assuming FSM_COS supplies it.
