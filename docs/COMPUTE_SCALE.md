# Compute Scale and Distributed Execution

FSM_COS defines a **composition boundary**, not a machine boundary.

That distinction is what allows the same composed functionality to be assembled for a desktop application, a browser, a Web application, an immersive environment, or a distributed collection of compute participants.

The important architectural question is therefore not:

> Where does FSM_COS run?

It is:

> **Where is the composition boundary, and what environment consumes the resulting RuntimeAssembly?**

## The scale axis

FSM_COS should be usable anywhere a runtime can:

1. receive a Runtime Manifest;
2. resolve the requested MicroBundles and their dependencies;
3. load the resulting capabilities;
4. arbitrate them to convergence; and
5. hand the stable RuntimeAssembly to an execution environment.

Conceptually:

~~~text
                    Runtime Manifest
                           |
                           v
                      +---------+
                      | FSM_COS |
                      +---------+
                           |
                    RuntimeAssembly
                           |
        +------------------+------------------+
        |                  |                  |
        v                  v                  v
     AnyApp            WebPage/WebApp        MyVR
     desktop              web              immersive
        |                  |                  |
        +------------------+------------------+
                           |
                           v
                    another environment
~~~

These are **manifestation/execution environments**, not layers in a dependency hierarchy.

## FSM_COS is the alignment boundary

A capability should not need to know whether its composition will ultimately execute:

- inside a Windows desktop process;
- in a browser;
- behind a web application;
- inside a VR experience;
- on a remote machine;
- across several cooperating machines;
- or in an environment that has not been invented yet.

The composition contract remains the same.

That gives the Workshop a powerful invariant:

~~~text
              capability definition
                       |
                       v
                MicroBundleDomain
                       |
                       v
                    FSM_COS
                       |
             RuntimeAssembly
                       |
       +---------------+---------------+
       |               |               |
       v               v               v
    local           remote          distributed
   execution       execution        execution
~~~

The environment changes. The composition boundary does not.

## AnyApp, WebPage, WebApp, and MyVR

These names are useful because they describe different ways of encountering or executing a composed system.

### AnyApp

AnyApp is a desktop host.

It provides a local process, desktop lifecycle, and platform manifestation. It can select a manifest, invoke FSM_COS, receive a RuntimeAssembly, and execute that composition locally.

AnyApp should therefore depend **on** the composition boundary.

FSM_COS should not depend on AnyApp.

### WebPage

WebPage is a browser-facing manifestation and proving ground.

It can perform the same composition operation and hand the resulting assembly to browser-oriented providers, GUI systems, or other web capabilities.

WebPage is not a special version of FSM_COS.

It is another consumer of it.

### WebApp

A WebApp can use the same boundary without inheriting the browser's execution assumptions.

This distinction becomes useful when a system has both:

~~~text
browser/client
      |
      v
    WebApp
      |
      v
   FSM_COS
      |
      v
RuntimeAssembly
~~~

The web application can therefore compose capabilities independently of how its client presents them.

### MyVR

MyVR is an immersive execution environment.

Its significance is not that VR requires a different composition model.

Its significance is that **the same composition model survives a radically different manifestation environment**.

A MicroBundle should not become a native host-only concept merely because MyVR happens to use native host or another immersive runtime.

## DistributedApp

**DistributedApp is a proposed sibling execution model for computational sharing.**

It is deliberately not another layer between FSM_COS and the application.

Instead, it answers a different question:

> **What if the computation required by a composed runtime is intentionally shared across more than one execution environment?**

That gives us a topology such as:

~~~text
                         Runtime Manifest
                                |
                                v
                           FSM_COS
                                |
                         RuntimeAssembly
                                |
                    DistributedApp boundary
                                |
             +------------------+------------------+
             |                  |                  |
             v                  v                  v
          AnyApp             MyVR              WebApp
        local compute      local compute      local compute
             |                  |                  |
             +---------- shared capabilities -----+
                                |
                                v
                       remote/pooled compute
~~~

The key is that **DistributedApp does not own the MicroBundle contract and does not replace FSM_COS**.

