using System;
using System.Linq;
using System.Text;
using SeadQueryCore.QueryBuilder;

namespace SeadQueryCore.QueryComposer;

/// <summary>
/// Builds the first target facet-content query from a composed anchor-filter query.
/// This initial implementation supports discrete target facets whose categories can be reached from the composed anchor table.
/// </summary>
public sealed class DiscreteFacetContentQueryComposer : IFacetContentQueryComposer
{
    private readonly IPathFinder _pathFinder;
    private readonly IJoinsClauseCompiler _joinsClauseCompiler;

    public DiscreteFacetContentQueryComposer(IPathFinder pathFinder, IJoinsClauseCompiler joinsClauseCompiler)
    {
        _pathFinder = pathFinder ?? throw new ArgumentNullException(nameof(pathFinder));
        _joinsClauseCompiler = joinsClauseCompiler ?? throw new ArgumentNullException(nameof(joinsClauseCompiler));
    }

    public FacetContentQueryPlan Compose(
        FacetsConfig2 facetsConfig,
        ComposedFilterQuery composedFilterQuery,
        string targetJoinColumn,
        string anchorToTargetSql
    )
    {
        ArgumentNullException.ThrowIfNull(facetsConfig);
        ArgumentNullException.ThrowIfNull(composedFilterQuery);
        ArgumentException.ThrowIfNullOrWhiteSpace(targetJoinColumn);

        var targetFacet =
            facetsConfig.TargetFacet ?? throw new ArgumentException("TargetFacet must be set on facetsConfig.", nameof(facetsConfig));
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
        var requiresTargetRoute = !string.Equals(resolvedTargetTable, composedFilterQuery.AnchorTable, StringComparison.OrdinalIgnoreCase);
        if (requiresTargetRoute && string.IsNullOrWhiteSpace(anchorToTargetSql))
        {
            throw new InvalidOperationException(
                $"Target facet '{targetFacet.FacetCode}' requires an anchor-to-target route from '{composedFilterQuery.AnchorTable}' to '{resolvedTargetTable}'."
            );
        }

        var categoryExpression = targetFacet.CategoryIdExpr;
        var targetTableName = targetTable.ResolvedSqlJoinName;
        var targetTableAliasOrName = targetTable.ResolvedAliasOrTableOrUdfName;
        var targetJoins = BuildTargetFacetJoins(facetsConfig, targetFacet, targetTableAliasOrName);

        return new FacetContentQueryPlan
        {
            TargetFacetCode = targetFacet.FacetCode,
            AnchorTable = composedFilterQuery.AnchorTable,
            AnchorKeyColumn = composedFilterQuery.AnchorKeyColumn,
            AnchorJoinColumn = targetJoinColumn,
            ComposedFilterSql = composedFilterQuery.Sql,
            Sql = BuildSql(
                composedFilterQuery.Sql,
                categoryExpression,
                targetTableName,
                targetTableAliasOrName,
                targetJoins,
                targetJoinColumn,
                composedFilterQuery.AnchorKeyColumn,
                anchorToTargetSql
            ),
        };
    }

    private static string BuildSql(
        string composedFilterSql,
        string categoryExpression,
        string targetTableName,
        string targetTableAliasOrName,
        string[] targetJoins,
        string targetJoinColumn,
        string anchorKeyColumn,
        string anchorToTargetSql
    )
    {
        var sql = new StringBuilder();
        sql.AppendLine("with composed_filter as (");
        sql.AppendLine(Indent(composedFilterSql.Trim(), "  "));
        if (!string.IsNullOrWhiteSpace(anchorToTargetSql))
        {
            sql.AppendLine("),");
            sql.AppendLine("target_route as (");
            sql.AppendLine(Indent(anchorToTargetSql.Trim(), "  "));
            sql.AppendLine(")");
        }
        else
        {
            sql.AppendLine(")");
        }
        sql.AppendLine($"select {categoryExpression} as category, count(*)::int as count");
        sql.AppendLine($"from {targetTableName}");
        foreach (var join in targetJoins)
        {
            sql.AppendLine(join);
        }
        if (!string.IsNullOrWhiteSpace(anchorToTargetSql))
        {
            sql.AppendLine($"join target_route on target_route.target_id = {targetTableAliasOrName}.{targetJoinColumn}");
            sql.AppendLine($"join composed_filter on composed_filter.{anchorKeyColumn} = target_route.source_id");
        }
        else
        {
            sql.AppendLine($"join composed_filter on composed_filter.{anchorKeyColumn} = {targetTableAliasOrName}.{targetJoinColumn}");
        }
        sql.AppendLine($"group by {categoryExpression}");
        sql.Append("order by ").Append(categoryExpression);
        return sql.ToString();
    }

    private string[] BuildTargetFacetJoins(FacetsConfig2 facetsConfig, Facet targetFacet, string rootTableName)
    {
        var targetTables = targetFacet
            .GetResolvedTableNames()
            .Where(name => !string.Equals(name, rootTableName, StringComparison.OrdinalIgnoreCase))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        if (targetTables.Count == 0)
        {
            return [];
        }

        var routes = _pathFinder.Find(rootTableName, targetTables, true);
        return _joinsClauseCompiler.Compile(routes, facetsConfig).ToArray();
    }

    private static string Indent(string text, string indent)
    {
        var lines = text.Replace("\r\n", "\n", StringComparison.Ordinal).Split('\n');
        return string.Join(Environment.NewLine, lines.Select(line => $"{indent}{line}"));
    }
}
