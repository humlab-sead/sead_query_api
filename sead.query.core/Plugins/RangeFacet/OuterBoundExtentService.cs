using System.Data;

namespace SeadQueryCore
{
    public class RangeOuterBoundExtentService(ITypedQueryProxy queryProxy) : IRangeOuterBoundExtentService
    {
        public ITypedQueryProxy QueryProxy { get; } = queryProxy;

        public (decimal, decimal) GetUpperLowerBounds(Facet facet)
        {
            string sql = new RangeOuterBoundSqlCompiler().Compile(null, facet);
            var item = QueryProxy.QueryRow(sql, r => new
            {
                lower = r.IsDBNull(0) ? 0 : r.GetDecimal(0),
                upper = r.IsDBNull(1) ? 0 : r.GetDecimal(1)
            });
            return item == null ? (0, 0) : (item.lower, item.upper);
        }

        public RangeExtent GetExtent(FacetConfig2 config, int default_interval_count = 120)
        {
            var (lower, upper) = GetUpperLowerBounds(config.Facet);   // Fetch from database
            return new RangeExtent
            {
                Lower = lower,
                Upper = upper,
                Count = default_interval_count
            };
        }
    }
}
