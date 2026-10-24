<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# Architecture and Component Diagrams

## Purpose
Answer: **Which subsystem owns each responsibility, and which dependency directions are permitted?**

Show projects/packages/components, responsibility boundaries, public interfaces, external systems, and direct dependency edges. Distinguish source project references, package references, runtime integrations, and conceptual future boundaries. Mark direct versus transitive dependencies and identify the source of dependency metadata. Include allowed/forbidden dependency direction when architecture rules exist.

Do not turn a planned package, optional integration, or external responsibility into an implemented component. For FSM_COS, keep MicroBundleDomain's contract, FSM_COS composition, and host execution responsibilities distinct.

## Acceptance
- Each component has a concise responsibility and non-goals.
- Dependency arrows are directional and evidence-backed.
- External systems and unknown relationships are labeled.
- Architecture decisions/invariants link to relevant tests or records.

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
