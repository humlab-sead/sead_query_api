using System;
using System.Data;
using System.Data.Common;
using Autofac.Features.Indexed;
using SeadQueryCore.QueryBuilder;

namespace SeadQueryCore
{
    public class RangeExtent
    {
        public decimal Lower { get; set; }
        public decimal Upper { get; set; }
        public int Count { get; set; }
    }

    public class RangeIntervalQueryInfo : FacetContent.IntervalQueryInfo
    {
        public RangeExtent FullExtent { get; set; }
    };

    public class RangeFacetContentService : FacetContentService
    {
        public RangeFacetContentService(
            IFacetSetting config,
            IRepositoryRegistry context,
            IQuerySetupBuilder builder,
            ICategoryCountService categoryCountService,
            IRangeIntervalSqlCompiler rangeIntervalSqlCompiler,
            IRangeOuterBoundExtentService outerBoundExtentService,
            ITypedQueryProxy queryProxy
        ) : base(config, context, builder, queryProxy)
        {
            CategoryCountService = categoryCountService;
            RangeIntervalSqlCompiler = rangeIntervalSqlCompiler;
            OuterBoundExtentService = outerBoundExtentService;
        }

        public IRangeIntervalSqlCompiler RangeIntervalSqlCompiler { get; }
        public IRangeOuterBoundExtentService OuterBoundExtentService { get; }

        private RangeExtent GetPickExtent(FacetConfig2 config, int default_interval_count = 120)
        {
            var picks = config.GetPickValues(true);                                  // get client picked bound if exists...

            if (picks.Count >= 2)
            {
                return new RangeExtent
                {
                    Lower = picks[0],
                    Upper = picks[1],
                    Count = ((picks.Count > 2) && ((int)picks[2] > 0)) ? (int)picks[2] : default_interval_count
                };
            }
            return null;
        }

        protected override FacetContent.IntervalQueryInfo CompileIntervalQuery(FacetsConfig2 facetsConfig, string facetCode, int default_interval_count = 120)
        {
            var facetConfig = facetsConfig.GetConfig(facetCode);
            var fullExtent = OuterBoundExtentService.GetExtent(facetConfig, default_interval_count);
            var pickExtent = GetPickExtent(facetConfig, default_interval_count) ?? fullExtent;

            var (lower, upper, interval_count) = (pickExtent.Lower, pickExtent.Upper, pickExtent.Count);

            int interval = Math.Max((int)Math.Floor((upper - lower) / interval_count), 1);

            string sql = RangeIntervalSqlCompiler.Compile(interval, (int)lower, (int)upper, interval_count);

            return new RangeIntervalQueryInfo
            {
                Count = interval,
                Query = sql,
                FullExtent = fullExtent
            };
        }

        protected override string GetName(IDataReader dr)
        {
            return $"{dr.GetInt32(1)} to {dr.GetInt32(2)}";
        }
    }
}
