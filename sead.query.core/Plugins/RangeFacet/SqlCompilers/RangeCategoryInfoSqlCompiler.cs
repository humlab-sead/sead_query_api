using System.Collections.Generic;
using System.Data;

namespace SeadQueryCore
{
    public class RangeCategoryInfoSqlCompiler : IRangeCategoryInfoSqlCompiler
    {
        public virtual string Compile(int interval, int min, int max, int interval_count)
        {
            string sql = $@"
            (
                SELECT n::text || ' => ' || (n + {interval})::text, n, n + {interval}
                FROM generate_series({min}, {max}, {interval}) as a(n)
                WHERE n < {max}
            )";

            return sql;
        }

        public CategoryItem ToItem(IDataReader dr)
        {
            return new CategoryItem()
            {
                Category = dr.GetString(0),
                Count = null,
                Extent = [dr.IsDBNull(1) ? 0 : dr.GetInt32(1), dr.IsDBNull(2) ? 0 : dr.GetInt32(2)],
                Name = dr.GetString(0),
            };
        }
    }
}
