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
