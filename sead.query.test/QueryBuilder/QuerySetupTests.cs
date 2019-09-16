using Moq;
using SeadQueryCore;
using SeadQueryCore.QueryBuilder;
using System;
using System.Collections.Generic;
using Xunit;

namespace SeadQueryTest.QueryBuilder
{
    public class QuerySetupTests : IDisposable
    {
        private MockRepository mockRepository;

        private Mock<FacetConfig2> mockFacetConfig2;
        private Mock<Facet> mockFacet;
        private Mock<List<string>> mockListString;
        private Mock<Dictionary<string, string>> mockDictionary;
        private Mock<List<GraphRoute>> mockListGraphRoute;
        private Mock<List<GraphRoute>> mockListGraphRoute;

        public QuerySetupTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockFacetConfig2 = this.mockRepository.Create<FacetConfig2>();
            this.mockFacet = this.mockRepository.Create<Facet>();
            this.mockListString = this.mockRepository.Create<List<string>>();
            this.mockDictionary = this.mockRepository.Create<Dictionary<string, string>>();
            this.mockListGraphRoute = this.mockRepository.Create<List<GraphRoute>>();
            this.mockListGraphRoute = this.mockRepository.Create<List<GraphRoute>>();
        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private QuerySetup CreateQuerySetup()
        {
            return new QuerySetup(
                this.mockFacetConfig2.Object,
                this.mockFacet.Object,
                this.mockListString.Object,
                this.mockDictionary.Object,
                this.mockListGraphRoute.Object,
                this.mockListGraphRoute.Object);
        }

        [Fact]
        public void TestMethod1()
        {
            // Arrange
            var querySetup = this.CreateQuerySetup();

            // Act


            // Assert
            Assert.True(false);
        }
    }
}
