using Moq;
using SeadQueryCore;
using SeadQueryCore.Model;
using SeadQueryCore.Services.Result;
using SQT.Infrastructure;
using System.Data;
using Xunit;

namespace SQT.Services
{
    [Collection("JsonSeededFacetContext")]
    public class DefaultResultServiceTests : DisposableFacetContextContainer
    {

        public DefaultResultServiceTests(JsonSeededFacetContextFixture fixture) : base(fixture)
        {
        }

        [Fact(Skip = "Not implemented")]
        public void Load_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var resultConfigCompiler = MockResultConfigCompiler("SQL", "result_facet");
            //var fakeResult = null;
            var queryProxy = new MockDynamicQueryProxyFactory().Create((DataTable)null);

            var service = new DefaultResultService(Registry, resultConfigCompiler.Object, queryProxy.Object);

            FacetsConfig2 facetsConfig = null;
            ResultConfig resultConfig = null;

            // Act
            var result = service.Load(facetsConfig, resultConfig);

            // Assert
            Assert.True(false);
        }

    }
}
