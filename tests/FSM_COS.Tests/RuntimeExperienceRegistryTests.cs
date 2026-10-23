using TheSingularityWorkshop.FSM_COS;
using TheSingularityWorkshop.MicroBundleDomain;
using Xunit;

namespace FSM_COS.Tests;

public sealed class RuntimeExperienceRegistryTests
{
    private static RuntimeAssembly ComposeEmpty(ulong runtimeId) =>
        new FsmCos(new EmptyCatalog()).Execute(RuntimeManifest.Empty(runtimeId));

    private sealed class EmptyCatalog : IMicroBundleCatalog
    {
        public bool TryResolve(ulong bundleId, out IMicroBundle? bundle)
        {
            bundle = null;
            return false;
        }

        public bool TryResolve(ulong bundleId, string version, out IMicroBundle? bundle)
        {
            bundle = null;
            return false;
        }
    }

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

    [Fact]
    public void Host_composes_replacement_before_activation_and_retains_both_experiences()
    {
        var host = new RuntimeExperienceHost(new FsmCos(new EmptyCatalog()));

        var first = host.ComposeAndRegister(RuntimeManifest.Empty(101));
        Assert.True(host.TryActivate(101));

        var replacement = host.ComposeAndRegister(RuntimeManifest.Empty(202));

        Assert.Equal((ulong)101, host.Registry.ActiveRuntimeId);
        Assert.Same(first, host.Registry.Assemblies[101]);
        Assert.Same(replacement, host.Registry.Assemblies[202]);

        Assert.True(host.TryActivate(202));
        Assert.Equal((ulong)202, host.Registry.ActiveRuntimeId);
        Assert.Equal(2, host.Registry.Assemblies.Count);
    }

    [Fact]
    public void Host_rejects_duplicate_runtime_identity_without_replacing_existing_assembly()
    {
        var host = new RuntimeExperienceHost(new FsmCos(new EmptyCatalog()));
        var original = host.ComposeAndRegister(RuntimeManifest.Empty(101));

        Assert.Throws<InvalidOperationException>(() =>
            host.ComposeAndRegister(RuntimeManifest.Empty(101)));

        Assert.Same(original, host.Registry.Assemblies[101]);
        Assert.Single(host.Registry.Assemblies);
    }

}
