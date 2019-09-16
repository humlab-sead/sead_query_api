using Moq;
using SeadQueryCore;
using System;
using Xunit;

namespace SeadQueryTest.Model.Entities
{
    public class ResultFieldTypeTests : IDisposable
    {
        private Moq.MockRepository mockRepository;



        public ResultFieldTypeTests()
        {
            this.mockRepository = new Moq.MockRepository(MockBehavior.Strict);


        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private ResultFieldType CreateResultFieldType()
        {
            return new ResultFieldType();
        }

        [Fact]
        public void TestMethod1()
        {
            // Arrange
            var resultFieldType = this.CreateResultFieldType();

            // Act


            // Assert
            Assert.True(false);
        }
    }
}
