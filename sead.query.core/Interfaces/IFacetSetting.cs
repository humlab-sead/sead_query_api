namespace SeadQueryCore
{
    public interface IFacetSetting
    {
        string DirectCountTable { get; set; }
        string DirectCountColumn { get; set; }
        string IndirectCountTable { get; set; }
        string IndirectCountColumn { get; set; }
        int ResultQueryLimit { get; set; }
        bool CategoryNameFilter { get; set; }
    }
}
