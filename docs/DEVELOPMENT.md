# FSM_COS Development

## Repository rule

This repository is the canonical home for the FSM_COS composition kernel, its tests, package definition, and FSM_COS-specific documentation.

The implementation project is:

~~~text
src/FSM_COS/FSM_COS.csproj
~~~

The test project is:

~~~text
tests/FSM_COS.Tests/FSM_COS.Tests.csproj
~~~

The solution files must reference those projects. There should not be a second root scaffold containing a placeholder Class1.cs project.

## Build

From the repository root:

~~~text
dotnet restore TheSingularityWorkshop.FSM_COS.sln
dotnet build TheSingularityWorkshop.FSM_COS.sln --configuration Release
dotnet test TheSingularityWorkshop.FSM_COS.sln --configuration Release
~~~

## Package

The package project is:

~~~text
src/FSM_COS/FSM_COS.csproj
~~~

The package identity is:

~~~text
TheSingularityWorkshop.FSM_COS
~~~

The current alpha line is 0.1.0-alpha.1.

The repository workflow restores, tests, and packs on pushes to master. Publishing to GitHub Packages and NuGet.org requires an explicit workflow dispatch with the publish input enabled.

## Documentation discipline

Documentation belongs with the repository whose contracts it explains.

FSM_COS-specific theory should be added under:

~~~text
docs/
~~~

Neighboring repositories may describe their relationship to FSM_COS, but they are not substitutes for FSM_COS documentation.

When a contract changes, update:

1. implementation;
2. tests;
3. the relevant FSM_COS documentation;
4. README architecture/usage material when the public boundary changes.

## Alpha development pattern

FSM_COS should evolve slice-by-slice.

A useful change should normally have this shape:

~~~text
contract
   ↓
small implementation
   ↓
focused tests
   ↓
documentation
   ↓
package
~~~

Do not add host-specific infrastructure merely because a consuming application currently needs it.

## Tests are architectural evidence

The tests should establish the semantics of:

- dependency ordering;
- duplicate dependency handling;
- cycle detection;
- missing bundle handling;
- configuration propagation;
- arbitration convergence;
- arbitration round counting;
- non-convergence;
- RuntimeAssembly contents.

A test that exposes an ambiguous contract is a reason to clarify the contract, not to weaken the assertion.

## Future work

Potential future slices include:

- richer catalog/resolver contracts;
- Warehouse-backed resolution;
- version compatibility;
- deterministic manifest validation;
- compact/binary manifest publication;
- explicit configuration conflict policy;
- integration with higher-level FSM composition;
- host handoff/execution contracts.

Those are future contracts, not assumptions that should be silently embedded in Alpha 1.


---

## 🔗 The Singularity Workshop

FSM_COS is one layer in a deliberately troublesome ecosystem:

- **[FSM_API](https://github.com/TrentBest/FSM_API)** — behavior and state.
- **[FSM_COS](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS)** — composition and runtime assembly.
- **[FSM_Serialization](https://github.com/TrentBest/TheSingularityWorkshop.FSM_Serialization)** — representation and the byte boundary.
- **[WebPage](https://github.com/TrentBest/WebPage)** — browser manifestation and proving ground.
- **[FSM_API_Unity](https://github.com/TrentBest/FSM_API_Unity)** — Unity manifestation.

<p align="center"><em>The Singularity Workshop — Tools for the curious, the bold, and the systemically inclined.</em><br><strong>Because state shouldn't be a mess.</strong><br><em>And because static boundaries are invitations to cause trouble.</em></p>
