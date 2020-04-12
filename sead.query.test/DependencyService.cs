using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using SeadQueryAPI;
using SeadQueryAPI.Serializers;
using SeadQueryCore;
using SeadQueryCore.QueryBuilder;
using SeadQueryCore.Services.Result;
using SeadQueryInfra;
using System.Diagnostics;

namespace SeadQueryTest.Infrastructure
{
    public class TestDependencyService : DependencyService
    {

        public IFacetContext FacetContext { get; set;  } = null;

        public override ISeadQueryCache GetCache(StoreSetting settings)
        {
            return new NullCache();
        }

        protected override void Load(ContainerBuilder builder)
        {
            Options ??= (ISetting)new SettingFactory().Create().Value;

            builder.RegisterInstance(Options).SingleInstance().ExternallyOwned();
            builder.RegisterInstance<IFacetSetting>(Options.Facet).SingleInstance().ExternallyOwned();
            builder.RegisterInstance<StoreSetting>(Options.Store).SingleInstance().ExternallyOwned();

            if (FacetContext is null) {

                Debug.Print("Warning: Falling back to default online DB connection for test");

                // builder.RegisterType<FacetContext>().As<IFacetContext>().SingleInstance().ExternallyOwned();

                builder.RegisterType<FacetContextFactory>()
                    .As<IFacetContextFactory>()
                    .InstancePerLifetimeScope();

                builder.Register(c => c.Resolve<IFacetContextFactory>().GetInstance())
                    .As<IFacetContext>()
                    .InstancePerLifetimeScope();

            } else {
                builder.RegisterInstance(FacetContext).SingleInstance().ExternallyOwned();
            }

            builder.Register(c => c.Resolve<IFacetContext>().TypedQueryProxy)
                .As<ITypedQueryProxy>()
                .InstancePerLifetimeScope();
             
            builder.Register(c => c.Resolve<IFacetContext>().DynamicQueryProxy)
                .As<IDynamicQueryProxy>()
                .InstancePerLifetimeScope();

            builder.RegisterType<RepositoryRegistry>().As<IRepositoryRegistry>().SingleInstance().ExternallyOwned();

            builder.RegisterType<FacetGraphFactory>().As<IFacetGraphFactory>().InstancePerLifetimeScope();
            builder.Register<IFacetsGraph>(c => c.Resolve<IFacetGraphFactory>().Build()).SingleInstance();

            builder.RegisterType<QuerySetupCompiler>().As<IQuerySetupCompiler>();
            builder.RegisterType<DiscreteBogusPickService>().As<IDiscreteBogusPickService>();
            builder.RegisterType<FacetConfigReconstituteService>().As<IFacetConfigReconstituteService>();

            builder.RegisterType<RangeCategoryBoundsService>().As<ICategoryBoundsService>();

            builder.RegisterType<UndefinedFacetPickFilterCompiler>().Keyed<IPickFilterCompiler>(0);
            builder.RegisterType<DiscreteFacetPickFilterCompiler>().Keyed<IPickFilterCompiler>(1);
            builder.RegisterType<RangeFacetPickFilterCompiler>().Keyed<IPickFilterCompiler>(2);
            builder.RegisterType<GeoFacetPickFilterCompiler>().Keyed<IPickFilterCompiler>(3);
            builder.RegisterType<PickFilterCompilerLocator>().As<IPickFilterCompilerLocator>();

            builder.RegisterType<RangeCategoryCountService>().Keyed<ICategoryCountService>(EFacetType.Range);
            builder.RegisterType<DiscreteCategoryCountService>().Keyed<ICategoryCountService>(EFacetType.Discrete);
            builder.RegisterType<DiscreteCategoryCountService>().As<IDiscreteCategoryCountService>();

            builder.RegisterType<ValidPicksSqlQueryCompiler>().As<IValidPicksSqlQueryCompiler>();
            builder.RegisterType<EdgeSqlCompiler>().As<IEdgeSqlCompiler>();
            builder.RegisterType<DiscreteContentSqlQueryBuilder>().As<IDiscreteContentSqlQueryCompiler>();
            builder.RegisterType<DiscreteCategoryCountSqlQueryCompiler>().As<IDiscreteCategoryCountSqlQueryCompiler>();
            builder.RegisterType<RangeCategoryCountSqlQueryCompiler>().As<IRangeCategoryCountSqlQueryCompiler>();
            builder.RegisterType<RangeIntervalSqlQueryCompiler>().As<IRangeIntervalSqlQueryCompiler>();
            builder.RegisterType<RangeOuterBoundSqlCompiler>().As<IRangeOuterBoundSqlCompiler>();
            builder.RegisterType<RangeFacetContentService>().Keyed<IFacetContentService>(EFacetType.Range);
            builder.RegisterType<DiscreteFacetContentService>().Keyed<IFacetContentService>(EFacetType.Discrete);

            builder.RegisterType<ResultQueryCompiler>().As<IResultQueryCompiler>();

            builder.RegisterType<RangeCategoryBoundSqlQueryCompiler>().Keyed<ICategoryBoundSqlQueryCompiler>(EFacetType.Range);

            builder.RegisterType<DefaultResultService>().Keyed<IResultService>("tabular");
            builder.RegisterType<MapResultService>().Keyed<IResultService>("map");

            builder.RegisterType<TabularResultSqlQueryCompiler>().Keyed<IResultSqlQueryCompiler>("tabular");
            builder.RegisterType<MapResultSqlQueryCompiler>().Keyed<IResultSqlQueryCompiler>("map");

            builder.Register(_ => GetCache(Options?.Store)).SingleInstance().ExternallyOwned();
            if (Options.Store.UseRedisCache) {
                builder.RegisterType<SeadQueryAPI.Services.CachedLoadFacetService>().As<SeadQueryAPI.Services.IFacetReconstituteService>();
                builder.RegisterType<SeadQueryAPI.Services.CachedLoadResultService>().As<SeadQueryAPI.Services.ILoadResultService>();
            } else {
                builder.RegisterType<SeadQueryAPI.Services.LoadFacetService>().As<SeadQueryAPI.Services.IFacetReconstituteService>();
                builder.RegisterType<SeadQueryAPI.Services.LoadResultService>().As<SeadQueryAPI.Services.ILoadResultService>();
            }
        }

        public static IContainer CreateContainer(IFacetContext facetContext, ISetting options)
        {
            var builder = new Autofac.ContainerBuilder();
            var dependencyService = new TestDependencyService() { FacetContext = facetContext, Options = options };
            dependencyService.Load(builder);
            return builder.Build();
        }
    }
}
