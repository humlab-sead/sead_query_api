using System.Collections.Generic;
using System.Linq;
using Moq;
using SeadQueryCore;
using SeadQueryCore.Model;
using SeadQueryCore.QueryBuilder;
using SeadQueryCore.Services.Result;
using SQT.Infrastructure;
using Xunit;

namespace SQT.Services
{
    [Collection("UsePostgresFixture")]
    public class ResultServiceTests : MockerWithFacetContext
    {
        public ResultServiceTests()
            : base() { }

        public virtual DataReaderBuilder FakeResultDataBuilder(ResultSpecification specification, int count)
        {
            var builder = new TabularResultDataReaderBuilder(specification).CreateNewTable().GenerateBogusRows(count);
            return builder;
        }

        protected Mock<IResultPayloadServiceLocator> MockResultPayloadServiceLocator(dynamic returnValue)
        {
            var mockService = new Mock<IResultPayloadService>();
            mockService.Setup(s => s.GetExtraPayload(It.IsAny<FacetsConfig2>(), It.IsAny<string>())).Returns(returnValue);
            var mock = new Mock<IResultPayloadServiceLocator>();
            mock.Setup(s => s.Locate(It.IsAny<string>())).Returns(mockService.Object);
            return mock;
        }

        protected ResultSpecification FixedResultSpecification(string specificationKey) =>
            FakeRegistry().Results.GetByKey(specificationKey);

        protected Mock<IDynamicQueryProxy> MockDynamicQueryProxyWithFakeData(int testItemCount, ResultSpecification fakeSpecification)
        {
            var fakeResultDataBuilder = FakeResultDataBuilder(fakeSpecification, testItemCount);
            var fakeDataTable = fakeResultDataBuilder.DataTable;
            var mockQueryProxy = new MockDynamicQueryProxyFactory().Create(fakeDataTable);
            return mockQueryProxy;
        }

        protected Mock<IResultProjectionHandoffBuilder> MockResultProjectionHandoffBuilder(
            QuerySetup querySetup,
            IEnumerable<ResultSpecificationField> resultFields
        )
        {
            var mock = new Mock<IResultProjectionHandoffBuilder>();
            mock.Setup(x => x.Build(It.IsAny<FacetsConfig2>(), It.IsAny<ResultConfig>()))
                .Returns(new ResultProjectionHandoff(querySetup, resultFields));
            return mock;
        }

        [Theory]
        [InlineData("constructions:constructions", "result_facet", "site_level", "map", 10)]
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
            var mockResultProjectionHandoffBuilder = MockResultProjectionHandoffBuilder(fakeQuerySetup, fakeResultConfig.GetSortedFields());

            // Act
            var service = new ResultService(
                mockQueryProxy.Object,
                mockResultProjectionHandoffBuilder.Object,
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
            mockResultProjectionHandoffBuilder.Verify(x => x.Build(fakeFacetsConfig, fakeResultConfig), Times.Once);
        }

        [Fact]
        public void Load_WithComposedHandoff_UsesHandoffQuerySetup()
        {
            // Arrange
            var fakeFacetsConfig = FakeFacetsConfig("sites:sites");
            var fakeResultConfig = FakeResultConfig("result_facet", "site_level", "tabular");
            var handoffQuerySetup = FakeResultQuerySetup(fakeFacetsConfig, "result_facet", "site_level");
            handoffQuerySetup.LeadingSql = "with composed_filter as (select 1 as target_id)";
            var handoffResultFields = fakeResultConfig.GetSortedFields().ToList();
            var mockResultProjectionHandoffBuilder = MockResultProjectionHandoffBuilder(handoffQuerySetup, handoffResultFields);
            var mockResultPayloadServiceLocator = MockResultPayloadServiceLocator(null);
            var mockQueryProxy = MockDynamicQueryProxyWithFakeData(3, fakeResultConfig.Specifications.FirstOrDefault());

            var mockResultSqlCompiler = new Mock<IResultSqlCompiler>();
            mockResultSqlCompiler
                .Setup(x => x.Compile(handoffQuerySetup, fakeResultConfig.Facet, handoffResultFields))
                .Returns("#RETURN-SQL#");
            var mockResultSqlCompilerLocator = new Mock<IResultSqlCompilerLocator>();
            mockResultSqlCompilerLocator.Setup(x => x.Locate(fakeResultConfig.ViewTypeId)).Returns(mockResultSqlCompiler.Object);

            var service = new ResultService(
                mockQueryProxy.Object,
                mockResultProjectionHandoffBuilder.Object,
                mockResultPayloadServiceLocator.Object,
                mockResultSqlCompilerLocator.Object
            );

            // Act
            var result = service.Load(fakeFacetsConfig, fakeResultConfig);

            // Assert
            Assert.Equal("#RETURN-SQL#", result.Query);
            mockResultProjectionHandoffBuilder.Verify(x => x.Build(fakeFacetsConfig, fakeResultConfig), Times.Once);
            mockResultSqlCompilerLocator.Verify(x => x.Locate(fakeResultConfig.ViewTypeId), Times.Once);
            mockResultSqlCompiler.Verify(x => x.Compile(handoffQuerySetup, fakeResultConfig.Facet, handoffResultFields), Times.Once);
        }

        [Fact]
        public void Load_WithViewTypePayloadService_SetsPayloadUsingFacetCode()
        {
            // Arrange
            var fakeFacetsConfig = FakeFacetsConfig("sites:sites");
            var fakeResultConfig = FakeResultConfig("result_facet", "site_level", "map");
            var fakeQuerySetup = FakeResultQuerySetup(fakeFacetsConfig, "result_facet", "site_level");
            var handoffResultFields = fakeResultConfig.GetSortedFields().ToList();
            var mockResultProjectionHandoffBuilder = MockResultProjectionHandoffBuilder(fakeQuerySetup, handoffResultFields);
            var mockQueryProxy = MockDynamicQueryProxyWithFakeData(2, fakeResultConfig.Specifications.FirstOrDefault());

            var payload = new Dictionary<string, object> { ["view"] = "map" };
            var mockPayloadService = new Mock<IResultPayloadService>();
            mockPayloadService.Setup(x => x.GetExtraPayload(fakeFacetsConfig, fakeResultConfig.Facet.FacetCode)).Returns(payload);
            var mockResultPayloadServiceLocator = new Mock<IResultPayloadServiceLocator>();
            mockResultPayloadServiceLocator.Setup(x => x.Locate(fakeResultConfig.ViewTypeId)).Returns(mockPayloadService.Object);

            var mockResultSqlCompilerLocator = MockResultSqlCompilerLocator("#RETURN-SQL#");

            var service = new ResultService(
                mockQueryProxy.Object,
                mockResultProjectionHandoffBuilder.Object,
                mockResultPayloadServiceLocator.Object,
                mockResultSqlCompilerLocator.Object
            );

            // Act
            var result = service.Load(fakeFacetsConfig, fakeResultConfig);

            // Assert
            Assert.Same(payload, result.Payload);
            mockResultPayloadServiceLocator.Verify(x => x.Locate(fakeResultConfig.ViewTypeId), Times.Once);
            mockPayloadService.Verify(x => x.GetExtraPayload(fakeFacetsConfig, fakeResultConfig.Facet.FacetCode), Times.Once);
        }
    }
}
