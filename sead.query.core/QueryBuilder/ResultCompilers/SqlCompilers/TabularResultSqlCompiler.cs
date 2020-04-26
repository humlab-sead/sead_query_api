using SeadQueryCore.QueryBuilder;
using SeadQueryCore.QueryBuilder.Ext;
using System.Collections.Generic;
using System.Linq;

namespace SeadQueryCore
{
    public class TabularResultSqlCompiler : IResultSqlCompiler {
        public string Compile(QueryBuilder.QuerySetup query, Facet facet, IEnumerable<ResultAggregateField> fields)
        {
            string sql = $@"
            SELECT {fields.GetAggregateCompiledDataFields().ToList().Combine(", ")}
            FROM (
                SELECT {fields.GetAggregateColumnNameAliasPairs().Select(x => $"{x.ColumnName} AS {x.Alias}").ToList().Combine(", ")}
                FROM {query.Facet.TargetTable.ResolvedSqlJoinName}
                     {query.Joins.Combine("")}
                WHERE 1 = 1
                {"AND ".GlueTo(query.Criterias.Combine(" AND "))}
                GROUP BY {fields.GetAggregateInnerGroupByFields().ToList().Combine(", ")}
            ) AS X
            {"GROUP BY ".GlueTo(fields.GetAggregateGroupByFields().ToList().Combine(", "))}
            {"ORDER BY ".GlueTo(fields.GetAggregateSortFields().ToList().Combine(", "))}
        ";
            return sql;
        }
    }
}