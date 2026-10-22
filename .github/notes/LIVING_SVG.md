<!-- The Singularity Workshop moniker -->
<p align="center">
  <a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop — animated workshop moniker" width="720" /></a>
</p>

> **Internal documentation-engineering note.** This describes the current diagram artifact and verification boundaries, not an end-user FSM_COS feature.

# Living SVG: From Contract Map to Living Picture

**Status:** first animated, source-backed contract diagram. It is not a live runtime trace, an interactive application, or proof of browser/assistive-technology verification.

## What is implemented now

FSM_COS's package workflow pins GUI source commit `6fd9632547421034212f9a64a2e429b86d4bab4a` and uses its `SvgGuiRenderer` to generate `docs/assets/fsm-cos-preflight.svg` from `samples/DocumentationVisuals/Program.cs`. The pinned renderer has a different namespace and scene contract from the current GUI repository's default-branch renderer, so generator claims must be checked against the exact revision in `.github/workflows/package.yml`.

The scene is built from SVG primitives and labels. A controlled post-processing step adds CSS motion to the seven composition-flow connectors. The stylesheet is inserted after GUI.SVG serialization because this pinned renderer does not expose an SVG stylesheet node in its scene mapping.

## What the motion means

The animated connectors guide the reader through the documented sequence:

1. Resolve requested root IDs and versions.
2. Validate bundle identity and dependency contracts.
3. Validate optional published schedule metadata.
4. Load prerequisites before dependents.
5. Arbitrate within the configured bound.
6. Return `RuntimeAssembly` only after convergence.
7. Hand the assembly to the host, which owns execution and manifestation.

This is **explanatory motion**, not runtime telemetry. It is not connected to `FsmCos.Execute`, does not report a real composition's current state, and must not be described as a live trace.

The generated SVG includes a `prefers-reduced-motion: reduce` rule. When reduced motion is requested, connector animation is disabled and the connectors return to solid lines. That is source-level support; hosted rendering and actual browser behavior still require separate verification.

## Regeneration and verification

The package workflow:

- checks out the pinned GUI.SVG source;
- runs `samples/DocumentationVisuals/DocumentationVisuals.csproj`;
- compares the generated file with the committed SVG;
- uploads the visual as a workflow artifact;
- runs the ordinary build/test, public-package-compatibility, source-integration, and package-verification gates.

Do not update the SVG by hand without updating the generator in the same change. The reproducibility check is the guard against the committed picture drifting away from its declared source.

## Boundaries and next steps

- **Current:** source-derived static structure with subtle explanatory connector motion and reduced-motion fallback.
- **Next:** verify the hosted SVG at normal and narrow widths, light/dark contexts, browser accessibility tree, keyboard behavior where relevant, and screen-reader exposure.
- **Later:** a host may expose a narrow, validated, read-only composition snapshot so a living picture can show actual results. That would be a separate integration; FSM_COS remains the composition kernel, the host owns execution and permissions, and GUI.SVG remains a manifestation adapter.
- **Not claimed:** interactive UML editing, automatic runtime binding, browser-native FSM_COS execution, or universal support across every GUI.SVG revision.

The target is a picture that can explain what the system actually did—but only after its displayed state is genuinely connected to that system.

---

## 🔗 Resources & Support

- **Project:** [FSM_COS](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS)
- **Workshop projects:** [FSM_API](https://github.com/TrentBest/FSM_API) · [FSM_Serialization](https://github.com/TrentBest/TheSingularityWorkshop.FSM_Serialization) · [WebPage](https://github.com/TrentBest/WebPage)
- **Connect:** [GitHub](https://github.com/TrentBest) · [Coder Legion profile](https://coderlegion.com/user/The%2BSingularity%2BWorkshop) · [Coder Legion community](https://coderlegion.com/)
- **Support:** [Patreon](https://www.patreon.com/c/TheSingularityWorkshop) · [PayPal](https://www.paypal.com/donate/?hosted_button_id=3Z7263LCQMV9J)

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar and brand image" width="200" /></a></p>
<p align="center"><em>The Singularity Workshop — Tools for the curious, the bold, and the systemically inclined.</em><br /><strong>Because state shouldn't be a mess.</strong><br /><em>And because static boundaries are invitations to cause trouble.</em></p>
