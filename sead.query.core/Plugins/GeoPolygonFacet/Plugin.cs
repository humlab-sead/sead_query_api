using Autofac;

namespace SeadQueryCore.Plugin.GeoPolygon;

public class GeoPolygonFacetPlugin(
    IGeoPolygonCategoryCountHelper categoryCountHelper,
    IGeoPolygonCategoryCountSqlCompiler categoryCountSqlCompiler,
    IGeoPolygonCategoryInfoService categoryInfoService,
    IGeoPolygonPickFilterCompiler pickFilterCompiler
) : IGeoPolygonFacetPlugin
{
    public ICategoryCountHelper CategoryCountHelper => categoryCountHelper;

    public ICategoryCountSqlCompiler CategoryCountSqlCompiler => categoryCountSqlCompiler;

    public ICategoryInfoService CategoryInfoService => categoryInfoService;

    public IPickFilterCompiler PickFilterCompiler => pickFilterCompiler;

    public void Register(ContainerBuilder builder)
    {
        RegisterPlugin(builder);
    }

    public static void RegisterPlugin(ContainerBuilder builder)
    {
        builder.RegisterType<GeoPolygonCategoryCountHelper>().Keyed<ICategoryCountHelper>(EFacetType.GeoPolygon);
        builder.RegisterType<GeoPolygonCategoryCountSqlCompiler>().Keyed<ICategoryCountSqlCompiler>(EFacetType.GeoPolygon);
        builder.RegisterType<GeoPolygonCategoryInfoService>().Keyed<ICategoryInfoService>(EFacetType.GeoPolygon);
        builder.RegisterType<GeoPolygonPickFilterCompiler>().Keyed<IPickFilterCompiler>(EFacetType.GeoPolygon);

        builder.RegisterType<GeoPolygonCategoryInfoSqlCompiler>().As<IGeoPolygonCategoryInfoSqlCompiler>();
        builder.RegisterType<GeoPolygonCategoryCountHelper>().As<IGeoPolygonCategoryCountHelper>();
        builder.RegisterType<GeoPolygonCategoryCountSqlCompiler>().As<IGeoPolygonCategoryCountSqlCompiler>();
        builder.RegisterType<GeoPolygonCategoryInfoService>().As<IGeoPolygonCategoryInfoService>();
        builder.RegisterType<GeoPolygonPickFilterCompiler>().As<IGeoPolygonPickFilterCompiler>();

        builder.RegisterType<GeoPolygonFacetPlugin>().As<IGeoPolygonFacetPlugin>();
        builder.RegisterType<GeoPolygonFacetPlugin>().Keyed<IFacetPlugin>(EFacetType.GeoPolygon);

    }
}
