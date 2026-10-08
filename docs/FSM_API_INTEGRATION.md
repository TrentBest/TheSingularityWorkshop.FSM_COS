# FSM_API Integration: The Behavior Layer Beneath FSM_COS

FSM_COS uses FSM_API as a lower-level behavior/state primitive. The two packages have different responsibilities and must remain independently useful.

- **FSM_API** defines and updates state-machine behavior.
- **MicroBundleDomain** owns the MicroBundle contract.
- **FSM_COS** resolves a runtime request, closes dependencies, loads and arbitrates MicroBundles, and returns a stable `RuntimeAssembly`.
- **The host** decides when and how the assembled result executes or manifests.

## Current integration

The current FSM_COS development line consumes the published FSM_API 1.x package. That string-backed implementation is sufficient for the composition responsibilities currently required by FSM_COS. The ecosystem does not need to wait for integer-backed storage before improving composition, documentation, integration, or the path toward a usable release.

```text
Host / application
      │
      │ owns execution and manifestation
      ▼
   FSM_COS ───────────────► RuntimeAssembly
      │
      │ uses behavior/state primitives
      ▼
   FSM_API
```

FSM_COS should depend on the public FSM_API contract it actually needs, not on internal registry details or a specific backing representation.

## FSM_API 2.0.0 direction

The next intended FSM_API release is **2.0.0**, not another 1.x release. The 2.0.0 direction is to support string and/or integer backing, with integer-backed operation still under development.

That roadmap must not be misrepresented as an already completed capability. Until the 2.0.0 contract is implemented, tested, and released:

- use the currently available package where it satisfies the consumer;
- do not make FSM_COS completion contingent on integer backing;
- do not couple FSM_COS documentation or implementation to an unfinished internal representation;
- update this guide when the actual 2.0.0 public contract changes.

The integration boundary should stay stable even if FSM_API changes how it stores or resolves state identity internally.

## Existing-project integration

If an application already uses FSM_API, do not replace its application loop or move domain ownership into FSM_COS. Add FSM_COS only when the application needs runtime composition of MicroBundles.

If an application only needs state machines, FSM_API alone is the correct dependency. If it needs to assemble a declared computation from MicroBundles, FSM_COS can consume those lower-level primitives while the host retains control of execution.

## Change synchronization rule

When a neighboring package changes, update both sides of the contract:

1. Update that package's own README and technical documentation first.
2. Check the public API and package version actually used by FSM_COS.
3. Update this integration guide and the relevant FSM_COS docs only where the contract or recommended usage changed.
4. Keep future plans clearly labeled as plans; do not describe a branch-only feature as a published capability.
5. Build and test the affected repositories. Keep package publishing disabled unless explicitly approved.

This is a living integration contract, not a promise that every package must release in lockstep.
