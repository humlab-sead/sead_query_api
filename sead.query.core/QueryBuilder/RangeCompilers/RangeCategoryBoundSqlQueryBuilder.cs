using SeadQueryCore.QueryBuilder;
using System;
using System.Linq;

namespace SeadQueryCore
{
    public class RangeCategoryBoundSqlQueryCompiler : ICategoryBoundSqlQueryCompiler
    {
        public string Compile(QuerySetup query, Facet facet, string facetCode)
        {
            string clauses = String.Join("", facet.Clauses.Select(x => x.Clause));
            string sql = $@"
               SELECT '{facetCode}' AS facet_code, MIN({facet.CategoryIdExpr}::real) AS min, MAX({facet.CategoryIdExpr}::real) AS max
               FROM {query.Facet.TargetTable.ObjectName}{query.Facet.TargetTable.ObjectArgs ?? ""} 
                 {query.Joins.Combine("")}
             {"WHERE ".GlueTo(clauses)}";
            return sql;
        }
    }
}