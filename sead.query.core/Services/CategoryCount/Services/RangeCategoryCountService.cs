using SeadQueryCore.QueryBuilder;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SeadQueryCore
{
    public class RangeCategoryCountService : CategoryCountService {

        public RangeCategoryCountService(
            IFacetSetting config,
            IRepositoryRegistry context,
            IQuerySetupBuilder builder,
            IRangeCategoryCountSqlCompiler rangeCountSqlCompiler,
            ITypedQueryProxy queryProxy
        ) : base(config, context, builder, queryProxy) {
            RangeCountSqlCompiler = rangeCountSqlCompiler;
        }

        private IRangeCategoryCountSqlCompiler RangeCountSqlCompiler { get; }

        protected override string Compile(Facet facet, FacetsConfig2 facetsConfig, string intervalQuery)
        {
            List<string> tables = new List<string>() { /* facet.TargetTable.TableOrUdfName, */ Config.CountTable };
            QuerySetup querySetup = QuerySetupBuilder.Build(facetsConfig, facet, tables);
            string sql = RangeCountSqlCompiler.Compile(querySetup, facet, intervalQuery, Config.CountColumn);
            return sql;
        }

        protected override string GetCategory(IDataReader x)
            => x.IsDBNull(0) ? "(null)" : x.GetString(0);

        protected override int GetCount(IDataReader x)
            => x.IsDBNull(3) ? 0 : x.GetInt32(3);

        protected override List<decimal> GetExtent(IDataReader x)
            => new List<decimal>() { x.IsDBNull(1) ? 0 : x.GetInt32(1), x.IsDBNull(2) ? 0 : x.GetInt32(2) };
    }

}
