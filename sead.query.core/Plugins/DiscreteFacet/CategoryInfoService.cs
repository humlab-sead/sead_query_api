using SeadQueryCore.QueryBuilder;

namespace SeadQueryCore
{
    public class DiscreteCategoryInfoService(IQuerySetupBuilder builder, IDiscreteCategoryInfoSqlCompiler contentSqlCompiler
        ) : ICategoryInfoService
    {
        IQuerySetupBuilder QuerySetupBuilder { get; } = builder;

        public IDiscreteCategoryInfoSqlCompiler SqlCompiler { get; } = contentSqlCompiler;

        ICategoryInfoSqlCompiler ICategoryInfoService.SqlCompiler => SqlCompiler;

        public FacetContent.CategoryInfo GetCategoryInfo(FacetsConfig2 facetsConfig, string facetCode, int count = 0)
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
