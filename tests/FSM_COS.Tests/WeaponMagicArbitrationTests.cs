using TheSingularityWorkshop.FSM_COS;
using Xunit;

namespace FSM_COS.Tests;

/// <summary>
/// Executable architectural story:
/// a weapon capability is already part of the requested composition,
/// then a magic capability is introduced and brings Elements with it.
/// Loading establishes the complete set first; arbitration then lets the
/// independent capabilities reconcile against that shared set.
/// </summary>
public sealed class WeaponMagicArbitrationTests
{
    private const ulong WeaponId = 100;
    private const ulong ElementsId = 200;
    private const ulong MagicId = 300;

    [Fact]
    public void Adding_magic_loads_elements_before_magic_and_arbitrates_with_existing_weapon()
    {
        var loadOrder = new List<ulong>();
        var weapon = new WeaponBundle(loadOrder);
        var elements = new ElementsBundle(loadOrder);
        var magic = new MagicBundle(loadOrder);

        var catalog = new TestCatalog(weapon, elements, magic);
        var manifest = new RuntimeManifest(
            RuntimeId: 7001,
            Bundles: new[]
            {
                BundleRequest.Unconfigured(WeaponId),
                BundleRequest.Unconfigured(MagicId)
            });

        var assembly = new FsmCos(catalog).Execute(manifest);

        Assert.Equal(new[] { WeaponId, ElementsId, MagicId }, assembly.Bundles.Select(bundle => bundle.Id));
        Assert.Equal(new[] { WeaponId, ElementsId, MagicId }, loadOrder);

        Assert.Equal(1, weapon.LoadCalls);
        Assert.Equal(1, elements.LoadCalls);
        Assert.Equal(1, magic.LoadCalls);

        Assert.True(magic.FoundElementsDuringArbitration);
        Assert.True(magic.FoundWeaponDuringArbitration);
        Assert.True(weapon.ReconciledWithMagic);
        Assert.Equal(1, assembly.ArbitrationRounds);
    }

    [Fact]
    public void Arbitration_sees_the_complete_installed_set_after_dependency_loading()
    {
        var weapon = new WeaponBundle(new List<ulong>());
        var elements = new ElementsBundle(new List<ulong>());
        var magic = new MagicBundle(new List<ulong>());

        var assembly = new FsmCos(new TestCatalog(weapon, elements, magic)).Execute(
            new RuntimeManifest(
                7002,
                new[]
                {
                    BundleRequest.Unconfigured(WeaponId),
                    BundleRequest.Unconfigured(MagicId)
                }));

        Assert.Equal(3, assembly.Bundles.Count);
        Assert.Contains(weapon, assembly.Bundles);
        Assert.Contains(elements, assembly.Bundles);
        Assert.Contains(magic, assembly.Bundles);
        Assert.True(weapon.ReconciledWithMagic);
        Assert.True(magic.FoundElementsDuringArbitration);
    }

    private sealed class WeaponBundle : IMicroBundle
    {
        private readonly List<ulong> _loadOrder;

        public WeaponBundle(List<ulong> loadOrder) => _loadOrder = loadOrder;

        public ulong Id => WeaponId;
        public IReadOnlyList<BundleRequest> Dependencies => Array.Empty<BundleRequest>();
        public int LoadCalls { get; private set; }
        public bool ReconciledWithMagic { get; private set; }

        public void Load(MicroBundleLoadContext context)
        {
            LoadCalls++;
            _loadOrder.Add(Id);
        }

        public bool Arbitrate(ArbitrationContext context, int roundIndex)
        {
            if (ReconciledWithMagic || !context.Contains(MagicId))
                return false;

            ReconciledWithMagic = true;
            return true;
        }
    }

    private sealed class ElementsBundle : IMicroBundle
    {
        private readonly List<ulong> _loadOrder;

        public ElementsBundle(List<ulong> loadOrder) => _loadOrder = loadOrder;

        public ulong Id => ElementsId;
        public IReadOnlyList<BundleRequest> Dependencies => Array.Empty<BundleRequest>();
        public int LoadCalls { get; private set; }

        public void Load(MicroBundleLoadContext context)
        {
            LoadCalls++;
            _loadOrder.Add(Id);
        }

        public bool Arbitrate(ArbitrationContext context, int roundIndex) => false;
    }

    private sealed class MagicBundle : IMicroBundle
    {
        private readonly List<ulong> _loadOrder;

        public MagicBundle(List<ulong> loadOrder) => _loadOrder = loadOrder;

        public ulong Id => MagicId;
        public IReadOnlyList<BundleRequest> Dependencies =>
            new[] { BundleRequest.Unconfigured(ElementsId) };

        public int LoadCalls { get; private set; }
        public bool FoundElementsDuringArbitration { get; private set; }
        public bool FoundWeaponDuringArbitration { get; private set; }

        public void Load(MicroBundleLoadContext context)
        {
            LoadCalls++;
            _loadOrder.Add(Id);
        }

        public bool Arbitrate(ArbitrationContext context, int roundIndex)
        {
            var foundElements = context.Contains(ElementsId);
            var foundWeapon = context.Contains(WeaponId);

            if (foundElements && foundWeapon)
            {
                FoundElementsDuringArbitration = true;
                FoundWeaponDuringArbitration = true;
                return roundIndex == 0;
            }

            return false;
        }
    }

    private sealed class TestCatalog : IMicroBundleCatalog
    {
        private readonly Dictionary<ulong, IMicroBundle> _bundles;

        public TestCatalog(params IMicroBundle[] bundles) =>
            _bundles = bundles.ToDictionary(bundle => bundle.Id);

        public bool TryResolve(ulong bundleId, out IMicroBundle? bundle) =>
            _bundles.TryGetValue(bundleId, out bundle);
    }
}
