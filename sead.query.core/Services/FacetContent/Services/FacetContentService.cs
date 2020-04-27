using Newtonsoft.Json;
using SeadQueryCore.QueryBuilder;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using static SeadQueryCore.FacetContent;

namespace SeadQueryCore
{
    public class FacetContentService : QueryServiceBase, IFacetContentService {

        public ICategoryCountService CategoryCountService { get; set; }
        public IFacetSetting Config { get; }
        public ITypedQueryProxy QueryProxy { get; }

        public FacetContentService(
            IFacetSetting config,
            IRepositoryRegistry context,
            IQuerySetupBuilder builder,
            ITypedQueryProxy queryProxy
        ) : base(context, builder)
        {
            Config = config;
            QueryProxy = queryProxy;
        }

        public FacetContent Load(FacetsConfig2 facetsConfig)
        {
            var intervalInfo        = CompileIntervalQuery(facetsConfig, facetsConfig.TargetCode);
            var categoryCounts      = QueryCategoryCounts(facetsConfig, intervalInfo.Query);
            var outerCategoryCounts = QueryOuterCategoryCounts(intervalInfo.Query, categoryCounts.Data).ToList();
            var userPicks           = facetsConfig.CollectUserPicks(facetsConfig.TargetCode);

            var facetContent = new FacetContent(facetsConfig, outerCategoryCounts, categoryCounts.Data, categoryCounts.SqlQuery, userPicks, intervalInfo);

            return facetContent;
        }

        protected virtual IntervalQueryInfo CompileIntervalQuery(FacetsConfig2 facetsConfig, string facetCode, int interval=120) => new IntervalQueryInfo();

        private CategoryCountService.CategoryCountResult QueryCategoryCounts(FacetsConfig2 facetsConfig, string intervalQuery)
        {
            var categoryCounts = CategoryCountService.Load(facetsConfig.TargetCode, facetsConfig, intervalQuery);
            return categoryCounts;
        }

        protected List<CategoryCountItem> QueryOuterCategoryCounts(string intervalQuery, Dictionary<string, CategoryCountItem> distribution)
        {
            var outerCategoryCounts = QueryProxy.QueryRows(intervalQuery, dr => CreateItem(dr, distribution)).ToList();
            return outerCategoryCounts;
        }

        protected CategoryCountItem CreateItem(IDataReader dr, Dictionary<string, CategoryCountItem> filteredCategoryCounts)
        {
            var category = GetCategory(dr);
            var name = GetName(dr);
            var categoryCountItem = filteredCategoryCounts.GetValueOrDefault(category);
            return CategoryCountItem.Create(categoryCountItem, category, name);
        }

        protected virtual string GetCategory(IDataReader dr) => dr.IsDBNull(0) ? "" : dr.GetString(0);
        protected virtual string GetName(IDataReader dr) => dr.IsDBNull(1) ? "" : dr.GetString(1);
    }
}