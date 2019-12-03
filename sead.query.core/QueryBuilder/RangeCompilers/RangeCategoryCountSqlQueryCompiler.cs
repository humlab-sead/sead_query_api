
namespace SeadQueryCore
{
    public class RangeCategoryCountSqlQueryCompiler : IRangeCategoryCountSqlQueryCompiler
    {
        public string Compile(QueryBuilder.QuerySetup query, Facet facet, string intervalQuery, string countColumn)
        {
            string sql = $@"

            WITH categories(category, lower, upper) AS (
                {intervalQuery}
            ), outerbounds(lower, upper) AS (
                SELECT MIN(lower), MAX(upper)
                FROM categories
            )
                SELECT c.category, c.lower, c.upper, COALESCE(r.count_column, 0) as count_column
                FROM categories c
                LEFT JOIN (
                    SELECT category, COUNT(DISTINCT {countColumn}) AS count_column
                    FROM {query.Facet.TargetTable.ResolvedSqlJoinName}
                    CROSS JOIN outerbounds
                    JOIN categories
		              ON categories.lower <= cast({facet.CategoryIdExpr} as decimal(15, 6))
		             AND categories.upper >= cast({facet.CategoryIdExpr} as decimal(15, 6))
		             AND (NOT (categories.upper < outerbounds.upper AND cast({facet.CategoryIdExpr} as decimal(15, 6)) = categories.upper))
                    {query.Joins.Combine("\n\t\t\t\t\t")}
                    WHERE TRUE
                      { "AND ".GlueTo(query.Criterias.Combine(" AND ")) }
                    GROUP BY category
                ) AS r
                  ON r.category = c.category
                ORDER BY c.lower";

            return sql;
        }
    }
}