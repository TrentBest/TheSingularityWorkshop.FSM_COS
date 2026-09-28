# WebPage Integration

## Purpose

WebPage is the first concrete host for FSM_COS. The relationship is intentionally asymmetric:

- **FSM_COS** composes a runtime.
- **WebPage** supplies the bundles available to that runtime and decides how the assembled result is manifested.
- **TheSingularityWorkshop.GUI** constructs the semantic GUI.
- **Blazor** is the browser manifestation layer.
- **Experiences** are what the user encounters; they are not owned by the composition kernel.

The browser should therefore not become a dependency of FSM_COS.

## Package boundary

WebPage consumes the published package:

~~~xml
<PackageReference Include="TheSingularityWorkshop.FSM_COS" Version="0.1.0-alpha.1" />
~~~

The package currently targets .NET 8 and depends on FSM_API. A WebPage project may consume it because the composition kernel is written against platform-neutral .NET APIs.

A browser-specific adapter is **not** required merely to execute FsmCos.Execute(). An adapter becomes necessary at the point where the assembled result must interact with a host-specific lifecycle or rendering system.

## Composition flow

The first vertical slice should look like:

~~~text
                WebPage
                   │
                   │ RuntimeManifest
                   ▼
              FsmCos.Execute()
                   │
                   ▼
          IMicroBundleCatalog
                   │
        ┌──────────┼──────────┐
        ▼          ▼          ▼
      FSM_API    GUI       Experience
     bundles    bundles      bundles
                   │
                   ▼
             RuntimeAssembly
                   │
                   ▼
          WebPage host bridge
                   │
                   ▼
          GUI / Blazor browser
~~~

The important point is that FsmCos does not need to know which of those bundles represents a page, a GUI, an FSM, or an Experience.

## WebPage catalog

WebPage should own an IMicroBundleCatalog implementation.

A minimal catalog can begin as an in-memory registry:

~~~csharp
public sealed class WebPageMicroBundleCatalog : IMicroBundleCatalog
{
    private readonly IReadOnlyDictionary<ulong, IMicroBundle> _bundles;

    public WebPageMicroBundleCatalog(IEnumerable<IMicroBundle> bundles)
    {
        _bundles = bundles.ToDictionary(bundle => bundle.Id);
    }

    public bool TryResolve(ulong bundleId, out IMicroBundle? bundle) =>
        _bundles.TryGetValue(bundleId, out bundle);
}
~~~

This is deliberately a WebPage concern. A later catalog can resolve bundles from generated manifests, Warehouse-backed delivery, or another source without changing FsmCos.

## First host integration

The first WebPage integration should not attempt to make FSM_COS responsible for rendering.

Instead:

1. Construct the WebPage catalog.
2. Construct FsmCos with that catalog.
3. Create a RuntimeManifest.
4. Execute the manifest.
5. Inspect the resulting RuntimeAssembly.
6. Pass the assembled semantic components into the existing GUI/Experience host.
7. Let the host drive browser lifecycle and presentation.

Conceptually:

~~~csharp
var catalog = new WebPageMicroBundleCatalog(bundles);
var cos = new FsmCos(catalog);

var assembly = cos.Execute(manifest);

// Host-specific work begins here.
return webPageHost.Manifest(assembly);
~~~

Manifest above is intentionally a host operation, not an FSM_COS API.

## What the first manifest should prove

The first WebPage manifest should be small enough to test the architecture rather than the entire Workshop.

A useful proving slice is:

~~~text
WebPage Runtime
├── FSM/API capability
├── GUI capability
└── one Experience capability
~~~

The test should establish that:

- the manifest identifies the runtime;
- dependencies are discovered by FSM_COS;
- dependencies load before dependents;
- configuration reaches the owning bundle;
- arbitration converges;
- RuntimeAssembly contains the expected bundles;
- WebPage can consume the assembly without adding browser concerns to FSM_COS.

## Platform adapters

Do not create FSM_COS.Blazor, FSM_COS.Unity, FSM_COS.Windows, or similar packages just because the hosts exist.

Those packages would only be justified when a reusable **host contract** has emerged that belongs to more than one consuming application.

For now the cleaner boundary is:

~~~text
FSM_COS
  │
  ▼
RuntimeAssembly
  │
  ├── WebPage host bridge → Blazor
  ├── Unity host bridge   → Unity
  └── Desktop host bridge → desktop runtime
~~~

The host bridge may be application-specific initially. Reusable adapters can be extracted later when their contract is proven.

## Future Warehouse integration

FSM_COS already has the correct abstraction point for eventual Warehouse participation:

~~~text
RuntimeManifest
      ↓
   FsmCos
      ↓
IMicroBundleCatalog
      ↓
 Warehouse-backed resolver
      ↓
 MicroBundle
~~~

The Warehouse does not become part of the FSM_COS package. It supplies or backs the catalog/resolution boundary.

## Current limitation

0.1.0-alpha.1 is a composition kernel, not yet a complete runtime-host framework.

The next meaningful evolution is therefore not adding platform APIs to FSM_COS. It is proving the complete handoff:

~~~text
manifest
   ↓
composition
   ↓
RuntimeAssembly
   ↓
host manifestation
   ↓
observable WebPage
~~~

Once that slice works, the boundary can be evaluated from real usage instead of speculation.