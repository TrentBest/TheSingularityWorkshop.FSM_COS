# Dependency Alignment Checklist

> **Status:** audit worklist based on source metadata inspected on 2026-10-08. This is not a release plan, merge approval, or claim that a package version is published. Confirm each item against the intended branch, package feed, and CI before changing references.
>
> **Reset / continuation brief:** use [CONTINUATION_BRIEF_ALPHA6_FORGE_ANYAPP_WEBPAGE.md](CONTINUATION_BRIEF_ALPHA6_FORGE_ANYAPP_WEBPAGE.md) for the current ordered TODOs, alpha.6 release gate, Forge/AnyApp/WebPage migration sequence, architecture boundaries, guardrails, and resume instructions. Re-check current branch heads and CI before relying on snapshot details.

## Guardrails

- Do not publish any NuGet package without explicit approval.
- Keep package-publishing workflows disabled by default with the publish condition set to && false.
- Do not merge open PRs merely to simplify the audit; reconcile their branch-specific source and documentation first.
- Treat project-file versions, published package versions, and consumer package pins as three distinct facts.
- Change dependency versions only with a build/test result demonstrating compatibility.

## Ordered worklist

### 1. Resolve known source-version drift

| Consumer / project | Current source finding | Next verification |
|---|---|---|
| AnyApp (development) | The active development project declares AnyApp alpha.1 and references FSM_COS alpha.5, GUI.WPF alpha.6, FSM_UserIO alpha.1, ProtocolAi alpha.2, GrammarAi alpha.2, and MicroBundleRepository.Rest alpha.5. The inspected FSM_COS development source declares alpha.6. | Verify alpha.5 package availability and API compatibility before changing the pin. Run the Windows restore/build/test path. The development README describes a repository-backed artifact path alongside a compiled compatibility catalog; confirm which path is exercised by tests. |
| MicroBundleRepository.FSM_COS | Adapter references FSM_COS alpha.5 while inspected FSM_COS source declares alpha.6. | Confirm whether alpha.5 is available and intentional; build the adapter against the selected compatible package/source. |
| MicroBundleRepository.Core | References FSM_Serialization alpha.2 and Ontology alpha.2; inspected source declarations were Serialization 1.0.0 and Ontology alpha.3. | Check NuGet availability and API/format compatibility; update pins only after validation. |
| MicroBundleRepository.Rest | References Ontology alpha.2 while inspected Ontology source declares alpha.3. Its FSM_REST alpha.4 reference matches inspected source metadata. | Verify whether alpha.2 is intentionally pinned for compatibility or simply stale. |
| FSM_COS | Development preserves the versioned `MicroBundleManifestEntry` roots and external configuration source; it now also carries optional `FSM_UserIO.SemanticIntent` through `RuntimeManifest` to `RuntimeAssembly`, with a direct FSM_UserIO alpha.1 dependency. The package workflow checks out MicroBundleDomain branch `architecture/runtime-contract-ownership` and packs a local dependency for CI. | Verify intent compatibility in CI and confirm the CI-only MicroBundleDomain contract matches published 1.0.1 used by consumers. A clean restore against public package versions is still required before release readiness. |
| Experiences.Moniker | References GUI.Core alpha.4 and MicroBundleDomain 1.0.1. | Determine whether its experience contract can be host-neutral and presentation loaded separately; avoid breaking current hosts without a proven replacement. |
| Renderer | References FSM_API 1.0.13, ProtocolAi alpha.2, and MicroBundleDomain 1.0.1. | Trace ProtocolAi usage and decide whether it is an essential renderer contract or an optional semantic adapter. |

### 2. Verify optionality and actual runtime discovery

