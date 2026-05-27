using System;
using System.Collections.Generic;
using System.Linq;
using SeadQueryCore;

namespace SeadQueryComposer.RouteCompiler;

/// <summary>
/// Interface for parsing and resolving arrow-based route expressions in the SEAD query system.
/// Routes define the join paths between database tables using arrow notation (e.g., "sites -> samples -> measurements").
/// </summary>
public interface IArrowRouteParser
{
    /// <summary>
    /// Resolves a route expression into a flattened list of concrete table names.
    /// Supports both direct table references and macro expansion for reusable route segments.
    /// </summary>
    /// <param name="route">Arrow-separated route expression (e.g., "sites -> samples -> results")</param>
    /// <returns>Ordered list of concrete table names representing the join path</returns>
    /// <example>
    /// Input: "sites -> SAMPLE_CHAIN -> results"
    /// Output: ["sites", "samples", "sample_groups", "results"]
    /// (where SAMPLE_CHAIN expands to "samples -> sample_groups")
    /// </example>
    List<string> ResolveRoute(string route);
}

/// <summary>
/// Parses and resolves arrow-based route expressions for the new route-based join system.
/// Replaces the complex monolithic join generation with reusable, composable route definitions.
///
/// Key Features:
/// - Supports multiple arrow notations: ->, →, ⇒, ⟶, =>
/// - Macro expansion for reusable route segments
/// - Cycle detection to prevent infinite recursion
/// - Flexible whitespace handling around arrows
///
/// This is a core component of the new CTE + INTERSECT architecture, enabling
/// facets to define joins using simple, readable route expressions instead of
/// complex SQL templates for every source→anchor combination.
/// </summary>
public sealed class ArrowRouteParser : IArrowRouteParser
{
    /// <summary>
    /// Repository for retrieving stored route definitions and macro expansions
    /// </summary>
    private readonly IRouteRepository _repository;

    /// <summary>
    /// The standard arrow notation used internally after normalizing input variants
    /// </summary>
    public const string DefaultArrow = "->";

    /// <summary>
    /// Initializes a new ArrowRouteParser with the specified route repository
    /// </summary>
    /// <param name="repository">Repository containing route definitions for macro expansion</param>
    /// <exception cref="ArgumentNullException">Thrown when repository is null</exception>
    public ArrowRouteParser(IRouteRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    /// <summary>
    /// Resolves a route expression into a flattened list of concrete table names.
    /// Handles macro expansion, cycle detection, and arrow notation normalization.
    /// </summary>
    /// <param name="route">Arrow-separated route expression to resolve</param>
    /// <returns>Ordered list of concrete table names representing the complete join path</returns>
    /// <exception cref="ArgumentException">Thrown when route is null or empty</exception>
    /// <exception cref="InvalidOperationException">Thrown when a circular reference is detected during macro expansion</exception>
    /// <example>
    /// // Simple direct route
    /// ResolveRoute("sites -> samples -> measurements")
    /// // Returns: ["sites", "samples", "measurements"]
    ///
    /// // Route with macro expansion
    /// ResolveRoute("sites -> DATING_CHAIN")
    /// // If DATING_CHAIN = "samples -> dating_results"
    /// // Returns: ["sites", "samples", "dating_results"]
    /// </example>
    public List<string> ResolveRoute(string route)
    {
        if (string.IsNullOrWhiteSpace(route))
            throw new ArgumentException("Route is null or empty.", nameof(route));

        var output = new List<string>();
        var expansionStack = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        // Split the route into individual tokens and expand each one
        foreach (var token in SplitArrowRoute(route))
        {
            Expand(token, output, expansionStack);
        }

        return output;
    }

    /// <summary>
    /// Recursively expands a single token, handling both concrete table names and macro references.
    /// Maintains an expansion stack to detect and prevent circular references during macro resolution.
    /// </summary>
    /// <param name="token">The token to expand (either a table name or macro reference)</param>
    /// <param name="output">The output list to append resolved table names to</param>
    /// <param name="expansionStack">Stack tracking currently expanding macros for cycle detection</param>
    /// <exception cref="InvalidOperationException">Thrown when a circular macro reference is detected</exception>
    private void Expand(string token, List<string> output, HashSet<string> expansionStack)
    {
        if (string.IsNullOrWhiteSpace(token))
            return;

        // Check if this token is a macro reference or a concrete table name
        if (!_repository.HasRoute(token))
        {
            // Not a macro: treat as concrete table name and add directly to output
            output.Add(token);
            return;
        }

        // This is a macro reference - check for circular dependencies
        if (!expansionStack.Add(token))
        {
            // Already expanding this token -> circular reference detected
            throw new InvalidOperationException(
                $"Route expansion cycle detected at '{token}'. " + $"Stack: {string.Join(" -> ", expansionStack)}"
            );
        }

        // Retrieve the macro definition and expand it recursively
        var inner = _repository.GetRoute(token)?.Specification;
        if (string.IsNullOrWhiteSpace(inner))
        {
            // Empty or null macro definition - remove from stack and continue
            expansionStack.Remove(token);
            return;
        }

        // Recursively expand each token in the macro definition
        foreach (var innerToken in SplitArrowRoute(inner))
        {
            Expand(innerToken, output, expansionStack);
        }

        // Remove this token from the expansion stack after successful expansion
        expansionStack.Remove(token);
    }

    /// <summary>
    /// Splits an arrow-separated route string into individual tokens.
    /// Normalizes various arrow notations and handles flexible whitespace.
    /// </summary>
    /// <param name="encoded">The encoded route string to split</param>
    /// <returns>Enumerable of trimmed, non-empty tokens</returns>
    /// <example>
    /// SplitArrowRoute("sites -> samples   →   results")
    /// Returns: ["sites", "samples", "results"]
    /// </example>
    private static IEnumerable<string> SplitArrowRoute(string encoded)
    {
        // Normalize common arrow variants to the standard DefaultArrow notation
        // Supports: ->, →, ⇒, ⟶, => for maximum flexibility in route definitions
        var separators = new[] { "->", "→", "⇒", "⟶", "=>" };
        string normalized = encoded;

        foreach (var sep in separators)
        {
            if (sep == DefaultArrow)
                continue;
            normalized = normalized.Replace(sep, DefaultArrow, StringComparison.Ordinal);
        }

        // Split on the normalized arrow and handle variable spacing
        // Supports: "A -> B", "A->B", "A  ->   B" all equivalently
        var parts = normalized.Split(DefaultArrow, StringSplitOptions.RemoveEmptyEntries);

        foreach (var p in parts)
        {
            var t = p.Trim();
            if (!string.IsNullOrWhiteSpace(t))
                yield return t;
        }
    }
}
