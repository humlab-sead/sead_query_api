using Autofac.Features.Indexed;
using QueryFacetDomain.QueryBuilder;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Common;
using System.Linq;
using static QueryFacetDomain.Utility;

namespace QueryFacetDomain
{
    public class FacetContent {

        public class CategoryItem {
            public EFacetType FacetType { get; set;  }
            public string Category { get; internal set; }
            public string DisplayName { get; internal set; }
            public string Name { get; internal set; }
            public int Count { get; internal set; }
            public Dictionary<EFacetPickType, decimal> CategoryDetails { get; internal set; }
        }

        public FacetsConfig2 FacetsConfig { get; set; }
        public List<CategoryItem> Items { get; set; } = new List<CategoryItem>();
        public Dictionary<string, CategeryCountValue> FilteredDistribution { get; set; }
        public Dictionary<string, FacetsConfig2.UserPickData> PickMatrix { get; set; }

        public string FacetCode { get => FacetsConfig.TargetCode; }
        public string RequestType { get => FacetsConfig.RequestType; }
        public int ItemCount { get => Items.Count(); }
        public int PageOffset { get => FacetsConfig.TargetConfig.StartRow; }
        public int PageSize { get => FacetsConfig.TargetConfig.RowCount; }

        public int Interval { get; set; }
        public string IntervalQuery { get; set; }
        public int CountOfSelections { get; set; } = 0;

        public FacetContent(
            FacetsConfig2 facetsConfig,
            List<CategoryItem> items,
            Dictionary<string, CategeryCountValue> filteredCounts,
            Dictionary<string, FacetsConfig2.UserPickData> pickMatrix,
            int interval, string intervalQuery)
        {
            FacetsConfig         = facetsConfig;
            Items                = items;
            FilteredDistribution = filteredCounts;
            Interval             = interval;
            IntervalQuery        = intervalQuery;
            PickMatrix           = pickMatrix ?? new Dictionary<string, FacetsConfig2.UserPickData>();
        }

        public (int,int) getPage(int minSize=12)
        {
            if (FacetsConfig.TargetFacet.FacetTypeId == EFacetType.Range) {
                return (0, 250);
            }
            (int offset, int size) = FacetsConfig.TargetConfig.GetPage();
            if (RequestType == "populate_text_search") {
                offset = array_find_index<CategoryItem>(Items, FacetsConfig.TargetConfig.TextFilter, z => z.Name);
                offset = Math.Max(0, Math.Min(offset, this.ItemCount - minSize));
            }
            return (offset, size);
        }
    }

    public class FacetContentLoader : QueryServiceBase {

        public IIndex<EFacetType, ICategoryCountService> CountServices { get; set; }

        public FacetContentLoader(IQueryBuilderSetting config, IUnitOfWork context, IQuerySetupBuilder builder, IIndex<EFacetType,ICategoryCountService> countServices) : base(config, context, builder)
        {
            CountServices = countServices;
        }

        public FacetContent load(FacetsConfig2 facetsConfig)
        {
            (int interval, string intervalQuery) = compileIntervalQuery(facetsConfig, facetsConfig.TargetCode);
            var distribution = getDistribution(facetsConfig, intervalQuery);
            List<FacetContent.CategoryItem> items = CompileItems(intervalQuery, distribution, facetsConfig).ToList();
            Dictionary<string, FacetsConfig2.UserPickData> pickMatrix = facetsConfig.collectUserPicks(facetsConfig.TargetCode);
            FacetContent facetContent = new FacetContent(facetsConfig, items, distribution, pickMatrix, interval, intervalQuery);
            return facetContent;
        }

        protected (int,string) compileIntervalQuery(FacetsConfig2 facetsConfig, string facetCode)
        {
            return (0, "");
        }

        private Dictionary<string, CategeryCountValue> getDistribution(FacetsConfig2 facetsConfig, string intervalQuery)
        {
            ICategoryCountService service = CountServices[facetsConfig.TargetFacet.FacetTypeId];
            IEnumerable<ICategeryCountValue> rawDistribution = service.load(facetsConfig.TargetCode, facetsConfig, intervalQuery);
            return rawDistribution;
        }

        protected virtual IEnumerable<FacetContent.CategoryItem> CompileItems(
            string intervalQuery,
            Dictionary<string, CategeryCountValue> distribution,
            FacetsConfig2 facetsConfig)
        {
            var rows = Context.QueryRows(intervalQuery, dr => CreateItem(dr, distribution, extraCategoryInfo));
            return rows;
        }

