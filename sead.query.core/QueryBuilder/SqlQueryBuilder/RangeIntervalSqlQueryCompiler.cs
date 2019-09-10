using System.Collections.Generic;

namespace SeadQueryCore
{
    public class RangeIntervalSqlQueryCompiler : IRangeIntervalSqlQueryCompiler
    {
        public string Compile(int interval, int min, int max, int interval_count)
        {
            List<string> pieces = new List<string>();
            //for (int i = 0, lower = min; i <= interval_count && lower <= max; i++) {
            //    int upper = lower + interval;
            //    pieces.Add($@"('{lower} => {upper}', {lower}, {upper})");
            //    lower += interval;
            //}
            //int lower = min;
            //while (lower <= max) {
            //    int upper = lower + interval;
            //    pieces.Add($@"('{lower} => {upper}', {lower}, {upper})");
            //    lower = upper;
            //}
            //string values = String.Join("\n,", pieces);
            //string sql = $"(VALUES {values})";

            string sql = $"" +
                $"(SELECT n::text || ' => ' || (n + {interval})::text, n, n + {interval} \n" +
                $" FROM generate_series({min}, {max}, {interval}) as a(n) WHERE n < {max})\n";
            return sql;
        }
    }
}