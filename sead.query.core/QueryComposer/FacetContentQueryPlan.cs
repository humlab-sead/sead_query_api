namespace SeadQueryCore.QueryComposer;

/// <summary>
/// Represents a target facet-content query built from a composed anchor-filter query.
/// </summary>
public sealed class FacetContentQueryPlan
{
    /// <summary>
    /// Target facet code for the content query.
    /// </summary>
    public string TargetFacetCode { get; init; } = string.Empty;

    /// <summary>
    /// Anchor table expected by the content query.
    /// </summary>
    public string AnchorTable { get; init; } = string.Empty;

    /// <summary>
    /// Anchor key column expected by the content query.
    /// The first vertical slice uses the shared `target_id` alias.
    /// </summary>
    public string AnchorKeyColumn { get; init; } = QueryComposerAliases.AnchorKeyColumn;

    /// <summary>
    /// SQL used to define the composed anchor set consumed by this content query.
    /// </summary>
    public string ComposedFilterSql { get; init; } = string.Empty;

    /// <summary>
    /// Final SQL for the target facet-content query.
    /// </summary>
    public string Sql { get; init; } = string.Empty;
}
