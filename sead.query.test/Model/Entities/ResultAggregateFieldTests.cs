using Moq;
using SeadQueryCore;
using System;
using Xunit;

namespace SeadQueryTest.Model.Entities
{
    public class ResultAggregateFieldTests : IDisposable
    {
        private Moq.MockRepository mockRepository;



        public ResultAggregateFieldTests()
        {
            this.mockRepository = new Moq.MockRepository(MockBehavior.Strict);


        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private ResultAggregateField CreateResultAggregateField()
        {
            return new ResultAggregateField();
        }

        [Fact]
        public void TestMethod1()
        {
            // Arrange
            var resultAggregateField = this.CreateResultAggregateField();

            // Act


            // Assert
            Assert.True(false);
        }
    }
}
