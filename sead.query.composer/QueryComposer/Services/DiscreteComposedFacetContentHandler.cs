using System;
using System.Collections.Generic;
using System.Linq;
using SeadQueryCore;
using SeadQueryCore.Plugin.Discrete;
using SeadQueryCore.QueryComposer;

namespace SeadQueryComposer.QueryComposer.Services;

public sealed class DiscreteComposedFacetContentHandler : IComposedFacetContentHandler
{
    private readonly ITypedQueryProxy _queryProxy;
    private readonly IFacetContentQueryComposer _facetContentQueryComposer;
    private readonly IDiscreteCategoryInfoService _discreteCategoryInfoService;

    public DiscreteComposedFacetContentHandler(
        ITypedQueryProxy queryProxy,
        IFacetContentQueryComposer facetContentQueryComposer,
        IDiscreteCategoryInfoService discreteCategoryInfoService
    )
    {
        _queryProxy = queryProxy ?? throw new ArgumentNullException(nameof(queryProxy));
        _facetContentQueryComposer = facetContentQueryComposer ?? throw new ArgumentNullException(nameof(facetContentQueryComposer));
        _discreteCategoryInfoService = discreteCategoryInfoService ?? throw new ArgumentNullException(nameof(discreteCategoryInfoService));
    }

    public EFacetType FacetType => EFacetType.Discrete;

    public bool TryValidate(FacetsConfig2 facetsConfig, IReadOnlyList<FacetConfig2> predicateConfigs, out string failureReason)
    {
        failureReason = string.Empty;
        return true;
    }

    public string ResolveTargetJoinColumn(Facet targetFacet, string anchorKeyColumn)
    {
        return ComposedFacetContentSupport.ResolveSimpleTargetJoinColumn(targetFacet);
    }

    public FacetContent Load(FacetsConfig2 facetsConfig, ComposedFacetContentRequest request, ComposedFilterQuery composedFilterQuery)
    {
        var contentQueryPlan = _facetContentQueryComposer.Compose(
            facetsConfig,
            composedFilterQuery,
            request.TargetJoinColumn,
            request.AnchorToTargetSql,
            null
        );
        var categoryItems = _queryProxy.QueryRows(contentQueryPlan.Sql, ComposedFacetContentSupport.ToCategoryItem);
        var userPicks = facetsConfig.CollectUserPicks(facetsConfig.TargetCode);

        if (request.PredicateConfigs.Count == 0 && !string.IsNullOrWhiteSpace(request.AnchorToTargetSql))
        {
            return CreateTargetOnlyFacetContent(facetsConfig, contentQueryPlan.Sql, userPicks, categoryItems);
        }

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

    private FacetContent CreateTargetOnlyFacetContent(
        FacetsConfig2 facetsConfig,
        string sqlQuery,
        Dictionary<string, FacetsConfig2.UserPickData> userPicks,
        List<CategoryItem> categoryItems
    )
    {
        var categoryInfo = _discreteCategoryInfoService.GetCategoryInfo(facetsConfig, facetsConfig.TargetCode);
        var categoryCounts = categoryItems.ToDictionary(item => item.Category ?? "(null)");
        var outerCategoryCounts = _queryProxy.QueryRows(categoryInfo.Query, _discreteCategoryInfoService.SqlCompiler.ToItem).ToList();

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
            SqlQuery = sqlQuery,
            Picks = userPicks ?? [],
        };
    }
}
