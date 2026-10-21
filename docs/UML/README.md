![The Singularity Workshop animated moniker](https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Renderer/development/docs/assets/moniker.svg)

# FSM_COS UML — repository map

> **Current status:** the source maps below are manually authored, not GUI.SVG-generated. The target folder layout in this document is the agreed direction, not a claim that every view or generator already exists.
>
> **Current source baseline for the existing companion pages:** `ce43e8f5bf3dfc229f28678fc2e0d944c5c8d38a`.

This file is the entry point for the repository's type-level documentation. It should eventually let a reader scan the whole model, understand how views relate, and open a focused page for any source file, type, collaboration, or use case.

## 1. Target folder layout

```text
docs/UML/
├── README.md                 # master index: coverage, links, status, generation evidence
├── AST/                      # one source-file AST card per tracked .cs file
├── ClassDiagram/             # one type-centered relationship view per documented type
├── Collaboration/            # interaction/sequence views for meaningful collaborations
├── UseCases/                 # user-goal scenarios, grouped by the featured type
├── src/                      # current manually authored source companion pages (migration source)
├── tests/                    # current test/evidence companion pages
└── samples/                  # current sample companion pages
```

The legacy `src/`, `tests/`, and `samples/` pages remain in place until their content has been reconciled and migrated. Do not delete or bulk-move them merely to make the tree look tidy. The master index must distinguish **implemented**, **planned**, **generated**, **manually maintained**, and **not yet verified** artifacts.

## 2. What each view means

### AST — one card per source file

Each tracked `.cs` file gets an AST document that records its declarations and source structure. A file may declare multiple types, and a type may be partial across multiple files, so the index must map **source files to declared types** rather than assume a one-file/one-class relationship.

The compact card is the default overview. Expanded detail may expose namespaces, declarations, members, modifiers, generic parameters, attributes, and source locations. Generated facts must retain links back to the exact source file and line/range where feasible.

### ClassDiagram — one focal type per view

Each type-centered diagram should show:

- **Above the focal class:** source-grounded types that use or call it, when those relationships are known.
- **Center:** the focal type's compact AST/class card, linking to its detailed AST page.
- **Below the focal class:** types and abstractions it uses, owns, calls, or references, with relationship kinds distinguished rather than inferred from layout alone.
- **Left rail:** primitive/framework language types such as `string`, `int`, `float`, and `double`, but only when they occur in the focal card. Give each type a stable color and matching edge stub on the card. **Do not draw relationship lines** between these rail entries and the card.
- **Right rail:** external packages and third-party dependencies, such as Newtonsoft.Json, only when evidenced by the source/project dependency graph. Apply the same stable-color and matching-stub rule, with **no lines** to the rail.

The rails are compact legends, not relationship graphs. Actual semantic relationships belong in the diagram's central model. Color is a visual aid, not the sole carrier of meaning; labels and a textual relationship list must remain available.

### Collaboration — how participants interact

Use collaboration/sequence views for a concrete interaction across multiple participants: calls, messages, state hand-offs, or an execution path. Name the scenario and link each participant to its type card. Draw only interactions supported by source, tests, or a clearly labeled proposed design. Do not use a class diagram as a substitute for a temporal interaction diagram.

### UseCases — why a type matters to a caller

Organize use cases by the class or interface they feature, for example `UseCases/FsmCos/UC-001-compose-runtime.md`. Each scenario should identify its actor, goal, trigger, preconditions, success path, alternate/failure paths, and evidence. A use case may link to several type cards and collaboration diagrams; it should not be forced into one class's folder if that would hide a cross-cutting scenario.

Use-case intent is not safely derivable from syntax alone. Keep the human-authored goal and actor explicit, and label it **proposed** until reviewed. Ground implementation behavior in source and tests, and distinguish observed behavior from intended behavior.

## 3. Compact cards and interaction

Default views should show only the highest-value, top-level information. Detail should be available through links to focused documents. Where the viewer supports genuine interaction, sections may expand/collapse in place; otherwise use linked expanded views rather than promising an interaction that GitHub's Markdown renderer cannot provide.

For every visualization, provide an accessible textual equivalent: a concise summary, labeled relationships, and links to source/evidence. Do not rely on color, animation, or hover behavior alone. Respect reduced-motion preferences. A visual that looks interactive is not evidence that it is interactive.

## 4. Coverage and provenance

The current manually authored inventory contains companion pages for 23 tracked C# files in its recorded baseline: 16 runtime source files, 5 test-project files, and 2 sample files. Runtime pages map declarations and source-visible relationships; test pages map asserted evidence; sample pages describe examples rather than production guarantees. This is a **baseline-specific count**, not a claim about all current or future source files.

