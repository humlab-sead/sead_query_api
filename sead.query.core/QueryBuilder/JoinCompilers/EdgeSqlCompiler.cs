
namespace SeadQueryCore
{
    public class EdgeSqlCompiler : IEdgeSqlCompiler
    {
        public string Compile(IFacetsGraph graph, GraphEdge edge, bool innerJoin = false)
        {
            var resolvedTableName = graph.ResolveTargetName(edge.TargetName);
            var resolvedAliasName = graph.ResolveAliasName(edge.TargetName);
            var joinType = innerJoin ? "inner" : "left";
            var sql = $" {joinType} join {resolvedTableName} {resolvedAliasName ?? ""}" +
                    $" on {resolvedAliasName ?? resolvedTableName}.\"{edge.TargetKeyName}\" = " +
                            $"{edge.SourceName}.\"{edge.SourceKeyName}\" ";
            return sql;
        }
    }
}