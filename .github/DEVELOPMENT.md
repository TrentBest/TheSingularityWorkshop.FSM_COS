<!-- The Singularity Workshop moniker -->
<p align="center">
  <a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop — animated workshop moniker" width="720" /></a>
</p>

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

---

## 🔗 Resources & Support

- **Project:** [FSM_COS](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS)
- **Workshop projects:** [FSM_API](https://github.com/TrentBest/FSM_API) · [FSM_Serialization](https://github.com/TrentBest/TheSingularityWorkshop.FSM_Serialization) · [WebPage](https://github.com/TrentBest/WebPage)
- **Connect:** [GitHub](https://github.com/TrentBest) · [Coder Legion profile](https://coderlegion.com/user/The%2BSingularity%2BWorkshop) · [Coder Legion community](https://coderlegion.com/)
- **Support:** [Patreon](https://www.patreon.com/c/TheSingularityWorkshop) · [PayPal](https://www.paypal.com/donate/?hosted_button_id=3Z7263LCQMV9J)

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
