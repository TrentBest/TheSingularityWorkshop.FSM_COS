using TheSingularityWorkshop.FSM_COS;
using TheSingularityWorkshop.GrammarAi;
using TheSingularityWorkshop.MicroBundleDomain;
using TheSingularityWorkshop.ProtocolAi;
using TheSingularityWorkshop.Workshop.Gui;

namespace FSM_COS.Tests;

/// <summary>
/// Proves that FSM_COS can assemble the real AI and GUI packages without
/// referencing any of them from the composition kernel itself.
/// </summary>
public sealed class AiGuiCapabilityHandoffTests
{
    [Fact]
    public void Core_does_not_reference_AI_or_GUI_packages()
    {
        var references = typeof(FsmCos).Assembly
            .GetReferencedAssemblies()
            .Select(x => x.Name)
            .ToHashSet(StringComparer.Ordinal);

        Assert.DoesNotContain(typeof(ProtocolDefinition).Assembly.GetName().Name!, references);
        Assert.DoesNotContain(typeof(GrammarDefinition).Assembly.GetName().Name!, references);
        Assert.DoesNotContain(typeof(GuiNode).Assembly.GetName().Name!, references);
    }

    [Fact]
    public void Execute_assembles_ProtocolAI_GrammarAI_and_GUI_as_capabilities()
    {
        var protocol = new ProtocolCapabilityBundle();
        var grammar = new GrammarCapabilityBundle();
        var gui = new AiExchangeGuiCapabilityBundle();

        var assembly = new FsmCos(
                new TestCatalog(protocol, grammar, gui))
            .Execute(new RuntimeManifest(
                0xCAFEUL,
                new[] { new MicroBundleManifestEntry(gui.Id, "0.1.0-test") }));

        Assert.Equal(
            new[] { protocol.Id, grammar.Id, gui.Id },
            assembly.Bundles.Select(x => x.Id));

        Assert.True(assembly.TryGetBundle<ProtocolCapabilityBundle>(out var resolvedProtocol));
        Assert.True(assembly.TryGetBundle<GrammarCapabilityBundle>(out var resolvedGrammar));
        Assert.True(assembly.TryGetBundle<AiExchangeGuiCapabilityBundle>(out var resolvedGui));

        Assert.Same(protocol, resolvedProtocol);
        Assert.Same(grammar, resolvedGrammar);
        Assert.Same(gui, resolvedGui);

        Assert.NotNull(gui.Composition);
        Assert.Contains("PROTOCOL", gui.ExchangeText, StringComparison.Ordinal);
        Assert.Contains("GRAMMAR", gui.ExchangeText, StringComparison.Ordinal);
        Assert.Equal(
            "ai.extract",
            gui.Composition!.Find("ai-extract").Properties["command"]);
        Assert.Equal(
            "ai.submit",
            gui.Composition.Find("ai-submit").Properties["command"]);
    }

    [Fact]
    public void Grammar_capability_references_the_ProtocolAI_capability_without_copying_its_vocabulary()
    {
        var protocol = new ProtocolCapabilityBundle();
        var grammar = new GrammarCapabilityBundle();

        var assembly = new FsmCos(
                new TestCatalog(protocol, grammar))
            .Execute(new RuntimeManifest(
                0xBEEFUL,
                new[] { new MicroBundleManifestEntry(grammar.Id, "0.1.0-test") }));

        Assert.Equal(protocol.Protocol.Id, grammar.Grammar.Rules
            .SelectMany(rule => rule.RightHandSide)
            .Single(symbol => symbol.IsProtocolSymbol)
            .ProtocolReference!.Value.ProtocolId);

        Assert.True(assembly.TryGetBundle<ProtocolCapabilityBundle>(out var resolvedProtocol));
        Assert.Same(protocol, resolvedProtocol);
    }

    private sealed class TestCatalog : IMicroBundleCatalog
    {
        private readonly IReadOnlyDictionary<ulong, IMicroBundle> _bundles;

        public TestCatalog(params IMicroBundle[] bundles) =>
            _bundles = bundles.ToDictionary(x => x.Id);

        public bool TryResolve(ulong bundleId, string version, out IMicroBundle? bundle) =>
            _bundles.TryGetValue(bundleId, out bundle) &&
            string.Equals(bundle.Descriptor.Version, version, StringComparison.Ordinal);

        public bool TryResolve(ulong bundleId, out IMicroBundle? bundle) =>
            _bundles.TryGetValue(bundleId, out bundle);
    }

    private sealed class ProtocolCapabilityBundle : IMicroBundle
    {
        public const ulong BundleId = 0xA100UL;

        public ProtocolCapabilityBundle()
        {
            Descriptor = new MicroBundleDescriptor(BundleId, "0.1.0-test");
        }

