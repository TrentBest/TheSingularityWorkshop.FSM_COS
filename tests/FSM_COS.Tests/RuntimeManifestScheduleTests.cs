using Xunit;

namespace FSM_COS.Tests;

public sealed class RuntimeManifestScheduleTests
{
    [Fact]
    public void Schedule_separates_entries_without_prerequisites_from_dependency_constrained_entries()
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

        Assert.Equal(new ulong[] { 1 }, schedule.IndependentEntries.Select(e => e.Reference.BundleId));
        Assert.Equal(
            new ulong[] { 2, 3 },
            schedule.DependencyEntries.Select(e => e.Reference.BundleId));
    }

    [Fact]
    public void Dependency_is_not_ready_until_its_required_bundle_is_loaded()
    {
        var schedule = new RuntimeManifestSchedule(
            new[] { Entry(1), Entry(2) },
            new[] { new RuntimeManifestDependency(2, 1) });

        var loaded = new HashSet<ulong>();

        Assert.False(schedule.IsDependencyReady(2, id => loaded.Contains(id)));

        loaded.Add(1);

        Assert.True(schedule.IsDependencyReady(2, id => loaded.Contains(id)));
    }

    [Fact]
    public void Completed_localization_is_not_a_dependency_load_requirement()
    {
        var schedule = new RuntimeManifestSchedule(
            new[] { Entry(1), Entry(2) },
            new[] { new RuntimeManifestDependency(2, 1) });

        var localized = new HashSet<ulong> { 1 };
        var loaded = new HashSet<ulong>();

        Assert.False(schedule.IsDependencyReady(2, id => loaded.Contains(id)));
        Assert.Contains(1UL, localized);
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
