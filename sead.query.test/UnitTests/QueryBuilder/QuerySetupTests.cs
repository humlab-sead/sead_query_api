using Moq;
using SeadQueryCore;
using SeadQueryCore.QueryBuilder;
using System;
using System.Collections.Generic;
using Xunit;

using MockRepository = Moq.MockRepository;

namespace SQT.QueryBuilder
{
    public class QuerySetupTests : IDisposable
    {
        private MockRepository mockRepository;

        private Mock<FacetConfig2> mockFacetConfig2;
        private Mock<Facet> mockFacet;
        private Mock<List<string>> mockSqlJoins;
        private Mock<List<string>> mockCriterias;
        private Mock<List<GraphRoute>> mockRoutes;

        public QuerySetupTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockFacetConfig2 = this.mockRepository.Create<FacetConfig2>();
            this.mockFacet = this.mockRepository.Create<Facet>();
            this.mockSqlJoins = this.mockRepository.Create<List<string>>();
            this.mockCriterias = this.mockRepository.Create<List<string>>();
            this.mockRoutes = this.mockRepository.Create<List<GraphRoute>>();
        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private QuerySetup CreateQuerySetup()
        {
            return new QuerySetup() {
                TargetConfig = this.mockFacetConfig2.Object,
                Facet = this.mockFacet.Object,
                Joins = this.mockSqlJoins.Object,
                Criterias = this.mockCriterias.Object,
                Routes = this.mockRoutes.Object
            };
        }

        [Fact(Skip ="Not implemented")]
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
