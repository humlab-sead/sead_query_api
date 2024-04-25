using SeadQueryCore.QueryBuilder;

namespace SeadQueryCore
{
    public interface IRangeOuterBoundSqlCompiler : ISqlCompiler
    {
        string Compile(QuerySetup query, Facet facet);
    }
}