namespace TheSingularityWorkshop.FSM_COS;

/// <summary>
/// A published dependency edge: BundleId requires DependencyId.
/// </summary>
public readonly record struct RuntimeManifestDependency(
    ulong BundleId,
    ulong DependencyId)
{
    public bool IsValid =>
        BundleId != 0 &&
        DependencyId != 0 &&
        BundleId != DependencyId;
}
