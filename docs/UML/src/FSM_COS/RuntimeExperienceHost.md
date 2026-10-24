<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# RuntimeExperienceHost

**Purpose.** Coordinates composition and retention of multiple runtime experiences for a host.

**Declaration.** `public sealed class RuntimeExperienceHost` in `src/FSM_COS/RuntimeExperienceHost.cs`.

## Contract card

| Member | Meaning |
|---|---|
| Constructor | Requires an `IFsmCos` composer; accepts an optional `RuntimeExperienceRegistry`. |
| `Registry` | Registry used to retain composed experiences. |
| `ComposeAndRegister(manifest, configurationSource)` | Composes through `IFsmCos.Execute`, registers the resulting assembly, and throws if its runtime identity is already registered. |
| `TryActivate(runtimeId)` | Selects a previously composed experience through the registry. |
| `ClearActive()` | Clears the active selection but retains registered experiences. |

## Boundary

Composing/registering does not activate an experience. The host owns presentation and execution; this class coordinates composition and registry selection only.

## Use cases

- **Actor:** application host. **Goal:** prepare an experience before switching selection. **Outcome:** successful composition is retained while the current active selection remains unchanged.
- **Duplicate identity:** registration failure throws `InvalidOperationException`; the newly composed assembly is not registered.
- **Activation failure:** returns false when the requested runtime identity is absent.

## Evidence

- Source: [RuntimeExperienceHost.cs](../../../../src/FSM_COS/RuntimeExperienceHost.cs)
- Related types: [IFsmCos](IFsmCos.md), [RuntimeExperienceRegistry](RuntimeExperienceRegistry.md), [RuntimeAssembly](RuntimeAssembly.md).
- Provenance: source-checked against `development`; manually authored documentation.

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
