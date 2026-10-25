![The Singularity Workshop animated moniker](https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Renderer/development/docs/assets/moniker.svg)

# FSM_COS — Compute Operating System for Runtime Composition

![FSM_COS composition flow: a manifest request enters the kernel, which resolves dependencies and returns a RuntimeAssembly to a host.](https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.FSM_COS/development/docs/assets/fsm-cos-overview.svg)

**FSM_COS turns a declared request for computation into an assembled runtime.** It resolves requested MicroBundles and their dependencies, loads them in dependency order, runs bounded arbitration, and returns a `RuntimeAssembly` to the host.

FSM_COS **directly depends on [FSM_API](https://github.com/TrentBest/FSM_API)** for finite-state-machine and state/context primitives. FSM_COS uses that foundation for runtime composition; it is not itself the application scheduler or user-facing application.

## 🟦 01 — What does FSM_COS do?

A host supplies a `RuntimeManifest`, a catalog that can resolve requested MicroBundles, and optionally a configuration source. FSM_COS validates the requested composition, discovers its dependency closure, loads prerequisites before dependents, performs bounded arbitration, and returns a `RuntimeAssembly` only when composition succeeds.

The MicroBundle contract belongs to [MicroBundleDomain](https://www.nuget.org/packages/TheSingularityWorkshop.MicroBundleDomain). FSM_COS consumes that contract and owns the composition algorithm.

## 🟣 02 — Why use it?

Use FSM_COS when different hosts or experiences need different combinations of reusable capabilities without duplicating dependency resolution and composition rules in every host.

- Describe requested roots with a `RuntimeManifest`.
- Reuse a consistent dependency and load-order contract.
- Fail composition rather than hand off a successful result when required composition steps fail.
- Keep composition separate from artifact storage, application execution, UI, and presentation.

FSM_COS does not make incompatible bundles compatible automatically, fetch artifacts from the internet, or turn the result into a running application.

## 🩵 03 — How do I install and use it?

Check the [NuGet package page](https://www.nuget.org/packages/TheSingularityWorkshop.FSM_COS) for versions actually published. If a suitable version is available, install it with:

```bash
dotnet add package TheSingularityWorkshop.FSM_COS --prerelease
```

The repository project currently targets **.NET 8** and declares **0.1.0-alpha.7**; that source version alone does not mean alpha.7 has been published.

The host supplies the catalog and optional configuration source. It also owns application lifecycle, execution, rendering, networking, storage, and presentation. FSM_COS owns composition.

Related packages:
- [FSM_API](https://www.nuget.org/packages/TheSingularityWorkshop.FSM_API) — state-machine and state/context foundation.
- [MicroBundleDomain](https://www.nuget.org/packages/TheSingularityWorkshop.MicroBundleDomain) — MicroBundle contract.
- [FSM_UserIO](https://www.nuget.org/packages/TheSingularityWorkshop.FSM_UserIO) — platform-neutral semantic-intent boundary.

## 🟢 04 — See it in a minute

The source repository contains a runnable example that requests a report bundle whose prerequisite is a data-source bundle. You need Git and the .NET 8 SDK:

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

## 🟪 05 — Choose your next step

- [Consumer guide](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/docs/CONSUMING_MICROBUNDLES.md) — build the catalog and manifest.
- [Architecture](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/docs/ARCHITECTURE.md) — understand the composition algorithm and ownership boundaries.
- [Runtime Manifest](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/docs/RUNTIME_MANIFEST.md) — understand the request.
- [RuntimeAssembly](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/docs/RUNTIME_ASSEMBLY.md) — understand the handoff.
- [Arbitration](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/docs/ARBITRATION.md) — understand bounded reconciliation.
- [Gallery](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/docs/GALLERY.md) — inspect the visual explanations.
- [Documentation index](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/DOCUMENTATION_INDEX.md) — navigate the full guide and reference set.

[Source repository](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS) · [NuGet package](https://www.nuget.org/packages/TheSingularityWorkshop.FSM_COS)

---

<p align="center">
  <strong>The Singularity Workshop</strong><br />
  <a href="https://github.com/TrentBest">GitHub</a> · <a href="https://coderlegion.com/">Coder Legion</a> · <a href="https://www.patreon.com/c/TheSingularityWorkshop">Patreon</a> · <a href="https://www.paypal.com/donate/?hosted_button_id=3Z7263LCQMV9J">PayPal</a>
</p>

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
