using System.Collections.Generic;
using System.Linq;
using SeadQueryCore.Model.Ext;

namespace SeadQueryCore
{
    public class TabularResultSqlCompiler : IResultSqlCompiler
    {
        string IResultSqlCompiler.ViewTypeId => "tabular";

        public string Compile(QueryBuilder.QuerySetup querySetup, Facet notUsed, IEnumerable<ResultSpecificationField> fields)
        {
            string sql =
                $@"
            {querySetup.LeadingSql}
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
