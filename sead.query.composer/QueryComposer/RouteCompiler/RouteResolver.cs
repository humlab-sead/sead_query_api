using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using SeadQueryCore;

namespace SeadQueryComposer.RouteCompiler;

/// <summary>
/// Factory for creating RouteGraph instances with database relationship data.
/// Provides a centralized way to construct graph representations of table relationships
/// for the new route-based join system.
/// Currently uses FK/PK relationships found in facet schema (table_relation).
/// This might change in the future when old query engine is deprecated.
/// </summary>
public class RouteGraphFactory : IRouteGraphFactory
{
    /// <summary>
    /// Repository registry for accessing table relationship data
    /// </summary>
    private readonly IRepositoryRegistry Registry;

    /// <summary>
    /// Initializes a new RouteGraphFactory with the specified repository registry
    /// </summary>
    /// <param name="registry">Repository registry containing table relationship information</param>
    public RouteGraphFactory(IRepositoryRegistry registry) => Registry = registry;

    /// <summary>
    /// Creates a new RouteGraph populated with all available table relationships.
    /// The graph represents the foreign key/primary key relationships between tables
    /// that can be used for generating join paths in the new query architecture.
    /// </summary>
    /// <returns>A RouteGraph containing all table relationships from the database</returns>
    public RouteResolver CreateGraph()
    {
        return new RouteResolver(Registry.Relations.GetEdges());
    }
}

/// <summary>
/// Factory interface for creating RouteGraph instances
/// </summary>
public interface IRouteGraphFactory
{
    /// <summary>
    /// Creates a new RouteGraph instance with current database relationships
    /// </summary>
    /// <returns>A populated RouteGraph ready for route resolution</returns>
    RouteResolver CreateGraph();
}

/// <summary>
/// Interface for graph-based route resolution between database tables.
/// Provides access to table relationships and route calculation functionality
/// for the new CTE + INTERSECT query architecture.
/// </summary>
public interface IRouteResolver
{
    /// <summary>
    /// Dictionary of all table relationships indexed by (SourceTable, TargetTable) pairs.
    /// Contains both forward and reverse relationships for bidirectional navigation.
    /// </summary>
    IReadOnlyDictionary<(string SourceTable, string TargetTable), TableRelation> Relations { get; }

    /// <summary>
    /// Dictionary of all table nodes in the graph indexed by table name.
    /// Includes the actual Table entities for metadata access.
    /// </summary>
    IReadOnlyDictionary<string, Table> Nodes { get; }

    /// <summary>
    /// Resolves a sequence of table names into the corresponding TableRelation objects
    /// that represent the join path between those tables.
    /// </summary>
    /// <param name="tables">Ordered sequence of table names representing the desired join path</param>
    /// <returns>List of TableRelation objects representing the joins needed to connect the tables</returns>
    /// <exception cref="InvalidOperationException">Thrown when no relationship exists between consecutive tables</exception>
    /// <exception cref="KeyNotFoundException">Thrown when a table name cannot be resolved in the graph</exception>
    List<TableRelation> Resolve(IEnumerable<string> tables);
}

/// <summary>
/// Resolves routes using table relationships in the database.
///
/// This class is a core component of the new route-based join system, replacing the complex
/// monolithic join path generation with a clean graph-based approach. It maintains a graph
/// of foreign key/primary key relationships between tables and provides efficient route
/// resolution for generating join sequences.
///
/// Key Features:
/// - Bidirectional relationship support (automatically creates reverse edges)
/// - Flexible table name resolution (handles variations like "tbl_" prefixes)
/// - Fast relationship lookup using dictionary indexing
/// - Integration with the ArrowRouteParser for complete route resolution
///
/// Usage in New Architecture:
/// 1. ArrowRouteParser resolves macro routes to concrete table sequences
/// 2. RouteGraph converts table sequences to actual TableRelation join paths
/// 3. RouteSqlCompiler generates SQL from the resolved TableRelation paths
/// </summary>
public sealed class RouteResolver : IRouteResolver
{
    /// <summary>
    /// Dictionary of all table relationships indexed by (SourceTable, TargetTable) pairs.
    /// Provides O(1) lookup for relationship existence and details between any two tables.
    /// </summary>
    public IReadOnlyDictionary<(string SourceTable, string TargetTable), TableRelation> Relations { get; }

    /// <summary>
    /// Dictionary of all table nodes in the graph indexed by table name.
    /// Contains the actual Table entities for accessing metadata like column information.
    /// </summary>
    public IReadOnlyDictionary<string, Table> Nodes { get; }

