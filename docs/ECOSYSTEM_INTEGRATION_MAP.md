# Ecosystem Integration Map

> **Status:** architecture working document. This map separates verified repository/package facts from proposed composition decisions. It is not a release plan and does not authorize publishing, merging, or breaking package changes.

## Purpose and decision rule

The goal is to make **FSM_COS the composition boundary for the Workshop ecosystem**, while reducing unnecessary always-installed NuGet dependencies. A repository existing as a NuGet package today does not mean it must remain a separately installed runtime dependency forever; equally, a package must not be collapsed into a MicroBundle merely because it is small.

Every candidate must be classified by responsibility, runtime needs, reuse, and its relationship to the composition boundary.

**Core rule:** retain independently useful, stable contracts as packages; consider runtime-composed capabilities as MicroBundles when they can be discovered, configured, arbitrated, and hosted without creating upward dependencies or duplicating foundational infrastructure.

## The target shape

```text
Host applications (WebPage, AnyApp, WebApp, other hosts)
                    |
                    v
          FSM_COS composition boundary
          - manifests and configuration
          - catalog / discovery
          - arbitration and conflict policy
          - runtime assembly / handoff
                    |
                    v
          MicroBundle capability ecosystem
          - optional, composable domain behaviors
          - capability-specific providers
          - per-bundle configuration and defaults
                    |
                    v
          Stable foundational contracts
          - FSM_API / FSM_Layer
          - domain contracts and shared primitives
```

This is a **conceptual target**, not a claim that every repository is integrated today. The exact dependency graph must be verified from each project file, package metadata, and implementation before declaring a package integrated.

### Directional constraints

- `FSM_API` must remain foundational and independent of `FSM_COS`.
- `FSM_Layer` may build on `FSM_API`; lower-level libraries must not depend upward on `FSM_COS`.
- `FSM_COS` composes capabilities; it should not absorb every domain's implementation.
- Hosts own presentation, platform integration, and application-specific lifecycle. They consume the composition boundary rather than becoming prerequisites for lower packages.
- MicroBundles express optional capabilities. Their domain contracts should not require a specific GUI, host, or execution loop.
- A NuGet package may remain valuable as a **development-time contract** even when its concrete runtime capability is delivered as a MicroBundle.

## Verified source snapshot (master branches inspected 2026-10-08)

These are facts read directly from project files during this audit. They describe repository source metadata, **not confirmation that every listed package version is published or currently consumed successfully**.

