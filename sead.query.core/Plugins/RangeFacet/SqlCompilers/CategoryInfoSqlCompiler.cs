using System;
using System.Collections.Generic;
using System.Data;

namespace SeadQueryCore
{
    public class RangeCategoryInfoSqlCompiler : IRangeCategoryInfoSqlCompiler
    {
        public virtual string Compile(QueryBuilder.QuerySetup query, Facet facet, dynamic payload)
        {
            var range = (Interval)payload;
            string sql = $@"
            (
                SELECT n::text || ' => ' || (n + {range.Width})::text, n, n + {range.Width}
                FROM generate_series({range.Lower}, {range.Upper}, {range.Width}) as a(n)
                WHERE n < {range.Upper}
            )";

            return sql;
        }

        public CategoryItem ToItem(IDataReader dr)
        {
            return new CategoryItem()
            {
                Category = dr.GetString(0),
                Count = null,
                Extent = [dr.IsDBNull(1) ? 0 : dr.GetDecimal(1), dr.IsDBNull(2) ? 0 : dr.GetDecimal(2)],
                Name = dr.GetString(0),
            };
        }
    }
}
