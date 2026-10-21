# Preserved living-SVG branch snapshots

Verbatim snapshots from `work/fsm-cos-living-svg-flow`, 2026-10-21. Historical content retained for comparison and recovery; generated visuals and generator source must be checked for reproducibility before adoption.


---

## DOCUMENTATION_INDEX.md
Source blob: `972a2d061a98fa9dfd9041c3005d82baa37072b4`

```text
# FSM_COS Documentation Index

This index is the navigation map. It does not duplicate the contracts below.

## Start here

| Reader goal | Read |
|---|---|
| Understand the package quickly | [README](README.md) |
| Learn the idea without assuming programming experience | [What Is FSM_COS?](docs/WHAT_IS_FSM_COS.md) |
| Run a concrete consumer | [Minimal consumer sample](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/samples/FSM_COS.MinimalConsumer/Program.cs) |
| Understand ownership and runtime flow | [Architecture](docs/ARCHITECTURE.md) |
| Understand the rationale and design invariants | [Theory](docs/THEORY.md) |
| Integrate the package | [Consuming MicroBundles](docs/CONSUMING_MICROBUNDLES.md) |
| Review the candidate's scope and limitations | [Alpha.7 release notes](docs/releases/0.1.0-alpha.7.md) |
| Verify release readiness | [Alpha.7 readiness gate](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/docs/0.1.0-alpha.7-READINESS.md) |
| Review trust assumptions and adversarial security coverage | [Security](SECURITY.md) |
| Build, test, and understand CI modes | [Development](docs/DEVELOPMENT.md) |

## Contract authority map

Use one primary document for each question. Related pages should explain context and link here rather than restating the same contract.

| Concern | Primary authority | Scope |
|---|---|---|
| What FSM_COS owns | [Domain](docs/DOMAIN.md) | Domain boundary, not API reference |
| How the implementation works | [Architecture](docs/ARCHITECTURE.md) | Current code flow and failure behavior |
| Why the composition boundary exists | [Theory](docs/THEORY.md) | Rationale and invariants |
| Request shape and root version selection | [Runtime Manifest](docs/RUNTIME_MANIFEST.md) | Public manifest contract |
| Staged metadata and its limits | [Staged Manifest Loading](docs/STAGED_MANIFEST_LOADING.md) | Planning/state metadata versus end-to-end localization |
| Host handoff | [RuntimeAssembly](docs/RUNTIME_ASSEMBLY.md) | Output contract and explicit non-promises |
| Runtime ownership limits | [Runtime Boundary](docs/RUNTIME_BOUNDARY.md) | What belongs to hosts and adjacent packages |
| Resolution, loading, and consuming bundles | [Consuming MicroBundles](docs/CONSUMING_MICROBUNDLES.md) | Consumer-facing use and errors |
| Arbitration and convergence | [Arbitration](docs/ARBITRATION.md) | Negotiation semantics and limits |
| AI-related composition concepts | [AI Composition](docs/AI_COMPOSITION.md) | Architectural exploration; not proof of provider integration |
| WebPage host integration | [WebPage Integration](docs/WEBPAGE_INTEGRATION.md) | Host-specific integration boundary |
| Release-specific behavior | [Alpha.7 release notes](docs/releases/0.1.0-alpha.7.md) | Version scope and deferrals |
| Release gate | [Alpha.7 readiness](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/docs/0.1.0-alpha.7-READINESS.md) | Evidence and release criteria |
| Security boundaries and adversarial test plan | [Security](SECURITY.md) | Threat model, mitigations, external baselines, and residual risks |

## Documentation layers

- **Public orientation:** `README.md` and `NuGet.README.md`. The root README is the repository entry point; the NuGet README is the package-consumer entry point.
- **Current contract and explanation:** the primary authority documents listed above.
- **Repository process:** [Development](docs/DEVELOPMENT.md), [Documentation Standard](DOCUMENTATION_STANDARD.md), and [AI agent instructions](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/AGENTS.md).
- **Working and historical records:** branch reconciliation, dependency alignment, ecosystem integration, continuation, and pause-point documents are planning records. They are not API specifications or release evidence; verify any current-state statement against source, tests, and current CI before acting on it. These records are intentionally excluded from the consumer package.

## Accuracy and release evidence

The shared [Documentation Standard](DOCUMENTATION_STANDARD.md) governs writing quality, source accuracy, and link hygiene. The [alpha.7 readiness gate](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/docs/0.1.0-alpha.7-READINESS.md) governs exact-candidate release evidence. Historical planning notes are context, not current specifications; verify present-tense claims against source, tests, and current CI.

## Living documentation

The diagrams are explanatory assets, not independent specifications. Each diagram must agree with its adjacent contract and current implementation. The [Living Documentation Visual Audit](docs/LIVING_DOCUMENTATION_AUDIT.md) tracks visual coverage and accessibility-source checks; [Living SVG](docs/LIVING_SVG.md) explains the generated preflight picture, reduced-motion behavior, and the boundary between explanatory motion and a future host-bound live view. Neither document claims browser or screen-reader verification unless that evidence is explicitly recorded.

---

<p align="center"><em>The Singularity Workshop — Tools for the curious, the bold, and the systemically inclined.</em><br><strong>Because state shouldn't be a mess.</strong></p>

```


