
namespace SeadQueryCore
{
    public class DiscreteContentSqlCompiler : IDiscreteContentSqlCompiler
    {
        public virtual string Compile(QueryBuilder.QuerySetup query, Facet facet, string categoryNameFilter)
        {
            string sql = $@"
            SELECT cast({facet.CategoryIdExpr} AS varchar) AS category, {facet.CategoryNameExpr} AS name
            FROM {query.Facet.TargetTable.ResolvedSqlJoinName}
                 {query.Joins.Combine("")}
            WHERE 1 = 1
              {"AND ".GlueTo(CategoryNameLike(facet, categoryNameFilter)) }
              {"AND ".GlueTo(query.Criterias.Combine(" AND "))}
            GROUP BY 1, 2
            {SortBy(facet)}";
            return sql;
        }

        private static string CategoryNameLike(Facet facet, string categoryNameFilter)
            => categoryNameFilter.IsEmpty() ? "" : $"{facet.CategoryNameExpr} ILIKE '{categoryNameFilter}'";

        private static string SortBy(Facet facet)
            => facet.SortExpr.IsEmpty() ? "" : $", {facet.SortExpr} ORDER BY {facet.SortExpr}";

    }
}