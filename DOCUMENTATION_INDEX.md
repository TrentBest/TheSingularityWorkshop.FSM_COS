# FSM_COS Documentation Index

Use this page to find the right level of explanation. The README is the orientation page; the documents below provide focused authority for specific topics.

## Choose a path

| If you want to... | Start here |
|---|---|
| Understand the purpose and boundary of FSM_COS | [README](../README.md) |
| Understand the architecture and component ownership | [Architecture](ARCHITECTURE.md) |
| Learn the conceptual model and rationale | [FSM_COS Theory](THEORY.md) |
| Understand the request that enters the kernel | [Runtime Manifest](RUNTIME_MANIFEST.md), then [Manifest Theory](MANIFEST_THEORY.md) |
| Understand the object handed to a host | [RuntimeAssembly](RUNTIME_ASSEMBLY.md) |
| Understand what stays outside the kernel | [Runtime Boundary](RUNTIME_BOUNDARY.md) |
| Learn how MicroBundles are resolved and consumed | [Consuming MicroBundles](CONSUMING_MICROBUNDLES.md) |
| Understand MicroBundle arbitration and convergence | [Arbitration](ARBITRATION.md) |
| Explore AI-oriented composition | [AI Composition](AI_COMPOSITION.md) |
| Understand the domain FSM_COS owns | [Domain](DOMAIN.md) |
| Integrate with WebPage | [WebPage Integration](WEBPAGE_INTEGRATION.md) |
| Contribute, build, and verify changes | [Development](DEVELOPMENT.md) |

## Recommended reading order

For a first encounter:

1. [README](../README.md) — identity, responsibility boundary, and quick orientation.
2. [Architecture](ARCHITECTURE.md) — how the pieces fit together and which layer owns each concern.
3. [FSM_COS Theory](THEORY.md) — why composition is a distinct operation.
4. [Runtime Manifest](RUNTIME_MANIFEST.md) — what the caller requests.
5. [RuntimeAssembly](RUNTIME_ASSEMBLY.md) — what the host receives.
6. [Runtime Boundary](RUNTIME_BOUNDARY.md) — where FSM_COS hands off responsibility.
7. The relevant integration or development guide for the task at hand.

Experienced readers can jump directly to the contract or integration guide they need.

## Documentation authority

- The README is the public front door, not the exhaustive specification.
- Architecture and theory documents explain ownership and rationale.
- Focused contract guides describe the manifest, assembly, dependency, loading, and arbitration boundaries.
- Source code and tests determine the behavior of the current implementation. If prose and implementation disagree, record and resolve the discrepancy rather than silently assuming the prose is correct.
- The repository-wide documentation rules live in [Documentation Standard](../DOCUMENTATION_STANDARD.md).

## Status discipline

FSM_COS is evolving. Read any version or status statement in context: a design target is not automatically implemented, and code present on a development branch is not automatically part of a published NuGet release.

---

<p align="center"><em>The Singularity Workshop — Tools for the curious, the bold, and the systemically inclined.</em><br><strong>Because state shouldn't be a mess.</strong><br><em>And because static boundaries are invitations to cause trouble.</em></p>
