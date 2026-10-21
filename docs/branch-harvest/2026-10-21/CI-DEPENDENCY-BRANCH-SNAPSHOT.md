# Preserved CI/development-dependency branch snapshots

Verbatim snapshots from `work/ci-development-dependencies`, 2026-10-21. Workflow content is inert text for review only and must never be copied wholesale. Compare publication guards, dependency provenance, permissions, action pinning, and job conditions before adopting any idea.


---

## .github/workflows/package.yml
Source blob: `be993cb16013691fe6c8c69028671c6cccf36180`

```text
name: Pack and publish

on:
  push:
    branches:
      - master
      - development
  pull_request:
  workflow_dispatch:
    inputs:
      publish:
        description: 'Publish the verified package to NuGet.org'
        required: true
        default: false
        type: boolean

permissions:
  contents: read

jobs:
  build-and-test:
    runs-on: ubuntu-24.04
    steps:
      - name: Checkout
        uses: actions/checkout@v7

      - name: Checkout MicroBundleDomain development dependency
        if: ${{ (github.event_name == 'pull_request' && github.base_ref == 'development') || github.ref == 'refs/heads/development' }}
        uses: actions/checkout@v7
        with:
          repository: TrentBest/TheSingularityWorkshop.MicroBundleDomain
          ref: development
          path: _dependencies/MicroBundleDomain

      - name: Setup .NET
        uses: actions/setup-dotnet@v6
        with:
          dotnet-version: 8.0.x

      - name: Pack MicroBundleDomain development dependency
        if: ${{ (github.event_name == 'pull_request' && github.base_ref == 'development') || github.ref == 'refs/heads/development' }}
        run: |
          dotnet restore _dependencies/MicroBundleDomain/TheSingularityWorkshop.MicroBundleDomain.slnx
          dotnet build _dependencies/MicroBundleDomain/TheSingularityWorkshop.MicroBundleDomain.slnx --configuration Release --no-restore
          mkdir -p _dependencies/nuget
          dotnet pack _dependencies/MicroBundleDomain/TheSingularityWorkshop.MicroBundleDomain.csproj --configuration Release --no-build --output _dependencies/nuget

      - name: Configure development dependency feed
        if: ${{ (github.event_name == 'pull_request' && github.base_ref == 'development') || github.ref == 'refs/heads/development' }}
        shell: bash
        run: |
          set -euo pipefail
          mkdir -p .ci
          cat > .ci/NuGet.Development.config <<EOF
          <?xml version="1.0" encoding="utf-8"?>
          <configuration>
            <packageSources>
              <clear />
              <add key="MicroBundleDomainDevelopment" value="$GITHUB_WORKSPACE/_dependencies/nuget" />
              <add key="nuget.org" value="https://api.nuget.org/v3/index.json" />
            </packageSources>
            <packageSourceMapping>
              <packageSource key="MicroBundleDomainDevelopment">
                <package pattern="TheSingularityWorkshop.MicroBundleDomain" />
              </packageSource>
              <packageSource key="nuget.org">
                <package pattern="*" />
              </packageSource>
            </packageSourceMapping>
          </configuration>
          EOF

      - name: Restore using development dependencies
        if: ${{ (github.event_name == 'pull_request' && github.base_ref == 'development') || github.ref == 'refs/heads/development' }}
        run: dotnet restore TheSingularityWorkshop.FSM_COS.sln --configfile .ci/NuGet.Development.config

      - name: Restore using published NuGet dependencies
        if: ${{ !( (github.event_name == 'pull_request' && github.base_ref == 'development') || github.ref == 'refs/heads/development' ) }}
        run: dotnet restore TheSingularityWorkshop.FSM_COS.sln --source https://api.nuget.org/v3/index.json

      - name: Build
        run: dotnet build TheSingularityWorkshop.FSM_COS.sln --configuration Release --no-restore

      - name: Run and verify minimal consumer sample
        shell: bash
        run: |
          set -euo pipefail
          output="$(dotnet run --project samples/FSM_COS.MinimalConsumer/FSM_COS.MinimalConsumer.csproj --configuration Release --no-build)"
          printf '%s\n' "$output"
          grep -Fq "Assembly order: 2 -> 1" <<< "$output"

      - name: Test with coverage
        run: >
          dotnet test tests/FSM_COS.Tests/FSM_COS.Tests.csproj
          --configuration Release
          --no-build
          --verbosity normal
          --collect:"XPlat Code Coverage;Format=cobertura"
          --results-directory ./TestResults
          --logger "trx;LogFileName=testresults.trx"

      - name: Prepare coverage report
        shell: bash
        run: |
          set -euo pipefail
          coverage_file="$(find ./TestResults -type f -name 'coverage.cobertura.xml' -print -quit)"
          if [[ -z "$coverage_file" ]]; then
            echo "No coverage.cobertura.xml file was produced."
            find ./TestResults -maxdepth 4 -type f -print || true
            exit 1
          fi
          cp "$coverage_file" ./TestResults/coverage.cobertura.xml

          # Keep one canonical Cobertura report so Codecov does not ingest the same
          # test coverage multiple times from the nested collector output and root copy.
          find ./TestResults -type f -name 'coverage.cobertura.xml' ! -path './TestResults/coverage.cobertura.xml' -delete
          remaining_coverage_files="$(find ./TestResults -type f -name 'coverage.cobertura.xml' | wc -l)"
          if [[ "$remaining_coverage_files" -ne 1 ]]; then
            echo "Expected exactly one canonical Cobertura report, found $remaining_coverage_files."
            find ./TestResults -type f -name 'coverage.cobertura.xml' -print
            exit 1
          fi
          echo "Prepared canonical coverage report: ./TestResults/coverage.cobertura.xml"

      - name: Upload coverage to Codecov
        uses: codecov/codecov-action@v6
        with:
          token: ${{ secrets.CODECOV_TOKEN }}
          slug: TrentBest/TheSingularityWorkshop.FSM_COS
          files: ./TestResults/coverage.cobertura.xml
          fail_ci_if_error: true

      - name: Pack
        run: dotnet pack src/FSM_COS/FSM_COS.csproj --configuration Release --no-build --output ./artifacts

      - name: Upload NuGet package artifact
        uses: actions/upload-artifact@v7
        with:
          name: fsm-cos-nuget-package
          path: ./artifacts/*.nupkg

      - name: Upload test and coverage artifacts
        uses: actions/upload-artifact@v7
        with:
          name: fsm-cos-test-results
          path: ./TestResults/*

  public-package-compatibility:
    runs-on: ubuntu-24.04
    steps:
      - name: Checkout
        uses: actions/checkout@v7

      - name: Setup .NET
        uses: actions/setup-dotnet@v6
        with:
          dotnet-version: 8.0.x

      # Deliberately do not add the CI-only local MicroBundleDomain feed here.
      # This proves the published dependency contract used by downstream consumers.
      - name: Restore from public NuGet
        run: dotnet restore TheSingularityWorkshop.FSM_COS.sln --source https://api.nuget.org/v3/index.json

      - name: Build against published dependencies
        run: dotnet build TheSingularityWorkshop.FSM_COS.sln --configuration Release --no-restore

      - name: Run and verify minimal consumer against published dependencies
        shell: bash
        run: |
          set -euo pipefail
          output="$(dotnet run --project samples/FSM_COS.MinimalConsumer/FSM_COS.MinimalConsumer.csproj --configuration Release --no-build)"
          printf '%s\n' "$output"
          grep -Fq "Assembly order: 2 -> 1" <<< "$output"

      - name: Test against published dependencies
        run: dotnet test tests/FSM_COS.Tests/FSM_COS.Tests.csproj --configuration Release --no-build --verbosity normal

      - name: Pack against published dependencies
        run: dotnet pack src/FSM_COS/FSM_COS.csproj --configuration Release --no-build --output ./artifacts-public

      - name: Upload public-feed package artifact
        uses: actions/upload-artifact@v7
        with:
          name: fsm-cos-nuget-package-public-feed
          path: ./artifacts-public/*.nupkg

  publish_nuget:
    needs: [build-and-test, public-package-compatibility]
    # Publishing is available only when deliberately dispatched from development
    # with the boolean input checked. Pushes and pull requests never publish.
    if: ${{ github.event_name == 'workflow_dispatch' && inputs.publish == true && github.ref == 'refs/heads/development' && false }}
    runs-on: ubuntu-24.04
    permissions:
      contents: read
      id-token: write

    steps:
      - name: Download NuGet package
        uses: actions/download-artifact@v8
        with:
          name: fsm-cos-nuget-package-public-feed
          path: ./artifacts-public

      - name: Authenticate with NuGet Trusted Publishing
        uses: NuGet/login@v1
        id: login
        with:
          user: TheSingularityWorkshop

      - name: Publish package to NuGet.org
        run: dotnet nuget push "./artifacts-public/*.nupkg" --source "https://api.nuget.org/v3/index.json" --api-key "${{ steps.login.outputs.NUGET_API_KEY }}" --skip-duplicate

```


