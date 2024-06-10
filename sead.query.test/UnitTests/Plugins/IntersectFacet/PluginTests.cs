using Autofac;
using SeadQueryCore;
using SeadQueryCore.Plugin.Intersect;
using SQT.Infrastructure;
using SQT.SQL.Matcher;
using Xunit;

namespace SQT.Plugins.Intersect
{
 

    [Collection("SeadJsonFacetContextFixture")]
    public class PluginTests(SeadJsonFacetContextFixture facetContextFixture): IntegrationTestBase(facetContextFixture)
    {

        [Fact]
        public void Plugin_CanResolve_Test() {

            IFacetPlugin plugin = Container.Resolve<IIntersectFacetPlugin>();

            Assert.NotNull(plugin);

            plugin = Container.ResolveKeyed<IFacetPlugin>(EFacetType.Intersect);

            Assert.NotNull(plugin);

        }

    }
}
