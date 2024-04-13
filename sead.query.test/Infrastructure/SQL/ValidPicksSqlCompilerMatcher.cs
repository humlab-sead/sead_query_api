namespace SQT.SQL.Matcher
{
    public class ValidPicksSqlCompilerMatcher : GenericSelectSqlMatcher
    {
        public static string SqlRegExpr { get; } = $@"
            SELECT DISTINCT pick_id, (?<CategoryExpr>[\w\._]+) AS name
            FROM (?<TargetSql>[\w\."",\(\)]+)(?: AS \w*)? ?(INNER JOIN [\w\d._]* ON [\w\d._]* = [\w\d._]* ?)*
            JOIN \(VALUES \((?:'[\w\d]*'::text)\)\) AS x\(pick_id\)
              ON x.pick_id = (?:[\w\d\._]+)::text
             (?<JoinSql>.*?(?=WHERE))?WHERE (?<CriteriaSql>.*)
        ";

        public override string ExpectedSql { get; } = SqlRegExpr;
    }
}
