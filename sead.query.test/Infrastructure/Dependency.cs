using Autofac;
using SeadQueryAPI.Serializers;
using SeadQueryCore;
using SeadQueryCore.QueryBuilder;
using SeadQueryCore.Plugin;
using SeadQueryCore.Services.Result;
using SeadQueryInfra;
using System;
using System.Diagnostics;

namespace SQT.Infrastructure
{
    public class DependencyService : Module, IDisposable
    {
        public ISetting Options { get; set; }

        public IFacetContext FacetContext { get; set; } = null;
        public JsonFacetContextFixture Fixture { get; set; } = null;

        private readonly DisposableFacetContextContainer MockService;

        public DependencyService(JsonFacetContextFixture fixture)
        {
            Fixture = fixture;
            MockService = new DisposableFacetContextContainer(Fixture);
            // FacetContext = MockService.FacetContext;
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

            if (FacetContext is null)
            {
                Debug.Print("Warning: Falling back to default online DB connection for test");

                // builder.RegisterType<FacetContext>().As<IFacetContext>().SingleInstance().ExternallyOwned();

                builder.RegisterType<FacetContextFactory>()
                    .As<IFacetContextFactory>()
                    .InstancePerLifetimeScope();

                builder.Register(c => c.Resolve<IFacetContextFactory>().GetInstance())
                    .As<IFacetContext>()
                    .InstancePerLifetimeScope();
            }
            else
            {
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
            builder.RegisterType<ResultConfigReconstituteService>().As<IResultConfigReconstituteService>();

            builder.RegisterType<RangeOuterBoundExtentService>().As<IRangeOuterBoundExtentService>();

            // These compilers are registered both by interface and keyed by EFacetType below
            // This is a temporary solution until a more elegant solution is found
            builder.RegisterType<DiscreteCategoryCountSqlCompiler>().As<IDiscreteCategoryCountSqlCompiler>();
            builder.RegisterType<RangeCategoryCountSqlCompiler>().As<IRangeCategoryCountSqlCompiler>();

            builder.RegisterType<UndefinedPickFilterCompiler>().Keyed<IPickFilterCompiler>(EFacetType.Unknown);

            builder.RegisterType<DiscreteCategoryCountSqlCompiler>().Keyed<ICategoryCountSqlCompiler>(EFacetType.Discrete);
            builder.RegisterType<DiscreteFacetPickFilterCompiler>().Keyed<IPickFilterCompiler>(EFacetType.Discrete);
            builder.RegisterType<DiscreteCategoryCountHelper>().Keyed<ICategoryCountHelper>(EFacetType.Discrete);
            builder.RegisterType<DiscreteCategoryInfoService>().Keyed<ICategoryInfoService>(EFacetType.Discrete);


            builder.RegisterType<RangeCategoryCountSqlCompiler>().Keyed<ICategoryCountSqlCompiler>(EFacetType.Range);
            builder.RegisterType<RangeFacetPickFilterCompiler>().Keyed<IPickFilterCompiler>(EFacetType.Range);
            builder.RegisterType<RangeCategoryCountHelper>().Keyed<ICategoryCountHelper>(EFacetType.Range);
            builder.RegisterType<RangeCategoryInfoService>().Keyed<ICategoryInfoService>(EFacetType.Range);

            builder.RegisterType<GeoPolygonCategoryCountSqlCompiler>().Keyed<ICategoryCountSqlCompiler>(EFacetType.GeoPolygon);
            builder.RegisterType<GeoPolygonPickFilterCompiler>().Keyed<IPickFilterCompiler>(EFacetType.GeoPolygon);
            builder.RegisterType<DiscreteCategoryCountHelper>().Keyed<ICategoryCountHelper>(EFacetType.GeoPolygon);
            builder.RegisterType<GeoPolygonCategoryInfoService>().Keyed<ICategoryInfoService>(EFacetType.GeoPolygon);

            builder.RegisterType<RangesIntersectPickFilterCompiler>().Keyed<IPickFilterCompiler>(EFacetType.RangesIntersect);

            builder.RegisterType<PickFilterCompilerLocator>().As<IPickFilterCompilerLocator>();

            builder.RegisterType<PicksFilterCompiler>().As<IPicksFilterCompiler>();

            builder.RegisterType<CategoryCountService>().As<ICategoryCountService>();

            builder.RegisterType<ValidPicksSqCompiler>().As<IValidPicksSqlCompiler>();
            builder.RegisterType<JoinSqlCompiler>().As<IJoinSqlCompiler>();
            builder.RegisterType<JoinsClauseCompiler>().As<IJoinsClauseCompiler>();

            builder.RegisterType<DiscreteCategoryInfoSqlCompiler>().As<IDiscreteCategoryInfoSqlCompiler>();
            builder.RegisterType<GeoPolygonCategoryInfoSqlCompiler>().As<IGeoPolygonCategoryInfoSqlCompiler>();

            builder.RegisterType<RangeCategoryInfoSqlCompiler>().As<IRangeCategoryInfoSqlCompiler>();
            builder.RegisterType<RangeOuterBoundSqlCompiler>().As<IRangeOuterBoundSqlCompiler>();

            builder.RegisterType<FacetContentService>().As<IFacetContentService>();

            // Not used:
            // builder.RegisterType<RangeCategoryBoundSqlCompiler>().Keyed<ICategoryBoundSqlCompiler>(EFacetType.Range);

            builder.RegisterType<ResultService>().As<IResultService>();

            builder.RegisterType<CategoryCountPayloadService>().Keyed<IResultPayloadService>("map");
            builder.RegisterType<NullPayloadService>().Keyed<IResultPayloadService>("tabular");
            builder.RegisterType<ResultPayloadServiceLocator>().As<IResultPayloadServiceLocator>();

            builder.RegisterType<TabularResultSqlCompiler>().Keyed<IResultSqlCompiler>("tabular");
            builder.RegisterType<MapResultSqlCompiler>().Keyed<IResultSqlCompiler>("map");
            builder.RegisterType<ResultSqlCompilerLocator>().As<IResultSqlCompilerLocator>();

            builder.Register(_ => GetCache(Options?.Store)).SingleInstance().ExternallyOwned();
            if (Options.Store.UseRedisCache)
            {
                builder.RegisterType<SeadQueryAPI.Services.LoadFacetWithCachingService>().As<SeadQueryAPI.Services.ILoadFacetService>();
                builder.RegisterType<SeadQueryAPI.Services.LoadResultWithCachingService>().As<SeadQueryAPI.Services.ILoadResultService>();
            }
            else
            {
                builder.RegisterType<SeadQueryAPI.Services.LoadFacetService>().As<SeadQueryAPI.Services.ILoadFacetService>();
                builder.RegisterType<SeadQueryAPI.Services.LoadResultService>().As<SeadQueryAPI.Services.ILoadResultService>();
            }
        }

        public static IContainer CreateContainer(IFacetContext facetContext, string jsonFolder, ISetting options)
        {
            var builder = new Autofac.ContainerBuilder();
            var fixture = new JsonFacetContextFixture(jsonFolder);
            var dependencyService = new DependencyService(fixture) { FacetContext = facetContext, Options = options };
            dependencyService.Load(builder);
            return builder.Build();
        }
    }
}
