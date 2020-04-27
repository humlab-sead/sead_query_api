using SeadQueryCore;

namespace SQT.SQL.Matcher
{
    public class RangeCategoryCountSqlCompilerMatcher : CategoryCountSqlCompilerMatcher
    {
        public static string DP = @"-?[0-9]+(\.[0-9]+)?";

        public static string OuterSqlRegExpr { get; } =
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

        public static string InnerSqlRegExpr { get; } =
            @"SELECT (?<CategoryExpr>category), COUNT\(DISTINCT (?<ValueExpr>[\w\."",\(\)]+)\) AS count_column
              FROM (?<TargetSql>[\w\."",\(\)]+)(?: AS \w*)?(?<JoinSql>.*)?
              WHERE TRUE\s?(?<CriteriaSql>.*)?
              GROUP BY category
            ".Squeeze();

        public class RangeLoadInnerSelectClauseMatcher : GenericSelectSqlMatcher
        {
            public override string ExpectedSql { get; } = InnerSqlRegExpr;
        }

        public override string ExpectedSql { get; } = OuterSqlRegExpr;

        public override GenericSelectSqlMatcher InnerSqlMatcher { get; } = new RangeLoadInnerSelectClauseMatcher();

    }

}