---

## docs/DEVELOPMENT.md
Source blob: `a9719f0d3fc4499abec7174ed67a3c70f3cd0af4`

```text
# FSM_COS Development

> There are two active branches by design: master and development.

This repository deliberately avoids a forest of simultaneously-current feature branches.

## Branch policy

### master

`master` is the stable baseline.

It should represent code that has been deliberately consolidated and is suitable to treat as the public stable line. Release preparation is performed from a known-good development state; it is not an alternative permanent development branch.

### development

`development` is the current integration and architecture branch.

All normal FSM_COS evolution should happen here unless a short-lived branch is genuinely useful for isolating a risky experiment.

> **If you need to ask which branch is current, the answer should be `development`.**

### Short-lived branches

A temporary branch is acceptable for a substantial architecture experiment, focused documentation overhaul, risky implementation change, or benchmark/investigation that should remain isolated.

When accepted, merge it into `development` and close the temporary PR. Do not allow old branches to become competing definitions of “current.”

## Consolidation record

The repository previously accumulated overlapping branches for the domain-contract correction, staged manifest loading, and release preparation.

The domain-contract correction was merged into `development`.

The staged-manifest and release-preparation PRs were closed as superseded designs. Their branches remain historical references; they are not active development lanes.

This is intentional housekeeping, not deletion of architectural history.

## Development sequence

```text
idea / issue
    ↓
