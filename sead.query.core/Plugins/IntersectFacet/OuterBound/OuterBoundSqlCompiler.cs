
namespace SeadQueryCore.Plugin.Intersect;

public class IntersectOuterBoundSqlCompiler : IIntersectOuterBoundSqlCompiler
{
    public string Compile(QueryBuilder.QuerySetup querySetup, Facet facet)
    {
        var categoryType = facet?.CategoryIdType ?? "int4range";
        var rangeType = categoryType.StartsWith("int") ? "integer" : "decimal";

        string sql = $@"
            SELECT MIN(LOWER({facet.CategoryIdExpr}))::{rangeType} AS lower,
                   MAX(UPPER({facet.CategoryIdExpr}))::{rangeType} AS upper
            FROM {facet.TargetTable.ResolvedSqlJoinName}
    ";
        return sql;
    }
}
