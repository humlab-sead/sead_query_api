using Autofac;

namespace SeadQueryCore.Plugin.Range;

public class RangeFacetPlugin(
    IRangeCategoryCountHelper categoryCountHelper,
    IRangeCategoryCountSqlCompiler categoryCountSqlCompiler,
    IRangeCategoryInfoService categoryInfoService,
    IRangePickFilterCompiler pickFilterCompiler
) : IRangeFacetPlugin
{
    public ICategoryCountHelper CategoryCountHelper { get; } = categoryCountHelper;

    public ICategoryCountSqlCompiler CategoryCountSqlCompiler { get; } = categoryCountSqlCompiler;

    public ICategoryInfoService CategoryInfoService { get; } = categoryInfoService;

    public IPickFilterCompiler PickFilterCompiler { get; } = pickFilterCompiler;

    public void Register(ContainerBuilder builder)
    {
        RegisterPlugin(builder);
    }

    public static void RegisterPlugin(ContainerBuilder builder)
    {
        builder.RegisterType<RangeCategoryCountHelper>().Keyed<ICategoryCountHelper>(EFacetType.Range);
        builder.RegisterType<RangeCategoryCountSqlCompiler>().Keyed<ICategoryCountSqlCompiler>(EFacetType.Range);
        builder.RegisterType<RangeCategoryInfoService>().Keyed<ICategoryInfoService>(EFacetType.Range);
        builder.RegisterType<RangePickFilterCompiler>().Keyed<IPickFilterCompiler>(EFacetType.Range);
        builder.RegisterType<RangeOuterBoundService>().As<IRangeOuterBoundService>();

        builder.RegisterType<RangeCategoryInfoSqlCompiler>().As<IRangeCategoryInfoSqlCompiler>();
        builder.RegisterType<RangeOuterBoundSqlCompiler>().As<IRangeOuterBoundSqlCompiler>();

        // Needed by Plugin
        builder.RegisterType<RangeCategoryCountSqlCompiler>().As<IRangeCategoryCountSqlCompiler>();
        builder.RegisterType<RangeCategoryCountHelper>().As<IRangeCategoryCountHelper>();
        builder.RegisterType<RangeCategoryInfoService>().As<IRangeCategoryInfoService>();
        builder.RegisterType<RangePickFilterCompiler>().As<IRangePickFilterCompiler>();

        builder.RegisterType<RangeFacetPlugin>().As<IRangeFacetPlugin>();
        builder.RegisterType<RangeFacetPlugin>().Keyed<IFacetPlugin>(EFacetType.Range);

    }
}
