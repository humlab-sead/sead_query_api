using Moq;
using SeadQueryCore;
using SeadQueryCore.Model;
using SeadQueryCore.Services.Result;
using SQLitePCL;
using SQT.Infrastructure;
using SQT.SQL.Matcher;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using Xunit;

namespace SQT.Services
{
    [Collection("JsonSeededFacetContext")]
    public class ResultServiceTests : DisposableFacetContextContainer
    {
        public ResultServiceTests(JsonFacetContextFixture fixture) : base(fixture)
        {
        }

        public virtual DataReaderBuilder FakeResultDataBuilder(ResultSpecification specification, int count)
        {
            var builder = new TabularResultDataReaderBuilder(specification)
                .CreateNewTable()
                .GenerateBogusRows(count);
            return builder;
        }

        protected Mock<IResultPayloadServiceLocator> MockResultPayloadServiceLocator(dynamic returnValue)
        {
            var mockService = new Mock<IResultPayloadService>();
            mockService.Setup(s => s.GetExtraPayload(It.IsAny<FacetsConfig2>(), It.IsAny<string>()))
                .Returns(returnValue);
            var mock = new Mock<IResultPayloadServiceLocator>();
            mock.Setup(s => s.Locate(It.IsAny<string>()))
                .Returns(mockService.Object);
            return mock;
        }

        protected ResultSpecification FixedResultSpecification(string specificationKey)
            => FakeRegistry().Results.GetByKey(specificationKey);

        protected Mock<IDynamicQueryProxy> MockDynamicQueryProxyWithFakeData(int testItemCount, ResultSpecification fakeSpecification)
        {
            var fakeResultDataBuilder = FakeResultDataBuilder(fakeSpecification, testItemCount);
            var fakeDataTable = fakeResultDataBuilder.DataTable;
            var mockQueryProxy = new MockDynamicQueryProxyFactory().Create(fakeDataTable);
            return mockQueryProxy;
        }

        [Theory]
        [InlineData("sites:country@5/sites@4,5", "result_facet", "site_level", "map", 10)]
        [InlineData("sites:data_types@5/rdb_codes@13,21/sites", "result_facet", "site_level", "map", 10)]
        [InlineData("sites:data_types@5/rdb_codes@13,21/sites", "result_facet", "site_level", "tabular", 10)]
        public void Load_VariousConfigs_Success(string uri, string resultCode, string specificationKey, string viewType, int testItemCount)
        {
            // Arrange
            var mockResultPayloadServiceLocator = MockResultPayloadServiceLocator(null);
            var mockResultSqlCompilerLocator = MockResultSqlCompilerLocator("#RETURN-SQL#");
            var fakeFacetsConfig = FakeFacetsConfig(uri);
            var fakeResultConfig = FakeResultConfig(resultCode, specificationKey, viewType);
            var mockQueryProxy = MockDynamicQueryProxyWithFakeData(testItemCount, fakeResultConfig.Specifications.FirstOrDefault());
            var fakeQuerySetup = FakeResultQuerySetup(fakeFacetsConfig, resultCode, specificationKey);
            var mockQuerySetupBuilder = MockQuerySetupBuilder(fakeQuerySetup);

            // Act
            var service = new ResultService(
                FakeRegistry(),
                mockQueryProxy.Object,
                mockQuerySetupBuilder.Object,
                mockResultPayloadServiceLocator.Object,
                mockResultSqlCompilerLocator.Object
            );

            var result = service.Load(fakeFacetsConfig, fakeResultConfig);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Meta);
            Assert.NotNull(result.Data);
            Assert.NotEmpty(result.Meta.Columns);
            Assert.NotEmpty(result.Data.DataCollection);
            Assert.Equal(testItemCount, result.Data.DataCollection.Count);
            Assert.Equal("#RETURN-SQL#", result.Query);

        }
    }
}
