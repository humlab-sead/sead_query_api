using SeadQueryCore.QueryBuilder;

namespace SeadQueryCore
{
    public class MapResultSqlQueryBuilder : IResultSqlQueryCompiler {
        // FIXME Use ResultQuerySetup to build query. If possible, merge TabularResultSqlQueryBuilder & MapResultSqlQueryBuilder
        public string Compile(QueryBuilder.QuerySetup query, Facet facet, ResultQuerySetup config)
        {
            string sql = $@"
            SELECT DISTINCT {facet.CategoryIdExpr} AS id_column, {facet.CategoryNameExpr} AS name, coalesce(latitude_dd, 0.0) AS latitude_dd, coalesce(longitude_dd, 0) AS longitude_dd
            FROM {query.Facet.TargetTableName} {"AS ".GlueTo(query.Facet.AliasName)}
                 {query.Joins.Combine("")}
            WHERE 1 = 1
            {"AND ".GlueTo(query.Criterias.Combine(" AND "))}
        ";
            return sql;
        }
    }

}