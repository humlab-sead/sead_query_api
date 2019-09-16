using Moq;
using SeadQueryCore;
using System;
using Xunit;

namespace SeadQueryTest.Model.Entities
{
    public class ResultViewTypeTests : IDisposable
    {
        private Moq.MockRepository mockRepository;



        public ResultViewTypeTests()
        {
            this.mockRepository = new Moq.MockRepository(MockBehavior.Strict);


        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private ResultViewType CreateResultViewType()
        {
            return new ResultViewType();
        }

        [Fact]
        public void TestMethod1()
        {
            // Arrange
            var resultViewType = this.CreateResultViewType();

            // Act


            // Assert
            Assert.True(false);
        }
    }
}
