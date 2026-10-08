# FSM_COS Documentation Index

Use this page to find the right level of explanation. The README is the orientation page; the documents below provide focused authority for specific topics.

## Choose a path

| If you want to... | Start here |
|---|---|
| Understand the purpose and boundary of FSM_COS | [README](README.md) |
| Understand how Workshop packages fit together, including package-versus-MicroBundle decisions and Profiles | [Ecosystem Integration Map](docs/ECOSYSTEM_INTEGRATION_MAP.md) |
| Understand the architecture and component ownership | [Architecture](docs/ARCHITECTURE.md) |
| Learn the conceptual model and rationale | [FSM_COS Theory](docs/THEORY.md) |
| Understand the request that enters the kernel | [Runtime Manifest](docs/RUNTIME_MANIFEST.md), then [Manifest Theory](docs/MANIFEST_THEORY.md) |
| Understand the object handed to a host | [RuntimeAssembly](docs/RUNTIME_ASSEMBLY.md) |
| Understand what stays outside the kernel | [Runtime Boundary](docs/RUNTIME_BOUNDARY.md) |
| Learn how MicroBundles are resolved and consumed | [Consuming MicroBundles](docs/CONSUMING_MICROBUNDLES.md) |
| Understand MicroBundle arbitration and convergence | [Arbitration](docs/ARBITRATION.md) |
| Explore AI-oriented composition | [AI Composition](docs/AI_COMPOSITION.md) |
| Understand the domain FSM_COS owns | [Domain](docs/DOMAIN.md) |
| Integrate with WebPage | [WebPage Integration](docs/WEBPAGE_INTEGRATION.md) |
| Contribute, build, and verify changes | [Development](docs/DEVELOPMENT.md) |

## Recommended reading order

For a first encounter:

1. [README](README.md) — identity, responsibility boundary, and quick orientation.
2. [Architecture](docs/ARCHITECTURE.md) — how the pieces fit together and which layer owns each concern.
3. [Ecosystem Integration Map](docs/ECOSYSTEM_INTEGRATION_MAP.md) — package responsibilities, integration gaps, and MicroBundle candidates.
4. [FSM_COS Theory](docs/THEORY.md) — why composition is a distinct operation.
5. [Runtime Manifest](docs/RUNTIME_MANIFEST.md) — what the caller requests.
6. [RuntimeAssembly](docs/RUNTIME_ASSEMBLY.md) — what the host receives.
7. [Runtime Boundary](docs/RUNTIME_BOUNDARY.md) — where FSM_COS hands off responsibility.
8. The relevant integration or development guide for the task at hand.

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
