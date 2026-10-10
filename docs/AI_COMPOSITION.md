# AI Composition with FSM_COS

## Purpose

Hosts may compose ProtocolAI and GrammarAI MicroBundles through FSM_COS. The kernel should not absorb their responsibilities or take a hard dependency on their concrete packages.

The following is a conceptual responsibility stack, **not a chronological execution sequence**. The future-facing layers are architectural proposals, not a claim that every capability is implemented today:

```text
Domain
  |
  v
ProtocolAI          WHAT
  |
  v
GrammarAI           HOW
  |
  v
AI Exchange         WHAT + HOW + CONTEXT + REQUEST
  |
  v
Provider Adapter    transport / authentication
  |
  v
LLM
  |
  v
validated response
  |
  v
FSM_COS             composition
  |
  v
RuntimeAssembly
  |
  v
Host / Experience
```

The exact direction of the arrows may vary by host, but the ownership boundaries should remain recognizable.

---

## Why FSM_COS should consume them

FSM_COS already answers:

> What must be assembled before a host can receive a stable runtime?

ProtocolAI and GrammarAI can become MicroBundle capabilities that contribute:

- semantic vocabularies;
- grammar definitions;
- AI-facing exchange capabilities;
- provider adapters;
- contextual interaction services.

The composition kernel should assemble those capabilities without becoming an AI framework.

For example:

```text
RuntimeManifest
    |
    +-- ProtocolAI bundle
    |
    +-- GrammarAI bundle
    |
    +-- AI Exchange bundle
    |
    +-- GUI bundle
    |
    +-- Experience bundle
    |
    v
FSM_COS
    |
    v
RuntimeAssembly
```

A different runtime may request only ProtocolAI and GrammarAI.

Another may add a connected provider.

Another may use clipboard-only interaction.

The composition mechanism remains the same.

---

## Clipboard is a host capability, not a composition primitive

A clipboard round trip can demonstrate that the semantic protocol does not require a network connection.

```text
assembled context
      |
      v
copy
      |
      v
human-facing LLM
      |
     copy
      |
      v
paste
      |
      v
host validation
```

FSM_COS should assemble the capability required to produce and consume the exchange.

It should not call the clipboard itself.

Clipboard access belongs to the host manifestation layer.

---

## Connected providers

A connected interaction introduces another MicroBundle boundary:

```text
AI Exchange
    |
    v
Provider Adapter
    |
    +-- endpoint
    +-- model
    +-- credential reference
    |
    v
LLM
```

The provider adapter owns transport.

The host owns credential acquisition and policy.

FSM_COS assembles the adapter when the manifest requests it.

This allows a runtime to remain provider-neutral at the composition level.

---

## Credential boundary

An API key must never become ordinary RuntimeManifest content.

The manifest may identify a provider capability or a credential reference, but the secret itself belongs to a secure host-owned credential boundary.

```text
RuntimeManifest
    |
    | provider = X
    | credentialRef = local-user
    v
FSM_COS
    |
    v
Provider Adapter
    ^
    |
Host credential store
    |
    +--> actual secret
```

This is a future contract.

The alpha should not invent a credential store merely to make the diagram executable.

---

## Contextual interaction at speed

The architecture should distinguish **semantic context** from **transport payload**.

A contextual session may retain:

- active protocol definitions;
- active grammar;
- selected runtime objects;
- accepted prior exchanges;
- host capabilities;
- current state;
- current request.

The provider or clipboard transport can then receive a representation of that context.

```text
persistent semantic session
          |
          +-- stable context
          |
          +-- changing delta
          |
          v
     exchange builder
          |
      +---+---+
      |       |
 clipboard  provider
      |       |
      +---+---+
          |
          v
        LLM
```

The phrase **speed** should therefore be tested at multiple levels:

- human interaction latency;
- context construction time;
- serialization time;
- provider round-trip time;
- response validation time;
- token usage;
- repeated-turn payload growth.

FSM_COS should not claim performance until those measurements exist.

---

## The architectural stop line

FSM_COS assembles the AI capabilities.

It does not become:

- the LLM;
- a provider SDK;
- a prompt editor;
- a credential vault;
- a GUI;
- a clipboard manager;
- a conversation UI;
- an Experience.

That keeps the same invariant that governs the rest of the kernel:

> **Assemble what was requested. Return a stable composition. Hand it to the host.**

---

## Source-level host composition pattern

The source-level integration exercises the intended composition boundary:

```text
RuntimeManifest
    |
    +-- AI exchange capability
    |     |
    |     +-- ProtocolAI
    |     +-- GrammarAI
    |
    +-- GUI-facing capability
    |
    v
FSM_COS
    |
    v
RuntimeAssembly
    |
    v
host
    |
    +-- shared GUI builder
    +-- clipboard
    +-- provider transport
```

The source-level proof is limited to the composition contracts and test path; it does not establish that the visible WebPage experience is fully driven by `RuntimeAssembly`. See [WebPage Integration](WEBPAGE_INTEGRATION.md) for that explicit boundary.

The architectural point is that **FSM_COS does not need to reference the AI or GUI packages to compose them**.

A host supplies an `IMicroBundleCatalog`. The catalog resolves concrete capability bundles, while FSM_COS handles dependency ordering, loading, arbitration, and the stable handoff.

A GUI-capability bundle could build a platform-neutral semantic tree using `TheSingularityWorkshop.GUI.Core`; a host-specific renderer such as Blazor could manifest that tree. Treat this as the intended integration shape until the complete host path is demonstrated.

