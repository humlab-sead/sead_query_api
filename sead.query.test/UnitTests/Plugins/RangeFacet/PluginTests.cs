using Autofac;
using SeadQueryCore;
using SeadQueryCore.Plugin.Range;
using SQT.Infrastructure;
using Xunit;

namespace SQT.Plugins.Range
{


    [Collection("UsePostgresFixture")]
    public class PluginTests() : IntegrationTestBase()
    {

        [Fact]
        public void Plugin_CanResolve_Test()
        {

            IFacetPlugin plugin = Container.Resolve<IRangeFacetPlugin>();

            Assert.NotNull(plugin);

            plugin = Container.ResolveKeyed<IFacetPlugin>(EFacetType.Range);

            Assert.NotNull(plugin);

        }
    }
}
