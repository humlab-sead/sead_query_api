using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.AggregateService;
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

        public override IContainer Register(IServiceCollection services, IQueryBuilderSetting options)
        {
            var builder = new Autofac.ContainerBuilder();

            builder.RegisterInstance<IQueryBuilderSetting>(options).SingleInstance().ExternallyOwned();
            builder.Register(c => GetCacheManager()).SingleInstance().ExternallyOwned();
            builder.RegisterAggregateService<IQueryCache>();

            DomainModelDbContext context = new DomainModelDbContext(options);
            IUnitOfWork unitOfWork = new UnitOfWork(context);

            builder.Register(c => context).SingleInstance();
            builder.Register(c => unitOfWork).SingleInstance();

            //builder.RegisterType<DomainModelDbContext>().SingleInstance().SingleInstance();
            //builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().SingleInstance();

            builder.Register(c => GetCacheManager()).SingleInstance().ExternallyOwned();

            builder.Register<IFacetsGraph>(c => new FacetGraphFactory().Build(c.Resolve<IUnitOfWork>()));
            builder.RegisterType<QuerySetupBuilder>().As<IQuerySetupBuilder>();
            builder.RegisterType<DeleteBogusPickService>().As<IDeleteBogusPickService>();

            builder.RegisterType<RangeCategoryBoundsService>().As<ICategoryBoundsService>();

            builder.RegisterType<RangeCategoryCountService>().Keyed<ICategoryCountService>(EFacetType.Range);
            builder.RegisterType<DiscreteCategoryCountService>().Keyed<ICategoryCountService>(EFacetType.Discrete);

            builder.RegisterType<RangeFacetContentService>().Keyed<IFacetContentService>(EFacetType.Range);
            builder.RegisterType<DiscreteFacetContentService>().Keyed<IFacetContentService>(EFacetType.Discrete);

            builder.RegisterAggregateService<IQuerySetupCompilers>();
            builder.RegisterType<DefaultQuerySetupCompiler>();
            builder.RegisterType<MapQuerySetupCompiler>();

            builder.RegisterAggregateService<IControllerServiceAggregate>();

            builder.RegisterAggregateService<IResultServiceAggregate>();
            builder.RegisterType<ResultService>();
            builder.RegisterType<MapResultService>();

            builder.RegisterType<QuerySeadAPI.Services.LoadFacetService>().As<QuerySeadAPI.Services.ILoadFacetService>();

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

    [TestClass]
    public class Startup
    {
        public static IConfigurationRoot Configuration;
        public static QueryBuilderSetting Options;

        [AssemblyInitialize()]
        public static void AssemblyInit(TestContext context)
        {
            Configure(context);
            ConfigureServices(context);
        }

        public static void Configure(TestContext context)
        {
            NpgsqlLogManager.Provider = new ConsoleLoggingProvider(NpgsqlLogLevel.Debug, false, false);
            var builder = new ConfigurationBuilder()
                .SetBasePath(System.AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile("appsettings.test.json", optional: true);
            Configuration = builder.Build();
            Options = Configuration.GetSection("QueryBuilderSetting").Get<QueryBuilderSetting>();
        }

        public static void ConfigureServices(TestContext context)
        {
        }

        [AssemblyCleanup()]
        public static void AssemblyCleanup()
        {
            Debug.WriteLine("AssemblyCleanup");
        }
    }
}
