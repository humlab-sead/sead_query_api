namespace SeadQueryCore
{
    public interface IEdgeSqlCompiler
    {
        string Compile(IFacetsGraph graph, GraphEdge edge, bool innerJoin = false);
    }
}