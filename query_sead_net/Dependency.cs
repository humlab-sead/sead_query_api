using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.AggregateService;
using CacheManager.Core;
using DataAccessPostgreSqlProvider;
using Microsoft.Extensions.DependencyInjection;
using QuerySeadDomain;
using QuerySeadDomain.QueryBuilder;

namespace QuerySeadAPI {

    public interface IControllerServiceAggregate
    {
        IQueryBuilderSetting Setting { get; set; }
        IQueryBuilderSetting IQueryCache { get; set; }
        IQueryBuilderSetting IUnitOfWork { get; set; }
    }

    public class DependencyService
    {
        public virtual IContainer Register(IServiceCollection services)
        {
            var builder = new Autofac.ContainerBuilder();

            // http://docs.autofac.org/en/latest/register/registration.html
            
            builder.RegisterInstance(new SettingFactory().Create()).SingleInstance().ExternallyOwned();
            // builder.RegisterInstance(new QueryCacheFactory().Create()).SingleInstance().ExternallyOwned();
            builder.Register(c => new QueryCacheFactory().Create()).SingleInstance().ExternallyOwned();

            builder.RegisterAggregateService<IQueryCache>();

            builder.RegisterType<DomainModelDbContext>().SingleInstance().InstancePerLifetimeScope();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();
            builder.Register<IFacetsGraph>(c => new FacetGraphFactory().Build(c.Resolve<IUnitOfWork>()));
            builder.RegisterType<QuerySetupBuilder>().As<IQuerySetupBuilder>();
            builder.RegisterType<DeleteBogusPickService>().As<IDeleteBogusPickService>();

            builder.RegisterType<RangeCategoryBoundsService>().As<ICategoryBoundsService>();

            #region __Count Services__
            builder.RegisterAggregateService<ICategoryCountServiceAggregate>();
            builder.RegisterType<RangeCategoryCountService>();
            builder.RegisterType<DiscreteCategoryCountService>();
            #endregion

            #region __Content Services__
            //builder.RegisterType<RangeFacetContentService>().As<IFacetContentService>().Keyed<EFacetType>(EFacetType.Range);
            //builder.RegisterType<DiscreteFacetContentService>().As<IFacetContentService>().Keyed<EFacetType>(EFacetType.Discrete);
            builder.RegisterAggregateService<IFacetContentServiceAggregate>();
            builder.RegisterType<RangeFacetContentService>();
            builder.RegisterType<DiscreteFacetContentService>();
            #endregion

            builder.RegisterAggregateService<IQuerySetupCompilers>();
            builder.RegisterType<DefaultQuerySetupCompiler>();
            builder.RegisterType<MapQuerySetupCompiler>();

            builder.RegisterAggregateService<IControllerServiceAggregate>();

            #region __Result Services__
            builder.RegisterAggregateService<IResultServiceAggregate>();
            builder.RegisterType<ResultService>();
            builder.RegisterType<MapResultService>();

            //builder.RegisterType<ResultService>().As<IResultService>().Keyed<EFacetType>(EResultViewType.Tabular);
            //builder.RegisterType<MapResultService>().As<IResultService>().Keyed<EFacetType>(EResultViewType.Map);
            #endregion

            /* App Services */
            builder.RegisterType<Services.LoadFacetService>().As<Services.ILoadFacetService>();

            if (services != null)
                builder.Populate(services);

            var container = builder.Build();

            return container;
        }
        //builder.RegisterType<RangeCategoryCountService>().As<ICategoryCountService>().Keyed<EFacetType>(EFacetType.Range);
        //builder.RegisterType<DiscreteCategoryCountService>().As<ICategoryCountService>().Keyed<EFacetType>(EFacetType.Discrete);
   }
}
