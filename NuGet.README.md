![The Singularity Workshop animated moniker](https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Renderer/development/docs/assets/moniker.svg)

# FSM_COS — Compute Operating System for Runtime Composition

![FSM_COS composition flow: a manifest request enters the kernel, which resolves dependencies and returns a RuntimeAssembly to a host.](https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.FSM_COS/development/docs/assets/fsm-cos-overview.svg)

**FSM_COS turns a declared request for computation into an assembled runtime.** It resolves requested MicroBundles and their dependencies, loads them in dependency order, runs bounded arbitration, and returns a `RuntimeAssembly` to the host.

The **FSM** name reflects the Workshop's [FSM_API](https://github.com/TrentBest/FSM_API) foundation. FSM_COS uses state/context primitives in a composition system; it is not itself the application scheduler or user-facing application.

## Why use it?

Use FSM_COS when you want different hosts or experiences to assemble different combinations of reusable capabilities without duplicating the dependency-composition algorithm in every host.

- Describe the requested roots with a `RuntimeManifest`.
- Supply an `IMicroBundleCatalog` that resolves the MicroBundles your host can access.
- Let FSM_COS discover and validate dependency closure, then load prerequisites before dependents.
- Let loaded bundles participate in bounded arbitration.
- Receive a `RuntimeAssembly` only when composition succeeds, then let your host decide how to execute or present it.

This separates **composition** from **artifact storage, application execution, UI, and presentation**. It does not make incompatible bundles compatible automatically, fetch artifacts from the internet, or turn the result into a running application.

## Try the minimal consumer

The source repository contains a runnable example that requests a report bundle whose prerequisite is a data-source bundle.

```bash
git clone https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS.git
cd TheSingularityWorkshop.FSM_COS
dotnet run --project samples/FSM_COS.MinimalConsumer/FSM_COS.MinimalConsumer.csproj
```

Expected final line:

```text
Assembly order: 2 -> 1
```

The sample uses its own in-memory catalog and example MicroBundles. It demonstrates dependency-first composition; it does not imply those demo bundles ship in the package. The example targets the source project so it can be checked independently of public-feed publication.

## Install

Check the [NuGet package page](https://www.nuget.org/packages/TheSingularityWorkshop.FSM_COS) for versions actually published. If a suitable version is available, install it with:

```bash
dotnet add package TheSingularityWorkshop.FSM_COS --prerelease
```

The repository project currently targets **.NET 8** and declares **0.1.0-alpha.7**; that source version alone does not mean alpha.7 has been published.

## Dependency and responsibility boundaries

- [FSM_API](https://www.nuget.org/packages/TheSingularityWorkshop.FSM_API) — state-machine and state/context foundation.
- [MicroBundleDomain](https://www.nuget.org/packages/TheSingularityWorkshop.MicroBundleDomain) — owns the MicroBundle contract.
- [FSM_UserIO](https://www.nuget.org/packages/TheSingularityWorkshop.FSM_UserIO) — platform-neutral semantic-intent boundary.

The host supplies the catalog and optional configuration source. The host also owns application lifecycle, execution, rendering, networking, storage, and presentation. FSM_COS owns composition.

## Read next

- [Consumer guide](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/docs/CONSUMING_MICROBUNDLES.md) — build the catalog and manifest.
- [Architecture](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/docs/ARCHITECTURE.md) — see the composition algorithm and ownership boundaries.
- [Runtime Manifest](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/docs/RUNTIME_MANIFEST.md) — understand the request.
- [RuntimeAssembly](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/docs/RUNTIME_ASSEMBLY.md) — understand the handoff.
- [Arbitration](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/docs/ARBITRATION.md) — understand bounded reconciliation.
- [Gallery](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/docs/GALLERY.md) — inspect the source-backed visual explanations.

[Source repository](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS) · [NuGet package](https://www.nuget.org/packages/TheSingularityWorkshop.FSM_COS) · [Documentation index](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/DOCUMENTATION_INDEX.md)

---

<p align="center">
  <a href="https://github.com/TrentBest/WebPage" aria-label="Visit The Singularity Workshop">
    <img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="112" />
  </a><br />
  <strong>The Singularity Workshop</strong><br />
  <em>If a picture is worth a thousand words, what is a living picture worth?</em><br />
  <a href="https://github.com/TrentBest">GitHub</a> · <a href="https://coderlegion.com/">Coder Legion</a> · <a href="https://www.patreon.com/c/TheSingularityWorkshop">Patreon</a> · <a href="https://www.paypal.com/donate/?hosted_button_id=3Z7263LCQMV9J">PayPal</a>
</p>
