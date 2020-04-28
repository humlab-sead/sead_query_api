
namespace SQT.SQL.Matcher
{
    public class DiscreteContentSqlCompilerMatcher : GenericSelectSqlMatcher
    {
        public static string SqlRegExpr { get; }= $@"
                SELECT cast\((?<CategoryExpr>[\w\._]+) AS varchar\) AS category, (?<ValueExpr>[\w\._]+) AS name
                FROM (?<TargetSql>[\w\."",\(\)]+)(?: AS \w*)?(?<JoinSql>.*)?
                WHERE (?<CriteriaSql>.*)
                GROUP BY \1, \2.*";

        public override string ExpectedSql { get; } = SqlRegExpr;

    }
}