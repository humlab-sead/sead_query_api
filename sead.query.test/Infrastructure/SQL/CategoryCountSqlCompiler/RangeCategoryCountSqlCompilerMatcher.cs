using SeadQueryCore;

namespace SQT.SQL.Matcher
{
    public class RangeCategoryCountSqlCompilerMatcher : CategoryCountSqlCompilerMatcher
    {
        public static string NumberExpr = @"-?[0-9]+(\.[0-9]+)?";
        public static string TypeExpr = @"\w+(\(\d+,\d+\))*";

        public static string OuterSqlRegExpr { get; } =
            $@"WITH categories\(category, lower, upper\) AS \( \((?:\s+\#INTERVAL-QUERY\#\s+|
                    SELECT n::text \|\| ' to ' \|\| \(n \+ [\d\.]+\)::{TypeExpr}::text, n, \(n \+ {NumberExpr}\)::{TypeExpr}
                    FROM generate_series\({NumberExpr}::{TypeExpr}, {NumberExpr}::{TypeExpr}, {NumberExpr}::{TypeExpr}\) as a\(n\)
                    WHERE n < {NumberExpr}::{TypeExpr}
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
