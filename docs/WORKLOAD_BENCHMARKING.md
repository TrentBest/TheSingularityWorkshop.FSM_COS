# Workload Benchmarking: Composition vs. Execution

This guide defines the measurement boundary for comparing a direct FSM_API workload with the same workload prepared as a MicroBundle through FSM_COS. It is an experiment design, not a performance claim.

## The architecture being measured

```text
Host selects workload + manifest
              |
              v
       IMicroBundleCatalog
              |
              v
           FSM_COS
   resolve dependencies / load /
      arbitrate to convergence
              |
              v
        RuntimeAssembly
              |
              v
Host owns lifecycle, tick loop, instrumentation, and output
              |
              v
         Workload updates
```

The `RuntimeAssembly` exposes the composed `IMicroBundle` instances, including `TryGetBundle<TBundle>` for retrieving a known bundle type. The host may then invoke the workload's host-facing API. FSM_COS composes the workload; it is not a general-purpose scheduler, frame loop, GUI, or simulation runtime.

## Keep the timing buckets separate

| Measurement | Start/end boundary |
|---|---|
| Direct setup | Before constructing/registering the direct workload until it is ready |
| FSM_COS composition | Immediately before `FsmCos.Execute(manifest)` until it returns a `RuntimeAssembly` |
| Workload activation | Any host-owned initialization required after composition; report separately if nontrivial |
| Warm-up | Explicit updates excluded from reported samples |
| Steady-state execution | Host tick/update call and equivalent workload work, after setup |
| Teardown | Cleanup and destruction; report separately from steady-state execution |

If the MicroBundle performs meaningful initialization in `IMicroBundle.Load`, that time belongs to composition. Do not divide a one-time composition cost by every measured tick and call it runtime overhead. If startup latency matters to a real use case, report it as a separate user-visible startup metric.

## Equivalence is the gate

Before interpreting performance, validate that both paths:
- use the same workload operations, state transitions, worker counts, and update intervals;
- execute the same number of actual worker updates;
- produce the same observable result or deterministic work counter;
- use the same runtime, build configuration, machine, and sampling protocol;
- exclude console output, CSV serialization, and result aggregation from the timed workload interval unless those costs are intentionally part of both paths.

A faster measurement that completes less work is not a valid speedup.

## Recommended first experiment

Use the existing SimpleDemos worker-throughput workload as the control. Keep its current behavior as the historical baseline, then create an equivalent MicroBundle whose identity and version are explicit and whose host-facing API exposes the workload to the host. Begin with small worker counts and deterministic work-count assertions; only then scale to the existing high-load configurations.

Record cold composition/setup separately from repeated steady-state updates. Use repeated independent samples and preserve raw CSV rows with runtime, OS, CPU, commit, worker count, tick interval, sample count, completed work, elapsed time, and allocation observations.

The first iteration may use an in-memory catalog. That isolates FSM_COS composition from artifact delivery. A local filesystem repository and local REST host are separate increments; neither should be silently charged to the FSM_COS composition measurement.

## What this benchmark does not establish

- It does not claim that FSM_COS is faster than direct FSM_API.
- It does not establish a universal overhead independent of workload, machine, runtime, or configuration.
- It does not conflate composition with steady-state execution.
- It does not require Azure or a remote server for the first experiment.

The outcome should be a reproducible, use-case-specific estimate of composition cost and steady-state difference, with enough raw evidence to repeat the experiment later.
