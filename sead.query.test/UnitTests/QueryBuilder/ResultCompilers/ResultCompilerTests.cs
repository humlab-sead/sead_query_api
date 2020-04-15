using Autofac.Features.Indexed;
using Moq;
using SeadQueryCore;
using SeadQueryCore.Model;
using SeadQueryCore.QueryBuilder;
using System;
using Xunit;

namespace SeadQueryTest.QueryBuilder.ResultCompilers
{
    public class ResultCompilerTests : IDisposable
    {
        private MockRepository mockRepository;

        private Mock<ISetting> mockQueryBuilderSetting;
        private Mock<IRepositoryRegistry> mockRepositoryRegistry;
        private Mock<IQuerySetupBuilder> mockQuerySetupBuilder;
        private Mock<IIndex<string, IResultQuerySetupSqlCompiler>> mockIndex;

        public ResultCompilerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockQueryBuilderSetting = this.mockRepository.Create<ISetting>();
            this.mockRepositoryRegistry = this.mockRepository.Create<IRepositoryRegistry>();
            this.mockQuerySetupBuilder = this.mockRepository.Create<IQuerySetupBuilder>();
            this.mockIndex = this.mockRepository.Create<IIndex<string, IResultQuerySetupSqlCompiler>>();
        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private ResultQueryCompiler CreateResultCompiler()
        {
            return new ResultQueryCompiler(
                this.mockRepositoryRegistry.Object,
                this.mockQuerySetupBuilder.Object,
                this.mockIndex.Object);
        }

        [Fact(Skip = "Not implemented")]
        public void Compile_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var resultCompiler = this.CreateResultCompiler();
            FacetsConfig2 facetsConfig = null;
            ResultConfig resultConfig = null;
            string facetCode = null;

            // Act
            var result = resultCompiler.Compile(
                facetsConfig,
                resultConfig,
                facetCode);

            // Assert
            Assert.True(false);
        }
    }
}
