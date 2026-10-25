![The Singularity Workshop animated moniker](https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg)

# FSM_COS Benchmark Catalog

**Status: initial executable catalog; no performance conclusions have been established.** This catalog names stable benchmark definitions and defines the identity/evidence contract for every execution. It is separate from the package runtime and from unit-test correctness assertions.

## Identity rules

- Definition ID answers what is being measured and is permanent: COS-<FAMILY>-<NNN>.
- Run ID answers which invocation produced this evidence and must be unique. The harness writes BenchmarkDotNet.Artifacts/BenchmarkRunIdentity.txt on every invocation. CI labels runs COSRUN-<workflow-run-id>-<attempt>-<commit-prefix>; local runs include a UTC timestamp and random suffix.
- A new semantic operation or materially changed workload receives a new definition ID. Do not reuse or renumber retired IDs. Keep retired entries here with their retirement reason.
- A run is not identified by its timestamp alone. Preserve the run ID with the commit SHA, runtime, OS, architecture, processor count, benchmark tool version, parameters, build configuration and raw BenchmarkDotNet artifacts.
- A definition ID is embedded in the benchmark method name and BenchmarkDotNet description so results remain searchable if display names or source paths change.

## Implemented benchmark definitions

| Stable ID | Benchmark method | Measurement boundary | Workload parameters |
|---|---|---|---|
| COS-MAN-001 | ManifestBenchmarks.COS_MAN_001_ConstructRuntimeManifest | Allocate a root-entry array and construct a RuntimeManifest; graph entries are prepared outside the timed method | 1, 10, 100, 1,000 roots |
| COS-CMP-001 | CompositionBenchmarks.COS_CMP_001_ExecuteEmptyManifest | Execute an empty manifest against an empty in-memory catalog; includes the returned RuntimeAssembly | Empty |
| COS-CMP-002 | CompositionBenchmarks.COS_CMP_002_ComposeIndependentRoots | Execute a manifest with independent root bundles; catalog, bundle instances and manifest are prepared in global setup | 1, 10, 100, 1,000 roots |
| COS-CMP-003 | DependencyChainBenchmarks.COS_CMP_003_ComposeDependencyChain | Resolve and compose a linear dependency chain from one requested root; setup excluded | 1, 10, 100 bundles |
| COS-CMP-004 | SharedDependencyBenchmarks.COS_CMP_004_ComposeSharedDependency | Compose multiple requested roots that all depend on one shared bundle; setup excluded | 1, 10, 100, 1,000 roots plus one shared dependency |
| COS-ERR-001 | FailureBenchmarks.COS_ERR_001_TryExecuteUnresolvedRoot | Run the non-throwing composition API for a missing requested root; expected failure result is returned | One missing root |

All timed methods return their result to BenchmarkDotNet to prevent dead-code elimination. MemoryDiagnoser records managed allocation and GC data. ShortRunJob is the initial CI-friendly sampling profile, not a final publication-quality protocol. Re-run any apparent regression with a longer, controlled job before making a performance claim.

## Planned coverage queue

Add each item as a separately named definition only after its workload contract and equivalence checks are explicit.

| Family | Planned coverage |
|---|---|
| COS-MAN | Staged-plan validation at empty/small/large sizes; immutable snapshot and validation overhead |
| COS-CAT | Catalog lookup success/miss, version match/mismatch, dictionary scale and lookup counts |
| COS-DEP | Wide dependency fan-out, multiple roots, deep chains, shared dependency closure, duplicate roots, duplicate/conflicting versions |
| COS-LOAD | Load callback cost, configuration retrieval/copy, configuration size, load-order bookkeeping |
| COS-ARB | Stable first-round arbitration, multi-round convergence, near-limit convergence and non-convergence/failure reporting |
| COS-ERR | Request validation, dependency missing, invalid graph/cycle, load failure and rollback/compensation paths |
| COS-RES | Resource bounds, configuration byte bounds, large manifest/dependency inputs |
| COS-E2E | Direct workload versus equivalent MicroBundle composition; startup/composition separated from steady-state execution and teardown |
| COS-CON | Concurrency only where a real concurrent host workload exists; FSM_COS composition itself is not assumed thread-safe |
| COS-MEM | Allocation and GC pressure across repeated independent composition runs |

## Evidence and comparison rules

1. Do not compare unlike workloads or call composition cost per-tick overhead. FSM_COS composes a runtime; the host owns the steady-state update loop.
2. Keep setup, catalog creation, bundle creation and manifest creation outside composition measurements unless a benchmark explicitly names them as part of the measured operation.
3. Preserve raw CSV/Markdown/HTML and environment evidence; never overwrite previous evidence as if it were the same execution.
4. Record whether the benchmark uses the current FSM_COS source commit and which dependency lane it uses. A passing run does not itself prove performance stability.
5. Initially collect evidence without failing builds on noisy timing thresholds. Add CiAi regression policy only after stable baselines, variance rules and runner consistency are established.
6. No result, speedup, complexity claim or baseline is asserted until the suite has actually run and raw artifacts have been inspected.

## Run locally

From the repository root:

```sh
dotnet restore benchmarks/FSM_COS.Benchmarks/FSM_COS.Benchmarks.slnx
dotnet run --project benchmarks/FSM_COS.Benchmarks/FSM_COS.Benchmarks.csproj --configuration Release -- --filter '*COS_*'
```

The workflow retains BenchmarkDotNet.Artifacts/**, the run identity file and runner environment report as a uniquely named artifact. The project is intentionally excluded from the package solution and package contents.

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
