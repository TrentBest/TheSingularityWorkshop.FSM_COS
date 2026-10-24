<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# IMicroBundleRollback

**Purpose.** Optional compensation contract for a MicroBundle that can undo side effects from its own load attempt.

**Why it exists.** Composition can fail after one or more load attempts. FSM_COS can call compensating callbacks in reverse load-attempt order, but compensation is best-effort and is not transactional atomicity.

**Declaration.** `public interface IMicroBundleRollback` in `src/FSM_COS/IMicroBundleRollback.cs`.

## Contract card

| Member | Contract |
|---|---|
| `RollbackLoad(IMicroBundleLoadContext context)` | Attempts to undo side effects associated with the corresponding load attempt. |

## Relationships

- Depends on `IMicroBundleLoadContext` for the rollback context.
- Invoked by the composition failure/rollback path; confirm the exact call site in the current source before treating a sequence as fully source-derived.
- Optional: bundles that do not implement the contract cannot be compensated through this interface.

## Use cases

- **Actor:** host/composition workflow. **Goal:** reduce residual side effects after a later load or arbitration failure. **Outcome:** each participating rollback implementation gets a best-effort callback in reverse attempt order.
- **Failure/limit:** callback implementations may themselves fail or be incomplete; rollback does not guarantee atomicity.

## Evidence

- Source: [IMicroBundleRollback.cs](../../../../src/FSM_COS/IMicroBundleRollback.cs)
- Provenance: source-checked against `development`; manually authored documentation.
- Scope limitation: no runtime trace or performance measurement attached.
- Related standard: [class diagrams](../../Standard/CLASS_DIAGRAMS.md), [sequence and complexity](../../Standard/SEQUENCE_DIAGRAMS_AND_COMPLEXITY.md).

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