    /// <summary>
    /// Initializes a new RouteGraph with the specified table relationships.
    /// </summary>
    /// <param name="edges">List of TableRelation objects representing database foreign key relationships</param>
    /// <param name="bidirectional">If true (default), automatically creates reverse relationships for bidirectional navigation</param>
    /// <exception cref="ArgumentNullException">Thrown when edges parameter is null</exception>
    /// <remarks>
    /// The bidirectional parameter enables navigation in both directions along foreign key relationships.
    /// For example, if there's a FK from "samples" to "sites", bidirectional mode also creates
    /// a reverse relationship from "sites" to "samples" for flexible route planning.
    /// </remarks>
    public RouteResolver(List<TableRelation> edges, bool bidirectional = true)
    {
        ArgumentNullException.ThrowIfNull(edges);

        // Create bidirectional edges by adding reverse relationships
        if (bidirectional)
            edges = [.. edges, .. edges.ReversedEdges()];

        // Build fast lookup dictionaries for relationships and nodes
        Relations = edges.ToDictionary(tr => (tr.SourceTable.Name, tr.TargetTable.Name), tr => tr);
        Nodes = edges.GetNodes();
    }

    /// <summary>
    /// Resolves a sequence of table names into the corresponding TableRelation join path.
    /// This method converts the output from ArrowRouteParser into actual database relationship
    /// objects that can be used for SQL generation.
    /// </summary>
    /// <param name="tables">Ordered sequence of table names representing the desired join path</param>
    /// <returns>List of TableRelation objects representing the joins needed to connect the tables in sequence</returns>
    /// <exception cref="InvalidOperationException">Thrown when no relationship exists between consecutive tables in the sequence</exception>
    /// <exception cref="KeyNotFoundException">Thrown when a table name cannot be resolved in the graph</exception>
    /// <example>
    /// // Given tables: ["sites", "samples", "measurements"]
    /// // Returns: [SitesToSamplesRelation, SamplesToMeasurementsRelation]
    /// var route = graph.GetRoute(new[] { "sites", "samples", "measurements" });
    /// </example>
    public List<TableRelation> Resolve(IEnumerable<string> tables)
    {
        var tableList = tables.ToList();
        var route = new List<TableRelation>();

        // Walk through consecutive table pairs to build the join path
        for (int i = 0; i < tableList.Count - 1; i++)
        {
            var source = tableList[i];
            var target = tableList[i + 1];

            // Resolve table names and find the relationship between them
            var resolvedSource = ResolveNodeName(source);
            var resolvedTarget = ResolveNodeName(target);

            if (resolvedSource == resolvedTarget)
            {
                continue;
            }

            if (Relations.TryGetValue((resolvedSource, resolvedTarget), out var relation))
            {
                route.Add(relation);
            }
            else
            {
                throw new InvalidOperationException(
                    $"No relation found between '{source}' ({resolvedSource}) and '{target}' ({resolvedTarget}). "
                        + $"Check that a foreign key relationship exists between these tables."
                );
            }
        }

        return route;
    }

    /// <summary>
    /// Resolves a table name to its canonical form in the graph, handling common naming variations.
    /// This method provides flexibility in table name specification by trying multiple common patterns.
    /// </summary>
    /// <param name="name">The table name to resolve (e.g., "sites", "site", "tbl_sites")</param>
    /// <returns>The canonical table name as stored in the graph</returns>
    /// <exception cref="KeyNotFoundException">Thrown when the table name cannot be resolved to any known table</exception>
    /// <remarks>
    /// Tries the following name patterns in order:
    /// 1. Exact name as provided
    /// 2. Name with "tbl_" prefix
    /// 3. Name with "tbl_" prefix and "s" suffix (for pluralization)
    ///
    /// This allows flexible route specification where users can write "sites" even if
    /// the actual table is named "tbl_sites" in the database.
    /// </remarks>
    private string ResolveNodeName(string name)
    {
        // Try multiple common naming patterns for maximum flexibility
        foreach (var tryName in new[] { name, $"tbl_{name}", $"tbl_{name}s" })
        {
            if (Nodes.TryGetValue(tryName, out var node))
                return node.TableOrUdfName;
        }

        // If no pattern matches, provide helpful error message
        var availableNodes = string.Join(", ", Nodes.Keys.Take(10));
        var suffix = Nodes.Count > 10 ? "..." : "";
        throw new KeyNotFoundException($"Table '{name}' not found in graph. Available tables include: {availableNodes}{suffix}");
    }
}
