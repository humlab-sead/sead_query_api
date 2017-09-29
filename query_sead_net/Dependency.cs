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

namespace QuerySeadAPI {

    public interface IControllerServiceAggregate
    {
        IQueryBuilderSetting Setting { get; set; }
        IQueryCache QueryCache { get; set; }
        IUnitOfWork UnitOfWork { get; set; }
    }

    public class DependencyService
    {
        public virtual ICacheManager<object> GetCacheManager()
        {
            return new QueryCacheFactory().Create();
        }

        public virtual IContainer Register(IServiceCollection services, IQueryBuilderSetting options)
        {
            var builder = new Autofac.ContainerBuilder();

            // http://docs.autofac.org/en/latest/register/registration.html

            builder.RegisterInstance<IQueryBuilderSetting>(options).SingleInstance().ExternallyOwned();
            builder.Register(c => GetCacheManager()).SingleInstance().ExternallyOwned();
            builder.RegisterAggregateService<IQueryCache>();

            builder.RegisterType<DomainModelDbContext>().SingleInstance().InstancePerLifetimeScope();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();
            builder.Register<IFacetsGraph>(c => new FacetGraphFactory().Build(c.Resolve<IUnitOfWork>()));
            builder.RegisterType<QuerySetupBuilder>().As<IQuerySetupBuilder>();
            builder.RegisterType<DeleteBogusPickService>().As<IDeleteBogusPickService>();

            builder.RegisterType<RangeCategoryBoundsService>().As<ICategoryBoundsService>();

            #region __Count Services__
            builder.RegisterType<RangeCategoryCountService>().Keyed<ICategoryCountService>(EFacetType.Range);
            builder.RegisterType<DiscreteCategoryCountService>().Keyed<ICategoryCountService>(EFacetType.Discrete);

            //builder.RegisterAggregateService<ICategoryCountServiceAggregate>();
            //builder.RegisterType<RangeCategoryCountService>();
            //builder.RegisterType<DiscreteCategoryCountService>();
            #endregion

            #region __Content Services__

            // alt #1 using index
            builder.RegisterType<RangeFacetContentService>().Keyed<IFacetContentService>(EFacetType.Range);
            builder.RegisterType<DiscreteFacetContentService>().Keyed<IFacetContentService>(EFacetType.Discrete);

            // alt #2 using an aggregate
            //builder.RegisterAggregateService<IFacetContentServiceAggregate>();
            //builder.RegisterType<RangeFacetContentService>();
            //builder.RegisterType<DiscreteFacetContentService>();
            #endregion

            builder.RegisterType<ResultQueryCompiler>().As<IResultQueryCompiler>();

            builder.RegisterAggregateService<IControllerServiceAggregate>();

            #region __Result Services__
            builder.RegisterType<ResultService>().Keyed<IResultService>("tabular");
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
