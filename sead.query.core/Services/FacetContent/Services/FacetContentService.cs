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
    public class FacetContentService : QueryServiceBase, IFacetContentService
    {
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
            /* Compile Interval Sql and number of categories */
            var intervalInfo = CompileIntervalQuery(facetsConfig, facetsConfig.TargetCode);

            /* Compute (filtered) category counts */
            var categoryCounts = QueryCategoryCounts(facetsConfig, intervalInfo.Query);

            /* Compile counts for full set of categories */
            var outerCategoryCounts = QueryOuterCategoryCounts(intervalInfo.Query, categoryCounts.CategoryCounts).ToList();

            /* Collect user picks */
            var userPicks = facetsConfig.CollectUserPicks(facetsConfig.TargetCode);

            var facetContent = new FacetContent
            {
                FacetsConfig = facetsConfig,
                Items = outerCategoryCounts.Where(z => z.Count != null).ToList(), /* add empty categories then remove, hmmm...? */
                Distribution = categoryCounts.CategoryCounts,
                IntervalInfo = intervalInfo,
                SqlQuery = categoryCounts.SqlQuery,
                Picks = userPicks ?? new Dictionary<string, FacetsConfig2.UserPickData>()
            };
            return facetContent;
        }

        /// <summary>
        /// Compile Sql query that returns the categories
        /// </summary>
        /// <param name="facetsConfig"></param>
        /// <param name="facetCode"></param>
        /// <param name="interval"></param>
        /// <returns></returns>
        protected virtual IntervalQueryInfo CompileIntervalQuery(FacetsConfig2 facetsConfig, string facetCode, int interval = 120) => new IntervalQueryInfo();

        private CategoryCountService.CategoryCountData QueryCategoryCounts(FacetsConfig2 facetsConfig, string intervalQuery)
        {
            var categoryCounts = CategoryCountService.Load(facetsConfig.TargetCode, facetsConfig, intervalQuery);
            return categoryCounts;
        }

        /// <summary>
        /// Compute counts for full set of categories
        /// </summary>
        /// <param name="intervalQuery"></param>
        /// <param name="categoryCounts"></param>
        /// <returns></returns>
        protected List<CategoryCountItem> QueryOuterCategoryCounts(string intervalQuery, Dictionary<string, CategoryCountItem> categoryCounts)
        {
            var outerCategoryCounts = QueryProxy.QueryRows(intervalQuery, dr => CreateItem(dr, categoryCounts)).ToList();
            return outerCategoryCounts;
        }

        protected CategoryCountItem CreateItem(IDataReader dr, Dictionary<string, CategoryCountItem> categoryCounts)
        {
            var category = GetCategory(dr);
            var name = GetName(dr);
            var value = categoryCounts.GetValueOrDefault(category);
            return CategoryCountItem.Create(value, category, name);
        }

        protected virtual string GetCategory(IDataReader dr) => dr.IsDBNull(0) ? "" : dr.GetString(0);
        protected virtual string GetName(IDataReader dr) => dr.IsDBNull(1) ? "" : dr.GetString(1);
    }

}
