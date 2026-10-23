using TheSingularityWorkshop.FSM_COS;
using Xunit;

namespace FSM_COS.Tests;

public sealed class RuntimeExperienceRegistryTests
{
    private static RuntimeAssembly ComposeEmpty(ulong runtimeId) =>
        new FsmCos(new TestCatalog()).Execute(RuntimeManifest.Empty(runtimeId));

    [Fact]
    public void Registry_retains_multiple_experiences_and_switches_without_removing_either()
    {
        var first = ComposeEmpty(101);
        var second = ComposeEmpty(202);
        var registry = new RuntimeExperienceRegistry();

        Assert.True(registry.TryRegister(first));
        Assert.True(registry.TryRegister(second));
        Assert.True(registry.TryActivate(101));
        Assert.Equal((ulong)101, registry.ActiveRuntimeId);
        Assert.True(registry.TryActivate(202));
        Assert.Equal((ulong)202, registry.ActiveRuntimeId);
        Assert.True(registry.TryGet(101, out var retainedFirst));
        Assert.Same(first, retainedFirst);
        Assert.True(registry.TryGet(202, out var retainedSecond));
        Assert.Same(second, retainedSecond);
        Assert.Equal(2, registry.Assemblies.Count);
    }

    [Fact]
    public void Registry_rejects_duplicate_identity_and_failed_activation_preserves_selection()
    {
        var first = ComposeEmpty(101);
        var differentObjectWithSameIdentity = ComposeEmpty(101);
        var registry = new RuntimeExperienceRegistry();

        Assert.True(registry.TryRegister(first));
        Assert.False(registry.TryRegister(differentObjectWithSameIdentity));
        Assert.True(registry.TryActivate(101));

        Assert.False(registry.TryActivate(999));
        Assert.Equal((ulong)101, registry.ActiveRuntimeId);
        Assert.Same(first, registry.Assemblies[101]);
    }

    [Fact]
    public void Removing_active_experience_clears_selection_but_keeps_other_experiences()
    {
        var first = ComposeEmpty(101);
        var second = ComposeEmpty(202);
        var registry = new RuntimeExperienceRegistry();
        registry.TryRegister(first);
        registry.TryRegister(second);
        registry.TryActivate(101);

        Assert.True(registry.TryRemove(101));
        Assert.Null(registry.ActiveRuntimeId);
        Assert.False(registry.TryGet(101, out _));
        Assert.True(registry.TryGet(202, out var retained));
        Assert.Same(second, retained);
        Assert.False(registry.TryRemove(101));
    }

    [Fact]
    public void Assembly_snapshot_cannot_mutate_registry_membership()
    {
        var registry = new RuntimeExperienceRegistry();
        registry.TryRegister(ComposeEmpty(101));

        var snapshot = registry.Assemblies;

        Assert.Throws<NotSupportedException>(() =>
            ((IDictionary<ulong, RuntimeAssembly>)snapshot).Add(202, ComposeEmpty(202)));
        Assert.Single(registry.Assemblies);
    }
}
