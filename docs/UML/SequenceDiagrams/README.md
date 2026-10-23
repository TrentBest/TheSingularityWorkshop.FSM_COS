# Sequence Diagrams

Manually authored sequence diagrams for source-reviewed FSM_COS workflows. These are not generated call graphs or runtime traces.

| ID | Workflow | Coverage | Status |
|---|---|---|---|
| [SEQ-FSMCOS-001](FsmCos-Execute.md) | Compose a runtime from a manifest | Ordinary composition success, failure compensation, static complexity notes | Source-reviewed; tests not run for this documentation change |
| [SEQ-FSMCOS-002](FsmCos-Published-PreResolved.md) | Restore a published, pre-resolved runtime | Flat dependency closure, published load order, exact resolved configuration verification, recorded arbitration round | Conditional source path reviewed; tests not run for this documentation change |\n| [SEQ-FSMCOS-003](RuntimeExperienceHost-Selection.md) | Prepare, select, and retire runtime experiences | Compose/register separation, retained selections, duplicate IDs, removal semantics, complexity notes | Source-reviewed against host, registry, assembly, and test source; tests not run for this documentation change |

Coverage is intentionally partial. SEQ-FSMCOS-003 now covers host experience selection and registry retention; other important workflows still need focused diagrams. The published/pre-resolved diagram applies only when `WORKSHOP_USE_REPOSITORY_SOURCES` is enabled; neither diagram is generated or freshness-checked by CI.
