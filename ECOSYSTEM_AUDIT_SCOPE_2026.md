<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# 2026 Ecosystem Audit Scope and Coverage Ledger

**Owner:** Trent Best  
**Status:** Active; inventory and first-pass coverage only  
**Scope:** Repositories operated on during 2026 that are part of, support, demonstrate, benchmark, or host The Singularity Workshop ecosystem. This deliberately excludes unrelated older projects unless current-year work brings them back into scope.

This is a **coverage ledger**, not a claim that every listed repository has already been audited. Repository README inspection alone does not count as source-level or integration verification. The audit is looking for missed reuse of existing Workshop technology, duplicated responsibilities, unfinished cross-repository contracts, and proof gaps.

## Release readiness companion

The current release triage is maintained in [RELEASE_READINESS_PORTFOLIO.md](RELEASE_READINESS_PORTFOLIO.md). It records exact development HEADs and workflow evidence, separates package blockers from ecosystem integration work, and explicitly avoids readiness percentages where evidence is not comparable. Treat it as a dated snapshot; refresh it when candidate SHAs change. No entry authorizes publication.

## 01 — Confirmed high-priority architecture findings

| Priority | Finding | Current evidence | Next proof |
|---|---|---|---|
| P0 | Host execution/lifecycle boundaries are not yet unified through Foundry | Foundry development architecture says FSM_COS integration and a reusable FSM_API adapter are not implemented; WebApp documents a private heartbeat and 100 actor FSM contexts as transitional | Trace real AnyApp, WebPage, WebApp, and The callers; define a tested adapter without conflating Foundry workflows, FSM_COS composition, and FSM_API ticks |
| P0 | WebPage's composition and Hub activation may create two execution paths | FSM_COS audit found composition followed by a separate Hub bundle activation; exact equivalence/intent needs caller and test verification | Trace lifecycle, cancellation, disposal, errors, and observable results end to end |
| P1 | Published Experience resolution is not yet a single shared host path | AnyApp has repository-backed resolution; WebPage still has a fixed composition catalog; MicroBundleRepository has a version-aware catalog | Clean-checkout resolution by immutable BundleId + Version + ContentHash |
| P1 | Publication, artifact delivery, and Experience discovery do not yet form a proven round trip | Publisher/workflow/catalog findings in the FSM_COS audit; repository REST catalog is read-oriented in inspected paths | Clean-room publish → catalog discovery → immutable retrieval → host composition test |
| P1 | Foundry lifecycle exists, but cross-host adoption is incomplete | Foundry documents its own sequential runner/coordinator, while WebApp and The describe separate transitional lifecycle implementations | Host-by-host adapter and migration evidence; preserve host-owned presentation/capabilities |
| P1 | Runtime observations and benchmark evidence are not yet proven to feed metaDev as a reproducible feedback loop | metaDev design and benchmark plans exist; implementation of the collector/adapter still needs source verification | Raw measurements with workload, commit, source/package mode, runtime/machine, allocations, phase timings, and outcomes |
| P1 | WebPage manifest activation needs cross-owner atomicity proof | `SwitchManifestAsync` mutates Hub activation before committing the Experience execution adapter; the outer failure path does not restore prior Hub state if a later commit operation throws | Inject post-Hub-activation failure and prove prior Hub bundles, execution adapter, manifest, and assembly remain active |
| P2 | Serialization consumer boundaries need a complete round-trip audit | AnyApp has source-level stable-ID/link round-trip, unreadable-save preservation, and a two-process restart probe; StoryBuilder browser persistence has fake-JS coordinator round-trip coverage, not a real browser reload test | Run the AnyApp restart probe in CI; add real browser reload/storage coverage; continue consumer inventory and malformed/truncated/compatibility tests |
| P2 | Semantic intent integration needs shared conformance fixtures | AnyApp integrates ProtocolAi, GrammarAi, and FSM_UserIO; cross-package fixture coverage is not yet established | Unknown symbols, grammar rejection, protocol mismatch, and admission-vs-authorization cases |
| P2 | DataWarehouse adoption should be evidence-driven | Low-level store and separate benchmark laboratory exist; no specific higher-level consumer benefit has yet been proven by this audit | Select a real simulation/telemetry workload and compare correctness, allocations, and performance |
| P2 | MicroBundleIngestor publication policy needs repository-side proof | Forge-only Put authority is described as a policy; the repository host must enforce it | Negative authorization tests and contract-valid generated artifacts |

