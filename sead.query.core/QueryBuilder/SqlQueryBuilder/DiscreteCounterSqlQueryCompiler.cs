
namespace SeadQueryCore
{
    public class DiscreteCounterSqlQueryCompiler : IDiscreteCounterSqlQueryCompiler
    {
        public string Compile(QueryBuilder.QuerySetup query, Facet facet, Facet countFacet, string aggType)
        {
            string sql = $@"
            SELECT category, {aggType}(value) AS count
            FROM (
                SELECT {facet.CategoryIdExpr} AS category, {countFacet.CategoryIdExpr} AS value
                FROM {query.Facet.TargetTableName} {"AS ".GlueTo(query.Facet.AliasName)}
                     {query.Joins.Combine("")}
                WHERE 1 = 1
                {"AND ".GlueTo(query.Criterias.Combine(" AND "))}
                GROUP BY {facet.CategoryIdExpr}, {countFacet.CategoryIdExpr}
            ) AS x
            GROUP BY category;
        ";
            return sql;
        }
    }
}