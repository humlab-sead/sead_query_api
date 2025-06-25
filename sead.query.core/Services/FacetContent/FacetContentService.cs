using System.Data;
using System.Linq;
using SeadQueryCore.QueryBuilder;

namespace SeadQueryCore
{
    public class FacetContentService(
        IFacetSetting config,
        IRepositoryRegistry context,
        IQuerySetupBuilder builder,
        ITypedQueryProxy queryProxy,
        ICategoryCountService categoryCountService
    ) : QueryServiceBase(context, builder), IFacetContentService
    {
        public ICategoryCountService CategoryCountService { get; set; } = categoryCountService;
        public IFacetSetting Config { get; } = config;
        public ITypedQueryProxy QueryProxy { get; } = queryProxy;

        public FacetContent Load(FacetsConfig2 facetsConfig)
        {
            /* Fetch category counts */
            var categoryCounts = CategoryCountService.Load(facetsConfig.TargetCode, facetsConfig);

            /* Collect user picks */
            var userPicks = facetsConfig.CollectUserPicks(facetsConfig.TargetCode);

            var facetContent = new FacetContent
            {
                FacetsConfig = facetsConfig,
                Items = categoryCounts.OuterCategoryCounts.Where(z => z.Count != null).ToList(),
                Distribution = categoryCounts.CategoryCounts,
                IntervalInfo = categoryCounts.CategoryInfo,
                SqlQuery = categoryCounts.SqlQuery,
                Picks = userPicks ?? [],
            };
            return facetContent;
        }
    }
}
