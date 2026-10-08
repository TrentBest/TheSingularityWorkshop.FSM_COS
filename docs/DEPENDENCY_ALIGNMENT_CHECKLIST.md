# Dependency Alignment Checklist

> **Status:** audit worklist based on source metadata inspected on 2026-10-08. This is not a release plan, merge approval, or claim that a package version is published. Confirm each item against the intended branch, package feed, and CI before changing references.

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
| FSM_COS | Master `RuntimeAssembly.cs` and `RuntimeManifest.cs` use `SemanticIntent` from FSM_UserIO; the development `.cs`/`.csproj` snapshot has no matching reference despite the direct package pin. Both source project files declare FSM_COS alpha.6, FSM_API 1.0.13, FSM_UserIO alpha.1, and MicroBundleDomain 1.0.1. The package workflow also checks out MicroBundleDomain branch `architecture/runtime-contract-ownership` and packs a local dependency for CI. | This is a real API divergence, not proof that FSM_UserIO is unused. Decide whether the intent-bearing manifest/assembly contract from master is the desired direction and port/test it on development, or explicitly redesign the contract before removing the dependency. Confirm the pinned MicroBundleDomain branch is intentional and ensure CI also demonstrates compatibility with the package version consumers actually restore. |
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
