
using System.Data;

namespace SeadQueryCore.Plugin.Range;

public class RangeCategoryCountSqlCompiler : IRangeCategoryCountSqlCompiler
{
    public string Compile(QueryBuilder.QuerySetup query, Facet facet, CompilePayload payload)
    {
        string sql = $@"

        WITH categories(category, lower, upper) AS (
            {payload.IntervalQuery}
        ), outerbounds(lower, upper) AS (
            SELECT MIN(lower), MAX(upper)
            FROM categories
        )
            SELECT c.category, c.lower, c.upper, COALESCE(r.count_column, 0) as count_column
            FROM categories c
            LEFT JOIN (
                SELECT category, COUNT(DISTINCT {payload.CountColumn}) AS count_column
                FROM {query.Facet.TargetTable.ResolvedSqlJoinName}
                CROSS JOIN outerbounds
                JOIN categories
                    ON categories.lower <= {facet.CategoryIdExpr}::{facet.CategoryIdType}
                    AND categories.upper >= {facet.CategoryIdExpr}::{facet.CategoryIdType}
                    AND (NOT (categories.upper < outerbounds.upper AND {facet.CategoryIdExpr}::{facet.CategoryIdType} = categories.upper))
                {query.Joins.Combine("\n\t\t\t\t\t")}
                WHERE TRUE
                    { "AND ".GlueTo(query.Criterias.Combine(" AND ")) }
                GROUP BY category
            ) AS r
                ON r.category = c.category
            ORDER BY c.lower";

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