        protected virtual FacetContent.CategoryItem CreateItem(DbDataReader dr, Dictionary<string, CategeryCountValue> distribution, Dictionary<string, string> extraCategoryInfo)
        {
            return null;
        }

    }

    class DiscreteFacetContentLoader : FacetContentLoader {

        public DiscreteFacetContentLoader(IQueryBuilderSetting config, IUnitOfWork context, IQuerySetupBuilder builder) : base(config, context, builder)
        {
        }

        protected (int, string) compileIntervalQuery(FacetsConfig2 facetsConfig, string facetCode, int count=0)
        {
            QuerySetup query = QueryBuilder.Build(facetsConfig, facetsConfig.TargetCode, null, facetsConfig.GetFacetCodes());
            string sql = DiscreteContentSqlQueryBuilder.compile(query, facetsConfig.TargetFacet, facetsConfig.GetTargetTextFilter());
            return ( 1, sql );
        }

        protected override IEnumerable<FacetContent.CategoryItem> CompileItems(
            string intervalQuery,
            Dictionary<string, CategeryCountValue> distribution,
            FacetsConfig2 facetsConfig)
        {
            Dictionary<string, string> extraCategoryInfo = getExtraCategoryInfo(facetsConfig); // facet.extra_row_info_facet);
            var rows = Context.QueryRows(intervalQuery, dr => CreateItem(dr, distribution, extraCategoryInfo));
            return rows;
        }

        protected override FacetContent.CategoryItem CreateItem(DbDataReader dr, Dictionary<string, CategeryCountValue> distribution, Dictionary<string, string> extraCategoryInfo)
        {
            string category = dr.GetInt32(0).ToString();
            string name = dr.GetString(1) + (extraCategoryInfo.ContainsKey(category) ? extraCategoryInfo[category] : "");
            CategeryCountValue countValue = distribution.ContainsKey(category) ? distribution[category] : null;
            return new FacetContent.CategoryItem() {
                Category = category,
                DisplayName = name, // $extra = ""; //array_key_exists($id, $extraCategoryInfo) ? $extraCategoryInfo[$id] : "";
                Name = name,
                Count = countValue?.Count ?? 0,
                CategoryDetails = countValue?.Details
            };
        }

        protected List<(string,int)> getCategoryItemValue(DbDataReader row)
        {
            return new List<(string, int)>() { ("discrete", (int)row["category"]) };
        }

        protected List<Key1Value<int,string>> getExtraCategoryInfo(FacetsConfig2 facetsConfig)
        {
            FacetDefinition facet = facetsConfig.TargetFacet.extra_row_info_facet;
            if (facet == null)
                return new List<Key1Value<int, string>>();
            QuerySetup query = QueryBuilder.Build(facetsConfig, facet.FacetCode, null, facetsConfig.GetFacetCodes());
            string sql = FacetContentExtraRowInfoSqlQueryBuilder.compile(query, facet);
            var values = Context.QueryKeyValues<int,string>(sql) ?? [];
            return values;
        }
    }

    class RangeFacetContentLoader : FacetContentLoader {

        public RangeFacetContentLoader(IQueryBuilderSetting config, IUnitOfWork context, IQuerySetupBuilder builder) : base(config, context, builder)
        {
        }

        private (decimal, decimal) getLowerUpperBound(FacetConfig2 config)
        {
            var bounds = config.GetPickedLowerUpperBounds();      // Get client picked bound if exists...
            if (bounds.Count != 2) {
                bounds = config.getStorageLowerUpperBounds();     // ...else fetch from database
            }
            return (bounds[EFacetPickType.lower], bounds[EFacetPickType.upper]);
        }

        protected (int,string) compileIntervalQuery(FacetsConfig2 facetsConfig, string facetCode, int interval_count=120)
        {
            (decimal lower, decimal upper) = this.getLowerUpperBound(facetsConfig.GetConfig(facetCode));
            int interval = (int)Math.Floor((upper - lower) / interval_count);
            if (interval <= 0) {
                interval = 1;
            }
            string sql = RangeIntervalSqlQueryBuilder.compile(interval, (int)lower, (int)upper, interval_count);
            return ( interval, sql );
        }

        protected List<(string,int)> getCategoryItemValue(DbDataReader row)
        {
            return new List<(string, int)>() { ("lower", (int)row["lower"]), ("upper", (int)row["upper"]) };
        }

        protected dynamic getCategoryItemName(DbDataReader row, string extra)
        {
            return $"{(int)row["lower"]} to {(int)row["upper"]}";
        }
    }
}