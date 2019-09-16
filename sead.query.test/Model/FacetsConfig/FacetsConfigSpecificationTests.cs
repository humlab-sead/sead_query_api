using Moq;
using SeadQueryCore;
using System;
using Xunit;

namespace SeadQueryTest.Model.FacetsConfig
{
    public class FacetsConfigSpecificationTests : IDisposable
    {
        private MockRepository mockRepository;



        public FacetsConfigSpecificationTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);


        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private FacetsConfigSpecification CreateFacetsConfigSpecification()
        {
            return new FacetsConfigSpecification();
        }

        [Fact]
        public void IsSatisfiedBy_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var facetsConfigSpecification = this.CreateFacetsConfigSpecification();
            FacetsConfig2 facetsConfig = null;

            // Act
            var result = facetsConfigSpecification.IsSatisfiedBy(
                facetsConfig);

            // Assert
            Assert.True(false);
        }

        [Fact]
        public void IsSatisfiedBy_StateUnderTest_ExpectedBehavior1()
        {
            // Arrange
            var facetsConfigSpecification = this.CreateFacetsConfigSpecification();
            List configs = null;

            // Act
            var result = facetsConfigSpecification.IsSatisfiedBy(
                configs);

            // Assert
            Assert.True(false);
        }
    }
}
