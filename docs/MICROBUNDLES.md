# MicroBundles

A **MicroBundle** is a focused unit that participates in runtime composition.

“Micro” describes the **focus of responsibility**, not a promise about byte size.

![MicroBundle cartridge](assets/microbundle-cartridge.svg)

## The composition contract

The current alpha contract is:

```csharp
public interface IMicroBundle
{
    ulong Id { get; }
    IReadOnlyList<BundleRequest> Dependencies { get; }
    void Load(MicroBundleLoadContext context);
    bool Arbitrate(ArbitrationContext context, int roundIndex);
}
```

A MicroBundle tells FSM_COS:

1. **who it is**;
2. **what it depends on**;
3. **how it installs itself into the composition**;
4. **whether arbitration changed the composition**.

It does not need to know whether the final runtime is a browser, desktop application, Unity experience, or something that does not exist yet.

## MicroBundle identity

The `Id` is the machine-oriented identity used by the composition system.

A human-readable name can exist in authoring or catalog systems, but the alpha composition contract does not require FSM_COS to carry human-readable naming through the runtime.

That keeps the kernel focused on composition rather than metadata management.

## Dependencies

A bundle can request other bundles:

```text
Workshop Experience
├── GUI
│   └── FSM_API
└── Physics
    └── FSM_API
```

FSM_COS resolves the graph and prevents duplicate installation of the same bundle ID.

A missing bundle or dependency cycle is an explicit composition failure.

## Configuration

A dependency request can carry configuration:

```text
A
└── requests B + configuration X
```

FSM_COS transports X to B.

B owns the meaning of X.

This is important: the composition kernel should not have to understand every domain-specific configuration format.

## Load is installation

`Load()` establishes the bundle inside the assembled composition.

It is **not** an application-start callback.

A MicroBundle should not use `Load()` to:

- start an Experience;
- render a GUI;
- open a browser;
- create a Unity scene;
- schedule arbitrary processes;
- assume a particular host.

Those operations belong beyond the composition boundary.

## Arbitration

After all reachable bundles have loaded, FSM_COS enters arbitration.

A bundle can inspect the shared composition through `ArbitrationContext` and report whether its participation changed that composition.

```text
load
  ↓
arbitrate
  ↓
changed?
 ├── yes → another round
 └── no  → stable
```

See [Arbitration and Convergence](ARBITRATION.md).

## A useful mental model

Think of a MicroBundle as a **sealed capability cartridge**:

- the catalog locates it;
- FSM_COS installs it;
- dependencies tell FSM_COS what else must be installed;
- configuration arrives with the request;
- arbitration lets it participate in composition;
- the host eventually decides what the assembled capability means to a user.

That separation is what lets the same composition kernel serve multiple hosts.

## Related concepts

- [Runtime Manifest](RUNTIME_MANIFEST.md)
- [RuntimeAssembly](RUNTIME_ASSEMBLY.md)
- [FSM_COS Theory](THEORY.md)
