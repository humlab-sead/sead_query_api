
namespace SeadQueryCore
{
    public class DiscreteCategoryCountSqlCompiler : IDiscreteCategoryCountSqlCompiler
    {
        public string Compile(QueryBuilder.QuerySetup query, Facet facet, CompilePayload payload)
        {
            string sql = $@"
            SELECT category, {payload.AggregateType}(value) AS count
            FROM (
                SELECT {facet.CategoryIdExpr} AS category, {payload.AggregateFacet.CategoryIdExpr} AS value
                FROM {query.Facet.TargetTable.ResolvedSqlJoinName}
                     {query.Joins.Combine("\t\t\t\t\t\n")}
                WHERE 1 = 1
                {"AND ".GlueTo(query.Criterias.Combine(" AND "))}
                GROUP BY {facet.CategoryIdExpr}, {payload.AggregateFacet.CategoryIdExpr}
            ) AS x
            GROUP BY category;
        ";
            return sql;
        }
    }
}
