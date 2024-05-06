namespace SeadQueryCore.Plugin.Intersect;

public class IntersectOuterBoundService(ITypedQueryProxy queryProxy) : IIntersectOuterBoundService
{
    public ITypedQueryProxy QueryProxy { get; } = queryProxy;

    public (decimal, decimal) GetUpperLowerBounds(Facet facet)
    {
        string sql = new IntersectOuterBoundSqlCompiler().Compile(null, facet);
        var values = QueryProxy.QueryScalars<decimal>(sql);
        return values?.Count != 2 ? (0, 0) : (values[0], values[1]);
    }
}
