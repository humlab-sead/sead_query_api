using SeadQueryCore.QueryBuilder;

namespace SeadQueryCore.Plugin.Range;

public interface IRangeOuterBoundSqlCompiler : ISqlCompiler
{
    string Compile(QuerySetup query, Facet facet);
}
