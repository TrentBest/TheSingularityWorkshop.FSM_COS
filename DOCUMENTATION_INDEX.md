# FSM_COS Documentation Index

This index is the navigation map. It does not duplicate the contracts below.

## Start here

| Reader goal | Read |
|---|---|
| Understand the package quickly | [README](README.md) |
| Learn the idea without assuming programming experience | [What Is FSM_COS?](docs/WHAT_IS_FSM_COS.md) |
| Run a concrete consumer | [Minimal consumer sample](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/samples/FSM_COS.MinimalConsumer/Program.cs) |
| Understand ownership and runtime flow | [Architecture](docs/ARCHITECTURE.md) |
| Understand the rationale and design invariants | [Theory](docs/THEORY.md) |
| Integrate the package | [Consuming MicroBundles](docs/CONSUMING_MICROBUNDLES.md) |
| Review the candidate's scope and limitations | [Alpha.7 release notes](docs/releases/0.1.0-alpha.7.md) |
| Verify release readiness | [Alpha.7 readiness gate](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/docs/0.1.0-alpha.7-READINESS.md) |
| Review trust assumptions and adversarial security coverage | [Security](SECURITY.md) |
| Build, test, and understand CI modes | [Development](docs/DEVELOPMENT.md) |

## Contract authority map

Use one primary document for each question. Related pages should explain context and link here rather than restating the same contract.

| Concern | Primary authority | Scope |
|---|---|---|
| What FSM_COS owns | [Domain](docs/DOMAIN.md) | Domain boundary, not API reference |
| How the implementation works | [Architecture](docs/ARCHITECTURE.md) | Current code flow and failure behavior |
| Why the composition boundary exists | [Theory](docs/THEORY.md) | Rationale and invariants |
| Request shape and root version selection | [Runtime Manifest](docs/RUNTIME_MANIFEST.md) | Public manifest contract |
| Staged metadata and its limits | [Staged Manifest Loading](docs/STAGED_MANIFEST_LOADING.md) | Planning/state metadata versus end-to-end localization |
| Host handoff | [RuntimeAssembly](docs/RUNTIME_ASSEMBLY.md) | Output contract and explicit non-promises |
| Runtime ownership limits | [Runtime Boundary](docs/RUNTIME_BOUNDARY.md) | What belongs to hosts and adjacent packages |
| Resolution, loading, and consuming bundles | [Consuming MicroBundles](docs/CONSUMING_MICROBUNDLES.md) | Consumer-facing use and errors |
| Arbitration and convergence | [Arbitration](docs/ARBITRATION.md) | Negotiation semantics and limits |
| AI-related composition concepts | [AI Composition](docs/AI_COMPOSITION.md) | Architectural exploration; not proof of provider integration |
| WebPage host integration | [WebPage Integration](docs/WEBPAGE_INTEGRATION.md) | Host-specific integration boundary |
| Release-specific behavior | [Alpha.7 release notes](docs/releases/0.1.0-alpha.7.md) | Version scope and deferrals |
| Release gate | [Alpha.7 readiness](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/docs/0.1.0-alpha.7-READINESS.md) | Evidence and release criteria |
| Security boundaries and adversarial test plan | [Security](SECURITY.md) | Threat model, mitigations, external baselines, and residual risks |

## Documentation layers

- **Public orientation:** `README.md` and `NuGet.README.md`. The root README is the repository entry point; the NuGet README is the package-consumer entry point.
- **Current contract and explanation:** the primary authority documents listed above.
- **Repository process:** [Development](docs/DEVELOPMENT.md), [Documentation Standard](DOCUMENTATION_STANDARD.md), and [AI agent instructions](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/AGENTS.md).
- **Working and historical records:** branch reconciliation, dependency alignment, ecosystem integration, continuation, and pause-point documents are planning records. They are not API specifications or release evidence; verify any current-state statement against source, tests, and current CI before acting on it. These records are intentionally excluded from the consumer package.

## Accuracy and release evidence

The shared [Documentation Standard](DOCUMENTATION_STANDARD.md) governs writing quality, source accuracy, and link hygiene. The [alpha.7 readiness gate](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/docs/0.1.0-alpha.7-READINESS.md) governs exact-candidate release evidence. Historical planning notes are context, not current specifications; verify present-tense claims against source, tests, and current CI.

## Living documentation

The diagrams are explanatory assets, not independent specifications. Each diagram must agree with its adjacent contract and current implementation. The [Living Documentation Visual Audit](docs/LIVING_DOCUMENTATION_AUDIT.md) tracks visual coverage and accessibility-source checks; it does not claim browser or screen-reader verification unless that evidence is explicitly recorded.

---

<p align="center"><em>The Singularity Workshop — Tools for the curious, the bold, and the systemically inclined.</em><br><strong>Because state shouldn't be a mess.</strong></p>