## 02 — Repository inventory

Status legend:
- **Source review started** — explicit source/document reads have produced findings, but the repository is not fully audited.
- **Cross-repo sampled** — relevant entry-point/docs were read; code/tests/workflows still need inspection.
- **Queued** — in-scope repository identified; individual source and integration review remains.
- **Separate audit** — repository has a focused workstream (for example release readiness or benchmark coverage) that must be coordinated with this ecosystem audit rather than duplicated.

### Runtime, composition, and hosts

| Repository | Coverage | Focus |
|---|---|---|
| [TheSingularityWorkshop.Foundry](https://github.com/TrentBest/TheSingularityWorkshop.Foundry) | Source review started | Workflow execution, retained lifecycle, cancellation, FSM_API adapter, host migration |
| [AnyApp](https://github.com/TrentBest/AnyApp) | Source review started | Runtime composition, repository catalog, metaDev, StoryBuilder persistence, semantic exchange |
| [WebPage](https://github.com/TrentBest/WebPage) | Source review started | Legacy Hub lifecycle, FSM_COS composition, activation path, fixed catalog, host adapter |
| [TheSingularityWorkshop.WebApp](https://github.com/TrentBest/TheSingularityWorkshop.WebApp) | Source review started | Private heartbeat/actor FSMs, AnyApp parity, browser input and presentation |
| [The](https://github.com/TrentBest/The) | Source review started | Architect async-void startup callback; pure lifecycle-policy tests exist but do not prove awaited provider/shutdown behavior |
| [TheSingularityWorkshop.MyVR](https://github.com/TrentBest/TheSingularityWorkshop.MyVR) | Cross-repo sampled | Device capability, immersive lifecycle, bounded Experience interaction |
| [StoryBuilder](https://github.com/TrentBest/StoryBuilder) | Source review started | Authoring and FSM_COS composition test exist; package-to-manifest-to-artifact-to-host vertical slice is explicitly not complete |
| [TheSingularityWorkshop.Experiences](https://github.com/TrentBest/TheSingularityWorkshop.Experiences) | Source review started | Experience identity, publisher workflow, host-neutral lifecycle and demo integration |
| [TheSingularityWorkshop.FSM_COS](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS) | Source review started | Composition, RuntimeAssembly, arbitration, catalog seams, docs and release boundaries |
| [TheSingularityWorkshop.FSM_REST](https://github.com/TrentBest/TheSingularityWorkshop.FSM_REST) | Source review started | Reusable REST capability substrate; inspect real capability consumers and transport/error conformance |
| [TheSingularityWorkshop.MicroBundleRepository](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleRepository) | Source review started | Immutable artifact identity, REST catalog, publication/discovery, authorization |
| [TheSingularityWorkshop.MicroBundleDomain](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleDomain) | Cross-repo sampled | Contract fit, lifecycle semantics, package neutrality |
| [TheSingularityWorkshop.MicroBundleIngestor](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleIngestor) | Cross-repo sampled | OpenAPI ingestion, generated capability conformance, Forge-only publication authority |

### Core FSM, persistence, and semantic boundaries

| Repository | Coverage | Focus |
|---|---|---|
| [FSM_Layer_Generics](https://github.com/TrentBest/FSM_Layer_Generics) | Queued | Generic FSM layer contract; verify whether host projects duplicate layer behavior or intentionally stay on FSM_API |
| [FSM_Layer_DependencyInjection](https://github.com/TrentBest/FSM_Layer_DependencyInjection) | Source review started | Inspected class is empty and project has no DI contract/dependencies; do not treat as implemented adapter |
| [FSM_PackageManager](https://github.com/TrentBest/FSM_PackageManager) | Source review started | Project metadata advertises runtime package management/arbitration, but inspected implementation is an empty placeholder; historical package/consumer status unverified |
| [FSM_UnityIntegrationAdvanced](https://github.com/TrentBest/FSM_UnityIntegrationAdvanced) | Queued | Unity adapter ownership and current relevance; keep Unity-specific types outside FSM_API core |
| [FSM_ProjectTemplates](https://github.com/TrentBest/FSM_ProjectTemplates) | Source review started | Single WPF template pins FSM_API 1.0.9; current integration work uses 1.0.13; compatibility not tested |
| [FSM_Dungeon](https://github.com/TrentBest/FSM_Dungeon) | Queued | FSM_API consumer patterns and opportunities to reuse Experience/MicroBundle composition |
| [FSM_Bot](https://github.com/TrentBest/FSM_Bot) | Queued | FSM_API consumer patterns, lifecycle ownership, and host boundaries |
| [FSM_Bot_Editor](https://github.com/TrentBest/FSM_Bot_Editor) | Queued | Editor integration and GUI/domain boundary; verify actual shared GUI contracts |
| [TheForge](https://github.com/TrentBest/TheForge) | Source review started | Historical Unity micro-package/arbitration, GUI builders, and unsafe DataWarehouse overlap current packages conceptually; no drop-in reuse assumed |
| [RevitFamilyManagerBuilders](https://github.com/TrentBest/RevitFamilyManagerBuilders) | Cross-repo sampled | Composable Builders migration; live Revit catalog synchronization remains explicitly unverified for production |
| [FSM_API](https://github.com/TrentBest/FSM_API) | Source review started | Host execution mechanics, identity/cleanup, integration contract |
| [FSM_API_Benchmark](https://github.com/TrentBest/FSM_API_Benchmark) | Separate audit | Named benchmark coverage, historical comparisons, CI regression evidence |
| [TheSingularityWorkshop.FSM_Serialization](https://github.com/TrentBest/TheSingularityWorkshop.FSM_Serialization) | Separate audit | Serialization contracts, ownership, failure handling, consumers |
| [FSM_Serialization_Benchmarks](https://github.com/TrentBest/FSM_Serialization_Benchmarks) | Separate audit | Comprehensive serialization benchmarks and regression evidence |
| [FSM_UserIO](https://github.com/TrentBest/FSM_UserIO) | Separate audit | NuGet release readiness, user input/output contracts, host integration |
| [TheSingularityWorkshop.ProtocolAi](https://github.com/TrentBest/TheSingularityWorkshop.ProtocolAi) | Cross-repo sampled | Protocol identities, semantic exchange, conformance fixtures |
| [TheSingularityWorkshop.GrammarAi](https://github.com/TrentBest/TheSingularityWorkshop.GrammarAi) | Cross-repo sampled | Grammar construction, rejection semantics, consumer fixtures |
| [TheSingularityWorkshop.HeadlessAi](https://github.com/TrentBest/TheSingularityWorkshop.HeadlessAi) | Cross-repo sampled | Provider-neutral HTTP/protocol adapters; verify consumer reuse and bounded failure behavior |
| [TheSingularityWorkshop.HeadlessAi.AnyAppExample](https://github.com/TrentBest/TheSingularityWorkshop.HeadlessAi.AnyAppExample) | Queued | Actual consumer integration and example parity |
| [TheSingularityWorkshop.HeadlessAi.Benchmarks](https://github.com/TrentBest/TheSingularityWorkshop.HeadlessAi.Benchmarks) | Queued | Workload validity, benchmark linkage, regressions |
| [TheSingularityWorkshop.Ontology](https://github.com/TrentBest/TheSingularityWorkshop.Ontology) | Cross-repo sampled | Optional semantic contracts and actual consumers |

### GUI, rendering, visualization, physics, and procedural capability

| Repository | Coverage | Focus |
|---|---|---|
| [TheSingularityWorkshop.GUI](https://github.com/TrentBest/TheSingularityWorkshop.GUI) | Separate audit | Full GUI domain implementation and renderer/host contracts |
| [TheSingularityWorkshop.Renderer](https://github.com/TrentBest/TheSingularityWorkshop.Renderer) | Cross-repo sampled | Observer/representation contracts and actual consumers |
| [TheSingularityWorkshop.Renderer.Benchmarks](https://github.com/TrentBest/TheSingularityWorkshop.Renderer.Benchmarks) | Queued | Renderer workload matrix and performance evidence |
| [TheSingularityWorkshop.Visualize](https://github.com/TrentBest/TheSingularityWorkshop.Visualize) | Source review started | Trace ingestion, replay, generated documentation, GUI.SVG handoff |
| [TheSingularityWorkshop.Physics](https://github.com/TrentBest/TheSingularityWorkshop.Physics) | Cross-repo sampled | Deterministic physical model and demo integration |
| [TheSingularityWorkshop.Gestures](https://github.com/TrentBest/TheSingularityWorkshop.Gestures) | Cross-repo sampled | CPU reference evaluator, deterministic variation, renderer/physics consumers |

### Benchmarking, evidence, and CI

| Repository | Coverage | Focus |
|---|---|---|
| [FSM_COS_Benchmarks](https://github.com/TrentBest/FSM_COS_Benchmarks) | Separate audit | Broad named benchmark coverage and historical deltas |
| [MicroBundleDomain_Benchmarks](https://github.com/TrentBest/MicroBundleDomain_Benchmarks) | Queued | Contract/runtime cost, workload relevance |
| [TheSingularityWorkshop.CiAi](https://github.com/TrentBest/TheSingularityWorkshop.CiAi) | Source review started | Reusable named evidence, provenance, explicit release authorization |
| [TheSingularityWorkshop.CiAi.CogA](https://github.com/TrentBest/TheSingularityWorkshop.CiAi.CogA) | Queued | Actual cog responsibility and reusable integration |
| [TheSingularityWorkshop.CiAi.CogB](https://github.com/TrentBest/TheSingularityWorkshop.CiAi.CogB) | Queued | Actual cog responsibility and reusable integration |
| [TheSingularityWorkshop.CiAi.CogC](https://github.com/TrentBest/TheSingularityWorkshop.CiAi.CogC) | Queued | Actual cog responsibility and reusable integration |
| [TheSingularityWorkshop.CiAi.CogD](https://github.com/TrentBest/TheSingularityWorkshop.CiAi.CogD) | Queued | Actual cog responsibility and reusable integration |
| [TheSingularityWorkshop.CiAi.CogE](https://github.com/TrentBest/TheSingularityWorkshop.CiAi.CogE) | Queued | Actual cog responsibility and reusable integration |

### Data, business, and higher-level capability consumers

| Repository | Coverage | Focus |
|---|---|---|
| [TheSingularityWorkshop.DataWarehouse](https://github.com/TrentBest/TheSingularityWorkshop.DataWarehouse) | Cross-repo sampled | Data structure boundaries, real consumers, no unsupported performance claims |
| [TheSingularityWorkshop.Benchmarks.DataWarehouse](https://github.com/TrentBest/TheSingularityWorkshop.Benchmarks.DataWarehouse) | Cross-repo sampled | Workload coverage and measured benefit |
| [SingularityWarehouse](https://github.com/TrentBest/SingularityWarehouse) | Queued | Core/API/WPF/styling and Revit/Inventor adapters; distinguish from low-level DataWarehouse |
| [TheSingularityWorkshop.Profiles](https://github.com/TrentBest/TheSingularityWorkshop.Profiles) | Cross-repo sampled | Profile contract reuse and consumers |
| [TheSingularityWorkshop.Economy](https://github.com/TrentBest/TheSingularityWorkshop.Economy) | Queued | Domain contract reuse and runtime integration |
| [TheSingularityWorkshop.CreatorNetwork](https://github.com/TrentBest/TheSingularityWorkshop.CreatorNetwork) | Queued | Creator identity/distribution, repository/Experience integration |

### Demonstrations, article evidence, and workshop root

| Repository | Coverage | Focus |
|---|---|---|
| [SimpleDemos](https://github.com/TrentBest/SimpleDemos) | Source review started | Trace-to-visualization, stress methodology, real consumer proofs |
| [TheSingularityWorkshop](https://github.com/TrentBest/TheSingularityWorkshop) | Queued | Shared docs/brand/contracts and cross-repository navigation |

## 03 — How coverage advances

For each repository, the audit should record:
1. **Identity and activity:** confirm it belongs to 2026 work and read the intended active branch (usually `development`).
2. **Actual implementation:** inspect entry points, project references, public contracts, adapters, and tests—not only README claims.
3. **Neighbor contracts:** inspect the upstream/downstream repository where a boundary is crossed.
4. **Reuse decision:** mark as confirmed missed integration, intentional boundary, already implemented, or unresolved candidate.
5. **Proof:** run the narrow relevant tests/build/workflow when available; explicitly label anything that remains source-review only.
6. **Record:** add source links, confidence, priority, acceptance evidence, and date to the canonical audit.

Prioritize by architectural leverage: runtime ownership and duplicate execution first; artifact publication/resolution next; serialization, semantic, and rendering contracts after that; measured performance opportunities next; documentation and low-risk cleanup last.

## 04 — Important exclusions and constraints

- Do not publish NuGet, deploy, or authorize a release as part of this audit.
- Do not delete branches or assume every non-`master`/non-`development` branch is disposable.
- Do not move host-specific UI, credentials, platform permissions, or device APIs into FSM_COS.
- Do not make Foundry workflow steps a substitute for FSM_API state transitions or treat a RuntimeAssembly as a workflow definition.
- Do not adopt DataWarehouse, GUI, Ontology, or any other package merely because it exists; demonstrate a real consumer and measurable or contractual value.
- Do not call this scope complete until every in-scope repository has an explicit disposition.

## 05 — Current limitations

This inventory is assembled from 2026 conversation/workstream context, current-year commit search results, current development-branch tip checks, and repository source/README reads. Some newly added repositories are confirmed as touched in 2026 by documentation commits only and remain queued for source-level audit. It is a working scope, not a GitHub-wide proof of every repository touched during the year. Repositories may be added when commit history or work records show 2026 activity. Some linked repositories have only had README/architecture-level sampling; their code, tests, and workflows remain unaudited.

## 06 — Repository-local issue handoff

Every source-backed gap discovered during this audit must be handed to the owning repository's development issue tracker, not left only in the central audit document.

- Search existing open issues first. Link the existing issue when it already owns the gap; do not create a duplicate merely to restate it.
- Create an issue in the owning repository when no suitable issue exists. Give the repository LLM a source-backed problem statement, exact files/contracts to inspect, architectural ownership and non-goals, focused implementation steps, deterministic acceptance tests, documentation expectations, and explicit no-release/no-deployment constraints.
- Keep uncertainty honest: distinguish a demonstrated defect from a reachable risk, an unimplemented contract, a missing test, and a candidate integration that still needs consumer evidence.
- Cross-link the owning issue from [CROSS_REPOSITORY_MISSED_INTEGRATIONS.md](CROSS_REPOSITORY_MISSED_INTEGRATIONS.md) so the central audit remains navigable.
- An issue being created or a test existing in source is not proof of implementation, CI success, or release readiness. Record actual execution evidence separately.
- Preserve ownership boundaries and use the `development` branch; do not publish NuGet, deploy, or change `master` without explicit authorization.

Initial handoffs created during this iteration:
- [WebPage #126 — atomic manifest activation](https://github.com/TrentBest/WebPage/issues/126)
- [StoryBuilder #5 — real browser persistence/reload proof](https://github.com/TrentBest/StoryBuilder/issues/5)
- [FSM_PackageManager #2 — reconcile advertised behavior with implementation](https://github.com/TrentBest/FSM_PackageManager/issues/2)
- [The #20 — awaited Architect startup lifecycle](https://github.com/TrentBest/The/issues/20)
- [GUI #59 — shared semantic adapter conformance](https://github.com/TrentBest/TheSingularityWorkshop.GUI/issues/59)
- [Experiences #28 — StoryBuilder capture-to-AnyApp vertical slice](https://github.com/TrentBest/TheSingularityWorkshop.Experiences/issues/28)

Existing issues reused rather than duplicated include [Foundry #1](https://github.com/TrentBest/TheSingularityWorkshop.Foundry/issues/1), [WebApp #10](https://github.com/TrentBest/TheSingularityWorkshop.WebApp/issues/10), [AnyApp #23](https://github.com/TrentBest/AnyApp/issues/23), and [MicroBundleRepository #22/#23](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleRepository/issues/23).

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
