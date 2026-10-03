# Performance, Footprint, and Efficiency

FSM_COS exists to compose functionality without forcing the composition kernel to become a large application framework.

That makes **performance and physical footprint part of the architecture story**, not an afterthought.

The Workshop already has measured evidence from FSM_API and an emerging physical baseline for AnyApp. This document keeps those facts together while clearly separating:

- **measured results**;
- **derived observations**;
- **working hypotheses**; and
- **future measurements we still need**.

## Why performance belongs in the FSM_COS story

FSM_COS is intentionally small in responsibility:

~~~mermaid
flowchart LR
    A[Runtime Manifest] --> B[FSM_COS]
    B --> C[RuntimeAssembly]
    C --> D[AnyApp]
    C --> E[WebPage / WebApp]
    C --> F[MyVR]
    C --> G[Distributed execution]
~~~

The kernel does not own rendering, storage, transport, GUI, or application lifecycle.

That separation has a practical consequence:

> **The composition boundary can remain small even when the system composed at that boundary becomes large.**

This is one reason FSM_COS should remain a kernel rather than becoming an application framework.

## The FSM_API performance foundation

FSM_COS uses FSM_API as its state/context primitive.

A published BenchmarkDotNet baseline for the current string-backed FSM_API measured:

| Active process groups | Mean | Allocated |
|---:|---:|---:|
| 1 | 305.1 ns | 360 B |
| 10 | 3,115.7 ns | 3,600 B |
| 50 | 15,736.6 ns | 18,000 B |

The benchmark also reported approximately linear growth across those workloads.

For 50 groups, 15.74 µs is about **0.094% of a 16.67 ms 60 FPS frame budget** for the measured FSM update machinery alone.

That does **not** mean an application using FSM_API consumes only 0.094% of a frame. The benchmark measures the FSM machinery, not the application work executed by its states.

The complete benchmark discussion is published in Coder Legion:

