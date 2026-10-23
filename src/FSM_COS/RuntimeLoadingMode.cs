namespace TheSingularityWorkshop.FSM_COS;

/// <summary>Controls whether FSM_COS loads MicroBundles sequentially or attempts safe parallel loading.</summary>
public enum RuntimeLoadingMode
{
    /// <summary>Loads bundles in deterministic dependency-first order. This is the default.</summary>
    Sequential = 0,

    /// <summary>Allows independent bundles explicitly marked parallel-safe to load concurrently.</summary>
    Parallel = 1
}
