using Moq;
using SeadQueryCore;
using System;
using Xunit;

namespace SeadQueryTest.Model.Entities
{
    public class ResultAggregateTests : IDisposable
    {
        private Moq.MockRepository mockRepository;



        public ResultAggregateTests()
        {
            this.mockRepository = new Moq.MockRepository(MockBehavior.Strict);


        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private ResultAggregate CreateResultAggregate()
        {
            return new ResultAggregate();
        }

        [Fact]
        public void GetResultFields_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var resultAggregate = this.CreateResultAggregate();

            // Act
            var result = resultAggregate.GetResultFields();

            // Assert
            Assert.True(false);
        }

        [Fact]
        public void GetFields_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var resultAggregate = this.CreateResultAggregate();

            // Act
            var result = resultAggregate.GetFields();

            // Assert
            Assert.True(false);
        }
    }
}
