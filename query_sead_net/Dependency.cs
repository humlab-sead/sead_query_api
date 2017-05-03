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
        public IContainer Register(IServiceCollection services)
        {
            var builder = new Autofac.ContainerBuilder();

            // http://docs.autofac.org/en/latest/register/registration.html
            
            builder.RegisterInstance(new SettingFactory().Create()).SingleInstance().ExternallyOwned();
            builder.RegisterInstance<IQueryCache>(new QueryCacheFactory().Create()).SingleInstance().ExternallyOwned();

            builder.RegisterType<DomainModelDbContext>().SingleInstance();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();
            builder.Register<IFacetsGraph>(c => new FacetGraphFactory().Build(c.Resolve<IUnitOfWork>()));
            builder.RegisterType<QuerySetupBuilder>().As<IQuerySetupBuilder>();
            builder.RegisterType<DeleteBogusPickService>().As<IDeleteBogusPickService>();

            builder.RegisterType<RangeCategoryBoundsService>().As<ICategoryBoundsService>();

            builder.RegisterAggregateService<ICategoryCountServiceAggregate>();
            builder.RegisterType<RangeCategoryCountService>();
            builder.RegisterType<DiscreteCategoryCountService>();

            builder.RegisterAggregateService<IFacetContentServiceAggregate>();
            builder.RegisterType<RangeFacetContentService>();
            builder.RegisterType<DiscreteFacetContentService>();

            builder.RegisterAggregateService<IQuerySetupCompilers>();
            builder.RegisterType<DefaultQuerySetupCompiler>();
            builder.RegisterType<MapQuerySetupCompiler>();

            builder.RegisterAggregateService<IControllerServiceAggregate>();

            builder.Populate(services);

            var container = builder.Build();

            return container;
        }
        //builder.RegisterType<RangeCategoryCountService>().As<ICategoryCountService>().Keyed<EFacetType>(EFacetType.Range);
        //builder.RegisterType<DiscreteCategoryCountService>().As<ICategoryCountService>().Keyed<EFacetType>(EFacetType.Discrete);
   }
}
