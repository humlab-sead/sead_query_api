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
            IRangeCategoryCountSqlQueryCompiler rangeCountSqlCompiler,
            IDatabaseQueryProxy queryProxy
        ) : base(config, context, builder) {
            RangeCountSqlCompiler = rangeCountSqlCompiler;
            QueryProxy = queryProxy;
        }

        private IRangeCategoryCountSqlQueryCompiler RangeCountSqlCompiler { get; }
        public IDatabaseQueryProxy QueryProxy { get; }

        protected override string Compile(Facet facet, FacetsConfig2 facetsConfig, string intervalQuery)
        {
            List<string> tables = new List<string>() { /* facet.TargetTable.TableOrUdfName, */ Config.CountTable };
            QuerySetup querySetup = QuerySetupBuilder.Build(facetsConfig, facet, tables);
            string sql = RangeCountSqlCompiler.Compile(querySetup, facet, intervalQuery, Config.CountColumn);
            return sql;
        }

        protected override List<CategoryCountItem> Query(string sql)
        {
            return QueryProxy.QueryRows<CategoryCountItem>(sql,
                x => new CategoryCountItem() {
                    Category = x.IsDBNull(0) ? "(null)" : x.GetString(0),
                    Count = x.IsDBNull(3) ? 0 : x.GetInt32(3),
                    Extent = new List<decimal>() { x.IsDBNull(1) ? 0 : x.GetInt32(1), x.IsDBNull(2) ? 0 : x.GetInt32(2) }
                }).ToList();
        }
    }

}
