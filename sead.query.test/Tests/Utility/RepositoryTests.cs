using Autofac;
using SeadQueryCore;
using SeadQueryInfra;
using SeadQueryTest;
using System.Linq;
using Xunit;

namespace SeadQueryTest2.Repository
{

    public class RepositoryTests {

        [Fact]
        public void TestResolveUnitOfWork()
        {
            var builder = new ContainerBuilder();

            ISetting setting = (ISetting)new SettingFactory().Create();
            builder.RegisterInstance<ISetting>(setting).SingleInstance().ExternallyOwned();
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