development
    ↓
implementation + tests + documentation
    ↓
CI
    ↓
review
    ↓
development remains current
    ↓
stable checkpoint
    ↓
master
    ↓
explicit release
```

Do not publish a package merely because a development branch builds.

## Documentation standard

FSM_COS documentation should explain four things together:

1. Why the boundary exists.
2. What contract crosses the boundary.
3. How the implementation realizes the contract.
4. What does not belong here.

A document is not finished merely because the code example compiles. It should leave a reader able to answer who owns a concept, who supplies it, who interprets it, where it stops, what happens when an optional piece is absent, and what the stable handoff is.

Architecture diagrams are part of that explanation, not decoration.

## Current architecture direction

```text
MicroBundleDomain ───────── defines the MicroBundle contract ──────┐
                                                                   │
Optional sources / catalogs ─ locate and resolve available bundles ┤
  ├── in-memory or generated catalog                              │
  ├── repository-backed catalog                                   │
  └── host-owned implementation                                   │
                                                                   ▼
                                                               FSM_COS
                                                                   │
                                                        composes requested runtime
                                                                   ▼
                                                            RuntimeAssembly
                                                                   │
                                                                   ▼
                                                            Host / Experience
```

MicroBundleRepository is one possible source behind a catalog, not a mandatory hop in every composition. A small host can use an in-memory catalog; another host can supply repository-backed discovery without changing FSM_COS's composition algorithm.

The Runtime Manifest belongs to the composition request:

```text
Manifest
  ├── MicroBundle identity
  └── requested version
```

Per-MicroBundle configuration is deliberately separate:

```text
Configuration source
  ├── MicroBundle A → configuration
  ├── MicroBundle B → configuration
  └── MicroBundle C → absent → defaults
```

FSM_COS may consume configuration through an abstraction, but it does not become a file reader, serializer, repository, or application configuration framework.

## Dependency modes in CI

The same repository has two deliberately different dependency modes. They answer different questions and must not be blended into one ambiguous build.

| Context | MicroBundleDomain source | What the build proves |
|---|---|---|
| Push to `development` or a pull request targeting it | Checkout the dependency's `development` branch and pack it into an isolated local feed | Current integration code is tested against current dependency development code |
| Push to `master`, or a pull request targeting another base | Restore dependencies from public NuGet | The stable/release line does not silently rely on an unpublished source checkout |
| Public-package compatibility job | Public NuGet only, with no local dependency feed | The documented package dependency graph can restore and build as a consumer would |

For development restores, NuGet package-source mapping routes **only** `TheSingularityWorkshop.MicroBundleDomain` to the locally packed development package. Other package IDs resolve from nuget.org. This matters because the development checkout can carry the same declared package version as the published package: adding a local feed without source mapping would not prove which copy restore selected.

The public-package compatibility job remains separate from the development integration build. A green development build is not evidence that a new package version is published, and a green public-feed build is not evidence that the latest dependency development code has been integrated.

This workflow is verification, not release authorization. Publication remains behind the existing explicit workflow-dispatch input and disabled safety gate; do not remove that gate or publish as part of routine development.

## Verification

Run the minimal consumer sample as part of validating the public composition path:

```bash
dotnet run --project samples/FSM_COS.MinimalConsumer/FSM_COS.MinimalConsumer.csproj
```

It should load bundle 2 before bundle 1 and print `Assembly order: 2 -> 1`. The sample references the source project and is a source-level integration proof, not proof that a particular NuGet version is published or installable.

Before a development change is considered complete:

- production code builds with zero warnings;
- tests pass;
- XML documentation remains coherent;
- README and architecture docs describe the actual code;
- package dependencies point at published/staged versions intentionally;
- examples do not reference removed contracts;
- branch purpose is obvious from GitHub;
- no accidental publication occurs.

## Release discipline

A release is a separate decision from development:

```text
development
    ↓
stable documentation
    ↓
green CI
    ↓
explicit approval
    ↓
merge to master
    ↓
explicit publication
```

No automatic branch state should be interpreted as permission to publish a NuGet package.

---

<p align="center"><em>The Singularity Workshop — Tools for the curious, the bold, and the systemically inclined.</em><br><strong>Because state shouldn't be a mess.</strong><br><em>And because static boundaries are invitations to cause trouble.</em></p>

```
