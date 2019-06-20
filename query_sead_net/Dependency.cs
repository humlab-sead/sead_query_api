using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.AggregateService;
using CacheManager.Core;
using DataAccessPostgreSqlProvider;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QuerySeadDomain;
using QuerySeadDomain.QueryBuilder;
using System;

namespace QuerySeadAPI {

    //public interface IControllerServiceAggregate
    //{
    //    IQueryBuilderSetting Setting { get; set; }
    //    ICacheContainer QueryCache { get; set; }
    //    IUnitOfWork UnitOfWork { get; set; }
    //}

    public class DependencyService
    {

        public virtual ICache GetCache(StoreSetting settings)
        {
            try {
                if (settings?.UseRedisCache == true)
                    return new RedisCacheProvider();
            } catch (InvalidOperationException) {
                Console.WriteLine("Failed to connect to Redis!");
            }
            Console.WriteLine("Warning: Using in memoty cache provider!");
            return new SimpleMemoryCacheProvider();
        }

        public virtual IContainer Register(IServiceCollection services, IQueryBuilderSetting options)
        {
            var builder = new Autofac.ContainerBuilder();

            // http://docs.autofac.org/en/latest/register/registration.html

            builder.RegisterInstance<IQueryBuilderSetting>(options).SingleInstance().ExternallyOwned();

            // builder.Register(c => GetCacheManager(options?.Store)).SingleInstance().ExternallyOwned();
            // builder.RegisterAggregateService<ICacheContainer>();
            builder.Register(z => GetCache(options?.Store)).SingleInstance().ExternallyOwned();

            builder.RegisterType<DomainModelDbContext>().SingleInstance().InstancePerLifetimeScope();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();

            builder.RegisterType<FacetGraphFactory>().As<IFacetGraphFactory>().InstancePerLifetimeScope();
            builder.Register<IFacetsGraph>(c => c.Resolve<IFacetGraphFactory>().Build());

            builder.RegisterType<QuerySetupBuilder>().As<IQuerySetupBuilder>();
            builder.RegisterType<DeleteBogusPickService>().As<IDeleteBogusPickService>();

            builder.RegisterType<RangeCategoryBoundsService>().As<ICategoryBoundsService>();

            builder.RegisterType<UndefinedFacetPickFilterCompiler>().Keyed<IFacetPickFilterCompiler>(0);
            builder.RegisterType<DiscreteFacetPickFilterCompiler>().Keyed<IFacetPickFilterCompiler>(1);
            builder.RegisterType<RangeFacetPickFilterCompiler>().Keyed<IFacetPickFilterCompiler>(2);
            builder.RegisterType<GeoFacetPickFilterCompiler>().Keyed<IFacetPickFilterCompiler>(3);

            #region __Count Services__
            builder.RegisterType<RangeCategoryCountService>().Keyed<ICategoryCountService>(EFacetType.Range);
            builder.RegisterType<DiscreteCategoryCountService>().Keyed<ICategoryCountService>(EFacetType.Discrete);

            //builder.RegisterAggregateService<ICategoryCountServiceAggregate>();
            //builder.RegisterType<RangeCategoryCountService>();
            //builder.RegisterType<DiscreteCategoryCountService>();
            #endregion

            builder.RegisterType<RangeFacetContentService>().Keyed<IFacetContentService>(EFacetType.Range);
            builder.RegisterType<DiscreteFacetContentService>().Keyed<IFacetContentService>(EFacetType.Discrete);

            builder.RegisterType<ResultQueryCompiler>().As<IResultQueryCompiler>();

            //builder.RegisterAggregateService<IControllerServiceAggregate>();

            builder.RegisterType<RangeCategoryBoundSqlQueryBuilder>().Keyed<ICategoryBoundSqlQueryBuilder>(EFacetType.Range);

            #region __Result Services__
            builder.RegisterType<DefaultResultService>().Keyed<IResultService>("tabular");
            builder.RegisterType<MapResultService>().Keyed<IResultService>("map");

            builder.RegisterType<TabularResultSqlQueryBuilder>().Keyed<IResultSqlQueryCompiler>("tabular");
            builder.RegisterType<MapResultSqlQueryBuilder>().Keyed<IResultSqlQueryCompiler>("map");

            #endregion

            /* App Services */

            if (options.Store.UseRedisCache) {
                builder.RegisterType<Services.CachedLoadFacetService>().As<Services.ILoadFacetService>();
                builder.RegisterType<Services.CachedLoadResultService>().As<Services.ILoadResultService>();
            } else {
                builder.RegisterType<Services.LoadFacetService>().As<Services.ILoadFacetService>();
                builder.RegisterType<Services.LoadResultService>().As<Services.ILoadResultService>();
            }
            if (services != null)
                builder.Populate(services);

            var container = builder.Build();

            return container;
        }
    }
}
