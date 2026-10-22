<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# FSM_COS Branch Reconciliation — Sith Policy

**Audience:** maintainers only. This is not product documentation.

**Policy:** the intended persistent branches are exactly `master` and `development`. Preserve useful work before branch removal. Do not force-reset or merge divergent histories merely to make branch counts look tidy. Branch deletion is an owner action after reviewing this ledger.

**Audit date:** 2026-10-22 (21:10 UTC). Live branch refs, both comparison directions, and current release workflow evidence were queried.

## Persistent branches

| Branch | Observed head | Role | Action |
|---|---|---|---|
| `master` | `732690a2b37ac23971b3f9aea1cc595fd68c0a7f` | Published/release line | Preserve. Its release workflow and public-package compatibility protections must not be overwritten casually. |
| `development` | `75e6b169affbb4c16ce993ff0d90012cbcc1e54c` | Active integration and documentation line | Preserve. This is the current validated development candidate; its CI evidence is recorded below and does not authorize merge or publication. |

The live branch inventory returned **exactly two branches: `master` and `development`**. The previously tracked `work/alpha7-educational-docs` branch is no longer present in the live branch listing. No branch deletion was performed by this audit.

## Current divergence and release integration

Draft PR #35 (`development` → `master`) remains open and should remain draft while the histories are reconciled. The current comparison reports **904 commits ahead and 48 behind** for development relative to master. This is substantial divergence, not a routine merge. The merge base is `e9c9968b9cdbec0478e77d9f89b4e74a0e404d3d`.

The current `master` head is `732690a2b37ac23971b3f9aea1cc595fd68c0a7f`; the current `development` head is `75e6b169affbb4c16ce993ff0d90012cbcc1e54c`. Reconcile source/API/test differences, master-only public-dependency compatibility checks, release history, repository documentation, and workflow configuration deliberately. Preserve the master-only publication safeguard. Do not force-reset either persistent branch.

## Current candidate evidence (development HEAD)

Candidate SHA: `75e6b169affbb4c16ce993ff0d90012cbcc1e54c`.

- Pack-and-publish workflow [39854327909](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/actions/runs/39854327909): build/test, public-package compatibility, and source-integration jobs succeeded; `publish_nuget` was skipped.
- CodeQL [39854327925](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/actions/runs/39854327925): succeeded on the same SHA.
- Primary package artifact: `fsm-cos-nuget-package`, 127,994-byte artifact ZIP, SHA-256 `bf00c1e6864eb511642bca732d4b2921606fd1b61676f4a49f4aac03ced02d55`. Contained `TheSingularityWorkshop.FSM_COS.0.1.0-alpha.7.nupkg`, SHA-256 `69b762b3170b3f04c95dcdf7f745319fa034c0b1281dd6954f4d4692fb423b9f`.
- Public-feed compatibility package artifact: `fsm-cos-nuget-package-public-feed`, SHA-256 `b1c831ae767a079b48ce2b463959f87c9cde5567597cc7579a9656f1722c0692`. Contained the same package ID/version, SHA-256 `ddf8054d56ad9b411465064e180cb3b7e9b8fed4c1482677069d39e785bf4a7c`.
- Inspected both .nupkg archives: package ID `TheSingularityWorkshop.FSM_COS`, version `0.1.0-alpha.7`, .NET 8 assembly and XML docs present, package README and license present, expected three public dependencies present, alpha.7 release notes and consumer docs/assets included, and internal release-readiness/branch-ledger notes excluded.
- The package workflow run at the immediately preceding source candidate `8dbe9207d811678aac4d2efd54d9d7e87eeea802` also passed all three validation jobs and CodeQL, with publication skipped. The current `75e6...` run is the fresher evidence and its package artifact was inspected directly.

This is strong evidence for the current **development** candidate, not certification of a final **master** release commit. Hosted rendering and browser accessibility-tree, keyboard, and screen-reader behavior remain unverified unless independently checked. A final master candidate must be selected and checked against its exact SHA after branch-history and workflow reconciliation.

## Owner checklist

- [x] Confirm the live persistent branch inventory is exactly `master` and `development`.
- [x] Refresh divergence numbers from live refs: development 904 ahead / 48 behind master.
- [x] Re-run pack/test, public-package compatibility, source integration, and CodeQL on the same current development SHA.
- [x] Inspect both current-head NuGet artifacts and record package metadata and hashes.
- [ ] Reconcile the 904-ahead / 48-behind histories and preserve all master-only release safeguards.
- [ ] Verify hosted SVG rendering and applicable browser accessibility behavior.
- [ ] Select the exact final master candidate and repeat all applicable gates and package inspection on that SHA.
- [ ] Obtain explicit owner authorization before publication.

This ledger records observed state, not merge approval or release authorization.

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
