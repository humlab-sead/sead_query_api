
namespace SeadQueryCore
{
    public class RangeCategoryCountSqlQueryCompiler : IRangeCategoryCountSqlQueryCompiler
    {
        public string Compile(QueryBuilder.QuerySetup query, Facet facet, string intervalQuery, string countColumn)
        {
            string sql = $@"

            WITH categories(category, lower, upper) AS (
                {intervalQuery}
            )
                SELECT c.category, c.lower, c.upper, COALESCE(r.count_column, 0) as count_column
                FROM categories c
                LEFT JOIN (
                    SELECT category, COUNT(DISTINCT {countColumn}) AS count_column
                    FROM {query.Facet.TargetTable.TableOrUdfName}{query.Facet.TargetTable.UdfCallArguments ?? ""}  {"AS ".GlueTo(query.Facet.AliasName)}
                    JOIN categories
                      ON cast({facet.CategoryIdExpr} as decimal(15, 2)) between categories.lower and categories.upper
                    {query.Joins.Combine("\n\t\t\t\t")}
                    WHERE TRUE
                      { "AND ".GlueTo(query.Criterias.Combine(" AND "))}
                    GROUP BY category
                ) AS r
                  ON r.category = c.category
                ORDER BY c.lower";

            return sql;
        }
    }
}