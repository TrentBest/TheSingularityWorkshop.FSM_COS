using TheSingularityWorkshop.FSM_COS;
using Xunit;

namespace FSM_COS.Tests;

public sealed class SnapshotIsolationTests
{
    [Fact]
    public void Schedule_snapshots_all_input_and_derived_collections()
    {
        var independent = Entry(1);
        var dependent = Entry(2);
        var edge = new RuntimeManifestDependency(2, 1);
        var entries = new List<RuntimeManifestEntry> { independent, dependent };
        var dependencies = new List<RuntimeManifestDependency> { edge };

        var schedule = new RuntimeManifestSchedule(entries, dependencies);

        entries.Clear();
        dependencies.Clear();

        Assert.Equal(new ulong[] { 1, 2 }, schedule.Entries.Select(x => x.Reference.BundleId));
        Assert.Equal(new[] { edge }, schedule.Dependencies);
        Assert.Equal(new ulong[] { 1 }, schedule.IndependentEntries.Select(x => x.Reference.BundleId));
        Assert.Equal(new ulong[] { 2 }, schedule.DependencyEntries.Select(x => x.Reference.BundleId));
    }

    [Fact]
    public void Load_plan_snapshots_input_entries()
    {
        var first = Entry(1);
        var second = Entry(2);
        var entries = new List<RuntimeManifestEntry> { first, second };

        var plan = new RuntimeManifestLoadPlan(entries);

        entries.Clear();

        Assert.Equal(new ulong[] { 1, 2 }, plan.Entries.Select(x => x.Reference.BundleId));
        Assert.Equal(MicroBundleRuntimeState.Published, plan.GetState(1));
        Assert.Equal(MicroBundleRuntimeState.Published, plan.GetState(2));
        Assert.Throws<NotSupportedException>(() =>
            ((IList<RuntimeManifestEntry>)plan.Entries).Clear());
    }

    [Fact]
    public void Schedule_derived_collections_reject_mutation_through_list_interfaces()
    {
        var independent = Entry(1);
        var dependent = Entry(2);
        var schedule = new RuntimeManifestSchedule(
            new[] { independent, dependent },
            new[] { new RuntimeManifestDependency(2, 1) });

        Assert.Throws<NotSupportedException>(() =>
            ((IList<RuntimeManifestEntry>)schedule.IndependentEntries).Clear());
        Assert.Throws<NotSupportedException>(() =>
            ((IList<RuntimeManifestEntry>)schedule.DependencyEntries).Clear());

        Assert.Equal(new ulong[] { 1 }, schedule.IndependentEntries.Select(x => x.Reference.BundleId));
        Assert.Equal(new ulong[] { 2 }, schedule.DependencyEntries.Select(x => x.Reference.BundleId));
    }

    private static RuntimeManifestEntry Entry(ulong id) =>
        new(new MicroBundleReference(id, "1.0.0", id.ToString("x64")));
}
