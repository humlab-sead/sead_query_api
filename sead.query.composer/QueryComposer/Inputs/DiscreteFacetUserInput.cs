using System.Collections.Generic;

namespace SeadQueryComposer.QueryComposer.Inputs;

/// <summary>
/// Normalized user input for a discrete facet selection.
/// This is the first promoted contract from the shelved BackBurner work.
/// </summary>
public sealed class DiscreteFacetUserInput
{
    /// <summary>
    /// Selected facet values.
    /// </summary>
    public IReadOnlyList<object> Picks { get; init; } = [];

    /// <summary>
    /// Comparison operator for the selected values.
    /// Defaults to an inclusion check.
    /// </summary>
    public string Operator { get; init; } = "in";

    /// <summary>
    /// True when at least one facet value has been selected.
    /// </summary>
    public bool HasPicks => Picks.Count > 0;
}
