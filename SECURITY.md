# Security posture and adversarial test plan

FSM_COS is a composition kernel that invokes host-supplied catalog, configuration-provider, and MicroBundle code. This document separates guarantees enforced by FSM_COS from guarantees that belong to the host and deployment.

## Security boundary

FSM_COS validates identity, dependency declarations, graph consistency, and optional published schedule metadata before invoking any bundle's `Load` method. It snapshots the root/load-plan collections, resolved bundle instances, dependency request structures, and configuration bytes used during loading. Graph traversal is iterative and rejects manifests above the configured bundle and dependency-request limits (defaults: 10,000 bundles, 100,000 dependency requests, and 64 MiB of copied configuration data per composition); arbitration is bounded by the configured round limit.

The `latest-all` .NET analyzer gate treats all unsuppressed runtime-assembly warnings as errors. `CA1707` is intentionally suppressed because the published `FSM_COS` assembly and namespace names are compatibility contracts; do not rename them solely to satisfy a style rule. Other analyzer warnings are fixed or reviewed explicitly, not blanket-suppressed.

**FSM_COS is not a sandbox.** A catalog, configuration provider, `Load`, or `Arbitrate` implementation executes in-process with the process's permissions. Malicious code can access files, network, environment variables, other process memory available to it, or mutate shared state if the host grants those capabilities. Use process/OS isolation, least-privilege identities, containers, or virtualization when executing code that is not trusted. Do not treat a MicroBundle descriptor, hash string, or manifest as proof that executable code is trustworthy.

Composition is not transactional. If a bundle mutates external state and then throws, or an earlier dependency succeeds before a later bundle fails, FSM_COS cannot generically roll back those side effects. Hosts should own lifecycle cleanup, authorization, and audit logging.

## Adversarial verification matrix

| Attack / failure class | FSM_COS control | Evidence expected |
| --- | --- | --- |
| Invalid, zero, conflicting, or mismatched identities/versions | Reject before loading | Unit tests for malformed roots, catalog mismatch, descriptor mismatch, and dependency contract mismatch |
| Missing dependency or catalog exception | Resolve the complete closure before loading | Assert no bundle `Load` callback runs |
| Dependency cycle, self-cycle, duplicate edge/request | Reject during graph/schedule validation | Cycle, duplicate, and malformed-graph tests |
| Catalog time-of-check/time-of-use swap | Retain resolved instances; don't re-resolve during load | Adversarial catalog test with lookup count and object identity assertions |
| Mutating caller-owned root/load-plan/schedule/configuration buffers | Snapshot mutable inputs used by execution | Mutation-after-validation and mutation-during-callback tests |
| Deep graph / recursive stack exhaustion | Iterative graph validation and installation | Deep-chain stress tests |
| Non-convergent or throwing arbitration | Bounded rounds; propagate original exception; no assembly handoff | Non-convergence and callback-failure tests |
| Partial side effects from `Load`/`Arbitrate` failure | Explicitly no rollback guarantee | Failure tests and host cleanup contract |
| Malformed package or accidental GUI dependency | Package metadata/content verifier and runtime assembly boundary test | Verify exact candidate package, dependency list, and assembly references |
| Vulnerable transitive dependencies / unsafe code patterns | Must be checked by dependency audit and static analysis in CI | Record the tool, database date, findings, and disposition; a clean unit-test run is not evidence of a clean security scan |
| Mutable third-party GitHub Action tags | Pin each action to a verified full commit SHA; update pins deliberately and rerun workflows | Workflow source review plus successful exact-candidate CI |
| Untrusted bundle code / privilege abuse | Outside the in-process kernel's security boundary | Host isolation and least privilege; do not claim sandboxing |
| Resource exhaustion from extremely large manifests/graphs/configuration or pathological callbacks | Bounded enumeration for roots, schedules, and load plans; configurable bundle, dependency-request, and configuration-byte limits; iterative traversal. Arbitrary callbacks still need host execution policy | Tests for configured limits and deep graphs; host-level timeout/cancellation remains necessary |

## Release security checklist

Before treating a candidate as security-reviewed:

- [x] Run the exact candidate's full test suite and retain its TRX and coverage artifacts.
- [x] Confirm the current CI dependency audit reports no vulnerable direct or transitive NuGet packages; investigate any advisory rather than silently ignoring it.
- [x] Confirm the `latest-all` .NET analyzer job passes with warnings treated as errors, and review the documented `CA1707` naming suppression.
- [x] Review the CodeQL C# scan for new high/critical findings; the retained SARIF artifact for the candidate contains zero results.
- [x] Verify package identity, version, dependencies, file inventory, and that GUI.SVG is not a runtime dependency.
- [x] Inspect the exact packed assembly and XML API documentation, not just the source tree.
- [x] Pin third-party GitHub Actions to verified full commit SHAs; record the upstream major version beside each pin.
- [x] Confirm the workflow's publish gate is disabled unless explicitly authorized.
- [x] Verify generated SVG assets against the pinned GUI.SVG generator and inspect that they describe current implementation behavior.
- [x] Review the remaining limitations above, especially in-process trust and non-transactional side effects.

The CodeQL analysis workflow completed successfully for the candidate and uploaded SARIF results. The retained SARIF artifact was inspected and contains zero results; a successful analysis run alone would not have established that.

## External baselines

Use these as evolving review checklists, not as a claim that every category applies to this library:

- [OWASP Top 10:2025](https://top10.owasp.org/2025/) — access control, misconfiguration, supply chain, cryptography, injection, design, authentication, integrity, logging, and exceptional conditions.
- [MITRE CWE Top 25:2025](https://cwe.mitre.org/top25/) — recurring dangerous implementation weaknesses including input validation, null handling, resource limits, code injection, and unsafe deserialization.
- [Microsoft .NET security code-analysis rules](https://learn.microsoft.com/en-us/dotnet/fundamentals/code-analysis/quality-rules/security-warnings).
- [Microsoft secure coding guidelines for .NET](https://learn.microsoft.com/en-us/dotnet/standard/security/secure-coding-guidelines).

The relevant attack surface for this package is narrower than a web application, so apply each baseline to the actual code and boundaries instead of adding irrelevant web-specific controls to the kernel.

## Reporting a vulnerability

Please report suspected vulnerabilities privately to the repository maintainer rather than publishing an exploit before a fix is available. Do not include secrets, production data, or credentials in public issues.