| Project file | Declared package version | Direct package/project dependencies visible in source | Immediate implication |
|---|---|---|---|
| FSM_COS `src/FSM_COS/FSM_COS.csproj` | `0.1.0-alpha.6` | `FSM_API 1.0.13`; `FSM_UserIO 0.1.0-alpha.1`; `MicroBundleDomain 1.0.1` | UserIO is currently a hard package dependency of FSM_COS. Review whether the semantic interaction contract belongs in the mandatory composition kernel or should be an optional capability; do not remove it without tracing code/API use. |
| MicroBundleDomain `TheSingularityWorkshop.MicroBundleDomain.csproj` | `1.0.1` | No PackageReference shown in the project file inspected | A comparatively neutral domain contract is a plausible stable dependency foundation. |
| FSM_Serialization `TheSingularityWorkshop.FSM_Serialization.csproj` | `1.0.0` | No PackageReference shown in the project file inspected | Treat as a stable serialization package candidate; inspect API/format compatibility before making it a core dependency. |
| Ontology `TheSingularityWorkshop.Ontology.csproj` | `0.1.0-alpha.3` | `MicroBundleDomain 1.0.1` | Ontology is currently built above the bundle domain contract, not the reverse. |
| MicroBundleRepository Core | `0.1.0-alpha.3` | `FSM_Serialization 0.1.0-alpha.2`; `Ontology 0.1.0-alpha.2` | References do not match the current version declarations found in the inspected Serialization and Ontology project files; determine whether older packages are intentionally pinned or stale. |
| MicroBundleRepository REST | `0.1.0-alpha.5` | `FSM_REST 0.1.0-alpha.4`; `Ontology 0.1.0-alpha.2`; project reference to Core | REST and Core should be evaluated as separate adapter layers, not automatically bundled into every FSM_COS consumer. |
| MicroBundleRepository FSM_COS adapter | `0.1.0-alpha.1` | `FSM_COS 0.1.0-alpha.5`; `MicroBundleDomain 1.0.1`; project reference to REST | This adapter currently targets FSM_COS alpha.5 while its inspected source project declares alpha.6. That is a concrete alignment item to resolve with build/compatibility evidence. |
| Experiences Moniker | `0.1.0-alpha.1` | `GUI.Core 0.1.0-alpha.4`; `MicroBundleDomain 1.0.1` | The Moniker capability currently depends on GUI.Core at compile/package time. Decide whether the bundle contract should expose a host-neutral experience capability and load presentation separately. |
| Experiences Elements | `0.1.0-alpha.1` | `MicroBundleDomain 1.0.1` | This is closer to the optional domain-capability pattern; validate whether it can be loaded and arbitrated by FSM_COS today. |
| Renderer | `0.1.0-alpha.2` | `FSM_API 1.0.13`; `ProtocolAi 0.1.0-alpha.2`; `MicroBundleDomain 1.0.1` | ProtocolAi is currently a hard dependency of Renderer. Confirm it is essential to the renderer's stable contract or isolate it behind an optional semantic adapter. |
| GUI.Core | `0.1.0-alpha.4` | No PackageReference shown in its project file inspected | Platform-neutral GUI core may be reusable independently; host/platform adapters should remain separate. |
| GUI.WPF | `0.1.0-alpha.6` | Project reference to GUI.Core | WPF belongs at the host-adapter edge, not in FSM_COS or MicroBundleDomain. |
| FSM_UserIO | `0.1.0-alpha.1` | No PackageReference shown in its project file inspected | The declared package is platform-neutral and small by intent; its mandatory placement in FSM_COS should be justified by actual use. |

### Confirmed documentation/integration gap: Profiles

The inspected `Profiles` repository's `master` tree contains only a minimal `README.md` and no visible `.csproj`, source, or test files. Its broader profile/sharing model is known as a design direction, but the implementation, package identity, contracts, and integration path cannot yet be verified from that branch. Treat it as **unclassified / not demonstrated as integrated**, not as a functioning MicroBundle. Before designing an adapter, check all branches and open issues for work not present on master.

### Highest-value follow-up checks

1. Verify actual NuGet availability and API compatibility for the version pairs called out above.
2. Trace FSM_COS's direct use of FSM_UserIO and Renderer’s direct use of ProtocolAi before changing either dependency.
3. Verify that Experiences artifacts implement the current MicroBundleDomain contracts and are discoverable by a real FSM_COS catalog.
4. Audit every package project file and consumer before deciding which packages are foundations, adapters, optional bundles, or standalone tools.
5. Keep version alignment changes separate from documentation changes, with builds/tests and explicit release approval.

## Package versus MicroBundle

Use this decision table as the default, then record exceptions with evidence.

| Question | Keep as a package when… | Consider a MicroBundle when… |
|---|---|---|
| Is it foundational? | Other components require its stable types or algorithms. | It is an optional capability selected at runtime. |
| Does it define contracts? | Multiple independent consumers need compile-time contracts. | It implements a capability behind existing bundle/provider contracts. |
| Is runtime choice useful? | The code is required for the host or a core contract. | Users should install, enable, configure, replace, or arbitrate it independently. |
| Does it need platform APIs? | It provides a reusable adapter package for a platform. | A host-specific bundle can isolate optional integration behind host-provided interfaces. |
| Is it a domain? | The domain model is a stable, independently reused foundation. | A domain behavior can be represented as a self-contained, discoverable capability. |
| Would merging reduce value? | Separate versioning, testing, or reuse is materially beneficial. | A package mostly exists to deliver one optional runtime behavior and adds permanent dependency overhead. |

**Do not optimize for the smallest package count at the expense of clear contracts.** The target is the smallest *required runtime dependency surface*, with optional capabilities loaded only when needed.

