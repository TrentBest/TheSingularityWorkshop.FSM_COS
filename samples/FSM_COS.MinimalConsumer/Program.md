![The Singularity Workshop animated moniker](https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Renderer/development/docs/assets/moniker.svg)

# Minimal Consumer Sample

**Source:** [`Program.cs`](Program.cs)  
**Source file blob:** `0e403eed8d81f041e8dbcd73878bbd0271fba45b`  
**Authorship:** manually authored and checked against the current `development` source; not generated from reflection.

## Purpose

Demonstrates the smallest useful consumer path: define two MicroBundles, declare that the requested report depends on a data source, resolve them through an in-memory `IMicroBundleCatalog`, submit a `RuntimeManifest`, and let `FsmCos.Execute` return a dependency-ordered `RuntimeAssembly`.

The sample is illustrative. It does not implement persistence, network resolution, application execution, or a production catalog.

## UML class card

```mermaid
classDiagram
    class Program {
        +Create DemoBundle instances
        +Create RuntimeManifest
        +Call FsmCos.Execute
        +Print assembly order
    }
    class DemoCatalog {
        -IReadOnlyDictionary~ulong, IMicroBundle~ _bundles
        +TryResolve(bundleId, version, bundle) bool
        +TryResolve(bundleId, bundle) bool
    }
    class DemoBundle {
        +MicroBundleDescriptor Descriptor
        +ulong Id
        +IReadOnlyList~MicroBundleDependencyRequest~ Dependencies
        +Load(context) void
        +Arbitrate(context, roundIndex) bool
    }
    class IMicroBundleCatalog {
        <<interface>>
    }
    class IMicroBundle
    class FsmCos {
        +Execute(manifest) RuntimeAssembly
    }
    class RuntimeManifest
    class RuntimeAssembly

    DemoCatalog ..|> IMicroBundleCatalog
    DemoBundle ..|> IMicroBundle
    Program --> DemoCatalog : supplies catalog
    Program --> RuntimeManifest : requests report
    Program --> FsmCos : executes composition
    FsmCos --> IMicroBundleCatalog : resolves dependencies
    FsmCos ..> RuntimeAssembly : returns
    DemoCatalog o-- DemoBundle : in-memory entries
```

The diagram summarizes the sample's local types and their roles. Public API signatures are abbreviated; the C# source and referenced package contracts remain authoritative.

## Use cases

1. **Run a minimal consumer:** restore and run the `FSM_COS.MinimalConsumer` project with the repository's documented development or published dependency mode.
2. **Observe dependency closure:** request only the report bundle and see the catalog resolve its prerequisite.
3. **Verify order:** inspect the printed assembly order; the data source must precede the report.
4. **Replace the catalog:** implement `IMicroBundleCatalog` against a real host-owned source without changing the manifest/composition boundary.

## Important boundaries

- The sample catalog is in-memory and resolves exact versions when requested.
- `DemoBundle.Load` writes a console message; this is sample behavior, not an FSM_COS logging promise.
- `Arbitrate` returns `false` to model a stable demo bundle.
- FSM_COS composes bundles and returns an assembly; the host owns execution and manifestation.

## Reproduce

Run `samples/FSM_COS.MinimalConsumer/FSM_COS.MinimalConsumer.csproj` under the repository's supported .NET SDK and dependency setup. CI's consumer verification is the authoritative automated check; this companion page is documentation, not evidence of a fresh run.

---

## 🔗 Resources & Support

- [FSM_COS](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS) · [FSM_API](https://github.com/TrentBest/FSM_API) · [FSM_Serialization](https://github.com/TrentBest/TheSingularityWorkshop.FSM_Serialization) · [WebPage](https://github.com/TrentBest/WebPage)
- [GitHub](https://github.com/TrentBest) · [Coder Legion](https://coderlegion.com/) · [Profile](https://coderlegion.com/user/The%2BSingularity%2BWorkshop) · [Patreon](https://www.patreon.com/c/TheSingularityWorkshop) · [PayPal](https://www.paypal.com/donate/?hosted_button_id=3Z7263LCQMV9J)

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar and brand image" width="200" /></a></p>
<p align="center"><em>The Singularity Workshop — Tools for the curious, the bold, and the systemically inclined.</em><br /><strong>Because state shouldn't be a mess.</strong><br /><em>And because static boundaries are invitations to cause trouble.</em></p>
