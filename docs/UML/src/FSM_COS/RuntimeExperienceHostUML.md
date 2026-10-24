<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# RuntimeExperienceHost — UML neighborhood

**Evidence class:** source-checked declaration relationships; use cases authored from XML documentation and implementation. **Target:** `TheSingularityWorkshop.FSM_COS.RuntimeExperienceHost`. **Source revision:** `development`; manually maintained.

```mermaid
flowchart TB
    Consumer["Application host"]
    Target["RuntimeExperienceHost"]
    Composer["IFsmCos"]
    Registry["RuntimeExperienceRegistry"]
    Manifest["RuntimeManifest"]
    Config["IMicroBundleConfigurationSource (optional)"]
    Assembly["RuntimeAssembly"]
    Consumer -->|"coordinates"| Target
    Target -->|"composition collaborator"| Composer
    Target -->|"owns registry reference"| Registry
    Target -->|"ComposeAndRegister input"| Manifest
    Target -->|"optional input"| Config
    Composer -->|"Execute result"| Assembly
    Registry -->|"retains assembly"| Assembly
```

## Use cases

1. Compose and register an experience without changing the currently selected experience.
2. Activate an already registered experience by runtime ID.
3. Clear the selection while retaining composed experiences.

## Limits

The diagram does not imply that this class executes MicroBundles or owns their lifecycle. See [source page](RuntimeExperienceHost.md) and [runtime boundary](../../../../RUNTIME_BOUNDARY.md).

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
