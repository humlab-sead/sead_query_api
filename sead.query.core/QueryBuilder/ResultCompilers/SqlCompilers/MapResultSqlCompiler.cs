using System.Collections.Generic;

namespace SeadQueryCore
{
    public class MapResultSqlCompiler : IResultSqlCompiler {
        public string Compile(QueryBuilder.QuerySetup query, Facet facet, IEnumerable<ResultAggregateField> fields)
        {
            string sql = $@"
            SELECT DISTINCT {facet.CategoryIdExpr} AS id_column, {facet.CategoryNameExpr} AS name, coalesce(latitude_dd, 0.0) AS latitude_dd, coalesce(longitude_dd, 0) AS longitude_dd
            FROM {query.Facet.TargetTable.ResolvedSqlJoinName}
                 {query.Joins.Combine("")}
            WHERE 1 = 1
            {"AND ".GlueTo(query.Criterias.Combine(" AND "))}
        ";
            return sql;
        }
    }

}