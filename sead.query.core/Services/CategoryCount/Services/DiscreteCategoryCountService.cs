using SeadQueryCore.QueryBuilder;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SeadQueryCore
{
    public class DiscreteCategoryCountService : CategoryCountService, IDiscreteCategoryCountService
    {

        public DiscreteCategoryCountService(
            IFacetSetting config,
            IRepositoryRegistry registry,
            IQuerySetupBuilder builder,
            IDiscreteCategoryCountQueryCompiler countSqlCompiler,
            ITypedQueryProxy queryProxy) : base(config, registry, builder, queryProxy) {
            CountSqlCompiler = countSqlCompiler;
        }

        public IDiscreteCategoryCountQueryCompiler CountSqlCompiler { get; }

        protected override string Compile(Facet facet, FacetsConfig2 facetsConfig, string payload)
        {
            // FIXME: Fix confusing args, when does facet differ from targetFacet?
            var aggregateFacet = Facets.Get(facet.AggregateFacetId);
            var targetFacet = Facets.GetByCode(facetsConfig.TargetCode);
            var tableNames = GetTables(facetsConfig, targetFacet, aggregateFacet);
            var facetCodes = facetsConfig.GetFacetCodes();

            facetCodes.InsertAt(targetFacet.FacetCode, aggregateFacet.FacetCode);

            var querySetup = QuerySetupBuilder.Build(facetsConfig, aggregateFacet, tableNames, facetCodes);
            var sql = CountSqlCompiler.Compile(querySetup, targetFacet, aggregateFacet, facet.AggregateType ?? "count");
            return sql;
        }

        private List<string> GetTables(FacetsConfig2 facetsConfig, Facet targetFacet, Facet computeFacet)
        {
            return targetFacet.GetResolvedTableNames()
                .Union(computeFacet.GetResolvedTableNames())
                    .Distinct().ToList();
        }

        private string Category2String(IDataReader x, int ordinal)
        {
            if (x.GetDataTypeName(ordinal) == "numeric")
                return String.Format("{0:0.####}", x.GetDecimal(ordinal));
            return x.GetInt32(ordinal).ToString();
        }

        protected override string GetCategory(IDataReader x)
            => x.IsDBNull(0) ? null : Category2String(x, 0);

        protected override int GetCount(IDataReader x)
            => x.IsDBNull(1) ? 0 : x.GetInt32(1);

        protected override List<decimal> GetExtent(IDataReader x)
            => new List<decimal>() { x.IsDBNull(1) ? 0 : x.GetInt32(1) };
    }
}
