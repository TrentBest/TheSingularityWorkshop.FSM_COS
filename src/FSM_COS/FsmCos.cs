using TheSingularityWorkshop.MicroBundleDomain;

namespace TheSingularityWorkshop.FSM_COS;

/// <summary>Composition engine that assembles domain-owned MicroBundles into a runtime.</summary>
public sealed class FsmCos : IFsmCos
{
    public const int DefaultMaximumArbitrationRounds = 10;

    private readonly IMicroBundleCatalog _catalog;
    private readonly int _maximumArbitrationRounds;

    public FsmCos(
        IMicroBundleCatalog catalog,
        int maximumArbitrationRounds = DefaultMaximumArbitrationRounds)
    {
        if (maximumArbitrationRounds < 1)
            throw new ArgumentOutOfRangeException(nameof(maximumArbitrationRounds));

        _catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
        _maximumArbitrationRounds = maximumArbitrationRounds;
    }

    public RuntimeAssembly Execute(
        RuntimeManifest manifest,
        IMicroBundleConfigurationSource? configurationSource = null)
    {
        ArgumentNullException.ThrowIfNull(manifest);
        ArgumentNullException.ThrowIfNull(manifest.Bundles);

        // Snapshot caller-owned collections once so concurrent or callback-driven mutation
        // cannot change the request between validation, resolution, loading, and handoff.
        manifest = manifest with
        {
            Bundles = Array.AsReadOnly(manifest.Bundles.ToArray()),
            LoadPlan = manifest.LoadPlan is null
                ? null
                : Array.AsReadOnly(manifest.LoadPlan.ToArray())
        };
        manifest.ValidateStagedPlan();

        // Validate root identity/version requests before loading anything. Explicit root
        // versions also take precedence when those same bundles appear as dependencies.
        var requestedVersions = new Dictionary<ulong, string>();
        foreach (var entry in manifest.Bundles)
        {
            // A record struct's default value bypasses its validating constructor.
            // Reject malformed root entries here so they never reach catalog lookup.
            if (entry.BundleId == 0)
                throw new InvalidOperationException("Runtime manifest contains a root MicroBundle entry with ID 0.");
            if (string.IsNullOrWhiteSpace(entry.Version))
                throw new InvalidOperationException(
                    $"Runtime manifest root MicroBundle {entry.BundleId} has no requested version.");

            if (requestedVersions.TryGetValue(entry.BundleId, out var requestedVersion) &&
                !string.Equals(requestedVersion, entry.Version, StringComparison.Ordinal))
            {
                throw new InvalidOperationException(
                    $"Runtime manifest requests MicroBundle {entry.BundleId} at conflicting versions " +
                    $"'{requestedVersion}' and '{entry.Version}'.");
            }

            requestedVersions[entry.BundleId] = entry.Version;
        }

        var requestedRoots = new Dictionary<ulong, IMicroBundle>();
        foreach (var (bundleId, version) in requestedVersions)
        {
            if (!_catalog.TryResolve(bundleId, version, out var bundle) || bundle is null)
                throw new InvalidOperationException(
                    $"MicroBundle {bundleId} version '{version}' could not be resolved.");

            ValidateResolvedBundle(bundleId, version, bundle);
            requestedRoots.Add(bundleId, bundle);
        }

        // Resolve and validate the complete graph before any bundle Load method can have side effects.
        // Keep the exact resolved objects and dependency snapshots for both validation and loading.
        var resolvedBundles = ResolveDependencyGraph(requestedRoots, manifest.Schedule is not null, out var dependencyRequestsById);

        if (manifest.Schedule is not null)
            ValidateScheduleAgainstResolvedGraph(manifest, resolvedBundles, dependencyRequestsById);

        var loaded = new List<IMicroBundle>();
        var loadedIds = new HashSet<ulong>();
        var loading = new HashSet<ulong>();
        var loadContext = new MicroBundleLoadContext(manifest.RuntimeId);

        foreach (var entry in manifest.Bundles)
        {
            LoadRoot(
                entry,
                resolvedBundles,
                dependencyRequestsById,
                loaded,
                loadedIds,
                loading,
                loadContext,
                configurationSource,
                manifest.RuntimeId);
        }

        var arbitrationContext = new ArbitrationContext(
            manifest.RuntimeId,
            loaded,
            manifest.ExperienceContext);

        var rounds = 0;
        for (; rounds < _maximumArbitrationRounds; rounds++)
        {
            var changed = false;
            foreach (var bundle in loaded)
                changed |= bundle.Arbitrate(arbitrationContext, rounds);

            if (!changed)
                break;
        }

        if (rounds == _maximumArbitrationRounds)
            throw new InvalidOperationException(
                $"FSM_COS arbitration did not converge within {_maximumArbitrationRounds} rounds.");

        return new RuntimeAssembly(manifest.RuntimeId, loaded, rounds, manifest.Intent);
    }

