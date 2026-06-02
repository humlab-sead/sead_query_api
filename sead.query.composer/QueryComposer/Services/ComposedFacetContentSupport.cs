using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using SeadQueryCore;

namespace SeadQueryComposer.QueryComposer.Services;

/// <summary>
/// Provides shared helper methods for resolving join columns, predicate keys, predicate criteria,
/// and category-item mappings used by composed facet content handlers and factories.
/// </summary>
internal static class ComposedFacetContentSupport
{
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
