using Moq;
using SeadQueryCore;
using System;
using Xunit;

namespace SeadQueryTest.Model.FacetsConfig
{
    public class FacetConfigPickTests : IDisposable
    {
        private MockRepository mockRepository;



        public FacetConfigPickTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);


        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private FacetConfigPick CreateFacetConfigPick()
        {
            return new FacetConfigPick(
                TODO,
                TODO,
                TODO);
        }

        [Fact]
        public void ToDecimal_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var facetConfigPick = this.CreateFacetConfigPick();

            // Act
            var result = facetConfigPick.ToDecimal();

            // Assert
            Assert.True(false);
        }

        [Fact]
        public void ToInt_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var facetConfigPick = this.CreateFacetConfigPick();

            // Act
            var result = facetConfigPick.ToInt();

            // Assert
            Assert.True(false);
        }

        [Fact]
        public void CreateLowerUpper_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var facetConfigPick = this.CreateFacetConfigPick();
            decimal lower = 0;
            decimal upper = 0;

            // Act
            var result = facetConfigPick.CreateLowerUpper(
                lower,
                upper);

            // Assert
            Assert.True(false);
        }
    }
}
