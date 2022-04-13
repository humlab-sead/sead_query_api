using System.Collections.Generic;
using System.Diagnostics;

namespace SeadQueryCore
{
    public class MapResultSqlCompiler : IResultSqlCompiler
    {
        // FIXME Check if facet can be refactored away
        public string Compile(QueryBuilder.QuerySetup querySetup, Facet facet, IEnumerable<ResultSpecificationField> fields)
        {

            Debug.Assert(querySetup.Facet.FacetCode.Equals(facet.FacetCode), "Refactor check: Refactor away facet");

            string sql = $@"
            SELECT DISTINCT {facet.CategoryIdExpr} AS id_column, {facet.CategoryNameExpr} AS name, coalesce(latitude_dd, 0.0) AS latitude_dd, coalesce(longitude_dd, 0) AS longitude_dd
            FROM {querySetup.Facet.TargetTable.ResolvedSqlJoinName}
                 {querySetup.Joins.Combine("")}
            WHERE 1 = 1
            {"AND ".GlueTo(querySetup.Criterias.Combine(" AND "))}
        ";
            return sql;
        }
    }
}