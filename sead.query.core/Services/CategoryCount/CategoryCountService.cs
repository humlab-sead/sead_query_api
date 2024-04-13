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
            IIndex<EFacetType, ICategoryCountSqlCompiler> sqlCompilers
            ) : QueryServiceBase(registry, builder), ICategoryCountService
    {
        public class CategoryCountData
        {
            public string SqlQuery { get; set; }
            public Dictionary<string, CategoryCountItem> CategoryCounts { get; set; }
        }

        public ITypedQueryProxy QueryProxy { get; } = queryProxy;
        public IIndex<EFacetType, ICategoryCountSqlCompiler> SqlCompilers { get; } = sqlCompilers;
        public IIndex<EFacetType, ICategoryCountHelper> Helpers { get; } = helpers;

        public IFacetSetting Config { get; } = config;

        public CategoryCountData Load(string facetCode, FacetsConfig2 facetsConfig, string intervalQuery = null, EFacetType facetTypeOverride = EFacetType.Unknown)
        {
            var facet = Facets.GetByCode(facetCode);
            var aggregateFacet = Facets.Get(facet.AggregateFacetId);
            var targetFacet = Facets.GetByCode(facetsConfig.TargetCode); // facetsConfig.TargetFacet;

            var facetType = facetTypeOverride == EFacetType.Unknown ? facet.FacetTypeId : facetTypeOverride;

            var helper = Helpers[facetType];
            var compiler = SqlCompilers[facetType];

            CompilePayload compilePayload = new CompilePayload()
            {
                ResultFacet = facet,
                TargetFacet = targetFacet,
                AggregateFacet = aggregateFacet,
                IntervalQuery = intervalQuery,
                CountColumn = Config.CountColumn,
                AggregateType = facet.AggregateType ?? "count"
            };

            var tableNames = helper.GetTables(facetsConfig, compilePayload);
            var facetCodes = helper.GetFacetCodes(facetsConfig, compilePayload);

            var querySetup = QuerySetupBuilder.Build(facetsConfig, facet, tableNames, facetCodes);
            var sqlQuery = compiler.Compile(querySetup, facet, compilePayload);

            var categoryCounts =  QueryProxy.QueryRows<CategoryCountItem>(sqlQuery,
                x => new CategoryCountItem()
                {
                    Category = helper.GetCategory(x),
                    Count = helper.GetCount(x),
                    Extent = helper.GetExtent(x)
                }).ToDictionary(z => z.Category ?? "(null)");

            return new CategoryCountData
            {
                SqlQuery = sqlQuery,
                CategoryCounts = categoryCounts
            };
        }

    }

}
