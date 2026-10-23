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
