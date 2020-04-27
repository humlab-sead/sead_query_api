
using SeadQueryCore;

namespace SQT.SQL.RegularExpressions
{
    public static class RangeCategoryCountSqlCompiler
    {
        public static string DP = @"-?[0-9]+(\.[0-9]+)?";

        public static string SqlRegExpr { get; } =
            $@"WITH categories\(category, lower, upper\) AS \( \((?:\s+\#INTERVAL-QUERY\#\s+|
                    SELECT .*::text, n, n \+ {DP}
                    FROM generate_series\({DP}, {DP}, {DP}\) as a\(n\)
                    WHERE n < {DP}
                )\) \), outerbounds\(lower, upper\) AS \(
                    SELECT MIN\(lower\), MAX\(upper\)
                    FROM categories
                \)
                    SELECT c.category, c.lower, c.upper, COALESCE\(r.count_column, 0\) as count_column
                    FROM categories c
                    LEFT JOIN \((?<InnerSql>.*)\) AS r
                      ON r.category = c.category
                    ORDER BY c.lower
            ".Squeeze();

    }
}