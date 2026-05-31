using Autofac;
using SeadQueryCore.Plugin.Range;

namespace SeadQueryCore.Plugin.Intersect;

public class IntersectFacetPlugin(
    IRangeCategoryCountHelper categoryCountHelper,
    IIntersectCategoryCountSqlCompiler categoryCountSqlCompiler,
    IIntersectCategoryInfoService categoryInfoService,
    IIntersectPickFilterCompiler pickFilterCompiler
) : IIntersectFacetPlugin
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
        RegisterLegacyRuntime(builder);
        RegisterComposerRuntime(builder);
        RegisterSharedPlugin(builder);
    }

    public static void RegisterLegacyRuntime(ContainerBuilder builder)
    {
        builder.RegisterType<IntersectCategoryCountHelper>().Keyed<ICategoryCountHelper>(EFacetType.Intersect);
        builder.RegisterType<IntersectCategoryCountSqlCompiler>().Keyed<ICategoryCountSqlCompiler>(EFacetType.Intersect);
        builder.RegisterType<IntersectCategoryInfoService>().Keyed<ICategoryInfoService>(EFacetType.Intersect);
        builder.RegisterType<IntersectPickFilterCompiler>().Keyed<IPickFilterCompiler>(EFacetType.Intersect);
    }

    public static void RegisterComposerRuntime(ContainerBuilder builder)
    {
        builder.RegisterType<IntersectOuterBoundService>().As<IIntersectOuterBoundService>();
        builder.RegisterType<IntersectCategoryInfoSqlCompiler>().As<IIntersectCategoryInfoSqlCompiler>();
        builder.RegisterType<IntersectOuterBoundSqlCompiler>().As<IIntersectOuterBoundSqlCompiler>();

        builder.RegisterType<IntersectCategoryCountSqlCompiler>().As<IIntersectCategoryCountSqlCompiler>();
        builder.RegisterType<IntersectCategoryCountHelper>().As<IIntersectCategoryCountHelper>();
        builder.RegisterType<IntersectCategoryInfoService>().As<IIntersectCategoryInfoService>();
        builder.RegisterType<IntersectPickFilterCompiler>().As<IIntersectPickFilterCompiler>();
    }

    public static void RegisterSharedPlugin(ContainerBuilder builder)
    {
        builder.RegisterType<IntersectFacetPlugin>().As<IIntersectFacetPlugin>();
        builder.RegisterType<IntersectFacetPlugin>().Keyed<IFacetPlugin>(EFacetType.Intersect);
    }
}
