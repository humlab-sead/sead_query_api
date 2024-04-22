
namespace SQT.SQL.Matcher
{
    public class GeoPolygonCategoryCountSqlCompilerMatcher : CategoryCountSqlCompilerMatcher
    {
        public static string SqlRegExpr { get; } = $@"
                SELECT (?<CategoryExpr>[\w\._]+) AS category, 1 as count, ([\w\._]+).longitude_dd, ([\w\._]+).latitude_dd
                FROM (?<TargetSql>[\w\."",\(\)]+)(?: AS \w*)?(?<JoinSql>.*?(?= WHERE))?
                WHERE(?<CriteriaSql>.*?(?= GROUP BY|\s?$))
                GROUP BY [\w\._]+, 3, 4";

        public override string ExpectedSql { get; } = SqlRegExpr;
    }
}




