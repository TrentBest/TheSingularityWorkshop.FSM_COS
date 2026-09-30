namespace TheSingularityWorkshop.FSM_COS;

/// <summary>Classifies a published manifest entry for initial scheduling.</summary>
public enum ManifestExecutionClass
{
    /// <summary>
    /// Not required by another manifest entry and therefore eligible for
    /// broad parallel localization.
    /// </summary>
    Independent = 0,

    /// <summary>
    /// Required by another manifest entry and therefore subject to dependency
    /// ordering during composition.
    /// </summary>
    Dependency = 1
}
