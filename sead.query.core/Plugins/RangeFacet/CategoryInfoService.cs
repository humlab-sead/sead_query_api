using System;
using SeadQueryCore.Services.Result;

namespace SeadQueryCore
{
    // FIXME: Make FacetContent.CategoryInfo more generic using e.g. a dictionary
    // Also similar to CategeryItem
    public class RangeExtent
    {
        public decimal Lower { get; set; }
        public decimal Upper { get; set; }
        public int Count { get; set; }
    }

    public class RangeCategoryInfo : FacetContent.CategoryInfo
    {
        public RangeExtent FullExtent { get; set; }
    };

    public class RangeCategoryInfoService(
        IRangeCategoryInfoSqlCompiler categoryInfoSqlCompiler,
        IRangeOuterBoundExtentService outerBoundExtentService
        ) : ICategoryInfoService
    {
        public IRangeCategoryInfoSqlCompiler CategoryInfoSqlCompiler { get; } = categoryInfoSqlCompiler;
        public IRangeOuterBoundExtentService OuterBoundExtentService { get; } = outerBoundExtentService;
        public ICategoryInfoSqlCompiler SqlCompiler => CategoryInfoSqlCompiler;

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

        public FacetContent.CategoryInfo GetCategoryInfo(FacetsConfig2 facetsConfig, string facetCode, dynamic payload = null)
        {
            var default_interval_count = (int)(payload ?? 120);
            var facetConfig = facetsConfig.GetConfig(facetCode);
            var fullExtent = OuterBoundExtentService.GetExtent(facetConfig, default_interval_count);
            var pickExtent = GetPickExtent(facetConfig, default_interval_count) ?? fullExtent;

            var (lower, upper, interval_count) = (pickExtent.Lower, pickExtent.Upper, pickExtent.Count);
            int interval = Math.Max((int)Math.Floor((upper - lower) / interval_count), 1);

            dynamic compile_payload = new
            {
                Lower = pickExtent.Lower,
                Upper = pickExtent.Upper,
                Interval = Math.Max((int)Math.Floor((pickExtent.Upper - pickExtent.Lower) / pickExtent.Count), 1),
                IntervalCount = pickExtent.Count
            };

            string sql = CategoryInfoSqlCompiler.Compile(null, null, compile_payload);

            return new RangeCategoryInfo
            {
                Count = interval,
                Query = sql,
                FullExtent = fullExtent
            };
        }
    }
}
