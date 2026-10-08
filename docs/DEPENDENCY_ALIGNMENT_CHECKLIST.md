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
| FSM_COS | Current source references FSM_API 1.0.13, FSM_UserIO alpha.1, and MicroBundleDomain 1.0.1. | Trace the exact FSM_UserIO/SemanticIntent use before deciding whether UserIO belongs in the mandatory composition floor. |
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

### 4. Documentation rollout

- [ ] Reconcile FSM_COS README changes with open PR #22 (base development), PR #23 (base master), and draft PR #14 before standardizing README headings.
- [ ] Apply the shared numbered topic taxonomy repository-by-repository, retaining project-specific content and the stable semantic/color mapping.
- [ ] Ensure docs distinguish implemented behavior, intended architecture, and future work.
- [ ] Audit links after each batch and record the exact branch/commit reviewed.

## Release-safety checkpoint

- [x] The FSM_COS development workflow at .github/workflows/package.yml now includes an explicit && false in its publish job condition. The inspected master workflow already had an && false safeguard. No NuGet publication was performed.
- [ ] Audit the remaining ecosystem package workflows individually; a repository-wide search is only a discovery aid and is not proof that every workflow is safe.

## Completion criteria

A row is complete only when its intended version is identified, the package/source compatibility is demonstrated by restore/build/tests, the consumer’s actual usage is understood, and any release action has explicit approval. Documentation alone does not make a dependency aligned or a capability integrated.
