
namespace SQT.SQL.Matcher
{
    public class GeoPolygonCategoryCountSqlCompilerMatcher : CategoryCountSqlCompilerMatcher
    {
        public static string SqlRegExpr { get; } = $@"
                SELECT [\w_]+.[\w_]+ AS category, 1 as count, COALESCE\([\w_]+.longitude_dd,0\), COALESCE\([\w_]+.latitude_dd,0\)
                FROM (?<TargetSql>[\w\.,\(\)]+)(?: AS \w*)?(?<JoinSql>.*?(?= WHERE))?
                WHERE(?<CriteriaSql>.*?(?= GROUP BY|\s?$))
                GROUP BY [\w\._]+, 3, 4";

        public override string ExpectedSql { get; } = SqlRegExpr;
    }
}




