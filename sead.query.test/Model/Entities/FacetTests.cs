using Moq;
using SeadQueryCore;
using System;
using System.Collections.Generic;
using Xunit;

namespace SeadQueryTest.Model.Entities
{
    public class FacetTests : IDisposable
    {
        private Moq.MockRepository mockRepository;



        public FacetTests()
        {
            this.mockRepository = new Moq.MockRepository(MockBehavior.Strict);


        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private Facet CreateFacet()
        {
            return new Facet();
        }

        [Fact]
        public void IsAffectedBy_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var facet = this.CreateFacet();
            List<string> facetCodes = null;
            Facet targetFacet = null;

            // Act
            var result = facet.IsAffectedBy(
                facetCodes,
                targetFacet);

            // Assert
            Assert.True(false);
        }
    }
}
