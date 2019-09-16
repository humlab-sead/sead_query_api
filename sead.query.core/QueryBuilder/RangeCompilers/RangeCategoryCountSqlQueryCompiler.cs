
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
                LEFT JOIN {query.Facet.TargetTable.ObjectName}{query.Facet.TargetTable.ObjectArgs ?? ""}  {"AS ".GlueTo(query.Facet.AliasName)}
                  ON {facet.CategoryIdExpr}::integer >= lower
                 AND {facet.CategoryIdExpr}::integer <= upper
                {query.Joins.Combine("")}
                {"AND ".GlueTo(query.Criterias.Combine(" AND "))}
                GROUP BY category, lower, upper
                ORDER BY lower";
            return sql;
        }
    }
}