# Living UML and Data-Card Visual Standard

This repository follows the Workshop's GUI data-card direction: **a picture should explain the structure, and a living picture should let readers follow the meaning as it changes.**

The canonical design rationale and broader interaction contract live in the GUI repository's [UML-Inspired Data Cards](https://github.com/TrentBest/TheSingularityWorkshop.GUI/blob/development/docs/DATA_CARDS_AND_UML.md). This page records the practical rules for FSM_COS documentation and for future Workshop repositories adopting the same style.

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
