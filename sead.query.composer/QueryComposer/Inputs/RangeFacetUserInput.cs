using System;

namespace SeadQueryComposer.QueryComposer.Inputs;

/// <summary>
/// Normalized user input for a range facet selection.
/// Represents an interval range for filtering range-based facet categories.
/// </summary>
public sealed class RangeFacetUserInput
{
    /// <summary>
    /// Lower bound of the selected range (inclusive).
    /// </summary>
    public int? Lower { get; init; }

    /// <summary>
    /// Upper bound of the selected range (exclusive).
    /// </summary>
    public int? Upper { get; init; }

    /// <summary>
    /// True when at least one bound has been specified.
    /// </summary>
    public bool HasPicks => Lower.HasValue || Upper.HasValue;

    /// <summary>
    /// Comparison operator for the selected range.
    /// Defaults to interval overlap ("&amp;&amp;" for PostgreSQL int4range types).
    /// </summary>
    public string Operator { get; init; } = "&&";
}
