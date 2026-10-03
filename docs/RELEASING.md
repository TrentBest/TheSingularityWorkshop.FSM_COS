# NuGet Publication Runbook

This is the Workshop's reference note for NuGet publication. The same safety pattern is used across the NuGet-facing repositories.

## Safe default

**Nothing publishes to NuGet during ordinary development.**

Every NuGet publishing job ends its condition with:

```yaml
&& false
```

That final `false` is an intentional hard gate. It remains in `master` after a release.

The workflows may still:

- restore;
- build;
- test;
- collect coverage;
- pack;
- upload package artifacts.

The publication job is simply skipped.

## Deliberate publication

When a package is ready to publish:

1. Confirm the package version in the project metadata.
2. Confirm build, tests, coverage, and packaging are clean.
3. On `master`, change **only the final publication gate** from:
   ```yaml
   && false
   ```
   to:
   ```yaml
   && true
   ```
4. Push that deliberate release commit.
5. Let the normal verification job run first.
6. Let the publication job authenticate with **NuGet Trusted Publishing** using `NuGet/login@v1`.
7. Verify the package publication on NuGet.org.
8. **Immediately restore the publication gate to `&& false` and push that restoration commit.**

The release authorization is therefore visible in Git history and temporary by design.

## Why we do this

A package workflow should be able to run continuously without silently publishing a new package because someone merged code into `master`.

The invariant is:

```text
development
   |
   +--> build / test / pack
   |       |
   |       +--> artifacts
   |
   +--> publish job
           |
           +--> && false
                   |
                   v
                 STOP
```

Publication is an explicit release operation, not a side effect of development.

## NuGet Trusted Publishing

The publishing job should use NuGet Trusted Publishing rather than a long-lived NuGet API key when the repository is configured for it.

The expected pattern is:

```text
GitHub Actions
     |
     | OIDC identity
     v
NuGet/login@v1
     |
     | short-lived NuGet credential
     v
NuGet.org
```

The publishing job therefore needs:

```yaml
permissions:
  contents: read
  id-token: write
```

The NuGet Trusted Publishing policy must identify the correct GitHub owner, repository, workflow, and package scope.

## AI packages

The same publication mechanism applies to the Workshop's AI packages:

- `TheSingularityWorkshop.ProtocolAi`
- `TheSingularityWorkshop.GrammarAi`

The AI packages are **not an exception** to the release safety rule.

Their workflows:

1. build and test the package;
2. collect coverage;
3. pack the NuGet artifact;
4. upload the artifact;
5. keep `publish_nuget` behind `&& false` on `master);
6. use `NuGet/login@v1) when publication is deliberately enabled;
7. publish with `dotnet nuget push ... --skip-duplicate`;
8. return to the hard-gated state immediately after release.

The AI workflow is therefore:

```text
ProtocolAI / GrammarAI source
          |
          v
     build + test
          |
          v
       pack .nupkg
          |
          v
   publication gate
      && false
          |
          v
        STOP

For a release:
      && false
          |
      temporary
      && true
          |
          v
   NuGet Trusted Publishing
          |
          v
       NuGet.org
          |
          v
    restore && false
```

ProtocolAI and GrammarAI also use the architectural split documented in their theory documents:

```text
ProtocolAI = WHAT
GrammarAI  = HOW
Host       = POLICY + EXECUTION
```

That architectural distinction is independent of the package-publication mechanism.

## AI maintainer checklist

If an AI agent is asked to publish one of the Workshop's NuGet packages:

1. Inspect the repository's current workflow on `master).
2. Confirm the package version and intended release.
3. Confirm the publication job is currently hard-gated by `&& false`.
4. Do not publish while that gate is false.
5. For an authorized release, change only that final `false` to `true`.
6. Push the release commit and monitor the verification/publication jobs.
7. Confirm the exact package/version result.
8. Restore the final gate to `&& false`.
9. Push the restoration commit.
10. Leave `master` safe for ordinary future development.

**Never leave `master` with NuGet publication enabled after a release.**

## Current FSM_COS release

- Package: `TheSingularityWorkshop.FSM_COS`
- Current published release: `0.1.0-alpha.5`
- Workflow: `.github/workflows/package.yml`
- Publication: NuGet Trusted Publishing
- Current state: **publication hard-gated**

The release-specific trigger that was used for alpha.5 is historical. Future releases should use the explicit temporary gate pattern above rather than leaving a release-specific publishing condition permanently enabled.
