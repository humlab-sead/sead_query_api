using SeadQueryCore.QueryBuilder;
using SeadQueryCore.Model.Ext;
using System.Collections.Generic;
using System.Linq;

namespace SeadQueryCore
{
    public class TabularResultSqlCompiler : IResultSqlCompiler {

        public string Compile(QueryBuilder.QuerySetup querySetup, Facet notUsed, IEnumerable<ResultSpecificationField> fields)
        {
            string sql = $@"
            SELECT {fields.GetResultCompiledValueFields().ToList().Combine(", ")}
            FROM (
                SELECT {fields.GetResultColumnNameAliasPairs().Select(x => $"{x.ColumnName} AS {x.Alias}").ToList().Combine(", ")}
                FROM {querySetup.Facet.TargetTable.ResolvedSqlJoinName}
                     {querySetup.Joins.Combine("")}
                WHERE 1 = 1
                {"AND ".GlueTo(querySetup.Criterias.Combine(" AND "))}
                GROUP BY {fields.GetResultInnerGroupByFields().ToList().Combine(", ")}
            ) AS X
            {"GROUP BY ".GlueTo(fields.GetResultGroupByFields().ToList().Combine(", "))}
            {"ORDER BY ".GlueTo(fields.GetResultSortFields().ToList().Combine(", "))}
        ";
            return sql;
        }
    }
}