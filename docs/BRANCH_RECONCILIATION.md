<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# FSM_COS Branch Reconciliation Ledger

**Maintainer-only release record.** This ledger documents branch policy and the current observed state; it is not consumer documentation.

## Live Sith activity audit — 2026-11-25 UTC

This is the current FSM_COS branch snapshot, refreshed from live GitHub refs and exact commit metadata. Older snapshots below are historical evidence and must not be read as the current branch state.

### Branch activity — newest first

| UTC committer time | Branch | Author / committer | Latest subject | Head SHA |
|---|---|---|---|---|
| 2026-11-25 03:50:08 | [development](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/tree/development) | TrentBest / TrentBest | Clarify animated moniker alternative text | [fb84797451736459ee9cd77331c1ed13cd9d342a](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/commit/fb84797451736459ee9cd77331c1ed13cd9d342a) |
| 2026-11-24 14:50:39 | [master](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/tree/master) | github-actions[bot] / github-actions[bot] | chore: refresh Sith policy evidence | [e4f2e3edc79980c1580ce24b93dae3a683aa1b82](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/commit/e4f2e3edc79980c1580ce24b93dae3a683aa1b82) |
| 2026-11-16 05:49:11 | [fix/markdown-footer-validation](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/tree/fix/markdown-footer-validation) | TrentBest / TrentBest | fix: safely quote CiAi evidence summary fields | [1e1b139f13d049835ed02e84f75831d9c88580f5](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/commit/1e1b139f13d049835ed02e84f75831d9c88580f5) |
| 2026-11-14 09:58:35 | [work/ciai-evidence-hardening](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/tree/work/ciai-evidence-hardening) | TrentBest / TrentBest | Fail closed on missing or ambiguous CiAi source runs | [0158272cd516312e41e8ffa6282882325ddde22a](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/commit/0158272cd516312e41e8ffa6282882325ddde22a) |
| 2026-11-03 21:05:08 | [policy/sith-branch-activity-audit](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/tree/policy/sith-branch-activity-audit) | TrentBest / TrentBest | Record live Sith branch activity and verification audit | [26fcd53770efdf01cedad8e9d884e0b3e13b8790](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/commit/26fcd53770efdf01cedad8e9d884e0b3e13b8790) |

### Reconciliation and verification

- Live FSM_COS inventory: **five branches** — `development`, `master`, and three noncanonical branches. Sith is **NON-COMPLIANT** until the owner removes the reviewed extras.
- Divergence: `development` is 306 commits ahead of `master`; `master` is 28 commits ahead of `development`; merge base `56ad34dbe4722ba1edf9a4da6a00812eeaa29101`. Master-only differences include source, tests, benchmarks, documentation, and workflow files. Preserve this history; do not force-reset or replace master as part of branch cleanup.
- Exact-head verification for development `fb84797451736459ee9cd77331c1ed13cd9d342a`:
  - [Pack and publish run 45542682163](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/actions/runs/45542682163): build, test/coverage, package verification, public-package compatibility, and source integration passed. NuGet publication was **skipped**; only artifacts were produced.
  - [CodeQL run 45542682047](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/actions/runs/45542682047): passed.
  - [Sith policy run 45542682049](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/actions/runs/45542682049): failed because the repository has five branches rather than two.
  - [Benchmark evidence run 45542682094](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/actions/runs/45542682094): pending at snapshot time.
  - [CiAi commit evidence run 45542682028](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/actions/runs/45542682028): in progress at snapshot time.
- Release baseline: no exact public-package-to-master-SHA association was established by this audit. Do not infer one from the branch name.

### Noncanonical branch dispositions

