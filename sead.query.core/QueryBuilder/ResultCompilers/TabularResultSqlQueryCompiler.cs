using SeadQueryCore.QueryBuilder;
using System.Linq;

namespace SeadQueryCore
{
    public class TabularResultSqlQueryCompiler : IResultSqlQueryCompiler {
        public string Compile(QueryBuilder.QuerySetup query, Facet facet, ResultQuerySetup config)
        {
            string sql = $@"
            SELECT {config.DataFields.Combine(", ")}
            FROM (
                SELECT {config.AliasPairs.Select(x => $"{x.Item1} AS {x.Item2}").ToList().Combine(", ")}
                FROM {query.Facet.TargetTable.ResolvedSqlJoinName}
                     {query.Joins.Combine("")}
                WHERE 1 = 1
                {"AND ".GlueTo(query.Criterias.Combine(" AND "))}
                GROUP BY {config.InnerGroupByFields.Combine(", ")}
            ) AS X
            {"GROUP BY ".GlueTo(config.GroupByFields.Combine(", "))}
            {"ORDER BY ".GlueTo(config.SortFields.Combine(", "))}
        ";
            return sql;
        }
    }
}