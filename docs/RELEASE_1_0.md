# FSM_COS 1.0.0 Release Contract

## Purpose

This document defines what **1.0.0 means for FSM_COS**.

The goal is not merely to change the version number. The goal is to make the composition kernel a stable dependency for the packages that sit above it.

## Stable dependency closure

FSM_COS 1.0.0 depends only on stable first-party packages:

```text
FSM_API 1.0.13 ───────────────┐
                              ├──> FSM_COS 1.0.0
MicroBundleDomain 1.0.0 ─────┘
```

The closure is intentionally small. GUI, REST, serialization, repository/storage, browser, desktop, and Unity concerns remain outside the kernel.

## What is being stabilized

The release boundary includes:

- Runtime Manifest input;
- MicroBundle dependency closure;
- configuration propagation;
- staged localization/loading concepts;
- configured MicroBundle loading;
- arbitration and bounded convergence;
- stable `RuntimeAssembly` handoff;
- catalog abstraction;
- host-neutral composition semantics.

The release does **not** promise that FSM_COS owns:

- application lifecycle;
- GUI rendering;
- browser or Unity lifecycle;
- storage or repository implementation;
- REST transport;
- serialization formats;
- Experience presentation;
- arbitrary scheduling.

Those remain neighboring responsibilities.

## Operational contract

A host should be able to reason about FSM_COS as:

```text
published request
      │
      ▼
  FSM_COS
      │
      ├── resolve dependency closure
      ├── carry configuration
      ├── load/install
      ├── arbitrate
      └── require convergence
      │
      ▼
RuntimeAssembly
      │
      ▼
     host
```

The kernel stops at the handoff.

## Release review checklist

Before the branch is considered ready for human release approval:

- [ ] Release build succeeds.
- [ ] Complete test suite succeeds.
- [ ] Coverage collection succeeds.
- [ ] Package contains the intended README, license, and documentation.
- [ ] Public XML documentation produces no unexplained warnings.
- [ ] Package `.nuspec` contains only stable first-party dependencies.
- [ ] README explains installation and the composition model.
- [ ] Theory documentation explains why the boundary exists.
- [ ] Visual documentation accurately represents the boundary.
- [ ] Publication remains manual and explicit.
- [ ] No merge or NuGet publication occurs automatically.

## Why this matters

FSM_COS is a dependency boundary. Once packages above it depend on 1.0.0, changing the meaning of the composition contract becomes more expensive.

That is why 1.0.0 is treated as a **contract stabilization point**, not merely a maturity badge.

> **A stable composition kernel should make the layers above it easier to stabilize, not force them to inherit its uncertainty.**
