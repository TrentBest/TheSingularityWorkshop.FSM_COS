# Archived Dependency Alignment Checklist

> **Archive status:** cross-repository package/source inventory captured on October 8, 2026. Package pins, project versions, branch names, and proposed migrations in the original checklist are historical observations. Do not use them as current dependency facts or a release plan.

## Current authorities for FSM_COS alpha.7

- [Alpha.7 release-readiness gate](0.1.0-alpha.7-READINESS.md) — exact candidate SHA, CI, package, and documentation evidence.
- [Development and CI modes](DEVELOPMENT.md) — current source-versus-public-package verification modes.
- [Domain completion roadmap](DOMAIN_COMPLETION_ROADMAP.md) — direct dependency boundary and kernel responsibilities.
- [FSM_COS project file](../src/FSM_COS/FSM_COS.csproj) — declared target framework, version, and direct package references.
- [Documentation index](../DOCUMENTATION_INDEX.md) — primary document for each topic.

## Durable dependency rules

- Distinguish a project-file version declaration, an available public NuGet version, a consumer's pinned version, and the exact package artifact produced by a workflow.
- Do not infer package availability or API compatibility from source metadata alone; use clean restore/build/test evidence against the intended package feed.
- Verify dependencies at the public package boundary as well as against source checkouts when both modes are supported.
- Keep FSM_COS platform-neutral. GUI, browser, rendering, storage, REST, repository transport, and application lifecycle remain outside the kernel unless a concrete, tested shared contract proves otherwise.
- Lower-level packages must not acquire dependencies on FSM_COS or other higher-level hosts.
- Do not publish or change consumer pins as a side effect of an audit. Consumer migrations require their own scoped change and verification.

## Historical inventory

The original cross-repository matrix and branch comparisons remain in repository history. They were removed from this working document because their dated alpha.5/alpha.6 references and former `&& false` workflow descriptions no longer match the current alpha.7 candidate workflow. Rebuild a fresh matrix from live project files, package feeds, and tests when a specific consumer migration is authorized.
