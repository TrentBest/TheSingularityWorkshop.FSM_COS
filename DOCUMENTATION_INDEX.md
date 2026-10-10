# FSM_COS Documentation Index

Use this page to find the right level of explanation. The README is the orientation page; the documents below provide focused authority for specific topics.

## Choose a path

| If you want to... | Start here |
|---|---|
| Understand the purpose and boundary of FSM_COS | [README](README.md) |
| Start with no programming experience | [What Is FSM_COS?](docs/WHAT_IS_FSM_COS.md) |
| Understand how Workshop packages fit together, including package-versus-MicroBundle decisions and Profiles (source-repository working map) | [Ecosystem Integration Map](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/docs/ECOSYSTEM_INTEGRATION_MAP.md) |
| Understand the architecture and component ownership | [Architecture](docs/ARCHITECTURE.md) |
| Learn the conceptual model and rationale | [FSM_COS Theory](docs/THEORY.md) |
| Understand the request that enters the kernel | [Runtime Manifest](docs/RUNTIME_MANIFEST.md), then [Manifest Theory](docs/MANIFEST_THEORY.md) |
| Understand staged loading, dependency planning, and the current implementation boundary | [Staged Manifest Loading](docs/STAGED_MANIFEST_LOADING.md) |
| Understand the object handed to a host | [RuntimeAssembly](docs/RUNTIME_ASSEMBLY.md) |
| Understand what stays outside the kernel | [Runtime Boundary](docs/RUNTIME_BOUNDARY.md) |
| Run the minimal consumer example | [Minimal Consumer sample](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/samples/FSM_COS.MinimalConsumer/Program.cs) |
| Learn how MicroBundles are resolved and consumed | [Consuming MicroBundles](docs/CONSUMING_MICROBUNDLES.md) |
| Understand MicroBundle arbitration and convergence | [Arbitration](docs/ARBITRATION.md) |
| Explore AI-oriented composition | [AI Composition](docs/AI_COMPOSITION.md) |
| Understand the domain FSM_COS owns | [Domain](docs/DOMAIN.md) |
| Integrate with WebPage | [WebPage Integration](docs/WEBPAGE_INTEGRATION.md) |
| Contribute, build, and verify changes | [Development](docs/DEVELOPMENT.md) |

## Recommended reading order

For a first encounter:

1. [README](README.md) — identity, responsibility boundary, and quick orientation.
2. [What Is FSM_COS?](docs/WHAT_IS_FSM_COS.md) — a plain-language introduction for readers new to software composition.
3. Run the [Minimal Consumer sample](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/samples/FSM_COS.MinimalConsumer/Program.cs) to see dependency ordering.
4. [Architecture](docs/ARCHITECTURE.md) — how the pieces fit together and which layer owns each concern.
5. [Ecosystem Integration Map](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/docs/ECOSYSTEM_INTEGRATION_MAP.md) — live cross-repository working map of package responsibilities, integration gaps, and MicroBundle candidates.
6. [FSM_COS Theory](docs/THEORY.md) — why composition is a distinct operation.
7. [Runtime Manifest](docs/RUNTIME_MANIFEST.md) — what the caller requests.
8. [RuntimeAssembly](docs/RUNTIME_ASSEMBLY.md) — what the host receives.
9. [Runtime Boundary](docs/RUNTIME_BOUNDARY.md) — where FSM_COS hands off responsibility.
10. The relevant integration or development guide for the task at hand.

Experienced readers can jump directly to the contract or integration guide they need.

## Documentation authority

- The README is the public front door, not the exhaustive specification.
- Architecture and theory documents explain ownership and rationale.
- Focused contract guides describe the manifest, assembly, dependency, loading, and arbitration boundaries.
- Source code and tests determine the behavior of the current implementation. If prose and implementation disagree, record and resolve the discrepancy rather than silently assuming the prose is correct.
- The repository-wide documentation rules live in [Documentation Standard](DOCUMENTATION_STANDARD.md).

## Status discipline

FSM_COS is evolving. Read any version or status statement in context: a design target is not automatically implemented, and code present on a development branch is not automatically part of a published NuGet release.

---

<p align="center"><em>The Singularity Workshop — Tools for the curious, the bold, and the systemically inclined.</em><br><strong>Because state shouldn't be a mess.</strong><br><em>And because static boundaries are invitations to cause trouble.</em></p>
