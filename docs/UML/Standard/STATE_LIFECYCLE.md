# State and Lifecycle Diagrams

## Purpose
Answer: **What states can an entity occupy, what causes transitions, and who owns those transitions?**

For each state, document meaning and invariants. For each transition, identify trigger, guard, action/side effect, source of authority, next state, and invalid/ignored transition behavior. Mark initial, terminal, transient, deferred, failed, and recovery states when applicable. Include concurrency/reentrancy rules where they affect correctness.

Use source-derived transitions only when they can be justified from source; otherwise label the diagram as a human-authored contract or conceptual proposal. Do not imply that a metadata enum alone implements a lifecycle, scheduler, activation process, or runtime transition.

## Acceptance
- States and transition triggers are defined.
- Guards, side effects, invalid transitions, and terminal behavior are addressed.
- Ownership of transition logic is explicit.
- Planned lifecycle states are visibly distinguished from implemented behavior.
