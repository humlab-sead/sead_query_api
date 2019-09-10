using Autofac;
using Autofac.Extensions.DependencyInjection;
using DataAccessPostgreSqlProvider;
using Microsoft.Extensions.DependencyInjection;
using SeadQueryAPI;
using SeadQueryCore;
using SeadQueryCore.QueryBuilder;
using SeadQueryInfra;

namespace SeadQueryTest
{

    public class TestDependencyService : DependencyService
    {
        public TestDependencyService()
        {
        }

        public override ISeadQueryCache GetCache(StoreSetting settings)
        {
            return new NullCacheProvider();
        }

        public override IContainer Register(IServiceCollection services, IQueryBuilderSetting options)
        {
            options = options ?? (IQueryBuilderSetting)new MockOptionBuilder().Build().Value;

            var builder = new Autofac.ContainerBuilder();

            builder.RegisterInstance(options).SingleInstance().ExternallyOwned();
            builder.Register(c => GetCache(options?.Store)).SingleInstance().ExternallyOwned();

            builder.RegisterType<FacetContext>().As<IFacetContext>().SingleInstance().ExternallyOwned();
            builder.RegisterType<RepositoryRegistry>().As<IRepositoryRegistry>().SingleInstance().ExternallyOwned();

            builder.RegisterType<FacetGraphFactory>().As<IFacetGraphFactory>().InstancePerLifetimeScope();
            builder.Register<IFacetsGraph>(c => c.Resolve<IFacetGraphFactory>().Build());

            builder.RegisterType<QuerySetupBuilder>().As<IQuerySetupBuilder>();
            builder.RegisterType<DeleteBogusPickService>().As<IDeleteBogusPickService>();

            builder.RegisterType<RangeCategoryBoundsService>().As<ICategoryBoundsService>();

            builder.RegisterType<UndefinedFacetPickFilterCompiler>().Keyed<IPickFilterCompiler>(0);
            builder.RegisterType<DiscreteFacetPickFilterCompiler>().Keyed<IPickFilterCompiler>(1);
            builder.RegisterType<RangeFacetPickFilterCompiler>().Keyed<IPickFilterCompiler>(2);
            builder.RegisterType<GeoFacetPickFilterCompiler>().Keyed<IPickFilterCompiler>(3);

            builder.RegisterType<RangeCategoryCountService>().Keyed<ICategoryCountService>(EFacetType.Range);
            builder.RegisterType<DiscreteCategoryCountService>().Keyed<ICategoryCountService>(EFacetType.Discrete);

            builder.RegisterType<RangeFacetContentService>().Keyed<IFacetContentService>(EFacetType.Range);
            builder.RegisterType<DiscreteFacetContentService>().Keyed<IFacetContentService>(EFacetType.Discrete);

            builder.RegisterType<ResultQueryCompiler>().As<IResultQueryCompiler>();

            // builder.RegisterAggregateService<IControllerServiceAggregate>();

            builder.RegisterType<RangeCategoryBoundSqlQueryBuilder>().Keyed<ICategoryBoundSqlQueryBuilder>(EFacetType.Range);

            builder.RegisterType<DefaultResultService>().Keyed<IResultService>("tabular");
            builder.RegisterType<MapResultService>().Keyed<IResultService>("map");

            builder.RegisterType<TabularResultSqlQueryBuilder>().Keyed<IResultSqlQueryCompiler>("tabular");
            builder.RegisterType<MapResultSqlQueryBuilder>().Keyed<IResultSqlQueryCompiler>("map");

            if (options.Store.UseRedisCache) {
                builder.RegisterType<SeadQueryAPI.Services.CachedLoadFacetService>().As<SeadQueryAPI.Services.ILoadFacetService>();
                builder.RegisterType<SeadQueryAPI.Services.CachedLoadResultService>().As<SeadQueryAPI.Services.ILoadResultService>();
            } else {
                builder.RegisterType<SeadQueryAPI.Services.LoadFacetService>().As<SeadQueryAPI.Services.ILoadFacetService>();
                builder.RegisterType<SeadQueryAPI.Services.LoadResultService>().As<SeadQueryAPI.Services.ILoadResultService>();
            }
            if (services != null)
                builder.Populate(services);

            var container = builder.Build();

            return container;
        }

        public IContainer Register()
        {
            // This is a good place to injext stubs and mockups!
            return Register(null);
        }

        public IContainer Register(IServiceCollection services)
        {
            // This is a good place to injext stubs and mockups!
            return Register(services, Startup.Options);
        }
    }

}
