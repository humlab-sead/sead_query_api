using SeadQueryCore.QueryBuilder;

namespace SeadQueryCore.Plugin.Common
{
    public class CategoryInfoService(IQuerySetupBuilder builder, ICategoryInfoSqlCompiler compiler) : ICategoryInfoService
    {
        IQuerySetupBuilder QuerySetupBuilder { get; } = builder;

        public ICategoryInfoSqlCompiler SqlCompiler { get; } = compiler;

        public FacetContent.CategoryInfo GetCategoryInfo(FacetsConfig2 facetsConfig, string facetCode, dynamic payload = null)
        {
            var querySetup = QuerySetupBuilder.Build(facetsConfig, facetsConfig.TargetFacet, null, null);
            var sql = SqlCompiler.Compile(querySetup, facetsConfig.TargetFacet, facetsConfig.GetTargetTextFilter());
            return new FacetContent.CategoryInfo
            {
                Count = 1,
                Query = sql
            };
        }
    }
}
