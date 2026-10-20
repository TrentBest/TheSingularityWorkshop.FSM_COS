# DocumentationVisuals Program

**Source:** `samples/DocumentationVisuals/Program.cs`  
**Baseline:** `ce43e8f5bf3dfc229f28678fc2e0d944c5c8d38a`  
**Authorship:** manually authored; not GUI.SVG-generated.

## Source-grounded summary

Top-level sample uses GUI GuiBuilder and GUI.SVG APIs to author an SVG contract map and writes to the command-line path or docs/assets/fsm-cos-preflight.svg. It also imports XDocument.

## Diagram

```mermaid
flowchart LR
P[DocumentationVisuals Program] --> B[GUI GuiBuilder API]
P --> S[GUI.SVG scene API]
P --> X[XDocument]
P --> O[SVG output file]
```

## Evidence boundary

The source scene describes itself as a contract map, not a live execution trace. Connector motion is explanatory. This sample does not provide runtime telemetry from a running FsmCos instance.

For behavior beyond the declaration summarized here, inspect the implementation and relevant tests at the same baseline. This page is not evidence of hosted rendering or accessibility behavior by itself.
