using Autofac;
using SeadQueryCore;
using SeadQueryCore.Plugin.Discrete;
using SQT.Infrastructure;
using Xunit;

namespace SQT.Plugins.Discrete
{
 

    [Collection("SeadJsonFacetContextFixture")]
    public class PluginTests(SeadJsonFacetContextFixture facetContextFixture): IntegrationTestBase(facetContextFixture)
    {

        [Fact]
        public void Plugin_CanResolve_Test() {

            IFacetPlugin plugin = Container.Resolve<IDiscreteFacetPlugin>();

            Assert.NotNull(plugin);

            plugin = Container.ResolveKeyed<IFacetPlugin>(EFacetType.Discrete);

            Assert.NotNull(plugin);

        }

    }
}
