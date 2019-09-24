using Moq;
using SeadQueryCore;
using System;
using Xunit;

namespace SeadQueryTest.Services.CategoryCount
{
    public class CategoryCountItemTests : IDisposable
    {
        private MockRepository mockRepository;



        public CategoryCountItemTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);


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
