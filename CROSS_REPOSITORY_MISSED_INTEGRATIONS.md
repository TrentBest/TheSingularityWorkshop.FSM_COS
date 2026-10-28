<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# Cross-Repository Missed Integration Opportunities

**Status:** Active source-review addendum. Findings are opportunities to verify and close, not claims that every candidate is a confirmed defect.  
**Purpose:** Identify where existing Workshop technology can replace a parallel implementation or where a valuable integration remains unfinished.

## 01 — P1: Reuse the real trace-to-visualization path for the Fox-and-sheepdog demo

**Evidence**

- [SimpleDemos demonstration gallery](https://github.com/TrentBest/SimpleDemos/blob/development/demonstrations/README.md) documents actual C# FSM_API trace export, FSM_COS trace comparison, Visualize replay, and an automated Drone Construction path through Renderer → Visualize → GUI.SVG.
- The same gallery explicitly labels interactive browser control as **not implemented yet** in the older Fox-and-sheepdog demonstration path.
- [The Fox-and-sheepdog page](https://github.com/TrentBest/SimpleDemos/blob/development/docs/fox-and-sheepdog.html) uses a GUI.SVG 10×10 illustration, but distinguishes that illustration from compiled FSM execution.
- Drone Construction proves useful real-data rendering pieces exist; it does not prove the Fox-and-sheepdog page already uses the same pipeline.

**Opportunity**

Reuse the trace/snapshot-to-visualization pipeline for the Fox-and-sheepdog sample instead of maintaining a hand-modeled browser illustration as the only visual experience. Keep the illustration clearly labeled. Add deterministic C# trace export, a trace-driven GUI.SVG/Visualize view, then interactive controls that submit input to the authoritative simulation runner through an explicit host adapter.

**Boundary:** C# execution and trace remain authoritative. SVG presentation must not invent FSM transitions or imply that a picture is the production FSM.

**Acceptance evidence:** Direct and FSM_COS-composed fixed-seed runs emit documented traces; renderer consumes those traces without inventing state; tests cover trace schema and stable identities; interactive controls change authoritative input and corresponding output; docs distinguish illustration, trace replay, and interactive execution.

**Confidence:** High that this is an unfinished integration; the gallery explicitly marks part of this path incomplete. Re-check current Visualize/GUI source before implementation to reuse current adapters.

## 02 — P1: Close the measurement loop between runtime observations, benchmarks, and metaDev

**Evidence**

- [AnyApp metaDev boundary document](https://github.com/TrentBest/AnyApp/blob/development/docs/METADEV.md) defines metaDev as an observer above FSM_COS that supplies measured configuration evidence without making FSM_COS depend on a learning service.
- It describes allocation/high-water behavior, population counts, cleanup/reclamation, execution timings, and machine characteristics.
- [SimpleDemos demonstration gallery](https://github.com/TrentBest/SimpleDemos/blob/development/demonstrations/README.md) says FSM_COS stress results remain provisional and calls for controlled order, independent runs, phase instrumentation, and robust statistics.
- [SimpleDemos article notebook](https://github.com/TrentBest/SimpleDemos/blob/development/docs/ARTICLE_NOTEBOOK.md) specifies a benchmark matrix spanning validation, dependency closure, catalog lookup, loading, arbitration, assembly, failure paths, repeated composition, setup cost, reuse, and steady-state execution.

**Opportunity**

Connect real benchmark/runtime evidence to the configuration inputs metaDev is intended to inform. First verify whether a runtime collector or benchmark-to-configuration adapter already exists; the document alone does not prove implementation exists. If absent, add a narrow evidence record and reporting path outside FSM_COS. Include repository commit, package/source mode, machine/runtime details, workload identity, phase timings, allocations, high-water values, and failure outcome.

**Boundary:** Do not let metaDev silently rewrite a running manifest. Keep raw observations separate from recommendations and approved published configuration. Treat thresholds such as the documented illustrative 75% pressure threshold as hypotheses until measured.

**Acceptance evidence:** Named workloads produce repeatable raw evidence with provenance; summaries retain raw samples; recommendations link to measurements and require explicit acceptance; FSM_COS remains usable offline without a metaDev dependency.

**Confidence:** Medium. The intended boundary is documented; implementation coverage still needs source inspection.

## 03 — P1: Make repository-backed Experience resolution the shared published-consumer path

**Evidence**

- [AnyAppRuntime.ComposeAsync](https://github.com/TrentBest/AnyApp/blob/development/AnyAppRuntime.cs) supports repository-backed manifests and preloads bundle closure/configuration before invoking FSM_COS.
- [AnyApp README](https://github.com/TrentBest/AnyApp/blob/development/README.md) documents immutable artifact identity → repository → host → same Experience identity, alongside local-development and compatibility paths.
- [WebPage composition catalog](https://github.com/TrentBest/WebPage/blob/development/TheSingularityWorkshop/Workshop/Composition/WorkshopCompositionCatalog.cs) still has a fixed factory inventory in the inspected development source.
- [MicroBundleRepository REST catalog](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleRepository/blob/development/src/REST/TheSingularityWorkshop.MicroBundleRepository.Rest/RestMicroBundleCatalog.cs) already provides a version-aware foundation.

**Opportunity**

Make the repository catalog/provider contract the common published-Experience path for AnyApp and WebPage. Keep compiled factories as explicit local-development/test fixtures, not each host's permanent product inventory. Share artifact identity, closure resolution, and integrity guarantees; keep host-specific caching, credentials, permissions, and materialization policy at the appropriate boundary.

**Acceptance evidence:** A clean WebPage or AnyApp checkout resolves an Experience by exact immutable identity without requiring its bundle implementation to be compiled into the host. Tests cover missing artifacts, version/hash mismatch, cache reuse, and independent runtime instance ownership.

## 04 — P2: Share semantic-intent conformance tests across hosts

**Evidence**

- [AnyApp WorkshopSemanticExchange](https://github.com/TrentBest/AnyApp/blob/development/WorkshopSemanticExchange.cs) composes ProtocolAi vocabulary, GrammarAi structural rules, and FSM_UserIO SemanticIntent, including protocol and grammar admission checks.
- [AnyApp README](https://github.com/TrentBest/AnyApp/blob/development/README.md) explains the ownership boundary. This is real integration, not a missing implementation.

**Opportunity**

Provide shared conformance fixtures for valid symbol resolution, foreign protocol IDs, unknown symbols, structurally disallowed intents, and the distinction between grammar admission and authorization. Each host owns its vocabulary and authorization policy while using the same tested lower-level contracts.

**Acceptance evidence:** Tests run in package and source-integration modes; grammar approval does not imply authorization for privileged actions; a second host can define its own vocabulary without copying AnyApp-specific constants or creating dependencies back to AnyApp.

## 05 — P2: Reuse FSM_Serialization at durable state boundaries

**Evidence**

- [AnyApp README](https://github.com/TrentBest/AnyApp/blob/development/README.md) says its opt-in StoryBuilder native authoring slice persists a versioned binary workspace through FSM_Serialization with a temporary file and atomic replacement.
- The README also says the complete create → link → save → restart → reopen smoke test remains outstanding.
- Architecture maps in WebPage list FSM_Serialization, but a documentation link alone does not prove a particular state boundary uses it.

**Opportunity**

Inventory durable state boundaries across AnyApp, StoryBuilder, WebPage, and Experience tooling. Where the same domain contract crosses a durable byte boundary, reuse FSM_Serialization's supported representation rather than creating a separate format without a deliberate reason. Do not force it onto ephemeral view state or unrelated web DTOs.

**Acceptance evidence:** StoryBuilder passes a restart/reopen round trip, malformed/unsupported input handling, and preservation of the prior save on failure. Each additional consumer is justified by a concrete durable domain boundary, and no serializer is removed until behavior is covered.

## 06 — P2: Turn CiAi's read-only evidence demo into a reusable cross-repository gate

**Evidence**

- [SimpleDemos demonstration gallery](https://github.com/TrentBest/SimpleDemos/blob/development/demonstrations/README.md) describes the CiAi live-run example as read-only cross-repository run evidence and explicitly says it does not prove LLM-driven decisions or production orchestration.
- FSM_COS integration and benchmark goals need named checks and evidence for each source/package mode, not merely a green top-level build.

**Opportunity**

Use CiAi to standardize evidence collection: workflow run, source revision, package mode, named test/benchmark results, and a reviewable evidence bundle across FSM_COS, AnyApp, WebPage, Experiences, and MicroBundleRepository. Expand actions only where current CiAi contracts support them. Keep publish/release behind explicit owner authorization.

**Acceptance evidence:** Failures identify the named test/step and revision; a green workflow is not called end-to-end proof unless the required integration test ran; ordinary CI does not publish NuGet or artifacts without the existing owner-controlled gate.

## 07 — P3: Prove a shared renderer contract with two real consumers

AnyApp already composes semantic GUI content through FSM_COS and GUI.WPF. Drone Construction demonstrates a separate Renderer → Visualize → GUI.SVG route. These are useful foundations, but they do not prove all hosts consume one stable renderer command/semantic contract.

Use one bounded Experience rendered through GUI.WPF and GUI.SVG from the same semantic state/command contract. Compare output-relevant structure and stable entity IDs. Keep platform-specific input, GPU access, and presentation lifecycle in host adapters. Extract a broad shared package only after two real consumers prove the contract.

## 08 — Search discipline and limitations

This is an active source-review addendum, not an exhaustive audit. GitHub code search indexes default branches; branch-specific claims must be confirmed with explicit branch reads. Search results from development and default/master snapshots must not be conflated.

No builds, tests, live endpoints, browser sessions, or publication workflows were executed during this pass. The next review should inspect current Visualize/GUI.SVG trace ingestion, metaDev evidence collection, FSM_Serialization consumers, and CiAi result adapters, then update priorities with precise source/test evidence.


## 09 — P0/P1: Align host execution with Foundry without conflating it with FSM_COS composition

**New source evidence (2026-10-28, explicit `development` reads)**

- [Foundry README](https://github.com/TrentBest/TheSingularityWorkshop.Foundry/blob/development/README.md) says the sequential workflow runner, preflight validation, structured outcomes, and retained in-process lifecycle coordinator are implemented. It explicitly distinguishes Foundry workflow execution from FSM_COS composition and says hosts provide capabilities.
- [Foundry architecture](https://github.com/TrentBest/TheSingularityWorkshop.Foundry/blob/development/docs/ARCHITECTURE.md) says **Foundry ↔ FSM_COS integration is not implemented**, no reusable FSM_API adapter exists, and the in-memory lifecycle does not provide durable history or cross-process transport.
- [WorkflowExecutionCoordinator.cs](https://github.com/TrentBest/TheSingularityWorkshop.Foundry/blob/development/src/Foundry.Workflow/WorkflowExecutionCoordinator.cs) already provides retained submit/start/snapshot/cancel/dispose semantics, and [WorkflowExecutionCoordinatorTests.cs](https://github.com/TrentBest/TheSingularityWorkshop.Foundry/blob/development/tests/Foundry.Workflow.Tests/WorkflowExecutionCoordinatorTests.cs) has source-level tests for accepted-vs-running state, cancellation, invalid workflows, and duplicate starts. This is existing reusable lifecycle infrastructure; do not build a competing host coordinator. Those tests were inspected, not run during this audit.
- [WebApp README](https://github.com/TrentBest/TheSingularityWorkshop.WebApp/blob/development/README.md) explicitly says the current implementation creates a page FSM and 100 actor FSM contexts inside `WebAppExperienceRuntime` and advances them with a private heartbeat. It calls that a transitional prototype, not the target architecture.
- [The / Architect architecture memory](https://github.com/TrentBest/The/blob/development/docs/ARCHITECTURE_MEMORY.md) documents startup orchestration through FSM_API callbacks declared `async void`; completion, failure, and shutdown need an awaited lifecycle contract.
- [MyVR architecture](https://github.com/TrentBest/TheSingularityWorkshop.MyVR/blob/development/docs/ARCHITECTURE.md) correctly assigns VR device/session lifecycle and presentation to MyVR while explicitly excluding Experience composition and canonical artifact identity.

**Opportunity**

Treat Foundry as the common host-neutral execution/lifecycle authority for AnyApp, WebApp, The, and WebPage where the work being performed is a Foundry workflow. Migrate by tracing actual callers and preserving their distinct contracts: FSM_COS composes MicroBundles into a RuntimeAssembly; Foundry executes validated workflows and retains run outcomes; FSM_API remains the state-machine mechanic where directly used; each host owns its UI, device/browser capabilities, permissions, and presentation.

The first concrete duplication to remove is WebApp's private page/actor heartbeat, but only after a tested adapter proves how a composed Experience is submitted to and observed through the canonical execution boundary. The's async startup lifecycle is another candidate for an awaited, observable orchestration contract. Do **not** mechanically wrap every FSM tick as a Foundry workflow step or treat RuntimeAssembly as WorkflowDefinition.

**Acceptance evidence**

- One named scenario is executed through the supported Foundry contract, with status, cancellation/failure, and terminal outcome observable.
- WebApp no longer advances a second private Experience lifecycle; its browser inputs and presentation use a tested host adapter.
- AnyApp, WebPage, and The have explicit caller/adapter maps showing which work belongs to Foundry versus FSM_COS/FSM_API.
- MyVR remains a capability-bearing manifestation and only delegates workflows when its Experience contract calls for it.
- Tests demonstrate failure/cancellation/cleanup and prevent duplicate execution. No package publication is part of this work.

**Confidence:** High that integration work remains: Foundry's own architecture says FSM_COS integration and the reusable FSM_API adapter are not implemented; WebApp documents its private heartbeat as transitional. The exact minimal adapter is still an architectural question, not yet a proven implementation.

## 10 — P2: Keep DataWarehouse and MicroBundle ingestion on their proper boundaries, then prove the useful bridge

**Evidence**

- [DataWarehouse README](https://github.com/TrentBest/TheSingularityWorkshop.DataWarehouse/blob/development/README.md) describes a platform-neutral unmanaged fixed-size record store with generation-checked handles, dirty-slot enumeration, disposal, and tests. It explicitly does not promise persistence or serialization.
- [MicroBundleDomain README](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleDomain/blob/development/README.md) defines neutral capability contracts and leaves artifact delivery to MicroBundleRepository and composition to FSM_COS.
- [MicroBundleIngestor README](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleIngestor/blob/development/README.md) separates external API ingestion, capability definition, and runtime invocation. Its repository capability includes Get/Put, while publication authority is intended to be Forge-only and must be enforced by the repository host.
- [DataWarehouse benchmark laboratory](https://github.com/TrentBest/TheSingularityWorkshop.Benchmarks.DataWarehouse) is a separate performance project, appropriately distinct from the storage implementation.

**Opportunity**

Do not add dependencies from DataWarehouse into FSM_COS, MicroBundleDomain, or MicroBundleIngestor just because these are all foundational projects. Instead, identify a real high-volume consumer—such as a simulation or telemetry path—and compare the DataWarehouse-backed representation against the existing representation with repeatable workload evidence. Separately, ensure MicroBundleIngestor-generated capability metadata conforms to MicroBundleDomain contracts and that Forge-only Put authority is enforced by repository-side authorization, not merely described in a generated bundle.

**Acceptance evidence:** A named consumer and workload demonstrate correctness and measured benefit before adopting DataWarehouse; ingestion emits contract-valid artifacts; repository tests prove unauthorized publication is rejected. Until such source/tests are inspected, this is a **candidate integration**, not a confirmed missing implementation.

## 12 — P2: Turn GUI adapter diversity into a shared, executable conformance contract

**Evidence (current `development` README)**

- [GUI README](https://github.com/TrentBest/TheSingularityWorkshop.GUI/blob/development/README.md) confirms Core plus Blazor, WPF, and SVG adapters exist.
- The same README explicitly says adapter contracts and coverage differ and the shared conformance suite is intentionally narrow.
- [Visualize](https://github.com/TrentBest/TheSingularityWorkshop.Visualize) and SimpleDemos already provide trace/replay use cases; the previous sections identify GUI.SVG as one endpoint and AnyApp/GUI.WPF as another.

**Opportunity**

Before broadening GUI's public abstraction, define a small set of semantic scene/node fixtures that every applicable adapter must handle, with explicit unsupported-node behavior. Run the same fixtures through SVG and WPF first, then Blazor where its current contract applies. Compare semantic output and stable identities, not pixel equality where renderers intentionally differ. Keep adapter-specific capabilities explicit; do not force SVG animation, browser behavior, and WPF control lifecycles into a false common denominator.

**Acceptance evidence:** The same named fixture suite executes against at least two adapters; supported and unsupported nodes are reported explicitly; output semantics and IDs are stable; tests do not claim visual parity they do not measure.

**Confidence:** High that conformance coverage is intentionally narrow (the README states so); the exact missing fixture cases require test-source inspection.

## 14 — P1: Close StoryBuilder → Experience → manifest → host as one verified vertical slice

**Evidence (current `development` README, not a guessed missing feature)**

- [StoryBuilder README](https://github.com/TrentBest/StoryBuilder/blob/development/README.md) says its MicroBundle exposes authoring capabilities through MicroBundleDomain and an automated test composes it through FSM_COS. It also documents versioned FSM_Serialization persistence, browser-local autosave, and a live GUI.SVG map.
- [Experiences README](https://github.com/TrentBest/TheSingularityWorkshop.Experiences/blob/development/README.md) explicitly says the StoryBuilder launch in AnyApp is an early authoring integration proof, **not** proof that the complete package-to-manifest-to-FSM_COS path is finished. It links a vertical-slice document describing gaps between authored package, manifest, artifact resolution, composition, and native presentation.
- StoryBuilder's own README says embedding its Experience into WebPage remains open and that the browser demo is local-only (no cloud sync or multi-user sharing).

**Disposition:** This is a confirmed integration gap, not a missing persistence or missing composition feature. Do not rewrite those parts. The next valuable test is one clean end-to-end path that starts from a real authored workspace, produces a validated package/immutable artifacts, publishes or resolves its manifest, composes through FSM_COS, and presents the same Experience in AnyApp and then WebPage. Preserve local-only behavior as an honest demo limitation until remote storage is actually implemented.

**Acceptance evidence:** one authored fixture with stable Experience/bundle IDs and hashes; round-trip persistence; manifest resolution from a clean cache; FSM_COS composition; host-visible result; failures at every boundary produce actionable outcomes and leave no partial publication.

## 15 — P2: Validate reuse of FSM_REST and HeadlessAi at real consumer boundaries

**Evidence**

- [FSM_REST README](https://github.com/TrentBest/TheSingularityWorkshop.FSM_REST/blob/development/README.md) clearly defines a reusable REST capability/transport substrate, with opt-in FSM-backed request lifecycle and domain MicroBundles owning API meaning. It includes deterministic in-memory transport examples and a storefront comparison sample that defaults to test responses.
- [HeadlessAi README](https://github.com/TrentBest/TheSingularityWorkshop.HeadlessAi/blob/development/README.md) documents explicit raw-text, delegate, JSON, OpenAI Responses, Gemini, and Anthropic protocol adapters, while assigning intent, permissions, memory, validation, and consequential actions to the host.
- These documented capabilities are useful reuse surfaces; this review has **not yet proven** that every eligible Workshop consumer uses them rather than private HTTP/protocol code.

**Opportunity**

Search actual HTTP call sites in MicroBundleIngestor, StoryBuilder, AnyApp, WebPage, and the higher-level domain projects. Reuse FSM_REST only where the operation is naturally a domain capability with an explicit transport boundary; reuse HeadlessAi only for compatible endpoint protocols. Keep authorization, secret ownership, domain semantics, and approval of actions in the host. Avoid adding a package dependency merely to replace a tiny, appropriately isolated call.

**Acceptance evidence:** consumer inventory identifies each outbound HTTP path, its owner, protocol and failure behavior; each reuse decision is recorded as adopted, intentionally local, or blocked by a missing contract; tests cover timeout, malformed response, cancellation, and unauthorized operation where applicable.

## 16 — P2: Preserve the Revit production verification boundary while auditing builder reuse

**Evidence**

- [RevitFamilyManagerBuilders README](https://github.com/TrentBest/RevitFamilyManagerBuilders/blob/development/README.md) describes a migration to a declarative Composable Builders Engine and states that live catalog synchronization is not production-verified until transaction, regeneration, reread, and final model evidence exist.
- The README also calls its legacy WPF UI project a ghost, so the old WPF project must not be assumed to be the current implementation path.

**Opportunity**

Audit whether the newer Workshop GUI/Composable Builders contracts can replace duplicated builder primitives in the active Revit path, but only after mapping actual project references and the Revit thread/transaction boundary. Do not mark catalog sync production-ready from a successful compile or simulated test.

**Acceptance evidence:** actual Revit-hosted validation includes transaction success/failure, regeneration, reread of persisted model state, and a saved final model artifact. If no live Revit host is available, record that limitation rather than claiming production verification.

## 13 — P2: Treat CiAi evidence policy as reusable infrastructure, not merely a demo

**Additional evidence**

- [CiAi README](https://github.com/TrentBest/TheSingularityWorkshop.CiAi/blob/development/README.md) documents observing workflow runs by full commit SHA, collecting exact run attempts/jobs/steps, and writing JSON/Markdown evidence.
- Its documented policy fails closed when required evidence is missing, duplicated, non-terminal, unsuccessful, or incomplete; unrelated runs can remain diagnostic rather than blocking.
- The README explicitly says CiAi exposes CI facts to an LLM; it is not an AI-powered CI runner.

**Opportunity**

Reuse this strict evidence model across each repository's named tests and benchmarks, retaining source revision, exact workflow/run attempt, package-versus-source mode, and benchmark artifact identity. Keep decision and execution authority outside model-authored text. This complements (rather than replaces) each repository's tests and release gates.

**Acceptance evidence:** A cross-repository report can show which exact required workflows ran for a commit and which required checks are absent; failed or incomplete evidence cannot be summarized as green; NuGet publication remains separately owner-authorized.

**Confidence:** High for the evidence observer and fail-closed policy documented in CiAi; cross-repository rollout remains unverified.

## 11 — Audit coverage ledger

The broader 2026 repository scope and review status are maintained in [ECOSYSTEM_AUDIT_SCOPE_2026.md](ECOSYSTEM_AUDIT_SCOPE_2026.md). A repository listed there is not considered fully audited merely because its README was read. Each entry must progress from inventory to source review, then tests/workflows and cross-repository contract verification.


## 17 — P1: WebApp has a documented canonical architecture but still runs a parallel lifecycle

**Direct development-branch evidence**

- [WebApp README](https://github.com/TrentBest/TheSingularityWorkshop.WebApp/blob/development/README.md) says WebApp is “AnyApp for the web,” not another application runtime, and explicitly labels its gateway → Moniker → hub slice transitional.
- [WebApp architecture](https://github.com/TrentBest/TheSingularityWorkshop.WebApp/blob/development/docs/ARCHITECTURE.md) says the host should translate browser events to application requests, compose the selected manifest through FSM_COS, and progress the Experience through the canonical execution owner. It explicitly states that successful FSM_COS composition alone does not prove the Experience executes.
- [WebAppCosRuntime.cs](https://github.com/TrentBest/TheSingularityWorkshop.WebApp/blob/development/Infrastructure/WebAppCosRuntime.cs) composes a local Moniker bundle. Separately, [WebAppExperienceRuntime.cs](https://github.com/TrentBest/TheSingularityWorkshop.WebApp/blob/development/Infrastructure/WebAppExperienceRuntime.cs) creates page/actor FSMs and a private background heartbeat.
- [WebAppCompositionTests.cs](https://github.com/TrentBest/TheSingularityWorkshop.WebApp/blob/development/tests/TheSingularityWorkshop.WebApp.Tests/WebAppCompositionTests.cs) tests that local Moniker composition works and that the transitional actor runtime starts. These tests do not prove a published manifest → repository artifact → FSM_COS composition → canonical execution → browser result path.
- [AnyAppRuntime.cs](https://github.com/TrentBest/AnyApp/blob/development/AnyAppRuntime.cs) provides a concrete sibling-host composition path, including repository-backed closure preload and configuration resolution, but the AnyApp project is a Windows/WPF executable. WebApp architecture correctly warns against taking a direct project dependency on that desktop executable.

**Decision:** This is a real integration gap already acknowledged by WebApp's own documentation, not a newly discovered requirement. The next step is to extract/reuse only the host-neutral request/manifest/result contract that is actually needed, connect web input to artifact resolution and FSM_COS, then use Foundry for workflow-owned execution/lifecycle where appropriate. Do not make WebApp depend on WPF, do not equate RuntimeAssembly with WorkflowDefinition, and do not remove the private heartbeat until parity and lifecycle tests prove the replacement.

**Acceptance evidence:** a deterministic host-level test submits an Experience request, validates stable artifact identity, resolves a clean-cache manifest, composes through FSM_COS, starts exactly one canonical execution path, observes browser-facing state, and verifies cancellation/failure/disposal without duplicate ticks or leaked background work.

**Confidence:** High that the current implementation is transitional and does not yet prove the intended end-to-end path; migration design remains subject to current Foundry/FSM_COS contracts and existing behavior-preservation tests.

## 18 — P1: The Architect's async startup callbacks need an awaited lifecycle contract

**Direct development-branch evidence**

- [ArchitectFSM.cs](https://github.com/TrentBest/The/blob/development/Architect/ArchitectFSM.cs) declares its OnEnterFetchingStatus callback async void; startup also has a separate FetchingRepositories state.
- [ARCHITECTURE_MEMORY.md](https://github.com/TrentBest/The/blob/development/docs/ARCHITECTURE_MEMORY.md) already identifies async-void startup callbacks as a concern: completion, failure, and shutdown need deterministic tests and an awaited lifecycle contract.
- [StartupLifecyclePolicyTests.cs](https://github.com/TrentBest/The/blob/development/Architect.Tests/StartupLifecyclePolicyTests.cs) tests pure transition/degraded-status rules, but these policy tests alone do not prove asynchronous provider work is awaited, cancelled, or joined during shutdown.

**Opportunity:** use the same explicit operation lifecycle vocabulary as Foundry where startup work genuinely qualifies as a workflow, or introduce a narrow awaited operation abstraction if a full workflow is unnecessary. Do not put WPF view models or GitHub credentials into Foundry. Keep the Architect's presentation and local credential boundary in The.

**Acceptance evidence:** controlled fake providers prove success, failure, timeout/cancellation, no premature Ready state, no unobserved async exceptions, and shutdown waits for owned operations. Run these as deterministic tests; do not claim this behavior is fixed from the pure policy tests alone.

**Confidence:** High for the async-void callback and the limited scope of existing policy tests; runtime failure modes have not been executed in this audit.


## 19 — P2: FSM_PackageManager advertises a runtime manager but its inspected implementation is a placeholder

**Direct development-branch evidence**

- [FSM_PackageManager.csproj](https://github.com/TrentBest/FSM_PackageManager/blob/development/FSM_PackageManager/FSM_PackageManager.csproj) describes a package that manages runtime package loading, dynamic FSM definitions, and package-to-package arbitration, and references FSM_API.
- Its only inspected implementation file, [Class1.cs](https://github.com/TrentBest/FSM_PackageManager/blob/development/FSM_PackageManager/Class1.cs), contains an empty `Class1` type. The README is branding/documentation-standard content rather than an operational contract.

**Disposition:** The project metadata and implementation do not currently establish a reusable package-management capability. Do not wire it into FSM_COS, Foundry, or MicroBundleRepository on the strength of its package description. First determine whether it is intentionally abandoned, a scaffold, or a package whose implementation lives elsewhere. If its intended responsibility is still needed, define a minimal executable contract and tests before any consumer adopts it.

**Acceptance evidence:** documented scope and supported frameworks match public types; tests demonstrate the claimed load/resolve/arbitrate behavior; consumer and dependency direction are explicit; package publication remains separately authorized.

**Confidence:** High for the inspected development-branch placeholder; historical release contents and consumer usage were not exhaustively audited.

## 20 — P2: Treat TheForge as an architectural predecessor, not a source of drop-in implementations

**Direct development-branch evidence**

- [TheForge IMicroPackage](https://github.com/TrentBest/TheForge/blob/development/Assets/Scripts/MicroPackages/IMicroPackage.cs) defines Unity-message-associated process groups and package load/arbitrate callbacks.
- [TheForge IPackageArbitrator](https://github.com/TrentBest/TheForge/blob/development/Assets/Scripts/MicroPackages/IPackageArbitrator.cs) contains package names, process-group mappings, arbitration mutations, and provider registration—concepts that overlap at a high level with current MicroBundleDomain/FSM_COS composition and arbitration.
- [TheForge DataWarehouse](https://github.com/TrentBest/TheForge/blob/development/Assets/Scripts/Memory/DataWarehouse.cs) directly uses Unity Collections and unsafe raw memory. Its shown Dispose path frees the raw shelf but leaves NativeArray disposal commented out; this file is not interchangeable with the current platform-neutral DataWarehouse package.
- [TheForge GraphicalUserInterfaceBuilder](https://github.com/TrentBest/TheForge/blob/development/Assets/Scripts/Builders/GuiBuilders/GraphicalUserInterfaceBuilder.cs) builds Unity UIElements and owns Unity-specific styling/layout state; it is not a replacement for GUI.Core semantic contracts or GUI.SVG/WPF/Blazor adapters.

**Opportunity:** Map the valuable *concepts* from this older editor/tooling architecture to current owners: FSM_API for state-machine mechanics, MicroBundleDomain for capability contracts, FSM_COS for composition, GUI.Core for semantic UI, host adapters for Unity/WPF/Blazor/SVG presentation, and DataWarehouse for its own bounded memory-store contract. Only migrate a behavior after a concrete current consumer and parity tests exist. Do not copy Unity types, unsafe memory routines, or historical package-arbitration code into neutral packages.

**Acceptance evidence:** a concept-to-current-owner map distinguishes retained behavior, replaced behavior, and intentionally retired behavior; each migrated behavior has a consumer test; unsafe allocation/disposal receives dedicated tests before any reuse.

**Confidence:** High for the inspected files' ownership and technology dependencies; a complete TheForge source audit has not been performed.


## 21 — P2: Refresh project templates against the current FSM_API contract before treating them as canonical examples

**Direct development-branch evidence**

- [FSM_ProjectTemplates README](https://github.com/TrentBest/FSM_ProjectTemplates/blob/development/README.md) describes the repository as a starting point for FSM_API consumers and currently lists a single WPF Minesweeper template.
- [FSM_WPF.csproj](https://github.com/TrentBest/FSM_ProjectTemplates/blob/development/FSM_WPF/FSM_WPF.csproj) pins FSM_API 1.0.9, while the current Workshop runtime integration work references FSM_API 1.0.13. This is version skew in the template source, not evidence by itself that the sample is broken.
- [FSM_Layer_DependencyInjection.cs](https://github.com/TrentBest/FSM_Layer_DependencyInjection/blob/development/FSM_Layer_DependencyInjection/FSM_Layer_DependencyInjection.cs) is an empty placeholder class, and its project file currently declares only a .NET 8 target with no package or dependency-injection contract.

**Opportunity:** treat templates and layer adapters as tested integration examples. Update the template only after compiling and testing it against the current supported FSM_API version, and add a new host template only when a real reusable pattern exists. Do not promote FSM_Layer_DependencyInjection as an available integration until its public contract and tests exist.

**Acceptance evidence:** clean restore/build/test of the template against the intended FSM_API version; README accurately lists what is implemented; dependency-injection adapter, if retained, has a minimal public contract and consumer tests.

**Confidence:** High for the explicit version pin and placeholder class; template execution and package compatibility were not tested during this audit.


## 22 — P1: Make WebPage manifest activation atomic across Hub and Experience execution

**Direct development-branch evidence**

- [WorkshopExperienceService.SwitchManifestAsync](https://github.com/TrentBest/WebPage/blob/development/TheSingularityWorkshop/Services/WorkshopExperienceService.cs) prepares a candidate FSM_COS assembly and candidate Hub/capability assemblies, then saves the previous primary assembly and Hub bundles before committing activation.
- The activation sequence calls `_hubRuntime.ActivateManifestBundle(candidateHubBundle)` and then `_executionHost.ActivateExecutionRuntime(candidateExecutionRuntime)`. The `catch` path disposes candidate runtime objects when `activated` is still false, but it does not restore the previous Hub manifest bundle set if a later operation in that commit sequence fails.
- [HubRuntime.ActivateManifestBundle](https://github.com/TrentBest/WebPage/blob/development/TheSingularityWorkshop/Infrastructure/Hub/HubRuntime.cs) has rollback for failures inside its own load/arbitration operation. That local rollback does not cover a failure after it returns to the caller.
- [FSMManagerService.ActivateExecutionRuntime](https://github.com/TrentBest/WebPage/blob/development/TheSingularityWorkshop/Services/FSMManagerService.cs) currently validates the prepared adapter and writes it atomically, so the cross-step failure window is narrow today. The surrounding code nevertheless presents manifest switching as an atomic activation and captures old Hub state without using it for rollback.

**Opportunity:** make the entire manifest activation commit either succeed as a unit or restore the previous Hub bundle set and execution adapter. At minimum, make the commit sequence explicitly non-throwing after Hub mutation, or retain/restore the old execution adapter and Hub bundle state in a guarded rollback. Do not dispose old runtime objects until all state owners have committed successfully.

**Acceptance evidence:** inject a failure after Hub activation but before activation completion; assert the prior Hub bundle identities, execution adapter, manifest, assembly references, and observable runtime state remain active. Also verify a successful switch retires old disposable objects exactly once.

**Confidence:** Medium-high. The source sequence and missing outer rollback are visible; current adapter activation has few failure modes, so tests should establish whether this is an actual reachable defect before a large refactor.

## 23 — P2: Update persistence audit claims to reflect existing restart and round-trip tests

**Direct development-branch evidence**

- [AnyApp StoryBuilderDesktopPersistenceStoreTests](https://github.com/TrentBest/AnyApp/blob/development/tests/TheSingularityWorkshop.AnyApp.Tests/StoryBuilderDesktopPersistenceStoreTests.cs) includes stable-ID and event-to-manuscript-link round-trip coverage, byte-identical re-save coverage, unreadable-save preservation, and a native application process-restart probe that writes in one process and verifies in a second.
- [StoryBuilderPersistenceCoordinatorTests](https://github.com/TrentBest/StoryBuilder/blob/development/tests/StoryBuilder.Core.Tests/StoryBuilderPersistenceCoordinatorTests.cs) verifies binary workspace save/load into a new session through a fake `IJSRuntime`; this proves coordinator behavior, not a real browser reload or browser storage failure behavior.
- [StoryBuilderPersistenceCoordinator](https://github.com/TrentBest/StoryBuilder/blob/development/samples/StoryBuilder.WebDemo/StoryBuilderPersistenceCoordinator.cs) uses browser-local storage and preserves an unreadable saved value until the user explicitly chooses to clear it.

**Correction to the audit status:** it is no longer accurate to say that *all* create/link/save/restart/reopen coverage is simply outstanding. The AnyApp development branch contains a process-restart probe in source, although it has not been executed as part of this audit. Remaining proof includes running the native probe in CI and exercising the real browser storage/reload path; the fake-JS StoryBuilder test alone does not prove those browser behaviors.

**Acceptance evidence:** record the actual CI result for the AnyApp process-restart test; add or document a browser-level reload test for StoryBuilder's localStorage path; preserve malformed/unsupported input and explicit-reset behavior. Do not duplicate the already-existing persistence implementation merely to satisfy the audit.

**Confidence:** High for the presence and stated scope of the inspected tests; execution status is unverified in this audit.


---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>