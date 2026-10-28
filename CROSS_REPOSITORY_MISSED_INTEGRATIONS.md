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

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop" width="200" /></p>
