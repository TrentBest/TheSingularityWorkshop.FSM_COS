# AI Composition with FSM_COS

## Purpose

FSM_COS should eventually consume ProtocolAI and GrammarAI as composable capabilities.

It should not absorb their responsibilities.

The intended stack is:

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

The clipboard round trip is valuable because it proves that the semantic protocol does not require a network connection.

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

