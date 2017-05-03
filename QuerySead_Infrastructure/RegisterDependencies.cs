using Autofac;
using Autofac.Extras.AggregateService;
using DataAccessPostgreSqlProvider;
using Microsoft.Extensions.Configuration;
using QueryFacetDomain;
using QueryFacetDomain.QueryBuilder;
using System;
using System.Collections.Generic;
using System.Text;

namespace QueryFacetTest
{
    public class TestConfiguration {

        public static IQueryBuilderSetting Get()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();

            IQueryBuilderSetting setting = configuration
               .GetSection("QueryBuilderSetting")
               .Get<QueryBuilderSetting>();

            return setting;

        }
    }

    public class RegisterDependencies
    {
        public static IContainer Register()
        {
            var builder = new ContainerBuilder();

            // http://docs.autofac.org/en/latest/register/registration.html

            IQueryBuilderSetting setting = TestConfiguration.Get();

            builder.RegisterInstance(setting).As<IQueryBuilderSetting>().ExternallyOwned();
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

            //builder.RegisterType<RangeCategoryCountService>()
            //    .As<ICategoryCountService>()
            //    .Keyed<EFacetType>(EFacetType.Range);

            //builder.RegisterType<DiscreteCategoryCountService>()
            //    .As<ICategoryCountService>()
            //    .Keyed<EFacetType>(EFacetType.Discrete);

            //builder.RegisterType<RangeFacetContentService>()
            //     .As<IFacetContentService>()
            //     .Keyed<EFacetType>(EFacetType.Range);

            //builder.RegisterType<DiscreteFacetContentService>()
            //    .As<IFacetContentService>()
            //    .Keyed<EFacetType>(EFacetType.Discrete);

            builder.RegisterType<MapQuerySetupCompiler>()
                .As<IQuerySetupCompiler>()
                .Keyed<EResultViewType>(EResultViewType.Map);

            builder.RegisterType<DefaultQuerySetupCompiler>()
                .As<IQuerySetupCompiler>()
                .Keyed<EResultViewType>(EResultViewType.Tabular);

            var container = builder.Build();
            return container;
        }
    }

    //var builder = new ContainerBuilder();
    //builder.RegisterType<DomainModelDbContext>().SingleInstance();
    //builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();
    //var container = builder.Build();
    //using (var scope = container.BeginLifetimeScope()) {
    //    var service = scope.Resolve<IMyController>();
    //    service.CallMyDependent();
    //}
    //builder.RegisterType<FacetGraphFactory>().As<FacetGraphFactory>();
    //builder.Register(c => new ConfigReader("mysection")).As<IConfigReader>();
    //builder.RegisterType<FacetGraphFactory>().As<IFacetsGraph>()
    //    .OnActivating(e => { e.Instance.Dependent = e.Context.Resolve<IDependent>(); });
    //var output = new StringWriter();
    //builder.RegisterInstance(output).As<TextWriter>().ExternallyOwned();
    //builder.RegisterType<ConsoleLogger>();
    //builder.Register(c => new A(c.Resolve<B>()));
    //builder.Register(c => new A() { MyB = c.ResolveOptional<B>() });


}
