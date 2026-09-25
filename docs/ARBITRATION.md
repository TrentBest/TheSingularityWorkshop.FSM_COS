# Arbitration and Convergence

Arbitration is the reconciliation phase of FSM_COS.

Loading answers: what did the manifest request, including everything those requests depend on?

Arbitration answers: given everything now installed, does the composition need to change?

## Initial composition

Dependency graphs are loaded first. Only after the reachable set is installed does arbitration begin.

## Round semantics

Every loaded MicroBundle receives the same ArbitrationContext.

A bundle returns true when its participation changed the composition and false when it made no change.

FSM_COS combines those results for the round. If a complete round reports no changes, the assembly is stable.

## Why bounded convergence?

Arbitration permits emergent interaction. One bundle can reveal information that changes another bundle, which can expose a consequence to another bundle.

An unbounded loop would make composition unsafe.

FSM_COS therefore imposes a maximum arbitration count. The current default is ten rounds.

Non-convergence is an error rather than a partially trusted RuntimeAssembly.

## Arbitration is not universal priority arbitration

The mechanism does not define a universal winner. Load order comes from dependency resolution, and arbitration follows the assembled list.

A future policy layer may define richer priorities if required, but FSM_COS should not invent authority that the manifest or bundle model does not provide.

## Entangled dependencies

A dependency can be useful precisely because its behavior depends on the composition requesting it.

    A requests B + configuration X
    B requests C + configuration Y

FSM_COS ensures configuration is available at installation time. This allows dependencies to participate in the larger composition without requiring the parent to know their implementation.

## Convergence is the contract

A MicroBundle should treat arbitration as composition negotiation. It should not use arbitration to start arbitrary application processes, render UI, or escape the composition boundary.

> The goal is not activity. The goal is a stable assembled runtime.