---

## NuGet.README.md
Source blob: `4cb9a0877f843bd556f1f6693b5ccdf85d82a6b4`

```text
![The Singularity Workshop animated moniker](https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Renderer/development/docs/assets/moniker.svg)

# ✳️ 00 The Singularity Workshop — FSM_COS

![FSM_COS composition flow: a runtime manifest enters FSM_COS, which resolves and composes MicroBundles into a RuntimeAssembly for a host.](https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.FSM_COS/development/docs/assets/fsm-cos-overview.svg)

**FSM_COS composes requested capabilities into a `RuntimeAssembly`. It is a composition boundary—not an application, GUI, engine, repository, or execution loop.**

*If a picture is worth a thousand words, then what is a living picture worth?* FSM_COS uses source-backed diagrams to explain dependency closure, preflight validation, failure boundaries, and the host handoff. They illustrate the real contract; they are not presented as live execution traces.

## 🟦 01 — What is FSM_COS?

A `RuntimeManifest` identifies the MicroBundles and versions a caller wants. FSM_COS resolves their dependencies, loads the composition, performs bounded arbitration, and returns a `RuntimeAssembly` for the host to use.

## 🟣 02 — Why does it exist?

Hosts should not need to duplicate the same composition and dependency logic for every experience. FSM_COS gives that responsibility a focused home while leaving application behavior and presentation to the host.

Compatibility is not automatic: the selected bundles, their contracts, and the host must work together.

## 🩵 03 — How does it work?

```text
RuntimeManifest → FSM_COS → RuntimeAssembly → Host
                    │
                    ├─ resolve MicroBundles and dependencies
                    ├─ load the composition
                    └─ arbitrate toward convergence
