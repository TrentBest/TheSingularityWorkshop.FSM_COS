<!-- The Singularity Workshop moniker -->
<p align="center">
  <a href="https://github.com/TrentBest">
    <img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" />
  </a>
</p>

# The Singularity Workshop — Repository Guide

The Singularity Workshop is a set of independently useful libraries and applications for describing capabilities, composing runtime behavior, communicating with AI endpoints, and presenting software through visual interfaces. Each repository has a distinct job; use the project that solves the problem in front of you rather than adopting the entire ecosystem.

**Prefer packages?** [Search The Singularity Workshop on NuGet](https://www.nuget.org/packages?q=TheSingularityWorkshop).

## Composition and runtime foundations

| Project | Why you would use it |
|---|---|
| [FSM_API](https://github.com/TrentBest/FSM_API) | Finite-state-machine and state/context primitives for applications that need explicit, controllable state transitions. |
| [MicroBundleDomain](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleDomain) | The domain contract for describing a reusable MicroBundle capability. |
| [FSM_COS](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS) | Resolve requested MicroBundles and their dependencies into a `RuntimeAssembly` for a host. |
| [FSM_Serialization](https://github.com/TrentBest/TheSingularityWorkshop.FSM_Serialization) | Serialization and representation support for the Workshop's state and composition data. |

## AI-facing structure and communication

| Project | Why you would use it |
|---|---|
| [HeadlessAi](https://github.com/TrentBest/TheSingularityWorkshop.HeadlessAi) | Send inputs to configured AI-capable HTTP endpoints through explicit protocol adapters, without a browser or provider SDK in the direct HTTP path. |
| [ProtocolAi](https://github.com/TrentBest/TheSingularityWorkshop.ProtocolAi) | Define stable, integer-backed protocol identities and the vocabulary that gives symbols their meaning. |
| [GrammarAi](https://github.com/TrentBest/TheSingularityWorkshop.GrammarAi) | Describe how symbols can be arranged into ordered grammar productions while keeping external vocabulary ownership separate. |
| [CommandAi](https://github.com/TrentBest/TheSingularityWorkshop.CommandAi) | Represent proposed commands as bounded, versioned data for a host to inspect; it does not resolve authority or execute commands. |

### AI lineage and agent onboarding

The AI-facing packages are complementary layers, not one autonomous authority chain:

```text
HeadlessAi   -> configured endpoint request/response transport
ProtocolAi   -> application-owned vocabulary and symbol identity
GrammarAi    -> structural composition over protocol references
CommandAi    -> versioned command-proposal representation
Host         -> interpretation, trust, policy, authorization, approval, execution
```

Start with each repository's [agent instructions](https://github.com/TrentBest/TheSingularityWorkshop.HeadlessAi/blob/development/AGENTS.md), [ProtocolAi agent instructions](https://github.com/TrentBest/TheSingularityWorkshop.ProtocolAi/blob/development/AGENTS.md), [GrammarAi agent instructions](https://github.com/TrentBest/TheSingularityWorkshop.GrammarAi/blob/development/AGENTS.md), and [CommandAi agent instructions](https://github.com/TrentBest/TheSingularityWorkshop.CommandAi/blob/development/AGENTS.md). These are repository-specific onboarding guides subordinate to the [FSM_COS Documentation Standard](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/DOCUMENTATION_STANDARD.md). Verify each repository's current source, tests, and CI before assuming cross-package interoperability.

## Visual explanation and application experiences

| Project | Why you would use it |
|---|---|
| [GUI](https://github.com/TrentBest/TheSingularityWorkshop.GUI) | Build GUI structures and connect them to rendering implementations. |
| [Visualize](https://github.com/TrentBest/TheSingularityWorkshop.Visualize) | Explore visual representations and diagrams that help people understand systems and their relationships. |
| [Renderer](https://github.com/TrentBest/TheSingularityWorkshop.Renderer) | Present structured information as readable text, including output useful to AI-oriented workflows. |
| [WebPage](https://github.com/TrentBest/WebPage) | Explore the Workshop's web-hosted experience and living-page direction. |

## Artifact delivery, execution, and host integration

The following map is a source-grounded integration guide, not a claim that every proposed connection is already implemented. **Current direct package dependencies are distinguished from intended consumer relationships.** Reviewed against the FSM_COS `development` source on 2026-11-13.

### Verified FSM_COS package dependencies

The current `src/FSM_COS/FSM_COS.csproj` declares these direct package dependencies:

| Package / repository | Version | Responsibility at the FSM_COS boundary |
|---|---:|---|
| [FSM_API](https://github.com/TrentBest/FSM_API) | `1.0.13` | State-machine and context primitives used by the composition runtime. |
| [FSM_UserIO](https://github.com/TrentBest/FSM_UserIO) | `0.1.0-alpha.1` | Platform-neutral semantic interaction contracts; FSM_COS carries optional `SemanticIntent` through the manifest and assembly. |
| [MicroBundleDomain](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleDomain) | `1.0.1` | MicroBundle identity, dependency requests, providers, and domain-owned capability contracts. |

These are the only direct runtime package references in the inspected project file. A repository appearing elsewhere in this guide is **not** thereby a dependency of FSM_COS.

### Artifact and composition boundary

| Project | Owns | Relationship to FSM_COS |
|---|---|---|
| [MicroBundleRepository](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleRepository) | Immutable artifact addresses, content integrity, storage, retrieval, publication pointers, and catalogs. | Supplies verified artifact bytes through a host/catalog adapter. It does not own MicroBundle dependency closure or runtime composition. |
| [FSM_COS](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS) | Dependency closure, manifest validation, load ordering, bounded arbitration, and `RuntimeAssembly`. | The composition boundary. It does not own durable artifact storage or the application's execution loop. |
| [FSM_Foundry](https://github.com/TrentBest/TheSingularityWorkshop.FSM_Foundry) | Execution/stepping, workflow lifecycle, dispatch, cancellation, and execution outcomes. | Intended downstream execution owner for composed work. The RuntimeAssembly/scheduling-intent handoff is being coordinated; do not assume a direct package dependency is already present. |
| [FSM_REST](https://github.com/TrentBest/TheSingularityWorkshop.FSM_REST) | HTTP transport and optional REST adapters. | Can transport artifact/catalog operations; it is not the artifact authority or composition engine. |
| [FSM_Serialization](https://github.com/TrentBest/TheSingularityWorkshop.FSM_Serialization) | Serialization and representation support. | Remains independently usable; composition begins at FSM_COS rather than creating an inverse dependency. |

The intended artifact-to-runtime path is:

```text
Experience publication
        |
        v
MicroBundleRepository
  immutable artifact bytes + identity
        |
        v
host / catalog adapter
  retrieval + verification + materialization
        |
        v
FSM_COS
  dependency closure + composition
        |
        v
RuntimeAssembly
        |
        v
FSM_Foundry / host execution
```

The repository supplies content; the adapter proves and materializes it; FSM_COS composes it; Foundry owns execution. These boundaries must not collapse into package-to-package coupling.

### Experience authorship and hosts

| Project | Owns | Relationship to FSM_COS |
|---|---|---|
| [Experiences](https://github.com/TrentBest/TheSingularityWorkshop.Experiences) | Experience identity, published manifests/artifacts, lifecycle semantics, and domain-specific behavior. | Produces the manifest and requirements a host supplies to the composition boundary. |
| [StoryBuilder](https://github.com/TrentBest/StoryBuilder) | Narrative authoring, canonical story records, provenance, review, and publication. | Produces authored content/artifacts; it does not own MicroBundle composition or a host runtime. |
| [AnyApp](https://github.com/TrentBest/AnyApp) | Desktop host, user/session context, trust, policy, authorization, native catalog/materialization, and runtime hosting. | A host-side consumer of Experience manifests and FSM_COS composition; host authority stays in AnyApp. |
| [WebPage](https://github.com/TrentBest/WebPage) | Web-hosted Experience selection, presentation, and activation. | A composition consumer/host; it does not become artifact authority or duplicate FSM_COS dependency resolution. |
| [MyVR](https://github.com/TrentBest/TheSingularityWorkshop.MyVR) | VR device/session lifecycle, spatial interaction, and immersive presentation. | A host manifestation of an Experience, not a second story or composition runtime. |

### Optional interaction, presentation, and automation

| Project | FSM_UserIO relationship / integration status |
|---|---|
| [FSM_UserIO](https://github.com/TrentBest/FSM_UserIO) | Host-neutral semantic interaction contracts. The package has no package references of its own; it must not acquire dependencies on FSM_COS, GUI, AI packages, domain packages, or host frameworks just to participate. |
| [AnyApp](https://github.com/TrentBest/AnyApp), [WebPage](https://github.com/TrentBest/WebPage), [MyVR](https://github.com/TrentBest/TheSingularityWorkshop.MyVR) | Host-level integration candidates for semantic input, intent, and feedback where a concrete interaction path requires them. Each host retains its own session, trust, and authorization policy. |
| [GUI](https://github.com/TrentBest/TheSingularityWorkshop.GUI) | Optional UI structure/presentation integration; not a dependency of the composition kernel or FSM_UserIO. |
| [Renderer](https://github.com/TrentBest/TheSingularityWorkshop.Renderer), [Visualize](https://github.com/TrentBest/TheSingularityWorkshop.Visualize) | Composable text/visual manifestation capabilities. Their intended consumption model is MicroBundles, not NuGet package dependencies from FSM_COS. |
| [HeadlessAi](https://github.com/TrentBest/TheSingularityWorkshop.HeadlessAi), [ProtocolAi](https://github.com/TrentBest/TheSingularityWorkshop.ProtocolAi), [GrammarAi](https://github.com/TrentBest/TheSingularityWorkshop.GrammarAi), [CommandAi](https://github.com/TrentBest/TheSingularityWorkshop.CommandAi) | Independent AI-facing capabilities. Grammar validation may be useful at an authoring/publish boundary, but FSM_COS must not depend on AI packages to validate authority, execute commands, or compose runtime capabilities. |
| [CiAi](https://github.com/TrentBest/TheSingularityWorkshop.CiAi) | Build/test evidence observation and development automation. It is tooling, not a runtime dependency. |

### Dependency-direction rules

- FSM_COS depends downward on FSM_API, FSM_UserIO, and MicroBundleDomain. Those contracts do not depend upward on FSM_COS.
- MicroBundleRepository does not depend on FSM_COS; a host/catalog adapter bridges verified artifacts into the composition contract.
- FSM_COS does not depend on FSM_REST, MicroBundleRepository, FSM_Foundry, AnyApp, WebPage, MyVR, GUI, Renderer, Visualize, or the AI packages.
- FSM_COS returns a `RuntimeAssembly`; Foundry owns scheduling and execution lifecycle. FSM_COS must not grow a global scheduler, host loop, or workflow orchestrator.
- A proposed integration is not an approved package edge. Verify the owning repository's source, tests, and CI before changing a dependency graph.

## Choose your route

- **I want a small, focused library:** start with the individual package and its consuming guide.
- **I need reusable runtime composition:** begin with [FSM_COS](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS) and its [minimal consumer](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/tree/development/samples/FSM_COS.MinimalConsumer).
- **I need AI protocol vocabulary or structure:** read [ProtocolAi](https://github.com/TrentBest/TheSingularityWorkshop.ProtocolAi) first for WHAT, then [GrammarAi](https://github.com/TrentBest/TheSingularityWorkshop.GrammarAi) for HOW.
- **I want to inspect the broader source:** [Browse Trent Best's repositories on GitHub](https://github.com/TrentBest?tab=repositories).

---

<p align="center">
  <a href="https://github.com/TrentBest">GitHub</a> ·
  <a href="https://coderlegion.com/">Coder Legion</a> ·
  <a href="https://www.patreon.com/c/TheSingularityWorkshop">Patreon</a> ·
  <a href="https://www.paypal.com/donate/?hosted_button_id=3Z7263LCQMV9J">PayPal</a>
</p>

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
