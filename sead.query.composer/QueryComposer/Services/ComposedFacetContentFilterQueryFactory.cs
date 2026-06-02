using System;
using System.Linq;
using SeadQueryComposer.QueryComposer.Inputs;
using SeadQueryComposer.RouteCompiler;
using SeadQueryCore;
using SeadQueryCore.Plugin.Discrete;
using SeadQueryCore.QueryComposer;

namespace SeadQueryComposer.QueryComposer.Services;

/// <summary>
/// Factory for creating composed facet content filter queries based on the provided request parameters.
/// </summary>
public sealed class ComposedFacetContentFilterQueryFactory : IComposedFacetContentFilterQueryFactory
{
    private readonly IPathFinder _pathFinder;
    private readonly IDiscreteFacetPredicateResolver _predicateResolver;
    private readonly IComposedFilterQueryComposer _composedFilterQueryComposer;

    public ComposedFacetContentFilterQueryFactory(
        IPathFinder pathFinder,
        IDiscreteFacetPredicateResolver predicateResolver,
        IComposedFilterQueryComposer composedFilterQueryComposer
    )
    {
        _pathFinder = pathFinder ?? throw new ArgumentNullException(nameof(pathFinder));
        _predicateResolver = predicateResolver ?? throw new ArgumentNullException(nameof(predicateResolver));
        _composedFilterQueryComposer = composedFilterQueryComposer ?? throw new ArgumentNullException(nameof(composedFilterQueryComposer));
    }

    /// <summary>
    /// Creates a composed filter query based on the provided request parameters, including facet configurations and anchor table information.
    /// </summary>
    /// <param name="request">The request containing the parameters for the composed facet content filter query.</param>
    /// <returns>A composed filter query based on the provided request parameters.</returns>
    public ComposedFilterQuery Create(ComposedFacetContentRequest request)
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
            Sql = ComposedFacetContentSupport.CreateUnfilteredAnchorSql(
                request.AnchorTable,
                request.AnchorKeyColumnName,
                QueryComposerAliases.AnchorKeyColumn
            ),
        };
    }

    /// <summary>
    /// Creates a predicate query plan for a given facet configuration and composed facet content request, which includes generating the necessary SQL to link the facet's target table to the anchor table based on the user's selected facet values.
    /// </summary>
    /// <param name="config">The facet configuration for which to create the predicate query plan.</param>
    /// <param name="request">The composed facet content request containing the parameters for the query.</param>
    /// <returns>A predicate query plan containing the SQL and metadata for the facet.</returns>
    private PredicateQueryPlan CreatePredicateQueryPlan(FacetConfig2 config, ComposedFacetContentRequest request)
    {
        var sourceFacet = config.Facet;
        var sourceTableName = sourceFacet.TargetTable.TableOrUdfName;
        var sourceKeyColumn = ComposedFacetContentSupport.ResolvePredicateSourceKeyColumn(sourceFacet);
        var sourceCriteria = ComposedFacetContentSupport.ResolvePredicateCriteria(sourceFacet);
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
}
