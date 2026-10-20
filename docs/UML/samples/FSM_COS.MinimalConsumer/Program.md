# MinimalConsumer Program

**Source:** `samples/FSM_COS.MinimalConsumer/Program.cs`  
**Baseline:** `ce43e8f5bf3dfc229f28678fc2e0d944c5c8d38a`  
**Authorship:** manually authored; not GUI.SVG-generated.

## Source-grounded summary

Top-level console sample defines DemoCatalog and DemoBundle. It requests report bundle ID 1/version 1.0.0; report declares data bundle ID 2 as prerequisite. It prints the composed order.

## Diagram

```mermaid
sequenceDiagram
participant Sample
participant Catalog as DemoCatalog
participant Kernel as FsmCos
participant Data as DemoBundle 2
participant Report as DemoBundle 1
Sample->>Kernel: Execute(root report v1.0.0)
Kernel->>Catalog: resolve root and dependency
Kernel->>Data: Load
Kernel->>Report: Load
Kernel-->>Sample: RuntimeAssembly
Sample->>Sample: print order
```

## Evidence boundary

This is an in-memory example of dependency-first composition, not production catalog/storage/network/lifecycle code.

For behavior beyond the declaration summarized here, inspect the implementation and relevant tests at the same baseline. This page is not evidence of hosted rendering or accessibility behavior by itself.