```

![FSM_COS validation and composition path, generated with GUI.SVG](https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.FSM_COS/f71d8c00a3cb502048eba731163846f4501c1848/docs/assets/fsm-cos-preflight.svg)

*This is a source-backed contract map, not a live execution trace.*

- **MicroBundleDomain** owns the MicroBundle contract.
- **The host or repository** supplies the catalog used to resolve bundles.
- **FSM_COS** composes the requested capabilities.
- **The host** owns execution, lifecycle, UI, and presentation.

Alpha.7 exposes read-only snapshots for assembly and plan collections: callers cannot mutate the collection structure through those public views. This does **not** make the `MicroBundle` objects inside those collections immutable; their behavior remains owned by their domain contracts.

## 🟢 04 — See it in a minute

To install FSM_COS, choose an available version from the [NuGet package page](https://www.nuget.org/packages/TheSingularityWorkshop.FSM_COS). If you intend to use a prerelease, request prerelease versions explicitly:

```bash
dotnet add package TheSingularityWorkshop.FSM_COS --prerelease
```

Installation adds a composition library; it does not create an application. FSM_COS needs a catalog supplied by the host. To see the composition behavior running, clone the source repository and run its minimal consumer sample:

```bash
git clone https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS.git
cd TheSingularityWorkshop.FSM_COS
dotnet run --project samples/FSM_COS.MinimalConsumer/FSM_COS.MinimalConsumer.csproj
```

The sample creates a report bundle that depends on a data-source bundle. The expected result is `Assembly order: 2 -> 1`, proving the dependency is loaded before the requested bundle. It defines its own catalog and example bundles; these are not built-in FSM_COS types. The sample references the local source project so it can be verified before a candidate version is published.

For a complete consumer guide, see [Consuming MicroBundles](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/docs/CONSUMING_MICROBUNDLES.md). The package targets **.NET 8** and its direct dependencies are:

- [FSM_API](https://www.nuget.org/packages/TheSingularityWorkshop.FSM_API) — state/context primitives.
- [MicroBundleDomain](https://www.nuget.org/packages/TheSingularityWorkshop.MicroBundleDomain) — the MicroBundle contract.
- [FSM_UserIO](https://www.nuget.org/packages/TheSingularityWorkshop.FSM_UserIO) — the platform-neutral `SemanticIntent` boundary.

This source currently declares version `0.1.0-alpha.7`. A source version declaration alone does not establish publication status; check the [NuGet package page](https://www.nuget.org/packages/TheSingularityWorkshop.FSM_COS) for the version actually available.

## 🟪 05 — Available documentation and theory

- [Project overview](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/README.md) — the bigger picture and quick proof.
- [What Is FSM_COS?](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/docs/WHAT_IS_FSM_COS.md) — a plain-language introduction for readers new to software composition.
- [Theory](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/docs/THEORY.md) — why composition is a distinct operation.
- [Architecture](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/docs/ARCHITECTURE.md) — ownership and runtime flow.
- [Domain completion roadmap](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/docs/DOMAIN_COMPLETION_ROADMAP.md) — implemented capabilities, deferred work, dependency relationships, and follow-on engineering.
- [Runtime Manifest](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/docs/RUNTIME_MANIFEST.md) — the composition request.
- [RuntimeAssembly](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/docs/RUNTIME_ASSEMBLY.md) — the host handoff.
- [Arbitration](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/docs/ARBITRATION.md) — reconciliation and convergence.
- [Documentation Index](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/DOCUMENTATION_INDEX.md) — all focused guides.
- [Alpha.7 release notes](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/docs/releases/0.1.0-alpha.7.md) — included contracts and explicit limitations.
- [Development](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/docs/DEVELOPMENT.md) — build and verification.
- [Source and issues](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS)

---
<p align="center">
  <a href="https://github.com/TrentBest/WebPage" aria-label="Visit The Singularity Workshop">
    <img src="https://raw.githubusercontent.com/TrentBest/WebPage/development/TheSingularityWorkshop/wwwroot/Images/TheSingularityWorkshopLivingAvatar.svg" alt="The Singularity Workshop living avatar — a digital vortex of information converging on its central eye" width="112" />
  </a><br />
  <strong>The Singularity Workshop</strong><br />
  <em>If a picture is worth a thousand words, what is a living picture worth?</em><br />
  <a href="https://github.com/TrentBest">GitHub</a> ·
  <a href="https://coderlegion.com/">Coder Legion</a> ·
  <a href="https://www.patreon.com/c/TheSingularityWorkshop">Patreon</a> ·
  <a href="https://www.paypal.com/donate/?hosted_button_id=3Z7263LCQMV9J">PayPal</a><br />
  <a href="https://github.com/TrentBest/FSM_API">FSM_API</a> ·
  <a href="https://github.com/TrentBest/TheSingularityWorkshop.FSM_Serialization">FSM_Serialization</a> ·
  <a href="https://github.com/TrentBest/WebPage">WebPage</a><br />
  <strong>Because state shouldn't be a mess.</strong><br />
  <em>And because static boundaries are invitations to cause trouble.</em>
</p>

```


---

## docs/LIVING_SVG.md
Source blob: `afee6f5f6e9d34fb492323c505ddb4aed0ca70cf`

```text
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

```


---

## docs/assets/fsm-cos-preflight.svg
Source blob: `026102f6685184cafc7aa1982a9f9012ebcefe21`

