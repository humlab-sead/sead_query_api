using SeadQueryCore.QueryBuilder;

namespace SeadQueryCore.Plugin.Common
{
    public abstract class CategoryInfoService(ISupportedRequestQuerySetupFactory factory, ICategoryInfoSqlCompiler compiler) : ICategoryInfoService
    {
        ISupportedRequestQuerySetupFactory QuerySetupFactory { get; } = factory;

        public ICategoryInfoSqlCompiler SqlCompiler { get; } = compiler;

        public FacetContent.CategoryInfo GetCategoryInfo(FacetsConfig2 facetsConfig, string facetCode, dynamic payload = null)
        {
            var querySetup = QuerySetupFactory.Create(facetsConfig, facetsConfig.TargetFacet);
            var sql = SqlCompiler.Compile(querySetup, facetsConfig.TargetFacet, facetsConfig.GetTargetTextFilter());
            return new FacetContent.CategoryInfo
            {
                Count = 1,
                Query = sql
            };
        }
    }
}
