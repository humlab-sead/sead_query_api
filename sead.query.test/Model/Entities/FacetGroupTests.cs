using Moq;
using SeadQueryCore;
using System;
using Xunit;

namespace SeadQueryTest.Model.Entities
{
    public class FacetGroupTests : IDisposable
    {
        private Moq.MockRepository mockRepository;



        public FacetGroupTests()
        {
            this.mockRepository = new Moq.MockRepository(MockBehavior.Strict);


        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private FacetGroup CreateFacetGroup()
        {
            return new FacetGroup();
        }

        [Fact]
        public void TestMethod1()
        {
            // Arrange
            var facetGroup = this.CreateFacetGroup();

            // Act


            // Assert
            Assert.True(false);
        }
    }
}
