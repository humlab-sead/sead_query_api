using SeadQueryCore.QueryBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using static SeadQueryCore.Utility;

namespace SeadQueryCore
{
    //public interface ICategoryCountServiceAggregate
    //{
    //    RangeCategoryCountService RangeCategoryCountService { get; set; }
    //    DiscreteCategoryCountService DiscreteCategoryCountService { get; set; }
    //}

    /// <summary>
    /// Gives the number of occurrences (Count) of a category determined by given extent
    /// The extent is a single value for discrete facets (the category ID) and an interval (lower, upper) for range facets
    /// </summary>
    public class CategoryCountItem {
        public string Category { get; set; }
        public int? Count { get; set; }
        public List<decimal> Extent;
        //public Dictionary<EFacetPickType, decimal> CategoryValues;
    }

    public interface ICategoryCountService {
        Dictionary<string, CategoryCountItem> Load(string facetCode, FacetsConfig2 facetsConfig, string intervalQuery);
    }

    public class CategoryCountService : QueryServiceBase, ICategoryCountService {

        public CategoryCountService(IQueryBuilderSetting config, IRepositoryRegistry context, IQuerySetupBuilder builder) : base(config, context, builder)
        {
        }

        public Dictionary<string, CategoryCountItem> Load(string facetCode, FacetsConfig2 facetsConfig, string intervalQuery=null)
        {
            Facet facet = Context.Facets.GetByCode(facetCode);
            string sql = Compile(facet, facetsConfig, intervalQuery);
            var values =  Query(sql).ToList();
            Dictionary<string, CategoryCountItem> data = values.ToDictionary(z => Coalesce(z.Category, "(null)"));
            return data;
        }

        protected virtual List<CategoryCountItem> Query(string sql) => throw new NotSupportedException();

        protected virtual string Compile(Facet facet, FacetsConfig2 facetsConfig, string intervalQuery) => throw new NotSupportedException();
    }

    public class RangeCategoryCountService : CategoryCountService {

        public RangeCategoryCountService(
            IQueryBuilderSetting config,
            IRepositoryRegistry context,
            IQuerySetupBuilder builder,
            IRangeCategoryCountSqlQueryCompiler rangeCountSqlCompiler
        ) : base(config, context, builder) {
            RangeCountSqlCompiler = rangeCountSqlCompiler;
        }

        private IRangeCategoryCountSqlQueryCompiler RangeCountSqlCompiler { get; }

        protected override string Compile(Facet facet, FacetsConfig2 facetsConfig, string intervalQuery)
        {
            List<string> tables = new List<string>() { facet.TargetTableName, Config.DirectCountTable };
            QuerySetup query = QuerySetupBuilder.Build(facetsConfig, facet.FacetCode, tables);
            string sql = RangeCountSqlCompiler.Compile(query, facet, intervalQuery, Config.DirectCountColumn);
            return sql;
        }

        protected override List<CategoryCountItem> Query(string sql)
        {
            return Context.QueryRows<CategoryCountItem>(sql,
                x => new CategoryCountItem() {
                    Category = x.IsDBNull(0) ? "(null)" : x.GetString(0),
                    Count = x.IsDBNull(3) ? 0 : x.GetInt32(3),
                    Extent = new List<decimal>() { x.IsDBNull(1) ? 0 : x.GetInt32(1), x.IsDBNull(2) ? 0 : x.GetInt32(2) }
                }).ToList();
        }
    }

    public class DiscreteCategoryCountService : CategoryCountService {

        public DiscreteCategoryCountService(
            IQueryBuilderSetting config,
            IRepositoryRegistry context,
            IQuerySetupBuilder builder,
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

            QuerySetup query = QuerySetupBuilder.Build(facetsConfig, computeFacet.FacetCode, tables, facetCodes);
            string sql = CountSqlCompiler.Compile(query, targetFacet, computeFacet, Coalesce(facet.AggregateType, "count"));
            return sql;
        }

        private List<string> GetTables(FacetsConfig2 facetsConfig, Facet targetFacet, Facet computeFacet)
        {
            List<string> tables = targetFacet.ExtraTables.Select(x => x.TableName).ToList();
            if (facetsConfig.TargetCode != null) {
                tables.Add(targetFacet.ResolvedName);
            }
            if (computeFacet.FacetCode != targetFacet.FacetCode) {
                tables.Add(computeFacet.TargetTableName);
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
