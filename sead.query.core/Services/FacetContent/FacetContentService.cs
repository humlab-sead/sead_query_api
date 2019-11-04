using Newtonsoft.Json;
using SeadQueryCore.QueryBuilder;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using static SeadQueryCore.FacetContent;

namespace SeadQueryCore
{     
    public class FacetContentService : QueryServiceBase, IFacetContentService {

        public ICategoryCountService CountService { get; set; }
        public IFacetSetting Config { get; }

        public FacetContentService(
            IFacetSetting config,
            IRepositoryRegistry context,
            IQuerySetupCompiler builder
        ) : base(context, builder)
        {
            Config = config;
        }

        public FacetContent Load(FacetsConfig2 facetsConfig)
        {
            FacetContent.IntervalQueryInfo intervalInfo = CompileIntervalQuery(facetsConfig, facetsConfig.TargetCode);
            var distribution = GetCategoryCounts(facetsConfig, intervalInfo.Query);
            var items        = CompileItems(intervalInfo.Query, distribution).ToList();
            var picks        = facetsConfig.CollectUserPicks(facetsConfig.TargetCode);
            var facetContent = new FacetContent(facetsConfig, items, distribution, picks, intervalInfo);
            return facetContent;
        }

        protected virtual IntervalQueryInfo CompileIntervalQuery(FacetsConfig2 facetsConfig, string facetCode, int interval=120) => new IntervalQueryInfo();

        private Dictionary<string, CategoryCountItem> GetCategoryCounts(FacetsConfig2 facetsConfig, string intervalQuery)
        {
            Dictionary<string, CategoryCountItem> categoryCounts = CountService.Load(facetsConfig.TargetCode, facetsConfig, intervalQuery);
            return categoryCounts;
        }

        protected List<FacetContent.ContentItem> CompileItems(string intervalQuery, Dictionary<string, CategoryCountItem> distribution)
        {
            var rows = Context.QueryRows(intervalQuery, dr => CreateItem(dr, distribution)).ToList();
            return rows;
        }

        protected FacetContent.ContentItem CreateItem(DbDataReader dr, Dictionary<string, CategoryCountItem> distribution)
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

        protected virtual string GetCategory(DbDataReader dr) => dr.IsDBNull(0) ? "" : dr.GetString(0);
        protected virtual string GetName(DbDataReader dr) => dr.IsDBNull(1) ? "" : dr.GetString(1);
    }
}