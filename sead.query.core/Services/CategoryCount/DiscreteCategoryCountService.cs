using SeadQueryCore.QueryBuilder;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using static SeadQueryCore.Utility;

namespace SeadQueryCore
{
    public class DiscreteCategoryCountService : CategoryCountService, IDiscreteCategoryCountService
    {

        public DiscreteCategoryCountService(
            IFacetSetting config,
            IRepositoryRegistry registry,
            IQuerySetupCompiler builder,
            IDiscreteCategoryCountSqlQueryCompiler countSqlCompiler,
            ITypedQueryProxy queryProxy) : base(config, registry, builder) {
            CountSqlCompiler = countSqlCompiler;
            QueryProxy = queryProxy;
        }

        public IDiscreteCategoryCountSqlQueryCompiler CountSqlCompiler { get; }
        public ITypedQueryProxy QueryProxy { get; }

        protected override string Compile(Facet facet, FacetsConfig2 facetsConfig, string payload)
        {
            Facet computeFacet = Repository.Get(facet.AggregateFacetId);
            Facet targetFacet  = Repository.GetByCode(facetsConfig.TargetCode);

            List<string> tables     = GetTables(facetsConfig, targetFacet, computeFacet);
            List<string> facetCodes = facetsConfig.GetFacetCodes();

            facetCodes.InsertAt(targetFacet.FacetCode, computeFacet.FacetCode);

            QuerySetup query = QuerySetupBuilder.Build(facetsConfig, computeFacet, tables, facetCodes);
            string sql = CountSqlCompiler.Compile(query, targetFacet, computeFacet, Coalesce(facet.AggregateType, "count"));
            return sql;
        }

        private List<string> GetTables(FacetsConfig2 facetsConfig, Facet targetFacet, Facet computeFacet)
        {
            List<string> tables = targetFacet.Tables.Select(x => x.ResolvedAliasOrTableOrUdfName).ToList();
            if (computeFacet.FacetCode != targetFacet.FacetCode) {
                tables.AddRange(computeFacet.Tables.Select(x => x.ResolvedAliasOrTableOrUdfName));
            }
            return tables.Distinct().ToList();
        }

        private string Category2String(IDataReader x, int ordinal)
        {
            if (x.GetDataTypeName(ordinal) == "numeric")
                return String.Format("{0:0.####}", x.GetDecimal(ordinal));
            return x.GetInt32(ordinal).ToString();
        }

        protected override List<CategoryCountItem> Query(string sql)
        {
            return QueryProxy.QueryRows<CategoryCountItem>(sql,
                x => new CategoryCountItem() {
                    Category =  x.IsDBNull(0) ? null : Category2String(x, 0),
                    Count = x.IsDBNull(1) ? 0 : x.GetInt32(1),
                    Extent = new List<decimal>() { x.IsDBNull(1) ? 0 : x.GetInt32(1) }
                    //CategoryValues = new Dictionary<EFacetPickType, decimal>() {
                    //    { EFacetPickType.discrete, x.IsDBNull(1) ? 0 : x.GetInt32(1) }
                    //}
                }).ToList();
        }
    }
}
