using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using SeadQueryComposer.QueryComposer.Inputs;

namespace SeadQueryComposer.RouteCompiler;

public interface IDiscreteFacetPredicateResolver
{
    string ResolveSql(
        string targetTable,
        string targetId,
        DiscreteFacetUserInput userInput,
        AnchorTemplate anchorTemplate,
        string anchorTable,
        string anchorId,
        IReadOnlyList<string> sourceCriteria = null
    );
}

/// <summary>
/// Resolves SQL that maps discrete facet picks to matching anchor records.
/// </summary>
/// <remarks>
/// Example: if the user picks category IDs 1 and 2, the generated SQL returns the
/// anchor records linked to those selected values.
/// </remarks>
public sealed class DiscreteFacetPredicateResolver : IDiscreteFacetPredicateResolver
{
    private readonly IRouteSqlCompiler _routeSqlCompiler;

    public DiscreteFacetPredicateResolver(IRouteSqlCompiler routeSqlCompiler)
    {
        _routeSqlCompiler = routeSqlCompiler ?? throw new ArgumentNullException(nameof(routeSqlCompiler));
    }

    /// <summary>
    /// Resolves SQL that maps discrete facet picks to matching anchor records based on the provided parameters.
    /// </summary>
    /// <param name="targetTable">The name of the target table.</param>
    /// <param name="targetId">The name of the target ID column.</param>
    /// <param name="userInput">The user input containing the selected discrete facet values.</param>
    /// <param name="anchorTemplate">The anchor template defining the route and SQL for the anchor table.</param>
    /// <param name="anchorTable">The name of the anchor table.</param>
    /// <param name="anchorId">The name of the anchor ID column.</param>
    /// <param name="sourceCriteria">Optional list of source criteria for filtering.</param>
    /// <returns>The generated SQL string.</returns>
    public string ResolveSql(
        string targetTable,
        string targetId,
        DiscreteFacetUserInput userInput,
        AnchorTemplate anchorTemplate,
        string anchorTable,
        string anchorId,
        IReadOnlyList<string> sourceCriteria = null
    )
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(targetTable);
        ArgumentException.ThrowIfNullOrWhiteSpace(targetId);
        ArgumentNullException.ThrowIfNull(userInput);
        ArgumentNullException.ThrowIfNull(anchorTemplate);
        ArgumentException.ThrowIfNullOrWhiteSpace(anchorTable);
        ArgumentException.ThrowIfNullOrWhiteSpace(anchorId);

        sourceCriteria ??= [];

        if (!string.IsNullOrWhiteSpace(anchorTemplate.ExplicitSql))
        {
            return anchorTemplate.ExplicitSql;
        }

        var baseSql =
            anchorTemplate.Route.Count > 0
                ? BuildRouteSql(targetTable, targetId, anchorTemplate.Route, anchorTable, anchorId, sourceCriteria)
                : BuildIdentitySql(targetTable, targetId, anchorId, anchorTemplate.RequiresDistinct, sourceCriteria);

        if (!userInput.HasPicks)
        {
            return baseSql;
        }

        return WrapWithSourceFilter(baseSql, userInput);
    }

    /// <summary>
    /// Builds SQL that joins the target table to the anchor table through the specified route of intermediate tables.
    /// </summary>
    /// <param name="targetTable">The name of the target table.</param>
    /// <param name="sourceKeyColumn">The name of the source key column in the target table.</param>
    /// <param name="route">The list of intermediate tables to join.</param>
    /// <param name="anchorTable">The name of the anchor table.</param>
    /// <param name="anchorId">The name of the anchor ID column.</param>
    /// <param name="sourceCriteria">Optional list of source criteria for filtering.</param>
    /// <returns>The generated SQL string.</returns>
    private string BuildRouteSql(
        string targetTable,
        string sourceKeyColumn,
        IReadOnlyList<string> route,
        string anchorTable,
        string anchorId,
        IReadOnlyList<string> sourceCriteria
    )
    {
        var tableChain = new List<string>(route.Count + 2) { targetTable };
        tableChain.AddRange(route);
        tableChain.Add(anchorTable);

        return sourceCriteria?.Count > 0
            ? _routeSqlCompiler.Compile(tableChain, sourceKeyColumn, anchorId, sourceCriteria)
            : _routeSqlCompiler.Compile(tableChain, sourceKeyColumn, anchorId);
    }


    /// <summary>
    /// Builds SQL that directly links the target table to the anchor table without any intermediate route tables.
    /// </summary>
    /// <param name="targetTable">The name of the target table.</param>
    /// <param name="targetId">The name of the target ID column.</param>
    /// <param name="anchorId">The name of the anchor ID column.</param>
    /// <param name="requiresDistinct">Indicates whether the SQL should include a DISTINCT clause.</param>
    /// <param name="sourceCriteria">Optional list of source criteria for filtering.</param>
    /// <returns>The generated SQL string.</returns>
    private static string BuildIdentitySql(
        string targetTable,
        string targetId,
        string anchorId,
        bool requiresDistinct,
        IReadOnlyList<string> sourceCriteria
    )
    {
        var distinct = requiresDistinct ? " distinct" : string.Empty;

        if (sourceCriteria?.Count > 0)
        {
            return $"select{distinct} X_0.{targetId} as source_id, X_0.{anchorId} as target_id{Environment.NewLine}from {targetTable} as X_0{Environment.NewLine}where {string.Join(" and ", sourceCriteria)}";
        }

        return $"select{distinct} {targetId} as source_id, {anchorId} as target_id{Environment.NewLine}from {targetTable}";
    }

    private static string BuildWhereClause(DiscreteFacetUserInput userInput)
    {
        if (string.IsNullOrWhiteSpace(userInput.Operator))
        {
            throw new ArgumentException("Discrete facet operators must be non-empty.", nameof(userInput));
        }

        var normalizedOperator = userInput.Operator.Trim().ToLowerInvariant();

        if (normalizedOperator is "in" or "not in")
        {
            return $"where source_id {normalizedOperator} ({string.Join(", ", userInput.Picks.Select(FormatLiteral))})";
        }

        if (userInput.Picks.Count != 1)
        {
            throw new ArgumentException("Non-set operators require exactly one selected value.", nameof(userInput));
        }

        return $"where source_id {userInput.Operator} {FormatLiteral(userInput.Picks[0])}";
    }

    private static string WrapWithSourceFilter(string baseSql, DiscreteFacetUserInput userInput)
    {
        return $"select *{Environment.NewLine}from ({Environment.NewLine}{baseSql}{Environment.NewLine}) as predicate_query{Environment.NewLine}{BuildWhereClause(userInput)}";
    }

    private static string FormatLiteral(object value)
    {
        if (value is null)
        {
            return "null";
        }

        return value switch
        {
            string text => $"'{text.Replace("'", "''", StringComparison.Ordinal)}'",
            char ch => $"'{ch.ToString().Replace("'", "''", StringComparison.Ordinal)}'",
            bool boolean => boolean ? "true" : "false",
            Enum enumValue => Convert.ToInt64(enumValue, CultureInfo.InvariantCulture).ToString(CultureInfo.InvariantCulture),
            IFormattable formattable => formattable.ToString(null, CultureInfo.InvariantCulture),
            _ => $"'{value.ToString()?.Replace("'", "''", StringComparison.Ordinal)}'",
        };
    }
}
