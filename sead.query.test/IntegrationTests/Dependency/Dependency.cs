using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using SeadQueryAPI;
using SeadQueryAPI.Serializers;
using SeadQueryCore;
using SeadQueryCore.QueryBuilder;
using SeadQueryCore.Services.Result;
using SeadQueryInfra;
using System;
using System.Diagnostics;
using Xunit;

namespace SQT.Infrastructure
{
    [CollectionDefinition("JsonSeededFacetContext")]
    public class TestDependencyService : Module, IDisposable
    {
        public ISetting Options { get; set; }

        public IFacetContext FacetContext { get; set; } = null;
        public JsonSeededFacetContextFixture Fixture { get; set; } = null;

        private DisposableFacetContextContainer MockService;

        public TestDependencyService()
        {
            Fixture = new JsonSeededFacetContextFixture();
            MockService = new DisposableFacetContextContainer(Fixture);
        }

        public void Dispose()
        {
            MockService.Dispose();
        }

        public virtual ISeadQueryCache GetCache(StoreSetting settings)
        {
            return new NullCache();
        }

        protected override void Load(ContainerBuilder builder)
        {
            Options ??= MockService.CreateSettings();

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

            builder.RegisterType<QuerySetupBuilder>().As<IQuerySetupBuilder>();
            builder.RegisterType<BogusPickService>().As<IBogusPickService>();
            builder.RegisterType<FacetConfigReconstituteService>().As<IFacetConfigReconstituteService>();

            //builder.RegisterType<RangeCategoryBoundsService>().As<ICategoryBoundsService>();
            builder.RegisterType<RangeOuterBoundExtentService>().As<IRangeOuterBoundExtentService>();

            builder.RegisterType<UndefinedFacetPickFilterCompiler>().Keyed<IPickFilterCompiler>(0);
            builder.RegisterType<DiscreteFacetPickFilterCompiler>().Keyed<IPickFilterCompiler>(1);
            builder.RegisterType<RangeFacetPickFilterCompiler>().Keyed<IPickFilterCompiler>(2);
            builder.RegisterType<GeoFacetPickFilterCompiler>().Keyed<IPickFilterCompiler>(3);
            builder.RegisterType<PickFilterCompilerLocator>().As<IPickFilterCompilerLocator>();

            builder.RegisterType<RangeCategoryCountService>().Keyed<ICategoryCountService>(EFacetType.Range);
            builder.RegisterType<DiscreteCategoryCountService>().Keyed<ICategoryCountService>(EFacetType.Discrete);
            builder.RegisterType<DiscreteCategoryCountService>().As<IDiscreteCategoryCountService>();
            builder.RegisterType<CategoryCountServiceLocator>().As<ICategoryCountServiceLocator>();

            builder.RegisterType<ValidPicksSqCompiler>().As<IValidPicksSqlCompiler>();
            builder.RegisterType<JoinSqlCompiler>().As<IJoinSqlCompiler>();
            builder.RegisterType<DiscreteContentSqlCompiler>().As<IDiscreteContentSqlCompiler>();
            builder.RegisterType<DiscreteCategoryCountSqlCompiler>().As<IDiscreteCategoryCountQueryCompiler>();
            builder.RegisterType<RangeCategoryCountSqlCompiler>().As<IRangeCategoryCountSqlCompiler>();
            builder.RegisterType<RangeIntervalSqlCompiler>().As<IRangeIntervalSqlCompiler>();
            builder.RegisterType<RangeOuterBoundSqlCompiler>().As<IRangeOuterBoundSqlCompiler>();

            builder.RegisterType<RangeFacetContentService>().Keyed<IFacetContentService>(EFacetType.Range);
            builder.RegisterType<DiscreteFacetContentService>().Keyed<IFacetContentService>(EFacetType.Discrete);
            builder.RegisterType<FacetContentServiceLocator>().As<IFacetContentServiceLocator>();

            builder.RegisterType<ResultConfigCompiler>().As<IResultConfigCompiler>();

            builder.RegisterType<RangeCategoryBoundSqlCompiler>().Keyed<ICategoryBoundSqlCompiler>(EFacetType.Range);

            builder.RegisterType<DefaultResultService>().Keyed<IResultService>("tabular");
            builder.RegisterType<MapResultService>().Keyed<IResultService>("map");

            builder.RegisterType<TabularResultSqlCompiler>().Keyed<IResultSqlCompiler>("tabular");
            builder.RegisterType<MapResultSqlCompiler>().Keyed<IResultSqlCompiler>("map");
            builder.RegisterType<ResultSqlCompilerLocator>().As<IResultSqlCompilerLocator>();

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
