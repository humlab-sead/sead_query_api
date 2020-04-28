using Moq;
using SeadQueryCore;
using SeadQueryCore.Model;
using SeadQueryCore.Services.Result;
using SQT.Infrastructure;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Xunit;

namespace SQT.Services
{
    [Collection("JsonSeededFacetContext")]
    public class DefaultResultServiceTests : DisposableFacetContextContainer
    {

        public DefaultResultServiceTests(JsonSeededFacetContextFixture fixture) : base(fixture)
        {
        }
        public virtual List<CategoryCountItem> FakeResultItems(int count)
        {
            var fakeResult = new DiscreteCountDataReaderBuilder()
                .CreateNewTable()
                .GenerateBogusRows(count)
                .ToItems<CategoryCountItem>().ToList();
            return fakeResult;
        }

        [Theory]
        [InlineData("sites:sites")]
        public void Load_VariousConfigs_Success(string uri)
        {
            // Arrange
            var resultConfigCompiler = MockResultConfigCompiler("SQL", "result_facet");
            FacetsConfig2 facetsConfig = FakeFacetsConfig(uri);
            ResultConfig resultConfig = FakeResultConfig("site_level", "tabular");

            var fakeResult = FakeResultItems(10);
            var queryProxy = new MockDynamicQueryProxyFactory().Create((DataTable)null);

            // Act
            var service = new DefaultResultService(FakeRegistry(), resultConfigCompiler.Object, queryProxy.Object);
            var result = service.Load(facetsConfig, resultConfig);

            // Assert
            Assert.True(false);
        }

    }
}
