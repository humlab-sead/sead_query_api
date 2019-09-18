using Moq;
using SeadQueryCore;
using SeadQueryCore.QueryBuilder;
using System;
using Xunit;

namespace SeadQueryTest.QueryBuilder.FilterCompilers
{
    public class GeoFacetPickFilterCompilerTests : IDisposable
    {
        private MockRepository mockRepository;



        public GeoFacetPickFilterCompilerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);


        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private GeoFacetPickFilterCompiler CreateGeoFacetPickFilterCompiler()
        {
            return new GeoFacetPickFilterCompiler();
        }

        [Fact]
        public void Compile_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var geoFacetPickFilterCompiler = this.CreateGeoFacetPickFilterCompiler();
            Facet targetFacet = null;
            Facet currentFacet = null;
            FacetConfig2 config = null;

            // Act
            var result = geoFacetPickFilterCompiler.Compile(
                targetFacet,
                currentFacet,
                config);

            // Assert
            Assert.True(false);
        }
    }
}
