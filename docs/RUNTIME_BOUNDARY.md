<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# FSM_COS Runtime Boundary

**FSM_COS composes a runtime; the host runs and presents it.** This page is the explicit boundary checklist. For the domain model, see [Domain](DOMAIN.md); for the execution path, see [Architecture](ARCHITECTURE.md).

![FSM_COS runtime boundary: the composition kernel returns RuntimeAssembly to host-owned execution, rendering, communication, and platform integration.](assets/runtime-boundary.svg)

## The handoff

```text
RuntimeManifest + host-supplied catalog + optional configuration
                              │
                              ▼
                           FSM_COS
             resolve → validate → load → arbitrate
                              │
                              ▼
                       RuntimeAssembly
                              │
                              ▼
                host / application / Experience
```

## Inside FSM_COS

The current composition path:

1. Resolves requested MicroBundles by identity and version through the supplied catalog.
2. Discovers and validates the reachable dependency closure before bundle loading.
3. Obtains optional configuration through the supplied configuration-source contract.
4. Loads prerequisites before dependent bundles.
5. Runs bounded arbitration and returns `RuntimeAssembly` only after convergence.

The collection views on the alpha.7 result and plan types are read-only snapshots of collection structure. They do not make the domain-owned MicroBundle instances immutable.

## Outside FSM_COS

| Responsibility | Owner |
|---|---|
| Finding and delivering artifacts | Host/repository/catalog integration |
| Interpreting configuration bytes | The relevant MicroBundle |
| Serializing manifests or configuration | Serialization layer, such as [FSM_Serialization](https://github.com/TrentBest/TheSingularityWorkshop.FSM_Serialization) |
| Running application work or scheduling processes | Host and its runtime infrastructure |
| Rendering GUI, browser pages, desktop windows, or Unity scenes | Host-specific presentation/integration layer |
| User interaction policy, permissions, credentials, networking, and storage | Host and its chosen infrastructure |
| Activating deferred bundles, refreshing a live assembly, replacing generations, and retirement | Not implemented by the current FSM_COS package |

FSM_COS consumes the FSM_API state/context foundation; that does not make it the FSM_API process scheduler. It consumes the MicroBundleDomain contract; that does not make it the owner of the MicroBundle domain.

## Important non-promises

- A catalog is a resolution boundary, not necessarily a downloader.
- A content-hash field is metadata; this package does not fetch and verify artifact bytes from it.
- A successful load does not prove arbitrary bundles are semantically compatible.
- Bounded arbitration provides a defined convergence step, not a sandbox or transactional rollback. On failed loading or arbitration, FSM_COS can invoke the optional `IMicroBundleRollback` compensation contract in reverse attempt order; bundles without it and irreversible external effects cannot be undone.
- A `RuntimeAssembly` is a successful composition handoff, not a running application or live-update manager.
- Planning metadata for staged loading does not activate deferred loading in the current composition path.

For threat assumptions, resource limits, and in-process trust boundaries, read [Security](../SECURITY.md). For exact staged metadata behavior, read [Staged Manifest Loading](STAGED_MANIFEST_LOADING.md).

## The invariant

**Compose the request. Return the assembled result. Leave execution and manifestation to the host.**

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