## Initial ecosystem inventory

The entries below are starting points for an audit, not a final integration verdict. Confirm each row against its current repository, project references, NuGet metadata, tests, and actual consumers before changing package boundaries.

| Repository / package family | Known responsibility | Initial architectural treatment | Evidence still required |
|---|---|---|---|
| `FSM_API` | Foundational finite-state-machine API and behavior. | **Keep as foundational package.** It is the lowest-level behavior mechanism; next major release is expected to broaden backing options rather than publish another 1.x line. | Audit current branch, public API, direct consumers, package dependencies, and 2.0.0 readiness. |
| `FSM_Layer` | Layer above the core FSM API. | **Keep as a foundational package** if its reusable abstraction is confirmed by source and consumers. | Verify exact contract, package graph, and consumers. |
| `FSM_COS` | Manifest-driven composition/arbitration and runtime-assembly boundary. | **Keep as the composition package.** Do not turn it into a monolithic home for all capabilities. | Verify current source/package version split, all current bundle contracts, host integrations, and open PRs before branch changes. |
| `MicroBundleDomain` | Domain contracts for MicroBundles. | **Keep as a contract package** unless source audit shows a smaller stable contract boundary. | Verify interfaces, version, consumers, and which responsibilities are incorrectly coupled to runtime or UI. |
| `MicroBundleRepository` / repository-related code | Bundle location, storage, or retrieval boundary (exact scope to verify). | Likely a separate adapter/service boundary, not automatically part of every host's core dependencies. Some implementations may be optional bundles. | Identify exact repository/package name, code ownership, storage assumptions, and consumers. |
| `FSM_Serialization` | Serialization capability/package. | Keep a stable serialization contract if broadly reusable; investigate optional serializers/providers as runtime-composed capabilities. | Verify format contracts, dependency graph, whether serializers are mandatory or pluggable, and consumers. |
| `Profiles` | User-controlled profile data/sharing, including public versus work-facing profiles and individual/company/group concepts; access-log concepts have been discussed. | **Integration gap to investigate explicitly.** Do not assume FSM_COS integration exists. Model profile management and access policy as domain capabilities; decide which contracts stay packages and which implementations can be MicroBundles only after inspecting source. | README is currently effectively empty on the inspected `master` branch. Inspect source/tests/issues, define profile ontology and authorization boundaries, decide identity/access dependencies, and design an integration bundle plus manifest/configuration example. |
| `FSM_UserIO` | General user-input/output abstractions (not restricted to a human-biped model). | Preserve reusable input/output contracts; consider concrete channels, interaction modes, and optional integrations as bundles. | Verify source and consumers; separate interface from UI/platform implementation. |
| `Ontology` | Flexible ontology/layer/address concepts. | Likely a reusable domain contract/package; ontology-specific behaviors can be capabilities. Do not force every bundle into one fixed nine-layer scheme. | Verify actual repository/package, API maturity, indexing semantics, and consumers. |
| `ProtocolAi` / `GrammarAi` | Protocol/grammar-oriented concepts. | Optional integrations unless a specific contract genuinely requires them. Conceptual alignment alone is not a reason to add a hard dependency. | Verify package existence, public API, current consumers, and intended optionality. |
| `WebPage`, `WebApp`, `AnyApp` | Hosts/runtimes and user-facing experiences. | **Hosts consume FSM_COS.** Their platform/UI dependencies must not leak into foundational packages. | Compare their actual composition paths and manifest-driven behavior; identify duplicated or bypassed runtime logic. |
| Renderer / renderer adapters | Rendering and level-of-detail capability. | Keep core contracts separate from optional rendering backends; candidate rendering behaviors may be MicroBundles if the contracts support it. | Verify current repository/package names, runtime APIs, host coupling, and LOD responsibilities. |

### Inventory labels for the audit

Assign exactly one status per repository/package as evidence is gathered:

- **Verified foundation** — a stable contract or capability required by other layers.
- **Verified composition package** — infrastructure used to compose capabilities.
- **Candidate MicroBundle** — optional runtime capability whose contracts and lifecycle are compatible with the bundle system.
- **Host adapter** — platform-specific integration kept out of core.
- **Standalone tool** — useful repository that does not need to be part of FSM_COS runtime.
- **Unclassified** — insufficient source evidence; no package-boundary decision yet.

