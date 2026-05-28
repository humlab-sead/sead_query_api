using System.Collections.Generic;

namespace SeadQueryCore.QueryComposer;

/// <summary>
/// Represents the composed anchor-filter query for the new query-composer path.
/// </summary>
public sealed class ComposedFilterQuery
{
    /// <summary>
    /// Anchor table used by the composed query.
    /// </summary>
    public string AnchorTable { get; init; } = string.Empty;

    /// <summary>
    /// Anchor key column returned by the composed query.
    /// The first vertical slice uses the shared `target_id` alias.
    /// </summary>
    public string AnchorKeyColumn { get; init; } = QueryComposerAliases.AnchorKeyColumn;

    /// <summary>
    /// Predicate SQL fragments that were combined to form the composed query.
    /// </summary>
    public IReadOnlyList<string> PredicateQueries { get; init; } = [];

    /// <summary>
    /// Final SQL for the composed filter query.
    /// </summary>
    public string Sql { get; init; } = string.Empty;
}
