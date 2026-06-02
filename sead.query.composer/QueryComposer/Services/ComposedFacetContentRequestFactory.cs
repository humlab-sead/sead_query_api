using System;
using System.Collections.Generic;
using System.Linq;
using SeadQueryComposer.RouteCompiler;
using SeadQueryCore;

namespace SeadQueryComposer.QueryComposer.Services;

/// <summary>
/// Builds composed facet-content requests that describe the anchor table, join columns,
/// and predicate set for the composed path.
/// </summary>
public sealed class ComposedFacetContentRequestFactory : IComposedFacetContentRequestFactory
{
    private readonly IRepositoryRegistry _registry;
    private readonly IPathFinder _pathFinder;
    private readonly IRouteSqlCompiler _routeSqlCompiler;

    public ComposedFacetContentRequestFactory(IRepositoryRegistry registry, IPathFinder pathFinder, IRouteSqlCompiler routeSqlCompiler)
    {
        _registry = registry ?? throw new ArgumentNullException(nameof(registry));
        _pathFinder = pathFinder ?? throw new ArgumentNullException(nameof(pathFinder));
        _routeSqlCompiler = routeSqlCompiler ?? throw new ArgumentNullException(nameof(routeSqlCompiler));
    }

    public bool TryCreate(
        FacetsConfig2 facetsConfig,
        IComposedFacetContentHandler handler,
        out ComposedFacetContentRequest request,
        out string failureReason
    )
    {
        request = null;
        failureReason = string.Empty;

        var targetFacet = facetsConfig?.TargetFacet;
        if (targetFacet is null)
        {
            failureReason = "target facet is missing from the request.";
            return false;
        }

        var aggregateFacet = _registry.Facets.Get(targetFacet.AggregateFacetId) ?? targetFacet;
        var anchorTable = aggregateFacet.TargetTable?.TableOrUdfName;
        if (string.IsNullOrWhiteSpace(anchorTable))
        {
            failureReason =
                $"aggregate facet '{aggregateFacet.FacetCode ?? targetFacet.FacetCode ?? "(unknown)"}' does not expose an anchor table.";
            return false;
        }

        var anchorKeyColumn = ResolveAnchorKeyColumn(aggregateFacet);
        if (string.IsNullOrWhiteSpace(anchorKeyColumn))
        {
            failureReason =
                $"aggregate facet '{aggregateFacet.FacetCode ?? targetFacet.FacetCode ?? "(unknown)"}' does not expose a simple anchor key column on its target table.";
            return false;
        }

        var targetTableName = targetFacet.TargetTable?.TableOrUdfName;
        var targetJoinColumn = handler.ResolveTargetJoinColumn(targetFacet, anchorKeyColumn);
        if (string.IsNullOrWhiteSpace(targetTableName) || string.IsNullOrWhiteSpace(targetJoinColumn))
        {
            failureReason =
                $"target facet '{targetFacet.FacetCode ?? facetsConfig.TargetCode}' does not expose a routable target join column on its target table.";
            return false;
        }

        var predicateConfigs = ResolvePredicateConfigs(facetsConfig);
        if (!handler.TryValidate(facetsConfig, predicateConfigs, out failureReason))
        {
            return false;
        }

        if (predicateConfigs.Count > 0)
        {
            var emptyPredicateConfig = predicateConfigs.FirstOrDefault(config => !config.HasPicks() && !config.HasEnforcedConstraints());
            if (emptyPredicateConfig is not null)
            {
                failureReason =
                    $"predicate facet '{emptyPredicateConfig.FacetCode}' has no picks; remove empty secondary facet configs before using the composed facet-content path.";
                return false;
            }

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
                    $"target facet '{targetFacet.FacetCode}' cannot be routed from anchor table '{anchorTable}' to target table '{targetTableName}'.";
                return false;
            }
        }

        request = new ComposedFacetContentRequest(anchorTable, anchorKeyColumn, targetJoinColumn, anchorToTargetSql, predicateConfigs);
        failureReason = string.Empty;
        return true;
    }

    public ComposedFacetContentRequest Create(FacetsConfig2 facetsConfig, IComposedFacetContentHandler handler)
    {
        if (TryCreate(facetsConfig, handler, out var request, out var failureReason))
        {
            return request;
        }

        throw new InvalidOperationException($"Cannot create composed facet-content request: {failureReason}");
    }

    private List<FacetConfig2> ResolvePredicateConfigs(FacetsConfig2 facetsConfig)
    {
        var affectedConfigs = facetsConfig.GetConfigsThatAffectsTarget(facetsConfig.TargetCode, facetsConfig.GetFacetCodes());
        if (facetsConfig.HasDomainCode())
        {
            var domainConfig = facetsConfig.CreateDomainConfig();
            if (
                domainConfig is not null
                && (domainConfig.HasPicks() || domainConfig.HasEnforcedConstraints())
                && affectedConfigs.All(config =>
                    !string.Equals(config.FacetCode, domainConfig.FacetCode, StringComparison.OrdinalIgnoreCase)
                )
            )
            {
                affectedConfigs.Insert(0, domainConfig);
            }
        }

        return affectedConfigs
            .Where(config => !string.Equals(config.FacetCode, facetsConfig.TargetCode, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    private bool TryGetPredicateFailureReason(FacetConfig2 config, string anchorTable, out string failureReason)
    {
        failureReason = string.Empty;

        var facet = config?.Facet;
        var sourceTableName = facet?.TargetTable?.TableOrUdfName;

        if (facet?.FacetTypeId != EFacetType.Discrete)
        {
            failureReason =
                $"predicate facet '{config?.FacetCode ?? facet?.FacetCode ?? "(unknown)"}' uses facet type '{facet?.FacetTypeId}' which the composed facet-content path does not support as a secondary predicate.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(sourceTableName))
        {
            failureReason = $"predicate facet '{config?.FacetCode ?? facet?.FacetCode ?? "(unknown)"}' does not expose a source table.";
            return false;
        }

        if (!ComposedFacetContentSupport.TryResolvePredicateSourceKeyColumn(facet, out _))
        {
            failureReason =
                $"predicate facet '{config?.FacetCode ?? facet?.FacetCode ?? "(unknown)"}' does not expose a simple source key column on its target table.";
            return false;
        }

        if (!ComposedFacetContentSupport.TryResolvePredicateCriteria(facet, out _))
        {
            failureReason =
                $"predicate facet '{config?.FacetCode ?? facet?.FacetCode ?? "(unknown)"}' uses facet clauses that the composed predicate path cannot apply.";
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

    private static string ResolveAnchorKeyColumn(Facet anchorFacet)
    {
        return ComposedFacetContentSupport.TryResolveSimpleColumnOnTable(
            anchorFacet.CategoryIdExpr,
            anchorFacet.TargetTable,
            out var anchorKeyColumn
        )
            ? anchorKeyColumn
            : string.Empty;
    }

    private string CreateAnchorToTargetSql(string anchorTable, string anchorKeyColumn, string targetTable, string targetJoinColumn)
    {
        var routeTables = _pathFinder.Find(anchorTable, targetTable).ToTrail();
        return _routeSqlCompiler.Compile(routeTables, anchorKeyColumn, targetJoinColumn);
    }
}
