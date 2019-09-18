using System;
using System.Data.Common;
using Autofac.Features.Indexed;
using SeadQueryCore.QueryBuilder;

namespace SeadQueryCore
{
    public class RangeFacetContentService : FacetContentService {
        public RangeFacetContentService(
            IQueryBuilderSetting config,
            IRepositoryRegistry context,
            IQuerySetupBuilder builder,
            IIndex<EFacetType,
            ICategoryCountService> countServices,
            IRangeIntervalSqlQueryCompiler rangeSqlCompiler) : base(config, context, builder)
        {
            CountService = countServices[EFacetType.Range];
            RangeSqlCompiler = rangeSqlCompiler;
        }

        public IRangeIntervalSqlQueryCompiler RangeSqlCompiler { get; }

        private (decimal, decimal, int) GetLowerUpperBound(FacetConfig2 config, int default_interval_count=120)
        {
            var picks = config.GetPickValues(true);                    // Get client picked bound if exists...
            if (picks.Count >= 2) {
                return (picks[0], picks[1], (picks.Count > 2) && ((int)picks[2] > 0) ? (int)picks[2] : default_interval_count);
            }
            var bound = Context.Facets.GetUpperLowerBounds(config.Facet);     // ...else fetch from database
            return (bound.Item1, bound.Item2, default_interval_count);
        }

        protected override (int,string) CompileIntervalQuery(FacetsConfig2 facetsConfig, string facetCode, int default_interval_count=120)
        {
            (decimal lower, decimal upper, int interval_count) = GetLowerUpperBound(facetsConfig.GetConfig(facetCode), default_interval_count);
            // var width = upper - lower;
            // if (width < interval_count)
            //     interval_count = (int)width;
            int interval = Math.Max((int)Math.Floor((upper - lower) / interval_count), 1);
            string sql = RangeSqlCompiler.Compile(interval, (int)lower, (int)upper, interval_count);
            return ( interval, sql );
        }

        protected override string GetName(DbDataReader dr)
        {
            return $"{dr.GetInt32(1)} to {dr.GetInt32(2)}";
        }
    }
}