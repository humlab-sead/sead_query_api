using Moq;
using SeadQueryCore;
using System;
using Xunit;

namespace SeadQueryTest.Model
{
    public class ResultConfigTests : IDisposable
    {
        private Moq.MockRepository mockRepository;



        public ResultConfigTests()
        {
            this.mockRepository = new Moq.MockRepository(MockBehavior.Strict);


        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private ResultConfig CreateResultConfig()
        {
            return new ResultConfig();
        }

        [Fact]
        public void GetCacheId_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var resultConfig = this.CreateResultConfig();
            FacetsConfig2 facetsConfig = null;

            // Act
            var result = resultConfig.GetCacheId(
                facetsConfig);

            // Assert
            Assert.True(false);
        }
    }
}