[Benchmarking the FSM: Pure Strings, Already Fast — and About to Get Faster](https://coderlegion.com/26350/benchmarking-the-fsm-pure-strings-already-fast-and-about-to-get-faster)

~~~mermaid
xychart-beta
    title "FSM_API update cost by active process groups"
    x-axis ["1", "10", "50"]
    y-axis "Mean (ns)" 0 --> 16000
    line [305.1, 3115.7, 15736.6]
~~~

## Allocation matters too

The same benchmark measured allocation of approximately 360 bytes per active process group per update under that workload.

At 50 groups updated at 60 FPS, that corresponds to approximately:

~~~text
18,000 B/update
× 60 updates/second
≈ 1.08 MB/second of managed allocation
~~~

Again, this is a benchmark-derived observation, not a claim about all FSM_API workloads.

It gives us something more useful than a slogan: **a baseline that can be compared against future implementations**.

FSM_API is already being optimized toward more compact internal representations while preserving the expressive API. When that work is complete, the same benchmark can be rerun against the same workload.

## Why this matters to energy efficiency

Reducing allocation and unnecessary runtime work can reduce the computational work required to produce the same result.

That creates a legitimate efficiency question:

> If two implementations perform the same useful computation, but one creates substantially less temporary work for the runtime and garbage collector, how much energy does the lower-allocation implementation actually save?

That is a question worth measuring.

It is **not yet a measured FSM_COS energy result**.

The Workshop has discussed this idea in its Coder Legion writing, including the “32% Dividend” framing around code efficiency and energy. That material is useful as the motivation for measuring the question, but it should not be presented as a direct joule-per-operation measurement of FSM_API or FSM_COS.

~~~mermaid
flowchart LR
    A[Same useful computation] --> B[Less allocation]
    B --> C[Less reclamation pressure]
    C --> D[Potentially less runtime work]
    D --> E[Energy question]
    E --> F[Measure joules]
~~~

The next level of evidence is therefore straightforward:

1. hold the workload constant;
2. compare allocation and execution time;
3. measure CPU/package energy where the hardware exposes it;
4. repeat across representative workloads;
5. report uncertainty and hardware conditions.

Until that experiment exists, **“~30% more energy efficient” should remain a hypothesis or external article claim, not a package benchmark result.**

## Current physical footprint: AnyApp

AnyApp is the first concrete desktop proving ground for this architecture.

Its repository already contains a CI baseline that publishes several forms of the application and records both directory and executable size. The following values are the current working measurements from the local baseline work; they should be treated as **approximate physical observations**, not permanent package guarantees.

| AnyApp form | Approx. footprint | Approx. EXE |
|---|---:|---:|
| Framework-dependent publish | 0.33 MB | 0.15 MB |
| Self-contained publish | 160.10 MB | 0.15 MB |
| Single-file self-contained | 154.33 MB | 146.48 MB |
| Blank WPF Release executable milestone | — | **~149 KB** |

The distinction matters.

A self-contained deployment carries the .NET runtime with it, so its directory footprint is not comparable to the size of the application executable itself.

Likewise, a single-file deployment intentionally packages the runtime and application together.

The interesting engineering number for the current AnyApp scaffold is therefore the tiny application executable, while the deployment footprint depends strongly on publishing mode.

~~~mermaid
flowchart TD
    A[AnyApp] --> B[Framework-dependent]
    A --> C[Self-contained]
    A --> D[Single-file]
    B --> B1["~0.33 MB directory<br/>~0.15 MB EXE"]
    C --> C1["~160.10 MB directory<br/>~0.15 MB EXE"]
    D --> D1["~154.33 MB<br/>~146.48 MB EXE"]
    A --> E["Blank WPF milestone<br/>~149 KB EXE"]
~~~

### What these numbers do not prove

AnyApp is still a **scaffolded proving ground**.

It is functional enough to establish a real host/composition path, but it is not yet a mature production application.

Therefore these measurements do not establish:

- final AnyApp size;
- final startup time;
- final working-set size;
- final memory high-water mark;
- final streaming cost;
- final FSM_COS overhead;
- or final distributed-execution cost.

They establish a baseline from which those measurements can be made.

## MyVR: intentionally not pretending we know yet

MyVR is even earlier as a physical baseline.

The architectural case for MyVR is already clear: it is a radically different manifestation/execution environment that should be able to consume the same composition boundary.

But we do **not** currently have a comparable, reproducible MyVR footprint measurement in this repository evidence set.

So the honest status is:

| Environment | Current evidence |
|---|---|
| FSM_API | BenchmarkDotNet timing/allocation baseline |
| FSM_COS | Composition kernel; dedicated physical/energy benchmark not yet established |
| AnyApp | Functional-ish desktop proving ground with approximate size measurements |
| MyVR | Scaffolded execution environment; comparable footprint baseline still needed |
| WebPage/WebApp | Separate host measurements needed |
| DistributedApp | Design direction; no implementation benchmark yet |

That gap is useful rather than embarrassing.

It tells us exactly what metaDev should eventually measure.

## CLI versus application executable

There is currently no separate production **AnyApp CLI artifact** whose footprint should be presented as if it were a finished product.

AnyApp is presently a WPF desktop host.

If a dedicated command-line composition host is introduced later, it should get its own baseline:

| Artifact | Measure |
|---|---|
| CLI executable | executable bytes |
| CLI publish directory | total deployment bytes |
| startup | cold/warm startup |
| working set | idle + representative workload |
| allocation | steady-state + high-water |
| composition | FSM_COS execution cost |
| energy | joules per representative operation/workload |

This prevents a desktop host and a future CLI from being conflated.

## Why FSM_COS?

The performance question therefore becomes bigger than “is the composition algorithm fast?”

The architectural answer is:

> **FSM_COS should be small because its responsibility is small.**

It should not acquire:

- a GUI framework;
- a renderer;
- a repository;
- a transport stack;
- a serialization framework;
- an application lifecycle;
- a browser;
- Unity;
- or a distributed scheduler merely to make composition possible.

Those concerns belong at neighboring boundaries.

~~~mermaid
flowchart TB
    A[Small composition kernel] --> B[Small dependency surface]
    B --> C[Less framework baggage]
    C --> D[More execution environments]
    D --> E[More measurable deployment modes]
    E --> F[Optimization can happen at the correct boundary]
~~~

That is the deeper reason the size story belongs beside the architecture story.

## The measurement ladder

The Workshop should progressively replace estimates with reproducible measurements:

~~~text
CURRENT
  |
  +-- FSM_API timing/allocation benchmark
  |
  +-- AnyApp physical publish baseline
  |
  +-- AnyApp executable milestone
  |
  v
NEXT
  |
  +-- FSM_COS composition benchmark
  +-- AnyApp startup / working set
  +-- MyVR footprint
  +-- WebPage / WebApp footprint
  |
  v
THEN
  |
  +-- representative workload allocation
  +-- CPU time
  +-- package/runtime energy
  +-- distributed computation overhead
  |
  v
EVENTUALLY
  |
  +-- apples-to-apples energy-per-useful-computation
  +-- local vs remote computation cost
  +-- measured placement trade-offs
~~~

The objective is not to manufacture impressive numbers.

It is to make the architecture **measurable enough that impressive numbers, if they occur, can be trusted**.

## The pitch

The defensible version of the pitch is:

> **FSM_COS is a small, compute-environment-independent composition boundary.**
>
> FSM_API gives the Workshop a measured, performance-oriented state foundation. FSM_COS adds composition without turning that foundation into a monolithic application framework.
>
> AnyApp gives us a real desktop proving ground. MyVR gives us an immersive proving ground. WebPage and WebApp give us web environments. DistributedApp gives us a future topology for sharing computation.
>
> The important engineering question is not whether every environment is identical.
>
> It is whether the **same functionality can remain composed while the compute environment changes**.
>
> Then we measure what that costs.

## Evidence and next measurements

### Existing evidence

- FSM_API BenchmarkDotNet baseline: 305.1 ns / 360 B for one active process group; 15,736.6 ns / 18,000 B for fifty.
- AnyApp CI is already structured to publish framework-dependent, self-contained, single-file, and experimental trimmed variants and report directory/EXE sizes.
- AnyApp currently provides a small, functional-ish desktop proving ground rather than a finished application.
- MyVR currently lacks an equivalent reproducible physical footprint baseline.

### Still to measure

- FSM_COS manifest-to-RuntimeAssembly overhead;
- allocations caused by composition itself;
- startup and working-set cost by host;
- MyVR physical footprint;
- WebPage/WebApp physical footprint;
- CLI footprint if a dedicated CLI host is introduced;
- CPU/package energy under fixed workloads;
- distributed computation overhead;
- energy per unit of useful computation.

**The rule is simple: measured numbers are facts, estimates are labeled, and hypotheses become claims only after measurement.**
