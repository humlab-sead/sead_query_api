
using System.Data;

namespace SeadQueryCore.Plugin.Intersect;

public class IntersectCategoryCountSqlCompiler : IIntersectCategoryCountSqlCompiler
{
    public string Compile(QueryBuilder.QuerySetup query, Facet facet, CompilePayload payload)
    {
        string sql = $@"
        WITH categories(category, category_range) AS (
            {payload.IntervalQuery}
        )
            SELECT c.category, lower(c.category_range), upper(c.category_range), COALESCE(r.count_column, 0) as count_column
            FROM categories c
            LEFT JOIN (
                SELECT category, COUNT(DISTINCT {payload.CountColumn}) AS count_column
                FROM {query.Facet.TargetTable.ResolvedSqlJoinName}
                JOIN categories
                  ON categories.category_range {facet.CategoryIdOperator} {facet.CategoryIdExpr}::{facet.CategoryIdType}
                {query.Joins.Combine("\n\t\t\t\t\t")}
                WHERE TRUE
                    { "AND ".GlueTo(query.Criterias.Combine(" AND ")) }
                GROUP BY category
            ) AS r
                ON r.category = c.category
            ORDER BY c.category_range";

        return sql;
    }

    public CategoryItem ToItem(IDataReader dr)
    {
        return new CategoryItem()
        {
            Category = dr.IsDBNull(0) ? "(null)" : dr.GetString(0),
            Count = dr.IsDBNull(3) ? 0 : dr.GetInt32(3),
            Extent = [dr.IsDBNull(1) ? 0 : dr.GetDecimal(1), dr.IsDBNull(2) ? 0 : dr.GetDecimal(2)],
            Name = dr.GetString(0) ?? "",
        };
    }
}
