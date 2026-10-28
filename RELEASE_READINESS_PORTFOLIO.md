<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop moniker" width="720" /></a></p>

# Workshop Release Readiness Portfolio

**Assessment date:** 2026-10-28 UTC  
**Status:** Initial portfolio assessment from current `development` heads, recent GitHub Actions runs, and existing release checklists. This is a triage tool, not authorization to release and not an exhaustive audit of every Workshop repository.

This portfolio answers two different questions:

1. **Which package could be the next controlled release with the least remaining work?**
2. **Which unfinished cross-repository work is most likely to block a useful end-to-end Workshop experience?**

A green workflow is evidence for the exact SHA and jobs that ran. It does not prove manual package inspection, deployment readiness, full consumer compatibility, or owner authorization. No readiness percentages are assigned because the projects have different scopes and their gates are not interchangeable.

## 01 — Executive priority

### Closest release candidates

1. **FSM_COS 0.1.0-alpha.7 — closest scoped candidate, but currently blocked.** The latest package workflow on `013bfb328a87e2c9ca47ef0dc8f9dc575ff9c1b1` failed immediately in the Workshop Markdown branding gate. Public-package compatibility and development-source integration jobs passed, CodeQL passed, while the full build/test/package stage was skipped after the early failure. The benchmark and CiAi evidence workflows were still running at the time of this check. [Package workflow](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/actions/runs/40761037550) · [CodeQL](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/actions/runs/40761037201) · [Benchmark evidence](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/actions/runs/40761037558) · [CiAi evidence](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/actions/runs/40761037440). Fix the documentation gate first, then rerun the complete candidate pipeline on the resulting exact SHA and inspect that SHA's package artifact. The alpha.7 checklist explicitly defers parallel loading and several broader runtime lifecycle capabilities; those are not alpha.7 blockers unless the public claims or candidate scope change. [Candidate checklist](.github/release-readiness/0.1.0-alpha.7.md)

