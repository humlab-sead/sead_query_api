
namespace SeadQueryCore.Plugin.Intersect;

public class IntersectOuterBoundSqlCompiler: IIntersectOuterBoundSqlCompiler
{
    public string Compile(QueryBuilder.QuerySetup querySetup, Facet facet)
    {
        // NOTE: CategoryIdExpr MUST be a PostgreSQL range type
        string sql = $@"
            SELECT MIN(LOWER({facet.CategoryIdExpr}))::{facet.CategoryIdType} AS lower,
                   MAX(UPPER({facet.CategoryIdExpr}))::{facet.CategoryIdType} AS upper
            FROM {facet.TargetTable.ResolvedSqlJoinName}
    ";
        return sql;
    }
}
