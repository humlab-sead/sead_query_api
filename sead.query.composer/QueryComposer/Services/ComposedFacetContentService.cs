using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using SeadQueryComposer.QueryComposer.Inputs;
using SeadQueryComposer.RouteCompiler;
using SeadQueryCore;
using SeadQueryCore.Plugin.Range;
using SeadQueryCore.QueryComposer;

namespace SeadQueryComposer.QueryComposer.Services;

/// <summary>
/// Executes the first end-to-end composed facet-content path for discrete facets.
/// Unsupported requests must fall back to the legacy runtime.
/// </summary>
public sealed class ComposedFacetContentService : IComposedFacetContentService
{
    private readonly IRepositoryRegistry _registry;
    private readonly ITypedQueryProxy _queryProxy;
    private readonly IPathFinder _pathFinder;
    private readonly IRouteSqlCompiler _routeSqlCompiler;
    private readonly IDiscreteFacetPredicateResolver _predicateResolver;
    private readonly IComposedFilterQueryComposer _composedFilterQueryComposer;
    private readonly IFacetContentQueryComposer _facetContentQueryComposer;
    private readonly IRangeCategoryInfoService _rangeCategoryInfoService;

    public ComposedFacetContentService(
        IRepositoryRegistry registry,
        ITypedQueryProxy queryProxy,
        IPathFinder pathFinder,
        IRouteSqlCompiler routeSqlCompiler,
        IDiscreteFacetPredicateResolver predicateResolver,
        IComposedFilterQueryComposer composedFilterQueryComposer,
        IFacetContentQueryComposer facetContentQueryComposer,
        IRangeCategoryInfoService rangeCategoryInfoService
    )
    {
        _registry = registry ?? throw new ArgumentNullException(nameof(registry));
        _queryProxy = queryProxy ?? throw new ArgumentNullException(nameof(queryProxy));
        _pathFinder = pathFinder ?? throw new ArgumentNullException(nameof(pathFinder));
        _routeSqlCompiler = routeSqlCompiler ?? throw new ArgumentNullException(nameof(routeSqlCompiler));
        _predicateResolver = predicateResolver ?? throw new ArgumentNullException(nameof(predicateResolver));
        _composedFilterQueryComposer = composedFilterQueryComposer ?? throw new ArgumentNullException(nameof(composedFilterQueryComposer));
        _facetContentQueryComposer = facetContentQueryComposer ?? throw new ArgumentNullException(nameof(facetContentQueryComposer));
        _rangeCategoryInfoService = rangeCategoryInfoService ?? throw new ArgumentNullException(nameof(rangeCategoryInfoService));
    }

    public bool CanHandle(FacetsConfig2 facetsConfig)
    {
        return TryCreateRequest(facetsConfig, out _);
    }

    public FacetContent Load(FacetsConfig2 facetsConfig)
    {
        ArgumentNullException.ThrowIfNull(facetsConfig);

        if (!TryCreateRequest(facetsConfig, out var request))
        {
            throw new InvalidOperationException("The composed facet-content service cannot handle this request.");
        }

        var predicatePlans = request.PredicateConfigs.Select(config => CreatePredicateQueryPlan(config, request)).ToList();
        var composedFilterQuery = _composedFilterQueryComposer.Compose(
            predicatePlans,
            request.AnchorTable,
            QueryComposerAliases.AnchorKeyColumn
        );
        var categoryInfo =
            facetsConfig.TargetFacet.FacetTypeId == EFacetType.Range
                ? _rangeCategoryInfoService.GetCategoryInfo(facetsConfig, facetsConfig.TargetCode)
                : null;
        var contentQueryPlan = _facetContentQueryComposer.Compose(
            facetsConfig,
            composedFilterQuery,
            request.TargetJoinColumn,
            request.AnchorToTargetSql,
            categoryInfo?.Query
        );
        var userPicks = facetsConfig.CollectUserPicks(facetsConfig.TargetCode);

        if (facetsConfig.TargetFacet.FacetTypeId == EFacetType.Range)
        {
            var categoryCounts = _queryProxy
                .QueryRows(contentQueryPlan.Sql, ToRangeCategoryItem)
                .ToDictionary(item => item.Category ?? "(null)");
            var outerCategoryCounts = _queryProxy.QueryRows(categoryInfo.Query, _rangeCategoryInfoService.SqlCompiler.ToItem).ToList();

            foreach (var item in outerCategoryCounts)
            {
                if (categoryCounts.TryGetValue(item.Category ?? "(null)", out var countedItem))
                {
                    item.Count = countedItem.Count;
                }
            }

            return new FacetContent
            {
                FacetsConfig = facetsConfig,
                Items = outerCategoryCounts.Where(item => item.Count != null).ToList(),
                Distribution = categoryCounts,
                IntervalInfo = categoryInfo,
                SqlQuery = contentQueryPlan.Sql,
                Picks = userPicks ?? [],
            };
        }

        var categoryItems = _queryProxy.QueryRows(contentQueryPlan.Sql, ToCategoryItem);

        return new FacetContent
        {
            FacetsConfig = facetsConfig,
            Items = categoryItems.Where(item => item.Count != null).ToList(),
            Distribution = categoryItems.ToDictionary(item => item.Category ?? "(null)"),
            IntervalInfo = new FacetContent.CategoryInfo { Count = categoryItems.Count, Query = contentQueryPlan.Sql },
            SqlQuery = contentQueryPlan.Sql,
            Picks = userPicks ?? [],
        };
    }

