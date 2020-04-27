using SeadQueryCore;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SQT.SQL.Matcher
{
    public class OuterSelectClause
    {
        public bool Success;
        public string InnerSql;
        public string AggregateType;

        public SelectClause InnerSelect;
    }

    public class SelectClause
    {
        public bool Success;
        public string SelectFieldsSql = "";
        public string CategoryExpr;
        public string ValueExpr;
        public string TargetSql;
        public string JoinSql;
        public string CriteriaSql;
        public string GroupByFieldsSql = "";

        public JoinClause Join;
        public List<string> Tables;
        public List<string> Criterias;
    }

    public class JoinItem
    {
        public bool Success;
        public string Target;
        public string Alias;
        public string JoinCriteria;
    }

    public class JoinClause
    {
        public bool Success;
        public List<string> Joins;
        public List<JoinItem> Items;
    }

    public class FacetLoadSqlMatcher
    {
        public virtual string ExpectedSql { get; } = "";
        public virtual SelectClauseMatcher InnerSqlMatcher  { get; }

        public OuterSelectClause Match(string sqlQuery, string expectedOuterSql)
        {
            var rx = Regex.Match(sqlQuery.Squeeze(), expectedOuterSql.Squeeze());

            OuterSelectClause result = new OuterSelectClause
            {
                Success = rx.Success
            };

            if (!rx.Success)
                return result;

            Extract(rx, result);

            return result;
        }

        protected virtual void Extract(Match rx, OuterSelectClause result)
        {
            result.InnerSql = rx.Groups["InnerSql"].Value.Squeeze();
            result.AggregateType = rx.Groups?["AggregateType"]?.Value?.Squeeze() ?? "";

            result.InnerSelect = InnerSqlMatcher.Match(result.InnerSql);
        }

        public OuterSelectClause Match(string sqlQuery)
        {
            return Match(sqlQuery.Squeeze(), ExpectedSql);
        }

        public static FacetLoadSqlMatcher Create(EFacetType facetType)
        {
            if (facetType == EFacetType.Range)
                return new RangeLoadSqlMatcher();
            return new DiscreteLoadSqlMatcher();
        }

    }

    public class DiscreteLoadSqlMatcher : FacetLoadSqlMatcher
    {
        public class DiscreteLoadInnerSelectClauseMatcher : SelectClauseMatcher
        {
            public override string ExpectedSql { get; } =
                    @"SELECT (?<CategoryExpr>[\w\."",\(\)]+) AS category, (?<ValueExpr>[\w\."",\(\)]+) AS value
                  FROM (?<TargetSql>[\w\."",\(\)]+)(?: AS \w*)?(?<JoinSql>.*)?
                  WHERE 1 = 1\s?(?<CriteriaSql>.*)?
                  GROUP BY \1.*\2".Squeeze();
        }

        public override string ExpectedSql { get; } = 
            $@"
                SELECT category, (?<AggregateType>\w+)\(value\) AS count
                FROM \((?<InnerSql>.*)\) AS x
                GROUP BY category;
            ".Squeeze();

        public override SelectClauseMatcher InnerSqlMatcher { get; } = new DiscreteLoadInnerSelectClauseMatcher();

    }

    public class RangeLoadSqlMatcher : FacetLoadSqlMatcher
    {
        public class RangeLoadInnerSelectClauseMatcher : SelectClauseMatcher
        {
            public override string ExpectedSql { get; } =
                @"SELECT (?<CategoryExpr>category), COUNT\(DISTINCT (?<ValueExpr>[\w\."",\(\)]+)\) AS count_column
              FROM (?<TargetSql>[\w\."",\(\)]+)(?: AS \w*)?(?<JoinSql>.*)?
              WHERE TRUE\s?(?<CriteriaSql>.*)?
              GROUP BY category".Squeeze();
        }

        public static string DP = @"-?[0-9]+(\.[0-9]+)?";

        public override string ExpectedSql { get; } =
            $@"WITH categories\(category, lower, upper\) AS \( \((?:\s+\#INTERVAL-QUERY\#\s+|
                    SELECT .*::text, n, n \+ {DP}
                    FROM generate_series\({DP}, {DP}, {DP}\) as a\(n\)
                    WHERE n < {DP}
                )\) \), outerbounds\(lower, upper\) AS \(
                    SELECT MIN\(lower\), MAX\(upper\)
                    FROM categories
                \)
                    SELECT c.category, c.lower, c.upper, COALESCE\(r.count_column, 0\) as count_column
                    FROM categories c
                    LEFT JOIN \((?<InnerSql>.*)\) AS r
                      ON r.category = c.category
                    ORDER BY c.lower
            ".Squeeze();

        public override SelectClauseMatcher InnerSqlMatcher { get; } = new RangeLoadInnerSelectClauseMatcher();

    }

    public class SelectClauseMatcher
    {
        public virtual string ExpectedSql { get; } = "";

        public SelectClause Match(string sqlQuery)
        {
            return Match(sqlQuery, ExpectedSql);
        }

        public SelectClause Match(string sqlQuery, string expectedInnerSql)
        {
            var rx = Regex.Match(sqlQuery.Squeeze(), expectedInnerSql.Squeeze());

            SelectClause result = new SelectClause { Success = rx.Success };

            if (!rx.Success)
                return result;

            result.SelectFieldsSql = rx.Groups?["SelectFieldsSql"]?.Value?.Squeeze() ?? "";
            result.TargetSql = rx.Groups["TargetSql"].Value.Squeeze();
            result.JoinSql = rx.Groups["JoinSql"].Value.Squeeze();
            result.CriteriaSql = rx.Groups?["CriteriaSql"]?.Value?.Squeeze() ?? "";
            result.GroupByFieldsSql = rx.Groups?["GroupByFieldsSql"]?.Value?.Squeeze() ?? "";

            result.CategoryExpr = rx.Groups?["CategoryExpr"]?.Value?.Squeeze() ?? "";
            result.ValueExpr = rx.Groups?["ValueExpr"]?.Value?.Squeeze() ?? "";

            result.Join = MatchJoins(result.JoinSql);

            result.Tables = new List<string> { result.TargetSql }
                .Concat(result.Join.Items.Select(x => x.Target))
                .ToList();

            result.Criterias = result.CriteriaSql
                .Split("AND")
                .Select(x => x.Squeeze())
                .Where(x => !x.IsEmpty())
                .ToList();

            return result;
        }

        public static JoinClause MatchJoins(string joinClause)
        {
            var boundries = new string[] { "FULL OUTER JOIN", "CROSS JOIN", "INNER JOIN", "LEFT JOIN", "RIGHT JOIN", "JOIN" };

            string[] joins = joinClause.Split(boundries, System.StringSplitOptions.RemoveEmptyEntries);

            // boundries.ForEach(x => { joinClause = joinClause.Replace(x, "JOIN"); });
            // var expectedJoinClause = @"(?<joins>(?:(?:INNER|LEFT|RIGHT|OUTER|CROSS)?\s?JOIN\s[\w\."",\(\)]+\s(?:AS [""\w]+ )?ON\s[\w\."",\(\)]+\s=\s[\w\."",\(\)]*\s?))*";
            //var rx = Regex.Match(joinClause.Squeeze(), expectedJoinClause);

            JoinClause result = new JoinClause { Success = true };

            //if (!rx.Success)
            //    return result;

            // result.Joins = rx.Groups[1].Captures.Select(m => m.Value.Squeeze()).ToList();
            result.Joins = joins.Select(x => x.Squeeze()).ToList();

            //var expectedJoin = @"(?:INNER|LEFT|RIGHT|OUTER)?\s?JOIN\s(?<JoinTable>[\w\."",\(\)]+)\s(?:AS (?<JoinAlias>[""\w]+\s))?ON\s(?<LeftOnExpr>[\w\."",\(\)]+)\s=\s(?<RightOnExpr>[\w\."",\(\)]+)";
            var expectedJoin = @"(?<JoinTable>[\w\."",\(\)]+)(?:AS (?<JoinAlias>[""\w]+\s))?(?: ON\s(?<JoinCriteria>.*))?";

            result.Items = new List<JoinItem>();
            foreach (var join in result.Joins) {

                var joinItem = new JoinItem();
                result.Items.Add(joinItem);

                var rx = Regex.Match(join, expectedJoin);
                joinItem.Success = rx.Success;

                if (!rx.Success)
                    continue;

                joinItem.Target = rx.Groups["JoinTable"].Value.Squeeze();
                joinItem.Alias = rx.Groups?["JoinAlias"]?.Value?.Squeeze();
                joinItem.JoinCriteria = rx.Groups?["JoinCriteria"]?.Value?.Squeeze();

            }
            result.Success = result.Success && result.Items.All(z => z.Success);
            return result;
        }
    }
}
