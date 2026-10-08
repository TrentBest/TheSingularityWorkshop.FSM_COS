# Documentation Index

Choose a path by what you are trying to understand. You do not need to read every document in order.

## Start here

1. **What FSM_COS is:** return to the [repository README](../README.md) for the plain-language definition and the composition boundary.
2. **Why the boundary exists:** read [FSM_COS Theory](THEORY.md) for the reasoning behind composition, arbitration, and handoff.
3. **See the runtime flow:** follow [Runtime Manifest](RUNTIME_MANIFEST.md) → [Consuming MicroBundles](CONSUMING_MICROBUNDLES.md) → [RuntimeAssembly](RUNTIME_ASSEMBLY.md).

## Choose your reading path

- **New to the concept:** [README](../README.md) → [Architecture](ARCHITECTURE.md) → [Runtime Boundary](RUNTIME_BOUNDARY.md).
- **Trying to use FSM_COS:** [Consuming MicroBundles](CONSUMING_MICROBUNDLES.md) → [Runtime Manifest](RUNTIME_MANIFEST.md) → [WebPage Integration](WEBPAGE_INTEGRATION.md), if you are integrating that host.
- **Understanding design decisions:** [Theory](THEORY.md) → [Manifest Theory](MANIFEST_THEORY.md) → [Arbitration](ARBITRATION.md).
- **Changing or verifying the repository:** [Development](DEVELOPMENT.md), then the repository's solution and test projects.

## Reference map

| Document | What it answers |
|---|---|
| [Architecture](ARCHITECTURE.md) | Which contracts participate, and how composition flows through them? |
| [Theory](THEORY.md) | Why is composition a separate boundary, and what invariants govern it? |
| [Runtime Manifest](RUNTIME_MANIFEST.md) | What does a published composition request contain? |
| [Consuming MicroBundles](CONSUMING_MICROBUNDLES.md) | How does FSM_COS consume the domain-owned MicroBundle contract? |
| [RuntimeAssembly](RUNTIME_ASSEMBLY.md) | What is handed to a host after composition? |
| [Manifest Theory](MANIFEST_THEORY.md) | Why is the manifest separate from configuration and authoring metadata? |
| [Arbitration](ARBITRATION.md) | How does composition converge, and what happens when it does not? |
| [Runtime Boundary](RUNTIME_BOUNDARY.md) | What belongs in FSM_COS versus a host or neighboring package? |
| [WebPage Integration](WEBPAGE_INTEGRATION.md) | How does the browser host integrate without moving platform concerns into the kernel? |
| [Development](DEVELOPMENT.md) | How should this repository be built, tested, and changed? |
| [Ecosystem Documentation Standard](ECOSYSTEM_DOCUMENTATION_STANDARD.md) | What shared reader journey is being proposed for Workshop package documentation? |

## Reading conventions

The ecosystem documentation proposal uses a consistent reader journey: **01 What → 02 Why → 03 See It Work in 60 Seconds**. That standard is under review; this index does not claim the proposal has been adopted across repositories. Package-specific technical documents remain authoritative for their own contracts and behavior.

---

## Resources & Support

- [FSM_COS source](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS)
- [FSM_COS NuGet package](https://www.nuget.org/packages/TheSingularityWorkshop.FSM_COS)
- [The Singularity Workshop](https://github.com/TrentBest)
