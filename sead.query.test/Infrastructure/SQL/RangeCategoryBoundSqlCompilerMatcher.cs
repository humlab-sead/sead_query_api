
namespace SQT.SQL.Matcher
{
    public class RangeCategoryBoundSqlCompilerMatcher : GenericSelectSqlMatcher
    {
        public static string SqlRegExpr { get; } = $@"
                SELECT '(?<CategoryExpr>[\w\._]+)' AS facet_code, MIN\((?<ValueExpr>[\w\._]+)::real\) AS min, MAX\((?<ValueExpr>[\w\._]+)::real\) AS max
                FROM (?<TargetSql>[\w\."",\(\)]+)(?: AS \w*)?(?<JoinSql>.*?(?= WHERE))?
                (?:WHERE (?<CriteriaSql>.*))?
                ";

        public override string ExpectedSql { get; } = SqlRegExpr;
    }
}