using SeadQueryCore.QueryBuilder;

namespace SeadQueryCore.Plugin.Intersect;

public interface IIntersectOuterBoundSqlCompiler : ISqlCompiler
{
    string Compile(QuerySetup query, Facet facet);
}

public interface IIntersectOuterBoundService
{
    ITypedQueryProxy QueryProxy { get; }
    (decimal, decimal) GetUpperLowerBounds(Facet facet);
}
