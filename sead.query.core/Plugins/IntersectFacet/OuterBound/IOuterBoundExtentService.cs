namespace SeadQueryCore.Plugin.Intersect;

public interface IIntersectOuterBoundService
{
    ITypedQueryProxy QueryProxy { get; }
    (decimal, decimal) GetUpperLowerBounds(Facet facet);
}