Do not label a repository “integrated” because it references a NuGet package. Integration means a capability can be discovered/selected through the intended composition path, has a defined contract and configuration, is tested in that path, and has documented ownership.

## Profiles: required investigation path

Profiles should be treated as a first-class integration study, not a footnote.

1. **Recover the domain model.** Inspect current source, tests, issues, and intended profile types: individual, company, group, public profile, work-facing profile, and user-controlled sharing.
2. **Separate data from policy.** Profile records describe identity-related/profile data; access rules decide who may see or change which data. Audit/access logging records relevant decisions and activity. Do not conflate these responsibilities.
3. **Define stable contracts.** Identify the minimum interfaces needed to describe profiles, access decisions, and sharing without depending on a web UI or a specific host.
4. **Map ontology intentionally.** Decide whether profile kinds, audience, ownership, relationships, and permissions belong in an ontology, explicit contracts, or both. Avoid making a single fixed layer scheme mandatory.
5. **Compose optional behaviors.** Candidate bundles might include profile presentation, sharing workflows, group/company management, and audit views—but only after source review confirms coherent capabilities and dependencies.
6. **Declare security invariants.** Default-deny decisions where access is unresolved; enforce authorization at the data boundary, not only in UI; distinguish identity claims from authorization; document audit retention and sensitive-data handling. These are design requirements to validate, not claims that the current code already satisfies them.
7. **Demonstrate it end to end.** Add a manifest/configuration example, a host-independent test of arbitration/selection, and a host integration test showing the selected profile capability is actually used.

No implementation or security guarantee should be inferred from this plan alone.

## Integration definition of done

A package/capability is considered integrated only when all applicable checks are satisfied:

- [ ] Its responsibility and non-goals are documented.
- [ ] Its direct dependencies and dependency direction are verified from source/project metadata.
- [ ] It is classified as a foundation, composition package, MicroBundle candidate, host adapter, or standalone tool.
- [ ] Stable contracts are separated from optional implementation where doing so is useful.
- [ ] Runtime discovery, configuration defaults, conflict behavior, and failure behavior are documented when it is a MicroBundle.
- [ ] A manifest or configuration example demonstrates the intended composition route.
- [ ] Tests prove the composition path rather than only testing the package in isolation.
- [ ] The README links to the authoritative architecture and API documents.
- [ ] The published package state is distinguished from source state.
- [ ] Any package merge, split, deprecation, or migration has an explicit decision record and compatibility plan.

## Work sequence

1. **Establish the map:** enumerate every Workshop repository/package and all project/package references in consumers.
2. **Verify the foundations:** `FSM_API`, `FSM_Layer`, `MicroBundleDomain`, `FSM_COS`, and serialization/repository contracts.
3. **Audit the capability candidates:** start with `Profiles`, then user I/O, ontology/protocol/grammar, rendering, and other optional domains.
4. **Trace hosts:** verify WebPage, WebApp, and AnyApp consume FSM_COS through the same documented composition model, and record divergences.
5. **Make package-boundary decisions:** retain, merge, split, replace with a MicroBundle, or leave standalone—with evidence and migration consequences.
6. **Integrate incrementally:** one capability and test path at a time. Do not combine this work with unapproved package releases.
7. **Publish the architecture map:** keep this document current and link each repository to its own authoritative docs.

## Documentation and release safeguards

- Use the shared section sequence and color taxonomy defined in [the Documentation Standard](../DOCUMENTATION_STANDARD.md).
- Keep the source branch, published package, and intended target architecture distinct.
- Respect open pull requests and branch divergence; do not merge or rewrite branches as a side effect of a documentation audit.
- NuGet publishing remains explicitly gated and requires the user's separate approval.
- When a claim is unverified, record the missing evidence instead of turning a hypothesis into a fact.

---

*The Singularity Workshop — architecture should make capabilities composable without making every capability mandatory.*
