﻿namespace SeadQueryCore.Plugin.Range;

public class RangeCategoryInfoService(
    IRangeCategoryInfoSqlCompiler categoryInfoSqlCompiler,
    IRangeOuterBoundService outerBoundEService
) : IRangeCategoryInfoService
{
    public IRangeCategoryInfoSqlCompiler CategoryInfoSqlCompiler { get; } = categoryInfoSqlCompiler;
    public IRangeOuterBoundService OuterBoundEService { get; } = outerBoundEService;
    public ICategoryInfoSqlCompiler SqlCompiler => CategoryInfoSqlCompiler;

    public FacetContent.CategoryInfo GetCategoryInfo(FacetsConfig2 facetsConfig, string facetCode, dynamic payload = null)
    {
        var intervalCount = (int)(payload ?? 50);

        var facetConfig = facetsConfig.GetConfig(facetCode);
        // var precision = facet.CategoryIdType == "integer" ? 0 : 2;
        var (dataLow, dataHigh) = OuterBoundEService.GetUpperLowerBounds(facetConfig.Facet);

        var ticker = new AdaptiveTicker();
        var fullExtent = ticker.GetInterval(dataLow, dataHigh, intervalCount);
        var pickExtent = fullExtent;

        if (facetConfig.GetPickCount() >= 2)
        {
            var picks = facetConfig.GetPickValues();
            pickExtent = ticker.GetInterval(picks[0], picks[1], picks.Count > 2 ? (int)picks[2] : intervalCount);
        }

        string sql = CategoryInfoSqlCompiler.Compile(null, facetConfig.Facet, pickExtent);

        return new FacetContent.CategoryInfo
        {
            Count = pickExtent.IntervalCount,
            Query = sql,
            Extent = [ fullExtent.OuterLow, fullExtent.OuterLow, fullExtent.IntervalCount ],
            FullExtent = fullExtent,
            PickExtent = pickExtent,
        };
    }
}

