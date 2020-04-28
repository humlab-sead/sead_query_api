using Moq;
using SeadQueryCore;
using SeadQueryCore.QueryBuilder;
using SQT.Infrastructure;
using System;
using System.Collections.Generic;
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
        [InlineData("uri:uri")]
        public void Compile_StateUnderTest_ExpectedBehavior(string uri)
        {
            // Arrange
            QuerySetup fakeQuerySetup = FakeQuerySetup(uri);
            Facet facet = fakeQuerySetup.Facet;
            List<int> picks = null;

            // Act
            var validPicksSqlCompiler = new ValidPicksSqCompiler();
            var result = validPicksSqlCompiler.Compile(fakeQuerySetup, facet, picks);

            // Assert
            Assert.True(false);
        }
    }
}
