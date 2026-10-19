# Archived Branch Reconciliation Ledger

> **Archive status:** point-in-time branch inventory from October 9, 2026. The branch names, ahead/behind counts, pull-request states, and proposed dispositions below were observations at that date; they are not a current repository inventory or deletion instruction.

## Current authorities

- [Development and CI modes](DEVELOPMENT.md) — current branch policy.
- [Alpha.7 release-readiness gate](0.1.0-alpha.7-READINESS.md) — exact-candidate release evidence.
- [Domain completion roadmap](DOMAIN_COMPLETION_ROADMAP.md) — kernel ownership and deferred capabilities.
- [Documentation index](../DOCUMENTATION_INDEX.md) — primary document for each concern.

## Durable reconciliation rules

- Keep `master` as the stable line and `development` as the integration line unless the owner directs otherwise.
- Preserve useful source, tests, and documentation; do not merge divergent histories wholesale merely to simplify the branch list.
- Do not delete branches, close useful pull requests, or overwrite history without explicit owner direction.
- A branch's commit count, successful build, or source diff does not establish that its unique behavior is still needed or that a release is ready.
- Verify current branch heads and exact-commit workflows before acting. Historical run links prove only the commits they actually tested.
- NuGet publication remains a separate action requiring explicit owner authorization.

## Historical inventory

The original inventory and detailed branch-by-branch rationale remain in repository history. It was intentionally removed from this working document because its dated counts and instructions were repeatedly mistaken for current state. Rebuild an inventory from live GitHub refs only when a current cross-repository branch reconciliation is explicitly in scope.
