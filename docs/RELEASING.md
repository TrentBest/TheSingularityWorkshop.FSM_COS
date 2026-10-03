# Releasing FSM_COS to NuGet

## Purpose

This document is the release runbook for publishing the FSM_COS NuGet package. It exists so a future maintainer or AI agent does not need to reverse-engineer the GitHub Actions release path.

## Current release

- Package: `TheSingularityWorkshop.FSM_COS`
- Release: `0.1.0-alpha.5`
- Repository: `TrentBest/TheSingularityWorkshop.FSM_COS`
- Workflow: `.github/workflows/package.yml`
- NuGet publishing uses **NuGet Trusted Publishing** through `NuGet/login@v1`.
- The workflow has `id-token: write` only on the publishing job.

## Normal release path

The preferred path is:

1. Ensure the intended release commit is on `master`.
2. Run the **Pack and publish** workflow manually with `publish=true`.
3. The workflow first restores, builds, tests with coverage, uploads coverage, and packs the package.
4. Only after that job succeeds does `publish_nuget` download the exact generated artifact.
5. `NuGet/login@v1` authenticates the trusted publisher identity.
6. `dotnet nuget push` publishes the package to NuGet.org with `--skip-duplicate`.

## Why the workflow also has a release-commit trigger

The GitHub integration used by automated maintainers may be able to inspect and modify repositories but may not expose GitHub's **workflow_dispatch** API.

To make release automation deterministic without weakening ordinary CI, this repository also permits exactly one release commit to trigger the publishing job:

`release: publish FSM_COS 0.1.0-alpha.5`

The publish job checks all of these conditions:

- event is a push;
- branch is `master`;
- the complete head commit message is the release message above.

This is intentionally narrow. Ordinary pushes continue to run build/test/pack but do not publish.

## Important release safety rules

- Do **not** publish by adding a NuGet API key to repository secrets when Trusted Publishing is available.
- Do **not** make every push publish to NuGet.
- Do **not** remove the build/test dependency from `publish_nuget`.
- Do **not** bypass coverage or packaging validation to force a release.
- Do **not** publish a package version that does not match the project/package metadata.
- Do not claim a release is published until the publishing job succeeds and the package is visible on NuGet.org.
- A future alpha/beta/stable release should update this release-specific trigger and this document rather than silently broadening the trigger.

## For an AI maintainer

If you are asked to publish FSM_COS and workflow dispatch is unavailable:

1. Inspect `.github/workflows/package.yml`.
2. Verify the package version in `src/FSM_COS/FSM_COS.csproj`.
3. Verify the normal build/test/pack path is intact.
4. Update the release-specific commit message and matching workflow condition for the exact version being released.
5. Commit that release marker to `master`.
6. Wait for the resulting **Pack and publish** run.
7. Verify both `build-and-test` and `publish_nuget` succeeded.
8. Verify the package/version on NuGet.org.
9. Record the resulting commit and workflow run in the release notes or PR.

The release mechanism is deliberately explicit: **ordinary development proves the package; an exact release action authorizes publication.**
