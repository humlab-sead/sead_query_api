namespace SeadQueryCore.Plugin.Range;
using SeadQueryCore.QueryBuilder;

public interface IRangeOuterBoundService
{
    ITypedQueryProxy QueryProxy { get; }
    (decimal, decimal) GetUpperLowerBounds(Facet facet);
}

public interface IRangeOuterBoundSqlCompiler : ISqlCompiler
{
    string Compile(QuerySetup query, Facet facet);
}
