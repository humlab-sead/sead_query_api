using System;
using System.Collections.Generic;

namespace SeadQueryCore.Plugin;

public class ValidPicksSqCompiler : IValidPicksSqlCompiler
{
    public string Compile(QueryBuilder.QuerySetup query, List<int> picks)
    {
        if (picks.Count == 0)
            throw new ArgumentException("No picks specified!");

        string picks_clause = picks.Combine(",", x => $"('{x}'::text)");
        string sql = $@"
        SELECT DISTINCT pick_id, {query.Facet.CategoryNameExpr} AS name
        FROM {query.Facet.TargetTable.ResolvedSqlJoinName}
            {query.Joins.Combine("")}
        JOIN (VALUES {picks_clause}) AS x(pick_id)
            ON x.pick_id = {query.Facet.CategoryIdExpr}::text
        WHERE 1 = 1
            {" AND ".GlueTo(query.Criterias.Combine(" AND "))}
    ";
        return sql;
    }
}
