# Runtime Manifest Theory

> The manifest is the published request entering the composition kernel. For the larger architectural model, see [FSM_COS Theory](THEORY.md).

The [Runtime Manifest](RUNTIME_MANIFEST.md) is the handoff between authoring and assembly. It is a compiled request for a runtime, not an application configuration file.

![Runtime Manifest publication pipeline](assets/runtime-manifest-pipeline.svg)

## Editor time

Authoring systems may know human-readable names, ontology relationships, variants, dependencies, visual structure, provenance, and other rich relationships.

FSM_COS does not require those structures to survive intact.

## Publication

    rich editor model
          ↓
      validation
          ↓
  dependency closure
          ↓
  baked IDs/configuration
          ↓
    RuntimeManifest

The manifest is therefore a publication artifact. It contains enough information for composition to be deterministic.

## Runtime representation

A RuntimeManifest is a semantic composition request. Its eventual JSON, binary, generated-code, or other representation is a separate concern.

The current contract is intentionally small: runtime identity plus MicroBundleDependencyRequest values, where each request contains a machine ID and opaque configuration bytes.

This keeps FSM_COS independent from the serialization format selected by authoring/tooling.

When that representation becomes a concrete byte-level contract, the reusable serialization boundary belongs to [TheSingularityWorkshop.FSM_Serialization](https://github.com/TrentBest/TheSingularityWorkshop.FSM_Serialization). See its [serialization theory](https://github.com/TrentBest/TheSingularityWorkshop.FSM_Serialization/blob/master/docs/THEORY.md).

## Why not put everything in the manifest?

The manifest is a request, not the assembled runtime.

It should not become a duplicate MicroBundle implementation, a renderer description, a host lifecycle, a Warehouse database, or a second FSM system.

It identifies what is required and supplies the data needed to compose it.

## Manifest stability

A useful long-term property is:

> The same semantic manifest should produce the same composition when resolved against the same catalog and compatible bundle versions.

This does not require every host to render or execute the result identically. It requires the composition boundary to remain meaningful across hosts.

## Manifest and the Warehouse

The Warehouse can eventually provide the data needed to resolve a manifest, but that does not make the Warehouse part of the manifest contract.

    Manifest
       │
       ▼
    FSM_COS
       │
       ▼
 Catalog / resolver
       │
       ▼
    Warehouse

The manifest requests. The resolver locates. The Warehouse delivers. FSM_COS assembles.

## Manifest and Experiences

An Experience may be represented by a manifest, but FSM_COS does not become the Experience.

A manifest can request the MicroBundles needed to construct an Experience. The host decides how that Experience is executed and encountered.

This distinction prevents composition from becoming presentation.

---

## 🔗 The Singularity Workshop

FSM_COS is one layer in a deliberately troublesome ecosystem:

- **[FSM_API](https://github.com/TrentBest/FSM_API)** — behavior and state.
- **[FSM_COS](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS)** — composition and runtime assembly.
- **[FSM_Serialization](https://github.com/TrentBest/TheSingularityWorkshop.FSM_Serialization)** — representation and the byte boundary.
- **[WebPage](https://github.com/TrentBest/WebPage)** — browser manifestation and proving ground.

<p align="center"><em>The Singularity Workshop — Tools for the curious, the bold, and the systemically inclined.</em><br><strong>Because state shouldn't be a mess.</strong><br><em>And because static boundaries are invitations to cause trouble.</em></p>
