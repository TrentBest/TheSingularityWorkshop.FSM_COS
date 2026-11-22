using TheSingularityWorkshop.FSM_COS;
using TheSingularityWorkshop.MicroBundleDomain;
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



    [Fact]
    public void Domain_binding_keeps_platform_slot_separate_from_domain_id()
    {
        var bundle = new TestMicroBundle(42, "1.2.3");
        var binding = new PlatformMicroBundleDomainBinding(
            "workshop:capability/render",
            new PlatformMicroBundleSlot(10),
            42,
            bundle,
            "1.2.3");

        Assert.Equal(10UL, binding.PlatformSlot.Value);
        Assert.Equal(42UL, binding.DomainBundleId);
        Assert.NotEqual(binding.PlatformSlot.Value, binding.DomainBundleId);
        Assert.Same(bundle, binding.Bundle);
    }

    [Fact]
    public void Domain_binding_rejects_implicit_or_mismatched_domain_identity()
    {
        var bundle = new TestMicroBundle(42, "1.2.3");

        Assert.Throws<ArgumentException>(() =>
            new PlatformMicroBundleDomainBinding(
                "workshop:capability/render",
                new PlatformMicroBundleSlot(10),
                10,
                bundle,
                "1.2.3"));
    }

    [Fact]
    public void Binding_set_rejects_duplicate_platform_slots_and_domain_ids()
    {
        var first = new PlatformMicroBundleDomainBinding(
            "workshop:capability/render", new(10), 42, new TestMicroBundle(42, "1.0"), "1.0");
        var duplicateSlot = new PlatformMicroBundleDomainBinding(
            "workshop:capability/physics", new(10), 43, new TestMicroBundle(43, "1.0"), "1.0");
        var duplicateDomainId = new PlatformMicroBundleDomainBinding(
            "workshop:capability/input", new(11), 42, new TestMicroBundle(42, "1.0"), "1.0");

        Assert.Throws<ArgumentException>(() =>
            new PlatformMicroBundleDomainBindingSet([first, duplicateSlot]));
        Assert.Throws<ArgumentException>(() =>
            new PlatformMicroBundleDomainBindingSet([first, duplicateDomainId]));
    }

    private sealed class TestMicroBundle : IMicroBundle
    {
        public TestMicroBundle(ulong id, string version) =>
            Descriptor = new MicroBundleDescriptor(id, version);

        public MicroBundleDescriptor Descriptor { get; }
        public IReadOnlyList<MicroBundleDependencyRequest> Dependencies => Array.Empty<MicroBundleDependencyRequest>();
        public void Load(IMicroBundleLoadContext context) => throw new NotSupportedException();
        public bool Arbitrate(IMicroBundleArbitrationContext context, int roundIndex) => false;
    }

    [Fact]
    public void File_system_store_persists_assignments_across_store_instances()
    {
        var root = Path.Combine(Path.GetTempPath(), "fsm-cos-slot-tests", Guid.NewGuid().ToString("N"));
        try
        {
            var firstStore = new FileSystemPlatformMicroBundleSlotStore(root);
            var firstIndex = new PlatformMicroBundleSlotIndex(firstStore);
            var assigned = firstIndex.ResolveOrAssign("workshop:capability/render");

            var restartedStore = new FileSystemPlatformMicroBundleSlotStore(root);
            var restartedIndex = new PlatformMicroBundleSlotIndex(restartedStore);

            Assert.True(restartedIndex.TryResolve("workshop:capability/render", out var recovered));
            Assert.Equal(assigned, recovered);
            Assert.Equal(1, restartedStore.Read().Revision);
        }
        finally
        {
            if (Directory.Exists(root))
                Directory.Delete(root, recursive: true);
        }
    }

    [Fact]
    public void File_system_store_rejects_stale_compare_and_swap_writes()
    {
        var root = Path.Combine(Path.GetTempPath(), "fsm-cos-slot-tests", Guid.NewGuid().ToString("N"));
        try
        {
            var store = new FileSystemPlatformMicroBundleSlotStore(root);
            var stale = store.Read();
            var first = new PlatformMicroBundleSlotSnapshot(
                1,
                new Dictionary<string, PlatformMicroBundleSlot>(StringComparer.Ordinal)
                {
                    ["workshop:capability/render"] = new(1)
                });
            Assert.True(store.TryWrite(0, first, out _));

            var conflicting = new PlatformMicroBundleSlotSnapshot(
                1,
                new Dictionary<string, PlatformMicroBundleSlot>(StringComparer.Ordinal)
                {
                    ["workshop:capability/physics"] = new(1)
                });

            Assert.False(store.TryWrite(stale.Revision, conflicting, out var current));
            Assert.Equal(1, current.Revision);
            Assert.Contains("workshop:capability/render", current.Assignments.Keys);
            Assert.DoesNotContain("workshop:capability/physics", current.Assignments.Keys);
        }
        finally
        {
            if (Directory.Exists(root))
                Directory.Delete(root, recursive: true);
        }
    }

    [Fact]
    public void File_system_store_fails_closed_on_corrupt_snapshot()
    {
        var root = Path.Combine(Path.GetTempPath(), "fsm-cos-slot-tests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(root);
        try
        {
            File.WriteAllText(
                Path.Combine(root, "platform-microbundle-slots.json"),
                "{ this is not valid json");

            var store = new FileSystemPlatformMicroBundleSlotStore(root);

            Assert.Throws<InvalidDataException>(() => store.Read());
        }
        finally
        {
            if (Directory.Exists(root))
                Directory.Delete(root, recursive: true);
        }
    }

    [Theory]
    [InlineData("{\"Assignments\":{}}")]
    [InlineData("{\"Revision\":0,\"Revision\":1,\"Assignments\":{}}")]
    [InlineData("{\"Revision\":0,\"Assignments\":{},\"Unexpected\":true}")]
    [InlineData("{\"Revision\":-1,\"Assignments\":{}}")]
    [InlineData("{\"Revision\":0,\"Assignments\":{\"workshop:capability/render\":0}}")]
    [InlineData("{\"Revision\":0,\"Assignments\":{\"workshop:capability/render\":1,\"workshop:capability/render\":2}}")]
    public void File_system_store_rejects_invalid_or_ambiguous_snapshot_contracts(string json)
    {
        var root = Path.Combine(Path.GetTempPath(), "fsm-cos-slot-tests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(root);
        try
        {
            File.WriteAllText(Path.Combine(root, "platform-microbundle-slots.json"), json);
            var store = new FileSystemPlatformMicroBundleSlotStore(root);

            Assert.Throws<InvalidDataException>(() => store.Read());
        }
        finally
        {
            if (Directory.Exists(root))
                Directory.Delete(root, recursive: true);
        }
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
