namespace SeadQueryCore
{
    public interface IRangeOuterBoundExtentService
    {
        ITypedQueryProxy QueryProxy { get; }
        (decimal, decimal) GetUpperLowerBounds(Facet facet);
    }
}
