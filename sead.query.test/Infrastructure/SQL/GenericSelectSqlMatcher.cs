using SeadQueryCore;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SQT.SQL.Matcher
{
    public class OuterSelectMatch
    {
        public bool Success;
        public string InnerSql;
        public string AggregateType;

        public SelectMatch InnerSelect;
    }

    public class SelectMatch
    {
        public bool Success;
        public string SelectFieldsSql = "";
        public string CategoryExpr;
        public string ValueExpr;
        public string TargetSql;
        public string JoinSql;
        public string CriteriaSql;
        public string GroupByFieldsSql = "";

        public JoinsMatch Join;
        public List<string> Tables;
        public List<string> Criterias;
    }

    public class JoinMatch
    {
        public bool Success;
        public string Target;
        public string Alias;
        public string JoinCriteria;
    }

    public class JoinsMatch
    {
        public bool Success;
        public List<string> Joins;
        public List<JoinMatch> Items;
    }

    public class GenericSelectSqlMatcher
    {
        public virtual string ExpectedSql { get; } = "";

        public SelectMatch Match(string sqlQuery)
        {
            return Match(sqlQuery, ExpectedSql);
        }

        public SelectMatch Match(string sqlQuery, string expectedSql)
        {
            var rx = Regex.Match(sqlQuery.Squeeze(), expectedSql.Squeeze());

            SelectMatch result = new SelectMatch { Success = rx.Success };

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

        public static JoinsMatch MatchJoins(string joinClause)
        {
            var boundries = new string[] { "FULL OUTER JOIN", "CROSS JOIN", "INNER JOIN", "LEFT JOIN", "RIGHT JOIN", "JOIN" };

            string[] joins = joinClause.Split(boundries, System.StringSplitOptions.RemoveEmptyEntries);

            // boundries.ForEach(x => { joinClause = joinClause.Replace(x, "JOIN"); });
            // var expectedJoinClause = @"(?<joins>(?:(?:INNER|LEFT|RIGHT|OUTER|CROSS)?\s?JOIN\s[\w\."",\(\)]+\s(?:AS [""\w]+ )?ON\s[\w\."",\(\)]+\s=\s[\w\."",\(\)]*\s?))*";
            //var rx = Regex.Match(joinClause.Squeeze(), expectedJoinClause);

            JoinsMatch result = new JoinsMatch { Success = true };

            //if (!rx.Success)
            //    return result;

            // result.Joins = rx.Groups[1].Captures.Select(m => m.Value.Squeeze()).ToList();
            result.Joins = joins.Select(x => x.Squeeze()).ToList();

            //var expectedJoin = @"(?:INNER|LEFT|RIGHT|OUTER)?\s?JOIN\s(?<JoinTable>[\w\."",\(\)]+)\s(?:AS (?<JoinAlias>[""\w]+\s))?ON\s(?<LeftOnExpr>[\w\."",\(\)]+)\s=\s(?<RightOnExpr>[\w\."",\(\)]+)";
            var expectedJoin = @"(?<JoinTable>[\w\."",\(\)]+)(?:AS (?<JoinAlias>[""\w]+\s))?(?: ON\s(?<JoinCriteria>.*))?";

            result.Items = new List<JoinMatch>();
            foreach (var join in result.Joins) {

                var joinItem = new JoinMatch();
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