- [ ] Prove that an Elements experience can be discovered, loaded, and arbitrated by a real FSM_COS catalog.
- [ ] Verify AnyApp’s actual repository-backed composition path, including artifact identity/hash verification and materialization, rather than treating a repository reference as proof of runtime use.
- [ ] Prove the Moniker experience can be consumed consistently by WebPage and AnyApp, or document the current host-specific limitation.
- [ ] Confirm MicroBundleIngestor remains an authoring/ingestion tool, not a runtime dependency. Its repository REST dependency is conditional on UseMicroBundleRepositoryRest=true and disabled by default in the inspected development source.
- [ ] Keep REST, storage, Azure, WPF, and other host/platform adapters optional unless a consumer demonstrably requires them.
- [ ] Verify package dependency closure for a minimal host versus a host that enables repository REST, GUI, rendering, ontology, or Profiles capabilities.

### 3. Integrate Profiles deliberately

The inspected Profiles development branch contains a .NET 8 domain package, tests, and privacy/architecture/API documentation. Its current project does not reference MicroBundleDomain, so it is not yet an FSM_COS MicroBundle. The source includes stable profile identity, attribute-level disclosure, groups/relationships, observer-specific representations, access records, and licensing policy; persistence, transport, rendering, and authentication-provider duties are stated non-goals.