```text
<svg width="1600" height="900" viewBox="0 0 100 56.25" preserveAspectRatio="xMidYMid meet" role="img" aria-label="FSM_COS validation and composition path" aria-labelledby="gui-svg-title gui-svg-description" xmlns="http://www.w3.org/2000/svg"><title id="gui-svg-title">FSM_COS validation and composition path</title><desc id="gui-svg-description">Generated with GUI.SVG from a controlled scene. Shows full dependency-closure preflight, identity and dependency contract validation, optional schedule validation, iterative dependency-first loading, bounded arbitration, host handoff, and distinct failure boundaries. Connector motion is explanatory and honors reduced-motion preferences. It is a contract map, not a live execution trace.</desc><rect x="0" y="0" width="100" height="56.25" fill="#0b1725" id="background" /><defs id="definitions"><marker refX="5" refY="5" markerWidth="3" markerHeight="3" orient="auto" id="arrowhead"><polygon points="0,0 10,5 0,10" fill="#7ee0bd" id="arrowhead-shape" /></marker></defs><text x="3" y="3.2" fill="#eef6ff" id="heading" font-family="sans-serif" font-size="2.1" font-weight="700">FSM_COS  |  VALIDATION / COMPOSITION / HANDOFF</text><rect x="3" y="6" width="18" height="8" fill="#173b4c" rx="1" id="manifest-box" stroke="#7ee0bd" stroke-width="0.3" /><text x="12" y="10.5" fill="#eef6ff" id="manifest-label" font-family="sans-serif" font-size="1.45" text-anchor="middle">RuntimeManifest</text><rect x="25" y="6" width="20" height="8" fill="#173b4c" rx="1" id="roots-box" stroke="#7ee0bd" stroke-width="0.3" /><text x="35" y="9.4" fill="#eef6ff" id="roots-label" font-family="sans-serif" font-size="1.5" text-anchor="middle">Resolve root IDs</text><text x="35" y="11.5" fill="#d6e7f4" id="roots-sub-label" font-family="sans-serif" font-size="1.3" text-anchor="middle">and exact versions</text><rect x="49" y="6" width="22" height="8" fill="#302b58" rx="1" id="identity-box" stroke="#b9a7ff" stroke-width="0.3" /><text x="60" y="9.4" fill="#eef6ff" id="identity-label" font-family="sans-serif" font-size="1.4" text-anchor="middle">Validate bundle identity</text><text x="60" y="11.5" fill="#d6e7f4" id="identity-sub-label" font-family="sans-serif" font-size="1.05" text-anchor="middle">descriptor + dependency requests</text><rect x="75" y="6" width="22" height="8" fill="#302b58" rx="1" id="schedule-box" stroke="#b9a7ff" stroke-width="0.3" /><text x="86" y="9.4" fill="#eef6ff" id="schedule-label" font-family="sans-serif" font-size="1.35" text-anchor="middle">Optional schedule check</text><text x="86" y="11.5" fill="#d6e7f4" id="schedule-sub-label" font-family="sans-serif" font-size="1.25" text-anchor="middle">closure + edges</text><line x1="21" y1="10" x2="24" y2="10" stroke="#7ee0bd" stroke-width="0.45" id="manifest-to-roots" class="flow-line" marker-end="url(#arrowhead)" /><line x1="45" y1="10" x2="48" y2="10" stroke="#7ee0bd" stroke-width="0.45" id="roots-to-identity" class="flow-line" marker-end="url(#arrowhead)" /><line x1="71" y1="10" x2="74" y2="10" stroke="#7ee0bd" stroke-width="0.45" id="identity-to-schedule" class="flow-line" marker-end="url(#arrowhead)" /><rect x="75" y="21" width="22" height="8" fill="#173b4c" rx="1" id="load-box" stroke="#7ee0bd" stroke-width="0.3" /><text x="86" y="24.4" fill="#eef6ff" id="load-label" font-family="sans-serif" font-size="1.35" text-anchor="middle">Load dependencies first</text><text x="86" y="26.5" fill="#d6e7f4" id="load-sub-label" font-family="sans-serif" font-size="1.25" text-anchor="middle">then dependents</text><line x1="86" y1="14" x2="86" y2="20" stroke="#7ee0bd" stroke-width="0.45" id="schedule-to-load" class="flow-line" marker-end="url(#arrowhead)" /><rect x="49" y="21" width="22" height="8" fill="#173b4c" rx="1" id="arbitration-box" stroke="#7ee0bd" stroke-width="0.3" /><text x="60" y="24.4" fill="#eef6ff" id="arbitration-label" font-family="sans-serif" font-size="1.4" text-anchor="middle">Bounded arbitration</text><text x="60" y="26.5" fill="#d6e7f4" id="arbitration-sub-label" font-family="sans-serif" font-size="1.15" text-anchor="middle">stop when stable or fail</text><line x1="75" y1="25" x2="72" y2="25" stroke="#7ee0bd" stroke-width="0.45" id="load-to-arbitration" class="flow-line" marker-end="url(#arrowhead)" /><rect x="25" y="21" width="20" height="8" fill="#173b4c" rx="1" id="assembly-box" stroke="#7ee0bd" stroke-width="0.3" /><text x="35" y="24.4" fill="#eef6ff" id="assembly-label" font-family="sans-serif" font-size="1.4" text-anchor="middle">RuntimeAssembly</text><text x="35" y="26.5" fill="#d6e7f4" id="assembly-sub-label" font-family="sans-serif" font-size="1.25" text-anchor="middle">only on success</text><line x1="49" y1="25" x2="46" y2="25" stroke="#7ee0bd" stroke-width="0.45" id="arbitration-to-assembly" class="flow-line" marker-end="url(#arrowhead)" /><rect x="3" y="21" width="18" height="8" fill="#173b4c" rx="1" id="host-box" stroke="#7ee0bd" stroke-width="0.3" /><text x="12" y="24.4" fill="#eef6ff" id="host-label" font-family="sans-serif" font-size="1.25" text-anchor="middle">Host owns execution</text><text x="12" y="26.5" fill="#d6e7f4" id="host-sub-label" font-family="sans-serif" font-size="1.15" text-anchor="middle">FSM_COS hands off</text><line x1="25" y1="25" x2="22" y2="25" stroke="#7ee0bd" stroke-width="0.45" id="assembly-to-host" class="flow-line" marker-end="url(#arrowhead)" /><rect x="3" y="36" width="44" height="10" fill="#3a202c" rx="1" id="preflight-failure-box" stroke="#ff8c9d" stroke-width="0.3" /><text x="25" y="40" fill="#fff1f3" id="preflight-failure-label" font-family="sans-serif" font-size="1.5" font-weight="700" text-anchor="middle">Preflight rejection</text><text x="25" y="42.5" fill="#ffd5dc" id="preflight-failure-sub-label" font-family="sans-serif" font-size="1.15" text-anchor="middle">Bad identity, missing dependency, cycle, or schedule mismatch: reject before Load()</text><rect x="53" y="36" width="44" height="10" fill="#3a202c" rx="1" id="runtime-failure-box" stroke="#ff8c9d" stroke-width="0.3" /><text x="75" y="40" fill="#fff1f3" id="runtime-failure-label" font-family="sans-serif" font-size="1.5" font-weight="700" text-anchor="middle">Runtime failure</text><text x="75" y="42.5" fill="#ffd5dc" id="runtime-failure-sub-label" font-family="sans-serif" font-size="1.1" text-anchor="middle">Load / arbitration / non-convergence: no assembly</text><text x="3" y="51.5" fill="#d6e7f4" id="stage-note" font-family="sans-serif" font-size="1.25">LoadPlan stages are metadata; they do not automatically defer loading. Load failures do not roll back prior side effects.</text><style>@keyframes fsm-cos-flow { to { stroke-dashoffset: -2.2; } }
.flow-line { stroke-dasharray: 0.75 0.45; animation: fsm-cos-flow 2.8s linear infinite; }
@media (prefers-reduced-motion: reduce) {
  .flow-line { animation: none !important; stroke-dasharray: none !important; }
}</style></svg>

```


