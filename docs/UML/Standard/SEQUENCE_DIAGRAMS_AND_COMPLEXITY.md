# Sequence Diagrams and Complexity Analysis

## Purpose
Answer: **What calls what, in which order, along which success and failure paths, and what is the likely algorithmic cost?**

## Sequence content
Show the actor/host, public entry point, collaborators, returned values, state changes where relevant, validation, early exits, exceptions/failure results, and alternative paths. Label calls with method/member identity and link to source locations. Distinguish statically inferred calls from dynamically observed calls. Dynamic dispatch, reflection, dependency injection, callbacks, event handlers, generated code, and external systems can make static call graphs incomplete.

## Complexity table
For meaningful workflows, include a table with method/operation, input-size variables, dominant work, time complexity, auxiliary space/allocation notes, expected vs worst-case caveat, evidence class, and optimization/test suggestion. Explicitly flag plausible O(n²) or worse paths such as nested input-dependent loops, repeated full scans, repeated sorts, or repeated graph traversal without a visited set. Identify linear alternatives when supported by the algorithm and constraints.

## Rigor
Big-O describes asymptotic growth, not a wall-clock upper bound. Numeric upper bounds require defensible limits on inputs, runtime, hardware, allocations, scheduling, and external operations. Never turn a static heuristic into a measured claim. Benchmark claims identify revision, environment, workload, sample method, and results. If the bound is unknown, say **unknown / measure required** and specify what measurement would resolve it.

## Acceptance
- Source-linked call/member identities and scope.
- Success, failure, and relevant alternative paths.
- Explicit assumptions and uncertainty for complexity.
- Candidate hot paths prioritized by evidence and impact.
- Benchmarks and static estimates clearly separated.