    private void ValidateScheduleAgainstResolvedGraph(
        RuntimeManifest manifest,
        IReadOnlyDictionary<ulong, IMicroBundle> resolvedBundles,
        IReadOnlyDictionary<ulong, MicroBundleDependencyRequest[]> dependencyRequestsById)
    {
        var schedule = manifest.Schedule!;
        var plannedById = schedule.Entries.ToDictionary(entry => entry.Reference.BundleId);
        var resolvedIds = resolvedBundles.Keys.ToHashSet();

        if (!resolvedIds.SetEquals(plannedById.Keys))
        {
            var missing = resolvedIds.Except(plannedById.Keys).OrderBy(id => id).ToArray();
            var extra = plannedById.Keys.Except(resolvedIds).OrderBy(id => id).ToArray();
            throw new InvalidOperationException(
                $"Runtime manifest schedule does not match the resolved dependency closure. " +
                $"Missing scheduled IDs: [{string.Join(", ", missing)}]; " +
                $"unexpected scheduled IDs: [{string.Join(", ", extra)}].");
        }

        foreach (var (id, bundle) in resolvedBundles)
        {
            var planned = plannedById[id];
            if (!string.Equals(planned.Reference.Version, bundle.Descriptor.Version, StringComparison.Ordinal))
                throw new InvalidOperationException(
                    $"Runtime manifest schedule expects MicroBundle {id} version " +
                    $"'{planned.Reference.Version}', but the catalog resolved '{bundle.Descriptor.Version}'.");
        }

        var actualEdges = dependencyRequestsById.SelectMany(pair =>
            pair.Value.Select(dependency => (BundleId: pair.Key, DependencyId: dependency.BundleId))).ToHashSet();
        var plannedEdges = schedule.Dependencies
            .Select(edge => (BundleId: edge.BundleId, DependencyId: edge.DependencyId))
            .ToHashSet();
        if (!actualEdges.SetEquals(plannedEdges))
        {
            var missing = actualEdges.Except(plannedEdges).OrderBy(edge => edge.BundleId).ThenBy(edge => edge.DependencyId);
            var extra = plannedEdges.Except(actualEdges).OrderBy(edge => edge.BundleId).ThenBy(edge => edge.DependencyId);
            throw new InvalidOperationException(
                "Runtime manifest schedule dependency edges do not match the resolved MicroBundle dependency graph. " +
                $"Missing edges: [{string.Join(", ", missing.Select(edge => $"{edge.BundleId}->{edge.DependencyId}"))}]; " +
                $"unexpected edges: [{string.Join(", ", extra.Select(edge => $"{edge.BundleId}->{edge.DependencyId}"))}].");
        }
    }

