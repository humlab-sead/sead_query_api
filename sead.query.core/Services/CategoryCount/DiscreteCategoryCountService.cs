using SeadQueryCore.QueryBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using static SeadQueryCore.Utility;

namespace SeadQueryCore
{
    public class DiscreteCategoryCountService : CategoryCountService, IDiscreteCategoryCountService
    {

        public DiscreteCategoryCountService(
            IFacetSetting config,
            IRepositoryRegistry context,
            IQuerySetupCompiler builder,
            IDiscreteCategoryCountSqlQueryCompiler countSqlCompiler) : base(config, context, builder) {
            CountSqlCompiler = countSqlCompiler;
        }

        public IDiscreteCategoryCountSqlQueryCompiler CountSqlCompiler { get; }

        protected override string Compile(Facet facet, FacetsConfig2 facetsConfig, string payload)
        {
            Facet computeFacet = Context.Facets.Get(facet.AggregateFacetId); // default to ID 1 = "result_facet"

            string targetCode = Coalesce(facetsConfig?.TargetCode, computeFacet.FacetCode);

            Facet targetFacet = Context.Facets.GetByCode(targetCode);

            List<string> tables = GetTables(facetsConfig, targetFacet, computeFacet);
            List<string> facetCodes = facetsConfig.GetFacetCodes();

            facetCodes.MyInsertBeforeItem(targetFacet.FacetCode, computeFacet.FacetCode);

            QuerySetup query = QuerySetupBuilder.Build(facetsConfig, computeFacet, tables, facetCodes);
            string sql = CountSqlCompiler.Compile(query, targetFacet, computeFacet, Coalesce(facet.AggregateType, "count"));
            return sql;
        }

        private List<string> GetTables(FacetsConfig2 facetsConfig, Facet targetFacet, Facet computeFacet)
        {
            List<string> tables = targetFacet.Tables.Select(x => x.ResolvedAliasOrTableOrUdfName).ToList();
            if (computeFacet.FacetCode != targetFacet.FacetCode) {
                //FIXME: tables.Add(computeFacet.TargetTable?.TableOrUdfName);
                tables.AddRange(computeFacet.Tables.Select(x => x.ResolvedAliasOrTableOrUdfName));
            }
            return tables.Distinct().ToList();
        }

        private string Category2String(System.Data.Common.DbDataReader x, int ordinal)
        {
            if (x.GetDataTypeName(ordinal) == "numeric")
                return String.Format("{0:0.####}", x.GetDecimal(ordinal));
            return x.GetInt32(ordinal).ToString();
        }

        protected override List<CategoryCountItem> Query(string sql)
        {
            return Context.QueryRows<CategoryCountItem>(sql,
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
