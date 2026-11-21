using TheSingularityWorkshop.FSM_COS;
using Xunit;

namespace FSM_COS.Tests;

public sealed class PlatformMicroBundleSlotIndexTests
{
    [Fact]
    public void Repeated_resolution_returns_the_same_slot_and_does_not_advance_revision()
    {
        var store = new InMemoryCompareAndSwapSlotStore();
        var index = new PlatformMicroBundleSlotIndex(store);

        var first = index.ResolveOrAssign("workshop:capability/render");
        var second = index.ResolveOrAssign("workshop:capability/render");

        Assert.Equal(first, second);
        Assert.Equal(1, first.Value);
        Assert.Equal(1, store.Read().Revision);
    }

    [Fact]
    public void Different_ontologies_receive_distinct_nonzero_slots()
    {
        var index = new PlatformMicroBundleSlotIndex(new InMemoryCompareAndSwapSlotStore());

        var first = index.ResolveOrAssign("workshop:capability/render");
        var second = index.ResolveOrAssign("workshop:capability/physics");

        Assert.NotEqual(first, second);
        Assert.NotEqual(0UL, first.Value);
        Assert.NotEqual(0UL, second.Value);
    }

    [Fact]
    public void A_new_index_instance_recovers_assignments_from_the_same_store()
    {
        var store = new InMemoryCompareAndSwapSlotStore();
        var firstIndex = new PlatformMicroBundleSlotIndex(store);
        var assigned = firstIndex.ResolveOrAssign("workshop:capability/render");

        var restartedIndex = new PlatformMicroBundleSlotIndex(store);

        Assert.True(restartedIndex.TryResolve("workshop:capability/render", out var recovered));
        Assert.Equal(assigned, recovered);
    }

    [Fact]
    public void Compare_and_swap_conflict_reloads_and_reuses_the_winning_assignment()
    {
        var store = new ConflictOnceSlotStore();
        var index = new PlatformMicroBundleSlotIndex(store);

        var assigned = index.ResolveOrAssign("workshop:capability/render");

        Assert.Equal(new PlatformMicroBundleSlot(7), assigned);
        Assert.Equal(2, store.WriteAttempts);
    }

    [Fact]
    public void Separate_platform_stores_own_independent_slot_namespaces()
    {
        var firstPlatform = new PlatformMicroBundleSlotIndex(new InMemoryCompareAndSwapSlotStore());
        var secondPlatform = new PlatformMicroBundleSlotIndex(new InMemoryCompareAndSwapSlotStore());

        var first = firstPlatform.ResolveOrAssign("workshop:capability/render");
        var second = secondPlatform.ResolveOrAssign("workshop:capability/physics");

        Assert.Equal(1UL, first.Value);
        Assert.Equal(1UL, second.Value);
    }

    [Theory]
    [InlineData("")]
    [InlineData("  ")]
    [InlineData(" workshop:capability/render")]
    [InlineData("workshop:capability/render ")]
    public void Rejects_noncanonical_ontology_input(string ontology)
    {
        var index = new PlatformMicroBundleSlotIndex(new InMemoryCompareAndSwapSlotStore());

        Assert.Throws<ArgumentException>(() => index.ResolveOrAssign(ontology));
    }

    [Fact]
    public void Snapshot_rejects_duplicate_slot_assignments()
    {
        var assignments = new Dictionary<string, PlatformMicroBundleSlot>(StringComparer.Ordinal)
        {
            ["workshop:capability/render"] = new(3),
            ["workshop:capability/physics"] = new(3)
        };

        Assert.Throws<ArgumentException>(() => new PlatformMicroBundleSlotSnapshot(1, assignments));
    }

    [Fact]
    public void Slot_zero_is_reserved()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new PlatformMicroBundleSlot(0));
    }

    private sealed class InMemoryCompareAndSwapSlotStore : IPlatformMicroBundleSlotStore
    {
        private PlatformMicroBundleSlotSnapshot _snapshot =
            new(0, new Dictionary<string, PlatformMicroBundleSlot>(StringComparer.Ordinal));

        public PlatformMicroBundleSlotSnapshot Read() => _snapshot;

        public bool TryWrite(
            long expectedRevision,
            PlatformMicroBundleSlotSnapshot replacement,
            out PlatformMicroBundleSlotSnapshot current)
        {
            if (_snapshot.Revision != expectedRevision)
            {
                current = _snapshot;
                return false;
            }

            _snapshot = replacement;
            current = _snapshot;
            return true;
        }
    }

    private sealed class ConflictOnceSlotStore : IPlatformMicroBundleSlotStore
    {
        private PlatformMicroBundleSlotSnapshot _snapshot =
            new(0, new Dictionary<string, PlatformMicroBundleSlot>(StringComparer.Ordinal));
        private bool _conflict = true;

        public int WriteAttempts { get; private set; }

        public PlatformMicroBundleSlotSnapshot Read() => _snapshot;

        public bool TryWrite(
            long expectedRevision,
            PlatformMicroBundleSlotSnapshot replacement,
            out PlatformMicroBundleSlotSnapshot current)
        {
            WriteAttempts++;
            if (_conflict)
            {
                _conflict = false;
                _snapshot = new PlatformMicroBundleSlotSnapshot(
                    1,
                    new Dictionary<string, PlatformMicroBundleSlot>(StringComparer.Ordinal)
                    {
                        ["workshop:capability/render"] = new(7)
                    });
                current = _snapshot;
                return false;
            }

            if (_snapshot.Revision != expectedRevision)
            {
                current = _snapshot;
                return false;
            }

            _snapshot = replacement;
            current = _snapshot;
            return true;
        }
    }
}
