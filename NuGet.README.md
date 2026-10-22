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

![FSM_COS validation and composition path, generated with GUI.SVG](https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.FSM_COS/6d0efa34d08134c1f75cfd12955072cec465c86e/docs/assets/fsm-cos-preflight.svg)

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

The package targets **.NET 8** and is versioned `0.1.0-alpha.7`.

## 🟪 05 — Documentation

- [Consumer guide](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/docs/CONSUMING_MICROBUNDLES.md) — catalog, manifest, configuration, and MicroBundle contracts.
- [Architecture](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/docs/ARCHITECTURE.md) — composition flow and responsibility boundaries.
- [Runtime Manifest](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/docs/RUNTIME_MANIFEST.md) — the composition request.
- [RuntimeAssembly](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/docs/RUNTIME_ASSEMBLY.md) — the host handoff.
- [Arbitration](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/docs/ARBITRATION.md) — bounded reconciliation.
- [Gallery](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/docs/GALLERY.md) — visual walkthroughs.
- [Source repository](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS) — runnable sample and development instructions.
---
<p align="center">
  <a href="https://github.com/TrentBest/WebPage" aria-label="Visit The Singularity Workshop">
    <img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop living avatar — a digital vortex of information converging on its central eye" width="112" />
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

---

## 🔗 Resources & Support

- **Project:** [FSM_COS](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS)
- **Workshop projects:** [FSM_API](https://github.com/TrentBest/FSM_API) · [FSM_Serialization](https://github.com/TrentBest/TheSingularityWorkshop.FSM_Serialization) · [WebPage](https://github.com/TrentBest/WebPage)
- **Connect:** [GitHub](https://github.com/TrentBest) · [Coder Legion profile](https://coderlegion.com/user/The%2BSingularity%2BWorkshop) · [Coder Legion community](https://coderlegion.com/)
- **Support:** [Patreon](https://www.patreon.com/c/TheSingularityWorkshop) · [PayPal](https://www.paypal.com/donate/?hosted_button_id=3Z7263LCQMV9J)

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center">
  <a href="https://github.com/TrentBest" aria-label="The Singularity Workshop on GitHub">
    <img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar and brand image" width="200" />
  </a>
</p>

<p align="center">
  <em>The Singularity Workshop — Tools for the curious, the bold, and the systemically inclined.</em><br />
  <strong>Because state shouldn't be a mess.</strong><br />
  <em>And because static boundaries are invitations to cause trouble.</em>
</p>
