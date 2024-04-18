using SeadQueryCore.QueryBuilder;

namespace SeadQueryCore
{
    public class DiscreteCategoryInfoService : ICategoryInfoService
    {
        public DiscreteCategoryInfoService(
            IFacetSetting config,
            IRepositoryRegistry context,
            IQuerySetupBuilder builder,
            ICategoryCountService categoryCountService,
            IDiscreteCategoryInfoSqlCompiler contentSqlCompiler,
            ITypedQueryProxy queryProxy
            )
        {
            QuerySetupBuilder = builder;
            SqlCompiler = contentSqlCompiler;
        }

        IQuerySetupBuilder QuerySetupBuilder { get; }

        public IDiscreteCategoryInfoSqlCompiler SqlCompiler { get; }

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
