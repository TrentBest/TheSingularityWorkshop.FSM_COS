# Runtime Manifest Theory

The Runtime Manifest is the handoff between authoring and assembly. It is a compiled request for a runtime, not an application configuration file.

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

The current contract is intentionally small: runtime identity plus BundleRequest values, where each request contains a machine ID and opaque configuration bytes.

This keeps FSM_COS independent from the serialization format selected by authoring/tooling.

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