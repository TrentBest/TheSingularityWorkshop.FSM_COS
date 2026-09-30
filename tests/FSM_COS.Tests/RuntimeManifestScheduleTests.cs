using Xunit;

namespace FSM_COS.Tests;

public sealed class RuntimeManifestScheduleTests
{
    [Fact]
    public void Schedule_separates_independent_entries_from_dependency_entries()
    {
        var a = Entry(1);
        var b = Entry(2);
        var c = Entry(3);

        var schedule = new RuntimeManifestSchedule(
            new[] { a, b, c },
            new[]
            {
                new RuntimeManifestDependency(2, 1),
                new RuntimeManifestDependency(3, 2)
            });

        Assert.Equal(new ulong[] { 3 }, schedule.IndependentEntries.Select(e => e.Reference.BundleId));
        Assert.Equal(
            new ulong[] { 1, 2 },
            schedule.DependencyEntries.Select(e => e.Reference.BundleId));
    }

    [Fact]
    public void Dependency_is_not_ready_until_its_required_bundle_is_loaded()
    {
        var schedule = new RuntimeManifestSchedule(
            new[] { Entry(1), Entry(2) },
            new[] { new RuntimeManifestDependency(2, 1) });

        var loaded = new HashSet<ulong>();

        Assert.False(schedule.IsDependencyReady(2, loaded.Contains));

        loaded.Add(1);

        Assert.True(schedule.IsDependencyReady(2, loaded.Contains));
    }

    [Fact]
    public void Completed_localization_is_not_a_dependency_load_requirement()
    {
        var schedule = new RuntimeManifestSchedule(
            new[] { Entry(1), Entry(2) },
            new[] { new RuntimeManifestDependency(2, 1) });

        var localized = new HashSet<ulong> { 1 };
        var loaded = new HashSet<ulong>();

        Assert.False(schedule.IsDependencyReady(2, loaded.Contains));
        Assert.Contains(1, localized);
    }

    [Fact]
    public void Schedule_rejects_dependency_outside_manifest()
    {
        Assert.Throws<ArgumentException>(() =>
            new RuntimeManifestSchedule(
                new[] { Entry(1) },
                new[] { new RuntimeManifestDependency(1, 99) }));
    }

    private static RuntimeManifestEntry Entry(ulong id) =>
        new(
            new MicroBundleReference(id, "1.0.0", $"sha256:{id}"),
            BundleRequest.Unconfigured(id));
}
