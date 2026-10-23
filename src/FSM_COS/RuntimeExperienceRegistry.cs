using System.Collections.ObjectModel;

namespace TheSingularityWorkshop.FSM_COS;

/// <summary>
/// Retains independently composed runtime experiences and tracks which one a host has selected.
/// Selection is metadata only: presentation and execution remain the responsibility of the host.
/// </summary>
/// <remarks>
/// Registering an assembly does not transfer ownership of its MicroBundles, and removing an
/// entry does not unload or dispose them. The MicroBundleDomain contract does not currently
/// define a general unload operation.
/// </remarks>
public sealed class RuntimeExperienceRegistry
{
    private readonly object _gate = new();
    private readonly Dictionary<ulong, RuntimeAssembly> _assemblies = new();
    private ulong? _activeRuntimeId;

    /// <summary>The runtime identity currently selected by the host, or null when none is active.</summary>
    public ulong? ActiveRuntimeId
    {
        get { lock (_gate) return _activeRuntimeId; }
    }

    /// <summary>A stable snapshot of all retained runtime assemblies, keyed by RuntimeId.</summary>
    public IReadOnlyDictionary<ulong, RuntimeAssembly> Assemblies
    {
        get
        {
            lock (_gate)
                return new ReadOnlyDictionary<ulong, RuntimeAssembly>(
                    new Dictionary<ulong, RuntimeAssembly>(_assemblies));
        }
    }

    /// <summary>Retains a successfully composed assembly without changing the active experience.</summary>
    /// <returns>false when that RuntimeId is already registered.</returns>
    public bool TryRegister(RuntimeAssembly assembly)
    {
        ArgumentNullException.ThrowIfNull(assembly);
        lock (_gate)
            return _assemblies.TryAdd(assembly.RuntimeId, assembly);
    }

    /// <summary>Gets a retained assembly by its stable runtime identity.</summary>
    public bool TryGet(ulong runtimeId, out RuntimeAssembly? assembly)
    {
        lock (_gate)
            return _assemblies.TryGetValue(runtimeId, out assembly);
    }

    /// <summary>Selects a retained experience. A failed selection leaves the current selection unchanged.</summary>
    public bool TryActivate(ulong runtimeId)
    {
        lock (_gate)
        {
            if (!_assemblies.ContainsKey(runtimeId))
                return false;

            _activeRuntimeId = runtimeId;
            return true;
        }
    }

    /// <summary>Clears the active selection without removing any retained experiences.</summary>
    public void ClearActive()
    {
        lock (_gate)
            _activeRuntimeId = null;
    }

    /// <summary>Forgets a retained assembly. This does not unload or dispose its MicroBundles.</summary>
    /// <returns>false when the identity is not registered.</returns>
    public bool TryRemove(ulong runtimeId)
    {
        lock (_gate)
        {
            if (!_assemblies.Remove(runtimeId))
                return false;

            if (_activeRuntimeId == runtimeId)
                _activeRuntimeId = null;

            return true;
        }
    }
}
