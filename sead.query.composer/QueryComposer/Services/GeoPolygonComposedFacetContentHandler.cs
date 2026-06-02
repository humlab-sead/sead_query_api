using System;
using System.Collections.Generic;
using System.Linq;
using SeadQueryCore;
using SeadQueryCore.Plugin.GeoPolygon;
using SeadQueryCore.QueryComposer;

namespace SeadQueryComposer.QueryComposer.Services;

public sealed class GeoPolygonComposedFacetContentHandler : IComposedFacetContentHandler
{
    private readonly ITypedQueryProxy _queryProxy;
    private readonly IFacetContentQueryComposer _facetContentQueryComposer;
    private readonly IGeoPolygonCategoryInfoService _geoPolygonCategoryInfoService;

    public GeoPolygonComposedFacetContentHandler(
        ITypedQueryProxy queryProxy,
        IFacetContentQueryComposer facetContentQueryComposer,
        IGeoPolygonCategoryInfoService geoPolygonCategoryInfoService
    )
    {
        _queryProxy = queryProxy ?? throw new ArgumentNullException(nameof(queryProxy));
        _facetContentQueryComposer = facetContentQueryComposer ?? throw new ArgumentNullException(nameof(facetContentQueryComposer));
        _geoPolygonCategoryInfoService =
            geoPolygonCategoryInfoService ?? throw new ArgumentNullException(nameof(geoPolygonCategoryInfoService));
    }

    public EFacetType FacetType => EFacetType.GeoPolygon;

    public bool TryValidate(FacetsConfig2 facetsConfig, IReadOnlyList<FacetConfig2> predicateConfigs, out string failureReason)
    {
        if (predicateConfigs.Count > 0)
        {
            failureReason =
                $"target facet '{facetsConfig.TargetFacet.FacetCode}' is geo-polygon and does not support additional predicate facets on the composed facet-content path.";
            return false;
        }

        failureReason = string.Empty;
        return true;
    }

    public string ResolveTargetJoinColumn(Facet targetFacet, string anchorKeyColumn)
    {
        return ComposedFacetContentSupport.ResolveSimpleTargetJoinColumn(targetFacet);
    }

    public FacetContent Load(FacetsConfig2 facetsConfig, ComposedFacetContentRequest request, ComposedFilterQuery composedFilterQuery)
    {
        var categoryInfo = _geoPolygonCategoryInfoService.GetCategoryInfo(facetsConfig, facetsConfig.TargetCode);
        var contentQueryPlan = _facetContentQueryComposer.Compose(
            facetsConfig,
            composedFilterQuery,
            request.TargetJoinColumn,
            request.AnchorToTargetSql,
            categoryInfo.Query
        );
        var categoryItems = _queryProxy.QueryRows(contentQueryPlan.Sql, ComposedFacetContentSupport.ToGeoPolygonCategoryItem);
        var userPicks = facetsConfig.CollectUserPicks(facetsConfig.TargetCode);

        return new FacetContent
        {
            FacetsConfig = facetsConfig,
            Items = categoryItems.Where(item => item.Count != null).ToList(),
            Distribution = categoryItems.ToDictionary(item => item.Category ?? "(null)"),
            IntervalInfo = categoryInfo,
            SqlQuery = contentQueryPlan.Sql,
            Picks = userPicks ?? [],
        };
    }
}
