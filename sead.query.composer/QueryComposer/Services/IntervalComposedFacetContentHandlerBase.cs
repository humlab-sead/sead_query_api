using System;
using System.Collections.Generic;
using System.Linq;
using SeadQueryCore;
using SeadQueryCore.QueryComposer;

namespace SeadQueryComposer.QueryComposer.Services;

public abstract class IntervalComposedFacetContentHandlerBase : IComposedFacetContentHandler
{
    private readonly ITypedQueryProxy _queryProxy;
    private readonly IFacetContentQueryComposer _facetContentQueryComposer;
    private readonly ICategoryInfoService _categoryInfoService;

    protected IntervalComposedFacetContentHandlerBase(
        ITypedQueryProxy queryProxy,
        IFacetContentQueryComposer facetContentQueryComposer,
        ICategoryInfoService categoryInfoService
    )
    {
        _queryProxy = queryProxy ?? throw new ArgumentNullException(nameof(queryProxy));
        _facetContentQueryComposer = facetContentQueryComposer ?? throw new ArgumentNullException(nameof(facetContentQueryComposer));
        _categoryInfoService = categoryInfoService ?? throw new ArgumentNullException(nameof(categoryInfoService));
    }

    public abstract EFacetType FacetType { get; }

    public bool TryValidate(FacetsConfig2 facetsConfig, IReadOnlyList<FacetConfig2> predicateConfigs, out string failureReason)
    {
        failureReason = string.Empty;
        return true;
    }

    public string ResolveTargetJoinColumn(Facet targetFacet, string anchorKeyColumn)
    {
        return ComposedFacetContentSupport.ResolveIntervalTargetJoinColumn(targetFacet, anchorKeyColumn);
    }

    public FacetContent Load(FacetsConfig2 facetsConfig, ComposedFacetContentRequest request, ComposedFilterQuery composedFilterQuery)
    {
        var categoryInfo = _categoryInfoService.GetCategoryInfo(facetsConfig, facetsConfig.TargetCode);
        var contentQueryPlan = _facetContentQueryComposer.Compose(
            facetsConfig,
            composedFilterQuery,
            request.TargetJoinColumn,
            request.AnchorToTargetSql,
            categoryInfo.Query
        );
        var userPicks = facetsConfig.CollectUserPicks(facetsConfig.TargetCode);
        var categoryCounts = _queryProxy
            .QueryRows(contentQueryPlan.Sql, ComposedFacetContentSupport.ToRangeCategoryItem)
            .ToDictionary(item => item.Category ?? "(null)");
        var outerCategoryCounts = _queryProxy.QueryRows(categoryInfo.Query, _categoryInfoService.SqlCompiler.ToItem).ToList();

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
}
