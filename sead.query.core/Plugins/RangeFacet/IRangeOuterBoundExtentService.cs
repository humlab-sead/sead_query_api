namespace SeadQueryCore
{
    public interface IRangeOuterBoundExtentService
    {
        ITypedQueryProxy QueryProxy { get; }

        RangeExtent GetExtent(FacetConfig2 config, int default_interval_count = 120);
        (decimal, decimal) GetUpperLowerBounds(Facet facet);
    }
}