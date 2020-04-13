using Moq;
using SeadQueryCore;
using SeadQueryCore.QueryBuilder;
using SeadQueryTest.Infrastructure;
using SeadQueryTest.Mocks;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using Xunit;
using KellermanSoftware.CompareNetObjects;

namespace SeadQueryTest.Services.CategoryCount
{
    [Collection("JsonSeededFacetContext")]
    public class RangeCategoryCountServiceTests : DisposableFacetContextContainer
    {

        public RangeCategoryCountServiceTests(JsonSeededFacetContextFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public void Load_SingleRangeFacetWithoutPicks_ReturnsExpectedValues()
        {
            // Arrange
            var config = new SettingFactory().Create().Value.Facet;
            var mockRegistry = new Mock<IRepositoryRegistry>();
            var mockQueryProxy = new Mock<ITypedQueryProxy>();
            var mockQuerySetupBuilder = new Mock<IQuerySetupCompiler>();
            var MockRangeCountSqlCompiler = new Mock<IRangeCategoryCountSqlQueryCompiler>();
            var facet = Registry.Facets.GetByCode("tbl_denormalized_measured_values_33_0");

            mockRegistry.Setup(r => r.Facets).Returns(Registry.Facets);

            //var mockFacetsConfig = new MockFacetsConfigFactory(Registry.Facets)
            //    .Create("tbl_denormalized_measured_values_33_0:tbl_denormalized_measured_values_33_0");

            var mockFacetsConfig = new FacetsConfig2()
            {
                DomainCode = "",
                TargetCode = facet.FacetCode,
                TargetFacet = facet,
                TriggerCode = facet.FacetCode,
                TriggerFacet = facet,
                RequestId = "1",
                RequestType = "populate",
                FacetConfigs = new List<FacetConfig2>() {
                    // FacetConfigFactory.Create(facet, 0, new List<FacetConfigPick>())
                    new FacetConfig2 {
                        Facet = facet,
                        FacetCode = facet.FacetCode,
                        Position = 1,
                        TextFilter = "",
                        Picks = new List<FacetConfigPick>()
                        {
                            
                        }
                    }
                }
            };
            mockQuerySetupBuilder.Setup(x => x.Build(
                It.IsAny<FacetsConfig2>(),
                It.IsAny<Facet>(),
                It.IsAny<List<string>>()
            )).Returns(
                new QuerySetup
                {
                    Facet = facet,
                    Criterias = null,
                    Joins = null,
                    Routes = null,
                    TargetConfig = mockFacetsConfig.TargetConfig
                }
            );

            MockRangeCountSqlCompiler.Setup(c => c.Compile(
                It.IsAny<QuerySetup>(),
                It.IsAny<Facet>(),
                It.IsAny<string>(),
                It.IsAny<string>()
            )).Returns(
                ""
            );

            var fakeResult = new RangeCountDataReaderBuilder(0, 10)
                .CreateNewTable()
                .GenerateBogusRows(3)
                .ToItems<CategoryCountItem>().ToList();

            mockQueryProxy.Setup(foo => foo.QueryRows<CategoryCountItem>(
                        It.IsAny<string>(),
                        It.IsAny<Func<IDataReader, CategoryCountItem>>()
                )).Returns(
                    fakeResult
                );

            var service = new RangeCategoryCountService(
                config,
                mockRegistry.Object,
                mockQuerySetupBuilder.Object,
                MockRangeCountSqlCompiler.Object,
                mockQueryProxy.Object 
            );

            // Act
            var result = service.Load(facet.FacetCode, mockFacetsConfig);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Data);
            Assert.Equal(fakeResult.Count, result.Data.Count);

            CompareLogic compareLogic = new CompareLogic();

            Assert.True(compareLogic.Compare(fakeResult, result.Data.Values.ToList()).AreEqual);
        }

        [Theory]
        [InlineData("site:tbl_denormalized_measured_values_33_0")]
        public void Load_OfRangeCategoryCountsForVariousFacetsConfigs_ReturnsExpectedValues(string uri)
        {
            // Arrange
            var config = new SettingFactory().Create().Value.Facet;
            var mockRegistry = CreateRepositoryRegistryMock();
            var mockFacetsConfig = new MockFacetsConfigFactory(Registry.Facets).Create(uri);
            var facet = mockFacetsConfig.TargetFacet;
            var mockQuerySetupCompiler = CreateQuerySetupCompilerDummy();
            var mockRangeCountSqlCompiler = CreateRangeCategoryCountSqlQueryCompilerMock(returnSql: "");
            var fakeResult = CreateFakeRangeCategoryCountItems(start: 0, step: 10, count: 3);
            var mockQueryProxy = CreateFakeTypedQueryProxy(fakeResult);

            // Act
            var service = new RangeCategoryCountService(
                 config,
                 mockRegistry.Object,
                 mockQuerySetupCompiler.Object,
                 mockRangeCountSqlCompiler.Object,
                 mockQueryProxy.Object
             );
            var result = service.Load(facet.FacetCode, mockFacetsConfig);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Data);
            Assert.Equal(fakeResult.Count, result.Data.Count);

            CompareLogic compareLogic = new CompareLogic();

            Assert.True(compareLogic.Compare(fakeResult, result.Data.Values.ToList()).AreEqual);
        }

        private Mock<IRepositoryRegistry> CreateRepositoryRegistryMock()
        {
            // Default: a JSON-seeded FacetContext with Sqlite backend
            var mockRegistry = new Mock<IRepositoryRegistry>();
            mockRegistry.Setup(r => r.Facets).Returns(Registry.Facets);
            return mockRegistry;
        }

        private static Mock<IQuerySetupCompiler> CreateQuerySetupCompilerDummy()
        {
            var mockQuerySetupCompiler = new Mock<IQuerySetupCompiler>();
            mockQuerySetupCompiler.Setup(x => x.Build(
                        It.IsAny<FacetsConfig2>(),
                        It.IsAny<Facet>(),
                        It.IsAny<List<string>>()
                    )).Returns(new QuerySetup { /* not used */ });
            return mockQuerySetupCompiler;
        }

        private static Mock<ITypedQueryProxy> CreateFakeTypedQueryProxy(List<CategoryCountItem> fakeResult)
        {
            var mockQueryProxy = new Mock<ITypedQueryProxy>();
            mockQueryProxy.Setup(foo => foo.QueryRows<CategoryCountItem>(
                        It.IsAny<string>(),
                        It.IsAny<Func<IDataReader, CategoryCountItem>>()
                )).Returns(
                    fakeResult
                );
            return mockQueryProxy;
        }

        private static List<CategoryCountItem> CreateFakeRangeCategoryCountItems(int start, int step, int count)
        {
            var fakeResult = new RangeCountDataReaderBuilder(start, step)
                .CreateNewTable()
                .GenerateBogusRows(count)
                .ToItems<CategoryCountItem>().ToList();

            return fakeResult;
        }

        private static Mock<IRangeCategoryCountSqlQueryCompiler> CreateRangeCategoryCountSqlQueryCompilerMock(string returnSql)
        {
            var MockRangeCountSqlCompiler = new Mock<IRangeCategoryCountSqlQueryCompiler>();
            MockRangeCountSqlCompiler.Setup(c => c.Compile(
                It.IsAny<QuerySetup>(),
                It.IsAny<Facet>(),
                It.IsAny<string>(),
                It.IsAny<string>()
            )).Returns(
                returnSql
            );
            return MockRangeCountSqlCompiler;
        }
    }
}