    private PredicateQueryPlan CreatePredicateQueryPlan(FacetConfig2 config, ComposedFacetContentRequest request)
    {
        var sourceFacet = config.Facet;
        var sourceTable = sourceFacet.TargetTable;
        var sourceTableName = sourceTable.TableOrUdfName;
        var sourceKeyColumn = ResolvePredicateSourceKeyColumn(sourceFacet);
        var sourceCriteria = ResolvePredicateCriteria(sourceFacet);
        var isIdentityRoute = string.Equals(sourceTableName, request.AnchorTable, StringComparison.OrdinalIgnoreCase);
        var route = isIdentityRoute ? [] : _pathFinder.Find(sourceTableName, request.AnchorTable).ToTrail().Skip(1).SkipLast(1).ToList();

        var predicateSql = _predicateResolver.ResolveSql(
            sourceTableName,
            sourceKeyColumn,
            new DiscreteFacetUserInput { Picks = config.GetPickValues().Cast<object>().ToList() },
            new AnchorTemplate { Route = route, RequiresDistinct = true },
            request.AnchorTable,
            request.AnchorKeyColumnName,
            sourceCriteria
        );

        return new PredicateQueryPlan
        {
            FacetCode = config.FacetCode,
            AnchorTable = request.AnchorTable,
            SourceKeyColumn = QueryComposerAliases.SourceKeyColumn,
            AnchorKeyColumn = QueryComposerAliases.AnchorKeyColumn,
            Sql = predicateSql,
        };
    }

    private bool TryCreateRequest(FacetsConfig2 facetsConfig, out ComposedFacetContentRequest request)
    {
        request = null;

        if (facetsConfig?.TargetFacet?.FacetTypeId is not EFacetType.Discrete and not EFacetType.Range)
        {
            return false;
        }

        var targetFacet = facetsConfig.TargetFacet;
        var targetTable = targetFacet.TargetTable;
        var aggregateFacet = _registry.Facets.Get(targetFacet.AggregateFacetId) ?? targetFacet;
        var anchorFacetTable = aggregateFacet.TargetTable;
        var anchorTable = anchorFacetTable?.TableOrUdfName;
        if (string.IsNullOrWhiteSpace(anchorTable))
        {
            return false;
        }

        var anchorKeyColumn = ResolveAnchorKeyColumn(aggregateFacet);
        if (string.IsNullOrWhiteSpace(anchorKeyColumn))
        {
            return false;
        }

        var targetTableName = targetTable?.TableOrUdfName;
        var targetJoinColumn = ResolveTargetJoinColumn(targetFacet, anchorKeyColumn);
        if (string.IsNullOrWhiteSpace(targetTableName) || string.IsNullOrWhiteSpace(targetJoinColumn))
        {
            return false;
        }

        var affectedConfigs = facetsConfig.GetConfigsThatAffectsTarget(facetsConfig.TargetCode, facetsConfig.GetFacetCodes());
        if (affectedConfigs.Count == 0)
        {
            return false;
        }

        var predicateConfigs = affectedConfigs
            .Where(config => !string.Equals(config.FacetCode, facetsConfig.TargetCode, StringComparison.OrdinalIgnoreCase))
            .ToList();

        if (predicateConfigs.Count == 0)
        {
            return false;
        }

        if (predicateConfigs.Any(config => !config.HasPicks()))
        {
            return false;
        }

        foreach (var config in predicateConfigs)
        {
            if (!CanComposePredicate(config, anchorTable))
            {
                return false;
            }
        }

        var anchorToTargetSql = string.Empty;
        if (!string.Equals(anchorTable, targetTableName, StringComparison.OrdinalIgnoreCase))
        {
            try
            {
                anchorToTargetSql = CreateAnchorToTargetSql(anchorTable, anchorKeyColumn, targetTableName, targetJoinColumn);
            }
            catch (Exception)
            {
                return false;
            }
        }

        request = new ComposedFacetContentRequest(anchorTable, anchorKeyColumn, targetJoinColumn, anchorToTargetSql, predicateConfigs);
        return true;
    }