| Branch | Compare to development | PR state | Disposition |
|---|---:|---|---|
| `policy/sith-branch-activity-audit` | 2 ahead / 151 behind | [PR #52](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/pull/52) closed unmerged | Its useful portfolio-reporting requirements were incorporated into development `AGENTS.md`. Its 2026-11-03 ledger snapshot is superseded by the development ledger. **Ready for owner deletion** after a final ref check. |
| `work/ciai-evidence-hardening` | 2 ahead / 151 behind | [PR #53](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/pull/53) closed unmerged | Its workflow pins an older CiAi commit and .NET 8; development now pins the newer CiAi contract and .NET 10. The behavior is superseded. **Ready for owner deletion** after a final ref check. |
| `fix/markdown-footer-validation` | 5 ahead / 119 behind | [PR #56](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/pull/56) open | The safer `printf` summary formatting and moniker alt-text improvement were ported to development. The PR also removes the canonical Workshop footer from `AGENTS.md`, `CiAiLLM.md`, `FiAiLLM.md`, and `GUILLM.md`; that conflicts with the documentation standard and was **not** ported. Retain pending owner review or a revised PR. **Not ready for deletion.** |

No branch was deleted, no branch ref was rewritten, master was not changed, and no NuGet package was published in this audit.

## Portfolio branch-reduction snapshot — 2026-11-25 UTC

The live inventory covered 53 Workshop ecosystem repositories, including FSM_Dungeon. It contained 251 branch refs: 30 repositories already had exactly `master` and `development`; 23 repositories had extra branches; 145 refs were above the two-branch target.

The highest-churn repositories were FSM_Execution (27 branches), GUI (26), MyVR (16), MicroBundleRepository (15), and MicroBundleDomain (14). Across the portfolio, 43 repositories had master-only commit history relative to development. Several master diffs contain real source, tests, or documentation, not just policy badges. Do not replace master until each repository's release baseline and master-only work have been reviewed under the release procedure.

### Cross-repository branches currently verified as deletion candidates

These branch tips are ancestors of their repository's current development branch (`ahead_by = 0`), or their unique work has been preserved/superseded as noted. The owner performs deletion; recheck each live ref and PR immediately before the deletion batch.

| Repository / branch | Exact branch head | Latest committer UTC | Latest subject | Evidence / disposition |
|---|---|---|---|---|
| [TheSingularityWorkshop `feature/singularity-hub-microbundles`](https://github.com/TrentBest/TheSingularityWorkshop/tree/feature/singularity-hub-microbundles) | [93a7932ba9c68312c65b858fd5e776d049094c66](https://github.com/TrentBest/TheSingularityWorkshop/commit/93a7932ba9c68312c65b858fd5e776d049094c66) | 2025-06-21 02:01:54 | Add project files. | 0 ahead / 10 behind development; no PR found. |
| [MicroBundleRepository `adp/rest-identity-workshop-auth`](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleRepository/tree/adp/rest-identity-workshop-auth) | [b81de0792ad093c5c569cf2e496cc0ff6c27f03c](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleRepository/commit/b81de0792ad093c5c569cf2e496cc0ff6c27f03c) | 2026-11-20 04:56:49 | test(rest): reject legacy generic repository write key | 0 ahead / 74 behind; [PR #40](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleRepository/pull/40) merged into development. |
| [MicroBundleRepository `adp/revision-identity-reconcile`](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleRepository/tree/adp/revision-identity-reconcile) | [fdbe30f57f10ca300aab615bf5a81c6682aeecd0](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleRepository/commit/fdbe30f57f10ca300aab615bf5a81c6682aeecd0) | 2026-11-21 06:19:24 | docs: sync domain map with current integration status | 0 ahead / 80 behind; [PR #39](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleRepository/pull/39) merged into development. |
| [MicroBundleRepository `feat/workshop-rest-publisher`](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleRepository/tree/feat/workshop-rest-publisher) | [518b2a2628f148eab96abb1cdbf726a59894d354](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleRepository/commit/518b2a2628f148eab96abb1cdbf726a59894d354) | 2026-11-21 06:25:40 | docs: reconcile branch deletion readiness with current identity contract | PR #41 merged; its code head [d6a0cd0833bb06435de05ca7a0d0bc8ebf31dae8](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleRepository/commit/d6a0cd0833bb06435de05ca7a0d0bc8ebf31dae8) passed [Build/Test](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleRepository/actions/runs/44874835093) and [Package](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleRepository/actions/runs/44874835104); the remaining audit-doc commit is superseded. **Ready for owner deletion.** |
| [MicroBundleRepository `feature/revision-identity-bindings`](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleRepository/tree/feature/revision-identity-bindings) | [64829170c946b5862458920ab38cb4e4c3370be4](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleRepository/commit/64829170c946b5862458920ab38cb4e4c3370be4) | 2026-11-12 08:57:38 | docs: record recovered source integration validation | [PR #33](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleRepository/pull/33) closed as superseded by merged PR #39; stricter identity validation, filesystem/Azure bindings, failure handling, and tests are integrated. **Ready for owner deletion.** |
| [MicroBundleRepository `feature/microbundle-publisher`](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleRepository/tree/feature/microbundle-publisher) | [2848042206ecc16cc121b4dbbf7c3a834d60a9b7](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleRepository/commit/2848042206ecc16cc121b4dbbf7c3a834d60a9b7) | 2026-10-08 23:22:39 | test: verify local artifact round trip and materialization | [PR #17](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleRepository/pull/17) closed as superseded by merged PRs #40/#41 and the separate FSM_COS integration package; the old Core assembly-payload envelope was intentionally not adopted. **Ready for owner deletion.** |
| [MicroBundleRepository `feature/aspnetcore-10-dependencies`](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleRepository/tree/feature/aspnetcore-10-dependencies) | [7ee60d471c4d8627b5a0507a67e21a2c95d0e203](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleRepository/commit/7ee60d471c4d8627b5a0507a67e21a2c95d0e203) | 2026-11-15 09:19:02 | Upgrade ASP.NET Core test host to 10.0.15 | [PR #37](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleRepository/pull/37) closed unmerged; its only changed test-project file is byte-identical to development. **Ready for owner deletion.** |
| [MicroBundleRepository `work/portable-artifact-identity-contract`](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleRepository/tree/work/portable-artifact-identity-contract) | [18987e8d851989dbd4731a271c5922a84543e8f6](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleRepository/commit/18987e8d851989dbd4731a271c5922a84543e8f6) | 2026-11-20 01:38:00 | Prevent invalid default portable identities | [PR #38](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleRepository/pull/38) closed unmerged; its identity type/tests are superseded by PR #39. The proposed `{repository-local-key}` storage layout was rejected; numeric local address compatibility is retained. **Ready for owner deletion.** |
| [MicroBundleDomain `architecture/runtime-contract-ownership`](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleDomain/tree/architecture/runtime-contract-ownership) | [e81747aa3741ad6de5ae1b6077bf1d30ea0a3a4c](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleDomain/commit/e81747aa3741ad6de5ae1b6077bf1d30ea0a3a4c) | 2026-10-03 05:47:56 | Merge pull request #7 from TrentBest/docs/benchmarking | 0 ahead / 83 behind development; its runtime-contract PR was merged to master and its tip is now contained in development. |
| [MicroBundleDomain `docs/benchmarking`](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleDomain/tree/docs/benchmarking) | [b400a16f850a22edc841fb0acc824073770ce906](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleDomain/commit/b400a16f850a22edc841fb0acc824073770ce906) | 2026-10-03 00:31:59 | docs: add MicroBundleDomain performance baseline | 0 ahead / 88 behind; [PR #7](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleDomain/pull/7) merged into the architecture branch, whose tip is contained in development. |
| [MyVR `feature/myvr-fsm-foundry-reference-migration`](https://github.com/TrentBest/TheSingularityWorkshop.MyVR/tree/feature/myvr-fsm-foundry-reference-migration) | [3368e16db56b8d2b976f578a1c4fc754f9b807e3](https://github.com/TrentBest/TheSingularityWorkshop.MyVR/commit/3368e16db56b8d2b976f578a1c4fc754f9b807e3) | 2026-11-01 01:05:39 | Link README moniker to MyVR Pages site | 0 ahead / 4 behind development; no PR found. |
| [HeadlessAi `feature/bounded-artifact-http-adapter`](https://github.com/TrentBest/TheSingularityWorkshop.HeadlessAi/tree/feature/bounded-artifact-http-adapter) | [f61990c78a9e228aba564a158a5478d227db4e13](https://github.com/TrentBest/TheSingularityWorkshop.HeadlessAi/commit/f61990c78a9e228aba564a158a5478d227db4e13) | 2026-11-01 06:52:12 | Clarify adapter normalized output contract | 0 ahead / 14 behind development; no PR found. |
| [FSM_COS `policy/sith-branch-activity-audit`](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/tree/policy/sith-branch-activity-audit) | [26fcd53770efdf01cedad8e9d884e0b3e13b8790](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/commit/26fcd53770efdf01cedad8e9d884e0b3e8790) | 2026-11-03 21:05:08 | Record live Sith branch activity and verification audit | Useful AGENTS.md rules preserved in development; old ledger snapshot superseded; [PR #52](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/pull/52) closed unmerged. |
| [FSM_COS `work/ciai-evidence-hardening`](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/tree/work/ciai-evidence-hardening) | [0158272cd516312e41e8ffa6282882325ddde22a](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/commit/0158272cd516312e41e8ffa6282882325ddde22a) | 2026-11-14 09:58:35 | Fail closed on missing or ambiguous CiAi source runs | Superseded by newer development pin/runtime; [PR #53](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/pull/53) closed unmerged. |

MicroBundleRepository's own current development ledger identifies seven deletion-ready refs out of its 13 extras; its six retained branches are `feature/ontology-address-mapping`, `work/microbundle-artifact-discovery`, `docs/visual-readme`, `fix/repository-boundary`, `release-prep/core-1.0.0`, and `release-prep/rest-1.0.0`. See [its branch reconciliation ledger](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleRepository/blob/development/docs/BRANCH_RECONCILIATION.md) for the detailed identity and pagination boundary decisions.

This is an initial, verified deletion-ready set, not the final portfolio batch. Remaining branches with unique work are being grouped into coherent preservation/consolidation candidates. No deletion is authorized or performed here.


### Active consolidation queue — 2026-11-25 UTC

These are existing review lanes, not permission to merge. No PR was merged as part of this audit. The repository policy requires owner authorization before merge; branch deletion remains owner-only.

#### MyVR — one consolidated device/core proposal

- [PR #38: Consolidate Quest 2 Android session lifecycle work](https://github.com/TrentBest/TheSingularityWorkshop.MyVR/pull/38) — head `3000e9669a0dffb91f0c85f66672a3f9ff329759`, 86 commits, 26 changed files, base `development`. GitHub reports mergeable; merge state is unstable because Sith policy fails while extra branches exist.
- Exact-head [MyVR CI](https://github.com/TrentBest/TheSingularityWorkshop.MyVR/actions/runs/45543932911) passed. Exact-head [Quest 2 Android Host](https://github.com/TrentBest/TheSingularityWorkshop.MyVR/actions/runs/45543933036) passed, including Release build, signed APK publication, OpenXR loader verification, and artifact upload. Exact-head [Sith Policy](https://github.com/TrentBest/TheSingularityWorkshop.MyVR/actions/runs/45543922436) failed because the repository still has extra branches.
- PR #38 now includes the portable session core, device-session lifecycle bridge, typed tactical command workflow/tests, Quest 2 Android host, and OpenXR feasibility report. The OpenXR report explicitly says physical-headset behavior is **not** verified.
- [PR #37](https://github.com/TrentBest/TheSingularityWorkshop.MyVR/pull/37) was closed as superseded; its tactical contracts/workflow/tests are included in #38. No source branch was deleted.
- After #38 is merged, the following source branches become deletion candidates because their work is included in the consolidated PR: `feature/myvr-device-session-core`, `feature/myvr-tactical-command-facade`, `feature/myvr-tactical-command-workflow`, `feature/myvr-tactical-execution-lifecycle`, `feature/quest2-android-host-bootstrap`, and `feature/quest2-openxr-host-spike`. The separate `feature/myvr-fsm-foundry-reference-migration` is already in the deletion-ready set above.

#### FSM_Execution — active PR stack

| PR | Head branch / SHA | Base | Scope | Exact-head evidence / state |
|---|---|---|---|---|
| [#50 ADP approval queue](https://github.com/TrentBest/TheSingularityWorkshop.FSM_Execution/pull/50) | `feature/adp-human-approval-queue-dev` / `6b0a0c2c1a915bbd7494f35ff9d56820153ee424` | development | 149 commits, 50 files | [Execution proof passed](https://github.com/TrentBest/TheSingularityWorkshop.FSM_Execution/actions/runs/45340859254); mergeable; Sith check fails on extra refs. |
| [#48 Execution workbench Pages](https://github.com/TrentBest/TheSingularityWorkshop.FSM_Execution/pull/48) | `feature/execution-hub-pages-workbench` / `78e7533ff3fd48b0f9dc29e15b854189853507e4` | development | 16 commits, 4 files | [Pages build passed](https://github.com/TrentBest/TheSingularityWorkshop.FSM_Execution/actions/runs/45328750936) and [execution proof passed](https://github.com/TrentBest/TheSingularityWorkshop.FSM_Execution/actions/runs/45328751109); mergeable; Sith check fails on extra refs. |
| [#40 AgentOnboarding host integration](https://github.com/TrentBest/TheSingularityWorkshop.FSM_Execution/pull/40) | `feature/foundry-agent-onboarding-host-integration` / `3bd2962c9bce66c9b9ba2fa37052002a39196934` | development | 105 commits, 65 files | [Foundry CI passed](https://github.com/TrentBest/TheSingularityWorkshop.FSM_Execution/actions/runs/45015200825); merge conflicts remain. |
| [#44 dependency admission](https://github.com/TrentBest/TheSingularityWorkshop.FSM_Execution/pull/44) | `feature/dependency-admission-dormancy` / `259ef9bcb4477f2339d23ab5215602cdd7a0973c` | PR #40 head branch | 110 commits, 51 files | Mergeable as a stacked PR, but [exact-head CI failed](https://github.com/TrentBest/TheSingularityWorkshop.FSM_Execution/actions/runs/45329662528): pinned CiAi commit `d4d2aa01985cc03b1a102b00f98e4b77eb666ac6` does not match current CiAi development `2ea02e7f3f365ce8fbced48649e13fd6e2c34514`. Reconcile the dependency pin before considering merge. |
| [#26 host repository operation gate](https://github.com/TrentBest/TheSingularityWorkshop.FSM_Execution/pull/26) | `feature/foundry-host-repository-operation-gate` / `42fc3c7bd0645fa3cb35be7d6b7114d3712ce6c7` | development | 286 commits, 53 files | [Foundry CI passed](https://github.com/TrentBest/TheSingularityWorkshop.FSM_Execution/actions/runs/44935120846); merge conflicts remain. |

The #44 branch is intentionally stacked on #40; do not retarget or merge it independently without reconciling the parent PR. The `feature/foundry-agent-onboarding-admission` branch is contained in the later host-integration branch and can become a deletion candidate after #40 is merged. The other FSM_Execution branches still need unique-work review; these five PRs do not account for every extra ref.




## Canonical branch policy

Exactly two persistent branches are intended:

- development — authoritative integration branch and source of truth for the next release.
- master — prior-release baseline until the owner replaces it with the verified development candidate.

Do not merge legacy master into development for release preparation. Do not create new branches or delete branches on the owner's behalf. Branch deletion is an owner decision after any unique work has been reviewed and preserved.

## Live branch activity audit — 2026-11-15 UTC

This snapshot records the branch state at candidate `c4ff13d6521688ecefeb487f1360fce143e54343`, before this documentation refresh commit. It supersedes the older 2026-10-22 inventory for current branch-count and activity reporting; older release evidence remains historical and SHA-scoped.

### Branch activity — newest first

| UTC committer time | Branch | Author | Latest subject | Head SHA |
|---|---|---|---|---|
| 2026-11-15 07:11:24 | [development](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/tree/development) | The Singularity Workshop | fix: align package verification and consumer with .NET 10 | [c4ff13d6521688ecefeb487f1360fce143e54343](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/commit/c4ff13d6521688ecefeb487f1360fce143e54343) |
| 2026-11-14 11:27:04 | [master](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/tree/master) | github-actions[bot] | chore: refresh Sith policy evidence | [b42e505399d156bc9f80f027968f164149f5ba1f](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/commit/b42e505399d156bc9f80f027968f164149f5ba1f) |
| 2026-11-14 09:58:35 | [work/ciai-evidence-hardening](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/tree/work/ciai-evidence-hardening) | The Singularity Workshop | Fail closed on missing or ambiguous CiAi source runs | [0158272cd516312e41e8ffa6282882325ddde22a](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/commit/0158272cd516312e41e8ffa6282882325ddde22a) |
| 2026-11-03 21:05:08 | [policy/sith-branch-activity-audit](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/tree/policy/sith-branch-activity-audit) | The Singularity Workshop | Record live Sith branch activity and verification audit | [26fcd53770efdf01cedad8e9d884e0b3e13b8790](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/commit/26fcd53770efdf01cedad8e9d884e0b3e13b8790) |

### Reconciliation and verification

- Live inventory: four branches — `development`, `master`, `work/ciai-evidence-hardening`, and `policy/sith-branch-activity-audit`. Sith's exact-two-branch policy is **NON-COMPLIANT**.
- `development` is 181 commits ahead of `master`; `master` is 16 commits ahead of `development`; merge base `56ad34dbe4722ba1edf9a4da6a00812eeaa29101`.
- `work/ciai-evidence-hardening` is 2 commits ahead / 26 behind development. Its only changed file is `.github/workflows/observe-commit.yml`; that file's blob SHA is identical on the branch and development (`2cbaaa0ca3946fca9128c7bec281ea9beccc9cc7`). Draft PR [#53](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/pull/53) was closed as superseded, not merged. The branch is a deletion candidate for the owner's eventual batch, subject to a final whole-tree recheck immediately before deletion.
- `policy/sith-branch-activity-audit` is 2 commits ahead / 26 behind development. Its changed files are `AGENTS.md` and `docs/BRANCH_RECONCILIATION.md`. The useful reporting rules were selectively incorporated into development while retaining explicit owner authorization for temporary branches; the branch's 2026-11-03 snapshot was not copied. Draft PR [#52](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/pull/52) was closed as superseded, not merged. The branch is a deletion candidate for the owner's eventual batch, subject to a final whole-tree recheck immediately before deletion.
- Exact-head evidence for candidate `c4ff13d6521688ecefeb487f1360fce143e54343`: package/build/source integration/public-package compatibility and isolated package consumer passed; CodeQL passed; benchmark was pending; CiAi was waiting for that benchmark; Sith failed on the four-branch inventory. See [release portfolio](../RELEASE_READINESS_PORTFOLIO.md) for exact run IDs and package hash.
- `master` remains diverged and has 16 commits of unique history; review those commits before any owner-authorized replacement. Do not merge legacy master into development.
- No PR was merged, no branch was deleted, no branch ref was rewritten, and no NuGet package was published during this audit.

## Current observed state — 2026-10-22

| Branch | Observed head | Disposition |
|---|---|---|
| development | 826b011498942214f8f75c8f2f4406acfa1442a7 | Preserve; active alpha.7 candidate. |
| master | 732690a2b37ac23971b3f9aea1cc595fd68c0a7f | Preserve as prior-release baseline until owner-authorized replacement. |

The live branch inventory at this checkpoint contained exactly these two branches. No branch was deleted or rewritten as part of this record.

## Verification evidence

The [pack-and-publish workflow run 1114](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/actions/runs/39866658509) ran on development candidate 826b0114: build/test, public-package compatibility, and source integration succeeded; NuGet publication was skipped by the hard gate. This evidence belongs to that SHA only and does not authorize publication or branch replacement.

## Reconciliation rules

1. Compare any noncanonical branch's commits and file changes against current development.
2. Inspect actual source, tests, documentation, workflow changes, and related pull-request history.
3. Record work already integrated, unique useful work, conflicts, and obsolete or unsafe material.
4. Port useful work selectively and coherently to development; do not blindly merge stale snapshots.
5. Run relevant verification against the resulting candidate.
6. Tell the owner which branch is ready for manual deletion and why. Never delete it for the owner.

An old inventory or pull-request comment is not proof that a branch still exists or that its work has been reconciled. Refresh live refs and inspect current files before making a disposition decision.

## Release boundary

Branch replacement and NuGet publication are separate actions. Keep the publication job hard-gated by && false unless the owner explicitly authorizes the exact package/version publication attempt.

---
<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
