using System.Collections.Generic;
using SeadQueryInfra.DataAccessProvider;
using System.Linq;
using SeadQueryCore;
using Autofac;
using SeadQueryInfra;
using SeadQueryTest.Infrastructure;
using SeadQueryTest;
using Xunit;
using SeadQueryTest.Mocks;
using Microsoft.Extensions.Configuration;

namespace SeadQueryTest2.Repository {

    public class RepositoryTests {

        [Fact]
        public void TestResolveUnitOfWork()
        {
            var builder = new ContainerBuilder();

            builder.RegisterInstance<ISetting>(ScaffoldUtility.LoadSettings()).SingleInstance().ExternallyOwned();
            builder.RegisterType<FacetContext>().As<IFacetContext>().SingleInstance();
            builder.RegisterType<RepositoryRegistry>().As<IRepositoryRegistry>();

            using (var container = builder.Build())
            using (var scope = container.BeginLifetimeScope()) {
                var service = scope.Resolve<IRepositoryRegistry>();
                Assert.True(service.Facets.GetAll().Any());
            }
        }
    }
}
