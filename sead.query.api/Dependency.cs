using Autofac;
using SeadQueryAPI.Serializers;
using SeadQueryCore;
using SeadQueryCore.QueryBuilder;
using SeadQueryCore.Services.Result;
using SeadQueryInfra;
using System;

namespace SeadQueryAPI
{
    public class DependencyService : Module
    {
        public ISetting Options { get; set; }

        public virtual ISeadQueryCache GetCache(StoreSetting settings)
        {
            try
            {
                if (settings?.UseRedisCache == true)
                    return new RedisCacheFactory().Create(settings.CacheHost, settings.CachePort);
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine("Failed to connect to Redis!");
            }
            Console.WriteLine("Warning: Using in memory cache provider!");
            return new MemoryCacheFactory().Create();
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance<ISetting>(Options).SingleInstance().ExternallyOwned();
            builder.RegisterInstance<IFacetSetting>(Options.Facet).SingleInstance().ExternallyOwned();
            builder.RegisterInstance<StoreSetting>(Options.Store).SingleInstance().ExternallyOwned();

            builder.RegisterType<FacetContextFactory>()
                .As<IFacetContextFactory>()
                .InstancePerLifetimeScope();

            builder.Register(c => c.Resolve<IFacetContextFactory>().GetInstance())
                .As<IFacetContext>()
                .InstancePerLifetimeScope();

            builder.Register(c => c.Resolve<IFacetContext>().TypedQueryProxy)
                .As<ITypedQueryProxy>()
                .InstancePerLifetimeScope();

            builder.Register(c => c.Resolve<IFacetContext>().DynamicQueryProxy)
                .As<IDynamicQueryProxy>()
                .InstancePerLifetimeScope();

            builder.RegisterType<RepositoryRegistry>().As<IRepositoryRegistry>().InstancePerLifetimeScope();

            builder.RegisterType<FacetGraphFactory>().As<IFacetGraphFactory>().InstancePerLifetimeScope();
            builder.Register<IFacetsGraph>(c => c.Resolve<IFacetGraphFactory>().Build()).InstancePerLifetimeScope();

            builder.RegisterType<QuerySetupBuilder>().As<IQuerySetupBuilder>();
            builder.RegisterType<BogusPickService>().As<IBogusPickService>();
            builder.RegisterType<FacetConfigReconstituteService>().As<IFacetConfigReconstituteService>();
            builder.RegisterType<ResultConfigReconstituteService>().As<IResultConfigReconstituteService>();

            builder.RegisterType<RangeOuterBoundExtentService>().As<IRangeOuterBoundExtentService>();

            // FIXME: Use an abstract factory per EFacetType instead => closer to a plugin architecture

            // builder.RegisterType<DiscretePlugin>().Keyed<IServicesPlugin>(EFacetType.Range);
            // builder.RegisterType<RangePlugin>().Keyed<IServicesPlugin>(EFacetType.Discrete);
            // etc

            builder.RegisterType<UndefinedPickFilterCompiler>().Keyed<IPickFilterCompiler>(EFacetType.Unknown);
            builder.RegisterType<DiscreteFacetPickFilterCompiler>().Keyed<IPickFilterCompiler>(EFacetType.Discrete);
            builder.RegisterType<RangeFacetPickFilterCompiler>().Keyed<IPickFilterCompiler>(EFacetType.Range);
            builder.RegisterType<GeoPolygonPickFilterCompiler>().Keyed<IPickFilterCompiler>(EFacetType.GeoPolygon);
            builder.RegisterType<RangesIntersectPickFilterCompiler>().Keyed<IPickFilterCompiler>(EFacetType.RangesIntersect);
            builder.RegisterType<PickFilterCompilerLocator>().As<IPickFilterCompilerLocator>();
            
            builder.RegisterType<PicksFilterCompiler>().As<IPicksFilterCompiler>();

            builder.RegisterType<DiscreteCategoryCountHelper>().Keyed<ICategoryCountHelper>(EFacetType.Discrete);
            builder.RegisterType<RangeCategoryCountHelper>().Keyed<ICategoryCountHelper>(EFacetType.Range);
            // builder.RegisterType<GeoPolygonCategoryCountHelper>().Keyed<ICategoryCountHelper>(EFacetType.GeoPolygon);
            // builder.RegisterType<RangesIntersectCategoryCountHelper>().Keyed<ICategoryCountHelper>(EFacetType.RangesIntersect);
            builder.RegisterType<CategoryCountService>().As<ICategoryCountService>();
            // builder.RegisterType<CategoryCountServiceLocator>().As<ICategoryCountServiceLocator>();

            builder.RegisterType<ValidPicksSqCompiler>().As<IValidPicksSqlCompiler>();
            builder.RegisterType<JoinSqlCompiler>().As<IJoinSqlCompiler>();
            builder.RegisterType<JoinsClauseCompiler>().As<IJoinsClauseCompiler>();

            builder.RegisterType<DiscreteContentSqlCompiler>().As<IDiscreteContentSqlCompiler>();

            // These compilers are registered both by interface and keyed by EFacetType
            // This is a temporary solution until a more elegant solution is found
            builder.RegisterType<DiscreteCategoryCountSqlCompiler>().As<IDiscreteCategoryCountSqlCompiler>();
            builder.RegisterType<RangeCategoryCountSqlCompiler>().As<IRangeCategoryCountSqlCompiler>();

            builder.RegisterType<DiscreteCategoryCountSqlCompiler>().Keyed<ICategoryCountSqlCompiler>(EFacetType.Discrete);
            builder.RegisterType<RangeCategoryCountSqlCompiler>().Keyed<ICategoryCountSqlCompiler>(EFacetType.Range);
            // builder.RegisterType<GeoPolygonCategoryCountSqlCompiler>().Keyed<ICategoryCountSqlCompiler>(EFacetType.GeoPolygon);
            // builder.RegisterType<RangesIntersecctCategoryCountSqlCompiler>().Keyed<ICategoryCountSqlCompiler>(EFacetType.RangesIntersect);

            builder.RegisterType<RangeIntervalSqlCompiler>().As<IRangeIntervalSqlCompiler>();
            builder.RegisterType<RangeOuterBoundSqlCompiler>().As<IRangeOuterBoundSqlCompiler>();

            builder.RegisterType<RangeFacetContentService>().Keyed<IFacetContentService>(EFacetType.Range);
            builder.RegisterType<DiscreteFacetContentService>().Keyed<IFacetContentService>(EFacetType.Discrete);
            // builder.RegisterType<GeoPolygonFacetContentService>().Keyed<IFacetContentService>(EFacetType.GeoPolygon);
            // builder.RegisterType<RangesIntersectFacetContentService>().Keyed<IFacetContentService>(EFacetType.RangesIntersect);
            builder.RegisterType<FacetContentServiceLocator>().As<IFacetContentServiceLocator>();

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
                builder.RegisterType<Services.CachedLoadFacetService>().As<Services.ILoadFacetService>();
                builder.RegisterType<Services.CachedLoadResultService>().As<Services.ILoadResultService>();
            }
            else
            {
                builder.RegisterType<Services.LoadFacetService>().As<Services.ILoadFacetService>();
                builder.RegisterType<Services.LoadResultService>().As<Services.ILoadResultService>();
            }
        }
    }
}
