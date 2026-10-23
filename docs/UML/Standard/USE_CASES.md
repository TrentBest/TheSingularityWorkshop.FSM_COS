# Use Cases

## Rule: one file per use case
Each use case gets its own Markdown file under a stable category directory. Maintain an index that links each case and records evidence/status. Use cases describe actor goals, not just method names. They require human-authored direction grounded in product goals and source; AST/reflection alone cannot infer user intent.

## Required fields
- **ID and title**
- **Category** and status (implemented, partial, planned, unsupported, or retired)
- **Actor/consumer and goal**
- **Trigger and preconditions**
- **Primary success path**
- **Alternative, validation, and failure paths**
- **Inputs, outputs, side effects, and boundaries**
- **Relevant API/types, MicroBundles, and dependencies**
- **Tests and other evidence**, including revision when useful
- **Known limitations and next step**

## Suggested categories
User Goals; API and Contract; Data and Serialization; Operational; Integration; Performance; Security and Abuse; Maintenance and Evolution. Add a category only when it improves discovery; do not create empty categories merely for symmetry.

## Acceptance
A reader can identify who is trying to achieve what, how the flow is entered, what success means, what can fail, and which source/tests support each implementation claim.
