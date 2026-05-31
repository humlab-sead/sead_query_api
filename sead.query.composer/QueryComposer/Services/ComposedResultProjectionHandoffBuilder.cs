using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Logging;
using SeadQueryComposer.QueryComposer.Inputs;
using SeadQueryComposer.RouteCompiler;
using SeadQueryCore;
using SeadQueryCore.Model;
using SeadQueryCore.Model.Ext;
using SeadQueryCore.QueryBuilder;
using SeadQueryCore.QueryComposer;
using SeadQueryCore.Services.Result;

namespace SeadQueryComposer.QueryComposer.Services;

public sealed class ComposedResultProjectionHandoffBuilder : IResultProjectionHandoffBuilder
{
    private readonly IRepositoryRegistry _registry;
    private readonly ISupportedRequestQuerySetupFactory _querySetupFactory;
    private readonly IPickFilterCompilerLocator _pickFilterCompilerLocator;
    private readonly IPathFinder _pathFinder;
    private readonly IRouteSqlCompiler _routeSqlCompiler;
    private readonly IDiscreteFacetPredicateResolver _predicateResolver;
    private readonly IComposedFilterQueryComposer _composedFilterQueryComposer;
    private readonly ILogger<ComposedResultProjectionHandoffBuilder> _logger;

    public ComposedResultProjectionHandoffBuilder(
        IRepositoryRegistry registry,
        ISupportedRequestQuerySetupFactory querySetupFactory,
        IPickFilterCompilerLocator pickFilterCompilerLocator,
        IPathFinder pathFinder,
        IRouteSqlCompiler routeSqlCompiler,
        IDiscreteFacetPredicateResolver predicateResolver,
        IComposedFilterQueryComposer composedFilterQueryComposer,
        ILogger<ComposedResultProjectionHandoffBuilder> logger
    )
    {
        _registry = registry ?? throw new ArgumentNullException(nameof(registry));
        _querySetupFactory = querySetupFactory ?? throw new ArgumentNullException(nameof(querySetupFactory));
        _pickFilterCompilerLocator = pickFilterCompilerLocator ?? throw new ArgumentNullException(nameof(pickFilterCompilerLocator));
        _pathFinder = pathFinder ?? throw new ArgumentNullException(nameof(pathFinder));
        _routeSqlCompiler = routeSqlCompiler ?? throw new ArgumentNullException(nameof(routeSqlCompiler));
        _predicateResolver = predicateResolver ?? throw new ArgumentNullException(nameof(predicateResolver));
        _composedFilterQueryComposer = composedFilterQueryComposer ?? throw new ArgumentNullException(nameof(composedFilterQueryComposer));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public ResultProjectionHandoff Build(FacetsConfig2 facetsConfig, ResultConfig resultConfig)
    {
        ArgumentNullException.ThrowIfNull(facetsConfig);
        ArgumentNullException.ThrowIfNull(resultConfig);

        if (!TryCreateRequest(facetsConfig, resultConfig, out var request, out var failureReason))
        {
            _logger.LogInformation(
                "Rejecting unsupported composed result projection handoff for result facet '{FacetCode}' and view '{ViewTypeId}': {FailureReason}",
                resultConfig.Facet?.FacetCode ?? resultConfig.FacetCode,
                resultConfig.ViewTypeId ?? string.Empty,
                failureReason
            );

            throw new InvalidOperationException(
                $"The composed result projection handoff cannot handle this request: {failureReason} Unsupported result requests no longer fall back to the legacy runtime."
            );
        }

        var resultFields = resultConfig.GetSortedFields().ToList();
        var projectionQuerySetup = _querySetupFactory.CreateForResultProjection(facetsConfig, resultConfig.Facet, resultFields);
        var composedFilterQuery = CreateComposedFilterQuery(request);

        projectionQuerySetup.LeadingSql = BuildLeadingSql(composedFilterQuery.Sql, request.AnchorToTargetSql);
        projectionQuerySetup.Joins = CreateProjectionJoins(projectionQuerySetup, request).Concat(projectionQuerySetup.Joins ?? []).ToList();

        return new ResultProjectionHandoff(projectionQuerySetup, resultFields);
    }

    private ComposedFilterQuery CreateComposedFilterQuery(ComposedResultProjectionRequest request)
    {
        if (request.PredicateConfigs.Count > 0)
        {
            var predicatePlans = request.PredicateConfigs.Select(config => CreatePredicateQueryPlan(config, request)).ToList();
            return _composedFilterQueryComposer.Compose(predicatePlans, request.AnchorTable, QueryComposerAliases.AnchorKeyColumn);
        }

        return new ComposedFilterQuery
        {
            AnchorTable = request.AnchorTable,
            AnchorKeyColumn = QueryComposerAliases.AnchorKeyColumn,
            PredicateQueries = [],
            Sql = CreateUnfilteredAnchorSql(request.AnchorTable, request.AnchorKeyColumnName, QueryComposerAliases.AnchorKeyColumn),
        };
    }

    private PredicateQueryPlan CreatePredicateQueryPlan(FacetConfig2 config, ComposedResultProjectionRequest request)
    {
        return config.Facet.FacetTypeId == EFacetType.Discrete
            ? CreateDiscretePredicateQueryPlan(config, request)
            : CreateCompiledPredicateQueryPlan(config, request);
    }

    private PredicateQueryPlan CreateDiscretePredicateQueryPlan(FacetConfig2 config, ComposedResultProjectionRequest request)
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
            new AnchorTemplate
            {
                Route = route,
                IsIdentityRoute = isIdentityRoute,
                RequiresDistinct = true,
            },
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

    private PredicateQueryPlan CreateCompiledPredicateQueryPlan(FacetConfig2 config, ComposedResultProjectionRequest request)
    {
        var sourceFacet = config.Facet;
        var sourceTable = sourceFacet.TargetTable;
        var sourceTableName = sourceTable.TableOrUdfName;
        var sourceKeyColumn = ResolvePredicateRouteKeyColumn(sourceFacet);
        var predicateCriteria = ResolveCompiledPredicateCriteria(config);
        var isIdentityRoute = string.Equals(sourceTableName, request.AnchorTable, StringComparison.OrdinalIgnoreCase);
        var route = isIdentityRoute ? [] : _pathFinder.Find(sourceTableName, request.AnchorTable).ToTrail().Skip(1).SkipLast(1).ToList();
        var predicateSql = CreatePredicateSql(
            sourceTableName,
            sourceKeyColumn,
            route,
            request.AnchorTable,
            request.AnchorKeyColumnName,
            predicateCriteria
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

    private bool TryCreateRequest(
        FacetsConfig2 facetsConfig,
        ResultConfig resultConfig,
        out ComposedResultProjectionRequest request,
        out string failureReason
    )
    {
        request = null;
        failureReason = string.Empty;

        var resultFacet = resultConfig?.Facet;
        if (resultFacet is null)
        {
            failureReason = "result configuration does not specify a result facet.";
            return false;
        }

        var targetTable = resultFacet?.TargetTable;
        var aggregateFacet = _registry.Facets.Get(resultFacet?.AggregateFacetId ?? 0) ?? resultFacet;
        var anchorFacetTable = aggregateFacet?.TargetTable;
        var anchorTable = anchorFacetTable?.TableOrUdfName;
        if (string.IsNullOrWhiteSpace(anchorTable))
        {
            failureReason = $"aggregate facet '{aggregateFacet?.FacetCode ?? resultFacet.FacetCode}' does not expose an anchor table.";
            return false;
        }

        var anchorKeyColumn = ResolveAnchorKeyColumn(aggregateFacet);
        if (string.IsNullOrWhiteSpace(anchorKeyColumn))
        {
            failureReason =
                $"aggregate facet '{aggregateFacet?.FacetCode ?? resultFacet.FacetCode}' does not expose a simple anchor key column on its target table.";
            return false;
        }

        var targetTableName = targetTable?.TableOrUdfName;
        var targetJoinColumn = ResolveTargetJoinColumn(resultFacet, anchorKeyColumn);
        if (string.IsNullOrWhiteSpace(targetTableName) || string.IsNullOrWhiteSpace(targetJoinColumn))
        {
            failureReason = $"result facet '{resultFacet.FacetCode}' does not expose a routable target join column on its target table.";
            return false;
        }

        var predicateConfigs = facetsConfig
            .GetFacetConfigsAffectedBy(resultFacet, facetsConfig.GetFacetCodes())
            .Where(config => !string.Equals(config.FacetCode, resultFacet.FacetCode, StringComparison.OrdinalIgnoreCase))
            .Where(config => config.HasPicks() || CanComposeClauseOnlyPredicate(config))
            .ToList();

        if (predicateConfigs.Count > 0)
        {
            foreach (var config in predicateConfigs)
            {
                if (!TryGetPredicateFailureReason(config, anchorTable, out failureReason))
                {
                    return false;
                }
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
                failureReason =
                    $"result facet '{resultFacet.FacetCode}' cannot be routed from anchor table '{anchorTable}' to target table '{targetTableName}'.";
                return false;
            }
        }

        request = new ComposedResultProjectionRequest(anchorTable, anchorKeyColumn, targetJoinColumn, anchorToTargetSql, predicateConfigs);
        failureReason = string.Empty;
        return true;
    }

    private static bool CanComposeClauseOnlyPredicate(FacetConfig2 config)
    {
        return config?.Facet?.FacetTypeId == EFacetType.Discrete && config.HasCriterias();
    }

    private bool CanComposePredicate(FacetConfig2 config, string anchorTable)
    {
        return TryGetPredicateFailureReason(config, anchorTable, out _);
    }

    private bool TryGetPredicateFailureReason(FacetConfig2 config, string anchorTable, out string failureReason)
    {
        failureReason = string.Empty;

        var facet = config.Facet;
        var sourceTable = facet?.TargetTable;
        var sourceTableName = sourceTable?.TableOrUdfName;

        if (
            facet?.FacetTypeId
            is not EFacetType.Discrete
                and not EFacetType.Range
                and not EFacetType.Intersect
                and not EFacetType.GeoPolygon
        )
        {
            failureReason =
                $"predicate facet '{config?.FacetCode ?? facet?.FacetCode ?? "(unknown)"}' uses facet type '{facet?.FacetTypeId}' which the composed result path does not support.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(sourceTableName))
        {
            failureReason = $"predicate facet '{config?.FacetCode ?? facet?.FacetCode ?? "(unknown)"}' does not expose a source table.";
            return false;
        }

        if (facet.FacetTypeId == EFacetType.Discrete)
        {
            if (!TryResolvePredicateSourceKeyColumn(facet, out _))
            {
                failureReason =
                    $"predicate facet '{config?.FacetCode ?? facet?.FacetCode ?? "(unknown)"}' does not expose a simple source key column on its target table.";
                return false;
            }

            if (!TryResolvePredicateCriteria(facet, out _))
            {
                failureReason =
                    $"predicate facet '{config?.FacetCode ?? facet?.FacetCode ?? "(unknown)"}' uses facet clauses that the composed result path cannot apply.";
                return false;
            }
        }
        else if (!TryResolvePredicateRouteKeyColumn(facet, out _))
        {
            failureReason =
                $"predicate facet '{config?.FacetCode ?? facet?.FacetCode ?? "(unknown)"}' does not expose a routable source key column on its target table.";
            return false;
        }
        else if (!TryResolveCompiledPredicateCriteria(config, out _))
        {
            failureReason =
                $"predicate facet '{config?.FacetCode ?? facet?.FacetCode ?? "(unknown)"}' uses pick criteria that the composed result path cannot normalize.";
            return false;
        }

        try
        {
            _pathFinder.Find(sourceTableName, anchorTable);
            return true;
        }
        catch (Exception)
        {
            failureReason =
                $"predicate facet '{config?.FacetCode ?? facet?.FacetCode ?? "(unknown)"}' cannot be routed from source table '{sourceTableName}' to anchor table '{anchorTable}'.";
            return false;
        }
    }

    private List<string> CreateProjectionJoins(QuerySetup querySetup, ComposedResultProjectionRequest request)
    {
        var targetTableName = querySetup.Facet.TargetTable.ResolvedAliasOrTableOrUdfName;
        var joins = new List<string>();

        if (!string.IsNullOrWhiteSpace(request.AnchorToTargetSql))
        {
            joins.Add($" join target_route on target_route.target_id = {targetTableName}.{request.TargetJoinColumn}");
            joins.Add($" join composed_filter on composed_filter.target_id = target_route.source_id");
            return joins;
        }

        joins.Add($" join composed_filter on composed_filter.target_id = {targetTableName}.{request.TargetJoinColumn}");
        return joins;
    }

    private static string BuildLeadingSql(string composedFilterSql, string anchorToTargetSql)
    {
        var sql = new StringBuilder();
        sql.AppendLine("with composed_filter as (");
        sql.AppendLine(Indent(composedFilterSql, "  "));

        if (string.IsNullOrWhiteSpace(anchorToTargetSql))
        {
            sql.AppendLine(")");
            return sql.ToString();
        }

        sql.AppendLine("),");
        sql.AppendLine("target_route as (");
        sql.AppendLine(Indent(anchorToTargetSql, "  "));
        sql.AppendLine(")");
        return sql.ToString();
    }

    private string ResolveAnchorKeyColumn(Facet anchorFacet)
    {
        return TryResolveSimpleColumnOnTable(anchorFacet.CategoryIdExpr, anchorFacet.TargetTable, out var anchorKeyColumn)
            ? anchorKeyColumn
            : string.Empty;
    }

    private string ResolveTargetJoinColumn(Facet targetFacet, string anchorKeyColumn)
    {
        if (targetFacet?.FacetTypeId is EFacetType.Range or EFacetType.Intersect)
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

        var targetPrimaryKeyName = targetFacet.TargetTable?.Table?.PrimaryKeyName ?? string.Empty;
        return IsPlaceholderPrimaryKey(targetPrimaryKeyName) ? string.Empty : targetPrimaryKeyName;
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

    private string ResolvePredicateRouteKeyColumn(Facet sourceFacet)
    {
        if (TryResolvePredicateRouteKeyColumn(sourceFacet, out var sourceKeyColumn))
        {
            return sourceKeyColumn;
        }

        throw new InvalidOperationException(
            $"Facet '{sourceFacet.FacetCode}' does not expose a routable source key column on its target table."
        );
    }

    private IReadOnlyList<string> ResolveCompiledPredicateCriteria(FacetConfig2 config)
    {
        if (TryResolveCompiledPredicateCriteria(config, out var sourceCriteria))
        {
            return sourceCriteria;
        }

        throw new InvalidOperationException(
            $"Facet '{config.FacetCode}' uses pick criteria that the composed result path cannot normalize."
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

    private bool TryResolvePredicateRouteKeyColumn(Facet sourceFacet, out string sourceKeyColumn)
    {
        if (sourceFacet?.FacetTypeId == EFacetType.Discrete)
        {
            return TryResolvePredicateSourceKeyColumn(sourceFacet, out sourceKeyColumn);
        }

        sourceKeyColumn = string.Empty;

        var sourcePrimaryKey = sourceFacet?.TargetTable?.Table?.PrimaryKeyName ?? string.Empty;
        if (sourceFacet?.TargetTable is null || IsPlaceholderPrimaryKey(sourcePrimaryKey))
        {
            return false;
        }

        sourceKeyColumn = sourcePrimaryKey;
        return true;
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

    private bool TryResolveCompiledPredicateCriteria(FacetConfig2 config, out IReadOnlyList<string> sourceCriteria)
    {
        sourceCriteria = [];

        if (config?.Facet is null)
        {
            return false;
        }

        string predicateClause;
        try
        {
            predicateClause = _pickFilterCompilerLocator.Locate(config.Facet.FacetTypeId).Compile(config.Facet, config.Facet, config);
        }
        catch (Exception)
        {
            return false;
        }

        if (!TryNormalizeCompiledPredicateClause(config.Facet, predicateClause, out var normalizedClause))
        {
            return false;
        }

        sourceCriteria = [normalizedClause];
        return true;
    }

    private static bool TryNormalizeCompiledPredicateClause(Facet facet, string clause, out string normalizedClause)
    {
        normalizedClause = string.Empty;

        if (facet?.TargetTable is null || string.IsNullOrWhiteSpace(clause))
        {
            return false;
        }

        var candidateClause = clause.Trim();
        if (
            !candidateClause.Contains("X_0.", StringComparison.OrdinalIgnoreCase)
            && HasSimpleColumnExpression(facet.CategoryIdExpr)
            && !facet.CategoryIdExpr.Contains('.', StringComparison.Ordinal)
        )
        {
            candidateClause = candidateClause.Replace(
                facet.CategoryIdExpr.Trim(),
                $"X_0.{facet.CategoryIdExpr.Trim()}",
                StringComparison.OrdinalIgnoreCase
            );
        }

        return TryNormalizePredicateClause(facet.TargetTable, candidateClause, out normalizedClause);
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

    private string CreatePredicateSql(
        string sourceTableName,
        string sourceKeyColumn,
        IReadOnlyList<string> route,
        string anchorTable,
        string anchorKeyColumnName,
        IReadOnlyList<string> sourceCriteria
    )
    {
        return route.Count > 0
            ? _routeSqlCompiler.Compile([sourceTableName, .. route, anchorTable], sourceKeyColumn, anchorKeyColumnName, sourceCriteria)
            : CreateIdentityPredicateSql(sourceTableName, sourceKeyColumn, anchorKeyColumnName, sourceCriteria);
    }

    private static string CreateIdentityPredicateSql(
        string targetTable,
        string sourceKeyColumn,
        string anchorKeyColumn,
        IReadOnlyList<string> sourceCriteria
    )
    {
        if (sourceCriteria?.Count > 0)
        {
            return $"select distinct X_0.{sourceKeyColumn} as source_id, X_0.{anchorKeyColumn} as target_id{Environment.NewLine}from {targetTable} as X_0{Environment.NewLine}where {string.Join(" and ", sourceCriteria)}";
        }

        return $"select distinct {sourceKeyColumn} as source_id, {anchorKeyColumn} as target_id{Environment.NewLine}from {targetTable}";
    }

    private static bool HasSimpleColumnExpression(string categoryExpression)
    {
        if (string.IsNullOrWhiteSpace(categoryExpression))
        {
            return false;
        }

        return categoryExpression.Trim().IndexOfAny([' ', '(', ')']) < 0;
    }

    private static bool IsPlaceholderPrimaryKey(string primaryKeyName)
    {
        return string.IsNullOrWhiteSpace(primaryKeyName)
            || string.Equals(primaryKeyName, "xxx", StringComparison.OrdinalIgnoreCase)
            || string.Equals(primaryKeyName, "xxxx", StringComparison.OrdinalIgnoreCase);
    }

    private static string CreateUnfilteredAnchorSql(string anchorTable, string anchorKeyColumnName, string anchorKeyAlias)
    {
        var selectedAnchorKey = string.Equals(anchorKeyColumnName, anchorKeyAlias, StringComparison.Ordinal)
            ? anchorKeyColumnName
            : $"{anchorKeyColumnName} as {anchorKeyAlias}";

        return $"select distinct {selectedAnchorKey}{Environment.NewLine}from {anchorTable}";
    }

    private static string Indent(string text, string indent)
    {
        var lines = text.Replace("\r\n", "\n", StringComparison.Ordinal).Split('\n');
        return string.Join(Environment.NewLine, lines.Select(line => $"{indent}{line}"));
    }

    private sealed record ComposedResultProjectionRequest(
        string AnchorTable,
        string AnchorKeyColumnName,
        string TargetJoinColumn,
        string AnchorToTargetSql,
        IReadOnlyList<FacetConfig2> PredicateConfigs
    );
}