---

## samples/DocumentationVisuals/Program.cs
Source blob: `ab74c7ea6c3805d4ffe4b61d61c4c53dde3b0063`

```text
using TheSingularityWorkshop.Workshop.Gui;
using TheSingularityWorkshop.Workshop.Gui.Svg;
using System.Xml.Linq;

var output = args.Length > 0
    ? Path.GetFullPath(args[0])
    : Path.GetFullPath("docs/assets/fsm-cos-preflight.svg");

var scene = GuiBuilder.Create("svg-scene", "fsm-cos-preflight")
    .Text("FSM_COS validation and composition path")
    .Property("description",
        "Generated with GUI.SVG from a controlled scene. Shows full dependency-closure preflight, identity and dependency contract validation, optional schedule validation, iterative dependency-first loading, bounded arbitration, host handoff, and distinct failure boundaries. Connector motion is explanatory and honors reduced-motion preferences. It is a contract map, not a live execution trace.");

scene.Child("svg-rect", "background", node => node
    .Property("width", "100").Property("height", "56.25")
    .Property("fill", "#0b1725"));

scene.Child("svg-defs", "definitions", defs => defs
    .Child("svg-marker", "arrowhead", marker => marker
        .Property("refX", "5").Property("refY", "5")
        .Property("markerWidth", "3").Property("markerHeight", "3")
        .Property("orient", "auto")
        .Child("svg-polygon", "arrowhead-shape", polygon => polygon
            .Property("points", "0,0 10,5 0,10").Property("fill", "#7ee0bd"))));

scene.Child("svg-text", "heading", text => text
    .Text("FSM_COS  |  VALIDATION / COMPOSITION / HANDOFF")
    .Property("x", "3").Property("y", "3.2")
    .Property("font-family", "sans-serif").Property("font-size", "2.1")
    .Property("font-weight", "700").Property("fill", "#eef6ff"));

scene.Child("svg-rect", "manifest-box", node => node
    .Property("x", "3").Property("y", "6").Property("width", "18").Property("height", "8")
    .Property("rx", "1").Property("fill", "#173b4c").Property("stroke", "#7ee0bd").Property("stroke-width", "0.3"));
scene.Child("svg-text", "manifest-label", text => text
    .Text("RuntimeManifest").Property("x", "12").Property("y", "10.5")
    .Property("text-anchor", "middle").Property("font-family", "sans-serif").Property("font-size", "1.45").Property("fill", "#eef6ff"));

scene.Child("svg-rect", "roots-box", node => node
    .Property("x", "25").Property("y", "6").Property("width", "20").Property("height", "8")
    .Property("rx", "1").Property("fill", "#173b4c").Property("stroke", "#7ee0bd").Property("stroke-width", "0.3"));
scene.Child("svg-text", "roots-label", text => text
    .Text("Resolve root IDs").Property("x", "35").Property("y", "9.4")
    .Property("text-anchor", "middle").Property("font-family", "sans-serif").Property("font-size", "1.5").Property("fill", "#eef6ff"));
scene.Child("svg-text", "roots-sub-label", text => text
    .Text("and exact versions").Property("x", "35").Property("y", "11.5")
    .Property("text-anchor", "middle").Property("font-family", "sans-serif").Property("font-size", "1.3").Property("fill", "#d6e7f4"));

scene.Child("svg-rect", "identity-box", node => node
    .Property("x", "49").Property("y", "6").Property("width", "22").Property("height", "8")
    .Property("rx", "1").Property("fill", "#302b58").Property("stroke", "#b9a7ff").Property("stroke-width", "0.3"));
scene.Child("svg-text", "identity-label", text => text
    .Text("Validate bundle identity").Property("x", "60").Property("y", "9.4")
    .Property("text-anchor", "middle").Property("font-family", "sans-serif").Property("font-size", "1.4").Property("fill", "#eef6ff"));
scene.Child("svg-text", "identity-sub-label", text => text
    .Text("descriptor + dependency requests").Property("x", "60").Property("y", "11.5")
    .Property("text-anchor", "middle").Property("font-family", "sans-serif").Property("font-size", "1.05").Property("fill", "#d6e7f4"));

scene.Child("svg-rect", "schedule-box", node => node
    .Property("x", "75").Property("y", "6").Property("width", "22").Property("height", "8")
    .Property("rx", "1").Property("fill", "#302b58").Property("stroke", "#b9a7ff").Property("stroke-width", "0.3"));
scene.Child("svg-text", "schedule-label", text => text
    .Text("Optional schedule check").Property("x", "86").Property("y", "9.4")
    .Property("text-anchor", "middle").Property("font-family", "sans-serif").Property("font-size", "1.35").Property("fill", "#eef6ff"));
scene.Child("svg-text", "schedule-sub-label", text => text
    .Text("closure + edges").Property("x", "86").Property("y", "11.5")
    .Property("text-anchor", "middle").Property("font-family", "sans-serif").Property("font-size", "1.25").Property("fill", "#d6e7f4"));

scene.Child("svg-line", "manifest-to-roots", line => line
    .Property("x1", "21").Property("y1", "10").Property("x2", "24").Property("y2", "10")
    .Property("stroke", "#7ee0bd").Property("stroke-width", "0.45").Property("class", "flow-line").Property("marker-end", "url(#arrowhead)"));
scene.Child("svg-line", "roots-to-identity", line => line
    .Property("x1", "45").Property("y1", "10").Property("x2", "48").Property("y2", "10")
    .Property("stroke", "#7ee0bd").Property("stroke-width", "0.45").Property("class", "flow-line").Property("marker-end", "url(#arrowhead)"));
scene.Child("svg-line", "identity-to-schedule", line => line
    .Property("x1", "71").Property("y1", "10").Property("x2", "74").Property("y2", "10")
    .Property("stroke", "#7ee0bd").Property("stroke-width", "0.45").Property("class", "flow-line").Property("marker-end", "url(#arrowhead)"));

scene.Child("svg-rect", "load-box", node => node
    .Property("x", "75").Property("y", "21").Property("width", "22").Property("height", "8")
    .Property("rx", "1").Property("fill", "#173b4c").Property("stroke", "#7ee0bd").Property("stroke-width", "0.3"));
scene.Child("svg-text", "load-label", text => text
    .Text("Load dependencies first").Property("x", "86").Property("y", "24.4")
    .Property("text-anchor", "middle").Property("font-family", "sans-serif").Property("font-size", "1.35").Property("fill", "#eef6ff"));
scene.Child("svg-text", "load-sub-label", text => text
    .Text("then dependents").Property("x", "86").Property("y", "26.5")
    .Property("text-anchor", "middle").Property("font-family", "sans-serif").Property("font-size", "1.25").Property("fill", "#d6e7f4"));

scene.Child("svg-line", "schedule-to-load", line => line
    .Property("x1", "86").Property("y1", "14").Property("x2", "86").Property("y2", "20")
    .Property("stroke", "#7ee0bd").Property("stroke-width", "0.45").Property("class", "flow-line").Property("marker-end", "url(#arrowhead)"));

scene.Child("svg-rect", "arbitration-box", node => node
    .Property("x", "49").Property("y", "21").Property("width", "22").Property("height", "8")
    .Property("rx", "1").Property("fill", "#173b4c").Property("stroke", "#7ee0bd").Property("stroke-width", "0.3"));
scene.Child("svg-text", "arbitration-label", text => text
    .Text("Bounded arbitration").Property("x", "60").Property("y", "24.4")
    .Property("text-anchor", "middle").Property("font-family", "sans-serif").Property("font-size", "1.4").Property("fill", "#eef6ff"));
scene.Child("svg-text", "arbitration-sub-label", text => text
    .Text("stop when stable or fail").Property("x", "60").Property("y", "26.5")
    .Property("text-anchor", "middle").Property("font-family", "sans-serif").Property("font-size", "1.15").Property("fill", "#d6e7f4"));
scene.Child("svg-line", "load-to-arbitration", line => line
    .Property("x1", "75").Property("y1", "25").Property("x2", "72").Property("y2", "25")
    .Property("stroke", "#7ee0bd").Property("stroke-width", "0.45").Property("class", "flow-line").Property("marker-end", "url(#arrowhead)"));

scene.Child("svg-rect", "assembly-box", node => node
    .Property("x", "25").Property("y", "21").Property("width", "20").Property("height", "8")
    .Property("rx", "1").Property("fill", "#173b4c").Property("stroke", "#7ee0bd").Property("stroke-width", "0.3"));
scene.Child("svg-text", "assembly-label", text => text
    .Text("RuntimeAssembly").Property("x", "35").Property("y", "24.4")
    .Property("text-anchor", "middle").Property("font-family", "sans-serif").Property("font-size", "1.4").Property("fill", "#eef6ff"));
scene.Child("svg-text", "assembly-sub-label", text => text
    .Text("only on success").Property("x", "35").Property("y", "26.5")
    .Property("text-anchor", "middle").Property("font-family", "sans-serif").Property("font-size", "1.25").Property("fill", "#d6e7f4"));
scene.Child("svg-line", "arbitration-to-assembly", line => line
    .Property("x1", "49").Property("y1", "25").Property("x2", "46").Property("y2", "25")
    .Property("stroke", "#7ee0bd").Property("stroke-width", "0.45").Property("class", "flow-line").Property("marker-end", "url(#arrowhead)"));

scene.Child("svg-rect", "host-box", node => node
    .Property("x", "3").Property("y", "21").Property("width", "18").Property("height", "8")
    .Property("rx", "1").Property("fill", "#173b4c").Property("stroke", "#7ee0bd").Property("stroke-width", "0.3"));
scene.Child("svg-text", "host-label", text => text
    .Text("Host owns execution").Property("x", "12").Property("y", "24.4")
    .Property("text-anchor", "middle").Property("font-family", "sans-serif").Property("font-size", "1.25").Property("fill", "#eef6ff"));
scene.Child("svg-text", "host-sub-label", text => text
    .Text("FSM_COS hands off").Property("x", "12").Property("y", "26.5")
    .Property("text-anchor", "middle").Property("font-family", "sans-serif").Property("font-size", "1.15").Property("fill", "#d6e7f4"));
scene.Child("svg-line", "assembly-to-host", line => line
    .Property("x1", "25").Property("y1", "25").Property("x2", "22").Property("y2", "25")
    .Property("stroke", "#7ee0bd").Property("stroke-width", "0.45").Property("class", "flow-line").Property("marker-end", "url(#arrowhead)"));

scene.Child("svg-rect", "preflight-failure-box", node => node
    .Property("x", "3").Property("y", "36").Property("width", "44").Property("height", "10")
    .Property("rx", "1").Property("fill", "#3a202c").Property("stroke", "#ff8c9d").Property("stroke-width", "0.3"));
scene.Child("svg-text", "preflight-failure-label", text => text
    .Text("Preflight rejection").Property("x", "25").Property("y", "40")
    .Property("text-anchor", "middle").Property("font-family", "sans-serif").Property("font-size", "1.5").Property("font-weight", "700").Property("fill", "#fff1f3"));
scene.Child("svg-text", "preflight-failure-sub-label", text => text
    .Text("Bad identity, missing dependency, cycle, or schedule mismatch: reject before Load()").Property("x", "25").Property("y", "42.5")
    .Property("text-anchor", "middle").Property("font-family", "sans-serif").Property("font-size", "1.15").Property("fill", "#ffd5dc"));

scene.Child("svg-rect", "runtime-failure-box", node => node
    .Property("x", "53").Property("y", "36").Property("width", "44").Property("height", "10")
    .Property("rx", "1").Property("fill", "#3a202c").Property("stroke", "#ff8c9d").Property("stroke-width", "0.3"));
scene.Child("svg-text", "runtime-failure-label", text => text
    .Text("Runtime failure").Property("x", "75").Property("y", "40")
    .Property("text-anchor", "middle").Property("font-family", "sans-serif").Property("font-size", "1.5").Property("font-weight", "700").Property("fill", "#fff1f3"));
scene.Child("svg-text", "runtime-failure-sub-label", text => text
    .Text("Load / arbitration / non-convergence: no assembly").Property("x", "75").Property("y", "42.5")
    .Property("text-anchor", "middle").Property("font-family", "sans-serif").Property("font-size", "1.1").Property("fill", "#ffd5dc"));

scene.Child("svg-text", "stage-note", text => text
    .Text("LoadPlan stages are metadata; they do not automatically defer loading. Load failures do not roll back prior side effects.")
    .Property("x", "3").Property("y", "51.5")
    .Property("font-family", "sans-serif").Property("font-size", "1.25").Property("fill", "#d6e7f4"));

var svg = new SvgGuiRenderer().Render(scene.Build(), width: 1600, height: 900);

// The pinned GUI.SVG renderer emits the SVG scene; this small, controlled post-pass
// adds CSS motion plus a reduced-motion fallback without changing the scene contract.
XNamespace svgNamespace = "http://www.w3.org/2000/svg";
var document = XDocument.Parse(svg);
document.Root!.Add(new XElement(svgNamespace + "style", """
@keyframes fsm-cos-flow { to { stroke-dashoffset: -2.2; } }
.flow-line { stroke-dasharray: 0.75 0.45; animation: fsm-cos-flow 2.8s linear infinite; }
@media (prefers-reduced-motion: reduce) {
  .flow-line { animation: none !important; stroke-dasharray: none !important; }
}
"""));
var livingSvg = document.ToString(SaveOptions.DisableFormatting);
Directory.CreateDirectory(Path.GetDirectoryName(output)!);
File.WriteAllText(output, livingSvg + Environment.NewLine);
Console.WriteLine($"Generated FSM_COS living preflight visual with GUI.SVG: {output}");

```