2. **FSM_UserIO 0.1.0-alpha.2 — technically lining up, scope and final evidence still open.** The current `development` HEAD `5f3b9f9539970cee3263e46266a198c9af472ba9` passed [Build, test, and package](https://github.com/TrentBest/FSM_UserIO/actions/runs/40761032711). The Windows-audio adapter has a recent successful run at an earlier SHA, not the current HEAD. Alpha.2 remains provisional: decide which newly added semantic contracts are in scope, verify the zero-runtime-dependency/platform-neutral boundary and packed artifact, finish the branding/link review, and rerun every required adapter/sample gate against the frozen candidate. Existing work: [readiness checklist](https://github.com/TrentBest/FSM_UserIO/blob/development/docs/0.1.0-alpha.2-READINESS.md), [documentation issue #8](https://github.com/TrentBest/FSM_UserIO/issues/8).

3. **FSM_REST 0.1.0-alpha.5 + optional adapters — good automated evidence, manual and consumer-compatibility gates remain.** Current `development` HEAD `19e8672b9c58c459d441c5655d458ebe03e4e38c` has a successful [Package FSM REST workflow](https://github.com/TrentBest/TheSingularityWorkshop.FSM_REST/actions/runs/40536235482); the publish job is designed to remain skipped unless separately authorized. Remaining gates include the exact current artifact/manual content review, public XML/API and release-note review, and consumer compilation against the actual published package rather than a source checkout. The core package is string-body HTTP transport, not a binary/streaming abstraction. In The, `Architect.csproj` still pins published FSM_REST alpha.3 while development source declares alpha.5; do not assume alpha.5 is published. [Release checklist](https://github.com/TrentBest/TheSingularityWorkshop.FSM_REST/blob/development/docs/RELEASE_CHECKLIST.md)

### Foundational package with good current CI, but needs a release-specific gate

4. **MicroBundleDomain 1.0.1 — stable foundation candidate; confirm public contract and artifact before treating it as release-ready.** Current `development` HEAD `32f0b812205287ab3852d159859fbeae804b70ad` has a successful [Pack and publish workflow](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleDomain/actions/runs/39896419780). The source now includes a publish-resolved MicroBundle configuration contract, so the release review must specifically check public API semantics, XML docs, package contents, and consumer compatibility for that addition. This audit did not independently verify the exact artifact on NuGet or a clean external consumer. No new version or publication is proposed.

## 02 — Where the most work remains

| Repository / workstream | Current evidence | Readiness disposition | Highest-value next work |
|---|---|---|---|
| **MicroBundleRepository REST / Azure** | Current HEAD `de6aec4` has successful Build and Test, Package, and CiAi observation runs: [build](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleRepository/actions/runs/40646443290), [package](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleRepository/actions/runs/40646443329), [CiAi](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleRepository/actions/runs/40646443396). Existing REST readiness document explicitly says **NOT RELEASE-READY**. | **Most important backend release/integration gap.** Passing CI does not close documented blockers. | Define deterministic `(bundleId, version) → trusted content hash` resolution; finish and deploy explicit trusted-origin configuration; verify live Azure oversized-read/write behavior; complete host HTTP error/cancellation tests and end-to-end publish → catalog → retrieval → verification → FSM_COS composition/activation; inspect exact packages and dependency graph. Keep publication and write APIs secured. [REST readiness](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleRepository/blob/development/docs/REST_RELEASE_READINESS.md) |
| **FSM_COS runtime integration** | Alpha.7 scope is intentionally sequential. The current package workflow fails before build/test because of documentation branding; public-package compatibility and source-integration jobs passed. | **Near-term package blocker is narrow; ecosystem integration remains larger.** | Fix the Markdown branding gate, rerun all jobs on the new SHA, verify benchmark/CiAi results and inspect package. Separately continue the repository-backed catalog convergence, Experience lifecycle contract, atomic host activation, and publication/catalog registration issues. Do not turn every wider architecture issue into an alpha.7 blocker. |
| **AnyApp** | Current HEAD `5279de6` passed [.NET workflow](https://github.com/TrentBest/AnyApp/actions/runs/40759839915); several prior runs failed before that success. | **Not an independent release candidate yet; runtime correctness/integration candidate.** | Reconfirm the corrected runtime correctness tests on exact HEAD; prove repository-backed composition and activation against published dependencies as well as source integration. Track the existing Experience publication and compatibility gaps. Do not treat green tests alone as end-to-end Experience proof. |
| **Foundry** | Current HEAD `668f4d6` passed [FSM_API and Fox/Sheepdog proof](https://github.com/TrentBest/TheSingularityWorkshop.Foundry/actions/runs/40650351803). | **Implementation proof is progressing; no release-ready package claim made.** | Complete the execution-provider/Hub runtime ownership contract and its lifecycle/cancellation/outcome conformance tests; coordinate scheduling intent with MicroBundleDomain and FSM_COS. Keep GUI as presentation and Foundry as execution lifecycle. Decide a package scope only after the public API and consumers are clear. |
| **The (Architect / Workshop desktop)** | Current HEAD `7629222` passed [Windows tests](https://github.com/TrentBest/The/actions/runs/40761726828) and [source-derived UML](https://github.com/TrentBest/The/actions/runs/40761726785). | **Current app checks green; not evidence of a product/package release.** | Continue exact published-package compatibility for FSM_REST/FSM_API, replace async-void startup with an awaited cancellable lifecycle, and verify any remaining direct-HttpClient call sites against the intended REST transport boundary. |
| **Cross-host Experience path (WebPage, AnyApp, Experiences, repository)** | Source audit has found integration seams and repository-local issues, but no full clean-room end-to-end proof. | **Largest end-to-end product gap.** | Prove one manifest selection through immutable artifact identity, dependency closure, verified retrieval, composition, candidate activation, failure preservation, and retirement/shutdown. Use existing issues rather than creating duplicate work: [WebPage #126](https://github.com/TrentBest/WebPage/issues/126), [Experiences #28](https://github.com/TrentBest/TheSingularityWorkshop.Experiences/issues/28), [MicroBundleRepository #22](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleRepository/issues/22), [AnyApp #23](https://github.com/TrentBest/AnyApp/issues/23). |

## 03 — Practical order of work

1. **Unblock FSM_COS alpha.7 automation.** Fix the specific Markdown branding-gate failure on `development`; rerun the full package workflow. The latest failure occurred before compilation, tests, coverage, and package inspection, so those gates must not be inferred from the passing sub-jobs.
2. **Freeze one candidate at a time.** For FSM_COS alpha.7, FSM_UserIO alpha.2, and FSM_REST, record the exact source SHA, complete workflow results, package artifact digest, package metadata/dependencies, and clean-consumer evidence. Do not reuse historical green runs after any source or documentation commit.
3. **Make the next release decision explicit.** FSM_COS alpha.7 is the closest scoped target once its gate is repaired. FSM_UserIO alpha.2 is also close, but its public-contract scope is still a decision. FSM_REST has good automated checks but still requires current-candidate manual review. MicroBundleDomain is a foundational package whose latest source passes CI, but this assessment did not independently validate a candidate artifact on NuGet.
4. **Prioritize repository delivery before broad host polish.** The biggest system-level risk is not a missing badge or a green test; it is the incomplete, independently verifiable path from published Experience manifest to immutable verified content and active runtime.
5. **Keep the Foundry boundary clean.** Do not build a competing lifecycle coordinator in FSM_COS or GUI. FSM_COS composes; MicroBundleRepository delivers immutable artifacts; Foundry owns workflow/execution lifecycle; hosts own platform permissions and manifestation; GUI owns presentation.
6. **Keep releases controlled.** No NuGet publication, merge/release, deployment, branch deletion, or `master` change is authorized by this portfolio. Publishing a package is a separate human decision after the evidence gates pass.

## 04 — How to interpret this portfolio

- **Closest candidate** means the intended scope appears small enough that fixing the identified gates could enable a controlled release review. It does not mean “ready now.”
- **Automated green** means the named workflow succeeded on the named SHA; it does not substitute for manual package inspection or live deployment evidence.
- **Most work remaining** is split between package-release blockers and end-to-end ecosystem gaps. Those are different queues and should not be collapsed into a single percentage.
- **Unknown** means not independently verified in this pass. It does not mean failed, and it must not be silently treated as passed.

The portfolio should be refreshed whenever a candidate SHA changes, a release is explicitly authorized, or a blocking issue is closed with evidence.

---

## 🔗 Resources & Support

- **Project:** [FSM_COS](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS)
- **Workshop projects:** [FSM_API](https://github.com/TrentBest/FSM_API) · [FSM_REST](https://github.com/TrentBest/TheSingularityWorkshop.FSM_REST) · [MicroBundleDomain](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleDomain) · [MicroBundleRepository](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleRepository)
- **Connect:** [GitHub](https://github.com/TrentBest) · [Coder Legion community](https://coderlegion.com/)
- **Support:** [Patreon](https://www.patreon.com/c/TheSingularityWorkshop) · [PayPal](https://www.paypal.com/donate/?hosted_button_id=3Z7263LCQMV9J)

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
