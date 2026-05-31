using System.Collections.Generic;

namespace SeadQueryComposer.RouteCompiler;

/// <summary>
/// Minimal route-to-anchor configuration for the active query-composer path.
/// </summary>
public sealed class AnchorTemplate
{
    /// <summary>
    /// Optional full SQL override for cases where a route is not sufficient.
    /// </summary>
    public string ExplicitSql { get; init; }

    /// <summary>
    /// Ordered route segments between the facet source table and the anchor table.
    /// </summary>
    public IReadOnlyList<string> Route { get; init; } = [];

    /// <summary>
    /// True only when the facet source table is already the anchor table.
    /// </summary>
    public bool IsIdentityRoute { get; init; }

    /// <summary>
    /// True when the generated base query should enforce distinct source and anchor pairs.
    /// </summary>
    public bool RequiresDistinct { get; init; }

    /// <summary>
    /// Optional human-readable note for the route.
    /// </summary>
    public string Description { get; init; }
}
