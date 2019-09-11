using SeadQueryCore.QueryBuilder;

namespace SeadQueryCore
{
    public interface IRangeOuterBoundSqlCompiler
    {
        string Compile(QuerySetup query, Facet facet);
    }
}