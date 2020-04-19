using Autofac.Features.Indexed;
using Moq;
using SeadQueryCore;
using SeadQueryCore.Model;
using SeadQueryCore.QueryBuilder;
using SQT.Infrastructure;
using System;
using Xunit;

namespace SQT.SqlCompilers
{
    public class ResultConfigCompiler : DisposableFacetContextContainer
    {
        public ResultConfigCompiler(JsonSeededFacetContextFixture fixture) : base(fixture)
        {
        }

        public virtual Mock<IResultSqlCompilerLocator> MockResultSqlCompilerLocator(string returnSql)
        {
            var mockResultSqlCompiler = new Mock<IResultSqlCompiler>();
            mockResultSqlCompiler
                .Setup(z => z.Compile(It.IsAny<QuerySetup>(), It.IsAny<Facet>(), It.IsAny<ResultQuerySetup>()))
                .Returns(returnSql);
            var mockResultSqlCompilerLocator = new Mock<IResultSqlCompilerLocator>();
            mockResultSqlCompilerLocator
                .Setup(z => z.Locate(It.IsAny<string>()))
                .Returns(mockResultSqlCompiler.Object);
            return null;
        }

        public virtual ResultConfig FakeResultConfig(string aggregateKey, string viewTypeId)
            => new ResultConfig
        {
            AggregateKeys = new System.Collections.Generic.List<string> { aggregateKey },
            RequestId = "1",
            ViewTypeId = viewTypeId,
            SessionId = "1"
        };

        [Theory]
        [InlineData("sites:sites", "site_level", "tabular")]
        [InlineData("sites:sites", "site_level", "map")]
        public void Compile_StateUnderTest_ExpectedBehavior(string uri, string aggregateKey, string viewTypeId)
        {
            // Arrange
            var facetsConfig = FakeFacetsConfig(uri);
            var querySetup = FakeQuerySetup(uri);
            ResultConfig fakeResultConfig = FakeResultConfig(aggregateKey, viewTypeId);

            var mockQuerySetupBuilder = MockQuerySetupBuilder(querySetup);
            var mockResultSqlCompilerLocator = MockResultSqlCompilerLocator("#SQL#");

            string facetCode = facetsConfig.TargetCode;

            // Act
            var resultQueryCompiler = new SeadQueryCore.ResultConfigCompiler(
                base.Registry,
                mockQuerySetupBuilder.Object,
                mockResultSqlCompilerLocator.Object
            );

            var result = resultQueryCompiler.Compile(facetsConfig, fakeResultConfig, facetCode);

            // Assert
            Assert.NotEmpty(result);

            // MAP
            var expectedSql = "#SQL#";

            Assert.Equal(expectedSql, result);

        }

    }
}
