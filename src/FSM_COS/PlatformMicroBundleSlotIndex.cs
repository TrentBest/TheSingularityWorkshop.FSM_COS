using System.Collections.ObjectModel;

namespace TheSingularityWorkshop.FSM_COS;

/// <summary>
/// A platform-local runtime slot assigned to a portable MicroBundle ontology identity.
/// This value is not a Repository storage handle or a Domain MicroBundle ID.
/// </summary>
public readonly record struct PlatformMicroBundleSlot
{
    public PlatformMicroBundleSlot(ulong value)
    {
        if (value == 0)
            throw new ArgumentOutOfRangeException(nameof(value), "Platform-local slot 0 is reserved.");

        Value = value;
    }

    public ulong Value { get; }
}

/// <summary>
/// Immutable, revisioned snapshot of one platform's ontology-to-slot assignments.
/// </summary>
public sealed class PlatformMicroBundleSlotSnapshot
{
    public PlatformMicroBundleSlotSnapshot(
        long revision,
        IReadOnlyDictionary<string, PlatformMicroBundleSlot> assignments)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(revision);
        ArgumentNullException.ThrowIfNull(assignments);

        var copy = new Dictionary<string, PlatformMicroBundleSlot>(StringComparer.Ordinal);
        var usedSlots = new HashSet<ulong>();

        foreach (var (ontology, slot) in assignments)
        {
            PlatformMicroBundleSlotIndex.ValidateCanonicalOntology(ontology);
            if (slot.Value == 0)
                throw new ArgumentException("Platform-local slot 0 is reserved.", nameof(assignments));
            if (!copy.TryAdd(ontology, slot))
                throw new ArgumentException($"Duplicate ontology identity '{ontology}'.", nameof(assignments));
            if (!usedSlots.Add(slot.Value))
                throw new ArgumentException($"Platform-local slot {slot.Value} is assigned more than once.", nameof(assignments));
        }

        Revision = revision;
        Assignments = new ReadOnlyDictionary<string, PlatformMicroBundleSlot>(copy);
    }

    public long Revision { get; }
    public IReadOnlyDictionary<string, PlatformMicroBundleSlot> Assignments { get; }
}

/// <summary>
/// Durable compare-and-swap boundary for a platform's slot snapshot.
/// Implementations must make TryWrite atomic across all writers sharing the same platform store.
/// </summary>
public interface IPlatformMicroBundleSlotStore
{
    PlatformMicroBundleSlotSnapshot Read();

    /// <summary>
    /// Atomically writes the replacement only when the persisted revision equals expectedRevision.
    /// On conflict, returns false and supplies the latest persisted snapshot in current.
    /// </summary>
    bool TryWrite(
        long expectedRevision,
        PlatformMicroBundleSlotSnapshot replacement,
        out PlatformMicroBundleSlotSnapshot current);
}

/// <summary>
/// Allocates stable, platform-local numeric slots for canonical portable ontology identities.
/// </summary>
/// <remarks>
/// Slot identity is independent of artifact version, digest, Repository storage address, and
/// Domain MicroBundle ID. The store is responsible for durable, cross-process atomic compare-and-swap.
/// </remarks>
public sealed class PlatformMicroBundleSlotIndex
{
    private const int MaximumWriteAttempts = 64;
    private readonly IPlatformMicroBundleSlotStore _store;

    public PlatformMicroBundleSlotIndex(IPlatformMicroBundleSlotStore store) =>
        _store = store ?? throw new ArgumentNullException(nameof(store));

    public PlatformMicroBundleSlot ResolveOrAssign(string canonicalOntology)
    {
        ValidateCanonicalOntology(canonicalOntology);

        for (var attempt = 0; attempt < MaximumWriteAttempts; attempt++)
        {
            var current = _store.Read()
                ?? throw new InvalidOperationException("The platform slot store returned no snapshot.");
            ValidateSnapshot(current);

            if (current.Assignments.TryGetValue(canonicalOntology, out var existing))
                return existing;

            var nextValue = current.Assignments.Count == 0
                ? 1UL
                : checked(current.Assignments.Values.Max(slot => slot.Value) + 1UL);
            var assignments = new Dictionary<string, PlatformMicroBundleSlot>(
                current.Assignments,
                StringComparer.Ordinal)
            {
                [canonicalOntology] = new PlatformMicroBundleSlot(nextValue)
            };
            var replacement = new PlatformMicroBundleSlotSnapshot(
                checked(current.Revision + 1),
                assignments);

            if (_store.TryWrite(current.Revision, replacement, out var persisted))
            {
                ValidateSnapshot(persisted);
                if (persisted.Revision != replacement.Revision ||
                    !persisted.Assignments.TryGetValue(canonicalOntology, out var assigned) ||
                    assigned != replacement.Assignments[canonicalOntology])
                {
                    throw new InvalidOperationException(
                        "The platform slot store acknowledged a write without persisting the requested assignment.");
                }

                return assigned;
            }

            ValidateSnapshot(persisted);
            if (persisted.Assignments.TryGetValue(canonicalOntology, out existing))
                return existing;
        }

        throw new InvalidOperationException(
            $"Could not assign a platform-local slot for '{canonicalOntology}' after {MaximumWriteAttempts} compare-and-swap conflicts.");
    }

    public bool TryResolve(string canonicalOntology, out PlatformMicroBundleSlot slot)
    {
        ValidateCanonicalOntology(canonicalOntology);
        var snapshot = _store.Read()
            ?? throw new InvalidOperationException("The platform slot store returned no snapshot.");
        ValidateSnapshot(snapshot);
        return snapshot.Assignments.TryGetValue(canonicalOntology, out slot);
    }

    internal static void ValidateCanonicalOntology(string ontology)
    {
        if (string.IsNullOrWhiteSpace(ontology))
            throw new ArgumentException("A canonical ontology identity is required.", nameof(ontology));
        if (!string.Equals(ontology, ontology.Trim(), StringComparison.Ordinal))
            throw new ArgumentException(
                "A canonical ontology identity must not contain leading or trailing whitespace.",
                nameof(ontology));
    }

    private static void ValidateSnapshot(PlatformMicroBundleSlotSnapshot snapshot)
    {
        ArgumentNullException.ThrowIfNull(snapshot);
        // Reconstructing validates the complete snapshot, including duplicate slot values
        // supplied by a faulty or corrupted persistence implementation.
        _ = new PlatformMicroBundleSlotSnapshot(snapshot.Revision, snapshot.Assignments);
    }
}
