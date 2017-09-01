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
        public virtual IContainer Register(IServiceCollection services, IQueryBuilderSetting options)
        {
            var builder = new Autofac.ContainerBuilder();

            // http://docs.autofac.org/en/latest/register/registration.html

            builder.RegisterInstance<IQueryBuilderSetting>(options).SingleInstance().ExternallyOwned();
            builder.Register(c => new QueryCacheFactory().Create()).SingleInstance().ExternallyOwned();
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

            builder.RegisterAggregateService<IQuerySetupCompilers>();
            builder.RegisterType<DefaultQuerySetupCompiler>();
            builder.RegisterType<MapQuerySetupCompiler>();

            builder.RegisterAggregateService<IControllerServiceAggregate>();

            #region __Result Services__
            builder.RegisterAggregateService<IResultServiceAggregate>();
            builder.RegisterType<ResultService>();
            builder.RegisterType<MapResultService>();

            //builder.RegisterType<ResultService>().Keyed<IResultService>(EResultViewType.Tabular);
            //builder.RegisterType<MapResultService>().Keyed<IResultService>(EResultViewType.Map);
            #endregion

            /* App Services */
            builder.RegisterType<Services.LoadFacetService>().As<Services.ILoadFacetService>();

            if (services != null)
                builder.Populate(services);

            var container = builder.Build();

            return container;
        }
    }
}
