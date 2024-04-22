namespace SeadQueryCore
{
    public class RangeCategoryInfoService(
        IRangeCategoryInfoSqlCompiler categoryInfoSqlCompiler,
        IRangeOuterBoundExtentService outerBoundExtentService
    ) : ICategoryInfoService
    {
        public IRangeCategoryInfoSqlCompiler CategoryInfoSqlCompiler { get; } = categoryInfoSqlCompiler;
        public IRangeOuterBoundExtentService OuterBoundExtentService { get; } = outerBoundExtentService;
        public ICategoryInfoSqlCompiler SqlCompiler => CategoryInfoSqlCompiler;

        public FacetContent.CategoryInfo GetCategoryInfo(FacetsConfig2 facetsConfig, string facetCode, dynamic payload = null)
        {
            var intervalCount = (int)(payload ?? 120);

            var facetConfig = facetsConfig.GetConfig(facetCode);
            var (min, max) = OuterBoundExtentService.GetUpperLowerBounds(facetConfig.Facet);
            var fullExtent = Interval.Create([min, max, intervalCount]);
            var pickExtent = Interval.Create(facetConfig.GetPickValues(false)) ?? fullExtent;

            string sql = CategoryInfoSqlCompiler.Compile(null, null, pickExtent);

            return new FacetContent.CategoryInfo
            {
                Count = pickExtent.SegmentCount,
                Query = sql,
                Extent = fullExtent.ToList()
            };
        }
    }
}
