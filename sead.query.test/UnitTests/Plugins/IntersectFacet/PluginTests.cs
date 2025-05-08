using Autofac;
using SeadQueryCore;
using SeadQueryCore.Plugin.Intersect;
using SQT.Infrastructure;
using SQT.SQL.Matcher;
using Xunit;

namespace SQT.Plugins.Intersect
{


    [Collection("UsePostgresFixture")]
    public class PluginTests() : IntegrationTestBase()
    {

        [Fact]
        public void Plugin_CanResolve_Test()
        {

            IFacetPlugin plugin = Container.Resolve<IIntersectFacetPlugin>();

            Assert.NotNull(plugin);

            plugin = Container.ResolveKeyed<IFacetPlugin>(EFacetType.Intersect);

            Assert.NotNull(plugin);

        }

    }
}
