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
        manifest.ValidateStagedPlan();

        // Validate root identity/version requests before loading anything. Explicit root
        // versions also take precedence when those same bundles appear as dependencies.
        var requestedVersions = new Dictionary<ulong, string>();
        foreach (var entry in manifest.Bundles)
        {
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

        // Validate the schedule against the same explicitly selected root versions that
        // composition will use, still before any MicroBundle Load method can have side effects.
        if (manifest.Schedule is not null)
            ValidateScheduleAgainstResolvedGraph(manifest, requestedRoots);

        var loaded = new List<IMicroBundle>();
        var loadedIds = new HashSet<ulong>();
        var loading = new HashSet<ulong>();
        var loadContext = new MicroBundleLoadContext(manifest.RuntimeId);

        foreach (var entry in manifest.Bundles)
        {
            LoadRoot(
                entry,
                requestedRoots,
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
        IReadOnlyDictionary<ulong, IMicroBundle> requestedRoots)
    {
        var schedule = manifest.Schedule!;
        var plannedById = schedule.Entries.ToDictionary(entry => entry.Reference.BundleId);
        var resolvedById = new Dictionary<ulong, IMicroBundle>();
        var actualEdges = new HashSet<(ulong BundleId, ulong DependencyId)>();
        var visiting = new HashSet<ulong>();
        var visited = new HashSet<ulong>();

        IMicroBundle Resolve(ulong id, string? requestedVersion = null)
        {
            IMicroBundle? resolved;
            if (requestedRoots.TryGetValue(id, out var requestedRoot) &&
                (requestedVersion is null ||
                 string.Equals(requestedRoot.Descriptor.Version, requestedVersion, StringComparison.Ordinal)))
            {
                resolved = requestedRoot;
            }
            else
            {
                resolved = requestedVersion is null
                    ? _catalog.TryResolve(id, out var byId) ? byId : null
                    : _catalog.TryResolve(id, requestedVersion, out var byVersion) ? byVersion : null;
            }

            if (resolved is null)
                throw new InvalidOperationException(
                    $"Scheduled MicroBundle {id}" +
                    (requestedVersion is null ? string.Empty : $" version '{requestedVersion}'") +
                    " could not be resolved.");

            if (resolved.Descriptor is null || resolved.Id != id)
                throw new InvalidOperationException(
                    $"MicroBundle catalog resolved request {id} to an invalid or mismatched bundle.");

            if (requestedVersion is not null &&
                !string.Equals(resolved.Descriptor.Version, requestedVersion, StringComparison.Ordinal))
                throw new InvalidOperationException(
                    $"MicroBundle {id} resolved to version '{resolved.Descriptor.Version}', " +
                    $"but manifest requested '{requestedVersion}'.");

            return resolved;
        }

        void Visit(IMicroBundle bundle)
        {
            if (visited.Contains(bundle.Id))
                return;
            if (!visiting.Add(bundle.Id))
                throw new InvalidOperationException(
                    $"MicroBundle dependency cycle detected at {bundle.Id}.");

            resolvedById[bundle.Id] = bundle;
            foreach (var dependency in bundle.Dependencies)
            {
                actualEdges.Add((bundle.Id, dependency.BundleId));
                if (visited.Contains(dependency.BundleId))
                    continue;

                var dependencyBundle = Resolve(dependency.BundleId);
                Visit(dependencyBundle);
            }

            visiting.Remove(bundle.Id);
            visited.Add(bundle.Id);
        }

        foreach (var root in manifest.Bundles)
            Visit(Resolve(root.BundleId, root.Version));

        var resolvedIds = resolvedById.Keys.ToHashSet();
        if (!resolvedIds.SetEquals(plannedById.Keys))
        {
            var missing = resolvedIds.Except(plannedById.Keys).OrderBy(id => id).ToArray();
            var extra = plannedById.Keys.Except(resolvedIds).OrderBy(id => id).ToArray();
            throw new InvalidOperationException(
                $"Runtime manifest schedule does not match the resolved dependency closure. " +
                $"Missing scheduled IDs: [{string.Join(", ", missing)}]; " +
                $"unexpected scheduled IDs: [{string.Join(", ", extra)}].");
        }

        foreach (var (id, bundle) in resolvedById)
        {
            var planned = plannedById[id];
            if (!string.Equals(planned.Reference.Version, bundle.Descriptor.Version, StringComparison.Ordinal))
                throw new InvalidOperationException(
                    $"Runtime manifest schedule expects MicroBundle {id} version " +
                    $"'{planned.Reference.Version}', but the catalog resolved '{bundle.Descriptor.Version}'.");
        }

        var plannedEdges = schedule.Dependencies
            .Select(edge => (edge.BundleId, edge.DependencyId))
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

    private void LoadRoot(
        MicroBundleManifestEntry entry,
        IReadOnlyDictionary<ulong, IMicroBundle> requestedRoots,
        List<IMicroBundle> loaded,
        HashSet<ulong> loadedIds,
        HashSet<ulong> loading,
        MicroBundleLoadContext loadContext,
        IMicroBundleConfigurationSource? configurationSource,
        ulong runtimeId)
    {
        if (loadedIds.Contains(entry.BundleId))
        {
            var alreadyLoaded = loaded.First(bundle => bundle.Id == entry.BundleId);
            ValidateResolvedBundle(entry.BundleId, entry.Version, alreadyLoaded);
            return;
        }

        if (!loading.Add(entry.BundleId))
            throw new InvalidOperationException(
                $"MicroBundle dependency cycle detected at {entry.BundleId}.");

        try
        {
            var bundle = requestedRoots[entry.BundleId];
            ValidateResolvedBundle(entry.BundleId, entry.Version, bundle);

            LoadBundle(
                bundle,
                ReadOnlyMemory<byte>.Empty,
                loaded,
                loadedIds,
                loading,
                loadContext,
                configurationSource,
                requestedRoots,
                runtimeId);
        }
        finally
        {
            loading.Remove(entry.BundleId);
        }
    }

    private void LoadBundle(
        IMicroBundle bundle,
        ReadOnlyMemory<byte> dependencyConfiguration,
        List<IMicroBundle> loaded,
        HashSet<ulong> loadedIds,
        HashSet<ulong> loading,
        MicroBundleLoadContext loadContext,
        IMicroBundleConfigurationSource? configurationSource,
        IReadOnlyDictionary<ulong, IMicroBundle> requestedRoots,
        ulong runtimeId)
    {
        if (loadedIds.Contains(bundle.Id))
            return;

        var version = bundle.Descriptor.Version;
        var configuration = dependencyConfiguration;

        if (configurationSource is not null &&
            configurationSource.TryGetConfiguration(runtimeId, bundle.Id, version, out var externalConfiguration))
        {
            configuration = externalConfiguration;
        }

        loadContext.SetConfiguration(bundle.Id, configuration);

        foreach (var dependency in bundle.Dependencies)
        {
            if (loadedIds.Contains(dependency.BundleId))
                continue;

            if (!loading.Add(dependency.BundleId))
                throw new InvalidOperationException(
                    $"MicroBundle dependency cycle detected at {dependency.BundleId}.");

            try
            {
                IMicroBundle? dependencyBundle;
                if (requestedRoots.TryGetValue(dependency.BundleId, out var requestedRoot))
                {
                    dependencyBundle = requestedRoot;
                }
                else if (!_catalog.TryResolve(dependency.BundleId, out dependencyBundle) ||
                         dependencyBundle is null)
                {
                    throw new InvalidOperationException(
                        $"MicroBundle {dependency.BundleId} could not be resolved.");
                }

                if (dependencyBundle is null)
                    throw new InvalidOperationException(
                        $"MicroBundle {dependency.BundleId} could not be resolved.");

                if (dependencyBundle.Descriptor is null)
                    throw new InvalidOperationException(
                        $"MicroBundle {dependency.BundleId} returned no descriptor.");

                if (dependencyBundle.Id != dependency.BundleId)
                    throw new InvalidOperationException(
                        $"MicroBundle catalog resolved request {dependency.BundleId} to bundle {dependencyBundle.Id}.");

                LoadBundle(
                    dependencyBundle,
                    dependency.Configuration,
                    loaded,
                    loadedIds,
                    loading,
                    loadContext,
                    configurationSource,
                    requestedRoots,
                    runtimeId);
            }
            finally
            {
                loading.Remove(dependency.BundleId);
            }
        }

        bundle.Load(loadContext);
        loaded.Add(bundle);
        loadedIds.Add(bundle.Id);
    }

    private static void ValidateResolvedBundle(
        ulong requestedId,
        string requestedVersion,
        IMicroBundle bundle)
    {
        if (bundle.Descriptor is null)
            throw new InvalidOperationException(
                $"MicroBundle {requestedId} returned no descriptor.");

        if (bundle.Id != requestedId)
            throw new InvalidOperationException(
                $"MicroBundle catalog resolved request {requestedId} to bundle {bundle.Id}.");

        if (!string.Equals(bundle.Descriptor.Version, requestedVersion, StringComparison.Ordinal))
            throw new InvalidOperationException(
                $"MicroBundle {requestedId} resolved to version '{bundle.Descriptor.Version}', " +
                $"but manifest requested '{requestedVersion}'.");
    }
}
