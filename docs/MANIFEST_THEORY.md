<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop — animated workshop moniker" width="720" /></a></p>

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
  baked MicroBundle IDs + versions
          ↓
    RuntimeManifest

The manifest is therefore a publication artifact. It contains enough information for composition to be deterministic.

## Runtime representation

A RuntimeManifest is a semantic composition request. Its eventual JSON, binary, generated-code, or other representation is a separate concern.

The required composition request remains small: runtime identity plus versioned MicroBundle root entries. The current source contract also exposes optional fields for a host-provided state context, semantic intent, and staged load-plan/schedule metadata. These optional fields have distinct purposes: the context can be shared with arbitration, intent is passed through to `RuntimeAssembly`, and a supplied schedule is checked against the catalog's resolved dependency graph before loading. They do not turn the manifest into an application configuration file or make FSM_COS responsible for artifact localization, serialization, or background execution.

Configuration remains intentionally outside the manifest. This keeps FSM_COS independent from the serialization format selected by authoring/tooling.

When that representation becomes a concrete byte-level contract, the reusable serialization boundary belongs to [TheSingularityWorkshop.FSM_Serialization](https://github.com/TrentBest/TheSingularityWorkshop.FSM_Serialization). See its [serialization theory](https://github.com/TrentBest/TheSingularityWorkshop.FSM_Serialization/blob/master/docs/THEORY.md).

## Why not put everything in the manifest?

The manifest is a request, not the assembled runtime.

It should not become a duplicate MicroBundle implementation, a renderer description, a host lifecycle, a Warehouse database, or a second FSM system.

It identifies what MicroBundles and versions are required. A separate configuration source supplies optional per-MicroBundle configuration.

## Manifest stability

A useful long-term property is:

> The same semantic manifest should produce the same composition when resolved against the same catalog and compatible bundle versions.

This does not require every host to render or execute the result identically. It requires the composition boundary to remain meaningful across hosts.

A published manifest is an immutable, versioned request snapshot. A newer manifest is a new request; it does not mutate an already assembled Experience. The current FSM_COS package does not poll or stream updated manifests, switch a running composition to new bundle versions, or retire old bundle instances. Host-owned refresh and side-by-side replacement are outside the current package contract.

## Manifest and artifact resolution

The manifest describes what is requested; it does not locate or deliver artifacts. A host-supplied catalog resolves the requested MicroBundle identities and versions, while an optional configuration source supplies per-bundle configuration.

    RuntimeManifest
         │
         ▼
    Catalog / resolver ───── Configuration source
         │                         │
         ▼                         ▼
    MicroBundle instance       optional config
         │                         │
         └──────────┬──────────────┘
                    ▼
                 FSM_COS

Artifact storage and delivery remain outside the FSM_COS package.

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
- **[FSM_API_Unity](https://github.com/TrentBest/FSM_API_Unity)** — Unity manifestation.

<p align="center"><em>The Singularity Workshop — Tools for the curious, the bold, and the systemically inclined.</em><br><strong>Because state shouldn't be a mess.</strong><br><em>And because static boundaries are invitations to cause trouble.</em></p>

---

## 🔗 Resources & Support

- **Project:** [FSM_COS](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS)
- **Workshop projects:** [FSM_API](https://github.com/TrentBest/FSM_API) · [FSM_Serialization](https://github.com/TrentBest/TheSingularityWorkshop.FSM_Serialization) · [WebPage](https://github.com/TrentBest/WebPage)
- **Connect:** [GitHub](https://github.com/TrentBest) · [Coder Legion profile](https://coderlegion.com/user/The%2BSingularity%2BWorkshop) · [Coder Legion community](https://coderlegion.com/)
- **Support:** [Patreon](https://www.patreon.com/c/TheSingularityWorkshop) · [PayPal](https://www.paypal.com/donate/?hosted_button_id=3Z7263LCQMV9J)

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
