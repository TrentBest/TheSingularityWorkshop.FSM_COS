<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# RuntimeExperienceRegistry — UML neighborhood

**Evidence class:** source-checked declaration and synchronization behavior; use cases authored. **Target:** `TheSingularityWorkshop.FSM_COS.RuntimeExperienceRegistry`. **Source revision:** `development`; manually maintained.

```mermaid
flowchart TB
    Consumer["RuntimeExperienceHost / host caller"]
    Target["RuntimeExperienceRegistry"]
    Assembly["RuntimeAssembly"]
    Gate["private synchronization gate"]
    Consumer -->|"register / lookup / select / remove"| Target
    Target -->|"retains by RuntimeId"| Assembly
    Target -->|"guards shared state"| Gate
```

## Use cases

1. Register multiple composed assemblies without automatically activating them.
2. Select an existing runtime identity; an unknown identity leaves the selection unchanged.
3. Remove an entry without implying bundle disposal or unload.

## Lifecycle state

```mermaid
stateDiagram-v2
    [*] --> Registered: TryRegister succeeds
    Registered --> Active: TryActivate(id)
    Active --> Registered: ClearActive
    Active --> Removed: TryRemove(active id)
    Registered --> Removed: TryRemove(id)
    Removed --> [*]
```

The state view models registry metadata only. It does not represent MicroBundle activation, execution, disposal, or unloading.

See [source page](RuntimeExperienceRegistry.md) and [state/lifecycle standard](../../Standard/STATE_LIFECYCLE.md).

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
