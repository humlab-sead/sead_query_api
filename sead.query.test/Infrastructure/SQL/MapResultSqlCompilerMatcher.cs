namespace SQT.SQL.Matcher
{
    public class MapResultSqlCompilerMatcher : GenericSelectSqlMatcher
    {
        public static string SqlRegExpr { get; } = $@"
                SELECT DISTINCT (?<CategoryExpr>[\w\._]+) AS id_column, (?<NameExpr>[\w\._\|'\s]+) AS name, coalesce\(latitude_dd, 0\.0\) AS latitude_dd, coalesce\(longitude_dd, 0\) AS longitude_dd
                FROM (?<TargetSql>[\w\."",\(\)]+)(?: AS \w*)?(?<JoinSql>.*?(?= WHERE))?
                (?:WHERE (?<CriteriaSql>.*))?
            ";
        public override string ExpectedSql { get; } = SqlRegExpr;
    }
}
