# Runtime Manifest

> **The manifest is the request. FSM_COS turns that request into a composition; it does not treat the manifest as an application configuration file.**

The **Runtime Manifest** is the published request that crosses from authoring/tooling into FSM_COS.

It answers one question:

> **What runtime composition is being requested?**

It does not contain the runtime itself, application lifecycle, GUI instructions, host behavior, or the complete dependency graph.

![Runtime Manifest publication pipeline](assets/runtime-manifest-pipeline.svg)

## The actual alpha contract

The current implementation is deliberately small:

```csharp
public sealed record RuntimeManifest(
    ulong RuntimeId,
    IReadOnlyList<MicroBundleDependencyRequest> Bundles);
```

The request type is owned by [MicroBundleDomain](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleDomain).

Each root request identifies a MicroBundle and may carry opaque configuration:

```text
RuntimeManifest
├── RuntimeId
└── Bundles
    ├── BundleId + Configuration
    ├── BundleId + Configuration
    └── BundleId + Configuration
```

The manifest names the **roots**. FSM_COS discovers the dependency closure from those roots.

## Why the request type is domain-owned

The same dependency-request concept appears in two places:

```text
MicroBundle
    │
    └── declares MicroBundleDependencyRequest
                         ▲
                         │
                  RuntimeManifest
                         ▲
                         │
                      FSM_COS
```

That is intentional.

A capability author should not have to define one request type for MicroBundleDomain and another request type for FSM_COS. The composition host consumes the same contract that the capability declares.

This also keeps FSM_COS from inventing a competing MicroBundle domain model.

## A conceptual manifest

The semantic shape can be represented however an authoring or transport system requires:

```text
RuntimeId
  └── root dependency requests
          ├── identity
          └── opaque configuration
```

FSM_COS does **not** prescribe JSON, XML, YAML, binary, or another wire format.

If the semantic request crosses a concrete byte/serialization boundary, use [FSM_Serialization](https://github.com/TrentBest/TheSingularityWorkshop.FSM_Serialization). FSM_COS consumes the semantic runtime request; it does not become the serialization framework.

## Authoring → publication

An editor may know substantially more than the runtime needs:

```text
rich authoring model
        │
        ├── names
        ├── ontology
        ├── variants
        ├── provenance
        ├── dependency relationships
        └── visual/editor metadata
        │
        ▼
     validation
        │
        ▼
 published root requests
        │
        ▼
 RuntimeManifest
```

This is an intentional publication boundary.

The manifest should be **small enough to publish and stable enough to consume** without becoming a second copy of the authoring system.

## Roots versus dependencies

Suppose the manifest requests:

```text
A
├── B
│   └── C
└── D
```

The manifest only needs to name the root:

```text
[A]
```

FSM_COS discovers:

```text
C → B → D → A
```

The resulting order is derived from the dependency graph, not manually encoded into the manifest.

See [Dependency Resolution](ARCHITECTURE.md#dependency-resolution).

## What does not belong in a manifest?

A Runtime Manifest should not quietly become:

- an application configuration file;
- a GUI layout;
- a browser lifecycle description;
- a Unity scene;
- a Warehouse database;
- an Experience execution script;
- a serialized RuntimeAssembly.

The manifest requests.

**FSM_COS composes.**

The host manifests the result.

## Stability

A useful long-term property is:

> The same semantic manifest should produce the same composition when resolved against the same compatible catalog and bundle versions.

That does not mean every host must render the result identically. It means the composition request remains meaningful when moved between hosts.

## Related concepts

- [FSM_COS Architecture](ARCHITECTURE.md)
- [MicroBundles](MICROBUNDLES.md)
- [RuntimeAssembly](RUNTIME_ASSEMBLY.md)
- [Arbitration and Convergence](ARBITRATION.md)


---

## 🔗 The Singularity Workshop

FSM_COS is one layer in a deliberately troublesome ecosystem:

- **[FSM_API](https://github.com/TrentBest/FSM_API)** — behavior and state.
- **[FSM_COS](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS)** — composition and runtime assembly.
- **[FSM_Serialization](https://github.com/TrentBest/TheSingularityWorkshop.FSM_Serialization)** — representation and the byte boundary.
- **[WebPage](https://github.com/TrentBest/WebPage)** — browser manifestation and proving ground.
- **[FSM_API_Unity](https://github.com/TrentBest/FSM_API_Unity)** — Unity manifestation.

<p align="center"><em>The Singularity Workshop — Tools for the curious, the bold, and the systemically inclined.</em><br><strong>Because state shouldn't be a mess.</strong><br><em>And because static boundaries are invitations to cause trouble.</em></p>
