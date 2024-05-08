using SeadQueryCore;

namespace SQT.SQL.Matcher
{
    public class IntersectCategoryCountSqlCompilerMatcher : CategoryCountSqlCompilerMatcher
    {
        public static string NumberExpr = @"-?[0-9]+(\.[0-9]+)?";
        public static string TypeExpr = @"[\w\d]+(\(\d+,\d+\))*";

        public static string OuterSqlRegExpr { get; } =
            $@"WITH categories\(category, category_range\) AS \((?:\s*\#INTERVAL-QUERY\#\s*|
                    SELECT n::text \|\| ' to ' \|\| \(n \+ {NumberExpr}::{TypeExpr}\)::text, {TypeExpr}\(n, \(n \+ {NumberExpr}::{TypeExpr}\)\)
                    FROM generate_series\({NumberExpr}::{TypeExpr}, {NumberExpr}::{TypeExpr}, {NumberExpr}::{TypeExpr}\) as a\(n\)
                    WHERE n < {NumberExpr}
                )\)
                    SELECT c.category, lower\(c.category_range\)::{TypeExpr}, upper\(c.category_range\)::{TypeExpr}, COALESCE\(r.count_column, 0\) as count_column
                    FROM categories c
                    LEFT JOIN \((?<InnerSql>.*)\) AS r
                      ON r.category = c.category
                    ORDER BY c.category_range
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
