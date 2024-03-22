
namespace SQT.SQL.Matcher
{
    public class DiscreteContentSqlCompilerMatcher : GenericSelectSqlMatcher
    {
        //public static string SqlRegExpr { get; }= $@"
        //        SELECT cast\((?<CategoryExpr>[\w\._]+) AS varchar\) AS category, (?<ValueExpr>[\w\._]+) AS name
        //        FROM (?<TargetSql>[\w\."",\(\)]+)(?: AS \w*)?(?<JoinSql>.*?(?= WHERE))?
        //        WHERE (?<CriteriaSql>.*)
        //        GROUP BY \1, \2.*";
        public static string SqlRegExpr { get; } = $@"
                SELECT cast\((?<CategoryExpr>[\w\._]+) AS varchar\) AS category, (?<ValueExpr>[\w\._]+) AS name
                FROM (?<TargetSql>[\w\."",\(\)]+)(?: AS \w*)?(?<JoinSql>.*?(?= WHERE))?
                WHERE(?<CriteriaSql>.*?(?= GROUP BY|ORDER BY|\s?$))
                GROUP BY(?<GroupBySql> 1, 2(?:.*?(?= ORDER BY|\s?$)))?\s? ORDER BY(?<OrderBySql>.+$)?";

        public override string ExpectedSql { get; } = SqlRegExpr;
    }
}