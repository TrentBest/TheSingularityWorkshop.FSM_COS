<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# Living UML and Data-Card Visual Standard

This page is the FSM_COS-specific supplement to the shared [Documentation Standard](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/development/DOCUMENTATION_STANDARD.md), especially its canonical recursive, target-centered UML and data-card rules. A picture should explain the structure, and a living picture should let readers follow the meaning as it changes.

The canonical design rationale and broader interaction contract live in the GUI repository's [UML-Inspired Data Cards](https://github.com/TrentBest/TheSingularityWorkshop.GUI/blob/development/docs/DATA_CARDS_AND_UML.md). This page records the practical rules for FSM_COS documentation and for future Workshop repositories adopting the same style.

## Detailed view specifications

The cross-repository inventory and evidence policy is in the [Documentation Standard](../DOCUMENTATION_STANDARD.md). The per-view specifications are maintained in the [UML Standard index](UML/Standard/README.md): class/type diagrams, class neighborhoods and expandable data cards, sequence/complexity, activity/control flow, state/lifecycle, use cases, architecture/components, deployment topology, data models/lineage, MicroBundles/manifests, and requirement/test/decision traceability. This visual-style page governs how those views should look and how FSM_COS-specific boundaries must be represented; it does not claim that the planned source-analysis generator or CI gate is already implemented.

## The visual language

Use a **recursive, nested data-card composition**, not a flat pile of boxes. Every card has a complete, visibly thick border, a distinct header and body, readable dark text, and children nested inside the thing that owns them. The hierarchy should communicate ownership even when the image is viewed quickly.

**Keep static structure separate from dynamic behavior.** Nested cards show declarations and ownership: namespaces contain types, types contain members, and members can contain signature details. They do not, by themselves, show what executes first or how state changes over time. Use sequence/flow diagrams for interactions, state diagrams for lifecycle, and runtime traces only when backed by an actual captured run.

| Element | Border / fill direction | What it means |
| --- | --- | --- |
| Namespace | Thick blue border; pale indigo fill | Scope and ownership boundary |
| Class / struct / interface / record / enum | Thick orange border; warm peach fill | A declared type |
| Property | Yellow border; soft yellow fill | A value exposed through a type's contract |
| Field | Blue border accent; pale blue fill | Stored state |
| Method / constructor / event | Green border accent; pale green fill | Behavior or a declared event |
| Relationship | Explicit, labeled connector with a consistent line style | A source-supported dependency, inheritance, realization, association, or other relationship |

These are semantic colors, not decoration. Keep text high contrast, and **never rely on color alone**: labels, card shape, member kind, and connector labels must preserve the meaning for readers who cannot distinguish the palette. Use the same category colors across repositories and renderers. If a diagram family needs additional categories, document the extension before using it.

## Structure and reading order

1. Put the fully qualified namespace at the outside and preserve recursive containment.
2. Give each type its own card and identify its declaration kind.
3. Group members consistently. For class-oriented source views, show public contract first; keep protected and private detail available but visually subordinate or collapsed by default.
4. Keep signatures informative: types, parameters, return values, overload distinctions, and meaningful modifiers must not disappear just to make a picture smaller.
5. Separate a declaration from details that can expand: property accessors/initializers, method bodies, and other implementation detail.
6. Draw relationships only when supported by source, tests, or an explicitly labeled authored model. Label ambiguous, unresolved, or external relationships honestly.
7. Include a short caption that explains what the reader should learn from the picture and whether it is a source-derived structure, an authored concept, or a runtime trace.

## Living-image rules

- Prefer SVG generated from the same semantic model used by tests or the editor; do not manually maintain a second, competing version of source truth.
- Keep stable identity, accessible title/description, readable labels, and local links where useful.
- Make each picture answer a reader question: what owns this, what depends on it, what changes next, or what does this result mean?
- For behavior, use sequential or state-transition pictures rather than forcing everything into a class tree. A class diagram explains structure; a sequence diagram explains interactions over time; a state diagram explains lifecycle; a use-case diagram explains goals and boundaries.
- If an SVG is a conceptual illustration, label it as such. If it is generated from source, identify the source/revision or generation path. If it is a runtime trace, identify the captured run. Never imply that a static illustration proves execution.
- Keep an accessible textual explanation beside each important image. The image should add understanding, not make essential facts inaccessible to screen readers or plain-text readers.

