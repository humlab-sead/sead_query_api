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
        private bool clear_cache;

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

            DomainModelDbContext context = new DomainModelDbContext(options);
            IUnitOfWork unitOfWork = new UnitOfWork(context);

            //builder.Register(c => context).SingleInstance();
            //builder.Register(c => unitOfWork).SingleInstance();

            builder.RegisterType<DomainModelDbContext>().SingleInstance().SingleInstance();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().SingleInstance().ExternallyOwned();

            //builder.Register(c => GetCacheManager()).SingleInstance().ExternallyOwned();

            builder.Register<IFacetsGraph>(c => new FacetGraphFactory().Build(c.Resolve<IUnitOfWork>()));
            builder.RegisterType<QuerySetupBuilder>().As<IQuerySetupBuilder>();
            builder.RegisterType<DeleteBogusPickService>().As<IDeleteBogusPickService>();

            builder.RegisterType<RangeCategoryBoundsService>().As<ICategoryBoundsService>();

            builder.RegisterType<RangeCategoryCountService>().Keyed<ICategoryCountService>(EFacetType.Range);
            builder.RegisterType<DiscreteCategoryCountService>().Keyed<ICategoryCountService>(EFacetType.Discrete);

            builder.RegisterType<RangeFacetContentService>().Keyed<IFacetContentService>(EFacetType.Range);
            builder.RegisterType<DiscreteFacetContentService>().Keyed<IFacetContentService>(EFacetType.Discrete);

            builder.RegisterAggregateService<IQuerySetupCompilers>();
            builder.RegisterType<ResultQueryCompiler>();
            builder.RegisterType<MapQuerySetupCompiler>();

            // QuerySeadDomain.IQuerySetupCompiler

            builder.RegisterAggregateService<IControllerServiceAggregate>();

            builder.RegisterType<ResultService>().Keyed<IResultService>("tabular");
            builder.RegisterType<MapResultService>().Keyed<IResultService>("map");

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
