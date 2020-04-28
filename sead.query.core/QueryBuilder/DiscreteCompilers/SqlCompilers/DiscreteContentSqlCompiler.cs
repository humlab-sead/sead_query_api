using static SeadQueryCore.Utility;

namespace SeadQueryCore
{
    public class DiscreteContentSqlCompiler : IDiscreteContentSqlCompiler
    {
        public virtual string Compile(QueryBuilder.QuerySetup query, Facet facet, string text_filter)
        {
            string text_criteria = text_filter.IsEmpty() ? "" : $" AND {facet.CategoryNameExpr} ILIKE '{text_filter}' ";
            string sort_clause = empty(facet.SortExpr) ? "" : $", {facet.SortExpr} ORDER BY {facet.SortExpr}";

            string sql = $@"
            SELECT cast({facet.CategoryIdExpr} AS varchar) AS category, {facet.CategoryNameExpr} AS name
            FROM {query.Facet.TargetTable.ResolvedSqlJoinName}
                 {query.Joins.Combine("")}
            WHERE 1 = 1
              {text_criteria}
            {"AND ".GlueTo(query.Criterias.Combine(" AND "))}
            GROUP BY {facet.CategoryIdExpr}, {facet.CategoryNameExpr}
            {sort_clause}";
            return sql;
        }
    }
}