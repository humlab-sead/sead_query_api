using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SeadQueryCore.QueryComposer;

/// <summary>
/// Composes anchor-key predicate queries for one anchor type using SQL INTERSECT.
/// </summary>
public sealed class IntersectComposedFilterQueryComposer : IComposedFilterQueryComposer
{
    public ComposedFilterQuery Compose(IReadOnlyCollection<PredicateQueryPlan> predicateQueries, string anchorTable, string anchorKeyColumn)
    {
        ArgumentNullException.ThrowIfNull(predicateQueries);
        ArgumentException.ThrowIfNullOrWhiteSpace(anchorTable);
        ArgumentException.ThrowIfNullOrWhiteSpace(anchorKeyColumn);

        var normalizedQueries = predicateQueries
            .Where(static query => !string.IsNullOrWhiteSpace(query.Sql))
            .Select(NormalizePlan)
            .ToList();

        ValidateAnchorCompatibility(normalizedQueries, anchorTable, anchorKeyColumn);

        return new ComposedFilterQuery
        {
            AnchorTable = anchorTable,
            AnchorKeyColumn = anchorKeyColumn,
            PredicateQueries = normalizedQueries,
            Sql = BuildSql(normalizedQueries.Select(static query => query.Sql).ToList(), anchorKeyColumn),
        };
    }

    private static void ValidateAnchorCompatibility(
        IReadOnlyList<PredicateQueryPlan> predicateQueries,
        string anchorTable,
        string anchorKeyColumn
    )
    {
        foreach (var predicateQuery in predicateQueries)
        {
            if (!string.Equals(predicateQuery.AnchorTable, anchorTable, StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException(
                    $"Predicate query '{predicateQuery.FacetCode}' targets anchor table '{predicateQuery.AnchorTable}', expected '{anchorTable}'."
                );
            }

            if (!string.Equals(predicateQuery.AnchorKeyColumn, anchorKeyColumn, StringComparison.Ordinal))
            {
                throw new InvalidOperationException(
                    $"Predicate query '{predicateQuery.FacetCode}' exposes anchor key '{predicateQuery.AnchorKeyColumn}', expected '{anchorKeyColumn}'."
                );
            }
        }
    }

    private static string BuildSql(IReadOnlyList<string> predicateQueries, string anchorKeyColumn)
    {
        if (predicateQueries.Count == 0)
        {
            return $"select {anchorKeyColumn}{Environment.NewLine}from (values (null)) as empty_set({anchorKeyColumn}){Environment.NewLine}where 1 = 0";
        }

        var cteNames = Enumerable.Range(0, predicateQueries.Count).Select(index => $"predicate_{index}").ToList();
        var sql = new StringBuilder();

        sql.AppendLine("with");
        for (var index = 0; index < predicateQueries.Count; index++)
        {
            var separator = index == predicateQueries.Count - 1 ? string.Empty : ",";
            sql.AppendLine($"  {cteNames[index]} as (");
            sql.AppendLine(Indent(predicateQueries[index], "    "));
            sql.AppendLine($"  ){separator}");
        }

        for (var index = 0; index < cteNames.Count; index++)
        {
            if (index > 0)
            {
                sql.AppendLine("intersect");
            }

            sql.Append($"select distinct {QueryComposerAliases.AnchorKeyColumn}{Environment.NewLine}from {cteNames[index]}");
            if (index < cteNames.Count - 1)
            {
                sql.AppendLine();
            }
        }

        return sql.ToString();
    }

    private static PredicateQueryPlan NormalizePlan(PredicateQueryPlan predicateQuery)
    {
        return new PredicateQueryPlan
        {
            FacetCode = predicateQuery.FacetCode,
            AnchorTable = predicateQuery.AnchorTable,
            SourceKeyColumn = predicateQuery.SourceKeyColumn,
            AnchorKeyColumn = predicateQuery.AnchorKeyColumn,
            Sql = predicateQuery.Sql.Trim().TrimEnd(';'),
        };
    }

    private static string Indent(string text, string indent)
    {
        var lines = text.Replace("\r\n", "\n", StringComparison.Ordinal).Split('\n');
        return string.Join(Environment.NewLine, lines.Select(line => $"{indent}{line}"));
    }
}
