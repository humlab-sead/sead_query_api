using SeadQueryCore;
using System.Text.RegularExpressions;

namespace SQT.SQL.Matcher
{
    public class CategoryCountSqlCompilerMatcher
    {
        public virtual string ExpectedSql { get; } = "";
        public virtual GenericSelectSqlMatcher InnerSqlMatcher  { get; }

        public OuterSelectMatch Match(string sqlQuery, string expectedOuterSql)
        {
            var rx = Regex.Match(sqlQuery.Squeeze(), expectedOuterSql.Squeeze());

            OuterSelectMatch result = new OuterSelectMatch
            {
                Success = rx.Success
            };

            if (!rx.Success)
                return result;

            Extract(rx, result);

            return result;
        }

        protected virtual void Extract(Match rx, OuterSelectMatch result)
        {
            result.InnerSql = rx.Groups["InnerSql"].Value.Squeeze();
            result.AggregateType = rx.Groups?["AggregateType"]?.Value?.Squeeze() ?? "";

            result.InnerSelect = InnerSqlMatcher.Match(result.InnerSql);
        }

        public OuterSelectMatch Match(string sqlQuery)
        {
            return Match(sqlQuery.Squeeze(), ExpectedSql);
        }

        public static CategoryCountSqlCompilerMatcher Create(EFacetType facetType)
        {
            if (facetType == EFacetType.Range)
                return new RangeCategoryCountSqlCompilerMatcher();
            return new DiscreteCategoryCountSqlCompilerMatcher();
        }
    }
}
