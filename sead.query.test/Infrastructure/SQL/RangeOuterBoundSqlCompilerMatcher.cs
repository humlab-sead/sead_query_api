
namespace SQT.SQL.Matcher
{
    public class RangeOuterBoundSqlCompilerMatcher : GenericSelectSqlMatcher
    {
        public static string SqlRegExpr { get; } = $@"
            SELECT MIN\((?<CategoryExpr>[\w\._]+)\) AS lower, MAX\((?<CategoryExpr>[\w\._]+)\) AS upper
            FROM (?<TargetSql>[\w\."",\(\)]+)
        ";

        public override string ExpectedSql { get; } = SqlRegExpr;
    }
}