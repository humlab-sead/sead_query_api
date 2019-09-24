namespace SeadQueryCore
{
    public interface IQueryBuilderSetting
    {
        FacetSetting Facet { get; set; }
        StoreSetting Store { get; set; }
    }
}