It provides the machinery for deciding *where* computation can occur and how participating environments cooperate.

## MyVR using AnyApp

This is where the distinction becomes especially useful.

We should avoid an architecture that says:

~~~text
MyVR
  |
  v
AnyApp
~~~

because that makes a particular desktop host a dependency of an immersive host.

Instead:

~~~text
              MyVR
                |
                | participates
                v
         +---------------+
         | DistributedApp|
         +---------------+
                |
        computational sharing
                |
                v
             AnyApp
~~~

Now AnyApp is not being embedded inside MyVR.

AnyApp is a **peer compute participant**.

MyVR can ask for computation that happens to be available through another participant. The DistributedApp boundary describes that relationship without requiring MyVR to understand the implementation details of AnyApp.

That gives us a much stronger statement:

> **MyVR does not use AnyApp. MyVR participates in a distributed computation in which an AnyApp host may be one of the available compute participants.**

That distinction preserves replaceability.

Tomorrow the participant could be:

- another AnyApp instance;
- a server;
- a cloud process;
- a workstation;
- a WebApp;
- another VR client;
- a specialized accelerator;
- or an entirely new execution environment.

The computation remains shareable because the composition boundary remains aligned.

## Distributed does not mean every bundle becomes remote

DistributedApp should not imply that every MicroBundle is automatically distributed.

A composition may contain:

~~~text
Local
  ├── input
  ├── rendering
  └── interaction

Shared
  ├── simulation
  ├── physics
  └── world state

Remote
  ├── expensive analysis
  └── specialized computation
~~~

The distribution policy belongs outside the fundamental MicroBundle contract.

FSM_COS determines **what the composition is**.

A distributed execution system can determine **where parts of that composition execute**.

That separation is critical.

## Composition versus placement

This produces a useful two-axis model:

~~~text
                         WHERE?
                           |
              local ------+------ distributed
                           |
                           |
                  +--------+--------+
                  |     FSM_COS     |
                  |  WHAT EXISTS?   |
                  +--------+--------+
                           |
                           |
                        WHAT?
                           |
                  capability composition
~~~

FSM_COS answers:

> What must exist together for this runtime?

DistributedApp can answer:

> Which participating compute environment should perform which work?

Those are related problems, but they are not the same problem.

## The architecture this enables

The resulting Workshop topology can be described as:

~~~text
                         THE SINGULARITY WORKSHOP
                                  |
                    +-------------+-------------+
                    |                           |
              composition                  execution
                    |                           |
                 FSM_COS              +---------+---------+
                    |                  |    |    |    |   |
                    |                  v    v    v    v   v
                    |               AnyApp WebPage WebApp MyVR ...
                    |                  \    |    /    /
                    |                   \   |   /    /
                    |                    DistributedApp
                    |                           |
                    +---------------------------+
                              shared computation
~~~

The execution environments remain siblings.

DistributedApp provides a **computational sharing topology**, not a superclass for those environments.

## Why this matters to the pitch

This is much larger than “a state machine library” or even “a MicroBundle runtime.”

The pitch becomes:

> **FSM_COS is a portable composition boundary for software.**
>
> Define functionality once. Compose it into a stable runtime. Then let that runtime be executed wherever computation needs to happen — desktop, web, immersive, server, or distributed.

And the scale story becomes:

~~~text
one process
    ↓
many processes
    ↓
many machines
    ↓
many environments
    ↓
one composed system
~~~

The architecture does not need a different composition model at each scale.

**That is the point of the boundary.**

## Current status

DistributedApp is a **design direction**, not a completed package contract.

This document intentionally does not invent:

- a transport protocol;
- a scheduling algorithm;
- a remote procedure call model;
- a serialization format;
- a network topology;
- a security model;
- a workload placement algorithm;
- or a new MicroBundle interface.

Those are implementation questions to solve only after the computational-sharing boundary is sufficiently understood.

The architectural invariant comes first:

> **FSM_COS defines composition. Execution environments define manifestation. DistributedApp defines how computation may be shared across execution environments.**

That keeps the system open-ended without making FSM_COS responsible for everything.