## Workshop class-diagram composition (required layout)

A Workshop class diagram is **not a conventional UML sheet rearranged with different colors**. It is a recursive, reader-oriented map of one focal type and the people/systems that use it.

1. **Center the focal type.** The named class, interface, or record being explained is the visual anchor. Its public contract is immediately legible; internal helpers are secondary.
2. **Put consumers above the focal type.** Show the host, caller, authoring system, or other real user of the contract in cards above it. Connect each consumer downward to the entry point it invokes or supplies. Label the interaction in plain language (for example, “calls Execute” or “supplies catalog”); do not use unlabeled lines.
3. **Put collaborators and results below or to the sides.** Inputs, collaborators, outputs, and dependencies should be positioned by their role in the focal type's story, not scattered to imitate a textbook UML layout. Keep the focal type visually dominant.
4. **Preserve recursive ownership inside cards.** A namespace contains its types; a type contains member groups; member groups contain members; expandable details remain nested under their owner. Relationships between independently owned cards are connectors, not false containment.
5. **Use an explicit consumer/use-case section beside every class reference.** Describe who uses the type, what they are trying to accomplish, the entry point they use, what the type needs from them, and what they receive. Keep this goal-oriented section separate from source-structure facts. Include at least one realistic success path and relevant failure/boundary behavior.
6. **Show actual users, not generic UML stick figures.** Use labeled cards such as “Host application,” “WebApp,” “Authoring tool,” or “Test harness” only when the repository or example supports that role. If the caller is not known, label it “Consumer / host” rather than inventing a product.
7. **Distinguish structural facts from interactions.** The class card records declarations and source-backed relationships. Downward consumer connectors describe who calls or provides something; they do not imply ownership. Time-ordered execution belongs in a sequence/flow view, lifecycle belongs in a state view, and goals belong in the use-case text.
8. **Keep the diagram recursive and navigable.** Use the same layout rules at each zoom level. A reader should be able to start at the consumer, follow downward into the focal type, then follow collaborators or results without having to decode a web of crossing lines.
9. **Link each view to its companion and source.** Every class diagram page names the focal type, states its purpose, links the relevant source and tests, embeds the SVG, and includes a “Use cases” section. Mark manual diagrams as manually authored; do not imply automatic source generation.

The canonical file pattern is `[TypeName]UML.md` for the focused class-diagram page, alongside its SVG and source/type companion. A repository's UML index links these pages by the question a consumer is trying to answer. A type companion remains useful for its purpose, public contract, and UML class card; it does not replace the focused diagram/use-case page.

## Target-centered neighborhood layout

The focal type is the center of the composition—not simply one box in a conventional all-types UML layout.

- **Above the target:** place verified users/consumers, labeled with the actual relationship (constructs, calls, contains, implements, inherits, or another source-supported relation).
- **Center:** show the selected target type as the visual anchor, with declaration kind and name.
- **Below the target:** show types/declarations the target uses, including meaningful signature and member-type dependencies.
- **Right side:** show relevant package, assembly, project, and framework references. Distinguish direct references from transitive, inferred, and unresolved references.
- **On card edges:** show colored type-conduit stubs. Matching ends share a stable type identity/color; the receiving stub is rotated 180 degrees. Keep a visible label and accessible cue so color is never the only signal.

This arrangement answers who relies on a type, what it relies on, which data types appear at its boundaries, and which external packages are involved. It is semantic, not decorative: every relationship must be supported by source/tests or explicitly marked as an authored concept. Declare scan scope; absence from a partial scan does not prove absence of a consumer.

## Recursive detail and interaction

A data card is the same entity at multiple levels:

