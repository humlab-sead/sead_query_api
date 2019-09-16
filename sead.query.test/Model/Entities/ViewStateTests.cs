using Moq;
using SeadQueryCore;
using System;
using Xunit;

namespace SeadQueryTest.Model.Entities
{
    public class ViewStateTests : IDisposable
    {
        private Moq.MockRepository mockRepository;



        public ViewStateTests()
        {
            this.mockRepository = new Moq.MockRepository(MockBehavior.Strict);


        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private ViewState CreateViewState()
        {
            return new ViewState();
        }

        [Fact]
        public void TestMethod1()
        {
            // Arrange
            var viewState = this.CreateViewState();

            // Act


            // Assert
            Assert.True(false);
        }
    }
}
