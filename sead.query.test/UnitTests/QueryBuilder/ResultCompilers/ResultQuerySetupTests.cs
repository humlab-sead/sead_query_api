using Moq;
using SeadQueryCore;
using SeadQueryCore.QueryBuilder;
using System;
using System.Collections.Generic;
using Xunit;

namespace SeadQueryTest.QueryBuilder.ResultCompilers
{
    public class ResultQuerySetupTests : IDisposable
    {
        private MockRepository mockRepository;

        private Mock<List<ResultAggregateField>> mockList;

        public ResultQuerySetupTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockList = this.mockRepository.Create<List<ResultAggregateField>>();
        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private ResultQuerySetup CreateResultQuerySetup()
        {
            return new ResultQuerySetup(
                this.mockList.Object);
        }

        [Fact(Skip = "Not implemented")]
        public void TestMethod1()
        {
            // Arrange
            var resultQuerySetup = this.CreateResultQuerySetup();

            // Act


            // Assert
            Assert.True(false);
        }
    }
}
