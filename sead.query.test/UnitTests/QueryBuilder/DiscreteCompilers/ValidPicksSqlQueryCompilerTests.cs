using Moq;
using SeadQueryCore;
using SeadQueryCore.QueryBuilder;
using SQT.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SQT.SqlCompilers
{
    // FIXME Kolla vilka som saknar detta attribut!!!
    [Collection("JsonSeededFacetContext")]
    public class ValidPicksSqlQueryCompilerTests : DisposableFacetContextContainer
    {
        public ValidPicksSqlQueryCompilerTests(JsonSeededFacetContextFixture fixture) : base(fixture)
        {
        }

        [Theory]
        [InlineData("sites:sites")]
        public void Compile_StateUnderTest_ExpectedBehavior(string uri)
        {
            // Arrange
            FacetsConfig2 fakeFacetsConfig = FakeFacetsConfig(uri);
            QuerySetup fakeQuerySetup = FakeQuerySetup(uri);
            Facet facet = fakeFacetsConfig.TargetFacet;
            List<int> picks = fakeFacetsConfig.TargetConfig.Picks.Select(x => x.ToInt()).ToList();

            // Act
            var validPicksSqlCompiler = new ValidPicksSqCompiler();
            var result = validPicksSqlCompiler.Compile(fakeQuerySetup, facet, picks);

            // Assert
            Assert.True(false);
        }
    }
}
