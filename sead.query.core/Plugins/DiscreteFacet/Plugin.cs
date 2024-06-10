using Autofac;

namespace SeadQueryCore.Plugin.Discrete;

public class DiscreteFacetPlugin(
    IDiscreteCategoryCountHelper categoryCountHelper,
    IDiscreteCategoryCountSqlCompiler categoryCountSqlCompiler,
    IDiscreteCategoryInfoService categoryInfoService,
    IDiscretePickFilterCompiler pickFilterCompiler
) : IDiscreteFacetPlugin
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
        builder.RegisterType<DiscreteCategoryCountHelper>().Keyed<ICategoryCountHelper>(EFacetType.Discrete);
        builder.RegisterType<DiscreteCategoryCountSqlCompiler>().Keyed<ICategoryCountSqlCompiler>(EFacetType.Discrete);
        builder.RegisterType<DiscreteCategoryInfoService>().Keyed<ICategoryInfoService>(EFacetType.Discrete);
        builder.RegisterType<DiscretePickFilterCompiler>().Keyed<IPickFilterCompiler>(EFacetType.Discrete);

        builder.RegisterType<DiscreteCategoryInfoSqlCompiler>().As<IDiscreteCategoryInfoSqlCompiler>();
        builder.RegisterType<DiscreteCategoryCountHelper>().As<IDiscreteCategoryCountHelper>();
        builder.RegisterType<DiscreteCategoryCountSqlCompiler>().As<IDiscreteCategoryCountSqlCompiler>();
        builder.RegisterType<DiscreteCategoryInfoService>().As<IDiscreteCategoryInfoService>();
        builder.RegisterType<DiscretePickFilterCompiler>().As<IDiscretePickFilterCompiler>();

        builder.RegisterType<DiscreteFacetPlugin>().As<IDiscreteFacetPlugin>();
        builder.RegisterType<DiscreteFacetPlugin>().Keyed<IFacetPlugin>(EFacetType.Discrete);

    }
}
