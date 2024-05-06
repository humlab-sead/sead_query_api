namespace SeadQueryCore.Plugin.Range;

public interface IRangeOuterBoundService
{
    ITypedQueryProxy QueryProxy { get; }
    (decimal, decimal) GetUpperLowerBounds(Facet facet);
}
