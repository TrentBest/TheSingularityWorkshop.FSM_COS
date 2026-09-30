using TheSingularityWorkshop.FSM_API;
using Xunit;

namespace FSM_COS.Tests;

public sealed class RuntimeManifestLoadPlanTests
{
    [Fact]
    public void LoadPlan_starts_published_and_requires_localization_before_loading()
    {
        var entry = Entry(7);
        var plan = new RuntimeManifestLoadPlan(new[] { entry });

        Assert.Equal(MicroBundleRuntimeState.Published, plan.GetState(7));

        Assert.Throws<InvalidOperationException>(() => plan.MarkLoaded(7));

        plan.MarkLocalized(7);
        Assert.Equal(MicroBundleRuntimeState.Localized, plan.GetState(7));

        plan.MarkLoaded(7);
        Assert.Equal(MicroBundleRuntimeState.Loaded, plan.GetState(7));
    }

    [Fact]
    public void EvaluatePromotions_preserves_manifest_order_and_keeps_deferred_entries_experience_owned()
    {
        var resident = Entry(1, ManifestLoadStage.Resident);
        var deferred = Entry(2, ManifestLoadStage.Deferred);
        var plan = new RuntimeManifestLoadPlan(new[] { resident, deferred });

        plan.MarkLocalized(1);
        plan.MarkLocalized(2);

        var evaluator = new TestEvaluator(2);
        var promotions = plan.EvaluatePromotions(evaluator, new TestStateContext())
            .Select(x => x.Reference.BundleId)
            .ToArray();

        Assert.Equal(new ulong[] { 1, 2 }, promotions);
        Assert.Equal(new ulong[] { 2 }, evaluator.EvaluatedBundleIds);
    }

    [Fact]
    public void EvaluatePromotions_does_not_promote_deferred_entry_when_experience_declines()
    {
        var deferred = Entry(2, ManifestLoadStage.Deferred);
        var plan = new RuntimeManifestLoadPlan(new[] { deferred });

        plan.MarkLocalized(2);

        var promotions = plan.EvaluatePromotions(
                new TestEvaluator(),
                new TestStateContext())
            .ToArray();

        Assert.Empty(promotions);
    }

    [Fact]
    public void RuntimeManifest_can_carry_a_load_plan_without_replacing_legacy_requests()
    {
        var request = BundleRequest.Unconfigured(7);
        var entry = Entry(7);

        var manifest = new RuntimeManifest(
            42,
            new[] { request },
            LoadPlan: new[] { entry });

        Assert.Single(manifest.Bundles);
        Assert.Single(manifest.LoadPlan!);
        Assert.Equal(7UL, manifest.LoadPlan![0].Reference.BundleId);
    }

    [Fact]
    public void LoadPlan_rejects_identity_mismatch()
    {
        var invalid = new RuntimeManifestEntry(
            new MicroBundleReference(7, "1.0.0", "sha256:abc"),
            BundleRequest.Unconfigured(8));

        Assert.Throws<ArgumentException>(() =>
            new RuntimeManifestLoadPlan(new[] { invalid }));
    }

    private static RuntimeManifestEntry Entry(
        ulong id,
        ManifestLoadStage stage = ManifestLoadStage.Resident) =>
        new(
            new MicroBundleReference(id, "1.0.0", $"sha256:{id}"),
            BundleRequest.Unconfigured(id),
            stage);

    private sealed class TestEvaluator : IManifestLoadEvaluator
    {
        private readonly ulong _acceptedId;

        public TestEvaluator(ulong acceptedId = ulong.MaxValue) => _acceptedId = acceptedId;

        public List<ulong> EvaluatedBundleIds { get; } = new();

        public bool ShouldLoad(RuntimeManifestEntry entry, IStateContext? experienceContext)
        {
            EvaluatedBundleIds.Add(entry.Reference.BundleId);
            return entry.Reference.BundleId == _acceptedId;
        }
    }

    private sealed class TestStateContext : IStateContext
    {
        public string Name { get; set; } = "test";
        public bool IsValid { get; set; } = true;
    }
}
