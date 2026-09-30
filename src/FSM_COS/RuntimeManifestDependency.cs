namespace TheSingularityWorkshop.FSM_COS;

/// <summary>
/// One dependency edge baked into a published runtime execution plan.
/// </summary>
/// <param name="BundleId">The MicroBundle that requires the dependency.</param>
/// <param name="DependencyId">The MicroBundle required by the bundle.</param>
public readonly record struct RuntimeManifestDependency(
    ulong BundleId,
    ulong DependencyId)
{
    public bool IsValid =>
        BundleId != 0 &&
        DependencyId != 0 &&
        BundleId != DependencyId;
}