    private Dictionary<ulong, IMicroBundle> ResolveDependencyGraph(
        IReadOnlyDictionary<ulong, IMicroBundle> requestedRoots,
        bool validatingPublishedSchedule,
        out Dictionary<ulong, MicroBundleDependencyRequest[]> dependencyRequestsById)
    {
        var resolvedBundles = new Dictionary<ulong, IMicroBundle>();
        dependencyRequestsById = new Dictionary<ulong, MicroBundleDependencyRequest[]>();
        var discovered = requestedRoots.ToDictionary(pair => pair.Key, pair => pair.Value);
        var pending = new Stack<ulong>();

        foreach (var id in requestedRoots.Keys.Reverse())
            pending.Push(id);

        while (pending.Count > 0)
        {
            var id = pending.Pop();
            if (resolvedBundles.ContainsKey(id))
                continue;

            var bundle = discovered[id];
            ValidateResolvedDescriptorIdentity(id, bundle);
            var declaredDependencies = bundle.Dependencies;
            if (declaredDependencies is null)
                throw new InvalidOperationException($"MicroBundle {id} returned no dependency request list.");

            // Copy the configuration bytes as well as the request structs. ReadOnlyMemory
            // prevents writes through this view; it does not make the backing array immutable.
            var dependencySnapshot = declaredDependencies
                .Select(request => new MicroBundleDependencyRequest(
                    request.BundleId,
                    request.Configuration.ToArray()))
                .ToArray();
            ValidateResolvedIdentity(id, bundle, dependencySnapshot);
            resolvedBundles.Add(id, bundle);
            dependencyRequestsById.Add(id, dependencySnapshot);

            for (var index = dependencySnapshot.Length - 1; index >= 0; index--)
            {
                var dependencyId = dependencySnapshot[index].BundleId;
                if (discovered.ContainsKey(dependencyId))
                    continue;

                if (!_catalog.TryResolve(dependencyId, out var dependencyBundle) || dependencyBundle is null)
                {
                    var kind = validatingPublishedSchedule ? "Scheduled MicroBundle" : "MicroBundle";
                    throw new InvalidOperationException($"{kind} {dependencyId} could not be resolved.");
                }

                discovered.Add(dependencyId, dependencyBundle);
                pending.Push(dependencyId);
            }
        }

        ValidateDependencyGraphAcyclic(requestedRoots.Keys, dependencyRequestsById);
        return resolvedBundles;
    }

    private static void ValidateDependencyGraphAcyclic(
        IEnumerable<ulong> rootIds,
        IReadOnlyDictionary<ulong, MicroBundleDependencyRequest[]> dependencyRequestsById)
    {
        // Iterative depth-first traversal avoids exhausting the process stack on a deep or hostile graph.
        var states = new Dictionary<ulong, byte>();
        foreach (var rootId in rootIds)
        {
            if (states.ContainsKey(rootId))
                continue;

            states[rootId] = 1;
            var stack = new List<(ulong BundleId, int NextDependencyIndex)> { (rootId, 0) };

            while (stack.Count > 0)
            {
                var (bundleId, nextDependencyIndex) = stack[^1];
                var dependencies = dependencyRequestsById[bundleId];

                if (nextDependencyIndex < dependencies.Length)
                {
                    stack[^1] = (bundleId, nextDependencyIndex + 1);
                    var dependencyId = dependencies[nextDependencyIndex].BundleId;

                    if (states.TryGetValue(dependencyId, out var state))
                    {
                        if (state == 1)
                            throw new InvalidOperationException(
                                $"MicroBundle dependency cycle detected at {dependencyId}.");
                        continue;
                    }

                    states[dependencyId] = 1;
                    stack.Add((dependencyId, 0));
                    continue;
                }

                stack.RemoveAt(stack.Count - 1);
                states[bundleId] = 2;
            }
        }
    }

    private sealed class LoadFrame
    {
        public LoadFrame(IMicroBundle bundle, ReadOnlyMemory<byte> dependencyConfiguration)
        {
            Bundle = bundle;
            DependencyConfiguration = dependencyConfiguration;
        }

        public IMicroBundle Bundle { get; }
        public ReadOnlyMemory<byte> DependencyConfiguration { get; }
        public int NextDependencyIndex { get; set; }
        public bool Entered { get; set; }
    }

