namespace SeadQueryCore.Plugin.Intersect;

public class IntersectCategoryInfoService(
    IIntersectCategoryInfoSqlCompiler categoryInfoSqlCompiler,
    IIntersectOuterBoundService outerBoundService
) : IIntersectCategoryInfoService
{
    public IIntersectCategoryInfoSqlCompiler CategoryInfoSqlCompiler { get; } = categoryInfoSqlCompiler;
    public IIntersectOuterBoundService OuterBoundService { get; } = outerBoundService;
    public ICategoryInfoSqlCompiler SqlCompiler => CategoryInfoSqlCompiler;

    public FacetContent.CategoryInfo GetCategoryInfo(FacetsConfig2 facetsConfig, string facetCode, dynamic payload = null)
    {
        var intervalCount = (int)(payload ?? 80);

        var facetConfig = facetsConfig.GetConfig(facetCode);
        // var precision = facet.CategoryIdType == "integer" ? 0 : 2;
        var (dataLow, dataHigh) = OuterBoundService.GetUpperLowerBounds(facetConfig.Facet);

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
            Extent = [fullExtent.OuterLow, fullExtent.OuterLow, fullExtent.IntervalCount],
            FullExtent = fullExtent,
            PickExtent = pickExtent,
        };
    }
}
