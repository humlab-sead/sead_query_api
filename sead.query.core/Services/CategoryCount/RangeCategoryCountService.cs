using SeadQueryCore.QueryBuilder;
using System.Collections.Generic;
using System.Linq;

namespace SeadQueryCore
{
    public class RangeCategoryCountService : CategoryCountService {

        public RangeCategoryCountService(
            IFacetSetting config,
            IRepositoryRegistry context,
            IQuerySetupCompiler builder,
            IRangeCategoryCountSqlQueryCompiler rangeCountSqlCompiler
        ) : base(config, context, builder) {
            RangeCountSqlCompiler = rangeCountSqlCompiler;
        }

        private IRangeCategoryCountSqlQueryCompiler RangeCountSqlCompiler { get; }

        protected override string Compile(Facet facet, FacetsConfig2 facetsConfig, string intervalQuery)
        {
            List<string> tables = new List<string>() { /* facet.TargetTable.ObjectName, */ Config.DirectCountTable };
            QuerySetup querySetup = QuerySetupBuilder.Build(facetsConfig, facet, tables);
            string sql = RangeCountSqlCompiler.Compile(querySetup, facet, intervalQuery, Config.DirectCountColumn);
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

}
