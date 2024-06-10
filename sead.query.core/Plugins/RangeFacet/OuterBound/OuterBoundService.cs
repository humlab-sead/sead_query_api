namespace SeadQueryCore.Plugin.Range;

public class RangeOuterBoundService(ITypedQueryProxy queryProxy) : IRangeOuterBoundService
{
    public ITypedQueryProxy QueryProxy { get; } = queryProxy;

    public (decimal, decimal) GetUpperLowerBounds(Facet facet)
    {
        string sql = new RangeOuterBoundSqlCompiler().Compile(null, facet);
        var values = QueryProxy.QueryScalars<decimal>(sql);
        return values?.Count != 2 ? (0, 0) : (values[0], values[1]);
    }
}
