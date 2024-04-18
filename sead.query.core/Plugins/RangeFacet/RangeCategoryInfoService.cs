﻿using System;

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

    public class RangeCategoryInfoService : ICategoryInfoService
    {
        public RangeCategoryInfoService(
            IRangeCategoryInfoSqlCompiler rangeIntervalSqlCompiler,
            IRangeOuterBoundExtentService outerBoundExtentService
        )
        {
            RangeIntervalSqlCompiler = rangeIntervalSqlCompiler;
            OuterBoundExtentService = outerBoundExtentService;
        }

        public IRangeCategoryInfoSqlCompiler RangeIntervalSqlCompiler { get; }
        public IRangeOuterBoundExtentService OuterBoundExtentService { get; }

        public ICategoryInfoSqlCompiler SqlCompiler => RangeIntervalSqlCompiler;

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

        public FacetContent.CategoryInfo GetCategoryInfo(FacetsConfig2 facetsConfig, string facetCode, int default_interval_count = 120)
        {
            var facetConfig = facetsConfig.GetConfig(facetCode);
            var fullExtent = OuterBoundExtentService.GetExtent(facetConfig, default_interval_count);
            var pickExtent = GetPickExtent(facetConfig, default_interval_count) ?? fullExtent;

            var (lower, upper, interval_count) = (pickExtent.Lower, pickExtent.Upper, pickExtent.Count);

            int interval = Math.Max((int)Math.Floor((upper - lower) / interval_count), 1);

            string sql = RangeIntervalSqlCompiler.Compile(interval, (int)lower, (int)upper, interval_count);

            return new RangeCategoryInfo
            {
                Count = interval,
                Query = sql,
                FullExtent = fullExtent
            };
        }
    }
}
