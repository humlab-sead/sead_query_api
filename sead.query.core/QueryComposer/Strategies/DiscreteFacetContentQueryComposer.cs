using System;
using System.Collections.Generic;
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
        string anchorToTargetSql,
        string categoryInfoSql = null
    )
    {
        ArgumentNullException.ThrowIfNull(facetsConfig);
        ArgumentNullException.ThrowIfNull(composedFilterQuery);
        ArgumentException.ThrowIfNullOrWhiteSpace(targetJoinColumn);

        var targetFacet =
            facetsConfig.TargetFacet ?? throw new ArgumentException("TargetFacet must be set on facetsConfig.", nameof(facetsConfig));
        var targetTable = targetFacet.TargetTable ?? throw new InvalidOperationException("Target facet does not define a target table.");

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
        var targetCriteria = ResolveTargetFacetCriteria(targetFacet, targetTableAliasOrName);

        return new FacetContentQueryPlan
        {
            TargetFacetCode = targetFacet.FacetCode,
            AnchorTable = composedFilterQuery.AnchorTable,
            AnchorKeyColumn = composedFilterQuery.AnchorKeyColumn,
            AnchorJoinColumn = targetJoinColumn,
            ComposedFilterSql = composedFilterQuery.Sql,
            Sql = targetFacet.FacetTypeId switch
            {
                EFacetType.Discrete => BuildDiscreteSql(
                    composedFilterQuery.Sql,
                    categoryExpression,
                    targetTableName,
                    targetTableAliasOrName,
                    targetJoins,
                    targetCriteria,
                    targetJoinColumn,
                    composedFilterQuery.AnchorKeyColumn,
                    anchorToTargetSql
                ),
                EFacetType.Range => BuildRangeSql(
                    composedFilterQuery.Sql,
                    categoryInfoSql,
                    categoryExpression,
                    targetTableName,
                    targetTableAliasOrName,
                    targetJoins,
                    targetCriteria,
                    targetJoinColumn,
                    composedFilterQuery.AnchorKeyColumn,
                    anchorToTargetSql,
                    targetFacet.CategoryIdType
                ),
                EFacetType.Intersect => BuildIntersectSql(
                    composedFilterQuery.Sql,
                    categoryInfoSql,
                    categoryExpression,
                    targetTableName,
                    targetTableAliasOrName,
                    targetJoins,
                    targetCriteria,
                    targetJoinColumn,
                    composedFilterQuery.AnchorKeyColumn,
                    anchorToTargetSql,
                    targetFacet.CategoryIdType,
                    targetFacet.CategoryIdOperator
                ),
                EFacetType.GeoPolygon => BuildGeoPolygonSql(
                    composedFilterQuery.Sql,
                    categoryInfoSql,
                    targetJoinColumn,
                    composedFilterQuery.AnchorKeyColumn,
                    anchorToTargetSql
                ),
                _ => throw new InvalidOperationException(
                    $"Target facet '{targetFacet.FacetCode}' is not supported by the composed content composer."
                ),
            },
        };
    }

    private static string BuildDiscreteSql(
        string composedFilterSql,
        string categoryExpression,
        string targetTableName,
        string targetTableAliasOrName,
        string[] targetJoins,
        IReadOnlyList<string> targetCriteria,
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
        sql.AppendLine($"select {categoryExpression} as category, count(distinct composed_filter.{anchorKeyColumn})::int as count");
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
        AppendWhereClauses(sql, targetCriteria, string.Empty);
        sql.AppendLine($"group by {categoryExpression}");
        sql.Append("order by ").Append(categoryExpression);
        return sql.ToString();
    }

    private static string BuildRangeSql(
        string composedFilterSql,
        string categoryInfoSql,
        string categoryExpression,
        string targetTableName,
        string targetTableAliasOrName,
        string[] targetJoins,
        IReadOnlyList<string> targetCriteria,
        string targetJoinColumn,
        string anchorKeyColumn,
        string anchorToTargetSql,
        string categoryIdType
    )
    {
        if (string.IsNullOrWhiteSpace(categoryInfoSql))
        {
            throw new InvalidOperationException("Range target facets require a category-info SQL definition.");
        }

        var sql = new StringBuilder();
        sql.AppendLine("with composed_filter as (");
        sql.AppendLine(Indent(composedFilterSql.Trim(), "  "));
        sql.AppendLine("),");
        if (!string.IsNullOrWhiteSpace(anchorToTargetSql))
        {
            sql.AppendLine("target_route as (");
            sql.AppendLine(Indent(anchorToTargetSql.Trim(), "  "));
            sql.AppendLine("),");
        }
        sql.AppendLine("categories(category, lower, upper) as (");
        sql.AppendLine(Indent(categoryInfoSql.Trim(), "  "));
        sql.AppendLine("),");
        sql.AppendLine("outerbounds(lower, upper) as (");
        sql.AppendLine("  select min(lower), max(upper)");
        sql.AppendLine("  from categories");
        sql.AppendLine(")");
        sql.AppendLine("select c.category, c.lower, c.upper, coalesce(r.count_column, 0) as count_column");
        sql.AppendLine("from categories c");
        sql.AppendLine("left join (");
        sql.AppendLine($"  select category, count(distinct composed_filter.{anchorKeyColumn}) as count_column");
        sql.AppendLine($"  from {targetTableName}");
        sql.AppendLine("  cross join outerbounds");
        sql.AppendLine("  join categories");
        sql.AppendLine($"    on categories.lower <= {categoryExpression}::{categoryIdType}");
        sql.AppendLine($"   and categories.upper >= {categoryExpression}::{categoryIdType}");
        sql.AppendLine(
            $"   and (not (categories.upper < outerbounds.upper and {categoryExpression}::{categoryIdType} = categories.upper))"
        );
        foreach (var join in targetJoins)
        {
            sql.AppendLine($"  {join}");
        }
        if (!string.IsNullOrWhiteSpace(anchorToTargetSql))
        {
            sql.AppendLine($"  join target_route on target_route.target_id = {targetTableAliasOrName}.{targetJoinColumn}");
            sql.AppendLine($"  join composed_filter on composed_filter.{anchorKeyColumn} = target_route.source_id");
        }
        else
        {
            sql.AppendLine($"  join composed_filter on composed_filter.{anchorKeyColumn} = {targetTableAliasOrName}.{targetJoinColumn}");
        }
        AppendWhereClauses(sql, targetCriteria, "  ");
        sql.AppendLine("  group by category");
        sql.AppendLine(") as r");
        sql.AppendLine("  on r.category = c.category");
        sql.Append("order by c.lower");
        return sql.ToString();
    }

    private static string BuildIntersectSql(
        string composedFilterSql,
        string categoryInfoSql,
        string categoryExpression,
        string targetTableName,
        string targetTableAliasOrName,
        string[] targetJoins,
        IReadOnlyList<string> targetCriteria,
        string targetJoinColumn,
        string anchorKeyColumn,
        string anchorToTargetSql,
        string categoryIdType,
        string categoryOperator
    )
    {
        if (string.IsNullOrWhiteSpace(categoryInfoSql))
        {
            throw new InvalidOperationException("Intersect target facets require a category-info SQL definition.");
        }

        if (string.IsNullOrWhiteSpace(categoryOperator))
        {
            throw new InvalidOperationException("Intersect target facets require a category operator.");
        }

        var sql = new StringBuilder();
        sql.AppendLine("with composed_filter as (");
        sql.AppendLine(Indent(composedFilterSql.Trim(), "  "));
        sql.AppendLine("),");
        if (!string.IsNullOrWhiteSpace(anchorToTargetSql))
        {
            sql.AppendLine("target_route as (");
            sql.AppendLine(Indent(anchorToTargetSql.Trim(), "  "));
            sql.AppendLine("),");
        }
        sql.AppendLine("categories(category, category_range, lower, upper) as (");
        sql.AppendLine(Indent(categoryInfoSql.Trim(), "  "));
        sql.AppendLine(")");
        sql.AppendLine("select c.category, c.lower, c.upper, coalesce(r.count_column, 0) as count_column");
        sql.AppendLine("from categories c");
        sql.AppendLine("left join (");
        sql.AppendLine($"  select category, count(distinct composed_filter.{anchorKeyColumn}) as count_column");
        sql.AppendLine($"  from {targetTableName}");
        sql.AppendLine("  join categories");
        sql.AppendLine($"    on categories.category_range {categoryOperator} {categoryExpression}::{categoryIdType}");
        foreach (var join in targetJoins)
        {
            sql.AppendLine($"  {join}");
        }
        if (!string.IsNullOrWhiteSpace(anchorToTargetSql))
        {
            sql.AppendLine($"  join target_route on target_route.target_id = {targetTableAliasOrName}.{targetJoinColumn}");
            sql.AppendLine($"  join composed_filter on composed_filter.{anchorKeyColumn} = target_route.source_id");
        }
        else
        {
            sql.AppendLine($"  join composed_filter on composed_filter.{anchorKeyColumn} = {targetTableAliasOrName}.{targetJoinColumn}");
        }
        AppendWhereClauses(sql, targetCriteria, "  ");
        sql.AppendLine("  group by category");
        sql.AppendLine(") as r");
        sql.AppendLine("  on r.category = c.category");
        sql.Append("order by c.lower");
        return sql.ToString();
    }

    private static string BuildGeoPolygonSql(
        string composedFilterSql,
        string categoryInfoSql,
        string targetJoinColumn,
        string anchorKeyColumn,
        string anchorToTargetSql
    )
    {
        if (string.IsNullOrWhiteSpace(categoryInfoSql))
        {
            throw new InvalidOperationException("Geo-polygon target facets require a category-info SQL definition.");
        }

        var sql = new StringBuilder();
        sql.AppendLine("with composed_filter as (");
        sql.AppendLine(Indent(composedFilterSql.Trim(), "  "));
        sql.AppendLine("),");
        if (!string.IsNullOrWhiteSpace(anchorToTargetSql))
        {
            sql.AppendLine("target_route as (");
            sql.AppendLine(Indent(anchorToTargetSql.Trim(), "  "));
            sql.AppendLine("),");
        }
        sql.AppendLine("categories(category, count_column, longitude_dd, latitude_dd) as (");
        sql.AppendLine(Indent(categoryInfoSql.Trim(), "  "));
        sql.AppendLine(")");
        sql.AppendLine("select c.category, c.count_column, c.longitude_dd, c.latitude_dd");
        sql.AppendLine("from categories c");
        if (!string.IsNullOrWhiteSpace(anchorToTargetSql))
        {
            sql.AppendLine("join target_route on target_route.target_id = c.category");
            sql.AppendLine($"join composed_filter on composed_filter.{anchorKeyColumn} = target_route.source_id");
        }
        else
        {
            sql.AppendLine($"join composed_filter on composed_filter.{anchorKeyColumn} = c.category");
        }
        sql.Append("order by c.category");
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

    private static IReadOnlyList<string> ResolveTargetFacetCriteria(Facet targetFacet, string targetTableAliasOrName)
    {
        var clauses = targetFacet?.Clauses?.Where(clause => clause?.EnforceConstraint ?? false).ToList();
        if (clauses is null || clauses.Count == 0)
        {
            return [];
        }

        var resolvedClauses = new List<string>(clauses.Count);
        foreach (var clause in clauses)
        {
            var resolvedClause = clause.Clause?.Trim();
            if (string.IsNullOrWhiteSpace(resolvedClause))
            {
                continue;
            }

            var allowedQualifiers = new[]
            {
                targetFacet.TargetTable?.Alias,
                targetFacet.TargetTable?.ResolvedAliasOrTableOrUdfName,
                targetFacet.TargetTable?.TableOrUdfName,
            }
                .Where(value => !string.IsNullOrWhiteSpace(value))
                .Distinct(StringComparer.OrdinalIgnoreCase);

            foreach (var qualifier in allowedQualifiers)
            {
                resolvedClause = resolvedClause.Replace($"{qualifier}.", $"{targetTableAliasOrName}.", StringComparison.OrdinalIgnoreCase);
            }

            resolvedClauses.Add(resolvedClause);
        }

        return resolvedClauses;
    }

    private static void AppendWhereClauses(StringBuilder sql, IReadOnlyList<string> clauses, string indent)
    {
        if (clauses is null || clauses.Count == 0)
        {
            return;
        }

        sql.AppendLine($"{indent}where {clauses[0]}");
        for (var index = 1; index < clauses.Count; index++)
        {
            sql.AppendLine($"{indent}  and {clauses[index]}");
        }
    }

    private static string Indent(string text, string indent)
    {
        var lines = text.Replace("\r\n", "\n", StringComparison.Ordinal).Split('\n');
        return string.Join(Environment.NewLine, lines.Select(line => $"{indent}{line}"));
    }
}
