# What Is FSM_COS? A First Visit

You do not need to know how to program to start understanding FSM_COS. Begin with the problem it solves: **how can a program gather the right pieces of behavior without hard-wiring every possible combination into one giant application?**

FSM_COS is The Singularity Workshop's **runtime composition kernel**. In plain language, it helps a host put compatible pieces together for a requested experience. It does not, by itself, create the whole experience or run the finished application.

## Start with a familiar problem

Imagine you are setting up a workshop for a particular job. You need a set of tools, and some tools require other tools to be available first. You want to describe the job, find the necessary pieces, give them a defined opportunity to reconcile their shared setup, and then hand the prepared setup to the person who will actually do the work.

A software system faces a similar problem. One experience might need several independent capabilities. Some capabilities depend on others. Different hosts may want different combinations.

Without a separate composition step, each application can end up repeating the rules for finding dependencies and wiring capabilities together. FSM_COS gives that composition work a defined home.

The workshop analogy has a limit: FSM_COS is software with specific contracts and algorithms, not a human making judgment calls. The analogy is only a starting point for understanding the responsibilities.

## The idea in one picture

```text
A request for capabilities
          |
          v
   Runtime Manifest
   "I need these pieces"
          |
          v
       FSM_COS
   - finds requested bundles
   - resolves their dependencies
   - loads the composition
   - runs bounded arbitration
          |
          v
    RuntimeAssembly
   "Here is the assembled result"
          |
          v
   The host application
   decides how to use it
```

The manifest describes the request. FSM_COS composes the requested pieces and their dependencies. The resulting `RuntimeAssembly` is handed to the host, which owns the application experience and what happens next.

## Five terms, in everyday language

| Term | Plain-language meaning |
|---|---|
| **Host** | The application or program that provides the overall experience. It might be a desktop app, website, service, simulator, or something else. |
| **MicroBundle** | A unit of behavior that follows the MicroBundle contract. Think of it as a defined capability that can participate in a composition. It is not simply any small NuGet package. |
| **Runtime Manifest** | The request describing which MicroBundles and versions are wanted. It says what is requested, not every detail of how the host presents it. |
| **Dependency** | Another piece that must be available for a requested piece to be composed correctly. |
| **RuntimeAssembly** | The composition result FSM_COS hands to the host. It is a handoff, not a finished application. |

The MicroBundle contract belongs to the separate **MicroBundleDomain** package. FSM_COS consumes that contract; it does not redefine the entire MicroBundle domain.

## A small conceptual example

Suppose a host wants to provide a map experience. For the sake of this example, imagine that the requested map capability depends on a separate capability that supplies map data.

1. The manifest requests the map capability.
2. The catalog gives FSM_COS a way to locate the requested MicroBundles.
3. FSM_COS discovers that the map capability depends on the map-data capability.
4. It resolves the dependency and composes the pieces in dependency order.
5. FSM_COS produces a `RuntimeAssembly` for the host.
6. The host decides how to display the map and how people interact with it.

This is an **illustrative scenario**, not a claim that this repository ships a map MicroBundle. The point is the relationship between a request, its dependencies, the composition step, and the host's responsibility.

## What FSM_COS does—and does not do

**It does:**
- resolve requested MicroBundles and their dependencies through a supplied catalog;
- apply available configuration through the supported composition path;
- load the composition in dependency order;
- perform bounded arbitration, giving participants a limited opportunity to reconcile;
- return a `RuntimeAssembly` to the caller.

**It does not automatically:**
- find or download artifacts from the internet; the host or a repository integration must provide discovery and retrieval;
- turn arbitrary libraries into MicroBundles;
- guarantee that unrelated capabilities are compatible simply because they can be loaded;
- create a user interface, run an application's main loop, or decide how a host presents an experience;
- make every proposed future integration a current feature.

