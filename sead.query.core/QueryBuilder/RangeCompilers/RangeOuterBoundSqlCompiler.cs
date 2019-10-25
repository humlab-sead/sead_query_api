
namespace SeadQueryCore
{
    public class RangeOuterBoundSqlCompiler : IRangeOuterBoundSqlCompiler
    {
        public string Compile(QueryBuilder.QuerySetup query, Facet facet)
        {
            string sql = $@"
          SELECT MIN({facet.CategoryIdExpr}) AS lower, MAX({facet.CategoryIdExpr}) AS upper
          FROM {facet.TargetTable.TableOrUdfName}{facet.TargetTable.UdfCallArguments ?? ""}  {"AS ".GlueTo(facet.AliasName)}
        ";
            return sql;
        }
    }
}