using TheSingularityWorkshop.MicroBundleDomain;

namespace TheSingularityWorkshop.FSM_COS;

/// <summary>Composition engine that assembles domain-owned MicroBundles into a runtime.</summary>
public sealed class FsmCos : IFsmCos
{
    public const int DefaultMaximumArbitrationRounds = 10;
    public const int DefaultMaximumBundleCount = 10_000;
    public const int DefaultMaximumDependencyRequestCount = 100_000;
    public const long DefaultMaximumConfigurationBytes = 64L * 1024 * 1024;

    private readonly IMicroBundleCatalog _catalog;
    private readonly int _maximumArbitrationRounds;
    private readonly int _maximumBundleCount;
    private readonly int _maximumDependencyRequestCount;
    private readonly long _maximumConfigurationBytes;

    public FsmCos(
        IMicroBundleCatalog catalog,
        int maximumArbitrationRounds = DefaultMaximumArbitrationRounds)
        : this(
            catalog,
            maximumArbitrationRounds,
            DefaultMaximumBundleCount,
            DefaultMaximumDependencyRequestCount,
            DefaultMaximumConfigurationBytes)
    {
    }

    public FsmCos(
        IMicroBundleCatalog catalog,
        int maximumArbitrationRounds,
        int maximumBundleCount,
        int maximumDependencyRequestCount)
        : this(
            catalog,
            maximumArbitrationRounds,
            maximumBundleCount,
            maximumDependencyRequestCount,
            DefaultMaximumConfigurationBytes)
    {
    }

    public FsmCos(
        IMicroBundleCatalog catalog,
        int maximumArbitrationRounds,
        int maximumBundleCount,
        int maximumDependencyRequestCount,
        long maximumConfigurationBytes)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(maximumArbitrationRounds, 1);
        ArgumentOutOfRangeException.ThrowIfLessThan(maximumBundleCount, 1);
        ArgumentOutOfRangeException.ThrowIfLessThan(maximumDependencyRequestCount, 1);
        ArgumentOutOfRangeException.ThrowIfNegative(maximumConfigurationBytes);

