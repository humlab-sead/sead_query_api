using Moq;
using SeadQueryCore;
using System;
using Xunit;

namespace SeadQueryTest.Model.Entities
{
    public class ResultFieldTests : IDisposable
    {
        private Moq.MockRepository mockRepository;



        public ResultFieldTests()
        {
            this.mockRepository = new Moq.MockRepository(MockBehavior.Strict);


        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private ResultField CreateResultField()
        {
            return new ResultField();
        }

        [Fact]
        public void TestMethod1()
        {
            // Arrange
            var resultField = this.CreateResultField();

            // Act


            // Assert
            Assert.True(false);
        }
    }
}
