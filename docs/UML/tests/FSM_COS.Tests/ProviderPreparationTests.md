<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# ProviderPreparationTests

**Purpose.** Verifies the source-integration contract for typed provider discovery and preparation before MicroBundle loading.

**Source.** [ProviderPreparationTests.cs](../../../../tests/FSM_COS.Tests/ProviderPreparationTests.cs)

## Covered behaviors

| Test | Contract verified |
|---|---|
| `Execute_prepares_every_bundle_in_deterministic_order_before_any_load` | All opt-in preparation callbacks run in ascending bundle-ID order before the first `Load`; exact typed provider references resolve from the closure snapshot. |
| `Preparation_context_rejects_unpinned_or_mismatched_provider_references` | Unversioned and owner-version-mismatched references return false without invoking the host resolver. |
| `TryExecute_reports_provider_preparation_failure_before_loading` | A preparation exception is reported in `RuntimeCompositionPhase.ProviderPreparation`; no load callback is attempted. |

## Evidence limits

These are focused integration tests against the MicroBundleDomain development contract. They do not prove a concrete GUI provider, a real MicroBundle package load, persistence of provider handles, or Workshop creator-station rendering. The test is included in the repository-source and local-development-package integration modes; it is excluded from public-package compatibility until the domain package is released.

## Related contracts

- [MicroBundlePreparationContext](../../src/FSM_COS/MicroBundlePreparationContext.md)
- [Typed provider integration](../../../CONSUMING_MICROBUNDLES.md)
- [Composition sequence](../../SequenceDiagrams/FsmCos-Execute.md)

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