        _catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
        _maximumArbitrationRounds = maximumArbitrationRounds;
        _maximumBundleCount = maximumBundleCount;
        _maximumDependencyRequestCount = maximumDependencyRequestCount;
        _maximumConfigurationBytes = maximumConfigurationBytes;
    }

    public RuntimeAssembly Execute(
        RuntimeManifest manifest,
        IMicroBundleConfigurationSource? configurationSource = null) =>
        ExecuteCore(manifest, configurationSource, onStageChanged: null);

    /// <summary>
    /// Attempts composition and returns the phase and original exception when composition fails.
    /// This does not change the behavior of <see cref="Execute"/>; it is an opt-in alternative
    /// for hosts that want to report composition failures without parsing exception messages.
    /// </summary>
    public RuntimeCompositionResult TryExecute(
        RuntimeManifest manifest,
        IMicroBundleConfigurationSource? configurationSource = null)
    {
        var phase = RuntimeCompositionPhase.RequestValidation;
        try
        {
            var assembly = ExecuteCore(manifest, configurationSource, next => phase = next);
            return RuntimeCompositionResult.Success(assembly);
        }
        // The result API deliberately preserves exceptions from catalog, configuration,
        // bundle-loading, and arbitration callbacks without requiring consumers to parse messages.
#pragma warning disable CA1031 // Broad capture is the explicit TryExecute contract; Execute remains exception-based.
        catch (Exception exception)
#pragma warning restore CA1031
        {
            return RuntimeCompositionResult.Failed(phase, exception);
        }
    }

    private RuntimeAssembly ExecuteCore(
        RuntimeManifest manifest,
        IMicroBundleConfigurationSource? configurationSource,
        Action<RuntimeCompositionPhase>? onStageChanged)
    {
        onStageChanged?.Invoke(RuntimeCompositionPhase.RequestValidation);
        ArgumentNullException.ThrowIfNull(manifest);
        ArgumentNullException.ThrowIfNull(manifest.Bundles);

        // Reject oversized caller input before copying it into kernel-owned snapshots.
        if (manifest.Bundles.Count > _maximumBundleCount)
            throw new InvalidOperationException(
                $"Runtime manifest requests {manifest.Bundles.Count} root bundles; the configured maximum is {_maximumBundleCount}.");
        if (manifest.LoadPlan is { Count: var loadPlanCount } && loadPlanCount > _maximumBundleCount)
            throw new InvalidOperationException(
                $"Runtime manifest load plan contains {loadPlanCount} entries; the configured maximum is {_maximumBundleCount}.");
        if (manifest.Schedule is { } requestedSchedule)
        {
            if (requestedSchedule.Entries.Count > _maximumBundleCount)
                throw new InvalidOperationException(
                    $"Runtime manifest schedule contains {requestedSchedule.Entries.Count} entries; the configured maximum is {_maximumBundleCount}.");
            if (requestedSchedule.Dependencies.Count > _maximumDependencyRequestCount)
                throw new InvalidOperationException(
                    $"Runtime manifest schedule contains {requestedSchedule.Dependencies.Count} dependency edges; the configured maximum is {_maximumDependencyRequestCount}.");
        }

        // Snapshot caller-owned collections once so concurrent or callback-driven mutation
        // cannot change the request between validation, resolution, loading, and handoff.
        manifest = manifest with
        {
            Bundles = Array.AsReadOnly(SnapshotBounded(
                manifest.Bundles,
                _maximumBundleCount,
                "Runtime manifest root bundles")),
            LoadPlan = manifest.LoadPlan is null
                ? null
                : Array.AsReadOnly(SnapshotBounded(
                    manifest.LoadPlan,
                    _maximumBundleCount,
                    "Runtime manifest load plan"))
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

        onStageChanged?.Invoke(RuntimeCompositionPhase.DependencyResolution);
        var requestedRoots = new Dictionary<ulong, IMicroBundle>();
        foreach (var (bundleId, version) in requestedVersions)
        {
            if (!_catalog.TryResolve(bundleId, version, out var bundle) || bundle is null)
                throw new InvalidOperationException(
                    $"MicroBundle {bundleId} version '{version}' could not be resolved.");

            ValidateResolvedBundle(bundleId, version, bundle);
            requestedRoots.Add(bundleId, bundle);
        }

        // Ordinary authoring composition discovers the dependency closure. A published,
        // pre-resolved manifest already carries the complete flat closure and uses that
        // list directly rather than recursively discovering dependencies at runtime.
        Dictionary<ulong, IMicroBundle> resolvedBundles;
        Dictionary<ulong, MicroBundleDependencyRequest[]> dependencyRequestsById;
        long totalConfigurationBytes;
#if WORKSHOP_USE_REPOSITORY_SOURCES
        if (manifest.ArbitrationResolved)
        {
            resolvedBundles = ResolvePublishedFlatSet(
                manifest,
                requestedRoots,
                out dependencyRequestsById,
                out totalConfigurationBytes);
        }
        else
#endif
        {
            resolvedBundles = ResolveDependencyGraph(
                requestedRoots,
                manifest.Schedule is not null,
                out dependencyRequestsById,
                out totalConfigurationBytes);
        }

        if (manifest.Schedule is not null)
            ValidateScheduleAgainstResolvedGraph(manifest, resolvedBundles, dependencyRequestsById);

        onStageChanged?.Invoke(RuntimeCompositionPhase.BundleLoading);
        var loaded = new List<IMicroBundle>();
        var attempted = new List<IMicroBundle>();
        var loadedIds = new HashSet<ulong>();
        var loading = new HashSet<ulong>();
        var loadContext = new MicroBundleLoadContext(manifest.RuntimeId);

        try
        {
#if WORKSHOP_USE_REPOSITORY_SOURCES
            if (manifest.ArbitrationResolved)
            {
                LoadPublishedFlatPlan(
                    manifest,
                    resolvedBundles,
                    dependencyRequestsById,
                    loaded,
                    attempted,
                    loadedIds,
                    loadContext,
                    configurationSource,
                    ref totalConfigurationBytes);
            }
            else
#endif
            {
                foreach (var entry in manifest.Bundles)
                {
                    LoadRoot(
                        entry,
                        resolvedBundles,
                        dependencyRequestsById,
                        loaded,
                        attempted,
                        loadedIds,
                        loading,
                        loadContext,
                        configurationSource,
                        manifest.RuntimeId,
                        ref totalConfigurationBytes);
                }
            }

#if WORKSHOP_USE_REPOSITORY_SOURCES
            if (!manifest.ArbitrationResolved)
#endif
                onStageChanged?.Invoke(RuntimeCompositionPhase.Arbitration);
            int rounds;
#if WORKSHOP_USE_REPOSITORY_SOURCES
            if (manifest.ArbitrationResolved)
            {
                rounds = manifest.PublishedArbitrationRounds!.Value;
                if (rounds >= _maximumArbitrationRounds)
                    throw new InvalidOperationException(
                        $"Published arbitration round {rounds} is outside the configured bound of {_maximumArbitrationRounds}.");
            }
            else
#endif
            {
                var arbitrationContext = new ArbitrationContext(
                    manifest.RuntimeId,
                    loaded,
                    manifest.ExperienceContext);

                rounds = 0;
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
            }

#if WORKSHOP_USE_REPOSITORY_SOURCES
            var resolvedConfigurations = CaptureResolvedConfigurations(
                loaded,
                loadContext,
                requireAll: manifest.ArbitrationResolved);
            return new RuntimeAssembly(
                manifest.RuntimeId,
                loaded,
                rounds,
                manifest.Intent,
                resolvedConfigurations);
#else
            return new RuntimeAssembly(manifest.RuntimeId, loaded, rounds, manifest.Intent);
#endif
        }
#pragma warning disable CA1031 // Composition callbacks are untrusted extension points; compensate and preserve the failure.
        catch (Exception compositionException)
#pragma warning restore CA1031
        {
            var compensationExceptions = new List<Exception>();
            for (var index = attempted.Count - 1; index >= 0; index--)
            {
                if (attempted[index] is not IMicroBundleRollback rollback)
                    continue;

                try
                {
                    rollback.RollbackLoad(loadContext);
                }
#pragma warning disable CA1031 // Continue compensating remaining bundles if one rollback callback fails.
                catch (Exception compensationException)
#pragma warning restore CA1031
                {
                    compensationExceptions.Add(compensationException);
                }
            }

            if (compensationExceptions.Count > 0)
            {
                compensationExceptions.Insert(0, compositionException);
                throw new AggregateException(
                    "FSM_COS composition failed and one or more MicroBundles also failed during compensation.",
                    compensationExceptions);
            }

            throw;
        }
    }

#if WORKSHOP_USE_REPOSITORY_SOURCES
    private Dictionary<ulong, ReadOnlyMemory<byte>> CaptureResolvedConfigurations(
        IReadOnlyList<IMicroBundle> loaded,
        MicroBundleLoadContext loadContext,
        bool requireAll)
    {
        var result = new Dictionary<ulong, ReadOnlyMemory<byte>>();
        long totalBytes = 0;

        foreach (var bundle in loaded)
        {
            if (bundle is not IMicroBundleResolvedConfiguration exporter)
            {
                if (requireAll)
                    throw new InvalidOperationException(
                        $"Pre-resolved composition requires MicroBundle {bundle.Id} to implement {nameof(IMicroBundleResolvedConfiguration)}.");
                continue;
            }

            var exported = exporter.ExportResolvedConfiguration().ToArray();
            if (exported.LongLength > _maximumConfigurationBytes - totalBytes)
                throw new InvalidOperationException(
                    $"Resolved MicroBundle configurations exceed the configured maximum of {_maximumConfigurationBytes} bytes.");
            totalBytes += exported.LongLength;

            if (requireAll)
            {
                if (!loadContext.TryGetConfiguration(bundle.Id, out var supplied))
                    throw new InvalidOperationException(
                        $"Pre-resolved composition requires an explicit resolved configuration for MicroBundle {bundle.Id}.");

                if (!supplied.Span.SequenceEqual(exported))
                    throw new InvalidOperationException(
                        $"MicroBundle {bundle.Id} did not restore the exact resolved configuration supplied by the published manifest.");
            }

            result.Add(bundle.Id, exported);
        }

        return result;
    }

#endif

    private static void ValidateScheduleAgainstResolvedGraph(
        RuntimeManifest manifest,
        Dictionary<ulong, IMicroBundle> resolvedBundles,
        Dictionary<ulong, MicroBundleDependencyRequest[]> dependencyRequestsById)
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

#if WORKSHOP_USE_REPOSITORY_SOURCES
    private Dictionary<ulong, IMicroBundle> ResolvePublishedFlatSet(
        RuntimeManifest manifest,
        IReadOnlyDictionary<ulong, IMicroBundle> requestedRoots,
        out Dictionary<ulong, MicroBundleDependencyRequest[]> dependencyRequestsById,
        out long totalConfigurationBytes)
    {
        var plan = manifest.LoadPlan
            ?? throw new InvalidOperationException("A pre-resolved runtime manifest must include its complete flat load plan.");
        var resolvedBundles = new Dictionary<ulong, IMicroBundle>(plan.Count);
        dependencyRequestsById = new Dictionary<ulong, MicroBundleDependencyRequest[]>();
        totalConfigurationBytes = 0;
        var dependencyRequestCount = 0;

        // Bundles contains only authored composition roots. Resolve every remaining entry
        // from the published flat plan independently, without promoting dependencies to roots.
        foreach (var entry in plan)
        {
            var bundleId = entry.Reference.BundleId;
            IMicroBundle bundle;
            if (requestedRoots.TryGetValue(bundleId, out var requestedRoot))
            {
                bundle = requestedRoot;
                if (!string.Equals(bundle.Descriptor.Version, entry.Reference.Version, StringComparison.Ordinal))
                    throw new InvalidOperationException(
                        $"Runtime manifest load plan expects root MicroBundle {bundleId} version '{entry.Reference.Version}', " +
                        $"but the requested root is '{bundle.Descriptor.Version}'.");
            }
            else if (!_catalog.TryResolve(bundleId, entry.Reference.Version, out var dependencyBundle) ||
                     dependencyBundle is null)
            {
                throw new InvalidOperationException(
                    $"Pre-resolved dependency MicroBundle {bundleId} version '{entry.Reference.Version}' could not be resolved.");
            }
            else
            {
                bundle = dependencyBundle;
                ValidateResolvedBundle(bundleId, entry.Reference.Version, bundle);
            }

            ValidateResolvedDescriptorIdentity(bundleId, bundle);
            var declaredDependencies = bundle.Dependencies;
            if (declaredDependencies is null)
                throw new InvalidOperationException($"MicroBundle {bundleId} returned no dependency request list.");

            var dependencySnapshot = SnapshotDependencyRequests(
                bundleId,
                declaredDependencies,
                ref dependencyRequestCount,
                ref totalConfigurationBytes);
            ValidateResolvedIdentity(bundleId, bundle, dependencySnapshot);
            resolvedBundles.Add(bundleId, bundle);
            dependencyRequestsById.Add(bundleId, dependencySnapshot);
        }

        foreach (var (bundleId, dependencies) in dependencyRequestsById)
        {
            foreach (var dependency in dependencies)
            {
                if (!resolvedBundles.ContainsKey(dependency.BundleId))
                    throw new InvalidOperationException(
                        $"Pre-resolved manifest is missing dependency MicroBundle {dependency.BundleId} required by MicroBundle {bundleId}.");
            }
        }

        return resolvedBundles;
    }

    private void LoadPublishedFlatPlan(
        RuntimeManifest manifest,
        IReadOnlyDictionary<ulong, IMicroBundle> resolvedBundles,
        IReadOnlyDictionary<ulong, MicroBundleDependencyRequest[]> dependencyRequestsById,
        List<IMicroBundle> loaded,
        List<IMicroBundle> attempted,
        HashSet<ulong> loadedIds,
        MicroBundleLoadContext loadContext,
        IMicroBundleConfigurationSource? configurationSource,
        ref long totalConfigurationBytes)
    {
        if (configurationSource is null)
            throw new InvalidOperationException(
                "A pre-resolved runtime manifest requires explicit resolved configuration for every MicroBundle.");

        foreach (var entry in manifest.LoadPlan!)
        {
            var bundleId = entry.Reference.BundleId;
            var bundle = resolvedBundles[bundleId];

            foreach (var dependency in dependencyRequestsById[bundleId])
            {
                if (!loadedIds.Contains(dependency.BundleId))
                    throw new InvalidOperationException(
                        $"Published load order places MicroBundle {bundleId} before prerequisite {dependency.BundleId}.");
            }

            if (!configurationSource.TryGetConfiguration(
                    manifest.RuntimeId,
                    bundleId,
                    bundle.Descriptor.Version,
                    out var configuration))
                throw new InvalidOperationException(
                    $"Pre-resolved runtime manifest has no resolved configuration for MicroBundle {bundleId}.");

            configuration = SnapshotConfiguration(
                configuration,
                ref totalConfigurationBytes,
                $"published configuration for MicroBundle {bundleId}");
            loadContext.SetConfiguration(bundleId, configuration);

            attempted.Add(bundle);
            bundle.Load(loadContext);
            loaded.Add(bundle);
            loadedIds.Add(bundleId);
        }
    }

#endif

    private Dictionary<ulong, IMicroBundle> ResolveDependencyGraph(
        IReadOnlyDictionary<ulong, IMicroBundle> requestedRoots,
        bool validatingPublishedSchedule,
        out Dictionary<ulong, MicroBundleDependencyRequest[]> dependencyRequestsById,
        out long totalConfigurationBytes)
    {
        var resolvedBundles = new Dictionary<ulong, IMicroBundle>();
        dependencyRequestsById = new Dictionary<ulong, MicroBundleDependencyRequest[]>();
        totalConfigurationBytes = 0;
        var discovered = requestedRoots.ToDictionary(pair => pair.Key, pair => pair.Value);
        if (discovered.Count > _maximumBundleCount)
            throw new InvalidOperationException(
                $"Dependency graph starts with {discovered.Count} roots; the configured maximum is {_maximumBundleCount}.");
        var dependencyRequestCount = 0;
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

            // Enumerate with a hard bound rather than trusting a custom IReadOnlyList.Count.
            // Copy each payload because ReadOnlyMemory does not make its backing array immutable.
            var dependencySnapshot = SnapshotDependencyRequests(
                id,
                declaredDependencies,
                ref dependencyRequestCount,
                ref totalConfigurationBytes);
            ValidateResolvedIdentity(id, bundle, dependencySnapshot);
            resolvedBundles.Add(id, bundle);
            dependencyRequestsById.Add(id, dependencySnapshot);

            for (var index = dependencySnapshot.Length - 1; index >= 0; index--)
            {
                var dependencyId = dependencySnapshot[index].BundleId;
                if (discovered.ContainsKey(dependencyId))
                    continue;

                if (discovered.Count >= _maximumBundleCount)
                    throw new InvalidOperationException(
                        $"Dependency graph exceeds the configured maximum of {_maximumBundleCount} bundles.");

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
        Dictionary<ulong, MicroBundleDependencyRequest[]> dependencyRequestsById)
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
        Dictionary<ulong, IMicroBundle> resolvedBundles,
        Dictionary<ulong, MicroBundleDependencyRequest[]> dependencyRequestsById,
        List<IMicroBundle> loaded,
        List<IMicroBundle> attempted,
        HashSet<ulong> loadedIds,
        HashSet<ulong> loading,
        MicroBundleLoadContext loadContext,
        IMicroBundleConfigurationSource? configurationSource,
        ulong runtimeId,
        ref long totalConfigurationBytes)
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
                    configuration = SnapshotConfiguration(
                        externalConfiguration,
                        ref totalConfigurationBytes,
                        $"configuration provider for MicroBundle {bundle.Id}");
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

            attempted.Add(bundle);
            bundle.Load(loadContext);
            loaded.Add(bundle);
            loadedIds.Add(bundle.Id);
            loading.Remove(bundle.Id);
            stack.Pop();
        }
    }

    private MicroBundleDependencyRequest[] SnapshotDependencyRequests(
        ulong bundleId,
        IReadOnlyList<MicroBundleDependencyRequest> declaredDependencies,
        ref int dependencyRequestCount,
        ref long totalConfigurationBytes)
    {
        var remaining = _maximumDependencyRequestCount - dependencyRequestCount;
        var snapshot = new List<MicroBundleDependencyRequest>(Math.Min(remaining, 256));
        foreach (var request in declaredDependencies)
        {
            if (snapshot.Count >= remaining)
                throw new InvalidOperationException(
                    $"Dependency graph exceeds the configured maximum of {_maximumDependencyRequestCount} dependency requests.");

            var configuration = SnapshotConfiguration(
                request.Configuration,
                ref totalConfigurationBytes,
                $"dependency request for MicroBundle {bundleId}");
            snapshot.Add(new MicroBundleDependencyRequest(request.BundleId, configuration));
        }

        dependencyRequestCount += snapshot.Count;
        return snapshot.ToArray();
    }

    private ReadOnlyMemory<byte> SnapshotConfiguration(
        ReadOnlyMemory<byte> configuration,
        ref long totalConfigurationBytes,
        string source)
    {
        if (configuration.Length > _maximumConfigurationBytes - totalConfigurationBytes)
            throw new InvalidOperationException(
                $"Configuration data from {source} exceeds the configured maximum of {_maximumConfigurationBytes} bytes.");

        var snapshot = configuration.ToArray();
        totalConfigurationBytes += snapshot.Length;
        return snapshot;
    }

    private static T[] SnapshotBounded<T>(
        IEnumerable<T> source,
        int maximumCount,
        string description)
    {
        var snapshot = new List<T>(Math.Min(maximumCount, 256));
        foreach (var item in source)
        {
            if (snapshot.Count >= maximumCount)
                throw new InvalidOperationException(
                    $"{description} exceeds the configured maximum of {maximumCount} entries.");
            snapshot.Add(item);
        }

        return snapshot.ToArray();
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
        ValidateResolvedDescriptorIdentity(requestedId, bundle);

        if (!string.Equals(bundle.Descriptor.Version, requestedVersion, StringComparison.Ordinal))
            throw new InvalidOperationException(
                $"MicroBundle {requestedId} resolved to version '{bundle.Descriptor.Version}', " +
                $"but manifest requested '{requestedVersion}'.");
    }
}
