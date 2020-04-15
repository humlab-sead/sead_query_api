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

        public ICategoryCountService CountService { get; set; }
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
            var intervalInfo = CompileIntervalQuery(facetsConfig, facetsConfig.TargetCode);
            var countResult  = GetCategoryCounts(facetsConfig, intervalInfo.Query);
            var items        = CompileItems(intervalInfo.Query, countResult.Data).ToList();
            var picks        = facetsConfig.CollectUserPicks(facetsConfig.TargetCode);
            var facetContent = new FacetContent(facetsConfig, items, countResult.Data, picks, intervalInfo);
            return facetContent;
        }

        protected virtual IntervalQueryInfo CompileIntervalQuery(FacetsConfig2 facetsConfig, string facetCode, int interval=120) => new IntervalQueryInfo();

        private CategoryCountService.CategoryCountResult GetCategoryCounts(FacetsConfig2 facetsConfig, string intervalQuery)
        {
            var countResult = CountService.Load(facetsConfig.TargetCode, facetsConfig, intervalQuery);
            return countResult;
        }

        protected List<FacetContent.ContentItem> CompileItems(string intervalQuery, Dictionary<string, CategoryCountItem> distribution)
        {
            var rows = QueryProxy.QueryRows(intervalQuery, dr => CreateItem(dr, distribution)).ToList();
            return rows;
        }

        protected FacetContent.ContentItem CreateItem(IDataReader dr, Dictionary<string, CategoryCountItem> distribution)
        {
            string category = GetCategory(dr);
            string name = GetName(dr);
            CategoryCountItem countValue = distribution.ContainsKey(category) ? distribution[category] : null;
            return new FacetContent.ContentItem() {
                Category = category,
                DisplayName = name,
                Name = name,
                Count = countValue?.Count,
                Extent = countValue?.Extent
            };
        }

        protected virtual string GetCategory(IDataReader dr) => dr.IsDBNull(0) ? "" : dr.GetString(0);
        protected virtual string GetName(IDataReader dr) => dr.IsDBNull(1) ? "" : dr.GetString(1);
    }
}