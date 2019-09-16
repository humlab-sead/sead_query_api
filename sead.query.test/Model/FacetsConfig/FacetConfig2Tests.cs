using Moq;
using SeadQueryCore;
using System;
using Xunit;

namespace SeadQueryTest.Model.FacetsConfig
{
    public class FacetConfig2Tests : IDisposable
    {
        private MockRepository mockRepository;

        private Mock<IRepositoryRegistry> mockRepositoryRegistry;

        public FacetConfig2Tests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockRepositoryRegistry = this.mockRepository.Create<IRepositoryRegistry>();
        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private FacetConfig2 CreateFacetConfig2()
        {
            return new FacetConfig2(
                this.mockRepositoryRegistry.Object);
        }

        [Fact]
        public void HasPicks_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var facetConfig2 = this.CreateFacetConfig2();

            // Act
            var result = facetConfig2.HasPicks();

            // Assert
            Assert.True(false);
        }

        [Fact]
        public void ClearPicks_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var facetConfig2 = this.CreateFacetConfig2();

            // Act
            facetConfig2.ClearPicks();

            // Assert
            Assert.True(false);
        }

        [Fact]
        public void GetPickValues_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var facetConfig2 = this.CreateFacetConfig2();
            bool sort = false;

            // Act
            var result = facetConfig2.GetPickValues(
                sort);

            // Assert
            Assert.True(false);
        }

        [Fact]
        public void GetJoinTables_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var facetConfig2 = this.CreateFacetConfig2();

            // Act
            var result = facetConfig2.GetJoinTables();

            // Assert
            Assert.True(false);
        }
    }
}