The master index should ultimately be generated or validated against a deterministic source inventory and report:

- every tracked `.cs` file and every declared type, including nested and partial types;
- the AST card, class diagram, collaboration diagrams, and use cases associated with each relevant type;
- orphaned documentation and missing required artifacts;
- the source revision / generator version used for generated outputs;
- which claims are source-derived, test-supported, manually proposed, or still unverified.

A declaration establishes API shape, not all runtime behavior. A test name or assertion supports only the behavior it actually exercises. External, hosted, accessibility, lifecycle, concurrency, rollback, and performance claims remain open unless directly evidenced.

## 5. CI quality gates

The long-term UML/documentation gate should fail on missing or duplicate source coverage, stale generated artifacts, broken local links, invalid SVG/XML, missing accessibility text, and an undocumented source baseline. It should verify that regeneration produces no unexplained diff. Tests for the generator should include edge cases and malformed/ambiguous inputs, not merely a coverage percentage.

Keep separate results for source inventory, semantic extraction, relationship accuracy, rendering, accessibility, and hosted behavior. A successful build does not automatically prove a diagram is semantically correct, accessible, or rendered correctly on the hosting platform. Pin third-party GitHub Actions to immutable commit SHAs and review permission scopes.

## 6. Current pages

### Runtime source

- [ArbitrationContext](./src/FSM_COS/ArbitrationContext.md)
- [FsmCos](./src/FSM_COS/FsmCos.md)
- [IFsmCos](./src/FSM_COS/IFsmCos.md)
- [IManifestLoadEvaluator](./src/FSM_COS/IManifestLoadEvaluator.md)
- [IMicroBundleCatalog](./src/FSM_COS/IMicroBundleCatalog.md)
- [IMicroBundleConfigurationSource](./src/FSM_COS/IMicroBundleConfigurationSource.md)
- [ManifestLoadStage](./src/FSM_COS/ManifestLoadStage.md)
- [MicroBundleLoadContext](./src/FSM_COS/MicroBundleLoadContext.md)
- [MicroBundleManifestEntry](./src/FSM_COS/MicroBundleManifestEntry.md)
- [MicroBundleReference](./src/FSM_COS/MicroBundleReference.md)
- [RuntimeAssembly](./src/FSM_COS/RuntimeAssembly.md)
- [RuntimeManifest](./src/FSM_COS/RuntimeManifest.md)
- [RuntimeManifestDependency](./src/FSM_COS/RuntimeManifestDependency.md)
- [RuntimeManifestEntry](./src/FSM_COS/RuntimeManifestEntry.md)
- [RuntimeManifestLoadPlan](./src/FSM_COS/RuntimeManifestLoadPlan.md)
- [RuntimeManifestSchedule](./src/FSM_COS/RuntimeManifestSchedule.md)

### Tests

- [GlobalUsings](./tests/FSM_COS.Tests/GlobalUsings.md)
- [AiGuiCapabilityHandoffTests](./tests/FSM_COS.Tests/AiGuiCapabilityHandoffTests.md)
- [FsmCosTests](./tests/FSM_COS.Tests/FsmCosTests.md)
- [RuntimeManifestPlanningTests](./tests/FSM_COS.Tests/RuntimeManifestPlanningTests.md)
- [SnapshotIsolationTests](./tests/FSM_COS.Tests/SnapshotIsolationTests.md)

### Samples

- [MinimalConsumer Program](./samples/FSM_COS.MinimalConsumer/Program.md)
- [DocumentationVisuals Program](./samples/DocumentationVisuals/Program.md)

## 7. UML notation and evidence

The current class/flow diagrams are manually authored from the recorded source baseline. Solid relationships should be read as source-visible declarations or calls; dotted relationships indicate interface/contract interactions. Do not describe any diagram here as generated by GUI.SVG. That generator remains a separate staging effort and must pass its own source-reconciliation, relationship, rendering, and accessibility checks before any generated view is presented as authoritative.

---

**The Singularity Workshop**  
[GitHub](https://github.com/TrentBest) · [Coder Legion](https://coderlegion.com/) · [Patreon](https://www.patreon.com/c/TheSingularityWorkshop) · [PayPal](https://www.paypal.com/donate/?hosted_button_id=3Z7263LCQMV9J)  
[ FSM_API ](https://github.com/TrentBest/FSM_API) · [ FSM_Serialization ](https://github.com/TrentBest/TheSingularityWorkshop.FSM_Serialization) · [ WebPage ](https://github.com/TrentBest/WebPage) · [ FSM_COS Documentation Standard ](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/DOCUMENTATION_STANDARD.md)  
**Because state shouldn't be a mess.**  
*And because static boundaries are invitations to cause trouble.*
