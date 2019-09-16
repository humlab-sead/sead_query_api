using Moq;
using SeadQueryCore;
using System;
using Xunit;

namespace SeadQueryTest.Services
{
    public class CategoryCountItemTests : IDisposable
    {
        private Moq.MockRepository mockRepository;



        public CategoryCountItemTests()
        {
            this.mockRepository = new Moq.MockRepository(MockBehavior.Strict);


        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private CategoryCountItem CreateCategoryCountItem()
        {
            return new CategoryCountItem();
        }

        [Fact]
        public void TestMethod1()
        {
            // Arrange
            var categoryCountItem = this.CreateCategoryCountItem();

            // Act


            // Assert
            Assert.True(false);
        }
    }
}