        public MicroBundleDescriptor Descriptor { get; }
        public ulong Id => BundleId;
        public IReadOnlyList<MicroBundleDependencyRequest> Dependencies => Array.Empty<MicroBundleDependencyRequest>();

        public ProtocolDefinition Protocol { get; } =
            new ProtocolBuilder(0x2001UL, "WorkshopAI")
                .Define(0x2101UL, "Extract", "extract")
                .Define(0x2102UL, "Submit", "submit")
                .Define(0x2103UL, "Protocol", "protocol")
                .Define(0x2104UL, "Grammar", "grammar")
                .Build();

        public void Load(IMicroBundleLoadContext context) =>
            ArgumentNullException.ThrowIfNull(context);

        public bool Arbitrate(IMicroBundleArbitrationContext context, int roundIndex) => false;
    }

    private sealed class GrammarCapabilityBundle : IMicroBundle
    {
        public const ulong BundleId = 0xA200UL;

        public GrammarCapabilityBundle()
        {
            Descriptor = new MicroBundleDescriptor(
                BundleId,
                "0.1.0-test",
                new[] { new MicroBundleDependency(ProtocolCapabilityBundle.BundleId) });
        }

        public MicroBundleDescriptor Descriptor { get; }
        public ulong Id => BundleId;

        public IReadOnlyList<MicroBundleDependencyRequest> Dependencies => new[]
        {
            MicroBundleDependencyRequest.Unconfigured(ProtocolCapabilityBundle.BundleId)
        };

        public GrammarDefinition Grammar { get; } =
            new GrammarBuilder(0x3001UL, "WorkshopAIGrammar", 0x3101UL)
                .Rule(
                    0x3201UL,
                    0x3101UL,
                    GrammarSymbol.NonTerminal(0x3102UL))
                .Rule(
                    0x3202UL,
                    0x3102UL,
                    GrammarSymbol.Terminal(
                        new GrammarProtocolReference(0x2001UL, 0x2101UL)))
                .Build();

        public void Load(IMicroBundleLoadContext context) =>
            ArgumentNullException.ThrowIfNull(context);

        public bool Arbitrate(IMicroBundleArbitrationContext context, int roundIndex) => false;
    }

    private sealed class AiExchangeGuiCapabilityBundle : IMicroBundle
    {
        public const ulong BundleId = 0xA300UL;

        public AiExchangeGuiCapabilityBundle()
        {
            Descriptor = new MicroBundleDescriptor(
                BundleId,
                "0.1.0-test",
                new[]
                {
                    new MicroBundleDependency(ProtocolCapabilityBundle.BundleId),
                    new MicroBundleDependency(GrammarCapabilityBundle.BundleId)
                });
        }

        public MicroBundleDescriptor Descriptor { get; }
        public ulong Id => BundleId;

        public IReadOnlyList<MicroBundleDependencyRequest> Dependencies => new[]
        {
            MicroBundleDependencyRequest.Unconfigured(ProtocolCapabilityBundle.BundleId),
            MicroBundleDependencyRequest.Unconfigured(GrammarCapabilityBundle.BundleId)
        };

        public GuiNode? Composition { get; private set; }
        public string? ExchangeText { get; private set; }

        public void Load(IMicroBundleLoadContext context)
        {
            ArgumentNullException.ThrowIfNull(context);

            Composition = GuiBuilders.Panel("ai-exchange")
                .Child(GuiBuilders.Text("ai-title", "AI EXCHANGE"))
                .Child(GuiBuilders.TextBox("ai-protocol"))
                .Child(GuiBuilders.TextBox("ai-grammar"))
                .Child(GuiBuilders.TextBox("ai-input"))
                .Child(GuiBuilders.Button("ai-extract", "Extract to Clipboard")
                    .Property("command", "ai.extract"))
                .Child(GuiBuilders.Button("ai-submit", "Validate / Re-enter")
                    .Property("command", "ai.submit"))
                .Build();
        }

        public bool Arbitrate(IMicroBundleArbitrationContext context, int roundIndex)
        {
            ArgumentNullException.ThrowIfNull(context);

            var protocol = context.Bundles
                .OfType<ProtocolCapabilityBundle>()
                .Single();

            var grammar = context.Bundles
                .OfType<GrammarCapabilityBundle>()
                .Single();

            var exchange = string.Join(
                Environment.NewLine + Environment.NewLine,
                "PROTOCOL",
                protocol.Protocol.Describe(),
                "GRAMMAR",
                grammar.Grammar.Describe());

            var changed = !string.Equals(
                ExchangeText,
                exchange,
                StringComparison.Ordinal);

            ExchangeText = exchange;
            return changed;
        }
    }
}
