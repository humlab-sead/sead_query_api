
using SeadQueryCore;

namespace SQT.SQL.Matcher
{
    public class RangeIntervalSqlCompilerMatcher : GenericSelectSqlMatcher
    {
        public static string DP = @"-?[0-9]+(\.[0-9]+)?";

        public static string SqlRegExpr { get; } =
            $@"\(
                SELECT .*::text, n, n \+ {DP}
                FROM generate_series\({DP}, {DP}, {DP}\) as a\(n\)
                WHERE n < {DP}
            \)".Squeeze();

        public override string ExpectedSql => SqlRegExpr;
    }
}