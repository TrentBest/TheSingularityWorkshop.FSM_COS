# RuntimeCompositionResult

**Purpose.** Non-throwing composition outcome that reports success or preserves failure phase and exception.

**Declaration.** `public sealed class RuntimeCompositionResult` and `public enum RuntimeCompositionPhase` in `src/FSM_COS/RuntimeCompositionResult.cs`.

## Contract card

| Member | Meaning |
|---|---|
| `Succeeded` | True when an assembly is present. |
| `Assembly` | Composed `RuntimeAssembly`, or null on failure. |
| `FailurePhase` | Failed composition phase, or null on success. |
| `Exception` | Preserved exception, or null on success. |
| `RuntimeCompositionPhase` | Distinguishes request validation, dependency resolution, bundle loading, and arbitration. |

Construction and `Success`/`Failed` factories are private/internal implementation details; callers consume results from the public composition API.

## Relationships

- Result references `RuntimeAssembly` on success.
- Failure metadata references `RuntimeCompositionPhase` and `Exception`.
- A failure result does **not** guarantee rollback of external side effects.

## Use cases

- **Actor:** host application. **Goal:** inspect composition outcome without handling an exception as the only control path. **Outcome:** success carries an assembly; failure carries phase and original exception.
- **Failure path:** inspect `FailurePhase` and `Exception`; do not infer cleanup succeeded.

## Evidence

- Source: [RuntimeCompositionResult.cs](../../../../src/FSM_COS/RuntimeCompositionResult.cs)
- Provenance: source-checked against `development`; manually authored documentation.
- Related standard: [class diagrams](../../Standard/CLASS_DIAGRAMS.md), [sequence and complexity](../../Standard/SEQUENCE_DIAGRAMS_AND_COMPLEXITY.md).
