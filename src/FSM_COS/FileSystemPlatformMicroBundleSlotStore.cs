using System.Text.Json;

namespace TheSingularityWorkshop.FSM_COS;

/// <summary>
/// Filesystem-backed, cross-process compare-and-swap store for platform-local MicroBundle slots.
/// </summary>
/// <remarks>
/// The snapshot is replaced atomically on the same filesystem. Writers serialize through an
/// exclusive lock file, then re-read the persisted revision while holding that lock. This store
/// is for runtime identity metadata; it does not store MicroBundle artifact bytes.
/// </remarks>
public sealed class FileSystemPlatformMicroBundleSlotStore : IPlatformMicroBundleSlotStore
{
    private const string SnapshotFileName = "platform-microbundle-slots.json";
    private const string LockFileName = "platform-microbundle-slots.lock";
    private static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = true };

    private readonly string _snapshotPath;
    private readonly string _lockPath;

    public FileSystemPlatformMicroBundleSlotStore(string root)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(root);
        var fullRoot = Path.GetFullPath(root);
        Directory.CreateDirectory(fullRoot);
        _snapshotPath = Path.Combine(fullRoot, SnapshotFileName);
        _lockPath = Path.Combine(fullRoot, LockFileName);
    }

    public PlatformMicroBundleSlotSnapshot Read()
    {
        if (!File.Exists(_snapshotPath))
            return EmptySnapshot();

        try
        {
            using var stream = new FileStream(
                _snapshotPath, FileMode.Open, FileAccess.Read, FileShare.Read,
                bufferSize: 4096, options: FileOptions.SequentialScan);
            using var document = JsonDocument.Parse(stream);
            var root = document.RootElement;
            if (root.ValueKind != JsonValueKind.Object)
                throw new InvalidDataException("The platform slot snapshot must be a JSON object.");

            long? revision = null;
            JsonElement assignmentsElement = default;
            var seenRootProperties = new HashSet<string>(StringComparer.Ordinal);
            foreach (var property in root.EnumerateObject())
            {
                if (!seenRootProperties.Add(property.Name))
                    throw new InvalidDataException($"The platform slot snapshot contains duplicate property '{property.Name}'.");

                switch (property.Name)
                {
                    case "Revision":
                        if (property.Value.ValueKind != JsonValueKind.Number ||
                            !property.Value.TryGetInt64(out var parsedRevision) ||
                            parsedRevision < 0)
                            throw new InvalidDataException("The platform slot snapshot revision must be a non-negative integer.");
                        revision = parsedRevision;
                        break;
                    case "Assignments":
                        assignmentsElement = property.Value;
                        break;
                    default:
                        throw new InvalidDataException($"The platform slot snapshot contains unknown property '{property.Name}'.");
                }
            }

            if (revision is null)
                throw new InvalidDataException("The platform slot snapshot is missing its required revision.");
            if (assignmentsElement.ValueKind != JsonValueKind.Object)
                throw new InvalidDataException("The platform slot snapshot is missing its assignments object.");

            var assignments = new Dictionary<string, PlatformMicroBundleSlot>(StringComparer.Ordinal);
            foreach (var property in assignmentsElement.EnumerateObject())
            {
                if (property.Value.ValueKind != JsonValueKind.Number ||
                    !property.Value.TryGetUInt64(out var slotValue))
                    throw new InvalidDataException($"The platform slot assignment '{property.Name}' must be an unsigned integer.");

                if (!assignments.TryAdd(property.Name, new PlatformMicroBundleSlot(slotValue)))
                    throw new InvalidDataException($"The platform slot snapshot contains duplicate ontology '{property.Name}'.");
            }

            return new PlatformMicroBundleSlotSnapshot(revision.Value, assignments);
        }
        catch (Exception exception) when (
            exception is JsonException or ArgumentException or OverflowException)
        {
            throw new InvalidDataException("The persisted platform MicroBundle slot snapshot is invalid.", exception);
        }
    }

    public bool TryWrite(
        long expectedRevision,
        PlatformMicroBundleSlotSnapshot replacement,
        out PlatformMicroBundleSlotSnapshot current)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(expectedRevision);
        ArgumentNullException.ThrowIfNull(replacement);

        using var writeLock = AcquireWriteLock();
        current = Read();

        if (current.Revision != expectedRevision)
            return false;

        if (replacement.Revision != checked(expectedRevision + 1))
            throw new ArgumentException(
                "A replacement snapshot must advance the current revision by exactly one.",
                nameof(replacement));

        // Reconstruct to validate the full immutable snapshot before it reaches disk.
        var validated = new PlatformMicroBundleSlotSnapshot(replacement.Revision, replacement.Assignments);
        var persisted = new PersistedSnapshot(
            validated.Revision,
            validated.Assignments.ToDictionary(
                static pair => pair.Key,
                static pair => pair.Value.Value,
                StringComparer.Ordinal));

        WriteAtomically(persisted);
        current = validated;
        return true;
    }

    private FileStream AcquireWriteLock()
    {
        while (true)
        {
            try
            {
                return new FileStream(
                    _lockPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None,
                    bufferSize: 1, options: FileOptions.WriteThrough);
            }
            catch (IOException)
            {
                Thread.Sleep(25);
            }
        }
    }

    private void WriteAtomically(PersistedSnapshot snapshot)
    {
        var temporaryPath = _snapshotPath + ".tmp-" + Guid.NewGuid().ToString("N");
        try
        {
            using (var stream = new FileStream(
                temporaryPath, FileMode.CreateNew, FileAccess.Write, FileShare.None,
                bufferSize: 4096, options: FileOptions.WriteThrough))
            {
                JsonSerializer.Serialize(stream, snapshot, JsonOptions);
                stream.Flush(flushToDisk: true);
            }

            File.Move(temporaryPath, _snapshotPath, overwrite: true);
        }
        finally
        {
            if (File.Exists(temporaryPath))
                File.Delete(temporaryPath);
        }
    }

    private static PlatformMicroBundleSlotSnapshot EmptySnapshot() =>
        new(0, new Dictionary<string, PlatformMicroBundleSlot>(StringComparer.Ordinal));

    private sealed record PersistedSnapshot(
        long Revision,
        Dictionary<string, ulong>? Assignments);
}