    private void LoadRoot(
        MicroBundleManifestEntry entry,
        IReadOnlyDictionary<ulong, IMicroBundle> resolvedBundles,
        IReadOnlyDictionary<ulong, MicroBundleDependencyRequest[]> dependencyRequestsById,
        List<IMicroBundle> loaded,
        HashSet<ulong> loadedIds,
        HashSet<ulong> loading,
        MicroBundleLoadContext loadContext,
        IMicroBundleConfigurationSource? configurationSource,
        ulong runtimeId)
    {
        if (loadedIds.Contains(entry.BundleId))
            return;

        var stack = new Stack<LoadFrame>();
        stack.Push(new LoadFrame(resolvedBundles[entry.BundleId], ReadOnlyMemory<byte>.Empty));

        // Preserve the original depth-first semantics without using recursive calls:
        // configuration is requested on entry; Load runs only after all dependencies finish.
        while (stack.Count > 0)
        {
            var frame = stack.Peek();
            var bundle = frame.Bundle;

            if (!frame.Entered)
            {
                if (loadedIds.Contains(bundle.Id))
                {
                    stack.Pop();
                    continue;
                }

                if (!loading.Add(bundle.Id))
                    throw new InvalidOperationException(
                        $"MicroBundle dependency cycle detected at {bundle.Id}.");

                var configuration = frame.DependencyConfiguration;
                var version = bundle.Descriptor.Version;
                if (configurationSource is not null &&
                    configurationSource.TryGetConfiguration(runtimeId, bundle.Id, version, out var externalConfiguration))
                {
                    // A configuration provider may return memory over a mutable buffer.
                    // Copy it before storing it in the shared load context.
                    configuration = externalConfiguration.ToArray();
                }

                loadContext.SetConfiguration(bundle.Id, configuration);
                frame.Entered = true;
            }

            var dependencies = dependencyRequestsById[bundle.Id];
            if (frame.NextDependencyIndex < dependencies.Length)
            {
                var dependency = dependencies[frame.NextDependencyIndex++];
                if (loadedIds.Contains(dependency.BundleId))
                    continue;

                if (loading.Contains(dependency.BundleId))
                    throw new InvalidOperationException(
                        $"MicroBundle dependency cycle detected at {dependency.BundleId}.");

                stack.Push(new LoadFrame(resolvedBundles[dependency.BundleId], dependency.Configuration));
                continue;
            }

            bundle.Load(loadContext);
            loaded.Add(bundle);
            loadedIds.Add(bundle.Id);
            loading.Remove(bundle.Id);
            stack.Pop();
        }
    }

    private static void ValidateResolvedDescriptorIdentity(ulong requestedId, IMicroBundle bundle)
    {
        var descriptor = bundle.Descriptor;
        if (descriptor is null)
            throw new InvalidOperationException(
                $"MicroBundle {requestedId} returned no descriptor.");

        if (bundle.Id != requestedId)
            throw new InvalidOperationException(
                $"MicroBundle catalog resolved request {requestedId} to bundle {bundle.Id}.");

        if (descriptor.Id != requestedId)
            throw new InvalidOperationException(
                $"MicroBundle {requestedId} has a descriptor for bundle {descriptor.Id}.");
    }

    private static void ValidateResolvedIdentity(ulong requestedId, IMicroBundle bundle)
    {
        ValidateResolvedDescriptorIdentity(requestedId, bundle);
        var requests = bundle.Dependencies;
        if (requests is null)
            throw new InvalidOperationException(
                $"MicroBundle {requestedId} returned no dependency request list.");

        ValidateResolvedIdentity(requestedId, bundle, requests);
    }

    private static void ValidateResolvedIdentity(
        ulong requestedId,
        IMicroBundle bundle,
        IReadOnlyList<MicroBundleDependencyRequest> requests)
    {
        ValidateResolvedDescriptorIdentity(requestedId, bundle);

        var requestIds = new HashSet<ulong>();
        foreach (var request in requests)
        {
            if (request.BundleId == 0)
                throw new InvalidOperationException(
                    $"MicroBundle {requestedId} declares dependency ID 0.");

            if (!requestIds.Add(request.BundleId))
                throw new InvalidOperationException(
                    $"MicroBundle {requestedId} declares dependency {request.BundleId} more than once.");
        }

        var descriptorIds = bundle.Descriptor.Dependencies.Select(dependency => dependency.BundleId).ToHashSet();
        if (!descriptorIds.SetEquals(requestIds))
        {
            var missingRequests = descriptorIds.Except(requestIds).OrderBy(id => id);
            var unexpectedRequests = requestIds.Except(descriptorIds).OrderBy(id => id);
            throw new InvalidOperationException(
                $"MicroBundle {requestedId} descriptor dependency declarations do not match dependency requests. " +
                $"Missing requests: [{string.Join(", ", missingRequests)}]; " +
                $"unexpected requests: [{string.Join(", ", unexpectedRequests)}].");
        }
    }

    private static void ValidateResolvedBundle(
        ulong requestedId,
        string requestedVersion,
        IMicroBundle bundle)
    {
        ValidateResolvedIdentity(requestedId, bundle);

        if (!string.Equals(bundle.Descriptor.Version, requestedVersion, StringComparison.Ordinal))
            throw new InvalidOperationException(
                $"MicroBundle {requestedId} resolved to version '{bundle.Descriptor.Version}', " +
                $"but manifest requested '{requestedVersion}'.");
    }
}