The exchange itself can be assembled from the actual ProtocolAI and GrammarAI definitions:

```text
PROTOCOL
  [integer-backed vocabulary]

GRAMMAR
  [integer-backed structure]
```

A host could expose that deterministic representation through an **Extract** action, send it to an LLM by clipboard or provider transport, and receive a response through the host's input surface. The end-to-end interaction remains a host-level integration concern.

FSM_COS remains deliberately unaware of:

- browser clipboard APIs;
- Blazor or WPF rendering;
- provider SDKs;
- API keys or credential stores;
- LLM inference;
- arbitrary response parsing.

This source-level path is an initial proof of the composition boundary. A full host integration must separately demonstrate that manifestation and transport consume the assembled result end to end.

---

## Future MicroBundle graph

A plausible future graph is:

```text
                    RuntimeManifest
                          |
          +---------------+----------------+
          |               |                |
          v               v                v
     ProtocolAI       GrammarAI       GUI / Host
          |               |
          +-------+-------+
                  |
                  v
             AI Exchange
                  |
          +-------+-------+
          |               |
          v               v
      Clipboard      Provider Adapter
                          |
                          v
                         LLM
```

This is deliberately a **composition hypothesis**, not an alpha API promise.

The first implementation task is to make the semantic contracts stable enough that FSM_COS can consume them without redefining them.



---

## Semantic layers: from meaning to application coordination

The AI architecture is broader than the exchange transport.

The current semantic model is:

```text
ProtocolAI
    WHAT exists
       ↓
GrammarAI
    HOW identities may organize
       ↓
CommandAI
    FUNCTION assembled from grammar
       ↓
OperatingSystemAI
    DISTRIBUTION / ROUTING of commands
       ↓
AppAI
    APPLICATION-LEVEL coordination
```

This should be understood as a **capability hierarchy**, not a requirement that every interaction traverse every layer.

### ProtocolAI — meaning

ProtocolAI is the foundation.

It provides stable mappings between integer identities and application-owned semantic values. It is deliberately unaware of command execution.

### GrammarAI — legal structure

GrammarAI takes protocol identities and describes which combinations form valid semantic statements.

It is the structural strainer above the vocabulary strainer.

### CommandAI — executable semantic composition

CommandAI is a candidate layer above GrammarAI.

Its concern is not merely that a statement is grammatically valid, but that valid grammar can be assembled into a meaningful unit of functionality.

For example:

```text
Tool registration

Buttons
  A

Actions
  Click

Command
  Click(A)
```

The actual GUI object, delegate, framework event, or memory reference remains host-owned. The AI-facing representation contains semantic identities and command structure.

### OperatingSystemAI — distribution and routing

If this layer proves necessary, it belongs **above CommandAI**, not beneath it.

CommandAI answers:

> What functionality does this command represent?

An OperatingSystemAI layer would answer questions such as:

> Which command-capable domain owns this functionality?

> Which available application, process, service, or experience should receive it?

> How should a command move between those domains?

That makes it a coordinator of command-capable resources rather than another command syntax.

The name is intentionally provisional. The architectural role matters before the package name does.

### AppAI — application-level orchestration

An AppAI layer can then describe interaction across application boundaries.

For example:

```text
Application A
    ↓
extract semantic data
    ↓
AppAI
    ↓
Application B
    ↓
invoke exposed CommandAI capability
```

This is where moving data between applications, selecting capabilities exposed by those applications, and composing cross-application workflows becomes meaningful.

Again, not every host needs this layer.

---

## The important consequence: layers are interaction-dependent

A user interacting with a tool does not inherently invoke the entire AI stack.

The tooling determines the highest semantic layer required by the interaction.

```text
Simple semantic selection
    ProtocolAI

Structured statement
    ProtocolAI + GrammarAI

Tool operation
    ProtocolAI + GrammarAI + CommandAI

Cross-tool / system routing
    ... + OperatingSystemAI

Cross-application workflow
    ... + AppAI
```

This is important because it prevents the architecture from turning every interaction into a giant prompt.

The host should expose the **smallest semantic surface that can express the requested interaction**.

That is also where the token-efficiency hypothesis becomes concrete: higher layers should add only the additional vocabulary and structure required for the interaction they govern.

---

## Operational-domain extraction

An **Extract to Clipboard** action should therefore extract a semantic snapshot of the current operational domain rather than a prose description of the UI.

A tool can introduce itself when initialized:

```text
I expose:

Buttons
  A

Actions
  Click
```

The registration can become integer-backed protocol identities, after which GrammarAI can describe legal composition and CommandAI can assemble the executable semantic unit.

The resulting exchange is conceptually closer to:

```text
[Actions #]
    [Click]
        [Button A]
```

than:

```text
"Click the button named A."
```

The first form gives the model a constrained semantic address space. The second asks the model to infer relationships from language.

The architecture therefore aims to move probabilistic inference toward deterministic validation:

```text
LLM probability
      ↓
Protocol identity
      ↓
Grammar constraint
      ↓
Command composition
      ↓
host validation / authorization
      ↓
deterministic execution
```

This is the central reason for keeping the layers separate.

---

<p align="center"><em>The Singularity Workshop — Tools for the curious, the bold, and the systemically inclined.</em><br><strong>Because state shouldn't be a mess.</strong><br><em>And because static boundaries are invitations to cause trouble.</em></p>
