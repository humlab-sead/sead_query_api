
namespace SeadQueryCore
{
    public class RangeOuterBoundSqlCompiler : IRangeOuterBoundSqlCompiler
    {
        public string Compile(QueryBuilder.QuerySetup querySetup, Facet facet)
        {
            string sql = $@"
          SELECT MIN({facet.CategoryIdExpr}) AS lower, MAX({facet.CategoryIdExpr}) AS upper
          FROM {facet.TargetTable.ResolvedSqlJoinName}
        ";
            return sql;
        }
    }
}