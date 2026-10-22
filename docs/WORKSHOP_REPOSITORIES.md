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

## Visual explanation and application experiences

| Project | Why you would use it |
|---|---|
| [GUI](https://github.com/TrentBest/TheSingularityWorkshop.GUI) | Build GUI structures and connect them to rendering implementations. |
| [Visualize](https://github.com/TrentBest/TheSingularityWorkshop.Visualize) | Explore visual representations and diagrams that help people understand systems and their relationships. |
| [Renderer](https://github.com/TrentBest/TheSingularityWorkshop.Renderer) | Present structured information as readable text, including output useful to AI-oriented workflows. |
| [WebPage](https://github.com/TrentBest/WebPage) | Explore the Workshop's web-hosted experience and living-page direction. |

## Choose your route

- **I want a small, focused library:** start with the individual package and its consuming guide.
- **I need reusable runtime composition:** begin with [FSM_COS](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS) and its [minimal consumer](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/tree/development/samples/FSM_COS.MinimalConsumer).
- **I need AI protocol vocabulary or structure:** read [ProtocolAi](https://github.com/TrentBest/TheSingularityWorkshop.ProtocolAi) first for WHAT, then [GrammarAi](https://github.com/TrentBest/TheSingularityWorkshop.GrammarAi) for HOW.
- **I want to inspect the broader source:** [Browse Trent Best's repositories on GitHub](https://github.com/TrentBest?tab=repositories).

---

<p align="center">
  <a href="https://github.com/TrentBest">
    <img src="https://github.com/TrentBest.png?size=200" alt="The Singularity Workshop on GitHub" width="200" />
  </a>
</p>

<p align="center">
  <a href="https://github.com/TrentBest">GitHub</a> ·
  <a href="https://coderlegion.com/">Coder Legion</a> ·
  <a href="https://www.patreon.com/c/TheSingularityWorkshop">Patreon</a> ·
  <a href="https://www.paypal.com/donate/?hosted_button_id=3Z7263LCQMV9J">PayPal</a>
</p>
