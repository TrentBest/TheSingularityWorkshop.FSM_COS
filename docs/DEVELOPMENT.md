# FSM_COS Development

## Repository rule

This repository is the canonical home for the FSM_COS composition kernel, its tests, package definition, workflow, and FSM_COS-specific documentation.

The implementation project is:

```text
src/FSM_COS/FSM_COS.csproj
```

The test project is:

```text
tests/FSM_COS.Tests/FSM_COS.Tests.csproj
```

The solution files must reference those projects. There should not be a second placeholder implementation project.

## Current package line

The current package is:

```text
TheSingularityWorkshop.FSM_COS
0.1.0-alpha.5
```

The package currently consumes:

```text
TheSingularityWorkshop.FSM_API       1.0.13
TheSingularityWorkshop.MicroBundleDomain 1.0.1
```

Those are runtime dependencies because the composition kernel actually consumes their contracts.

Other ecosystem packages should remain outside the runtime dependency graph unless a future FSM_COS contract genuinely requires them.

See [Dependency & Boundary Guide](DEPENDENCIES.md).

## Build and test

From the repository root:

```text
dotnet restore TheSingularityWorkshop.FSM_COS.sln
dotnet build TheSingularityWorkshop.FSM_COS.sln --configuration Release
dotnet test TheSingularityWorkshop.FSM_COS.sln --configuration Release
```

The packaging workflow also produces coverage and a NuGet artifact before the explicit publication step.

## Documentation discipline

Documentation is part of the contract.

When a public composition boundary changes, update:

1. implementation;
2. tests;
3. architecture documentation;
4. README usage and dependency material;
5. any document whose examples or terminology became stale.

A documentation statement that describes a previous API is not harmless. It is another copy of the contract and can teach consumers to use the wrong architecture.

## Dependency discipline

Before adding a package reference, answer:

1. What contract from the package does FSM_COS actually require?
2. Is that contract part of composition rather than host implementation?
3. Can the concern be supplied through an adapter or input boundary?
4. Would the new reference pull storage, transport, GUI, serialization, or host lifecycle into the kernel?
5. Would the referenced package remain independent of FSM_COS?

If a lower-level package would need to reference FSM_COS to support the dependency, the direction is wrong.

## Tests are architectural evidence

Tests should establish the observable composition contract:

- root resolution;
- dependency ordering;
- duplicate dependency handling;
- cycle detection;
- missing bundle handling;
- configuration propagation;
- arbitration convergence;
- arbitration round counting;
- non-convergence failure;
- RuntimeAssembly contents.

A test that exposes an ambiguous contract is a reason to clarify the contract, not to weaken the assertion.

## Documentation standard for FSM_COS

A reader should be able to answer these questions without opening another repository:

- What does FSM_COS do?
- What does it deliberately not do?
- What enters the kernel?
- What leaves the kernel?
- How are dependencies resolved?
- How is configuration transported?
- What does arbitration mean?
- What constitutes convergence?
- What is RuntimeAssembly?
- Why does FSM_COS depend on FSM_API?
- Why does it depend on MicroBundleDomain?
- Why does it not depend on repository, REST, serialization, GUI, or host packages?

The neighboring package should explain its own domain in full. FSM_COS should explain **its use of that domain**.

## Alpha development pattern

A useful change should normally have this shape:

```text
contract
   ↓
small implementation
   ↓
focused tests
   ↓
documentation
   ↓
package
```

Do not add host-specific infrastructure merely because a consuming application currently needs it.

Do not publish a package until the verified package artifact and its dependency graph match the documented architecture.

## Future work

Potential future slices include:

- richer catalog/resolver contracts;
- Warehouse-backed resolution;
- version compatibility;
- deterministic manifest validation;
- compact/binary manifest publication;
- explicit configuration conflict policy;
- host handoff/execution contracts.

Those are future contracts, not assumptions that should silently become part of the alpha kernel.
