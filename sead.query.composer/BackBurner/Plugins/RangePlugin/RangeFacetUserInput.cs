namespace SeadQueryComposer.Plugins.DiscretePlugin;

public class RangeFacetUserInput
{
    public decimal? MinValue { get; set; }
    public decimal? MaxValue { get; set; }
    public bool HasRange => MinValue.HasValue || MaxValue.HasValue;
}

public class RangeFacetConfiguration : AnchorTemplate
{
    public IRangeBinningStrategy BinningStrategy { get; set; }
    public string RangeColumn { get; set; } = string.Empty;
}