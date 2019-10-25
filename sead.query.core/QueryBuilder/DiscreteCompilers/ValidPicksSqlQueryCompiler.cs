using System.Collections.Generic;

namespace SeadQueryCore
{

    public class ValidPicksSqlQueryCompiler : IValidPicksSqlQueryCompiler
    {
        public string Compile(QueryBuilder.QuerySetup query, Facet facet, List<int> picks)
        {
            string picks_clause = picks.Combine(",", x => $"('{x}'::text)");
            string sql = $@"
            SELECT DISTINCT pick_id, {facet.CategoryNameExpr} AS name
            FROM {query.Facet.TargetTable.TableOrUdfName}{query.Facet.TargetTable.UdfCallArguments ?? ""}  {"AS ".GlueTo(query.Facet.AliasName)}
            JOIN (VALUES {picks_clause}) AS x(pick_id)
              ON x.pick_id = {facet.CategoryIdExpr}::text
              {query.Joins.Combine("")}
            WHERE 1 = 1
              {" AND ".GlueTo(query.Criterias.Combine(" AND "))}
        ";
            return sql;
        }
    }
}