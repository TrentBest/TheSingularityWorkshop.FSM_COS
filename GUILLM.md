![The Singularity Workshop animated moniker](https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Renderer/development/docs/assets/moniker.svg)

# GUI LLM — FSM_COS Coordination

**Owner:** FSM_COS documentation and architecture  
**Counterpart:** TheSingularityWorkshop.GUI  
**Purpose:** Maintain a precise cross-repository contract for living documentation and source-derived UML. This file records coordination intent; it is not evidence that GUI staging work is integrated or accepted.

## Decision

The alpha.7 source-derived UML gate remains open. FSM_COS must not label manually authored Mermaid diagrams as GUI-generated UML, nor claim semantic relationships that the generator has not resolved from compiler symbols.

The desired path is:

`C# source + explicit target selection → source/symbol model → renderer-neutral diagram model → GUI.SVG SVG → generated documentation asset`

Generated artifacts must be deterministic, carry source provenance, and fail CI when committed output is stale. A diagram generated from one source revision is not evidence for another revision.

## Ownership and dependency boundary

- FSM_COS owns its architecture, runtime contracts, and consuming documentation.
- GUI owns reusable diagram models and SVG rendering contracts.
- Source analysis (including any Roslyn/compiler references) is tooling, not automatically part of the GUI runtime API.
- FSM_COS must consume a documented, supported GUI API/package boundary. Do not couple to staging-branch implementation types or assume an unmerged branch is available to consumers.
- GUI core must not depend upward on FSM_COS.
- The existing FSM_COS preflight visual is generated through GUI.SVG from a controlled scene and checked for committed-output parity. It is an explanatory contract map, not live runtime telemetry.
- Existing FSM_COS class and sequence diagrams authored in Mermaid remain explicitly manual until their generated replacements are accepted.

## Acceptance gate

The source-derived UML gate closes only when evidence covers all applicable items:

1. **Integrated state:** GUI work is reconciled into GUI `development`; build, tests, security, and package workflows pass on the exact integrated SHA. Record the SHA and workflow links.
2. **Semantic correctness:** inheritance, interface realization, associations, and type relationships are compiler-symbol-resolved or clearly labeled syntax-only. Draw connectors only when the model supports them.
3. **Fixture behavior:** fixture changes alter generated output; stale committed output fails CI; malformed C# fails closed; multi-type files have deterministic explicit target selection.
4. **Repository coverage:** the intended declaration inventory is explicit. The maintained standard is one source-grounded class view per maintained declared type; other UML families are produced when scenarios/architecture support them, not to satisfy a diagram count.
5. **SVG quality:** XML/accessibility metadata and viewBox bounds are checked, then visual clipping, legibility, relationship routing, and relevant viewport/theme behavior are inspected. XML validity alone is insufficient.
6. **Reproducibility and provenance:** output is deterministic, source revision is recorded, and regeneration parity is enforced.
7. **Package boundary:** document the supported public API and whether source parsing requires Roslyn/compiler references. Keep implementation-specific analysis out of the public GUI runtime surface unless intentionally designed and documented.
8. **Hosted accessibility:** verify browser rendering, accessibility-tree exposure, keyboard behavior, and screen-reader behavior separately.

## Current FSM_COS candidate boundary

The last verified FSM_COS code/package candidate was `43fae5aa6eec84aaf98f305b758bda03d0cc9a0e`:
- [Pack and publish run 39679881326](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/actions/runs/39679881326) passed configured build, test, source-integration, SVG, and package verification; the publish job was skipped.
- [CodeQL run 39679881297](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/actions/runs/39679881297) passed.
- These results do not establish GUI UML acceptance, hosted accessibility, or authorization to publish. Documentation changes after this SHA require fresh exact-head verification under the FSM_COS release gate.

## Coordination requests for GUI

Please maintain the counterpart decision and response in [GUI `FSM_COSLLM.md`](https://github.com/TrentBest/TheSingularityWorkshop.GUI/blob/development/FSM_COSLLM.md), based on GUI's integrated `development` state rather than staging branches. Identify the supported model/API entry point, intended package/version boundary, semantic UML gaps, and evidence for each acceptance item. State explicitly what is deferred and the residual limitation. Do not close the gate by relabeling a syntax projection.

## Safety and authorization

This coordination note authorizes no merge, PR closure, release, publication, deployment, branch deletion, or weakening of required checks. NuGet publication remains disabled until Trent explicitly declares that publication is authorized.
