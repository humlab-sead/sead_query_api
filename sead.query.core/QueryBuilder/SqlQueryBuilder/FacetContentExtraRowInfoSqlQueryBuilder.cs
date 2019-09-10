
namespace SeadQueryCore
{
    public static class FacetContentExtraRowInfoSqlQueryBuilder {
        public static string Compile(QueryBuilder.QuerySetup query, Facet facet)
        {
            string sql = $@"
            SELECT DISTINCT id, name
            FROM (
                SELECT {facet.CategoryIdExpr} AS id, COALESCE({facet.CategoryNameExpr},'No value') AS name, {facet.SortExpr} AS sort_column
                FROM {query.Facet.TargetTableName} {"AS ".GlueTo(query.Facet.AliasName)}
                     {query.Joins.Combine("")}
                WHERE 1 = 1
                {"AND ".GlueTo(query.Criterias.Combine(" AND "))}
                GROUP BY name, id, sort_column
                ORDER BY {facet.SortExpr}
            ) AS tmp
        ";
            return sql;
        }
    }
}