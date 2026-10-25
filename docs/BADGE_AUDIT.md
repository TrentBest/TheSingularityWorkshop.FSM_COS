# README Badge Audit — Initial Evidence Ledger

**Audit date:** 2026-10-25 UTC  
**Repository reviewed:** `TrentBest/TheSingularityWorkshop.FSM_COS`  
**Reviewed ref:** `master` (default branch)  
**Scope:** README badge definitions compared with repository metadata and workflow configuration. This is an initial manual audit, not a complete HTTP/image-render validation and not a Workshop-wide result.

## Summary

| Badge | Status | Evidence | Next action |
|---|---|---|---|
| GitHub Actions / CI | **Healthy configuration; presentation misordered** | `.github/workflows/package.yml` exists on both `master` and `development`. Its push trigger includes both branches and it runs build/test, Codecov upload, package verification, public-feed compatibility, and source-integration lanes. README image URL names `package.yml` and branch `development`, which match the workflow and trigger. | Keep badge; move to first position in the row when a safe README update is made. A live rendered image/status check was not performed here. |
| NuGet version | **Unverified** | README points to package ID `TheSingularityWorkshop.FSM_COS`. Source project declares `0.1.0-alpha.7`, but the project file is not evidence that this version (or any version) has been published. NuGet lookup in this audit did not provide authoritative package-page evidence. | Verify the exact package page and published versions before retaining as a confirmed published-package badge. If no package is published, omit both NuGet badges. |
| NuGet downloads | **Unverified** | Same package ID and publication uncertainty as the version badge. | Validate together with the version badge; omit both if no published package exists. |
| Codecov coverage | **Configured; live badge status unverified** | `package.yml` runs tests with Cobertura coverage, normalizes the output to `TestResults/coverage.cobertura.xml`, and invokes `codecov/codecov-action` with slug `TrentBest/TheSingularityWorkshop.FSM_COS` and `fail_ci_if_error: true`. | Keep provisionally; verify that Codecov has received coverage and that the badge image resolves. Configuration alone does not prove recent upload or visible badge health. |
| Last commit | **Missing (policy gap, not broken badge)** | README has no last-commit badge. | Add only if useful and after the core badge evidence is complete. |
| License | **License confirmed; link target should be improved** | GitHub repository metadata reports the MIT license. Root file is named `LICENSE.txt`; README badge links to the generic MIT page at opensource.org rather than this repository's actual license text. | Link the badge to `LICENSE.txt` in this repository. This is a destination-quality issue, not evidence that the MIT claim is false. |

## Presentation finding

The current README row is ordered **License → NuGet version → NuGet downloads → CI → Codecov**, which conflicts with the Workshop standard in `DOCUMENTATION_STANDARD.md`. The standard order is **CI → NuGet version → NuGet downloads → Codecov → Last commit → License**, omitting inapplicable/unverified items rather than filling slots.

No README badge has been removed or altered as part of this initial evidence pass. In particular, the NuGet badges are **unverified**, not declared broken, until the public package state is confirmed. A provider/network fetch failure is not sufficient evidence of a broken badge.

## Evidence checked

- README on `master`: https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/master/README.md
- Workflow on `master`: https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/master/.github/workflows/package.yml
- Workflow on `development`: https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/.github/workflows/package.yml
- Project package metadata: https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/master/src/FSM_COS/FSM_COS.csproj
- License text: https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/master/LICENSE.txt
- Canonical policy: https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/master/DOCUMENTATION_STANDARD.md#readme-badge-policy
- Tracking issue: https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/issues/37

## Next steps

1. Verify the package page through an authoritative NuGet response, then classify both package badges together.
2. Check Codecov's live project/badge state and actual badge image responses; record transient failures as unverified.
3. Check the moniker and footer images separately; they are image assets but not status badges.
4. Turn this ledger into the first machine-readable audit fixture and tests for badge parsing, canonical order, applicability, and uncertainty.
5. Extend the evidence ledger to other Workshop repositories before changing their public README rows.

## Limitations

This audit did not establish rendered badge image availability from every provider, a fresh Codecov upload timestamp, or the existence of a published FSM_COS package. It therefore makes no claim that all current badge URLs are healthy. No publishing, release-gate change, branch deletion, or destructive action was performed.
