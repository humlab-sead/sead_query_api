using Moq;
using SeadQueryCore.Model;
using System;
using Xunit;

namespace SeadQueryTest.Model
{
    public class ResultContentSetTests : IDisposable
    {
        private Moq.MockRepository mockRepository;



        public ResultContentSetTests()
        {
            this.mockRepository = new Moq.MockRepository(MockBehavior.Strict);


        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private ResultContentSet CreateResultContentSet()
        {
            return new ResultContentSet();
        }

        [Fact]
        public void TestMethod1()
        {
            // Arrange
            var resultContentSet = this.CreateResultContentSet();

            // Act


            // Assert
            Assert.True(false);
        }
    }
}
