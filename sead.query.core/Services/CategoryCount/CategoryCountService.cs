using Autofac.Features.Indexed;
using Microsoft.EntityFrameworkCore;
using SeadQueryCore.QueryBuilder;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SeadQueryCore
{

    public class CategoryCountService(
            IFacetSetting config,
            IRepositoryRegistry registry,
            IQuerySetupBuilder builder,
            ITypedQueryProxy queryProxy,
            IIndex<EFacetType, ICategoryCountHelper> helpers,
            IIndex<EFacetType, ICategoryCountSqlCompiler> sqlCompilers,
            IIndex<EFacetType, ICategoryInfoService> infoServices
            ) : QueryServiceBase(registry, builder), ICategoryCountService
    {
        public class CategoryCountData
        {
            public FacetContent.CategoryInfo CategoryInfo { get; set; }
            public string SqlQuery { get; set; }
            public Dictionary<string, CategoryItem> CategoryCounts { get; set; }
            public List<CategoryItem> OuterCategoryCounts { get; set; }
        }

        public ITypedQueryProxy QueryProxy { get; } = queryProxy;
        public IIndex<EFacetType, ICategoryCountSqlCompiler> SqlCompilers { get; } = sqlCompilers;
        public IIndex<EFacetType, ICategoryCountHelper> Helpers { get; } = helpers;
        public IIndex<EFacetType, ICategoryInfoService> CategoryInfoServices { get; } = infoServices;

        public IFacetSetting Config { get; } = config;

        public CategoryCountData Load(string facetCode, FacetsConfig2 facetsConfig, EFacetType facetTypeOverride = EFacetType.Unknown)
        {
            // Note:
            //  Facet load: Facet is same as target facet
            // Result load: Facet is result facet

            var facet = Facets.GetByCode(facetCode);
            var aggregateFacet = Facets.Get(facet.AggregateFacetId) ?? facet;

            var targetFacet = Facets.GetByCode(facetsConfig.TargetCode); // facetsConfig.TargetFacet;

            var facetType = facetTypeOverride == EFacetType.Unknown ? facet.FacetTypeId : facetTypeOverride;

            var helper = Helpers[facetType];
            var compiler = SqlCompilers[facetType];
            var infoService = CategoryInfoServices[facetType];

            var categoryInfo = infoService.GetCategoryInfo(facetsConfig, facetsConfig.TargetCode, null);

            CompilePayload compilePayload = new CompilePayload()
            {
                ResultFacet = facet,
                TargetFacet = targetFacet,
                AggregateFacet = aggregateFacet,
                IntervalQuery = categoryInfo.Query,
                CountColumn = Config.CountColumn,
                AggregateType = facet.AggregateType ?? "count"
            };

            var extraTableNames = helper.GetTables(compilePayload);
            var facetCodes = helper.GetFacetCodes(facetsConfig, compilePayload);

            var querySetup = QuerySetupBuilder.Build(facetsConfig, facet, extraTableNames, facetCodes);
            var sqlQuery = compiler.Compile(querySetup, facet, compilePayload);

            var categoryCounts = QueryProxy.QueryRows(sqlQuery, compiler.ToItem).ToDictionary(z => z.Category ?? "(null)");

            var outerCategoryCounts = QueryProxy.QueryRows(categoryInfo.Query, infoService.SqlCompiler.ToItem).ToList<CategoryItem>();

            foreach (var item in outerCategoryCounts)
                if (categoryCounts.TryGetValue(item.Category, out var value))
                    item.Count = value.Count;

            return new CategoryCountData
            {
                CategoryInfo = categoryInfo,
                SqlQuery = sqlQuery,
                CategoryCounts = categoryCounts,
                OuterCategoryCounts = outerCategoryCounts
            };
        }

    }

}
