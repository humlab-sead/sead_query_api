
namespace SeadQueryCore
{
    public class RangeCategoryCountSqlQueryCompiler : IRangeCategoryCountSqlQueryCompiler
    {
        public string Compile(QueryBuilder.QuerySetup query, Facet facet, string intervalQuery, string countColumn)
        {
            string sql = $@"
            WITH categories(category, lower, upper) AS ({intervalQuery})
                SELECT category, lower, upper, COUNT(DISTINCT {countColumn}) AS count_column
                FROM categories
                LEFT JOIN {query.Facet.TargetTableName} {"AS ".GlueTo(query.Facet.AliasName)}
                  ON {facet.CategoryIdExpr}::integer >= lower
                 AND {facet.CategoryIdExpr}::integer <= upper
                {query.Joins.Combine("")}
                {"AND ".GlueTo(query.Criterias.Combine(" AND "))}
                GROUP BY category, lower, upper
                ORDER BY lower";
            return sql;
            //string sql = $@"
            //SELECT category, count(category) AS count, lower, upper
            //FROM (
            //    SELECT COALESCE(lower || ' => ' || upper, 'data missing') AS category, count_column, lower, upper
            //    FROM  (
            //        SELECT lower, upper, {countColumn} AS count_column
            //        FROM {query.Facet.TargetName} {"AS ".AddIf(query.Facet.AliasName)}
            //        LEFT JOIN ( {intervalQuery} ) AS temp_interval
            //            ON {facet.CategoryIdExpr}::integer >= lower
            //            AND {facet.CategoryIdExpr}::integer < upper
            //                {query.Joins.Combine("")}
            //          {"AND ".AddIf(query.Criterias.Combine(" AND "))}
            //        GROUP BY lower, upper, {countColumn}
            //        ORDER BY lower) AS x
            //    GROUP by lower, upper, count_column) AS y
            //WHERE lower is not null
            //AND upper is not null
            //GROUP BY category, lower, upper
            //ORDER BY lower, upper";
            //return sql;
        }
    }
}