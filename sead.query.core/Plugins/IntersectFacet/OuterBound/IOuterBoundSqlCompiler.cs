using SeadQueryCore.QueryBuilder;

namespace SeadQueryCore.Plugin.Intersect;

public interface IIntersectOuterBoundSqlCompiler : ISqlCompiler
{
    string Compile(QuerySetup query, Facet facet);
}