1. **Collapsed:** declaration kind and name (for example, interface IResolver or record BundleIdentity).
2. **Contract:** relevant members, signatures, property/field types, parameters, return values, and modifiers.
3. **Declaration/AST:** syntax-tree structure, nested declarations, and member structure when compiler-backed source analysis provides it.
4. **Implementation:** method bodies or deeper details only when the underlying source model provides them.

When the output platform supports input, collapsed cards should be clickable and keyboard-operable to reveal the next supported level, and collapsible again. Preserve stable identity, relationship context, focus visibility, and accessible names. On static platforms, link to the deeper type page/source instead. Never describe a static image as interactive, reflection as a complete AST, or source structure as a runtime trace.

The renderer-neutral model should carry source identity/span, declaration kind, relationship kind, type identities, analysis scope, and provenance; SVG is a rendering, not the only semantic store. Use compiler-backed syntax/semantic analysis for AST and symbol relationships, reflection only for runtime-visible metadata, authored text for human use cases, and captured runs for runtime observations.

## Per-type documentation contract

For each in-scope source file, document each declared type in a companion Markdown page where practical. A source file containing multiple types must identify the target of each page. Each type page explains what the type is, why it exists, its responsibilities and non-goals, a compact data card, important members, source-backed relationships, external package references, examples/tests, limitations, and links to source and tests.

Provide a companion <TypeName>UML.md with the wider target-centered diagram and a **Use cases** section. A use case records actor/consumer, goal, trigger/preconditions, success outcome, important alternatives/failures, and evidence/status. AST or reflection cannot independently infer user intent; use cases need human/LLM-authored direction grounded in product and source. The compact card belongs in the main type page; the companion UML page supplies the wider neighborhood.

## Provenance and verification gate

Every source-derived diagram identifies target and scope (file/project/solution), source revision/input, generation path, analysis method (syntax-only, compiler-backed, reflection-derived, or manually authored), unresolved relationships, and regeneration path. Keep links to source locations when possible.

A release-quality gate inventories in-scope files/types, checks required type and UML pages, validates links/metadata, regenerates diagrams, and detects missing/stale output. If only fixtures or a staging proof are automated, say so. Do not claim repository-wide generation until CI verifies repository-wide coverage.

## What this means for FSM_COS

Use living pictures to teach the real composition contract: a host requests root MicroBundles; a catalog resolves the bundles; FSM_COS validates the reachable graph and loads dependencies before dependents; bounded arbitration determines convergence; a successful `RuntimeAssembly` returns control to the host.

Do not draw FSM_COS as if it owns the host's UI, application lifecycle, catalog storage, or execution of the resulting application. Label examples as examples, and distinguish diagrams of the contract from live telemetry.

## Adoption checklist for every repository

- [ ] Namespace/type/member cards use the shared semantic palette and nested containment.
- [ ] Static declaration structure is visually distinct from runtime behavior and time-ordered interactions.
- [ ] Diagram labels and relationships are grounded in source or explicitly marked as authored concepts.
- [ ] A reader-facing caption explains what the image teaches.
- [ ] SVGs have a title and description, readable contrast, and a text equivalent.
- [ ] The image type is clear: source-derived structure, conceptual illustration, or captured runtime behavior.
- [ ] Source-derived pictures have a regeneration and stale-check path; do not claim automation until CI proves it.
- [ ] A diagram change is reviewed with the source/model change it describes.

---

## 🔗 Resources & Support

- [FSM_COS](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS) · [FSM_API](https://github.com/TrentBest/FSM_API) · [FSM_Serialization](https://github.com/TrentBest/TheSingularityWorkshop.FSM_Serialization) · [WebPage](https://github.com/TrentBest/WebPage)
- [GitHub](https://github.com/TrentBest) · [Coder Legion](https://coderlegion.com/) · [Profile](https://coderlegion.com/user/The%2BSingularity%2BWorkshop) · [Patreon](https://www.patreon.com/c/TheSingularityWorkshop) · [PayPal](https://www.paypal.com/donate/?hosted_button_id=3Z7263LCQMV9J)

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
