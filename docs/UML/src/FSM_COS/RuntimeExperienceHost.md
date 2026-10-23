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
