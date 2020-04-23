namespace SeadQueryCore
{
    public interface ISetting
    {
        FacetSetting Facet { get; set; }
        StoreSetting Store { get; set; }
    }
}
