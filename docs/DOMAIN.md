# FSM_COS Domain

> **FSM_COS owns the composition of computation. It does not own the application that consumes the result.**

This is the domain map for this package. Concepts owned by neighboring packages are named only to explain the boundary across which FSM_COS consumes them.

## What FSM_COS owns

FSM_COS owns the operational problem of taking a computation request and producing a stable assembled computation.

- runtime composition requests;
- MicroBundle root selection and requested versions;
- dependency closure;
- composition-time loading;
- optional configuration delivery;
- arbitration rounds;
- convergence;
- the RuntimeAssembly handoff.

The kernel is deliberately **platform-neutral and application-neutral**.

## What computation platform means

FSM_COS is common operational machinery that can sit beneath many kinds of digital systems.

```text
                computation request
                       |
                       v
                    FSM_COS
                       |
          +------------+------------+
          v            v            v
       WebApp       AnyApp      DistributedApp
          |            |            |
          +------------+------------+
                       v
              any digital system
```

Those are examples, not special cases. FSM_COS does not contain a browser model, desktop model, Unity model, renderer model, spreadsheet model, game model, business-application model, or distributed-systems product model.

## The domain seam

FSM_COS consumes neighboring contracts rather than redefining neighboring domains.

```text
FSM_API ───────── state/context primitive ─────────────┐
MicroBundleDomain ─ MicroBundle runtime contract ──────┤
FSM_UserIO ──────── optional SemanticIntent value ─────┤
catalog / resolver ─ resolved MicroBundles ────────────┤
configuration source ─ optional configuration bytes ──┤
                                                       ▼
                                                     FSM_COS
                                                       │
                                                       ▼
                                                 RuntimeAssembly
```

FSM_COS does not define what a MicroBundle is. It consumes the runtime contract it needs to resolve, load, and arbitrate one.

Likewise, the repository/catalog owns artifact location and delivery. FSM_COS consumes the resolver boundary; it does not become the repository.

## Composition lifecycle

```text
RuntimeManifest
      | MicroBundle IDs + requested versions
      v
catalog / resolver
      | resolved MicroBundles
      v
dependency closure
      v
Load()
      v
Arbitrate()
      |
      +-- changed -> next round
      |
      +-- stable
             v
      RuntimeAssembly
```

The result is deliberately a handoff object. FSM_COS does not continue by becoming the user's application.

## Configuration boundary

Configuration is separate from the manifest.

```text
Manifest
  = which MicroBundles + requested versions

Configuration source
  = optional configuration for a particular MicroBundle

FSM_COS
  = delivers available configuration and performs composition

MicroBundle
  = interprets its own configuration
```

If configuration is absent, the MicroBundle receives no external configuration and uses its defaults.

FSM_COS does not prescribe whether configuration comes from a file, blob, repository artifact, generated resource, or another application-owned mechanism.

## What FSM_COS does not own

- MicroBundle definition and domain meaning;
- MicroBundle artifact storage;
- REST transport;
- serialization formats;
- GUI semantics or rendering;
- browser lifecycle;
- desktop lifecycle;
- Unity lifecycle;
- Experience semantics;
- application-specific scheduling;
- application configuration frameworks;
- databases or Warehouse infrastructure;
- credentials and provider sessions;
- networking protocols;
- product-specific behavior.

These can participate in a composition. They do not redefine the composition kernel.

## Developer use

A developer supplies a catalog, manifest, and optional configuration source appropriate to their system, then uses the kernel without adopting a particular application framework. The [Minimal Consumer sample](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/samples/FSM_COS.MinimalConsumer/Program.cs) provides a complete source-repository example.

The snippet below is schematic: `MyCatalog` and `MyConfigurationSource` are placeholders for implementations supplied by your application, not types included in FSM_COS.

```csharp
var catalog = new MyCatalog();
var configuration = new MyConfigurationSource();
var cos = new FsmCos(catalog);

var assembly = cos.Execute(manifest, configuration);
```

The developer remains free to decide where MicroBundles come from, how configuration is stored, how the assembly is executed, how it is exposed, whether there is a UI, and whether execution is local, remote, distributed, interactive, or headless.

FSM_COS supplies the composition machinery, not those application decisions.

## Documentation rule

If a concept requires a long explanation of its own domain, that explanation belongs in the package that owns the concept.

FSM_COS should instead explain:

1. what contract it consumes;
2. why it consumes it;
3. what FSM_COS does with it;
4. what crosses the boundary;
5. what remains outside FSM_COS.

This keeps package documentation useful without turning every README into an encyclopedia of the entire Workshop.

---

<p align="center"><em>The Singularity Workshop — Tools for the curious, the bold, and the systemically inclined.</em><br><strong>Because state shouldn't be a mess.</strong><br><em>And because static boundaries are invitations to cause trouble.</em></p>
