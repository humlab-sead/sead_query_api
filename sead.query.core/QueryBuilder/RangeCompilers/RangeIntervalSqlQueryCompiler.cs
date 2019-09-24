using System.Collections.Generic;

namespace SeadQueryCore
{
    public class RangeIntervalSqlQueryCompiler : IRangeIntervalSqlQueryCompiler
    {
        public string Compile(int interval, int min, int max, int interval_count)
        {
            string sql = $@"
            (
                SELECT n::text || ' => ' || (n + {interval})::text, n, n + {interval}
                FROM generate_series({min}, {max}, {interval}) as a(n)
                WHERE n < {max}
            )";

            return sql;
        }
    }
}