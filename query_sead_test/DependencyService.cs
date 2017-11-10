using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.AggregateService;
using CacheManager.Core;
using DataAccessPostgreSqlProvider;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Npgsql.Logging;
using QuerySeadAPI;
using QuerySeadDomain;
using QuerySeadDomain.QueryBuilder;
using System.Diagnostics;

namespace QuerySeadTests
{
    public class TestDependencyService : DependencyService
    {
        public TestDependencyService()
        {
        }

        public override ICacheManager<object> GetCacheManager()
        {
            var cache = new QueryCacheFactory().Create();
            return cache;
        }

        public override IContainer Register(IServiceCollection services, IQueryBuilderSetting options)
        {
            var builder = new Autofac.ContainerBuilder();

            builder.RegisterInstance<IQueryBuilderSetting>(options).SingleInstance().ExternallyOwned();
            builder.Register(c => GetCacheManager()).SingleInstance().ExternallyOwned();
            builder.RegisterAggregateService<IQueryCache>();

            //DomainModelDbContext context = new DomainModelDbContext(options);
            //IUnitOfWork unitOfWork = new UnitOfWork(context);

            builder.RegisterType<DomainModelDbContext>().SingleInstance().SingleInstance();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().SingleInstance().ExternallyOwned();

            builder.RegisterType<FacetGraphFactory>().As<IFacetGraphFactory>().InstancePerLifetimeScope();
            builder.Register<IFacetsGraph>(c => c.Resolve<IFacetGraphFactory>().Build());

            builder.RegisterType<QuerySetupBuilder>().As<IQuerySetupBuilder>();
            builder.RegisterType<DeleteBogusPickService>().As<IDeleteBogusPickService>();

            builder.RegisterType<RangeCategoryBoundsService>().As<ICategoryBoundsService>();

            builder.RegisterType<UndefinedFacetPickFilterCompiler>().Keyed<IFacetPickFilterCompiler>(0);
            builder.RegisterType<DiscreteFacetPickFilterCompiler>().Keyed<IFacetPickFilterCompiler>(1);
            builder.RegisterType<RangeFacetPickFilterCompiler>().Keyed<IFacetPickFilterCompiler>(2);
            builder.RegisterType<GeoFacetPickFilterCompiler>().Keyed<IFacetPickFilterCompiler>(3);

            builder.RegisterType<RangeCategoryCountService>().Keyed<ICategoryCountService>(EFacetType.Range);
            builder.RegisterType<DiscreteCategoryCountService>().Keyed<ICategoryCountService>(EFacetType.Discrete);

            builder.RegisterType<RangeFacetContentService>().Keyed<IFacetContentService>(EFacetType.Range);
            builder.RegisterType<DiscreteFacetContentService>().Keyed<IFacetContentService>(EFacetType.Discrete);

            builder.RegisterType<ResultQueryCompiler>().As<IResultQueryCompiler>();

            builder.RegisterAggregateService<IControllerServiceAggregate>();

            builder.RegisterType<RangeCategoryBoundSqlQueryBuilder>().Keyed<ICategoryBoundSqlQueryBuilder>(EFacetType.Range);

            builder.RegisterType<DefaultResultService>().Keyed<IResultService>("tabular");
            builder.RegisterType<MapResultService>().Keyed<IResultService>("map");

            builder.RegisterType<TabularResultSqlQueryBuilder>().Keyed<IResultSqlQueryCompiler>("tabular");
            builder.RegisterType<MapResultSqlQueryBuilder>().Keyed<IResultSqlQueryCompiler>("map");

            if (options.Store.UseRedisCache) {
                builder.RegisterType<QuerySeadAPI.Services.CachedLoadFacetService>().As<QuerySeadAPI.Services.ILoadFacetService>();
                builder.RegisterType<QuerySeadAPI.Services.CachedLoadResultService>().As<QuerySeadAPI.Services.ILoadResultService>();
            } else {
                builder.RegisterType<QuerySeadAPI.Services.LoadFacetService>().As<QuerySeadAPI.Services.ILoadFacetService>();
                builder.RegisterType<QuerySeadAPI.Services.LoadResultService>().As<QuerySeadAPI.Services.ILoadResultService>();
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
