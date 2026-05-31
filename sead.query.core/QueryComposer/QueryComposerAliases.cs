namespace SeadQueryCore.QueryComposer;

/// <summary>
/// Shared column aliases for the first query-composer vertical slice.
/// </summary>
public static class QueryComposerAliases
{
    /// <summary>
    /// Alias for the facet-source key produced by a predicate query.
    /// </summary>
    public const string SourceKeyColumn = "source_id";

    /// <summary>
    /// Alias for the anchor key produced by a predicate query and consumed by composed filter and content queries.
    /// </summary>
    public const string AnchorKeyColumn = "target_id";
}