These limits are intentional. Keeping the composition kernel focused makes it possible for different hosts to use it without forcing every host to adopt the same UI, storage system, or execution model.

## How to try something real

If you have Git and the **.NET 8 SDK** installed, clone the source repository and run its minimal consumer:

```bash
git clone https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS.git
cd TheSingularityWorkshop.FSM_COS
dotnet run --project samples/FSM_COS.MinimalConsumer/FSM_COS.MinimalConsumer.csproj
```

The sample creates an in-memory catalog with a report bundle that depends on a data-source bundle. You should see loading messages followed by:

```text
Assembly order: 2 -> 1
```

That output is the important result: FSM_COS loaded the prerequisite before the requested report. The catalog and example bundles are defined by the sample; they are not extra built-in types supplied by FSM_COS.

If you want to verify the behavior with a focused test instead, run:

```bash
dotnet test tests/FSM_COS.Tests/FSM_COS.Tests.csproj --filter "FullyQualifiedName~Execute_loads_dependencies_before_requesting_bundle"
```

This test checks the same dependency-ordering rule. FSM_COS is infrastructure, so its proof is a correct composition handoff—not a window or a screen. To build your own consumer, continue with [Consuming MicroBundles](CONSUMING_MICROBUNDLES.md), which explains the catalog, manifest, configuration, and bundle contracts in practical terms.

## A two-minute exercise

**Without writing code:** think of an experience you know—such as a report, a map, or a classroom page. Name one capability the experience requests, one other capability it might depend on, and the host responsibility that remains outside composition (for example, showing the result on screen). There is no single correct answer; the goal is to practice separating the request, its prerequisites, and the host.

**If you have the .NET 8 SDK:** run the sample above, then open `samples/FSM_COS.MinimalConsumer/Program.cs`. In the `RuntimeManifest`, change the requested root from `reportBundle.Id` to `dataBundle.Id` and run the sample again. The assembly should contain only bundle `2`, because the request no longer asks for the report bundle. Restore the original line afterwards. This small experiment changes the request, not the composition algorithm.

## A learning path, at your own pace

<details>
<summary><strong>I'm completely new to software</strong></summary>

Start here, then read [FSM_COS Theory](THEORY.md) for the reasoning behind the composition boundary. You do not need to memorize the names of every interface. Focus first on the distinction between **requesting pieces**, **assembling pieces**, and **using the result**.
</details>

<details>
<summary><strong>I understand applications, but not this architecture</strong></summary>

Read [Architecture](ARCHITECTURE.md), then [Runtime Manifest](RUNTIME_MANIFEST.md) and [RuntimeAssembly](RUNTIME_ASSEMBLY.md). Together they explain what enters the kernel, what it does, and what leaves it.
</details>

<details>
<summary><strong>I want to write code that uses FSM_COS</strong></summary>

Follow [Consuming MicroBundles](CONSUMING_MICROBUNDLES.md), then use [Development](DEVELOPMENT.md) to build and test the repository. The code examples and tests are the place to verify exact signatures and behavior.
</details>

<details>
<summary><strong>I need the precise limits and design rationale</strong></summary>

Read [Runtime Boundary](RUNTIME_BOUNDARY.md), [Arbitration and Convergence](ARBITRATION.md), and [Staged Manifest Loading](STAGED_MANIFEST_LOADING.md). The staged-loading guide explicitly distinguishes the current contract slice from a complete integrated staged-loading pipeline.
</details>

## The main takeaway

FSM_COS separates **what a host asks for**, **how the necessary pieces are composed**, and **what the host does with the result**. That separation is the value. It gives developers a reusable composition boundary without pretending that composition alone solves discovery, compatibility, presentation, execution, or every other problem in an application.

---

<p align="center"><em>The Singularity Workshop — Tools for the curious, the bold, and the systemically inclined.</em><br><strong>Because state shouldn't be a mess.</strong><br><em>And because static boundaries are invitations to cause trouble.</em></p>
