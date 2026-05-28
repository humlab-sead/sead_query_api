using System;
using System.Linq;
using System.Text;

namespace SeadQueryCore.QueryComposer;

/// <summary>
/// Builds the first target facet-content query from a composed anchor-filter query.
/// This initial implementation supports only discrete target facets whose target table matches the composed anchor table.
/// </summary>
public sealed class DiscreteFacetContentQueryComposer : IFacetContentQueryComposer
{
    public FacetContentQueryPlan Compose(FacetsConfig2 facetsConfig, ComposedFilterQuery composedFilterQuery)
    {
        ArgumentNullException.ThrowIfNull(facetsConfig);
        ArgumentNullException.ThrowIfNull(composedFilterQuery);

        var targetFacet = facetsConfig.TargetFacet ?? throw new ArgumentException("TargetFacet must be set on facetsConfig.", nameof(facetsConfig));
        var targetTable = targetFacet.TargetTable ?? throw new InvalidOperationException("Target facet does not define a target table.");

        if (targetFacet.FacetTypeId != EFacetType.Discrete)
        {
            throw new InvalidOperationException($"Target facet '{targetFacet.FacetCode}' is not a discrete facet.");
        }

        if (string.IsNullOrWhiteSpace(targetFacet.CategoryIdExpr))
        {
            throw new InvalidOperationException($"Target facet '{targetFacet.FacetCode}' does not define CategoryIdExpr.");
        }

        if (string.IsNullOrWhiteSpace(composedFilterQuery.Sql))
        {
            throw new ArgumentException("Composed filter query SQL must be provided.", nameof(composedFilterQuery));
        }

        var resolvedTargetTable = targetTable.TableOrUdfName;
        if (!string.Equals(resolvedTargetTable, composedFilterQuery.AnchorTable, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException(
                $"Target facet '{targetFacet.FacetCode}' uses table '{resolvedTargetTable}', expected anchor table '{composedFilterQuery.AnchorTable}'."
            );
        }

        var categoryExpression = targetFacet.CategoryIdExpr;
        var targetTableName = targetTable.ResolvedSqlJoinName;
        var targetTableAliasOrName = targetTable.ResolvedAliasOrTableOrUdfName;
        var targetKeyColumn = targetTable.Table?.PrimaryKeyName ?? throw new InvalidOperationException(
            $"Target facet '{targetFacet.FacetCode}' does not define a target table primary key."
        );

        return new FacetContentQueryPlan
        {
            TargetFacetCode = targetFacet.FacetCode,
            AnchorTable = composedFilterQuery.AnchorTable,
            AnchorKeyColumn = composedFilterQuery.AnchorKeyColumn,
            ComposedFilterSql = composedFilterQuery.Sql,
            Sql = BuildSql(
                composedFilterQuery.Sql,
                categoryExpression,
                targetTableName,
                targetTableAliasOrName,
                targetKeyColumn,
                composedFilterQuery.AnchorKeyColumn),
        };
    }

    private static string BuildSql(
        string composedFilterSql,
        string categoryExpression,
        string targetTableName,
        string targetTableAliasOrName,
        string targetKeyColumn,
        string anchorKeyColumn)
    {
        var sql = new StringBuilder();
        sql.AppendLine("with composed_filter as (");
        sql.AppendLine(Indent(composedFilterSql.Trim(), "  "));
        sql.AppendLine(")");
        sql.AppendLine($"select {categoryExpression} as category, count(*)::int as count");
        sql.AppendLine($"from {targetTableName}");
        sql.AppendLine(
            $"join composed_filter on composed_filter.{anchorKeyColumn} = {targetTableAliasOrName}.{targetKeyColumn}");
        sql.AppendLine($"group by {categoryExpression}");
        sql.Append("order by ").Append(categoryExpression);
        return sql.ToString();
    }

    private static string Indent(string text, string indent)
    {
        var lines = text.Replace("\r\n", "\n", StringComparison.Ordinal).Split('\n');
        return string.Join(Environment.NewLine, lines.Select(line => $"{indent}{line}"));
    }
}