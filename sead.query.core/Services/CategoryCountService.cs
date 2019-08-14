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
            FacetDefinition facet = Context.Facets.GetByCode(facetCode);
            string sql = Compile(facet, facetsConfig, intervalQuery);
            var values =  Query(sql).ToList();
            Dictionary<string, CategoryCountItem> data = values.ToDictionary(z => Coalesce(z.Category, "(null)"));
            return data;
        }

        protected virtual List<CategoryCountItem> Query(string sql) => throw new NotSupportedException();

        protected virtual string Compile(FacetDefinition facet, FacetsConfig2 facetsConfig, string intervalQuery) => throw new NotSupportedException();
    }

    public class RangeCategoryCountService : CategoryCountService {

        public RangeCategoryCountService(IQueryBuilderSetting config, IRepositoryRegistry context, IQuerySetupBuilder builder) : base(config, context, builder) { }

        protected override string Compile(FacetDefinition facet, FacetsConfig2 facetsConfig, string intervalQuery)
        {
            List<string> tables = new List<string>() { facet.TargetTableName, Config.DirectCountTable };
            QuerySetup query = QueryBuilder.Build(facetsConfig, facet.FacetCode, tables);
            string sql = RangeCounterSqlQueryBuilder.Compile(query, facet, intervalQuery, Config.DirectCountColumn);
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

        public DiscreteCategoryCountService(IQueryBuilderSetting config, IRepositoryRegistry context, IQuerySetupBuilder builder) : base(config, context, builder) { }

        protected override string Compile(FacetDefinition facet, FacetsConfig2 facetsConfig, string payload)
        {
            FacetDefinition computeFacet = Context.Facets.Get(facet.AggregateFacetId); // default to ID 1 = "result_facet"

            string targetCode = Coalesce(facetsConfig?.TargetCode, computeFacet.FacetCode);

            FacetDefinition targetFacet = Context.Facets.GetByCode(targetCode);

            List<string> tables = GetTables(facetsConfig, targetFacet, computeFacet);
            List<string> facetCodes = facetsConfig.GetFacetCodes();

            facetCodes.MyInsertBeforeItem(targetFacet.FacetCode, computeFacet.FacetCode);

            QuerySetup query = QueryBuilder.Build(facetsConfig, computeFacet.FacetCode, tables, facetCodes);
            string sql = DiscreteCounterSqlQueryBuilder.Compile(query, targetFacet, computeFacet, Coalesce(facet.AggregateType, "count"));
            return sql;
        }

        private List<string> GetTables(FacetsConfig2 facetsConfig, FacetDefinition targetFacet, FacetDefinition computeFacet)
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
