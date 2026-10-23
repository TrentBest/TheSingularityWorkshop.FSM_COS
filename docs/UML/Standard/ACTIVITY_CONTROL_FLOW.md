# Activity and Control-Flow Diagrams

## Purpose
Answer: **How does this operation progress through decisions, loops, early returns, exceptions, and convergence gates?**

Represent meaningful branches, guards, loops, validation, error paths, cancellation/timeout boundaries, retry logic, and exit conditions. Keep each node linked to a source span or clearly marked as authored conceptual behavior. For graph algorithms, show visited-set behavior, traversal order, cycle checks, and termination conditions when those details matter.

A control-flow diagram is not a sequence diagram: it focuses on a single algorithm's path decisions rather than message exchange among participants. It is not a runtime trace unless captured from a named run.

## Acceptance
- Branches and loops match the analyzed method/body scope.
- Exception/failure exits are not omitted when material.
- Loop bounds and termination assumptions are documented.
- Complexity annotations point to the operation that contributes the cost.
- Generated and conditional code scope is declared.
