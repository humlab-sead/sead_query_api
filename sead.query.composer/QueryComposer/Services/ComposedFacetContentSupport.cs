using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using SeadQueryComposer.QueryComposer.Inputs;
using SeadQueryComposer.RouteCompiler;
using SeadQueryCore;

namespace SeadQueryComposer.QueryComposer.Services;

/// <summary>
/// Provides shared helper methods for resolving join columns, predicate keys, predicate criteria,
/// and category-item mappings used by composed facet content handlers and factories.
/// </summary>
internal static class ComposedFacetContentSupport
{
    public static bool TryCreateDiscreteTemplateSql(
        Facet sourceFacet,
        FacetTemplateRuntimeSnapshot templateSnapshot,
        string requestedAnchorTable,
        string requestedAnchorKeyColumn,
        DiscreteFacetUserInput userInput,
        IPathFinder pathFinder,
        IRouteSqlCompiler routeSqlCompiler,
        out string sql
    )
    {
        sql = string.Empty;

        if (sourceFacet is null || templateSnapshot is null || string.IsNullOrWhiteSpace(requestedAnchorTable))
        {
            return false;
        }

        if (!string.IsNullOrWhiteSpace(templateSnapshot.TemplateContract)
            && !string.Equals(templateSnapshot.TemplateContract, "discrete", StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException(
                $"Facet '{sourceFacet.FacetCode}' uses unsupported template contract '{templateSnapshot.TemplateContract}' for discrete composition."
            );
        }

        if (templateSnapshot.AnchorSqlByTable.TryGetValue(requestedAnchorTable, out var anchorOverrideSql)
            && !string.IsNullOrWhiteSpace(anchorOverrideSql))
        {
            sql = RenderDiscreteTemplateSql(anchorOverrideSql, userInput);
            return true;
        }

        if (string.IsNullOrWhiteSpace(templateSnapshot.BaseSql) || string.IsNullOrWhiteSpace(templateSnapshot.BaseAnchor))
        {
            return false;
        }

        if (!TryResolveAnchorBindingByName(sourceFacet, templateSnapshot.BaseAnchor, out var baseAnchorBinding))
        {
            return false;
        }

        var baseAnchorTable = baseAnchorBinding.Anchor?.Table?.TableOrUdfName;
        var baseAnchorKeyColumn = baseAnchorBinding.Anchor?.Table?.PrimaryKeyName;
        if (string.IsNullOrWhiteSpace(baseAnchorTable) || string.IsNullOrWhiteSpace(baseAnchorKeyColumn))
        {
            return false;
        }

        var renderedBaseSql = RenderDiscreteTemplateSql(templateSnapshot.BaseSql, userInput);
        var normalizedBaseSql = NormalizeBaseTemplateSql(renderedBaseSql);

        if (string.Equals(baseAnchorTable, requestedAnchorTable, StringComparison.OrdinalIgnoreCase))
        {
            sql = normalizedBaseSql;
            return true;
        }

        var routeTables = ResolveProjectionRouteTables(sourceFacet, baseAnchorTable, requestedAnchorTable, pathFinder);
        if (routeTables.Count < 2)
        {
            return false;
        }

        var routeSql = routeSqlCompiler.Compile(routeTables, baseAnchorKeyColumn, requestedAnchorKeyColumn);
        sql =
            $"select distinct base_template.source_id as source_id, projected.target_id as target_id{Environment.NewLine}"
            + $"from ({Environment.NewLine}{Indent(normalizedBaseSql, "  ")}{Environment.NewLine}) as base_template{Environment.NewLine}"
            + $"join ({Environment.NewLine}{Indent(routeSql, "  ")}{Environment.NewLine}) as projected on projected.source_id = base_template.target_id";
        return true;
    }

    public static string ResolveSimpleTargetJoinColumn(Facet targetFacet)
    {
        if (TryResolveSimpleColumnOnTable(targetFacet.CategoryIdExpr, targetFacet.TargetTable, out var targetJoinColumn))
        {
            return targetJoinColumn;
        }

        var targetPrimaryKeyName = targetFacet.TargetTable?.Table?.PrimaryKeyName ?? string.Empty;
        return IsPlaceholderPrimaryKey(targetPrimaryKeyName) ? string.Empty : targetPrimaryKeyName;
    }

    public static string ResolveIntervalTargetJoinColumn(Facet targetFacet, string anchorKeyColumn)
    {
        var targetPrimaryKey = targetFacet.TargetTable?.Table?.PrimaryKeyName ?? string.Empty;
        if (!IsPlaceholderPrimaryKey(targetPrimaryKey))
        {
            return targetPrimaryKey;
        }

        return anchorKeyColumn;
    }

    public static string ResolvePredicateSourceKeyColumn(Facet sourceFacet)
    {
        if (TryResolvePredicateSourceKeyColumn(sourceFacet, out var sourceKeyColumn))
        {
            return sourceKeyColumn;
        }

        throw new InvalidOperationException(
            $"Facet '{sourceFacet.FacetCode}' does not expose a simple source key column on its target table."
        );
    }

    public static IReadOnlyList<string> ResolvePredicateCriteria(Facet sourceFacet)
    {
        if (TryResolvePredicateCriteria(sourceFacet, out var sourceCriteria))
        {
            return sourceCriteria;
        }

        throw new InvalidOperationException(
            $"Facet '{sourceFacet.FacetCode}' uses facet clauses that the composed predicate path cannot apply."
        );
    }

    public static bool TryResolvePredicateSourceKeyColumn(Facet sourceFacet, out string sourceKeyColumn)
    {
        sourceKeyColumn = string.Empty;

        var sourceTable = sourceFacet?.TargetTable;
        var sourcePrimaryKey = sourceTable?.Table?.PrimaryKeyName;
        if (sourceTable is null || string.IsNullOrWhiteSpace(sourcePrimaryKey) || !HasSimpleColumnExpression(sourceFacet.CategoryIdExpr))
        {
            return false;
        }

        var expressionSegments = sourceFacet.CategoryIdExpr.Trim().Split('.');
        if (expressionSegments.Length == 1)
        {
            if (!string.Equals(expressionSegments[0], sourcePrimaryKey, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            sourceKeyColumn = expressionSegments[0];
            return true;
        }

        if (TryResolveSimpleColumnOnTable(sourceFacet.CategoryIdExpr, sourceTable, out sourceKeyColumn))
        {
            return string.Equals(sourceKeyColumn, sourcePrimaryKey, StringComparison.OrdinalIgnoreCase)
                || string.Equals(sourcePrimaryKey, "xxxx", StringComparison.OrdinalIgnoreCase);
        }

        return false;
    }

    public static bool TryResolvePredicateCriteria(Facet sourceFacet, out IReadOnlyList<string> sourceCriteria)
    {
        sourceCriteria = [];

        var sourceTable = sourceFacet?.TargetTable;
        var clauses = sourceFacet?.Clauses;
        if (sourceTable is null || clauses is null || clauses.Count == 0)
        {
            return true;
        }

        var normalizedClauses = new List<string>(clauses.Count);
        foreach (var clause in clauses)
        {
            if (!TryNormalizePredicateClause(sourceTable, clause?.Clause, out var normalizedClause))
            {
                sourceCriteria = [];
                return false;
            }

            normalizedClauses.Add(normalizedClause);
        }

        sourceCriteria = normalizedClauses;
        return true;
    }

    public static bool TryResolveSimpleColumnOnTable(string categoryExpression, FacetTable facetTable, out string columnName)
    {
        columnName = string.Empty;

        if (facetTable is null || !HasSimpleColumnExpression(categoryExpression))
        {
            return false;
        }

        var expressionSegments = categoryExpression.Trim().Split('.');
        if (expressionSegments.Length == 1)
        {
            columnName = expressionSegments[0];
            return true;
        }

        if (expressionSegments.Length >= 2)
        {
            var qualifier = string.Join('.', expressionSegments[..^1]);
            if (
                string.Equals(qualifier, facetTable.ResolvedAliasOrTableOrUdfName, StringComparison.OrdinalIgnoreCase)
                || string.Equals(qualifier, facetTable.TableOrUdfName, StringComparison.OrdinalIgnoreCase)
            )
            {
                columnName = expressionSegments[^1];
                return true;
            }
        }

        return false;
    }

    public static bool IsPlaceholderPrimaryKey(string primaryKeyName)
    {
        return string.IsNullOrWhiteSpace(primaryKeyName)
            || string.Equals(primaryKeyName, "xxx", StringComparison.OrdinalIgnoreCase)
            || string.Equals(primaryKeyName, "xxxx", StringComparison.OrdinalIgnoreCase);
    }

    public static string CreateUnfilteredAnchorSql(string anchorTable, string anchorKeyColumnName, string anchorKeyAlias)
    {
        var selectedAnchorKey = string.Equals(anchorKeyColumnName, anchorKeyAlias, StringComparison.Ordinal)
            ? anchorKeyColumnName
            : $"{anchorKeyColumnName} as {anchorKeyAlias}";

        return $"select distinct {selectedAnchorKey}{Environment.NewLine}from {anchorTable}";
    }

    public static CategoryItem ToCategoryItem(IDataReader reader)
    {
        return new CategoryItem
        {
            Category = reader.Category2String(0),
            Count = reader.GetInt32(1),
            Extent = [reader.GetInt32(1)],
            Name = reader.Category2String(0),
        };
    }

    public static CategoryItem ToRangeCategoryItem(IDataReader reader)
    {
        return new CategoryItem
        {
            Category = reader.IsDBNull(0) ? "(null)" : reader.GetString(0),
            Count = reader.IsDBNull(3) ? 0 : reader.GetInt32(3),
            Extent = [reader.IsDBNull(1) ? 0 : reader.GetDecimal(1), reader.IsDBNull(2) ? 0 : reader.GetDecimal(2)],
            Name = reader.IsDBNull(0) ? "(null)" : reader.GetString(0),
        };
    }

    public static CategoryItem ToGeoPolygonCategoryItem(IDataReader reader)
    {
        return new CategoryItem
        {
            Category = reader.Category2String(0),
            Count = reader.GetInt32(1),
            Extent = [reader.GetDecimal(2), reader.GetDecimal(3)],
            Name = reader.Category2String(0),
        };
    }

    private static bool HasSimpleColumnExpression(string categoryExpression)
    {
        return !string.IsNullOrWhiteSpace(categoryExpression) && categoryExpression.Trim().IndexOfAny([' ', '(', ')']) < 0;
    }

    private static bool TryResolveAnchorBindingByName(Facet sourceFacet, string anchorName, out FacetAnchor binding)
    {
        binding = sourceFacet.FacetAnchors?.FirstOrDefault(facetAnchor =>
            string.Equals(facetAnchor.Anchor?.Name, anchorName, StringComparison.OrdinalIgnoreCase)
        );

        return binding is not null;
    }

    private static bool TryResolveAnchorBindingByTable(Facet sourceFacet, string anchorTable, out FacetAnchor binding)
    {
        binding = sourceFacet.FacetAnchors?.FirstOrDefault(facetAnchor =>
            string.Equals(facetAnchor.Anchor?.Table?.TableOrUdfName, anchorTable, StringComparison.OrdinalIgnoreCase)
        );

        return binding is not null;
    }

    private static IReadOnlyList<string> ResolveProjectionRouteTables(
        Facet sourceFacet,
        string baseAnchorTable,
        string requestedAnchorTable,
        IPathFinder pathFinder
    )
    {
        if (TryResolveAnchorBindingByTable(sourceFacet, requestedAnchorTable, out var requestedAnchorBinding))
        {
            var routeSpecification = requestedAnchorBinding.Route?.Specification ?? string.Empty;
            var parsedRoute = ParseRouteSpecification(routeSpecification);

            if (parsedRoute.Count >= 2
                && string.Equals(parsedRoute[0], baseAnchorTable, StringComparison.OrdinalIgnoreCase)
                && string.Equals(parsedRoute[^1], requestedAnchorTable, StringComparison.OrdinalIgnoreCase))
            {
                return parsedRoute;
            }
        }

        return pathFinder.Find(baseAnchorTable, requestedAnchorTable).ToTrail();
    }

    private static IReadOnlyList<string> ParseRouteSpecification(string specification)
    {
        if (string.IsNullOrWhiteSpace(specification))
        {
            return [];
        }

        return specification
            .Split("->", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .Select(step => step.Trim())
            .Where(step => step.Length > 0)
            .ToList();
    }

    private static string RenderDiscreteTemplateSql(string templateSql, DiscreteFacetUserInput userInput)
    {
        if (string.IsNullOrWhiteSpace(templateSql))
        {
            return string.Empty;
        }

        userInput ??= new DiscreteFacetUserInput();

        var pickValuesSql = userInput.HasPicks
            ? string.Join(", ", userInput.Picks.Select(FormatLiteral))
            : "null";

        var pickFilterSql = BuildPickFilterSql(userInput, pickValuesSql);

        var renderedSql = templateSql
            .Replace("{pick_values_sql}", pickValuesSql, StringComparison.OrdinalIgnoreCase)
            .Replace("{pick_filter_sql}", pickFilterSql, StringComparison.OrdinalIgnoreCase);

        if (ContainsUnresolvedPickPlaceholder(renderedSql))
        {
            throw new InvalidOperationException(
                "Template SQL contains unresolved placeholder tokens. Supported placeholders are {pick_filter_sql} and {pick_values_sql}."
            );
        }

        return renderedSql;
    }

    private static bool ContainsUnresolvedPickPlaceholder(string sql)
    {
        return !string.IsNullOrWhiteSpace(sql)
            && sql.IndexOf("{pick_", StringComparison.OrdinalIgnoreCase) >= 0;
    }

    private static string NormalizeBaseTemplateSql(string renderedBaseSql)
    {
        return
            $"select base.category_id as source_id, base.anchor_id as target_id{Environment.NewLine}"
            + $"from ({Environment.NewLine}{Indent(renderedBaseSql, "  ")}{Environment.NewLine}) as base";
    }

    private static string BuildPickFilterSql(DiscreteFacetUserInput userInput, string pickValuesSql)
    {
        if (userInput is null || !userInput.HasPicks)
        {
            return "1=1";
        }

        var normalizedOperator = (userInput.Operator ?? string.Empty).Trim().ToLowerInvariant();

        if (string.IsNullOrWhiteSpace(normalizedOperator))
        {
            throw new ArgumentException("Discrete facet operators must be non-empty.", nameof(userInput));
        }

        if (normalizedOperator is "in" or "not in")
        {
            return $"category_id {normalizedOperator} ({pickValuesSql})";
        }

        if (userInput.Picks.Count != 1)
        {
            throw new ArgumentException("Non-set operators require exactly one selected value.", nameof(userInput));
        }

        return $"category_id {userInput.Operator} {FormatLiteral(userInput.Picks[0])}";
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

    private static string Indent(string text, string indent)
    {
        var lines = text.Replace("\r\n", "\n", StringComparison.Ordinal).Split('\n');
        return string.Join(Environment.NewLine, lines.Select(line => $"{indent}{line}"));
    }

    private static bool TryNormalizePredicateClause(FacetTable sourceTable, string clause, out string normalizedClause)
    {
        normalizedClause = string.Empty;

        if (sourceTable is null || string.IsNullOrWhiteSpace(clause))
        {
            return false;
        }

        normalizedClause = clause.Trim();

        var allowedQualifiers = new[] { sourceTable.Alias, sourceTable.ResolvedAliasOrTableOrUdfName, sourceTable.TableOrUdfName }
            .Where(value => !string.IsNullOrWhiteSpace(value))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        foreach (var qualifier in allowedQualifiers)
        {
            normalizedClause = normalizedClause.Replace($"{qualifier}.", "X_0.", StringComparison.OrdinalIgnoreCase);
        }

        return normalizedClause.Contains("X_0.", StringComparison.OrdinalIgnoreCase)
            && !normalizedClause.Contains("tbl_", StringComparison.OrdinalIgnoreCase)
            && !normalizedClause.Contains("countries.", StringComparison.OrdinalIgnoreCase)
            && !normalizedClause.Contains("facet.", StringComparison.OrdinalIgnoreCase);
    }
}
