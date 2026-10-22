# FSM_COS Repository Development Guide

This is a maintainer guide. It is not part of the end-user documentation or NuGet package.

## Branches and integration

- `development` is the working integration branch.
- `master` is the stable baseline.
- Do not create additional long-lived branches or delete owner-managed branches without explicit authorization.
- A pull request or branch is not integrated until its changes are present on `development`.

## Build and test

The package targets .NET 8. From the repository root:

```bash
dotnet restore TheSingularityWorkshop.FSM_COS.sln
dotnet test tests/FSM_COS.Tests/FSM_COS.Tests.csproj --configuration Release
dotnet run --project samples/FSM_COS.MinimalConsumer/FSM_COS.MinimalConsumer.csproj
```

The minimal consumer demonstrates dependency-first loading. It uses example bundles and an in-memory catalog supplied by the sample, not built-in FSM_COS implementations.

## Verification

The package workflow verifies build/test results, source-integration mode, public-feed compatibility, NuGet package metadata and contents, local Markdown links, and SVG contract/accessibility attributes. See `.github/workflows/package.yml` and `scripts/verify_nuget_package.py`. CodeQL runs separately through `.github/workflows/codeql.yml`.

A successful run is evidence for its exact commit and artifact only. Any later change requires verification of the new candidate.

## Release safety

NuGet publication requires the owner's explicit authorization. Routine pushes and pull requests must never publish. The publish job is currently guarded off; do not remove or weaken that guard, dispatch a publish workflow, or publish a package unless the owner explicitly authorizes it.