    private bool CanComposePredicate(FacetConfig2 config, string anchorTable)
    {
        var facet = config.Facet;
        var sourceTable = facet?.TargetTable;
        var sourceTableName = sourceTable?.TableOrUdfName;

        if (facet?.FacetTypeId != EFacetType.Discrete)
        {
            return false;
        }

        if (
            string.IsNullOrWhiteSpace(sourceTableName)
            || !TryResolvePredicateSourceKeyColumn(facet, out _)
            || !TryResolvePredicateCriteria(facet, out _)
        )
        {
            return false;
        }

        try
        {
            _pathFinder.Find(sourceTableName, anchorTable);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    private static bool HasSimpleColumnExpression(string categoryExpression)
    {
        if (string.IsNullOrWhiteSpace(categoryExpression))
        {
            return false;
        }

        return categoryExpression.Trim().IndexOfAny([' ', '(', ')']) < 0;
    }

    private static string GetSimpleColumnName(string categoryExpression)
    {
        return categoryExpression.Trim().Split('.').Last();
    }

    private static CategoryItem ToCategoryItem(IDataReader reader)
    {
        return new CategoryItem
        {
            Category = reader.Category2String(0),
            Count = reader.GetInt32(1),
            Extent = [reader.GetInt32(1)],
            Name = reader.Category2String(0),
        };
    }

    private static CategoryItem ToRangeCategoryItem(IDataReader reader)
    {
        return new CategoryItem
        {
            Category = reader.IsDBNull(0) ? "(null)" : reader.GetString(0),
            Count = reader.IsDBNull(3) ? 0 : reader.GetInt32(3),
            Extent = [reader.IsDBNull(1) ? 0 : reader.GetDecimal(1), reader.IsDBNull(2) ? 0 : reader.GetDecimal(2)],
            Name = reader.IsDBNull(0) ? "(null)" : reader.GetString(0),
        };
    }

    private string ResolveAnchorKeyColumn(Facet anchorFacet)
    {
        return TryResolveSimpleColumnOnTable(anchorFacet.CategoryIdExpr, anchorFacet.TargetTable, out var anchorKeyColumn)
            ? anchorKeyColumn
            : string.Empty;
    }

    private string ResolveTargetJoinColumn(Facet targetFacet, string anchorKeyColumn)
    {
        if (targetFacet?.FacetTypeId == EFacetType.Range)
        {
            var targetPrimaryKey = targetFacet.TargetTable?.Table?.PrimaryKeyName ?? string.Empty;
            if (!IsPlaceholderPrimaryKey(targetPrimaryKey))
            {
                return targetPrimaryKey;
            }

            return anchorKeyColumn;
        }

        if (TryResolveSimpleColumnOnTable(targetFacet.CategoryIdExpr, targetFacet.TargetTable, out var targetJoinColumn))
        {
            return targetJoinColumn;
        }

        return targetFacet.TargetTable?.Table?.PrimaryKeyName ?? string.Empty;
    }

    private static bool IsPlaceholderPrimaryKey(string primaryKeyName)
    {
        return string.IsNullOrWhiteSpace(primaryKeyName)
            || string.Equals(primaryKeyName, "xxx", StringComparison.OrdinalIgnoreCase)
            || string.Equals(primaryKeyName, "xxxx", StringComparison.OrdinalIgnoreCase);
    }

    private string ResolvePredicateSourceKeyColumn(Facet sourceFacet)
    {
        if (TryResolvePredicateSourceKeyColumn(sourceFacet, out var sourceKeyColumn))
        {
            return sourceKeyColumn;
        }

        throw new InvalidOperationException(
            $"Facet '{sourceFacet.FacetCode}' does not expose a simple source key column on its target table."
        );
    }

    private IReadOnlyList<string> ResolvePredicateCriteria(Facet sourceFacet)
    {
        if (TryResolvePredicateCriteria(sourceFacet, out var sourceCriteria))
        {
            return sourceCriteria;
        }

        throw new InvalidOperationException(
            $"Facet '{sourceFacet.FacetCode}' uses facet clauses that the composed predicate path cannot apply."
        );
    }

    private static bool TryResolvePredicateSourceKeyColumn(Facet sourceFacet, out string sourceKeyColumn)
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

    private static bool TryResolvePredicateCriteria(Facet sourceFacet, out IReadOnlyList<string> sourceCriteria)
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

    private static bool TryResolveSimpleColumnOnTable(string categoryExpression, FacetTable facetTable, out string columnName)
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

    private string CreateAnchorToTargetSql(string anchorTable, string anchorKeyColumn, string targetTable, string targetJoinColumn)
    {
        var routeTables = _pathFinder.Find(anchorTable, targetTable).ToTrail();
        return _routeSqlCompiler.Compile(routeTables, anchorKeyColumn, targetJoinColumn);
    }

    private sealed record ComposedFacetContentRequest(
        string AnchorTable,
        string AnchorKeyColumnName,
        string TargetJoinColumn,
        string AnchorToTargetSql,
        IReadOnlyList<FacetConfig2> PredicateConfigs
    );
}
