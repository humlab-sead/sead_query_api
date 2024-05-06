using SeadQueryCore.QueryBuilder;

namespace SeadQueryCore;

public interface IIntersectOuterBoundSqlCompiler : ISqlCompiler
{
    string Compile(QuerySetup query, Facet facet);
}