- [ ] Preserve Profiles as a provider-neutral domain package.
- [ ] Resolve the identity/provider-link boundary tracked in [Profiles issue #2](https://github.com/TrentBest/Profiles/issues/2).
- [ ] If FSM_COS should select profile operations at runtime, add a separate optional adapter/MicroBundle with explicit contracts and authorization behavior.
- [ ] Test manifest-driven discovery, arbitration, and host behavior before describing Profiles as integrated.
- [ ] Preserve the current open [Profiles PR #1](https://github.com/TrentBest/Profiles/pull/1) branch distinction; do not claim the development implementation is already on master.

### 4. Branch and documentation reconciliation

- [ ] MicroBundleRepository master and development are materially diverged: the comparison reports 29 commits ahead on master and 22 commits behind relative to development, with changes to artifact payloads, the FSM_COS adapter, REST behavior, tests, and package references. Do not treat the older development project files as the current architecture or blindly merge one branch into the other. Reconcile the intended integration line and open PR bases before making dependency-version edits.
- [ ] Reconcile FSM_COS README changes with open PR #22 (base development), PR #23 (base master), and draft PR #14 before standardizing README headings.
- [ ] Apply the shared numbered topic taxonomy repository-by-repository, retaining project-specific content and the stable semantic/color mapping.
- [ ] Ensure docs distinguish implemented behavior, intended architecture, and future work.
- [ ] Audit links after each batch and record the exact branch/commit reviewed.

## Release-safety checkpoint

- [x] FSM_COS development workflow: .github/workflows/package.yml now includes an explicit && false in its publish job condition; the inspected master workflow already had this safeguard.
- [x] FSM_Serialization master, development, hardening/manual-nuget-publication, chore/standardize-ci-fsm-serialization, docs/benchmarking, and docs/readme-standard workflows all now have an explicit && false publish safeguard. This includes the active release-hardening branch and documentation branches that could otherwise carry an old workflow forward.
- [x] Ontology master, development, chore/standardize-ci-ontology, and docs/readme-standard workflows all now have an explicit && false publish safeguard.
- [x] FSM_API: added && false to stale feature/legacy branch publish jobs in feature/fsm-api-2.0, feature/fsm-api-2.0-work, feature/fsm-api-2.0-next, feature/fsm-api-2.0-registry, RefactoringToHashBacking, HireMeFSMDemo, and docs/benchmarking. Disabled the two legacy automatic downstream repository-push jobs in HireMeFSMDemo/WindowsFormsEditor. Other inspected FSM_API branches either already had the safeguard or had the publish job commented out.
- [x] MicroBundleRepository: added && false to the development and docs/readme-standard package workflows; master and the active feature/microbundle-publisher branch were already gated.
- [x] MicroBundleDomain: added && false to the development workflow; master was already gated.
- [x] GUI: disabled automatic publication from the development package workflow to GitHub Packages. Master’s NuGet.org package jobs were already gated.
- [x] Verified explicit publish safeguards in the inspected master workflows for FSM_UserIO, FSM_REST, ProtocolAi, GrammarAi, and Renderer.
- [ ] Finish checking any additional package-producing workflow paths and branch variants; repository-wide search is only a discovery aid, not proof every workflow is safe.

No NuGet publication was performed. These workflow-only edits have not been represented as passing CI unless a corresponding run confirms it.

## Branch reconciliation snapshot (2026-10-08)

These counts compare master (base) with development (head). They are repository-history facts, not a merge recommendation. A divergence must be understood before using one branch's project files as the authority for another.

| Repository | Comparison status | Development ahead / behind | Action |
|---|---|---:|---|
| FSM_API | Identical | 0 / 0 | The 2.0 feature branches remain separate; do not merge them into master until parity and tests are approved. |
| FSM_Serialization | Diverged | 1 / 1 | Review the workflow-only safety differences and preserve disabled publication on both branches. |
| Ontology | Diverged | 1 / 1 | Review the workflow-only safety differences and preserve disabled publication on both branches. |
| MicroBundleDomain | Diverged | 1 / 2 | Reconcile current package/contract changes and keep publishing disabled. |
| FSM_UserIO | Diverged | 12 / 19 | Inspect source/API changes before choosing the canonical integration line. |
| ProtocolAi | Development ahead | 6 / 0 | Review development changes and the open alpha.3 PR before pinning consumers. |
| GrammarAi | Identical | 0 / 0 | Branch histories align; keep the current publish gate disabled. |
| Renderer | Development ahead | 5 / 0 | Review renderer API/dependency changes before consumer version updates. |
| FSM_REST | Diverged | 24 / 4 | Reconcile the transport/API branch split before updating repository REST pins. |
| MicroBundleRepository | Diverged | 29 / 22 | Resolve the major source and adapter divergence before dependency alignment. |

## Completion criteria

A row is complete only when its intended version is identified, the package/source compatibility is demonstrated by restore/build/tests, the consumer’s actual usage is understood, and any release action has explicit approval. Documentation alone does not make a dependency aligned or a capability integrated.


### 5. Architecture disposition: MicroBundleRepository divergence (2026-10-08)

**Preliminary recommendation: preserve the useful work from both lines; do not wholesale-select or merge either branch yet.** The source tree comparison shows that these are not simply old/new copies. The development line adds a local filesystem implementation, REST host/client, CLI, and repository observer/observation DTOs. The master line has an FSM_COS materialization/catalog adapter, an Azure experience catalog/repository, and an assembly-payload/envelope path. Several pieces are complementary and should be reconciled as separate responsibilities.

| Area | Master line observed | Development line observed | Disposition |
|---|---|---|---|
| Core artifact contract | Core alpha.3 references FSM_Serialization alpha.2 and Ontology alpha.2; includes assembly payload/envelope-related artifacts. | Core alpha.1 has no package references and includes observer/observation contracts. | **Prefer a dependency-light Core as the target.** Repository Core should identify, store, retrieve, and verify opaque artifacts; serialization/materialization belongs in an explicit producer/consumer adapter. Assess whether experience catalog contracts truly belong in Core or are a separate optional catalog capability. |
| Azure storage | Azure implementation alpha.1 with Core project reference and Azure SDK dependencies. | Same alpha.1 and same project/package references in the inspected project file. | Keep; implementation is shared and should be tested against the chosen Core contract. |
| Local storage | No Local implementation in the inspected master tree. | File-system repository implementation exists. | Preserve as an optional implementation and test provider. It enables local development and low-cost testing without Azure; it must not become a dependency of Core. |
| REST transport | REST alpha.5 uses FSM_REST alpha.4 and Ontology alpha.2; has a separate FSM_COS adapter project. | REST alpha.2 uses FSM_REST alpha.4 and FSM_COS alpha.3; REST host and observer support are present, but no separate FSM_COS adapter project in the inspected development tree. | Keep REST optional. Align its DTO/transport contract with Core, remove any unnecessary ontology dependency, and keep FSM_COS materialization in the dedicated adapter—not in the REST transport package. Validate against current FSM_COS source alpha.6 before changing pins. |
| FSM_COS materialization | Dedicated `MicroBundleRepository.FSM_COS` alpha.1 adapter references FSM_COS alpha.5, MicroBundleDomain 1.0.1, and REST via project reference. | No matching adapter project in the inspected development tree. | Preserve the adapter concept, update only after compatibility tests. Its dependency on REST may be too strong: prefer an adapter over `IMicroBundleRepository`/artifact contracts so local, REST, or Azure-backed implementations can be selected without coupling composition to one transport. |
| Operational tooling | No CLI project in the inspected master tree. | CLI executable exists and uses the REST boundary. | Preserve as an operational tool, not a runtime NuGet dependency or MicroBundle. Keep it independently deployable and ensure publishing stays disabled. |
| Tests / verification | Master has Core, Azure, REST-related tests in its tree. | Development adds/retains Core, Azure, REST and related tests alongside its extra implementations. | Reconcile test coverage by contract and behavior, not by copying a branch wholesale. Add shared contract tests for every repository implementation and integration tests for artifact retrieval/materialization. |

#### Target project boundaries

```text
MicroBundleRepository.Core
  - artifact address, immutable artifact, repository contract
  - hash/content identity and provider-neutral metadata
  - no Azure, REST, FSM_COS, Ontology, GUI, or storage SDK dependency
             ^
             | implemented by
    +--------+---------+
    |        |         |
   Local    Azure     REST client/server adapter
                       |
                       v
             REST host / operational CLI

FSM_COS MicroBundleRepository adapter
  - depends on Core contracts
  - turns retrieved artifact bytes into a validated IMicroBundle
  - plugs into catalog/composition APIs
  - should not require REST when another repository provider is selected
```

This is a target boundary to validate, not a claim that all projects currently satisfy it.

#### NuGet versus MicroBundle disposition

- **Keep as NuGet/infrastructure packages:** repository Core contracts and independently reusable Local/Azure/REST provider adapters; the CLI remains an executable tool. These are delivery mechanisms and developer operations, not user-selectable domain behaviors.
- **Keep as an optional integration package:** the FSM_COS materializer/catalog adapter while its APIs are compiled against the runtime contracts. It should be installed only by hosts that compose repository-backed MicroBundles.
- **Generate MicroBundles for domain capabilities, not storage infrastructure:** experiences such as Moniker or Elements are better candidates for independently loadable MicroBundles. Their build/publish pipeline may use NuGet and serialization tooling, but the consumer should load the resulting bundle artifact through the repository and FSM_COS rather than carry a separate NuGet dependency for every experience.
- **Do not turn every NuGet package into a MicroBundle:** FSM_API, serialization primitives, repository contracts, and host-specific adapters provide compile-time/runtime infrastructure and should remain packages unless a concrete measured use case proves otherwise.
- **Avoid accidental dependency closure:** a minimal host should not pull in Azure SDKs, REST hosting, CLI, WPF, or unrelated domain capabilities merely to use FSM_COS or a local artifact repository.

#### Required verification before choosing a canonical branch

- [ ] Compare source/API differences in Core artifact types, hash verification, immutability, observer contracts, and experience catalog contracts.
- [ ] Build and run tests for both branch snapshots; no CI result is implied by this source inspection.
- [ ] Add/verify shared repository contract tests for Local and Azure, plus REST integration tests.
- [ ] Prove exact artifact bytes/hash survive put/get and that the FSM_COS adapter materializes the requested bundle ID.
- [ ] Verify the adapter can consume the Core abstraction without requiring REST specifically.
- [ ] Decide which branch becomes the integration base only after these checks; migrate complementary features in small commits and retain the other branch until parity is demonstrated.
- [ ] Keep all package publishing disabled. No dependency pins or package versions should be changed solely to make the branches look aligned.


#### FSM_COS source-difference finding: intent contract

A direct fetch of every `.cs` and `.csproj` file in the inspected FSM_COS master and development trees found a concrete difference: **master's `RuntimeAssembly` and `RuntimeManifest` use `SemanticIntent` from FSM_UserIO; development has no matching source reference even though its project file still declares the package.** Therefore the FSM_UserIO dependency cannot be called dead weight from package metadata alone. The next decision is whether to preserve the intent-bearing runtime contract and bring it into development with tests, or intentionally redesign the contract first. Do not remove the dependency as a cleanup-only change.


#### FSM_COS branch disposition: development is the stronger composition base, with a master-only intent feature to preserve

The source-level comparison shows more than the FSM_UserIO difference:

- **Development has the stronger manifest/configuration path:** version-specific root entries, version-aware catalog resolution, and an optional `IMicroBundleConfigurationSource` that supplies per-runtime/per-bundle configuration. This directly supports the target of manifest-driven, versioned MicroBundle composition.
- **Master has the simpler dependency-request API** and carries `SemanticIntent` from `RuntimeManifest` into `RuntimeAssembly`, but its inspected catalog contract does not require a version for the root request and its manifest does not expose the development configuration-source path.
- **Both retain the core composition behavior** of recursive loading, cycle detection, and bounded arbitration, but their loading contracts are not source-compatible as-is.

**Preliminary recommendation:** use development as the integration base for versioned, configured composition; port the master intent field into the development manifest/runtime assembly as a small, tested additive capability if its contract is still desired. Do not replace the development manifest/configuration work with master wholesale. Before committing to this choice, inspect MicroBundleDomain's dependency/version contract and run tests against the intended package/API version. This is an architectural recommendation from source inspection, not a claim that development currently passes CI or that the master-only intent feature has been migrated.


#### MicroBundleRepository source comparison: Core boundary and publisher branch (2026-10-08)

A direct comparison of the current Core and Azure project/source files confirms that the two lines share the same artifact address validation, SHA-256 verification, and immutable content-copy behavior. The main design difference is dependency placement:

- Development Core alpha.1 has no package references and treats artifacts as opaque bytes.
- Master Core alpha.3 adds a hard Ontology dependency solely to expose optional `OntologyAddress` metadata on `MicroBundleArtifact`, and declares FSM_Serialization as a Core package dependency for the assembly-payload/envelope path.
- The publisher branch adds `MicroBundleAssemblyPayload` to Core, coupling the repository's artifact contract to `IBinarySerializable`. It also adds a publisher executable that performs a repository round-trip and then verifies FSM_COS materialization.

**Disposition:** keep the dependency-light Core contract from development as the target. Optional semantic metadata should be carried by a separate catalog/manifest or optional metadata contract, not force every repository consumer to reference Ontology. Assembly-envelope serialization and materialization should live in an explicit payload/serialization or FSM_COS adapter boundary, not in the generic repository Core. Preserve the publisher's end-to-end round-trip/materialization check as a standalone tool or integration test; do not discard it just because its current implementation is coupled.

The publisher branch is therefore not simply redundant with development's CLI. The development CLI provides human-operated REST operations (health/list/put/get); the publisher branch demonstrates a stronger end-to-end publish → retrieve → materialize verification path. Preserve that unique test/tool behavior while deciding whether it belongs in a standalone publisher tool, automated integration test, or both.

One compatibility warning remains concrete in source: the master REST project references Ontology alpha.2, and the master FSM_COS adapter references FSM_COS alpha.5, while inspected current source declarations are Ontology alpha.3 and FSM_COS alpha.6. Development REST references FSM_COS alpha.3. These are source declarations, not proof of the versions available on NuGet. Do not change the pins until restore/build/tests validate the chosen target.

- [ ] Extract or redesign assembly payload/envelope handling outside repository Core, preserving format/version tests.
- [ ] Keep optional semantic metadata out of the required Core dependency closure unless a concrete consumer proves it is foundational.
- [ ] Preserve the publisher's materialization verification in an automated test path, including exact artifact hash and requested bundle ID.
- [ ] Verify Core/Azure immutability and hash checks with contract tests; inspect behavior when a content-addressed Azure object already exists but retrieval bytes fail validation.
- [ ] Confirm the final package workflow's pack set matches the projects intentionally published, and keep every publish job gated with `&& false`.


#### Publisher verification gap found and corrected in its feature branch (2026-10-08)

The open draft [MicroBundleRepository PR #17](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleRepository/pull/17) contains a useful no-Azure end-to-end smoke tool: repository put/get, byte-for-byte comparison, then FSM_COS materialization and requested-ID verification. Source inspection found its project was **not listed in the feature branch's solution**, so the normal solution build/test workflow would not compile that executable. Added the project to `TheSingularityWorkshop.MicroBundleRepository.slnx` on `feature/microbundle-publisher` in commit `7bb3ac5482585aaad5480cb92e7ec31342e4b833`.

**CI follow-up verified (2026-10-08):** the build-and-test run associated with that commit completed successfully: restore, solution build, and solution test steps all passed, so the newly included publisher project compiles as part of the solution. The package workflow's verification job also passed its build/test/pack steps, while its publish job was **skipped** by the explicit `&& false` safeguard. No NuGet publication occurred. Important limit: the CI run did **not** execute the publisher's `--local` round-trip against a real deterministic MicroBundle artifact; that behavior remains source-inspected, not end-to-end test-proven. PR #17 remains draft/open and must not be merged as a side effect of this audit.

- [x] Verify PR #17 CI builds the newly included publisher project.
- [ ] Add an automated test or explicit workflow step that runs the local round-trip against a deterministic test artifact and proves the requested bundle ID is materialized.
- [ ] Resolve the Core serialization boundary before treating the publisher as a clean architectural example; its end-to-end behavior is valuable, but current project references inherit the master branch's serialization/materializer coupling.


### 6. Forge / AnyApp release-readiness assessment (2026-10-09)

This section supersedes older dependency observations above where they conflict with the current inspected development heads. It is a source-level readiness assessment, not a package publication or a claim that consumer builds have passed against the next NuGet version.

**Release blocker found and corrected in source:** `src/FSM_COS/FSM_COS.csproj` had declared `<Version>0.1.0-alpha.5</Version>` while the development source exposes the newer versioned-root manifest and separate configuration-source contract. Because alpha.5 is the existing consumer baseline, I changed the development project metadata to `<Version>0.1.0-alpha.6</Version>` in commit [`3610890a470f86eee7f83b781ee9de143f16300f`](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/commit/3610890a470f86eee7f83b781ee9de143f16300f). This is a source-preparation change only: the candidate still requires a passing build/test/pack run and review before any publication. Keep the package version consistent across project metadata, release notes, package artifact, and consumer smoke tests.

#### Consumer findings

| Consumer | Inspected source | Required action for the next FSM_COS package |
|---|---|---|
| **AnyApp** | `development/AnyApp.csproj` pins `TheSingularityWorkshop.FSM_COS` `0.1.0-alpha.5`. Its README describes repository-backed artifact resolution, materialization into an `IMicroBundle`, composition through FSM_COS, then host-side GUI.WPF manifestation. It also retains a local compatibility catalog for legacy development manifests. | After the new package is actually available, update the pin in a dedicated consumer change and run the Windows restore/build/test path. Prove both the selected repository-backed route and the local compatibility route; do not treat the README alone as execution proof. No new WPF, REST, repository, GUI, ProtocolAi, GrammarAi, or FSM_UserIO dependency belongs in FSM_COS. |
| **The Forge** | `forge/native-experience-authoring` references FSM_COS `0.1.0-alpha.3`. Its `ForgeExperience.Compile()` still emits the older `BundleRequest`-based manifest with inline configuration bytes. | Migrate Forge to versioned `MicroBundleManifestEntry` roots and provide a Forge-owned `IMicroBundleConfigurationSource` when executing the compiled manifest. Keep authored configuration documents and persistence in Forge; do not put bytes or a serializer into the FSM_COS manifest. Validate the migration against the actual released package, not an assumed future API. |
| **WebPage** | FSM_COS's integration guide identifies WebPage as the browser proving ground but records that Living GUI behavior still passes through transitional host-local `PageFSM` / `LivingGuiFsm` code after composition. | Continue the host proof separately. Do not add browser lifecycle, GUI, rendering, or scheduling APIs to FSM_COS merely to make WebPage's local migration easier. |

#### API coverage found in current FSM_COS development source

The inspected development source already contains the core contracts the two immediate hosts need:

- `RuntimeManifest(RuntimeId, Bundles, ExperienceContext)`, with root entries carrying a bundle ID and requested version.
- `FsmCos.Execute(manifest, configurationSource)`, with configuration source optional.
- `IMicroBundleConfigurationSource.TryGetConfiguration(runtimeId, bundleId, version, out bytes)`; absent configuration means bundle defaults.
- Version-aware catalog resolution, dependency closure, bounded arbitration, and a returned `RuntimeAssembly`.
- Preflight rejection of conflicting requested versions for the same root identity, while repeated requests for the same ID/version are loaded once.

The current development source does **not** appear to need a new host-specific feature solely for Forge or AnyApp. The immediate gap is **consumer alignment and end-to-end verification**, not adding Forge or AnyApp responsibilities to the kernel.

#### Release gate for today's candidate

- [ ] Confirm the exact development commit to be released and run its build/test workflow on that commit.
- [ ] Verify the package version and package contents match the source contract documented above.
- [ ] Keep the NuGet workflow's publish condition disabled by default with the explicit `&& false` safeguard. This checklist is not release authorization.
- [ ] Run a package-consumer smoke test against the candidate package: construct a manifest with a versioned root, execute it through an in-memory catalog, and verify the expected bundle is present in `RuntimeAssembly`.
- [ ] Add/retain a configuration-source test proving bytes are delivered to the requested runtime/bundle/version and absence falls back to defaults.
- [ ] Retain regression coverage for same-version duplicate roots and conflicting-version roots failing before any load.
- [ ] After the package is available, update AnyApp and Forge in separate, reviewable changes; do not bundle their migration into the FSM_COS package release itself.
- [ ] For AnyApp, verify repository artifact hash/materialization and actual RuntimeAssembly-to-GUI handoff in the Windows path.
- [ ] For Forge, add tests for versioned manifest compilation, configuration-source lookup, missing-configuration defaults, and no accidental serialization/manifest coupling.
- [ ] Record exact consumer commit SHAs and build/test results before calling either consumer aligned.

**Disposition:** the kernel contract appears sufficient for the current Forge/AnyApp direction. Do not expand the kernel unless a concrete failing consumer test exposes a missing composition contract. The release is not proven by documentation or source inspection alone, and no publication is authorized by this document.

### 5. Alpha.6 release-candidate implementation checks (2026-10-09)

- [x] Keep the development version at `0.1.0-alpha.6`; this remains a candidate, not publish authorization.
- [x] Preserve the master-only optional `SemanticIntent` capability without replacing the newer versioned-root/external-configuration contract: `RuntimeManifest.Intent` flows to `RuntimeAssembly.Intent`, and `RuntimeManifest.Empty` accepts optional intent.
- [x] Add preflight validation when a published `RuntimeManifestSchedule` is present: resolve the dependency closure and compare bundle IDs, resolved versions, and exact dependency edges before any bundle `Load` side effects.
- [x] Add tests for matching schedule/dependency graph, graph mismatch rejection before loading, and semantic-intent pass-through.
- [x] Build, tests, coverage, and pack passed on candidate commit `77c05d68d70159e6f4ffb53bd2209a5ed6ed0abc` in [Actions run 37988552679](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/actions/runs/37988552679).
- [x] Clean restore/build/test/pack against public NuGet dependencies passed in the separate `public-package-compatibility` job of [Actions run 37988552679](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/actions/runs/37988552679), without the CI-only local MicroBundleDomain feed.
- [x] Add candidate release notes at `docs/releases/0.1.0-alpha.6.md`, explicitly separating implemented behavior from staged-loading limitations.
- [x] Inspected the public-feed `.nupkg`: version `0.1.0-alpha.6`, README/license included, dependencies are FSM_API 1.0.13, FSM_UserIO 0.1.0-alpha.1, and MicroBundleDomain 1.0.1; release notes are included and internal branch/continuation planning files are excluded. The workflow's publish condition remains `github.event_name == 'workflow_dispatch' && inputs.publish == true && false`.
- [ ] Verify consumer compatibility (at minimum Forge and AnyApp API migration plan); no consumer should be pinned to alpha.6 before the package is actually published.
- [ ] Final release review by owner; only explicit approval may ever authorize the publish mechanism.


### 6. Alpha.6 release-candidate verification and remaining blocker (2026-10-09)

- [x] Development code/package candidate `77c05d68d70159e6f4ffb53bd2209a5ed6ed0abc` passed [Actions run 37988552679](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/actions/runs/37988552679): build/test/coverage/pack and a separate public-NuGet restore/build/test/pack path succeeded; publication was skipped.
- [x] Development project version and release notes declare `0.1.0-alpha.6`; the publish condition remains explicitly gated by `&& false`.
- [ ] **Release blocker:** master and development both declare alpha.6 but expose different manifest APIs. Master still uses `MicroBundleDependencyRequest` with inline configuration; development uses versioned `MicroBundleManifestEntry` roots, external `IMicroBundleConfigurationSource`, and schedule-vs-resolved-graph preflight validation. The branch comparison reports development 172 ahead / 40 behind master. Do not publish until these source lines are reconciled, since a NuGet version cannot safely identify two different public contracts.
- [ ] Selectively reconcile the tested development contract into the master release line while preserving master-only documentation and branch history. Add the public-package compatibility job to master’s workflow, then run the full release checks on the exact master candidate.
- [ ] Inspect the master candidate package artifact, dependency metadata, README/license/release notes, and packaged-file exclusions; verify all package-producing workflows remain disabled by default. Then provide the owner a final release review. This checklist is not publication authorization.

The next repository-level milestone is **master alpha.6 source reconciliation**, not further staged-loading expansion and not WebPage work. Once the exact master candidate is verified and the release review is complete, WebPage can resume; actual publication still requires explicit owner approval.


### Master alpha.6 candidate — CI and package inspection passed (2026-10-09)

[PR #25](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/pull/25) carries the selective release contract onto a branch based on master. It preserves master history while bringing over the versioned manifest/configuration API, semantic intent, staged dependency-graph validation, tests, release notes, and public-package compatibility workflow. The PR is open, ready for review, mergeable, and not merged.

- [x] Verified exact PR head `385bec07588185d7cc98471db1f1015e9da69e5c` in [Actions run 37992111115](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/actions/runs/37992111115): build/test/coverage/pack and clean public-NuGet restore/build/test/pack succeeded; `publish_nuget` was skipped.
- [x] Inspected the generated package: `TheSingularityWorkshop.FSM_COS 0.1.0-alpha.6`, dependencies FSM_API 1.0.13, FSM_UserIO 0.1.0-alpha.1, MicroBundleDomain 1.0.1; README/license/release notes included; internal planning docs excluded. TRX reports 39/39 tests passed, with zero .NET compiler warnings/errors.
- [ ] Owner review and approval to merge PR #25 into master. After merge, verify the exact master-head workflow and package artifact again.
- [ ] Only after master verification, prepare the final release review. Keep publication disabled with `&& false` and do not publish without separate explicit approval.
