using Moq;
using SeadQueryCore;
using SeadQueryCore.QueryBuilder;
using System;
using System.Collections.Generic;
using Xunit;

using MockRepository = Moq.MockRepository;

namespace SeadQueryTest.QueryBuilder
{
    public class QuerySetupTests : IDisposable
    {
        private MockRepository mockRepository;

        private Mock<FacetConfig2> mockFacetConfig2;
        private Mock<Facet> mockFacet;
        private Mock<List<string>> mockSqlJoins;
        private Mock<Dictionary<string, string>> mockCriterias;
        private Mock<List<GraphRoute>> mockRoutes;
        private Mock<List<GraphRoute>> mockReducedRoutes;

        public QuerySetupTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockFacetConfig2 = this.mockRepository.Create<FacetConfig2>();
            this.mockFacet = this.mockRepository.Create<Facet>();
            this.mockSqlJoins = this.mockRepository.Create<List<string>>();
            this.mockCriterias = this.mockRepository.Create<Dictionary<string, string>>();
            this.mockRoutes = this.mockRepository.Create<List<GraphRoute>>();
            this.mockReducedRoutes = this.mockRepository.Create<List<GraphRoute>>();
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
                this.mockSqlJoins.Object,
                this.mockCriterias.Object,
                this.mockRoutes.Object,
                this.mockReducedRoutes.Object);
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
