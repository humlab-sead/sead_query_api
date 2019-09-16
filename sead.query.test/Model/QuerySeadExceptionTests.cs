using Moq;
using SeadQueryCore;
using System;
using Xunit;

namespace SeadQueryTest.Model
{
    public class QuerySeadExceptionTests : IDisposable
    {
        private Moq.MockRepository mockRepository;

        private Mock<Exception> mockException;

        public QuerySeadExceptionTests()
        {
            this.mockRepository = new Moq.MockRepository(MockBehavior.Strict);

            this.mockException = this.mockRepository.Create<Exception>();
        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private QuerySeadException CreateQuerySeadException()
        {
            string msg = "";
            return new QuerySeadException(
                msg,
                this.mockException.Object);
        }

        [Fact]
        public void TestMethod1()
        {
            // Arrange
            var querySeadException = this.CreateQuerySeadException();

            // Act


            // Assert
            Assert.True(false);
        }
    }
}
