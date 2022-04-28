using SeadQueryCore;

namespace SQT.SQL.Matcher
{
    public class TabularResultSqlCompilerMatcher : CategoryCountSqlCompilerMatcher
    {
        public class ResultInnerSqlMatcher : GenericSelectSqlMatcher
        {
            public override string ExpectedSql { get; } =
                    @"SELECT (?<SelectFieldsSql>.*?(?= FROM))
                      FROM (?<TargetSql>[\w\."",\(\)]+)(?: AS \w*)?(?<JoinSql>.*)?
                      WHERE 1 = 1\s?(?<CriteriaSql>.*)?(?:\sGROUP BY (?<GroupByFieldsSql>.*))?".Squeeze();
        }

        public override string ExpectedSql { get; } = @"
            SELECT (?<SelectItems>.*)(?= FROM \()
            FROM \((?<InnerSql>.*)(?=\) AS X GROUP BY)\) AS X
            GROUP BY (?<GroupByFields>.*)(?= ORDER)(?: ORDER BY (?<OrderByFields>.*))?";

        public override GenericSelectSqlMatcher InnerSqlMatcher { get; } = new ResultInnerSqlMatcher();
    }
}
