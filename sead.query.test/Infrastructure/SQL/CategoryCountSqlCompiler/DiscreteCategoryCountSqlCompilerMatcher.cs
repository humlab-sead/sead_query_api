using SeadQueryCore;

namespace SQT.SQL.Matcher
{
    public class DiscreteCategoryCountSqlCompilerMatcher : CategoryCountSqlCompilerMatcher
    {
        public static string OuterSqlRegExpr { get; } =
           $@"
                SELECT category, (?<AggregateType>\w+)\(value\) AS count
                FROM \((?<InnerSql>.*)\) AS x
                GROUP BY category;
            ".Squeeze();

        public static string InnerSqlRegExpr { get; } =
            @"
                SELECT (?<CategoryExpr>[\w\."",\(\)]+) AS category, (?<ValueExpr>[\w\."",\(\)]+) AS value
                FROM (?<TargetSql>[\w\."",\(\)]+)(?: AS \w*)?(?<JoinSql>.*)?
                WHERE 1 = 1\s?(?<CriteriaSql>.*)?
                GROUP BY \1.*\2
            ".Squeeze();

        public class InnerSelectClauseMatcher : GenericSelectSqlMatcher
        {
            public override string ExpectedSql { get; } = InnerSqlRegExpr;
        }

        public override string ExpectedSql { get; } = OuterSqlRegExpr;

        public override GenericSelectSqlMatcher InnerSqlMatcher { get; } = new InnerSelectClauseMatcher();
    }
}
