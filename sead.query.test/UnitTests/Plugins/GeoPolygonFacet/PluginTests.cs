using Autofac;
using SeadQueryCore;
using SeadQueryCore.Plugin.GeoPolygon;
using SQT.Infrastructure;
using SQT.SQL.Matcher;
using Xunit;

namespace SQT.Plugins.GeoPolygon
{


    [Collection("UsePostgresFixture")]
    public class PluginTests() : IntegrationTestBase()
    {

        [Fact]
        public void Plugin_CanResolve_Test()
        {

            IFacetPlugin plugin = Container.Resolve<IGeoPolygonFacetPlugin>();

            Assert.NotNull(plugin);

            plugin = Container.ResolveKeyed<IFacetPlugin>(EFacetType.GeoPolygon);

            Assert.NotNull(plugin);

        }

    }
}
