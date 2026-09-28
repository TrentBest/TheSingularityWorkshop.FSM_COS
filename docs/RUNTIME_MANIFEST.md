# Runtime Manifest

> The manifest is the request. [FSM_COS Theory](THEORY.md) explains why the request must remain distinct from the assembled runtime.

The **Runtime Manifest** is the published request that crosses from authoring/tooling into FSM_COS.

It answers one question:

> **What runtime composition is being requested?**

It does not contain the runtime itself, application lifecycle, GUI instructions, or host behavior.

![Runtime Manifest publication pipeline](assets/runtime-manifest-pipeline.svg)

## The actual alpha contract

The current implementation is deliberately small:

```csharp
public sealed record RuntimeManifest(
    ulong RuntimeId,
    IReadOnlyList<BundleRequest> Bundles);
```

Each root request is a `BundleRequest`:

```csharp
public readonly record struct BundleRequest(
    ulong BundleId,
    ReadOnlyMemory<byte> Configuration);
```

So, conceptually:

```text
RuntimeManifest
├── RuntimeId
└── Bundles
    ├── BundleId + Configuration
    ├── BundleId + Configuration
    └── BundleId + Configuration
```

The manifest names the **roots**. FSM_COS discovers the dependency closure from those roots.

## A real C# manifest

The alpha API can construct a manifest directly:

```csharp
var manifest = new RuntimeManifest(
    RuntimeId: 1001,
    Bundles:
    [
        BundleRequest.Unconfigured(10),
        new BundleRequest(
            BundleId: 20,
            Configuration: new byte[] { 0x01, 0x02, 0x03 })
    ]);
```

Here:

- `1001` identifies the runtime being assembled.
- Bundle `10` is requested without configuration.
- Bundle `20` is requested with opaque configuration bytes.
- FSM_COS does **not** interpret the bytes as JSON, XML, YAML, or any domain-specific format.
- The MicroBundle that owns the configuration interprets it during `Load()`.

## A conceptual serialized form

FSM_COS currently does **not** prescribe a serialization format. The following is therefore an illustrative representation, not a wire-format contract:

```json
{
  "runtimeId": 1001,
  "bundles": [
    {
      "bundleId": 10,
      "configuration": null
    },
    {
      "bundleId": 20,
      "configuration": "AQID"
    }
  ]
}
```

The important part is the semantic shape, not the spelling of the serialization.

**This is where the architecture intentionally hands off to [TheSingularityWorkshop.FSM_Serialization](https://github.com/TrentBest/TheSingularityWorkshop.FSM_Serialization).** FSM_COS does not need, and should not grow, a second serialization framework. If this conceptual representation becomes a concrete binary representation, the serialization boundary belongs to [FSM_Serialization](https://www.nuget.org/packages/TheSingularityWorkshop.FSM_Serialization).

An authoring system may eventually publish JSON, binary data, generated C#, a compact manifest format, or another representation. FSM_COS only needs the runtime contract represented by `RuntimeManifest`.

> **Representation is not the RuntimeManifest itself. The bytes are a representation of the semantic request. FSM_COS owns what that request means for composition; FSM_Serialization owns the byte boundary.**

For the deeper architectural treatment, see [FSM_COS Theory — Composition is not serialization](THEORY.md#14-composition-is-not-serialization) and [FSM_Serialization Theory](https://github.com/TrentBest/TheSingularityWorkshop.FSM_Serialization/blob/master/docs/THEORY.md).

## Authoring → publication

The editor may know vastly more than the runtime manifest needs:

```text
rich authoring model
        │
        ├── names
        ├── ontology
        ├── variants
        ├── dependency graph
        ├── provenance
        └── visual/editor metadata
        │
        ▼
     validation
        │
        ▼
 dependency closure
        │
        ▼
 baked IDs + opaque configuration
        │
        ▼
 RuntimeManifest
```

This is an intentional compression boundary.

The manifest should be **small enough to publish and stable enough to consume**, without becoming a second copy of the editor.

## Roots versus dependencies

Suppose the manifest requests:

```text
A
├── B
│   └── C
└── D
```

The manifest only needs to name the requested root:

```text
[A]
```

FSM_COS discovers the rest:

```text
C → B → D → A
```

The resulting order is an implementation consequence of the dependency graph, not something the manifest author has to manually encode.

See [Dependency Resolution](ARCHITECTURE.md#dependency-resolution) for the runtime behavior.

## What does not belong in a manifest?

A Runtime Manifest should not quietly become:

- an application configuration file;
- a GUI layout;
- a browser lifecycle description;
- a Unity scene;
- a Warehouse database;
- an Experience execution script;
- a serialized RuntimeAssembly.

Those concerns belong to other layers.

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
