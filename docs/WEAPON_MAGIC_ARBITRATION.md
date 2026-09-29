# Worked Example: Weapon + Magic + Elements

This example is the concrete arbitration story behind the MicroBundle model.

Imagine a runtime that already contains a **Weapons MicroBundle**. A developer now adds a **Magic MicroBundle**.

Magic requires an **Elements MicroBundle**:

~~~text
Weapons
Magic
└── Elements
~~~

The important point is that dependency loading and arbitration are **different operations**.

## 1. Loading establishes what exists

The manifest requests both the existing weapon capability and the new magic capability:

~~~text
RuntimeManifest
├── Weapons
└── Magic
    └── Elements
~~~

FSM_COS resolves the dependency closure before arbitration begins:

~~~text
Weapons
Elements
Magic
~~~

`Elements` is loaded before `Magic`, because Magic declared it as a dependency.

Each bundle is loaded once for the composition.

This is the structural phase:

> **What must exist together?**

## 2. Arbitration begins after the complete set exists

Only after loading does FSM_COS begin arbitration.

That means Magic can inspect the same installed composition that includes:

- Weapons;
- Elements;
- Magic itself.

The weapon can also observe that Magic has entered the composition.

No bundle has to directly construct or call another bundle. They participate through the shared `ArbitrationContext`.

Conceptually:

~~~text
                 ArbitrationContext
              ┌──────────────────────┐
              │ Weapons              │
              │ Elements             │
              │ Magic                │
              └──────────────────────┘
                  ▲            ▲
                  │            │
              Weapons       Magic
               arbitrate    arbitrate
~~~

This is the distinction the example is intended to make visible:

> **Dependencies establish the inventory. Arbitration reconciles the inventory.**

## 3. A real reconciliation

Suppose Weapons initially has no knowledge that elemental magic is available.

During the first arbitration round:

~~~text
Weapons
  └── sees Magic
       └── changes its composition state

Magic
  ├── sees Elements
  ├── sees Weapons
  └── changes its composition state
~~~

Because changes occurred, FSM_COS performs another round.

On the next round, both participants observe that their requirements are already satisfied:

~~~text
Weapons → unchanged
Elements → unchanged
Magic   → unchanged
~~~

A complete round with no changes means convergence.

The resulting assembly therefore represents a **stable relationship**, not merely a list of successfully loaded DLLs.

## 4. What the test proves

`WeaponMagicArbitrationTests` verifies all of the important boundaries:

1. Weapons is loaded.
2. Magic is loaded.
3. Magic's Elements dependency is loaded.
4. Elements is loaded before Magic.
5. Every bundle's `Load` operation occurs exactly once.
6. Arbitration begins only after the complete installed set exists.
7. Magic can observe both Elements and Weapons during arbitration.
8. Weapons can observe Magic during arbitration.
9. The participants make changes during the first round.
10. A subsequent unchanged round produces convergence.
11. The resulting `RuntimeAssembly` contains the complete composition.

The test is intentionally more than a unit test for a method. It is an executable architectural statement.

## 5. One important alpha boundary

The current FSM_COS API composes a **published RuntimeManifest**. It does not yet expose an API that mutates an already-running `RuntimeAssembly` in place.

Therefore, "the weapon is already installed and the developer adds magic" is represented in this example as:

> the published composition already contains Weapons, and a new manifest request introduces Magic.

That distinction matters. Incremental runtime composition may become a future capability, but it should not be smuggled into the current API merely to make the example sound more advanced than the implementation actually is.

The current contract remains deliberately clean:

~~~text
published manifest
      ↓
dependency closure
      ↓
load each bundle once
      ↓
arbitrate the complete set
      ↓
converge
      ↓
RuntimeAssembly
~~~

## The architectural lesson

The magic bundle does not need to know how FSM_COS loads things.

The weapon bundle does not need to know how Magic was authored.

Elements does not need to know either one.

They each expose their own focused responsibility and then participate in a shared composition.

That is the purpose of arbitration:

> **independently authored capabilities can discover one another through the assembled composition and reconcile toward a stable state without a universal application owner.**
