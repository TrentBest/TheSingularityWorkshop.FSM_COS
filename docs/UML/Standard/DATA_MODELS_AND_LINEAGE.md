# Data Models, Flow, and Lineage

## Purpose
Answer: **What shape is the data, who owns it, how is it validated/transformed/serialized, and where did it come from?**

Document schema and type identity, required/optional fields, defaults, invariants, nullability, mutability, ownership/lifetime, copying/aliasing, validation, serialization format, versioning/compatibility, and error behavior as applicable. Show transformations and boundaries from input to output with links to the responsible code and tests.

For serialized data, distinguish in-memory model, wire/storage representation, and reconstructed model. Identify lossy conversions, ordering guarantees, unknown-field behavior, and version compatibility when applicable. Do not infer schema guarantees solely from a sample payload.

## Acceptance
- Ownership and mutation rules are explicit.
- Input/output and transformation edges have provenance.
- Serialization and compatibility claims link to code/tests.
- Sensitive data and trust boundaries are identified where relevant.
