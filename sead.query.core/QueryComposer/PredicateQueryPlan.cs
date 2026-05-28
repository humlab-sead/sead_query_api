namespace SeadQueryCore.QueryComposer;

/// <summary>
/// Represents one anchor-key predicate query before composition.
/// </summary>
public sealed class PredicateQueryPlan
{
    /// <summary>
    /// Facet code that produced this predicate query.
    /// </summary>
    public string FacetCode { get; init; } = string.Empty;

    /// <summary>
    /// Anchor table targeted by this predicate query.
    /// </summary>
    public string AnchorTable { get; init; } = string.Empty;

    /// <summary>
    /// Alias used for the facet-source key produced by this predicate query.
    /// </summary>
    public string SourceKeyColumn { get; init; } = QueryComposerAliases.SourceKeyColumn;

    /// <summary>
    /// Alias used for the anchor key produced by this predicate query.
    /// </summary>
    public string AnchorKeyColumn { get; init; } = QueryComposerAliases.AnchorKeyColumn;

    /// <summary>
    /// SQL that returns the facet-source key and anchor key aliases.
    /// </summary>
    public string Sql { get; init; } = string.Empty;
}
