using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using DataAccessPostgreSqlProvider;
using System.Linq;
using SeadQueryCore;
using System.Diagnostics;
using Autofac;
using SeadQueryInfra;

namespace SeadQueryTest.Repository {

    public class RepositoryTests {

        IFacetContext context;
        readonly IContainer container;
        readonly IQueryBuilderSetting settings;

        public RepositoryTests()
        {
            container = new TestDependencyService().Register();
            settings = container.Resolve<IQueryBuilderSetting>();
        }

        [TestInitialize()]
        public void Initialize()
        {
            context = new FacetContext(settings);
        }

        [TestCleanup()]
        public void Cleanup()
        {
            context.Dispose();
        }

        [TestMethod]
        public void TestResolveUnitOfWork()
        {
            var builder = new ContainerBuilder();

            // http://docs.autofac.org/en/latest/register/registration.html

            builder.RegisterInstance<IQueryBuilderSetting>(Startup.Options).SingleInstance().ExternallyOwned();
            builder.RegisterType<FacetContext>().As<IFacetContext>().SingleInstance();
            builder.RegisterType<RepositoryRegistry>().As<IRepositoryRegistry>();

            var container = builder.Build();
            using (var scope = container.BeginLifetimeScope())
            {
                var service = scope.Resolve<IRepositoryRegistry>();
                Assert.IsTrue(service.Facets.GetAll().Count() > 0);
            }
        }

    }
}
